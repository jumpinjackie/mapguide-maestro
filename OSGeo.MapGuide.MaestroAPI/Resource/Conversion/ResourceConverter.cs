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
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Conversion
{
    /// <summary>
    /// Resource conversion services
    /// </summary>
    public class ResourceConverter : IResourceConversionService
    {
        private IList<IResourceConverter> _upgraders;

        internal ResourceConverter(IList<IResourceConverter> upgraders)
        {
            ValidateConverterList(upgraders);
            _upgraders = upgraders;
        }

        private void ValidateConverterList(IList<IResourceConverter> upgraders)
        {
            var conv = new Dictionary<ResourceTypeDescriptor, IResourceConverter>();
            foreach (var upg in upgraders)
            {
                var desc = new ResourceTypeDescriptor(upg.ResourceType, upg.SourceVersion.ToString());
                if (conv.ContainsKey(desc))
                    throw new ResourceConversionException(string.Format(Properties.Resources.ERR_CONVERTER_ALREADY_REGISTERED, upg.ResourceType + " " + upg.SourceVersion));

                conv.Add(desc, upg);
            }
        }

        private IResourceConverter FindUpgrader(ResourceTypes resourceType, Version source)
        {
            foreach (var conv in _upgraders)
            {
                if (conv.SourceVersion == source && conv.ResourceType == resourceType)
                    return conv;
            }
            return null;
        }

        /// <summary>
        /// Performs the upgrade of a given resource. If the versions differ
        /// by more than one revision, the upgrade is done incrementally. (eg. Upgrading
        /// a 1.0.0 Layer Definition to 1.2.0 will go from:
        ///  - 1.0.0 to 1.1.0
        ///  - 1.1.0 to 1.2.0
        ///  
        /// If the target version matches the resource's version, the original resource is returned
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public IResource Upgrade(IResource resource, Version version)
        {
            if (resource.ResourceVersion == version)
                return resource;

            var rt = resource.ResourceType;
            IResource res = (IResource)resource.Clone();
            Version currentVer = res.ResourceVersion;
            //Construct the upgrade path
            List<IResourceConverter> upgradePath = new List<IResourceConverter>();
            while (currentVer < version)
            {
                var conv = FindUpgrader(rt, currentVer);
                if (conv == null)
                    break;

                upgradePath.Add(conv);
                currentVer = conv.TargetVersion;
            }

            if (currentVer != version)
            {
                throw new ResourceConversionException(Properties.Resources.ERR_NO_UPGRADE_PATH);
            }

            for (int i = 0; i < upgradePath.Count; i++)
            {
                res = upgradePath[i].Convert(res);
            }
            return res;
        }
    }
}
