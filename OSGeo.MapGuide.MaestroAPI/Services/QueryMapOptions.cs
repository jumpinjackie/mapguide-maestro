#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Additional options for a QueryMapFeatures operation
    /// </summary>
    public class QueryMapOptions
    {
        /// <summary>
        /// An array of layer names to restrict the selection to
        /// </summary>
        public string[] LayerNames { get; set; }

        /// <summary>
        /// Bitmask specifying the attributes a layer must have to be considered in the selection process. The following attributes are supported:
        /// 1 - Layer is visible
        /// 2 - Layer is selectable
        /// 4 - Layer has a tooltip defined
        /// Combinations of one or more attributes are allowed.
        /// </summary>
        public QueryMapFeaturesLayerAttributes LayerAttributeFilter { get; set; }

        /// <summary>
        /// XML filter describing a set of previously selected features. This paramter is useful for obtaining the attribute values of a previously selected feature. 
        /// </summary>
        public string FeatureFilter { get; set; }
    }
}
