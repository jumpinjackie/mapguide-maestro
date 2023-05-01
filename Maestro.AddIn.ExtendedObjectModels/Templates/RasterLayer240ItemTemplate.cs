#region Disclaimer / License

// Copyright (C) 2019, Jackie Ng
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

using Maestro.Base.Templates;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class RasterLayer240ItemTemplate : ItemTemplate
    {
        public RasterLayer240ItemTemplate()
            : base(Strings.TPL_CATEGORY_MGOS24,
                   Res.layer,
                   Strings.TPL_RLDF_240_DESC,
                   Strings.TPL_RLDF_240_NAME,
                   ResourceTypes.LayerDefinition.ToString(),
                   "Raster", //NOXLATE
                   new Version(2, 4, 0))
        { }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 4);
            }
        }

        public override IResource CreateItem(string startPoint, OSGeo.MapGuide.MaestroAPI.IServerConnection conn)
        {
            using (var picker = new ResourcePicker(conn, ResourceTypes.FeatureSource.ToString(), ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(startPoint);
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var lyr = ObjectFactory.CreateDefaultLayer(OSGeo.MapGuide.ObjectModels.LayerDefinition.LayerType.Raster, new Version(2, 4, 0));
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