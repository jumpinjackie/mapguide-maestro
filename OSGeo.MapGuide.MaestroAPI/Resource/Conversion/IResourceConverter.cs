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

    /// <summary>
    /// Performs schematic upgrades of resources
    /// </summary>
    /// <example>
    /// This example shows how to convert a given Layer Definition to a specific version
    /// <code>
    /// <![CDATA[
    /// ILayerDefinition layerDef;
    /// ...
    /// IResourceConverter converter = new ResourceObjectConverter();
    /// ILayerDefinition converted = converter.Convert(layerDef, new Version(2, 3, 0));
    /// 
    /// Version convertedVersion = converted.ResourceVersion;
    /// // convertedVersion.Major == 2
    /// // convertedVersion.Minor == 3
    /// // convertedVersion.Build == 0
    /// ]]>
    /// </code>
    /// </example>
    public class ResourceObjectConverter : IResourceConverter
    {
        /// <summary>
        /// Converts the resource to the specified version
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="targetVersion">The target version.</param>
        /// <returns></returns>
        public IResource Convert(IResource resource, Version targetVersion)
        {
            //How does this work? If source and target versions are known, it means the classes
            //that represent them are also known and we just serialize from the source type to xml
            //and deserialize that xml to the target type, with any unsupported bits falling by the
            //wayside in the process, and any new bits flubbed with default values as part of deserialization

            var resVer = resource.GetResourceTypeDescriptor().Version;
            var dstVer = string.Format("{0}.{1}.{2}", targetVersion.Major, targetVersion.Minor, targetVersion.Build); //NOXLATE
            var dstXsd = resource.ValidatingSchema.Replace(resVer, dstVer);

            using (var sr = ResourceTypeRegistry.Serialize(resource))
            {
                using (var str = new StreamReader(sr))
                {
                    var xml = new StringBuilder(str.ReadToEnd());
                    xml.Replace(resource.ValidatingSchema, dstXsd);
                    xml.Replace("version=\"" + resVer, "version=\"" + dstVer); //NOXLATE

                    var convRes = ResourceTypeRegistry.Deserialize(xml.ToString());
                    convRes.CurrentConnection = resource.CurrentConnection;
                    convRes.ResourceID = resource.ResourceID;
                    return convRes;
                }
            }
        }
    }
}
