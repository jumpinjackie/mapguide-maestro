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
using Maestro.Shared.UI;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.LayerDefinition.Vector
{
    [ToolboxItem(true)]
    internal partial class VectorLayerSettingsSectionCtrl : EditorBindableCollapsiblePanel
    {
        public VectorLayerSettingsSectionCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edsvc;

        private IVectorLayerDefinition _vl;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);

            var res = service.GetEditedResource() as ILayerDefinition;
            Debug.Assert(res != null);

            _vl = res.SubLayer as IVectorLayerDefinition;
            Debug.Assert(_vl != null);

            TextBoxBinder.BindText(txtFeatureSource, _vl, "ResourceId");

            TextBoxBinder.BindText(txtFeatureClass, _vl, "FeatureName");
            TextBoxBinder.BindText(txtGeometry, _vl, "Geometry");
            TextBoxBinder.BindText(txtFilter, _vl, "Filter");
            TextBoxBinder.BindText(txtHyperlink, _vl, "Url");
            TextBoxBinder.BindText(txtTooltip, _vl, "ToolTip");

            //This is not the root object so no change listeners have been subscribed
            _vl.PropertyChanged += OnVectorLayerPropertyChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            //Init cached schemas and selected class
            if (!string.IsNullOrEmpty(txtFeatureClass.Text))
            {
                if (_cachedDesc == null)
                    _cachedDesc = _edsvc.FeatureService.DescribeFeatureSource(txtFeatureSource.Text);

                var cls = _cachedDesc.GetClass(txtFeatureClass.Text);
                if (cls != null)
                {
                    _selectedClass = cls;
                    OnFeatureClassChanged();
                }
            }
        }

        void OnVectorLayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnResourceChanged();
        }

        protected override void UnsubscribeEventHandlers()
        {
            if (_vl != null)
            {
                _vl.PropertyChanged -= OnVectorLayerPropertyChanged;
            }
            base.UnsubscribeEventHandlers();
        }

        private FeatureSourceDescription _cachedDesc;

        private void txtFeatureSource_TextChanged(object sender, EventArgs e)
        {
            _cachedDesc = _edsvc.FeatureService.DescribeFeatureSource(txtFeatureSource.Text);
        }

        internal event EventHandler FeatureClassChanged;

        private void OnFeatureClassChanged()
        {
            var handler = this.FeatureClassChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void btnBrowseFeatureSource_Click(object sender, EventArgs e)
        {
            var picker = new ResourcePicker(_edsvc.ResourceService, ResourceTypes.FeatureSource, ResourcePickerMode.OpenResource);
            if (picker.ShowDialog() == DialogResult.OK)
            {
                txtFeatureSource.Text = picker.ResourceID;
                //Invalidate
                _cachedFs = null;
            }
        }

        internal string FeatureSourceID
        {
            get { return txtFeatureSource.Text; }
        }

        private ClassDefinition _selectedClass;

        internal ClassDefinition GetSelectedClass()
        {
            return _selectedClass;
        }

        private IFeatureSource _cachedFs;

        private IFeatureSource GetFeatureSource()
        {
            if (_cachedFs == null)
                _cachedFs = (IFeatureSource)_edsvc.ResourceService.GetResource(txtFeatureSource.Text);

            return _cachedFs;
        }

        private void btnEditFilter_Click(object sender, EventArgs e)
        {
            var cls = GetSelectedClass();
            if (cls != null)
            {
                var fs = GetFeatureSource();
                var expr = _edsvc.EditExpression(txtFilter.Text, cls, fs.Provider, fs.ResourceID);
                if (expr != null)
                {
                    txtFilter.Text = expr;
                }
            }
        }

        private void btnEditHyperlink_Click(object sender, EventArgs e)
        {
            var cls = GetSelectedClass();
            if (cls != null)
            {
                var fs = GetFeatureSource();
                var expr = _edsvc.EditExpression(txtHyperlink.Text, cls, fs.Provider, fs.ResourceID);
                if (expr != null)
                {
                    txtHyperlink.Text = expr;
                }
            }
        }

        private void btnEditTooltip_Click(object sender, EventArgs e)
        {
            var cls = GetSelectedClass();
            if (cls != null)
            {
                var fs = GetFeatureSource();
                var expr = _edsvc.EditExpression(txtTooltip.Text, cls, fs.Provider, fs.ResourceID);
                if (expr != null)
                {
                    txtTooltip.Text = expr;
                }
            }
        }

        private void btnBrowseSchema_Click(object sender, EventArgs e)
        {
            var list = new List<ClassDefinition>(_cachedDesc.AllClasses).ToArray();
            var item = GenericItemSelectionDialog.SelectItem(null, null, list, "QualifiedName", "QualifiedName");
            if (item != null)
            {
                txtFeatureClass.Text = item.QualifiedName;
                _selectedClass = item;
                
                //See if geometry needs invalidation
                bool invalidate = true;
                foreach (var col in item.Properties)
                {
                    if (col.Type == PropertyDefinitionType.Geometry && col.Name.Equals(txtGeometry.Text))
                    {
                        invalidate = false;
                        break;
                    }
                }
                if (invalidate)
                {
                    txtGeometry.Text = string.Empty;
                }

                //See if we can auto-assign geometry
                List<PropertyDefinition> geoms = new List<PropertyDefinition>();
                foreach (var col in _selectedClass.Properties)
                {
                    if (col.Type == PropertyDefinitionType.Geometry)
                        geoms.Add(col);
                }

                if (geoms.Count == 1)
                    txtGeometry.Text = geoms[0].Name;

                OnFeatureClassChanged();
            }
        }

        private void btnBrowseGeometry_Click(object sender, EventArgs e)
        {
            if (_selectedClass != null)
            {
                List<PropertyDefinition> geoms = new List<PropertyDefinition>();
                foreach (var col in _selectedClass.Properties)
                {
                    if (col.Type == PropertyDefinitionType.Geometry)
                        geoms.Add(col);
                }

                var item = GenericItemSelectionDialog.SelectItem(null, null, geoms.ToArray(), "Name", "Name");
                if (item != null)
                {
                    txtGeometry.Text = item.Name;
                }
            }
        }
    }
}
