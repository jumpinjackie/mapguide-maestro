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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using Res = Maestro.Base.Properties.Resources;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Editors.Generic;

namespace Maestro.Base.Templates
{
    internal class WebLayoutItemTemplate : ItemTemplate
    {
        public WebLayoutItemTemplate()
        {
            Category = Strings.TPL_CATEGORY_DEFAULT;
            Icon = Res.application_browser;
            Description = Strings.TPL_WL_DESC;
            Name = Strings.TPL_WL_NAME;
            ResourceType = ResourceTypes.WebLayout.ToString();
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            //This is to just ensure we have a functional WebLayout when it's created
            using (var picker = new ResourcePicker(conn.ResourceService, ResourceTypes.MapDefinition, ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(startPoint);
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var wl = ObjectFactory.CreateWebLayout(conn, new Version(1, 0, 0), string.Empty);
                    wl.Map.ResourceId = picker.ResourceID;
                    return wl;
                }
            }
            return null;
        }
    }
}
