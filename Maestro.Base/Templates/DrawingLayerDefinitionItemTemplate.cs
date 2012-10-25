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
using Res = Maestro.Base.Properties.Resources;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Base.Templates
{
    internal class DrawingLayerDefinitionItemTemplate : ItemTemplate
    {
        public DrawingLayerDefinitionItemTemplate()
        {
            Category = Strings.TPL_CATEGORY_DEFAULT;
            Icon = Res.layer;
            Description = Strings.TPL_DLDF_DESC;
            Name = Strings.TPL_DLDF_NAME;
            ResourceType = ResourceTypes.LayerDefinition.ToString();
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            using (var picker = new ResourcePicker(conn.ResourceService, ResourceTypes.DrawingSource, ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(startPoint);
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var ldf = ObjectFactory.CreateDefaultLayer(conn, OSGeo.MapGuide.ObjectModels.LayerDefinition.LayerType.Drawing, new Version(1, 0, 0));
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
