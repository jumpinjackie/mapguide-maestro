#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using Maestro.Editors.Common;
using Maestro.Editors.Generic;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector
{
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

                txtFeatureClass.Text = _vl.FeatureName;
                txtGeometry.Text = _vl.Geometry;
                ResetErrorState();

                if (string.IsNullOrEmpty(txtFeatureClass.Text) || string.IsNullOrEmpty(txtGeometry.Text))
                {
                    TryFillUIFromNewFeatureSource(_vl.ResourceId);
                    if (!_edsvc.CurrentConnection.ResourceService.ResourceExists(_vl.ResourceId))
                    {
                        errorProvider.SetError(txtFeatureSource, Strings.LayerEditorFeatureSourceNotFound);
                        MessageBox.Show(Strings.LayerEditorHasErrors);
                    }
                }
                else
                {
                    bool bShowErrorMessage = false;
                    txtFeatureSource.Text = _vl.ResourceId;
                    string featureClass = txtFeatureClass.Text;
                    string geometry = txtGeometry.Text;
                    BusyWaitDialog.Run(null, () =>
                    {
                        var errors = new List<string>();
                        if (!_edsvc.CurrentConnection.ResourceService.ResourceExists(_vl.ResourceId))
                        {
                            errors.Add(Strings.LayerEditorFeatureSourceNotFound);
                        }
                        if (!string.IsNullOrEmpty(featureClass))
                        {
                            ClassDefinition clsDef = null;
                            try
                            {
                                clsDef = _edsvc.CurrentConnection.FeatureService.GetClassDefinition(_vl.ResourceId, featureClass);
                            }
                            catch
                            {
                                errors.Add(Strings.LayerEditorFeatureClassNotFound);
                                //These property mappings will probably be bunk if this is the case, so clear them
                                _vl.ClearPropertyMappings();
                            }

                            if (clsDef != null)
                            {
                                GeometricPropertyDefinition geom = clsDef.FindProperty(geometry) as GeometricPropertyDefinition;
                                if (geom == null)
                                {
                                    errors.Add(Strings.LayerEditorGeometryNotFound);
                                }
                            }
                            else
                            {
                                //This is probably true
                                errors.Add(Strings.LayerEditorGeometryNotFound);
                            }
                        }
                        return errors;
                    }, (result, ex) =>
                    {
                        if (ex != null)
                        {
                            ErrorDialog.Show(ex);
                        }
                        else
                        {
                            var list = (List<string>)result;
                            foreach (var err in list)
                            {
                                if (err == Strings.LayerEditorGeometryNotFound)
                                {
                                    errorProvider.SetError(txtGeometry, err);
                                    bShowErrorMessage = true;
                                }
                                else if (err == Strings.LayerEditorFeatureSourceNotFound)
                                {
                                    errorProvider.SetError(txtFeatureSource, err);
                                    //Don't show error message here if this is the only error as the user
                                    //will get a repair feature source prompt down the road
                                }
                                else if (err == Strings.LayerEditorFeatureClassNotFound)
                                {
                                    errorProvider.SetError(txtFeatureClass, err);
                                    bShowErrorMessage = true;
                                }
                            }
                            if (bShowErrorMessage)
                            {
                                MessageBox.Show(Strings.LayerEditorHasErrors);
                            }
                        }
                    });
                }

                txtFilter.Text = _vl.Filter;

                //Loose bind this one because 2.4 changes this behaviour making it
                //unsuitable for databinding via TextBoxBinder
                txtHyperlink.Text = _vl.Url;

                txtTooltip.Text = _vl.ToolTip;

                //This is not the root object so no change listeners have been subscribed
                _vl.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(OnVectorLayerPropertyChanged, (eh) => _vl.PropertyChanged -= eh);
            }
            finally
            {
                _init = false;
            }
        }

        private void ResetErrorState()
        {
            errorProvider.SetError(txtFeatureClass, null);
            errorProvider.SetError(txtGeometry, null);
            errorProvider.SetError(txtFeatureSource, null);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.DesignMode)
                return;

            //Init cached schemas and selected class
            if (!string.IsNullOrEmpty(txtFeatureClass.Text))
            {
                UIHelpers.LoadClassDefinition(_edsvc.CurrentConnection.FeatureService,
                    txtFeatureSource.Text,
                    txtFeatureClass.Text,
                    cls =>
                    {
                        _selectedClass = cls;
                        OnFeatureClassChanged();
                    });
            }
        }

        private void OnVectorLayerPropertyChanged(object sender, PropertyChangedEventArgs e) => OnResourceChanged();

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
            if (_init)
                return;

            if (string.IsNullOrEmpty(txtFeatureSource.Text))
                return;

            if (!string.IsNullOrEmpty(txtFeatureClass.Text))
            {
                //This feature source must have at least one class definition with a geometry property
                UIHelpers.LoadClassDefinition(_edsvc.CurrentConnection.FeatureService,
                    txtFeatureSource.Text,
                    txtFeatureClass.Text,
                    clsDef => SetFeatureClass(clsDef),
                    () =>
                    {
                        MessageBox.Show(string.Format(Strings.InvalidFeatureSourceNoClasses, txtFeatureSource.Text));
                        txtFeatureSource.Text = string.Empty;
                    });
            }
        }

        private void txtFeatureClass_TextChanged(object sender, EventArgs e)
        {
            if (txtFeatureClass.Text != _vl.FeatureName)
            {
                _vl.FeatureName = txtFeatureClass.Text;
                errorProvider.SetError(txtFeatureClass, null);
            }
        }

        private void txtGeometry_TextChanged(object sender, EventArgs e)
        {
            if (txtGeometry.Text != _vl.Geometry)
            {
                _vl.Geometry = txtGeometry.Text;
                errorProvider.SetError(txtGeometry, null);
            }
        }

        internal event EventHandler FeatureClassChanged;

        private void OnFeatureClassChanged()
        {
            this.FeatureClassChanged?.Invoke(this, EventArgs.Empty);

            if (_lastClassName != _selectedClass.QualifiedName && _lastClassName != null)
            {
                MessageBox.Show(Strings.LayerChangedFeatureClass);
            }
            _lastClassName = _selectedClass.QualifiedName;
        }

        private void btnBrowseFeatureSource_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edsvc.CurrentConnection, ResourceTypes.FeatureSource.ToString(), ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    string fsId = picker.ResourceID;
                    if (fsId != txtFeatureSource.Text)
                    {
                        TryFillUIFromNewFeatureSource(fsId);
                        OnResourceChanged(); //Maybe same feature class, different feature source
                    }
                }
            }
        }

        private void TryFillUIFromNewFeatureSource(string fsId)
        {
            try
            {
                _init = true;
                //Before setting the Feature Source, invalidate related parts too
                txtFeatureClass.Text = string.Empty;
                txtGeometry.Text = string.Empty;

                //But if this is a single-class FS, let's try to auto-fill this stuff
                string[] names = _edsvc.CurrentConnection.FeatureService.GetClassNames(fsId, null);
                if (names.Length == 1)
                    txtFeatureClass.Text = names[0];

                txtFeatureSource.Text = fsId;
                _vl.ResourceId = fsId;
                if (names.Length == 1)
                {
                    UIHelpers.LoadClassDefinition(_edsvc.CurrentConnection.FeatureService,
                        fsId,
                        names[0],
                        clsDef => SetFeatureClass(clsDef));
                }
            }
            finally
            {
                //Invalidate
                _cachedFs = null;
                _init = false;
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
                _cachedFs = (IFeatureSource)_edsvc.CurrentConnection.ResourceService.GetResource(txtFeatureSource.Text);

            return _cachedFs;
        }

        private void btnEditFilter_Click(object sender, EventArgs e)
        {
            var cls = GetSelectedClass();
            if (cls != null)
            {
                var fs = GetFeatureSource();
                var expr = _edsvc.EditExpression(txtFilter.Text, cls, fs.Provider, fs.ResourceID, ExpressionEditorMode.Filter, false);
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
                var expr = _edsvc.EditExpression(txtHyperlink.Text, cls, fs.Provider, fs.ResourceID, ExpressionEditorMode.Expression, true);
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
                var expr = _edsvc.EditExpression(txtTooltip.Text, cls, fs.Provider, fs.ResourceID, ExpressionEditorMode.Expression, true);
                if (expr != null)
                {
                    txtTooltip.Text = expr;
                }
            }
        }

        private void btnBrowseSchema_Click(object sender, EventArgs e)
        {
            var featSvc = _edsvc.CurrentConnection.FeatureService;
            var list = featSvc.GetClassNames(txtFeatureSource.Text, null);
            var item = GenericItemSelectionDialog.SelectItem(null, null, list);
            if (item != null)
            {
                UIHelpers.LoadClassDefinition(featSvc,
                    txtFeatureSource.Text,
                    item,
                    cls => SetFeatureClass(cls));
            }
        }

        private string _lastClassName = null;

        private void SetFeatureClass(ClassDefinition item)
        {
            txtFeatureClass.Text = item.QualifiedName;
            errorProvider.SetError(txtFeatureClass, null);
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

            errorProvider.SetError(txtGeometry, null);
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
            _vl.Url = string.IsNullOrEmpty(txtHyperlink.Text) ? null : txtHyperlink.Text;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _vl.Filter = string.IsNullOrEmpty(txtFilter.Text) ? null : txtFilter.Text;
        }

        private void txtTooltip_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _vl.ToolTip = string.IsNullOrEmpty(txtTooltip.Text) ? null : txtTooltip.Text;
        }

        private void btnGoToFeatureSource_Click(object sender, EventArgs e)
        {
            _edsvc.OpenResource(txtFeatureSource.Text);
        }

        internal void SetFeatureSource(string fsId)
        {
            txtFeatureSource.Text = fsId;
            errorProvider.SetError(txtFeatureSource, null);
        }
    }
}