#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

#endregion Disclaimer / License

using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Linq;

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    /// <summary>
    /// A simple coordinate transformation interface
    /// </summary>
    public interface ISimpleTransform : IDisposable
    {
        /// <summary>
        /// Transforms the specified point
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="tx">The transformed X coordinate</param>
        /// <param name="ty">The transformed Y coordinate</param>
        bool Transform(double x, double y, out double tx, out double ty);
    }

    /// <summary>
    /// A simple transform that wraps the NTS coordinate system transformation APIs
    /// </summary>
    public class DefaultSimpleTransform : ISimpleTransform
    {
        private ProjNet.CoordinateSystems.CoordinateSystem _source;
        private ProjNet.CoordinateSystems.CoordinateSystem _target;
        private ICoordinateTransformation _trans;

        static readonly string[] CSMAP_WGS84_PSEUDO_MERCATOR_ALIASES =
        {
            // The defn of WGS84.PseudoMercator may vary between releases of MapGuide, so add all known variants here
            @"PROJCS[""WGS84.PseudoMercator"",GEOGCS[""LL84"",DATUM[""WGS84"",SPHEROID[""WGS84"",6378137.000,298.25722293]],PRIMEM[""Greenwich"",0],UNIT[""Degree"",0.017453292519943295]],PROJECTION[""Popular Visualisation Pseudo Mercator""],PARAMETER[""false_easting"",0.000],PARAMETER[""false_northing"",0.000],PARAMETER[""central_meridian"",0.00000000000000],UNIT[""Meter"",1.00000000000000]]", //NOXLATE
             "PROJCS[\"WGS84.PseudoMercator\",GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722356]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Popular Visualisation Pseudo Mercator\"],PARAMETER[\"false_easting\",0.000],PARAMETER[\"false_northing\",0.000],PARAMETER[\"central_meridian\",0.00000000000000],UNIT[\"Meter\",1.00000000000000]]" //NOXLATE
        };

        // Proj.Net cannot handle the WGS84.PseudoMercator WKT. Here's an alternative WKT that is Proj.Net compatible and produces
        // approximately similar results:
        //
        // http://alastaira.wordpress.com/2011/01/23/the-google-maps-bing-maps-spherical-mercator-projection/
        //
        private const string POPULAR_VISUALISATION_CRS =
@"PROJCS[""Popular Visualisation CRS / Mercator"",
 GEOGCS[""Popular Visualisation CRS"",
  DATUM[""WGS84"",
    SPHEROID[""WGS84"", 6378137.0, 298.257223563, AUTHORITY[""EPSG"",""7059""]],
  AUTHORITY[""EPSG"",""6055""]],
 PRIMEM[""Greenwich"", 0, AUTHORITY[""EPSG"", ""8901""]],
 UNIT[""degree"", 0.0174532925199433, AUTHORITY[""EPSG"", ""9102""]],
 AXIS[""E"", EAST], AXIS[""N"", NORTH], AUTHORITY[""EPSG"",""4055""]],
PROJECTION[""Mercator""],
PARAMETER[""semi_minor"",6378137],
PARAMETER[""False_Easting"", 0],
PARAMETER[""False_Northing"", 0],
PARAMETER[""Central_Meridian"", 0],
PARAMETER[""Latitude_of_origin"", 0],
UNIT[""metre"", 1, AUTHORITY[""EPSG"", ""9001""]],
AXIS[""East"", EAST], AXIS[""North"", NORTH],
AUTHORITY[""EPSG"",""3785""]]"; //NOXLATE

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSimpleTransform"/> class.
        /// </summary>
        /// <param name="sourceCsWkt">The source cs WKT.</param>
        /// <param name="targetCsWkt">The target cs WKT.</param>
        internal DefaultSimpleTransform(string sourceCsWkt, string targetCsWkt)
        {
            //Check for and replace the WGS84.PseudoMercator WKT
            string srcWkt = CSMAP_WGS84_PSEUDO_MERCATOR_ALIASES.Contains(sourceCsWkt) ? POPULAR_VISUALISATION_CRS : sourceCsWkt;
            string dstWkt = CSMAP_WGS84_PSEUDO_MERCATOR_ALIASES.Contains(targetCsWkt) ? POPULAR_VISUALISATION_CRS : targetCsWkt;
            var fact = new CoordinateSystemFactory();
            _source = fact.CreateFromWkt(srcWkt);
            _target = fact.CreateFromWkt(dstWkt);
            var tfact = new CoordinateTransformationFactory();
            _trans = tfact.CreateFromCoordinateSystems(_source, _target);
        }

        /// <summary>
        /// Transforms the specified point
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="tx">The transformed X coordinate</param>
        /// <param name="ty">The transformed Y coordinate</param>
        public bool Transform(double x, double y, out double tx, out double ty)
        {
            tx = Double.NaN;
            ty = Double.NaN;

            double[] pts = _trans.MathTransform.Transform(new double[] { x, y });
            tx = pts[0];
            ty = pts[1];
            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}