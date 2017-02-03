#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using System.IO;
using System.Xml;

namespace Maestro.Editors.Diff
{
    /// <summary>
    /// Represents an XML comparison between a source and target
    /// </summary>
    public class XmlComparisonSet
    {
        internal XmlComparisonSet(TextFileDiffList source, TextFileDiffList target)
        {
            this.Source = source;
            this.Target = target;
        }

        /// <summary>
        /// Gets the difference list for the source
        /// </summary>
        public TextFileDiffList Source { get; }

        /// <summary>
        /// Gets the difference list for the target
        /// </summary>
        public TextFileDiffList Target { get; }
    }

    /// <summary>
    /// A helper utility that performs the necessary preparation of two resoures for XML comparison
    /// </summary>
    public static class XmlCompareUtil
    {
        /// <summary>
        /// Prepares the source and target resource content for XML comparison
        /// </summary>
        /// <param name="resSvc">The resource service</param>
        /// <param name="sourceId">The source resource ID</param>
        /// <param name="targetId">The target resource ID</param>
        public static XmlComparisonSet PrepareForComparison(IResourceService resSvc, string sourceId, string targetId)
        {
            //Route both source and target XML content through
            //XmlDocument objects to ensure issues like whitespacing do
            //not throw us off
            var sourceFile = Path.GetTempFileName();
            var targetFile = Path.GetTempFileName();

            IResource source = resSvc.GetResource(sourceId);
            IResource target = resSvc.GetResource(targetId);

            var sourceDoc = new XmlDocument();
            var targetDoc = new XmlDocument();

            using (var sourceStream = ObjectFactory.Serialize(source))
            using (var targetStream = ObjectFactory.Serialize(target))
            {
                sourceDoc.Load(sourceStream);
                targetDoc.Load(targetStream);

                sourceDoc.Normalize();
                targetDoc.Normalize();

                using (var fs = File.OpenWrite(sourceFile))
                using (var ft = File.OpenWrite(targetFile))
                {
                    sourceDoc.Save(fs);
                    targetDoc.Save(ft);
                }

                return new XmlComparisonSet(
                    new TextFileDiffList(sourceFile, true),
                    new TextFileDiffList(targetFile, true));
            }
        }
    }
}