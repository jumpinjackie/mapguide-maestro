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
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class VectorLayerSettingsSectionCtrl : EditorBindableCollapsiblePanel
    {
        public VectorLayerSettingsSectionCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edsvc;

        private IVectorLayerDefinition _vl;

        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            try
            {
                _init = true;
                _edsvc = service;
                _edsvc.RegisterCustomNotifier(this);

                var res = service.GetEditedResource() as ILayerDefinition;
                Debug.Assert(res != null);

                _vl = res.SubLayer as IVectorLayerDefinition;
                Debug.Assert(_vl != null);

                TextBoxBinder.BindText(txtFeatureSource, _vl, "ResourceId");

                TextBoxBinder.BindText(txtFeatureClass, _vl, "FeatureName");
                TextBoxBinder.BindText(txtGeometry, _vl, "Geometry");
                //TextBoxBinder.BindText(txtFilter, _vl, "Filter");
                txtFilter.Text = _vl.Filter;

                //Loose bind this one because 2.4 changes this behaviour making it
                //unsuitable for databinding via TextBoxBinder
                txtHyperlink.Text = _vl.Url;

                //TextBoxBinder.BindText(txtTooltip, _vl, "ToolTip");
                txtTooltip.Text = _vl.ToolTip;

                //This is not the root object so no change listeners have been subscribed
                _vl.PropertyChanged += OnVectorLayerPropertyChanged;
            }
            finally
            {
                _init = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.DesignMode)
                return;

            //Init cached schemas and selected class
            if (!string.IsNullOrEmpty(txtFeatureClass.Text))
            {                
                var cls = _edsvc.FeatureService.GetClassDefinition(txtFeatureSource.Text, txtFeatureClass.Text);
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

        private void txtFeatureSource_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFeatureSource.Text))
                return;

            if (!string.IsNullOrEmpty(txtFeatureClass.Text))
            {
                //This feature source must have at least one class definition with a geometry property
                ClassDefinition clsDef = _edsvc.FeatureService.GetClassDefinition(txtFeatureSource.Text, txtFeatureClass.Text);
                if (clsDef == null)
                {
                    MessageBox.Show(string.Format(Strings.InvalidFeatureSourceNoClasses, txtFeatureSource.Text));
                    txtFeatureSource.Text = string.Empty;
                    return;
                }
                SetFeatureClass(clsDef);
            }
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
            using (var picker = new ResourcePicker(_edsvc.ResourceService, ResourceTypes.FeatureSource, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    txtFeatureSource.Text = picker.ResourceID;
                    //Invalidate
                    _cachedFs = null;
                    OnResourceChanged(); //Maybe same feature class, different feature source
                }
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
                var expr = _edsvc.EditExpression(txtFilter.Text, cls, fs.Provider, fs.ResourceID, false);
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
                var expr = _edsvc.EditExpression(txtHyperlink.Text, cls, fs.Provider, fs.ResourceID, true);
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
                var expr = _edsvc.EditExpression(txtTooltip.Text, cls, fs.Provider, fs.ResourceID, true);
                if (expr != null)
                {
                    txtTooltip.Text = expr;
                }
            }
        }

        private void btnBrowseSchema_Click(object sender, EventArgs e)
        {
            var list = _edsvc.FeatureService.GetClassNames(txtFeatureSource.Text, null);
            var item = GenericItemSelectionDialog.SelectItem(null, null, list);
            if (item != null)
            {
                var cls = _edsvc.FeatureService.GetClassDefinition(txtFeatureSource.Text, item);
                SetFeatureClass(cls);
            }
        }

        private void SetFeatureClass(ClassDefinition item)
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

        private void txtHyperlink_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (string.IsNullOrEmpty(txtHyperlink.Text))
                _vl.Url = null;
            else
                _vl.Url = txtHyperlink.Text;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (string.IsNullOrEmpty(txtFilter.Text))
                _vl.Filter = null;
            else
                _vl.Filter = txtFilter.Text;
        }

        private void txtTooltip_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (string.IsNullOrEmpty(txtTooltip.Text))
                _vl.ToolTip = null;
            else
                _vl.ToolTip = txtTooltip.Text;
        }

        private void btnGoToFeatureSource_Click(object sender, EventArgs e)
        {
            _edsvc.OpenResource(txtFeatureSource.Text);
        }
    }
}
