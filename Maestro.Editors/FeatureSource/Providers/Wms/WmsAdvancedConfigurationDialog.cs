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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.Schema;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.Common;

namespace Maestro.Editors.FeatureSource.Providers.Wms
{
    public partial class WmsAdvancedConfigurationDialog : Form
    {
        private IEditorService _service;
        private WmsConfigurationDocument _config;
        private IFeatureSource _fs;

        public WmsConfigurationDocument Document { get { return _config; } }
        private BindingList<RasterWmsItem> _items;

        public WmsAdvancedConfigurationDialog(IEditorService service)
        {
            InitializeComponent();
            _items = new BindingList<RasterWmsItem>();
            _service = service;
            _fs = (IFeatureSource)_service.GetEditedResource();
            txtFeatureServer.Text = _fs.GetConnectionProperty("FeatureServer");
            string xml = _fs.GetConfigurationContent();
            if (!string.IsNullOrEmpty(xml))
            {
                try
                {
                    _config = (WmsConfigurationDocument)ConfigurationDocument.LoadXml(xml);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Properties.Resources.ErrorLoadingWmsConfig, ex.Message), Properties.Resources.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _config = BuildDefaultWmsDocument();
                }
            }
            else
            {
                _config = BuildDefaultWmsDocument();
            }

            _items = new BindingList<RasterWmsItem>(new List<RasterWmsItem>(_config.RasterOverrides));
            lstFeatureClasses.DataSource = _items;
        }

        private WmsConfigurationDocument BuildDefaultWmsDocument()
        {
            var doc = new WmsConfigurationDocument();
            var contexts = _fs.GetSpatialInfo(false);
            var schemaName = _fs.GetSchemaNames()[0];
            var clsNames = _fs.GetClassNames(schemaName);
            var schema = new FeatureSchema(schemaName, "");
            doc.AddSchema(schema);

            var defaultSc = contexts.SpatialContext[0];

            foreach (var clsName in clsNames)
            {
                var cls = new ClassDefinition(clsName, "");
                cls.AddProperty(new DataPropertyDefinition("Id", "")
                {
                    DataType = DataPropertyType.String,
                    Length = 256,
                    IsNullable = false
                }, true);
                cls.AddProperty(new RasterPropertyDefinition("Image", "")
                {
                    DefaultImageXSize = 1024,
                    DefaultImageYSize = 1024,
                    SpatialContextAssociation = defaultSc.Name
                });

                schema.AddClass(cls);

                var item = CreateDefaultItem(schema.Name, cls.Name, "Image", defaultSc);
                doc.AddRasterItem(item);
            }

            return doc;
        }

        private static RasterWmsItem CreateDefaultItem(string schemaName, string clsName, string rasName, IFdoSpatialContext defaultSc)
        {
            var item = new RasterWmsItem(clsName, rasName);
            item.ImageFormat = "PNG";
            item.IsTransparent = true;
            item.BackgroundColor = Color.White;
            item.SpatialContextName = defaultSc.Name;
            item.UseTileCache = false;
            item.AddLayer(new WmsLayerDefinition(schemaName + ":" + clsName) { Style = "default" });
            return item;
        }

        private static RasterPropertyDefinition GetRasterProperty(ClassDefinition cls)
        {
            foreach (var prop in cls.Properties)
            {
                if (prop.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Raster)
                    return (RasterPropertyDefinition)prop;
            }
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _fs.SetConfigurationContent(_config.ToXml());
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void lstFeatureClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (RasterWmsItem)lstFeatureClasses.SelectedItem;
            grpRaster.Controls.Clear();

            var ctrl = new RasterDefinitionCtrl(item, _service);
            ctrl.Dock = DockStyle.Fill;
            grpRaster.Controls.Add(ctrl);

            btnRemove.Enabled = true;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            /*
            var item = (RasterWmsItem)lstFeatureClasses.SelectedItem;
            _items.Remove(item);
            
            //Remove schema mapping item
            _config.RemoveRasterItem(item);

            //Remove mapped class from logical schema
            var schema = _config.Schemas[0];
            schema.RemoveClass(item.FeatureClass);
             */
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            /*
            var name = GenericInputDialog.GetValue(Properties.Resources.TitleNewFeatureClass, Properties.Resources.PromptName, null);
            var schema = _config.Schemas[0];

            var cls = new ClassDefinition(name, "");
            cls.AddProperty(new DataPropertyDefinition("Id", "")
            {
                DataType = DataPropertyType.String,
                Length = 256,
                IsNullable = false
            }, true);

            var rp = new RasterPropertyDefinition("Image", "")
            {
                DefaultImageXSize = 800,
                DefaultImageYSize = 800
            };
            cls.AddProperty(rp);

            schema.AddClass(cls);

            var item = CreateDefaultItem(cls, rp);
            _config.AddRasterItem(item);

            _items.Add(item);
             */
        }
    }
}
