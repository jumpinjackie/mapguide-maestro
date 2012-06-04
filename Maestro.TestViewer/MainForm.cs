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
using Maestro.Editors.Common;
using OSGeo.MapGuide.ExtendedObjectModels;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Editors.Generic;
using System.Diagnostics;

namespace Maestro.TestViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

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

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openMapDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourceTypes.MapDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadMap(picker.ResourceID);
                }
            }
        }

        private void LoadMap(string mdfId)
        {
            IMapDefinition mdf = (IMapDefinition)_conn.ResourceService.GetResource(mdfId);
            IMappingService mapSvc = (IMappingService)_conn.GetService((int)ServiceType.Mapping);

            var rtMap = mapSvc.CreateMap(mdf);
            mapViewer1.LoadMap(rtMap);
            saveBackToMapDefinitionToolStripMenuItem.Enabled = true;
        }

        private void legend1_DragDrop(object sender, DragEventArgs e)
        {
            Trace.TraceInformation("Legend: DragDrop");
        }

        private void legend1_DragEnter(object sender, DragEventArgs e)
        {
            Trace.TraceInformation("Legend: DragEnter");
        }

        private void legend1_DragLeave(object sender, EventArgs e)
        {
            Trace.TraceInformation("Legend: DragLeave");
        }

        private void legend1_DragOver(object sender, DragEventArgs e)
        {
            Trace.TraceInformation("Legend: DragOver");
            e.Effect = DragDropEffects.Move;
        }

        private void saveBackToMapDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rtMap = mapViewer1.GetMap();
            if (rtMap == null)
            {
                MessageBox.Show("No map loaded");
                return;
            }

            var mdf = rtMap.ToMapDefinition(true);
            var resSvc = mdf.CurrentConnection.ResourceService;
            using (var resPicker = new ResourcePicker(resSvc, ResourceTypes.MapDefinition, ResourcePickerMode.SaveResource))
            {
                if (resPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mdf.ResourceID = resPicker.ResourceID;
                    resSvc.SaveResource(mdf);
                    MessageBox.Show("Map saved to: " + resPicker.ResourceID);
                }
            }
        }
    }
}
