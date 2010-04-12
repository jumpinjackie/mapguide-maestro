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
    internal enum LoadProcType
    {
        SDF,
        SHP,
        SQLite,
        Other
    }

    public partial class LoadProcedureCtrl : UserControl, IResourceEditorControl
    {
        private ILoadProcedureEditor _childEditor;

        private EditorInterface _ed;
        private string _resourceID;
        private bool _isNew;

        internal LoadProcedureCtrl()
        {
            InitializeComponent();
        }

        public LoadProcedureCtrl(EditorInterface ed)
            : this()
        {
            _ed = ed;
            _isNew = true;
            InitChildEditor();
        }

        public LoadProcedureCtrl(EditorInterface ed, string resourceID)
            : this()
        {
            _ed = ed;
            _resourceID = resourceID;
            _isNew = false;
            InitChildEditor();
        }

        private void InitChildEditor()
        {
            if (_isNew)
            {
                var dlg = new LoadProcedurePicker();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    switch (dlg.LoadProcedureType)
                    {
                        case LoadProcType.SDF:
                            _childEditor = new SdfLoadProcedureCtrl(_ed);
                            break;
                        case LoadProcType.SHP:
                            _childEditor = new ShpLoadProcedureCtrl(_ed);
                            break;
                        default:
                            _childEditor = new DefaultLoadProcedureCtrl(_ed);
                            break;
                    }
                }
                else
                {
                    _childEditor = new DefaultLoadProcedureCtrl(_ed);
                }
            }
            else
            {
                MaestroAPI.LoadProcedure proc = (MaestroAPI.LoadProcedure)_ed.CurrentConnection.GetResource(_resourceID);
                if (proc.Item.GetType() == typeof(MaestroAPI.SdfLoadProcedureType))
                {
                    _childEditor = new SdfLoadProcedureCtrl(_ed, _resourceID);
                }
                else if (proc.Item.GetType() == typeof(MaestroAPI.ShpLoadProcedureType))
                {
                    _childEditor = new ShpLoadProcedureCtrl(_ed, _resourceID);
                }
                else
                {
                    _childEditor = new DefaultLoadProcedureCtrl(_ed, _resourceID);
                }

                _childEditor.Resource = proc;
                UpdateDisplay();
            }

            if (_childEditor != null)
            {
                _childEditor.ResourceModified += (sender, e) => { _ed.HasChanged(); };

                Control c = ((Control)_childEditor);
                c.Dock = DockStyle.Fill;
                childPanel.Controls.Add(c);
                btnLoadResources.Enabled = _childEditor.CanExecute;
            }
        }

        public object Resource
        {
            get
            {
                return _childEditor.Resource;
            }
            set
            {
                _childEditor.Resource = value;
            }
        }

        public void UpdateDisplay()
        {
            _childEditor.UpdateDisplay();
        }

        public string ResourceId
        {
            get
            {
                return _childEditor.ResourceId;
            }
            set
            {
                _childEditor.ResourceId = value;
            }
        }

        public bool Preview()
        {
            return _childEditor.Preview();
        }

        public bool ValidateResource(bool recursive)
        {
            return _childEditor.ValidateResource(recursive);
        }

        public bool Profile()
        {
            return _childEditor.Profile();
        }

        public bool SupportsPreview
        {
            get { return _childEditor.SupportsPreview; }
        }

        public bool SupportsValidate
        {
            get { return _childEditor.SupportsValidate; }
        }

        public bool SupportsProfiling
        {
            get { return _childEditor.SupportsProfiling; }
        }

        public bool Save(string savename)
        {
            return _childEditor.Save(savename);
        }

        private void btnLoadResources_Click(object sender, EventArgs e)
        {
            WaitForOperation dlg = new WaitForOperation();
            dlg.CancelAbortsThread = true;

            Form ownerForm = this.ParentForm;
            WaitForOperation.DoBackgroundWork worker = new WaitForOperation.DoBackgroundWork(ExecuteLoadProcedure);

            try
            {
                dlg.RunOperationAsync(ownerForm, worker, _ed.CurrentConnection, _resourceID);
            }
            catch (CancelException)
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        private object ExecuteLoadProcedure(BackgroundWorker worker, DoWorkEventArgs de, params object[] args)
        {
            MaestroAPI.LengthyOperationProgressCallBack cb = (s, cbArgs) =>
            {
                worker.ReportProgress(cbArgs.Progress, cbArgs.StatusMessage);
            };

            MaestroAPI.ServerConnectionI conn = (MaestroAPI.ServerConnectionI)args[0];
            string lpID = (string)args[1];
            conn.ExecuteLoadProcedure(lpID, true, cb);

            return true;
        }

        private void btnListAffected_Click(object sender, EventArgs e)
        {
            new AffectedResourceIdsDlg(_childEditor.GetAffectedResourceIds()).ShowDialog();
        }
    }
}
