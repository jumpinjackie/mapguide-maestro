﻿#region Disclaimer / License

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

using Maestro.Base.Templates;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class SQLiteLoadProcedureItemTemplate : ItemTemplate
    {
        public SQLiteLoadProcedureItemTemplate()
            : base(Strings.TPL_CATEGORY_MGOS22,
                   null,
                   Strings.TPL_LP_SQLITE_DESC,
                   Strings.TPL_LP_SQLITE_NAME,
                   ResourceTypes.LoadProcedure.ToString(),
                   "Sqlite", //NOXLATE
                   new Version(1, 0, 0))
        { }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 2);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Multiselect = true;
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickSqlite, "sqlite") + "|" + //NOXLATE
                             string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickSqlite, "db") + "|" + //NOXLATE
                             string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickSqlite, "sdx") + "|" + //NOXLATE
                             string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickSqlite, "slt"); //NOXLATE
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var proc = ObjectFactory.CreateLoadProcedure(LoadType.Sqlite, dlg.FileNames);
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