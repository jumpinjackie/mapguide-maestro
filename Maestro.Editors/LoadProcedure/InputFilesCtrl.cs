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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;
using System.Diagnostics;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;

namespace Maestro.Editors.LoadProcedure
{
    internal partial class InputFilesCtrl : EditorBindableCollapsiblePanel
    {
        public InputFilesCtrl()
        {
            InitializeComponent();
        }

        private IBaseLoadProcedure _lpt;

        public override void Bind(IEditorService service)
        {
            var lp = service.GetEditedResource() as ILoadProcedure;
            Debug.Assert(lp != null);

            _lpt = (IBaseLoadProcedure)lp.SubType;

            service.RegisterCustomNotifier(this);
            lstInputFiles.DataSource = _lpt.SourceFile;
        }

        private void lstInputFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = (lstInputFiles.SelectedItems.Count > 0);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string filter = GetFilter();
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Filter = filter;
                dlg.Multiselect = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _lpt.AddFiles(dlg.FileNames);
                }
            }
        }

        private string GetFilter()
        {
            string filter = Properties.Resources.FilterAll;
            if (_lpt.Type == LoadType.Sdf)
                filter = Properties.Resources.FilterSdf;
            else if (_lpt.Type == LoadType.Shp)
                filter = Properties.Resources.FilterShp;
            else if (_lpt.Type == LoadType.Dwf)
                filter = Properties.Resources.FilterDwf;
            else if (_lpt.Type == LoadType.Sqlite)
                filter = Properties.Resources.FilterSqlite;

            return filter;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstInputFiles.SelectedItems.Count > 0)
            {
                var files = new List<string>();
                foreach (var obj in lstInputFiles.SelectedItems)
                {
                    files.Add(obj.ToString());
                }
                foreach (var f in files)
                {
                    _lpt.RemoveFile(f);
                }
            }
        }
    }
}
