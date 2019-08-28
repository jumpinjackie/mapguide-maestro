using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using System;
using System.Collections.Concurrent;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    public class CoordinateSystemInfo
    {
        [XmlElement]
        public string MentorCode { get; set; }

        [XmlElement]
        public int EpsgCode { get; set; }

        [XmlElement]
        public string Wkt { get; set; }
    }

    public class TransformedCoordinate
    {
        [XmlElement]
        public double? X { get; set; }

        [XmlElement]
        public double? Y { get; set; }

        [XmlElement]
        public string Error { get; set; }
    }

    public class TransformedCoordinateCollection
    {
        [XmlElement]
        public CoordinateSystemInfo CoordinateSystem { get; set; }

        [XmlElement]
        public TransformedCoordinate[] TransformedCoordinate { get; set; }
    }

    internal class HttpCoordinateSystemTransform : ISimpleTransform
    {
        readonly HttpServerConnection _connection;
        readonly string _sourceWkt;
        readonly string _targetWkt;

        static ConcurrentDictionary<string, string> smWkt2Code = new ConcurrentDictionary<string, string>();

        public HttpCoordinateSystemTransform(HttpServerConnection connection, string sourceWkt, string targetWkt)
        {
            _connection = connection;
            _sourceWkt = sourceWkt;
            _targetWkt = targetWkt;
        }

        public void Dispose()
        {
            
        }

        public bool Transform(double x, double y, out double tx, out double ty)
        {
            tx = Double.NaN;
            ty = Double.NaN;
            try
            {
                var source = smWkt2Code.GetOrAdd(_sourceWkt, wkt =>
                {
                    return _connection.CoordinateSystemCatalog.ConvertWktToCoordinateSystemCode(_sourceWkt);
                });
                var target = smWkt2Code.GetOrAdd(_targetWkt, wkt =>
                {
                    return _connection.CoordinateSystemCatalog.ConvertWktToCoordinateSystemCode(_targetWkt);
                });

                var builder = _connection.RequestBuilder;
                var req = builder.TransformCoordinates(source, target, new[] { (x, y) });
                using (var s = _connection.OpenRead(req))
                {
                    var xformed = _connection.DeserializeObject<TransformedCoordinateCollection>(s);
                    if (xformed?.TransformedCoordinate?.Length == 1)
                    {
                        var txc = xformed.TransformedCoordinate[0];
                        if (string.IsNullOrEmpty(txc.Error) && txc.X.HasValue && txc.Y.HasValue)
                        {
                            tx = txc.X.Value;
                            ty = txc.Y.Value;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}