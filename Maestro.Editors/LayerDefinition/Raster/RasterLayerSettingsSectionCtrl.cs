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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System.Diagnostics;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Shared.UI;

namespace Maestro.Editors.LayerDefinition.Raster
{
    //NOTE: Unlike the Vector Layer editor, we have to do a full schema walk here because
    //we need to filter out non-raster feature classes, something that the existing GetSchemas()
    //and GetClassNames() cannot do for us.
    
    [ToolboxItem(false)]
    internal partial class RasterLayerSettingsSectionCtrl : EditorBindableCollapsiblePanel
    {
        public RasterLayerSettingsSectionCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edsvc;
        private IRasterLayerDefinition _rl;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);

            var res = service.GetEditedResource() as ILayerDefinition;
            Debug.Assert(res != null);

            _rl = res.SubLayer as IRasterLayerDefinition;
            Debug.Assert(_rl != null);

            TextBoxBinder.BindText(txtFeatureSource, _rl, "ResourceId"); //NOXLATE
            TextBoxBinder.BindText(txtFeatureClass, _rl, "FeatureName"); //NOXLATE
            TextBoxBinder.BindText(txtGeometry, _rl, "Geometry"); //NOXLATE
            _rl.PropertyChanged += OnRasterLayerPropertyChanged;
        }

        void OnRasterLayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnResourceChanged();
        }

        protected override void UnsubscribeEventHandlers()
        {
            if (_rl != null)
            {
                _rl.PropertyChanged -= OnRasterLayerPropertyChanged;
            }
            base.UnsubscribeEventHandlers();
        }

        private FeatureSourceDescription _cachedDesc;

        protected override void OnLoad(EventArgs e)
        {
            if (_cachedDesc == null)
                _cachedDesc = _edsvc.FeatureService.DescribeFeatureSource(txtFeatureSource.Text);

            //Init cached schemas and selected class
            if (!string.IsNullOrEmpty(txtFeatureClass.Text))
            {
                var cls = _cachedDesc.GetClass(txtFeatureClass.Text);
                if (cls != null)
                {
                    _selectedClass = cls;
                }
            }
            else
            {
                SetFeatureClass(_cachedDesc.Schemas[0].Classes[0]);
            }
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
                }
            }
        }

        private void btnBrowseSchema_Click(object sender, EventArgs e)
        {
            var list = new List<ClassDefinition>();
            foreach (var cls in _cachedDesc.AllClasses)
            {
                bool hasRaster = false;
                foreach (var prop in cls.Properties)
                {
                    var rp = prop as RasterPropertyDefinition;
                    if (rp != null)
                    {
                        hasRaster = true;
                        break;
                    }
                }

                if (hasRaster)
                    list.Add(cls);
            }
            if (list.Count == 0)
            {
                MessageBox.Show(Strings.NoRasterClasses);
                return;
            }

            var item = GenericItemSelectionDialog.SelectItem(null, null, list.ToArray(), "QualifiedName", "QualifiedName"); //NOXLATE
            if (item != null)
            {
                SetFeatureClass(item);
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
                if (col.Type == PropertyDefinitionType.Raster && col.Name.Equals(txtGeometry.Text))
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
                if (col.Type == PropertyDefinitionType.Raster)
                    geoms.Add(col);
            }

            if (geoms.Count == 1)
                txtGeometry.Text = geoms[0].Name;

            //OnFeatureClassChanged();
        }

        private void btnBrowseGeometry_Click(object sender, EventArgs e)
        {
            if (_selectedClass != null)
            {
                List<PropertyDefinition> geoms = new List<PropertyDefinition>();
                foreach (var col in _selectedClass.Properties)
                {
                    if (col.Type == PropertyDefinitionType.Raster)
                        geoms.Add(col);
                }

                var item = GenericItemSelectionDialog.SelectItem(null, null, geoms.ToArray(), "Name", "Name"); //NOXLATE
                if (item != null)
                {
                    txtGeometry.Text = item.Name;
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

        private void btnGoToFeatureSource_Click(object sender, EventArgs e)
        {
            _edsvc.OpenResource(txtFeatureSource.Text);
        }
    }
}
