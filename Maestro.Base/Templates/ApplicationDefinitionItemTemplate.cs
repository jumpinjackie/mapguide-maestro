#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using Res = Maestro.Base.Properties.Resources;

namespace Maestro.Base.Templates
{
    internal class ApplicationDefinitionItemTemplate : ItemTemplate
    {
        public ApplicationDefinitionItemTemplate()
            : base(Strings.TPL_CATEGORY_DEFAULT,
                   Res.applications_stack,
                   Strings.TPL_ADF_DESC,
                   Strings.TPL_ADF_NAME,
                   ResourceTypes.ApplicationDefinition.ToString(),
                   null,
                   new Version(1, 0, 0))
        { }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 0);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            //This is to just ensure we have a functional template when it's created
            using (var picker = new ResourcePicker(conn, ResourceTypes.MapDefinition.ToString(), ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(startPoint);
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var flex = Utility.CreateFlexibleLayout(conn, FusionTemplateNames.Slate);
                    var grp = flex.MapSet.GetGroupAt(0);
                    var map = grp.GetMapAt(0);

                    map.SetMapDefinition(picker.ResourceID);
                    return flex;
                }
            }
            return null;
        }
    }
}