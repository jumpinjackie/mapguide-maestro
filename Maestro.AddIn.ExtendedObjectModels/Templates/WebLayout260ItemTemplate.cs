#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using Maestro.Base.Templates;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using System;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class WebLayout260ItemTemplate : ItemTemplate
    {
        public WebLayout260ItemTemplate()
        {
            Category = Strings.TPL_CATEGORY_MGOS26;
            Icon = Res.application_browser;
            Description = Strings.TPL_WL_260_DESC;
            Name = Strings.TPL_WL_260_NAME;
            ResourceType = ResourceTypes.WebLayout.ToString();
        }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 6);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            //This is to just ensure we have a functional WebLayout when it's created
            using (var picker = new ResourcePicker(conn, ResourceTypes.MapDefinition.ToString(), ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(startPoint);
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var wl = ObjectFactory.CreateWebLayout(new Version(2, 6, 0), string.Empty);
                    wl.Map.ResourceId = picker.ResourceID;
                    return wl;
                }
            }
            return null;
        }
    }
}