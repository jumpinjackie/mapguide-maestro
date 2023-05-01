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

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;

namespace Maestro.Base.Templates
{
    internal class SdfLoadProcedureItemTemplate : ItemTemplate
    {
        public SdfLoadProcedureItemTemplate()
            : base(Strings.TPL_CATEGORY_DEFAULT,
                   null,
                   Strings.TPL_LP_SDF_DESC,
                   Strings.TPL_LP_SDF_NAME,
                   ResourceTypes.LoadProcedure.ToString(),
                   "Sdf", //NOXLATE
                   new Version(1, 0, 0))
        { }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Multiselect = true;
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickSdf, "sdf"); //NOXLATE
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var proc = ObjectFactory.CreateLoadProcedure(LoadType.Sdf, dlg.FileNames);
                    if (!string.IsNullOrEmpty(startPoint) && ResourceIdentifier.IsFolderResource(startPoint))
                    {
                        proc.SubType.RootPath = startPoint;
                        proc.SubType.SpatialDataSourcesPath = startPoint;
                        proc.SubType.LayersPath = startPoint;
                    }
                    return proc;
                }
                return null;
            }
        }
    }
}