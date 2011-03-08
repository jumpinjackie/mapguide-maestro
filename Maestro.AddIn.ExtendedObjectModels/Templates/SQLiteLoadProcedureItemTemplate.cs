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
using Maestro.Base.Templates;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class SQLiteLoadProcedureItemTemplate  : ItemTemplate
    {
        public SQLiteLoadProcedureItemTemplate()
        {
            Category = Res.TPL_CATEGORY_MGOS22;
            //Icon = Res.document;
            Description = Res.TPL_LP_SQLITE_DESC;
            Name = Res.TPL_LP_SQLITE_NAME;
            ResourceType = ResourceTypes.LoadProcedure.ToString();
        }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 2);
            }
        }

        public override IResource CreateItem(IServerConnection conn)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Multiselect = true;
                dlg.Filter = Properties.Resources.Filter_Sqlite_Files;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return ObjectFactory.CreateLoadProcedure(conn, LoadType.Sqlite, dlg.FileNames);
                }
                return null;
            }
        }
    }
}
