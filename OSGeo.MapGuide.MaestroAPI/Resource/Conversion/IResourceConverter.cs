#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using System.Text;
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Conversion
{
    /// <summary>
    /// Performs schematic upgrades of resources
    /// </summary>
    public interface IResourceConverter
    {
        /// <summary>
        /// Converts the resource to the specified version
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="targetVersion">The target version.</param>
        /// <returns></returns>
        IResource Convert(IResource resource, Version targetVersion);
    }

    public class ResourceObjectConverter : IResourceConverter
    {
        public IResource Convert(IResource resource, Version targetVersion)
        {
            var resVer = resource.GetResourceTypeDescriptor().Version;
            var dstVer = string.Format("{0}.{1}.{2}", targetVersion.Major, targetVersion.Minor, targetVersion.Build);
            var dstXsd = resource.ValidatingSchema.Replace(resVer, dstVer);

            using (var sr = ResourceTypeRegistry.Serialize(resource))
            {
                using (var str = new StreamReader(sr))
                {
                    var xml = new StringBuilder(str.ReadToEnd());
                    xml.Replace(resource.ValidatingSchema, dstXsd);
                    xml.Replace("version=\"" + resVer, "version=\"" + dstVer);

                    var convRes = ResourceTypeRegistry.Deserialize(xml.ToString());
                    convRes.CurrentConnection = resource.CurrentConnection;
                    return convRes;
                }
            }
        }
    }
}
