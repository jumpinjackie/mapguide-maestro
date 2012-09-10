#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.Xml;
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.CrossConnection
{
    /// <summary>
    /// Resource re-basing options
    /// </summary>
    public class RebaseOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RebaseOptions"/> class.
        /// </summary>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="targetFolder">The target folder.</param>
        public RebaseOptions(string sourceFolder, string targetFolder)
        {
            Check.Precondition(ResourceIdentifier.IsFolderResource(sourceFolder), "ResourceIdentifier.IsFolderResource(sourceFolder)"); //NOXLATE
            Check.Precondition(ResourceIdentifier.IsFolderResource(targetFolder), "ResourceIdentifier.IsFolderResource(targetFolder)"); //NOXLATE

            this.SourceFolder = sourceFolder;
            this.TargetFolder = targetFolder;
        }

        /// <summary>
        /// The source folder to look for in resource ids
        /// </summary>
        public string SourceFolder
        {
            get;
            private set;
        }

        /// <summary>
        /// The target folder to replace with
        /// </summary>
        public string TargetFolder
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// A helper class to re-base referenced resource ids in a resource document
    /// </summary>
    public class ResourceRebaser
    {
        private IResource _res;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRebaser"/> class.
        /// </summary>
        /// <param name="res">The res.</param>
        public ResourceRebaser(IResource res)
        {
            _res = res;
        }

        /// <summary>
        /// Re-bases any resource id references in the resource document
        /// </summary>
        /// <param name="sourceRoot"></param>
        /// <param name="targetRoot"></param>
        /// <returns>A re-based copy of the original resource</returns>
        public IResource Rebase(string sourceRoot, string targetRoot)
        {
            if (sourceRoot == targetRoot)
                return _res;

            var xml = ResourceTypeRegistry.SerializeAsString(_res);
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var elements = doc.GetElementsByTagName("ResourceId"); //NOXLATE
            foreach (XmlNode el in elements)
            {
                el.InnerText = el.InnerText.Replace(sourceRoot, targetRoot);
            }

            using (var ms = new MemoryStream())
            {
                doc.Save(ms);
                ms.Position = 0L;
                var modifiedRes = ResourceTypeRegistry.Deserialize(_res.ResourceType, ms);
                modifiedRes.CurrentConnection = _res.CurrentConnection;
                modifiedRes.ResourceID = _res.ResourceID;

                return modifiedRes;
            }
        }
    }
}
