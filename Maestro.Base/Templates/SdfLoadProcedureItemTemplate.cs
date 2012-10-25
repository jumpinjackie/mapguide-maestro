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
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;

namespace Maestro.Base.Templates
{
    internal class SdfLoadProcedureItemTemplate : ItemTemplate
    {
        public SdfLoadProcedureItemTemplate()
        {
            Category = Strings.TPL_CATEGORY_DEFAULT;
            //Icon = Res.document;
            Description = Strings.TPL_LP_SDF_DESC;
            Name = Strings.TPL_LP_SDF_NAME;
            ResourceType = ResourceTypes.LoadProcedure.ToString();
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Multiselect = true;
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickSdf, "sdf"); //NOXLATE
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var proc = ObjectFactory.CreateLoadProcedure(conn, LoadType.Sdf, dlg.FileNames);
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
