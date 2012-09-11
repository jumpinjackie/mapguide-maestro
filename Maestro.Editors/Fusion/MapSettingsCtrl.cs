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
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Editors.Fusion
{
    [ToolboxItem(false)]
    //[ToolboxItem(true)]
    internal partial class MapSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public MapSettingsCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edsvc;
        private IFusionService _fsvc;
        private IApplicationDefinition _flexLayout;
        private string _baseUrl;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            try
            {
                _fsvc = (IFusionService)service.GetService((int)ServiceType.Fusion);
                _baseUrl = service.GetCustomProperty("BaseUrl").ToString(); //NOXLATE

                if (!_baseUrl.EndsWith("/")) //NOXLATE
                    _baseUrl += "/"; //NOXLATE
            }
            catch
            {
                throw new NotSupportedException(Strings.IncompatibleConnection);
            }
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);
            _flexLayout = (IApplicationDefinition)service.GetEditedResource();

            foreach (var grp in _flexLayout.MapSet.MapGroups)
            {
                AddMap(grp);
            }
        }

        private void AddMap(IMapGroup group)
        {
            var item = new ListViewItem();
            item.ImageIndex = 0;
            item.Text = item.Name = group.id;
            item.Tag = group;
            group.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "id") //NOXLATE
                    item.Text = group.id;
            };
            lstMaps.Items.Add(item);
        }

        internal IMapGroup SelectedGroup
        {
            get
            {
                if (lstMaps.SelectedItems.Count == 1)
                {
                    var item = lstMaps.SelectedItems[0];
                    var group = (IMapGroup)item.Tag;

                    return group;
                }
                return null;
            }
        }

        private void lstMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveMap.Enabled = false;
            var grp = this.SelectedGroup;
            if (grp != null)
            {
                propertiesPanel.Controls.Clear();
                IMapWidget widget = null;

                var mapWidget = _flexLayout.GetFirstWidgetSet().MapWidget;
                //We're editing the one used by the map widget
                if (grp.id == mapWidget.MapId)
                    widget = mapWidget;

                var mapCtrl = new MapCtrl(_flexLayout, grp, _edsvc, widget);
                mapCtrl.Dock = DockStyle.Fill;
                propertiesPanel.Controls.Add(mapCtrl);
                btnRemoveMap.Enabled = true;
            }
        }

        private void btnRemoveMap_Click(object sender, EventArgs e)
        {
            if (lstMaps.SelectedItems.Count == 1)
            {
                var item = lstMaps.SelectedItems[0];
                var group = (IMapGroup)item.Tag;

                var mapWidget = _flexLayout.GetFirstWidgetSet().MapWidget;
                //The map group we removed is being referenced by the map widget
                if (group.id == mapWidget.MapId)
                {
                    if (_flexLayout.MapSet.MapGroupCount >= 2)
                    {
                        if (_flexLayout.MapSet.MapGroupCount == 2)
                        {
                            _flexLayout.MapSet.RemoveGroup(group);
                            lstMaps.Items.Remove(item);

                            mapWidget.MapId = _flexLayout.MapSet.GetGroupAt(0).id;
                            MessageBox.Show(string.Format(Strings.MapUpdatedToUseGroup, mapWidget.MapId));
                            OnResourceChanged();
                        }
                        else if (_flexLayout.MapSet.MapGroupCount > 2)
                        {
                            List<string> mapGroupIds = new List<string>();
                            for (int i = 0; i < _flexLayout.MapSet.MapGroupCount; i++)
                            {
                                mapGroupIds.Add(_flexLayout.MapSet.GetGroupAt(i).id);
                            }

                            mapGroupIds.Remove(group.id); //Remove the one to be removed from the list

                            string id = GenericItemSelectionDialog.SelectItem(Strings.PromptSelectMap, Strings.PromptUpdateMapWidgetReference, mapGroupIds.ToArray());
                            if (id != null) //A replacement has been selected, now we can remove
                            {
                                _flexLayout.MapSet.RemoveGroup(group);
                                lstMaps.Items.Remove(item);

                                mapWidget.MapId = id;

                                OnResourceChanged();
                            }
                        }
                    }
                }
                else
                {
                    _flexLayout.MapSet.RemoveGroup(group);
                    lstMaps.Items.Remove(item);
                    OnResourceChanged();
                }
            }
        }

        private void btnAddMap_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edsvc.ResourceService, ResourceTypes.MapDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    string resId = picker.ResourceID;
                    AddMapDefinition(resId, ResourceIdentifier.GetName(resId));
                }
            }
        }

        private void AddMapDefinition(string resId, string prefix)
        {
            int counter = 0;
            string name = prefix;

            while (_flexLayout.MapSet.GetGroupById(name) != null)
            {
                counter++;
                name = prefix + counter;
            }

            var grp = _flexLayout.AddMapGroup(name, true, resId);
            OnResourceChanged();
            AddMap(grp);
        }

        private void lstMaps_DragEnter(object sender, DragEventArgs e)
        {
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids == null || rids.Length == 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Copy;
        }

        private void lstMaps_DragOver(object sender, DragEventArgs e)
        {
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids == null || rids.Length == 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Copy;
        }

        private void lstMaps_DragDrop(object sender, DragEventArgs e)
        {
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids != null && rids.Length > 0)
            {
                //Only map definition resources apply

                foreach (var r in rids)
                {
                    if (r.ResourceType != ResourceTypes.MapDefinition)
                        continue;

                    AddMapDefinition(r.ToString(), r.Name);
                }
            }
        }
    }
}
