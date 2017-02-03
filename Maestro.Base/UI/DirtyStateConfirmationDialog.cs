#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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

using Maestro.Editors;
using Maestro.Editors.Diff;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using System;
using System.IO;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    internal partial class DirtyStateConfirmationDialog : Form
    {
        private DirtyStateConfirmationDialog()
        {
            InitializeComponent();
        }

        private readonly IEditorService _edSvc;

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
                var set = XmlCompareUtil.PrepareForComparison(_edSvc.CurrentConnection.ResourceService,
                                                              _edSvc.ResourceID,
                                                              _edSvc.EditedResourceID);
                sLF = set.Source;
                dLF = set.Target;
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
                dlg.SetLabels(_edSvc.ResourceID, Strings.EditedResource);
                dlg.ShowDialog();
                dlg.Dispose();
            }
            catch (Exception ex)
            {
                string nl = Environment.NewLine;
                string tmp = $"{ex.Message}{nl}{nl}***STACK***{nl}{ex.StackTrace}";
                MessageBox.Show(tmp, Strings.CompareError);
                return;
            }
        }
    }
}