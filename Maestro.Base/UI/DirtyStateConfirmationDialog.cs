#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors;
using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using System.Collections;
using Maestro.Editors.Diff;
using Maestro.Shared.UI;
using System.IO;
using Maestro.Base.Util;

namespace Maestro.Base.UI
{
    internal partial class DirtyStateConfirmationDialog : Form
    {
        private DirtyStateConfirmationDialog()
        {
            InitializeComponent();
        }

        private IEditorService _edSvc;

        public DirtyStateConfirmationDialog(IEditorService edSvc)
            : this()
        {
            _edSvc = edSvc;
            lblConfirm.Text = string.Format(lblConfirm.Text, _edSvc.ResourceID);
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void btnDiff_Click(object sender, EventArgs e)
        {
            TextFileDiffList sLF = null;
            TextFileDiffList dLF = null;
            string sourceFile = null;
            string targetFile = null;
            try
            {
                _edSvc.SyncSessionCopy();
                XmlCompareUtil.PrepareForComparison(_edSvc.ResourceService,
                                                    _edSvc.ResourceID,
                                                    _edSvc.EditedResourceID,
                                                    out sourceFile,
                                                    out targetFile);
                sLF = new TextFileDiffList(sourceFile);
                dLF = new TextFileDiffList(targetFile);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
                return;
            }
            finally
            {
                try { File.Delete(sourceFile); }
                catch { }
                try { File.Delete(targetFile); }
                catch { }
            }

            try
            {
                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(sLF, dLF, DiffEngineLevel.SlowPerfect);

                var rep = de.DiffReport();
                TextDiffView dlg = new TextDiffView(sLF, dLF, rep, time);
                dlg.Text += " - " + _edSvc.ResourceID; //NOXLATE
                dlg.ShowDialog();
                dlg.Dispose();
            }
            catch (Exception ex)
            {
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace);
                MessageBox.Show(tmp, Strings.CompareError);
                return;
            }
        }
    }
}
