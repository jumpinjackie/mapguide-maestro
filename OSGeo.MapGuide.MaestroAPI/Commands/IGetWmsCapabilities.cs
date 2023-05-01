#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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


using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    /// <summary>
    /// Version of WMS supported by MapGuide
    /// </summary>
    public enum WmsVersion
    {
        /// <summary>
        /// WMS v1.0.0
        /// </summary>
        v1_0_0,
        /// <summary>
        /// WMS v1.1.0
        /// </summary>
        v1_1_0,
        /// <summary>
        /// WMS v1.1.1
        /// </summary>
        v1_1_1,
        /// <summary>
        /// WMS v1.3.0
        /// </summary>
        v1_3_0,
    }

    /// <summary>
    /// Gets the document describing the WMS capabilities of this service
    /// </summary>
    public interface IGetWmsCapabilities : ICommand
    {
        /// <summary>
        /// Requests WMS capabilities for the given WMS version
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Stream Execute(WmsVersion version);

        /// <summary>
        /// Asynchronously requests WMS capabilities for the given WMS version
        /// </summary>
        /// <param name="version"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<Stream> ExecuteAsync(WmsVersion version, CancellationToken cancel = default);
    }
}
