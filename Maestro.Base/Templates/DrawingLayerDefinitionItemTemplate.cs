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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using Res = Maestro.Base.Properties.Resources;

namespace Maestro.Base.Templates
{
    internal class DrawingLayerDefinitionItemTemplate : ItemTemplate
    {
        public DrawingLayerDefinitionItemTemplate()
            : base(Strings.TPL_CATEGORY_DEFAULT,
                   Res.layer,
                   Strings.TPL_DLDF_DESC,
                   Strings.TPL_DLDF_NAME,
                   ResourceTypes.LayerDefinition.ToString(),
                   "Drawing", //NOXLATE
                   new Version(1, 0, 0))
        { }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            using (var picker = new ResourcePicker(conn, ResourceTypes.DrawingSource.ToString(), ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(startPoint);
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var ldf = ObjectFactory.CreateDefaultLayer(OSGeo.MapGuide.ObjectModels.LayerDefinition.LayerType.Drawing, new Version(1, 0, 0));
                    ldf.SubLayer.ResourceId = picker.ResourceID;
                    var dl = ((IDrawingLayerDefinition)ldf.SubLayer);
                    dl.LayerFilter = string.Empty;
                    dl.MinScale = 0;

                    IDrawingService dwSvc = (IDrawingService)conn.GetService((int)ServiceType.Drawing);
                    var sheets = dwSvc.EnumerateDrawingSections(picker.ResourceID);
                    dl.Sheet = sheets.Section[0].Name;

                    return ldf;
                }
                return null;
            }
        }
    }
}