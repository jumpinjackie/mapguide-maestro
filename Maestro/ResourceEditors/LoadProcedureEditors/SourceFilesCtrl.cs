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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    public partial class SourceFilesCtrl : UserControl
    {
        public SourceFilesCtrl()
        {
            InitializeComponent();
        }

        public string FileFilter
        {
            get { return openFileDlg.Filter; }
            set { openFileDlg.Filter = value; }
        }

        public string[] SourceFiles
        {
            get
            {
                List<string> files = new List<string>();
                foreach (object item in lstSourceFiles.Items)
                {
                    files.Add(item.ToString());
                }
                return files.ToArray();
            }
            set
            {
                lstSourceFiles.Items.Clear();
                if (value != null)
                {
                    foreach (string str in value)
                    {
                        if (!string.IsNullOrEmpty(str))
                            lstSourceFiles.Items.Add(str);
                    }
                }
                RaiseModified();
            }
        }

        public event EventHandler Modified;

        private void RaiseModified()
        {
            var handler = this.Modified;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string f in openFileDlg.FileNames)
                {
                    if (!string.IsNullOrEmpty(f))
                        lstSourceFiles.Items.Add(f);
                }
                RaiseModified();
            }
        }

        private void btnRemoveFiles_Click(object sender, EventArgs e)
        {
            List<object> selected = new List<object>();
            foreach (object obj in lstSourceFiles.SelectedItems)
            {
                selected.Add(obj);
            }

            if (selected.Count > 0)
            {
                foreach (object obj in selected)
                {
                    lstSourceFiles.Items.Remove(obj);
                }
                RaiseModified();
            }
        }

        private void lstSourceFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveFiles.Enabled = (lstSourceFiles.SelectedItems.Count > 0);
        }
    }
}
