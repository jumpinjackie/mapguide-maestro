using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.OverrideEditor
{
    [ToolboxItem(false)]
    internal partial class TableConfigCtrl : UserControl
    {
        public TableConfigCtrl()
        {
            InitializeComponent();
        }

        private string[] _scNames;

        public void SetSpatialContexts(string[] spatialContextNames)
        {
            _scNames = spatialContextNames;
        }

        private TableOverrideItem _item;

        public void Reset()
        {
            _item = null;
            chkGeometry.Checked = false;
            txtKey.Text = txtTable.Text = txtX.Text = txtY.Text = txtZ.Text = string.Empty;
        }

        internal void Init(TableOverrideItem item)
        {
            Reset();
            chkGeometry.Checked = !string.IsNullOrEmpty(item.Class.DefaultGeometryPropertyName);
            txtTable.Text = item.TableName;
            txtSpatialContext.Text = item.SpatialContext;
            txtKey.Text = item.Key;
            txtX.Text = item.X;
            txtY.Text = item.Y;
            txtZ.Text = item.Z;
            _item = item;
        }

        private void txtSpatialContext_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.SpatialContext = txtSpatialContext.Text;
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.Key = txtKey.Text;
        }

        private void txtX_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.X = txtX.Text;
        }

        private void txtY_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.Y = txtY.Text;
        }

        private void txtZ_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.Z = txtZ.Text;
        }

        private void chkGeometry_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkGeometry.Checked)
            {
                if (_item != null)
                {
                    //Remove existing geometry property if defined
                    if (!string.IsNullOrEmpty(_item.Class.DefaultGeometryPropertyName))
                    {
                        var prop = _item.Class.FindProperty(_item.Class.DefaultGeometryPropertyName);
                        if (prop != null)
                        {
                            _item.Class.RemoveProperty(prop);
                        }
                        _item.Class.DefaultGeometryPropertyName = null;
                    }
                }
                txtSpatialContext.Text = txtX.Text = txtY.Text = txtZ.Text = string.Empty;
            }
            else
            {
                if (_item != null)
                {
                    //Set logical geometry property
                    var prop = new GeometricPropertyDefinition("Geometry", ""); //NOXLATE
                    prop.GeometricTypes = FeatureGeometricType.Point;
                    prop.HasElevation = true;
                    prop.HasMeasure = true;

                    _item.Class.AddProperty(prop);
                    _item.Class.DefaultGeometryPropertyName = prop.Name;

                    if (_scNames.Length > 0)
                        txtSpatialContext.Text = _scNames[0];
                }
            }
        }

        private string[] GetPropertyNames()
        {
            List<string> values = new List<string>();
            foreach (var prop in _item.Class.Properties)
            {
                if (prop.Type == PropertyDefinitionType.Data)
                {
                    values.Add(prop.Name);
                }
            }
            return values.ToArray();
        }

        private string[] GetNumericPropertyNames()
        {
            List<string> values = new List<string>();
            foreach (var prop in _item.Class.Properties)
            {
                if (prop.Type == PropertyDefinitionType.Data)
                {
                    var dt = ((DataPropertyDefinition)prop).DataType;
                    switch (dt)
                    {
                        case DataPropertyType.Byte:
                        case DataPropertyType.Double:
                        case DataPropertyType.Int16:
                        case DataPropertyType.Int32:
                        case DataPropertyType.Int64:
                        case DataPropertyType.Single:
                            values.Add(prop.Name);
                            break;
                    }
                }
            }
            return values.ToArray();
        }

        private void btnKey_Click(object sender, EventArgs e)
        {
            var item = GenericItemSelectionDialog.SelectItem(Strings.SelectProperty, Strings.SelectProperty, GetPropertyNames());
            if (!string.IsNullOrEmpty(item))
            {
                txtKey.Text = item;

                //Change the mapped logical class
                var prop = _item.Class.FindProperty(item);
                if (prop != null && prop.Type == PropertyDefinitionType.Data)
                {
                    _item.Class.ClearIdentityProperties();
                    _item.Class.AddProperty((DataPropertyDefinition)prop, true);
                }
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (!chkGeometry.Checked)
            {
                MessageBox.Show(Strings.CheckGeometryFirst);
                return;
            }

            var item = GenericItemSelectionDialog.SelectItem(Strings.SelectProperty, Strings.SelectProperty, GetNumericPropertyNames());
            if (!string.IsNullOrEmpty(item))
            {
                txtX.Text = item;
            }
        }

        private void btnY_Click(object sender, EventArgs e)
        {
            if (!chkGeometry.Checked)
            {
                MessageBox.Show(Strings.CheckGeometryFirst);
                return;
            }

            var item = GenericItemSelectionDialog.SelectItem(Strings.SelectProperty, Strings.SelectProperty, GetNumericPropertyNames());
            if (!string.IsNullOrEmpty(item))
            {
                txtY.Text = item;
            }
        }

        private void btnZ_Click(object sender, EventArgs e)
        {
            if (!chkGeometry.Checked)
            {
                MessageBox.Show(Strings.CheckGeometryFirst);
                return;
            }

            var item = GenericItemSelectionDialog.SelectItem(Strings.SelectProperty, Strings.SelectProperty, GetNumericPropertyNames());
            if (!string.IsNullOrEmpty(item))
            {
                txtZ.Text = item;
            }
        }

        private void btnSpatialContext_Click(object sender, EventArgs e)
        {
            if (!chkGeometry.Checked)
            {
                MessageBox.Show(Strings.CheckGeometryFirst);
                return;
            }

            var item = GenericItemSelectionDialog.SelectItem(Strings.SelectSpatialContext, Strings.SelectSpatialContext, _scNames);
            if (!string.IsNullOrEmpty(item))
            {
                txtSpatialContext.Text = item;
            }
        }
    }
}
