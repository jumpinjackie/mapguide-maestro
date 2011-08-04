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
using System.IO;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ExtendedObjectModels;

namespace DwfInspector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;
        private IDrawingService _dwSvc;

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

                //Connections carry a capability property that tells you what is and isn't supported. 
                //Here we need to check if this connection supports the IDrawingService interface. If
                //it doesn't we can't continue.
                if (Array.IndexOf(_conn.Capabilities.SupportedServices, (int)ServiceType.Drawing) < 0)
                {
                    MessageBox.Show("This particular connection does not support the Drawing Service API");
                    Application.Exit();
                    return;
                }

                //For any non-standard service interface, we call GetService() passing in the service type and casting
                //it to the required service interface.
                _dwSvc = (IDrawingService)_conn.GetService((int)ServiceType.Drawing);
            }
            else //This sample does not work without an IServerConnection
            {
                Application.Exit();
            }
        }

        private void rdDrawingSource_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowseDs.Enabled = btnBrowseDwf.Enabled = btnUpload.Enabled = false;
            if (rdDrawingSource.Checked)
                btnBrowseDs.Enabled = true;
            else if (rdDwfFile.Checked)
                btnBrowseDwf.Enabled = btnUpload.Enabled = true;
        }

        private void btnBrowseDs_Click(object sender, EventArgs e)
        {
            //The ResourcePicker class, functions like a file dialog allowing the user
            //to easily select a given resource. In our case, we want the user to select
            //a Drawing Source
            using (var picker = new ResourcePicker(_conn.ResourceService,
                                                   ResourceTypes.DrawingSource,
                                                   ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtDrawingSource.Text = picker.ResourceID;
                }
            }
        }

        private void btnBrowseDwf_Click(object sender, EventArgs e)
        {
            using (var open = new OpenFileDialog())
            {
                open.Filter = "DWF Files (*.dwf)|*.dwf";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    txtDwfPath.Text = open.FileName;
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtDwfPath.Text))
            {
                MessageBox.Show("File does not exist");
                return;
            }

            // Create a MapGuide Drawing Source and store it in the session repository
            // The DWF file we upload must be the same name as the SourceName property
            // in the DrawingSource and the resource data name that we are uploading as

            IDrawingSource ds = ObjectFactory.CreateDrawingSource(_conn);
            ds.SourceName = Path.GetFileName(txtDwfPath.Text);

            string resId = "Session:" + _conn.SessionID + "//InspectedDwf.DrawingSource";
            _conn.ResourceService.SaveResourceAs(ds, resId);
            _conn.ResourceService.SetResourceData(resId, ds.SourceName, OSGeo.MapGuide.ObjectModels.Common.ResourceDataType.File, File.OpenRead(txtDwfPath.Text));

            txtDrawingSource.Text = resId;
            rdDrawingSource.Checked = true;
        }

        private void btnInspect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDrawingSource.Text) || !_conn.ResourceService.ResourceExists(txtDrawingSource.Text))
            {
                MessageBox.Show("Could not find the specified drawing source. Please select a valid drawing source or upload a DWF file");
                return;
            }

            //Call the EnumerateDrawingSections API of Drawing Service
            var sections = _dwSvc.EnumerateDrawingSections(txtDrawingSource.Text);

            //Bind the Section property which is a BindingList, to the sections ListBox
            lstSections.DataSource = sections.Section;
        }

        private void lstSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            var section = lstSections.SelectedItem as OSGeo.MapGuide.ObjectModels.Common.DrawingSectionListSection;
            if (section != null)
            {
                //Enumerate the drawing layers of this section and bind the result to the layers ListBox
                var layers = _dwSvc.EnumerateDrawingLayers(txtDrawingSource.Text, section.Name);
                lstLayers.DataSource = layers;

                //Load the section info control, which will show more information about this section
                var ctrl = new SectionInfoCtrl(_dwSvc, txtDrawingSource.Text, section);
                ctrl.Dock = DockStyle.Fill;
                grpSectionDetails.Controls.Clear();
                grpSectionDetails.Controls.Add(ctrl);
            }
            else
            {
                lstLayers.DataSource = null;
                grpSectionDetails.Controls.Clear();
            }
        }
    }
}
