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

namespace Maestro.Editors.LayerDefinition
{
    [ToolboxItem(true)]
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
            if (e.PropertyName == "ResourceId") //Feature Source changed
            {
                _vl.RemoveAllScaleRanges();
                PopulatePropertyList();
            }
        }

        internal void PopulatePropertyList()
        {
            if (string.IsNullOrEmpty(_vl.FeatureName))
                return;

            //TODO: Should just fetch the class definition
            var desc = _edsvc.FeatureService.DescribeFeatureSource(_vl.ResourceId);
            var cls = desc.GetClass(_vl.FeatureName);
            if (cls != null)
            {
                grdProperties.Rows.Clear();
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
            }
            else //Turned off
            {
                if (nsp != null)
                {
                    _props.Remove(nsp);
                    _vl.RemovePropertyMapping(nsp);
                }
            }
        }

        private void grdProperties_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = grdProperties.Rows[e.RowIndex];
                if (e.ColumnIndex == 0) //checkbox
                {
                    OnPropertyMappingChanged((bool)row.Cells[0].Value, row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
                }
            }
        }
    }
}
