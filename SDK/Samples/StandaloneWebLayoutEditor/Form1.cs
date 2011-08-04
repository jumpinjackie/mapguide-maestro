#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using Maestro.Editors;
using Maestro.Editors.WebLayout;
using OSGeo.MapGuide.ExtendedObjectModels;

namespace StandaloneWebLayoutEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

        /// <summary>
        /// The IEditorService interface provides all the required services needed by any resource editor
        /// </summary>
        private IEditorService _edSvc;

        protected override void OnLoad(EventArgs e)
        {
            //This call is a one-time only call that will instantly register all known resource 
            //version types and validators. This way you never have to manually reference a 
            //ObjectModels assembly of the desired resource type you want to work with
            ModelSetup.Initialize();

            //Anytime we work with the Maestro API, we require an IServerConnection
            //reference. The Maestro.Login.LoginDialog provides a UI to obtain such a 
            //reference.

            //If you need to obtain an IServerConnection reference programmatically and
            //without user intervention, use the ConnectionProviderRegistry class
            var login = new Maestro.Login.LoginDialog();
            if (login.ShowDialog() == DialogResult.OK)
            {
                _conn = login.Connection;
            }
            else //This sample does not work without an IServerConnection
            {
                Application.Exit();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openWebLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //The ResourcePicker class, functions like a file dialog allowing the user
            //to easily select a given resource. In our case, we want the user to select
            //a Feature Source
            using (var picker = new ResourcePicker(_conn.ResourceService,
                                                   ResourceTypes.WebLayout,
                                                   ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LoadWebLayoutEditor(picker.ResourceID);
                }
            }
        }

        private void LoadWebLayoutEditor(string webLayoutId)
        {
            //Tear down existing editor service if needed
            if (_edSvc != null)
            {
                _edSvc.DirtyStateChanged -= OnDirtyStateChanged;
            }

            _edSvc = new MyResourceEditorService(webLayoutId, _conn);
            _edSvc.DirtyStateChanged += OnDirtyStateChanged;
            //Each resource editor is named in the form: [Resource Type]EditorCtrl
            var ed = new WebLayoutEditorCtrl();
            ed.Dock = DockStyle.Fill;

            panel1.Controls.Clear();
            panel1.Controls.Add(ed);

            //Bind() performs all the setup work. Do this after adding the control to whatever parent container
            ed.Bind(_edSvc);
        }

        private void OnDirtyStateChanged(object sender, EventArgs e)
        {
            //Update title
            if (!this.Text.EndsWith(" *"))
                this.Text += " *";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //The editor has been modify an in-memory model. This needs to be serialized back to
            //the session copy first
            _edSvc.SyncSessionCopy();
            //Call save on the editor service to commit back the changes from the session copy
            //back to the original resource
            _edSvc.Save();
            //Restore title
            if (this.Text.EndsWith(" *"))
                this.Text = this.Text.Substring(0, this.Text.Length - 2);
            MessageBox.Show("Saved");
        }
    }
}
