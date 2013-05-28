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
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using Maestro.Editors.Generic;
using Maestro.Editors.MapDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.Common;

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
#if DEBUG
            debugToolStripMenuItem.Visible = true;
#endif
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
            var diag = new MapSettingsDialog(_conn, mdf, null);
            //if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Start off in the session, so the editor service knows this is a new resource
                mdf.ResourceID = "Session:" + _conn.SessionID + "//NewMap.MapDefinition";
                _conn.ResourceService.SaveResource(mdf);
                LoadMapDefinitionForEditing(mdf);
            }
            EvaluateCommandStates();
        }

        private bool _bComputeLayerCsAndExtentOnFirstLayerAdded = false;

        private void LoadMapDefinitionForEditing(IMapDefinition mdf)
        {
            CleanupExistingMap();

            if (mdf.BaseMap != null)
            {
                if (mdf.BaseMap.GroupCount > 0)
                {
                    MessageBox.Show(Strings.TiledMapNote, Strings.TitleTiledMap);
                }
            }

            _mapEditor = new LiveMapDefinitionEditorCtrl();
            _mapEditor.Bind(new ResourceEditorService(mdf.ResourceID, _conn));
            _bComputeLayerCsAndExtentOnFirstLayerAdded = _mapEditor.EditorService.IsNew;
            _mapEditor.Map.LayerAdded += OnMapLayerAdded;
            _mapEditor.Dock = DockStyle.Fill;
            rootPanel.Controls.Add(_mapEditor);
        }

        private static bool SupportsMutableMapProperties(RuntimeMap runtimeMap)
        {
            return runtimeMap.SupportsMutableBackgroundColor &&
                   runtimeMap.SupportsMutableCoordinateSystem &&
                   runtimeMap.SupportsMutableExtents &&
                   runtimeMap.SupportsMutableMetersPerUnit;
        }

        void OnMapLayerAdded(object sender, RuntimeMapLayer layer)
        {
            if (_bComputeLayerCsAndExtentOnFirstLayerAdded && _mapEditor.Map.Layers.Count == 1)
            {
                Debug.WriteLine("Computing map extents and CS based on first layer added");
                try
                {
                    ILayerDefinition layerDef = (ILayerDefinition)_conn.ResourceService.GetResource(layer.LayerDefinitionID);
                    string wkt;
                    IEnvelope env = layerDef.GetSpatialExtent(true, out wkt);
                    if (SupportsMutableMapProperties(_mapEditor.Map))
                    {
                        _mapEditor.Map.MapExtent = env;
                        _mapEditor.Map.CoordinateSystem = wkt;
                        if (CsHelper.DefaultCalculator != null)
                        {
                            _mapEditor.Map.MetersPerUnit = CsHelper.DefaultCalculator.Calculate(wkt, 1.0);
                        }
                        else
                        {
                            var calc = _mapEditor.Map.CurrentConnection.GetCalculator();
                            _mapEditor.Map.MetersPerUnit = calc.Calculate(wkt, 1.0);
                        }
                        _mapEditor.ReloadViewer();
                    }
                    else
                    {
                        //We have to tear down the current runtime map, update the shadow copy
                        //map definition and then rebuild a new runtime map
                        _mapEditor.SyncMap();
                        IMapDefinition mdf = _mapEditor.GetMapDefinition();
                        mdf.Extents = env;
                        mdf.CoordinateSystem = wkt;
                        CleanupExistingMap();
                        //If local, we'd be rebuilding off of the resource ID and not its in-memory
                        //object representation so flush
                        _mapEditor.EditorService.SyncSessionCopy();
                        _mapEditor.RebuildRuntimeMap();
                        _mapEditor.ReloadViewer();
                    }
                    Debug.WriteLine("Computed map extents and CS");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Uh-Oh: " + ex.ToString());
                }
            }
        }

        private void CleanupExistingMap()
        {
            if (_mapEditor != null)
            {
                if (_mapEditor.Map != null)
                {
                    _mapEditor.Map.LayerAdded -= OnMapLayerAdded;
                }
            }
        }

        void OnMapCoordSysAndExtentsChangedFromFirstLayer(object sender, EventArgs e)
        {
            _mapEditor.ReloadViewer();
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
            var diag = new MapSettingsDialog(_conn, _mapEditor.GetMapDefinition(), _mapEditor.Viewer);
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CleanupExistingMap();
                _mapEditor.EditorService.SyncSessionCopy();
                _mapEditor.RebuildRuntimeMap();
                _bComputeLayerCsAndExtentOnFirstLayerAdded = _mapEditor.EditorService.IsNew;
                _mapEditor.Map.LayerAdded += OnMapLayerAdded;
                _mapEditor.ReloadViewer();
            }
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
