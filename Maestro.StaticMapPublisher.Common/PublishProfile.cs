using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Maestro.StaticMapPublisher.Common
{
    public class BoundingBox
    {
        public double MinX { get; set; }

        public double MinY { get; set; }

        public double MaxX { get; set; }

        public double MaxY { get; set; }
    }

    public class TileSetRef
    {
        public string ResourceID { get; set; }

        public string GroupName { get; set; }
    }

    public abstract class ViewerOptionsBase
    {
        public abstract ViewerType Type { get; }
    }

    public class OpenLayersViewerOptions : ViewerOptionsBase
    {
        public override ViewerType Type => ViewerType.OpenLayers;
    }

    public class LeafletViewerOptions : ViewerOptionsBase
    {
        public override ViewerType Type => ViewerType.Leaflet;
    }

    public class PublishProfile : IStaticMapPublishingOptions
    {
        public string Title { get; set; }

        public int? MaxDegreeOfParallelism { get; set; }

        public string MapAgent { get; set; }

        public string OutputDirectory { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool RandomizeRequests { get; set; }

        public ViewerOptionsBase ViewerOptions { get; set; }

        public BoundingBox Bounds { get; set; }

        public TileSetRef ImageTileSet { get; set; }

        public TileSetRef UTFGridTileSet { get; set; }

        public IEnumerable<ExternalBaseLayer> ExternalBaseLayers { get; set; }

        public IEnumerable<OverlayLayer> OverlayLayers { get; set; }

        public ViewerType Viewer => ViewerOptions.Type;

        public string ImageTileSetDefinition => ImageTileSet?.ResourceID;

        public string ImageTileSetGroup => ImageTileSet?.GroupName;

        public string UTFGridTileSetDefinition => UTFGridTileSet?.ResourceID;

        public string UTFGridTileSetGroup => UTFGridTileSet?.GroupName;

        IEnvelope IStaticMapPublishingOptions.Bounds => ObjectFactory.CreateEnvelope(this.Bounds.MinX, this.Bounds.MinY, this.Bounds.MaxX, this.Bounds.MaxY);

        public IServerConnection Connection { get; private set; }

        public void Validate(TextWriter stdout)
        {
            var bounds = this.Bounds;
            if (!Utility.InRange(bounds.MinX, -180, 180))
                throw new Exception("minx not in range of [-180, 180]");
            if (!Utility.InRange(bounds.MaxX, -180, 180))
                throw new Exception("maxx not in range of [-180, 180]");
            if (!Utility.InRange(bounds.MinY, -90, 90))
                throw new Exception("miny not in range of [-90, 90]");
            if (!Utility.InRange(bounds.MaxY, -90, 90))
                throw new Exception("maxy not in range of [-90, 90]");

            if (bounds.MinX > bounds.MaxX)
                throw new Exception("Invalid BBOX: minx > maxx");
            if (bounds.MinY > bounds.MaxY)
                throw new Exception("Invalid BBOX: miny > maxy");

            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Url"] = this.MapAgent; //NOXLATE
            builder["Username"] = this.Username; //NOXLATE
            builder["Password"] = this.Password; //NOXLATE
            builder["Locale"] = "en"; //NOXLATE
            builder["AllowUntestedVersion"] = true; //NOXLATE

            string agent = "MapGuide Maestro v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); //NOXLATE

            var conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http", builder.ToString()); //NOXLATE
            conn.SetCustomProperty("UserAgent", agent); //NOXLATE

            // Must be MapGuide Open Source 4.0
            if (conn.SiteVersion < new Version(4, 0))
            {
                throw new Exception($"This tool requires capabilities present in MapGuide Open Source 4.0 and newer. Version {conn.SiteVersion} is not supported");
            }

            if (!string.IsNullOrEmpty(this.ImageTileSetDefinition))
            {
                var resId = new ResourceIdentifier(this.ImageTileSetDefinition);
                if (resId.ResourceType != nameof(ResourceTypes.TileSetDefinition))
                {
                    throw new Exception("Image tile set definiiton is not a tile set definition resource id");
                }

                var tsd = conn.ResourceService.GetResource(this.ImageTileSetDefinition) as ITileSetDefinition;
                if (tsd == null)
                    throw new Exception("Not an image tile set definition resource");

                if (tsd.TileStoreParameters.TileProvider != "XYZ")
                {
                    throw new Exception("Not a XYZ image tile set definition resource");
                }

                if (string.IsNullOrEmpty(this.ImageTileSetGroup))
                {
                    this.ImageTileSet.GroupName = tsd.BaseMapLayerGroups.First().Name;
                    stdout.WriteLine($"Defaulting to layer group ({this.ImageTileSetGroup}) for image tileset");
                }
                else
                {
                    if (!tsd.GroupExists(this.ImageTileSetGroup))
                        throw new Exception($"The specified group ({this.ImageTileSetGroup}) does not exist in the specified image tile set definition");
                }
            }
            if (!string.IsNullOrEmpty(this.UTFGridTileSetDefinition))
            {
                var resId = new ResourceIdentifier(this.UTFGridTileSetDefinition);
                if (resId.ResourceType != nameof(ResourceTypes.TileSetDefinition))
                {
                    throw new Exception("UTFGrid tile set definiiton is not a tile set definition resource id");
                }

                var tsd = conn.ResourceService.GetResource(this.UTFGridTileSetDefinition) as ITileSetDefinition;
                if (tsd == null)
                    throw new Exception("Not an UTFGrid tile set definition resource");

                if (tsd.TileStoreParameters.TileProvider != "XYZ")
                {
                    throw new Exception("Not a XYZ UTFGrid tile set definition resource");
                }

                if (string.IsNullOrEmpty(this.UTFGridTileSetGroup))
                {
                    this.UTFGridTileSet.GroupName = tsd.BaseMapLayerGroups.First().Name;
                    stdout.WriteLine($"Defaulting to layer group ({this.UTFGridTileSetGroup}) for utfgrid tileset");
                }
                else
                {
                    if (!tsd.GroupExists(this.UTFGridTileSetGroup))
                        throw new Exception($"The specified group ({this.UTFGridTileSetGroup}) does not exist in the specified utfgrid tile set definition");
                }
            }

            Connection = conn;
        }
    }


}
