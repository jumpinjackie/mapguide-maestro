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
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Xml;
using System.IO;

namespace Maestro.Base.Util
{
    internal class XmlCompareUtil
    {
        /// <summary>
        /// Prepares the source and target resource content for XML comparison
        /// </summary>
        /// <param name="resSvc"></param>
        /// <param name="sourceId"></param>
        /// <param name="targetId"></param>
        /// <param name="sourceFile"></param>
        /// <param name="targetFile"></param>
        public static void PrepareForComparison(IResourceService resSvc, string sourceId, string targetId, out string sourceFile, out string targetFile)
        {
            //Route both source and target XML content through
            //XmlDocument objects to ensure issues like whitespacing do
            //not throw us off
            sourceFile = Path.GetTempFileName();
            targetFile = Path.GetTempFileName();
            using (var sourceStream = resSvc.GetResourceXmlData(sourceId))
            using (var targetStream = resSvc.GetResourceXmlData(targetId))
            {
                var sourceDoc = new XmlDocument();
                var targetDoc = new XmlDocument();

                sourceDoc.Load(sourceStream);
                targetDoc.Load(targetStream);

                using (var fs = File.OpenWrite(sourceFile))
                using (var ft = File.OpenWrite(targetFile))
                {
                    sourceDoc.Save(fs);
                    targetDoc.Save(ft);
                }
            }
        }
    }
}
