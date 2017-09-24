#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using OSGeo.MapGuide.ObjectModels;
using System.Xml;

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
            Check.ThatPreconditionIsMet(ResourceIdentifier.IsFolderResource(sourceFolder), $"{nameof(ResourceIdentifier)}.{nameof(ResourceIdentifier.IsFolderResource)}({nameof(sourceFolder)})");
            Check.ThatPreconditionIsMet(ResourceIdentifier.IsFolderResource(targetFolder), $"{nameof(ResourceIdentifier)}.{nameof(ResourceIdentifier.IsFolderResource)}({nameof(targetFolder)})");

            this.SourceFolder = sourceFolder;
            this.TargetFolder = targetFolder;
        }

        /// <summary>
        /// The source folder to look for in resource ids
        /// </summary>
        public string SourceFolder
        {
            get;
        }

        /// <summary>
        /// The target folder to replace with
        /// </summary>
        public string TargetFolder
        {
            get;
        }
    }

    /// <summary>
    /// A helper class to re-base referenced resource ids in a resource document
    /// </summary>
    /// <example>
    /// How to use the ResourceRebaser
    /// <code>
    /// IServerConnection conn;
    /// ...
    /// IResource layerDef = conn.ResourceService.GetResource("Library://Test/Sample.LayerDefinition");
    /// var rebaser = new ResourceRebaser(layerDef);
    /// //Change all resource id references within to point to the new parent location
    /// rebaser.Rebase("Library://Test/", "Library://Rebased/");
    /// </code>
    /// </example>
    public class ResourceRebaser
    {
        private readonly IResource _res;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRebaser"/> class.
        /// </summary>
        /// <param name="res">The res.</param>
        public ResourceRebaser(IResource res)
        {
            Check.ArgumentNotNull(res, nameof(res));
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

            var xml = ObjectFactory.SerializeAsString(_res);
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var elements = doc.GetElementsByTagName("ResourceId"); //NOXLATE
            foreach (XmlNode el in elements)
            {
                el.InnerText = el.InnerText.Replace(sourceRoot, targetRoot);
            }

            using (var ms = MemoryStreamPool.GetStream())
            {
                doc.Save(ms);
                ms.Position = 0L;
                var modifiedRes = ObjectFactory.Deserialize(_res.ResourceType, ms);
                modifiedRes.ResourceID = _res.ResourceID;

                return modifiedRes;
            }
        }
    }
}