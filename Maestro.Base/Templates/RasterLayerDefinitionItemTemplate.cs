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
using OSGeo.MapGuide.MaestroAPI.Resource;
using Res = Maestro.Base.Properties.Resources;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.Base.Templates
{
    internal class RasterLayerDefinitionItemTemplate : ItemTemplate
    {
        public RasterLayerDefinitionItemTemplate()
        {
            Category = Strings.TPL_CATEGORY_DEFAULT;
            Icon = Res.layer;
            Description = Strings.TPL_RLDF_DESC;
            Name = Strings.TPL_RLDF_NAME;
            ResourceType = ResourceTypes.LayerDefinition.ToString();
        }

        //temp disable as raster support is still being ported from 2.x
        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 0);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            using (var picker = new ResourcePicker(conn.ResourceService, ResourceTypes.FeatureSource, ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(startPoint);
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var lyr = ObjectFactory.CreateDefaultLayer(conn, OSGeo.MapGuide.ObjectModels.LayerDefinition.LayerType.Raster, new Version(1, 0, 0));
                    var rl = (IRasterLayerDefinition)lyr.SubLayer;
                    rl.ResourceId = picker.ResourceID;
                    //Stub these for now, validation will ensure this never makes it
                    //into the session repository until all validation errors pass
                    rl.FeatureName = string.Empty;
                    rl.Geometry = string.Empty;
                    return lyr;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
