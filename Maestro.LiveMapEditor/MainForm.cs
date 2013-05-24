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
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using Maestro.Editors.MapDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.MapDefinition;

namespace Maestro.LiveMapEditor
{
    public partial class MainForm : Form
    {
        private IServerConnection _conn;

        private string _origTitle;

        public MainForm(IServerConnection conn)
        {
            InitializeComponent();
            _conn = conn;
            _origTitle = this.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoNewMap();
        }

        private void openMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoOpen();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSaveAs();
        }

        private LiveMapDefinitionEditorCtrl _mapEditor;

        private void DoNewMap()
        {
            ClearExistingEditor();

            var mdf = ObjectFactory.CreateMapDefinition(_conn, Strings.NewMap);
            var diag = new MapSettingsDialog(_conn, mdf);
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Start off in the session, so the editor service knows this is a new resource
                mdf.ResourceID = "Session:" + _conn.SessionID + "//NewMap.MapDefinition";
                _conn.ResourceService.SaveResource(mdf);
                LoadMapDefinitionForEditing(mdf);
            }
            EvaluateCommandStates();
        }

        private void LoadMapDefinitionForEditing(IMapDefinition mdf)
        {
            if (mdf.BaseMap != null)
            {
                if (mdf.BaseMap.GroupCount > 0)
                {
                    MessageBox.Show(Strings.TiledMapNote, Strings.TitleTiledMap);
                }
            }

            _mapEditor = new LiveMapDefinitionEditorCtrl();
            _mapEditor.Bind(new ResourceEditorService(mdf.ResourceID, _conn));
            _mapEditor.Dock = DockStyle.Fill;
            rootPanel.Controls.Add(_mapEditor);
        }

        private void DoOpen()
        {
            ClearExistingEditor();

            using (var picker = new ResourcePicker(_conn.ResourceService, ResourceTypes.MapDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var mdf = (IMapDefinition)_conn.ResourceService.GetResource(picker.ResourceID);
                    LoadMapDefinitionForEditing(mdf);
                }
            }
            EvaluateCommandStates();
        }

        private void DoSave()
        {
            if (_mapEditor.EditorService.IsNew)
            {
                DoSaveAs();
            }
            else
            {
                _mapEditor.SyncMap();                       //RuntimeMap to IMapDefinition
                _mapEditor.EditorService.SyncSessionCopy(); //IMapDefinition to session-copy
                _mapEditor.EditorService.Save();            //Session-copy to original resource
                EvaluateCommandStates();
            }
        }

        private void DoSaveAs()
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourceTypes.MapDefinition, ResourcePickerMode.SaveResource))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _mapEditor.SyncMap();                               //RuntimeMap to IMapDefinition
                    _mapEditor.EditorService.SyncSessionCopy();         //IMapDefinition to session-copy
                    _mapEditor.EditorService.SaveAs(picker.ResourceID); //Session-copy to specified resource
                }
            }
            EvaluateCommandStates();
        }

        private void EvaluateCommandStates()
        {
            btnMapProperties.Enabled = btnSaveMap.Enabled = btnSaveMapAs.Enabled = saveAsToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = (_mapEditor != null);
            UpdateTitle();
        }

        private void ClearExistingEditor()
        {
            if (_mapEditor != null)
            {
                if (MessageBox.Show(Strings.ConfirmNewMap, Strings.CaptionNewMap, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    DoSave();
                }
                rootPanel.Controls.Remove(_mapEditor);
                _mapEditor = null;
            }
        }

        private void UpdateTitle()
        {
            if (_mapEditor == null)
                this.Text = _origTitle;
            else
                this.Text = _origTitle + " - " + (_mapEditor.EditorService.IsNew ? Strings.CaptionNewMap : _mapEditor.EditorService.ResourceID);
        }

        private void btnMapProperties_Click(object sender, EventArgs e)
        {
            if (_mapEditor == null)
                return;

            _mapEditor.SyncMap();
            var diag = new MapSettingsDialog(_conn, _mapEditor.GetMapDefinition());
            diag.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        private void runtimeMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new Form())
            {
                form.Text = "Runtime Map";
                var propGrid = new PropertyGrid();
                propGrid.Dock = DockStyle.Fill;
                form.Controls.Add(propGrid);

                propGrid.SelectedObject = _mapEditor.Map;

                form.ShowDialog();
            }
        }
    }
}
