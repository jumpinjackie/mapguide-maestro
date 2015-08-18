#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

#endregion Disclaimer / License
using System.Collections.Generic;

namespace Maestro.AddIn.Rest.Model
{
    public class DataConfigurationResponse
    {
        public DataConfigurationResponse() { }

        /// <summary>
        /// The list of data configurations
        /// </summary>
        public DataConfigurationList DataConfigurationList { get; set; }
    }

    public class DataConfigurationList
    {
        public DataConfigurationList() { }

        /// <summary>
        /// The base endpoint URI for mapguide-rest
        /// </summary>
        public string RootUri { get; set; }
        /// <summary>
        /// The associated mapagent URL
        /// </summary>
        public string MapAgentUrl { get; set; }
        /// <summary>
        /// The array of configured published data sources
        /// </summary>
        public List<DataConfiguration> Configuration { get; set; }
    }

    public class DataConfiguration
    {
        public DataConfiguration() { }

        /// <summary>
        /// The relative URI part to read/write the configuration file
        /// </summary>
        public string ConfigUriPart { get; set; }
        /// <summary>
        /// The relative URI part to access the API documentation for this data configuration
        /// </summary>
        public string DocUriPart { get; set; }
    }
}
