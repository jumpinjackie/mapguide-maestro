using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.Editors.Common;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.LayerDefinition
{
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class LayerPropertiesSectionCtrl : EditorBindableCollapsiblePanel
    {
        public LayerPropertiesSectionCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edsvc;
        private IList<INameStringPair> _props;

        private ILayerDefinition _parent;
        private IVectorLayerDefinition _vl;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);

            _parent = service.GetEditedResource() as ILayerDefinition;
            Debug.Assert(_parent != null);

            _vl = _parent.SubLayer as IVectorLayerDefinition;
            Debug.Assert(_vl != null);

            if (service.IsNew)
            {
                //Let's try to auto-assign a feature class
                string[] classNames = _edsvc.FeatureService.GetClassNames(_vl.ResourceId, null);
                if (classNames.Length == 1) //Only one class in this Feature Source
                {
                    var clsDef = _edsvc.FeatureService.GetClassDefinition(_vl.ResourceId, classNames[0]);
                    if (!string.IsNullOrEmpty(clsDef.DefaultGeometryPropertyName)) //It has a default geometry
                    {
                        _vl.FeatureName = classNames[0];
                        _vl.Geometry = clsDef.DefaultGeometryPropertyName;
                    }
                }
            }

            _props = new List<INameStringPair>(_vl.PropertyMapping);
            //Modifying the visibility constitutes a change in the resource
            //_props.ListChanged += OnPropertyListChanged;
            PopulatePropertyList();
            _vl.PropertyChanged += OnVectorLayerPropertyChanged;
        }

        protected override void UnsubscribeEventHandlers()
        {
            //if (_props != null)
            //    _props.ListChanged -= OnPropertyListChanged;

            if (_vl != null)
                _vl.PropertyChanged -= OnVectorLayerPropertyChanged;

            base.UnsubscribeEventHandlers();
        }

        //void OnPropertyListChanged(object sender, ListChangedEventArgs e)
        //{
        //    OnResourceChanged();
        //}

        void OnVectorLayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Feature Source changed 
            if (e.PropertyName == "ResourceId") //NOXLATE
            {
                _vl.RemoveAllScaleRanges();
                PopulatePropertyList();
            }
        }

        internal void PopulatePropertyList()
        {
            //NOTE: This is being called before _vl is assigned in mono
            //so guard against it
            if (_vl == null || string.IsNullOrEmpty(_vl.FeatureName))
                return;

            if (_edsvc.ResourceService.ResourceExists(_vl.ResourceId))
            {
                var cls = _edsvc.FeatureService.GetClassDefinition(_vl.ResourceId, _vl.FeatureName);
                if (cls != null)
                {
                    grdProperties.Rows.Clear();
                    RemoveInvalidMappings(cls);
                    foreach (var col in cls.Properties)
                    {
                        if (col.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Data)
                        {
                            bool visible = false;
                            string disp = col.Name;
                            foreach (var item in _vl.PropertyMapping)
                            {
                                if (item.Name == col.Name)
                                {
                                    visible = true;
                                    disp = item.Value;
                                }
                            }
                            grdProperties.Rows.Add(visible, col.Name, disp);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(string.Format(Strings.PromptRepairBrokenFeatureSource, _vl.ResourceId));
                using (var picker = new ResourcePicker(_edsvc.ResourceService, ResourceTypes.FeatureSource, ResourcePickerMode.OpenResource))
                {
                    if (picker.ShowDialog() == DialogResult.OK)
                    {
                        _vl.ResourceId = picker.ResourceID;
                        OnResourceChanged();
                    }
                }
            }
        }

        private void RemoveInvalidMappings(OSGeo.MapGuide.MaestroAPI.Schema.ClassDefinition cls)
        {
            var remove = new List<INameStringPair>();
            foreach (var mp in _vl.PropertyMapping)
            {
                if (cls.FindProperty(mp.Name) == null)
                    remove.Add(mp);
            }

            foreach (var mp in remove)
            {
                _vl.RemovePropertyMapping(mp);
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdProperties.Rows)
            {
                if (!(bool)row.Cells[0].Value)
                {
                    row.Cells[0].Value = true;
                    OnPropertyMappingChanged((bool)row.Cells[0].Value, row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
                }
            }
            grdProperties.Refresh();
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdProperties.Rows)
            {
                if ((bool)row.Cells[0].Value)
                {
                    row.Cells[0].Value = false;
                    OnPropertyMappingChanged((bool)row.Cells[0].Value, row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
                }
            }
            grdProperties.Refresh();
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdProperties.Rows)
            {
                row.Cells[0].Value = !((bool)row.Cells[0].Value); //Negate
                OnPropertyMappingChanged((bool)row.Cells[0].Value, row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
            }
            grdProperties.Refresh();
        }

        private void OnPropertyMappingChanged(bool state, string propertyName, string displayName)
        {
            INameStringPair nsp = null;
            foreach (var item in _props)
            {
                if (item.Name == propertyName)
                {
                    nsp = item;
                    break;
                }
            }

            if (state)
            {
                if (nsp == null)
                {
                    var pair = _parent.CreatePair(propertyName, displayName);
                    _props.Add(pair);
                    _vl.AddPropertyMapping(pair);
                }
                else
                {
                    nsp.Value = displayName;
                }
            }
            else //Turned off
            {
                if (nsp != null)
                {
                    _props.Remove(nsp);
                    _vl.RemovePropertyMapping(nsp);
                }
            }
            OnResourceChanged();
        }

        private void grdProperties_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = grdProperties.Rows[e.RowIndex];
                if (e.ColumnIndex == 0 || e.ColumnIndex == 2) //checkbox
                {
                    OnPropertyMappingChanged((bool)row.Cells[0].Value, row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
                }
            }
        }

        private static DataGridViewRow CloneRow(DataGridViewRow row)
        {
            var clone = (DataGridViewRow)row.Clone();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                clone.Cells[i].Value = row.Cells[i].Value;
            }
            return clone;
        }

        static bool IsMapped(DataGridViewRow row)
        {
            return row.Cells[0].Value != null && Convert.ToBoolean(row.Cells[0].Value);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (grdProperties.SelectedRows.Count == 1)
            {
                var row = grdProperties.SelectedRows[0];
                var rowIdx = row.Index;
                var idx = rowIdx - 1;
                if (idx >= 0)
                {
                    grdProperties.ClearSelection();
                    var swap = grdProperties.Rows[idx];
                    grdProperties.Rows.RemoveAt(rowIdx);
                    grdProperties.Rows.RemoveAt(idx);
                    
                    var rowClone = CloneRow(row);
                    var swapClone = CloneRow(swap);
                    grdProperties.Rows.Insert(idx, rowClone);
                    grdProperties.Rows.Insert(rowIdx, swapClone);

                    if (IsMapped(row) && IsMapped(swapClone))
                    {
                        var mp = _vl.GetPropertyMapping(row.Cells[1].Value.ToString());
                        _vl.MoveUp(mp);
                    }

                    rowClone.Selected = true;
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (grdProperties.SelectedRows.Count == 1)
            {
                var row = grdProperties.SelectedRows[0];
                var rowIdx = row.Index;
                var idx = rowIdx + 1;
                if (idx < grdProperties.Rows.Count - 1)
                {
                    grdProperties.ClearSelection();
                    var swap = grdProperties.Rows[idx];
                    grdProperties.Rows.RemoveAt(idx);
                    grdProperties.Rows.RemoveAt(rowIdx);

                    var rowClone = CloneRow(row);
                    var swapClone = CloneRow(swap);
                    grdProperties.Rows.Insert(rowIdx, swapClone);
                    grdProperties.Rows.Insert(idx, rowClone);

                    if (IsMapped(row) && IsMapped(swapClone))
                    {
                        var mp = _vl.GetPropertyMapping(row.Cells[1].Value.ToString());
                        _vl.MoveDown(mp);
                    }

                    rowClone.Selected = true;
                }
            }
        }

        private void grdProperties_SelectionChanged(object sender, EventArgs e)
        {
            btnMoveDown.Enabled = btnMoveUp.Enabled = false;
            if (grdProperties.SelectedRows.Count == 1)
            {
                if (grdProperties.SelectedRows[0].Index > 0)
                    btnMoveUp.Enabled = true;

                if (grdProperties.SelectedRows[0].Index < grdProperties.Rows.Count - 1)
                    btnMoveDown.Enabled = true;
            }
        }
    }
}
