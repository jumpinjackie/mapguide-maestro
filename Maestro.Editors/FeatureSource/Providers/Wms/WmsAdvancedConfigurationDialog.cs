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
using Maestro.Shared.UI;

namespace Maestro.Editors.FeatureSource.Providers.Wms
{
    internal partial class WmsAdvancedConfigurationDialog : Form
    {
        private IEditorService _service;
        private WmsConfigurationDocument _config;
        private IFeatureSource _fs;

        public WmsConfigurationDocument Document { get { return _config; } }

        public WmsAdvancedConfigurationDialog(IEditorService service)
        {
            InitializeComponent();
            grdSpatialContexts.AutoGenerateColumns = false;
            _service = service;
            _fs = (IFeatureSource)_service.GetEditedResource();
            txtFeatureServer.Text = _fs.GetConnectionProperty("FeatureServer"); //NOXLATE
            string xml = _fs.GetConfigurationContent();
            if (!string.IsNullOrEmpty(xml))
            {
                try
                {
                    _config = (WmsConfigurationDocument)ConfigurationDocument.LoadXml(xml);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Strings.ErrorLoadingWmsConfig, ex.Message), Strings.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MakeDefaultDocument();
                }
            }
            else
            {
                MakeDefaultDocument();
            }

            lstFeatureClasses.DataSource = _config.RasterOverrides;
            grdSpatialContexts.DataSource = _config.SpatialContexts;
        }

        private WmsConfigurationDocument BuildDefaultWmsDocument()
        {
            var doc = new WmsConfigurationDocument();
            var contexts = _fs.GetSpatialInfo(false);
            var schemaName = _fs.GetSchemaNames()[0];
            var clsNames = _fs.GetClassNames(schemaName);
            var schema = new FeatureSchema(schemaName, string.Empty);
            doc.AddSchema(schema);

            foreach (var sc in contexts.SpatialContext)
            {
                doc.AddSpatialContext(sc);
            }

            var defaultSc = contexts.SpatialContext[0];

            foreach (var clsName in clsNames)
            {
                var className = clsName.Split(':')[1]; //NOXLATE
                var cls = new ClassDefinition(className, string.Empty);
                cls.AddProperty(new DataPropertyDefinition("Id", string.Empty) //NOXLATE
                {
                    DataType = DataPropertyType.String,
                    Length = 256,
                    IsNullable = false
                }, true);
                cls.AddProperty(new RasterPropertyDefinition("Image", string.Empty) //NOXLATE
                {
                    DefaultImageXSize = 1024,
                    DefaultImageYSize = 1024,
                    SpatialContextAssociation = defaultSc.Name
                });

                schema.AddClass(cls);

                var item = CreateDefaultItem(schema.Name, cls.Name, "Image", defaultSc); //NOXLATE
                doc.AddRasterItem(item);
            }

            return doc;
        }

        private static RasterWmsItem CreateDefaultItem(string schemaName, string clsName, string rasName, IFdoSpatialContext defaultSc)
        {
            var item = new RasterWmsItem(schemaName, clsName, rasName);
            item.ImageFormat = "PNG"; //NOXLATE
            item.IsTransparent = true;
            item.BackgroundColor = Color.White;
            item.SpatialContextName = defaultSc.Name;
            item.UseTileCache = false;
            item.AddLayer(new WmsLayerDefinition(clsName) { Style = "default" }); //NOXLATE
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

        private ClassDefinition _logicalClass;
        private bool _updatingLogicalClassUI = false;

        private void lstFeatureClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (RasterWmsItem)lstFeatureClasses.SelectedItem;
            grpRaster.Controls.Clear();
            _updatingLogicalClassUI = true;
            try
            {
                if (item != null)
                {
                    var ctrl = new RasterDefinitionCtrl(_config, item, _service);
                    ctrl.Dock = DockStyle.Fill;
                    grpRaster.Controls.Add(ctrl);

                    btnRemove.Enabled = true;

                    //Get logical class
                    string schemaName = item.SchemaName;
                    string className = item.FeatureClass;

                    if (!string.IsNullOrEmpty(schemaName) && !string.IsNullOrEmpty(className))
                    {
                        _logicalClass = _config.GetClass(schemaName, className);
                        if (_logicalClass != null)
                        {
                            txtClassName.Text = _logicalClass.Name;
                            txtClassDescription.Text = _logicalClass.Description;
                        }
                        else
                        {
                            txtClassName.Text = string.Empty;
                            txtClassDescription.Text = string.Empty;
                        }
                    }
                    else
                    {
                        _logicalClass = null;
                        txtClassName.Text = string.Empty;
                        txtClassDescription.Text = string.Empty;
                    }
                }
                else
                {
                    _logicalClass = null;
                    txtClassName.Text = string.Empty;
                    txtClassDescription.Text = string.Empty;
                }
            }
            finally
            {
                _updatingLogicalClassUI = false;
            }
            grpLogicalClass.Enabled = (_logicalClass != null);
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

            var cls = new ClassDefinition(name, string.Empty);
            cls.AddProperty(new DataPropertyDefinition("Id", string.Empty) //NOXLATE
            {
                DataType = DataPropertyType.String,
                Length = 256,
                IsNullable = false
            }, true);

            var rp = new RasterPropertyDefinition("Image", string.Empty) //NOXLATE
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            MakeDefaultDocument();
            lstFeatureClasses.DataSource = _config.RasterOverrides;
            grdSpatialContexts.DataSource = _config.SpatialContexts;
        }

        private void MakeDefaultDocument()
        {
            try
            {
                _config = (WmsConfigurationDocument)_service.FeatureService.GetSchemaMapping("OSGeo.WMS", _fs.ConnectionString); //NOXLATE
                //BOGUS: This was not as sufficient as I originally thought, nevertheless this contains
                //information that would not exist if we constructed the document the old fashioned way.
                string defaultScName = string.Empty;
                if (_config.SpatialContexts.Length > 0)
                {
                    defaultScName = _config.SpatialContexts[0].Name;
                }
                else
                {
                    var list = _fs.GetSpatialInfo(false);
                    if (list.SpatialContext.Count > 0)
                    {
                        defaultScName = list.SpatialContext[0].Name;
                    }
                    else //Really? What kind of WMS service are you????
                    {
                        var sc = new FdoSpatialContextListSpatialContext()
                        {
                            Name = "EPSG:4326", //NOXLATE
                            Description = "Maestro-generated spatial context", //NOXLATE
                            CoordinateSystemName = "EPSG:4326", //NOXLATE
                            CoordinateSystemWkt = "GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722293]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.01745329251994]]", //NOXLATE
                            Extent = new FdoSpatialContextListSpatialContextExtent()
                            {
                                LowerLeftCoordinate = new FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate()
                                {
                                    X = "-180.0", //NOXLATE
                                    Y = "-90.0" //NOXLATE
                                },
                                UpperRightCoordinate = new FdoSpatialContextListSpatialContextExtentUpperRightCoordinate()
                                {
                                    X = "180.0", //NOXLATE
                                    Y = "90.0" //NOXLATE
                                }
                            },
                            ExtentType = FdoSpatialContextListSpatialContextExtentType.Static,
                            IsActive = true,
                            XYTolerance = 0.0001,
                            ZTolerance = 0.0001,
                        };
                        _config.AddSpatialContext(sc);
                        defaultScName = sc.Name;
                    }
                }

                EnsureRasterProperties(defaultScName);
                _config.EnsureConsistency();
            }
            catch
            {
                _config = BuildDefaultWmsDocument();
            }
        }

        private void EnsureRasterProperties(string defaultScName)
        {
            foreach (var schema in _config.Schemas)
            {
                foreach (var cls in schema.Classes)
                {
                    //Add identity property if none found
                    if (cls.IdentityProperties.Count == 0)
                    {
                        cls.AddProperty(new DataPropertyDefinition("Id", string.Empty) //NOXLATE
                        {
                            DataType = DataPropertyType.String,
                            Length = 256,
                            IsNullable = false
                        }, true);
                    }
                    //Add raster property if there's only one property (the identity property we either just added or found)
                    if (cls.Properties.Count == 1)
                    {
                        cls.AddProperty(new RasterPropertyDefinition("Image", string.Empty) //NOXLATE
                        {
                            DefaultImageXSize = 1024,
                            DefaultImageYSize = 1024,
                            SpatialContextAssociation = defaultScName
                        });
                    }
                    else
                    {
                        bool bFoundRaster = false;
                        //Try to find this raster property
                        foreach (var prop in cls.Properties)
                        {
                            if (prop.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Raster)
                            {
                                bFoundRaster = true;
                                break;
                            }
                        }
                        if (!bFoundRaster)
                        {
                            cls.AddProperty(new RasterPropertyDefinition("Image", string.Empty) //NOXLATE
                            {
                                DefaultImageXSize = 1024,
                                DefaultImageYSize = 1024,
                                SpatialContextAssociation = defaultScName
                            });
                        }
                    }
                }
            }
        }

        private void grdSpatialContexts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                string wkt = _service.GetCoordinateSystem();
                if (!string.IsNullOrEmpty(wkt))
                {
                    grdSpatialContexts[e.ColumnIndex, e.RowIndex].Value = wkt;
                }
            }
        }

        private void txtClassName_TextChanged(object sender, EventArgs e)
        {
            if (_updatingLogicalClassUI) return;
            if (_logicalClass == null) return;
            var item = lstFeatureClasses.SelectedItem  as RasterWmsItem;
            if (item == null) return;

            _logicalClass.Name = txtClassName.Text;
            item.FeatureClass = _logicalClass.Name;
            lstFeatureClasses.DataSource = _config.RasterOverrides; //rebind
        }

        private void txtClassDescription_TextChanged(object sender, EventArgs e)
        {
            if (_updatingLogicalClassUI) return;
            if (_logicalClass == null) return;
            var item = lstFeatureClasses.SelectedItem as RasterWmsItem;
            if (item == null) return;

            _logicalClass.Description = txtClassDescription.Text;
            item.FeatureClass = _logicalClass.Name;
            lstFeatureClasses.DataSource = _config.RasterOverrides; //rebind
        }

        private void lnkSwap_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_logicalClass == null) return;
            var item = lstFeatureClasses.SelectedItem as RasterWmsItem;
            if (item == null) return;

            try
            {
                _updatingLogicalClassUI = true;
                var tmp = txtClassName.Text;
                txtClassName.Text = txtClassDescription.Text;
                txtClassDescription.Text = tmp;

                _logicalClass.Name = txtClassName.Text;
                _logicalClass.Description = txtClassDescription.Text;
                item.FeatureClass = _logicalClass.Name;
                lstFeatureClasses.DataSource = _config.RasterOverrides; //rebind
            }
            finally
            {
                _updatingLogicalClassUI = false;
            }
        }

        private void btnSwapAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Strings.ConfirmWmsLogicalClassSwap, string.Empty, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                using (new WaitCursor(this))
                {
                    _logicalClass = null;
                    lstFeatureClasses.SelectedItem = null;
                    foreach (var item in _config.RasterOverrides)
                    {
                        var cls = _config.GetClass(item.SchemaName, item.FeatureClass);
                        if (cls == null)
                            continue;

                        var tmp = cls.Name;
                        cls.Name = cls.Description;
                        cls.Description = tmp;

                        item.FeatureClass = cls.Name;
                    }
                    lstFeatureClasses.DataSource = _config.RasterOverrides; //rebind
                }
            }
        }
    }
}
