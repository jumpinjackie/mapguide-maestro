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
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Editors.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Maestro.Base.Commands.Test
{
    internal class TriggerDbXmlCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var mgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = mgr.GetConnection(exp.ConnectionName);

            string xml = 
                "<FeatureSource xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xsi:noNamespaceSchemaLocation=\"FeatureSource-1.0.0.xsd\">\n" + 
                "<Foo />\n" +
                "</FeatureSource>";
            try
            {
                string resId = "Session:" + conn.SessionID + "//Dummy.FeatureSource";
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    conn.ResourceService.SetResourceXmlData(resId, ms);
                }
            }
            catch (Exception ex)
            {
                XmlContentErrorDialog.CheckAndHandle(ex, xml, false);
            }
        }
    }
}
