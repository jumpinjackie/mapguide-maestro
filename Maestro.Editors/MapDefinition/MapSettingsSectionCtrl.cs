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
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.Editors.MapDefinition
{
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class MapSettingsSectionCtrl : EditorBindableCollapsiblePanel
    {
        public MapSettingsSectionCtrl()
        {
            InitializeComponent();
        }

        private IMapDefinition _map;

        const string META_START = "<MapDescription>";
        const string META_END = "</MapDescription>";

        private IEditorService _service;
        private bool _updatingExtents = false;

        public override void Bind(IEditorService service)
        {
            cmbBackgroundColor.ResetColors();

            _service = service;
            _service.RegisterCustomNotifier(this);
            _map = (IMapDefinition)service.GetEditedResource();

            var bmeta = new Binding("Text", _map, "Metadata");
            bmeta.Parse += (sender, e) =>
            {
                e.Value = META_START + e.Value + META_END;
            };
            bmeta.Format += (sender, e) =>
            {
                if (e.Value != null)
                {
                    var str = e.Value.ToString();
                    if (str.StartsWith(META_START) && str.EndsWith(META_END))
                    {
                        e.Value = str.Substring(META_START.Length, str.Length - (META_START.Length + META_END.Length));
                    }
                }
            };
            TextBoxBinder.BindText(txtDescription, bmeta);
            TextBoxBinder.BindText(txtCoordinateSystem, _map, "CoordinateSystem");

            //ColorComboBox requires custom databinding
            cmbBackgroundColor.CurrentColor = _map.BackgroundColor;
            cmbBackgroundColor.SelectedIndexChanged += (sender, e) =>
            {
                _map.BackgroundColor = cmbBackgroundColor.CurrentColor;
            };
            _map.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "BackgroundColor")
                {
                    cmbBackgroundColor.CurrentColor = _map.BackgroundColor;
                }
                else if (e.PropertyName == "Extents")
                {
                    UpdateExtentsFromMap();
                }
            };

            txtLowerX.Text = _map.Extents.MinX.ToString(CultureInfo.InvariantCulture);
            txtLowerY.Text = _map.Extents.MinY.ToString(CultureInfo.InvariantCulture);
            txtUpperX.Text = _map.Extents.MaxX.ToString(CultureInfo.InvariantCulture);
            txtUpperY.Text = _map.Extents.MaxY.ToString(CultureInfo.InvariantCulture);

            txtLowerX.TextChanged += (s, e) =>
            {
                if (_updatingExtents)
                    return;

                if (txtLowerX.Text.EndsWith(".")) //Maybe typing in decimals atm
                    return;

                double d;
                if (double.TryParse(txtLowerX.Text, out d))
                    _map.Extents.MinX = d;
            };

            txtLowerY.TextChanged += (s, e) =>
            {
                if (_updatingExtents)
                    return;

                if (txtLowerY.Text.EndsWith(".")) //Maybe typing in decimals atm
                    return;

                double d;
                if (double.TryParse(txtLowerY.Text, out d))
                    _map.Extents.MinY = d;
            };

            txtUpperX.TextChanged += (s, e) =>
            {
                if (_updatingExtents)
                    return;

                if (txtUpperX.Text.EndsWith(".")) //Maybe typing in decimals atm
                    return;

                double d;
                if (double.TryParse(txtUpperX.Text, out d))
                    _map.Extents.MaxX = d;
            };

            txtUpperY.TextChanged += (s, e) =>
            {
                if (_updatingExtents)
                    return;

                if (txtUpperY.Text.EndsWith(".")) //Maybe typing in decimals atm
                    return;

                double d;
                if (double.TryParse(txtUpperY.Text, out d))
                    _map.Extents.MaxY = d;
            };

            _map.Extents.PropertyChanged += (sender, e) => 
            {
                UpdateExtentsFromMap();
                OnResourceChanged(); 
            };
        }

        private void UpdateExtentsFromMap()
        {
            _updatingExtents = true;
            try
            {
                txtLowerX.Text = _map.Extents.MinX.ToString(CultureInfo.InvariantCulture);
                txtLowerY.Text = _map.Extents.MinY.ToString(CultureInfo.InvariantCulture);
                txtUpperX.Text = _map.Extents.MaxX.ToString(CultureInfo.InvariantCulture);
                txtUpperY.Text = _map.Extents.MaxY.ToString(CultureInfo.InvariantCulture);
            }
            finally
            {
                _updatingExtents = false;
            }
        }

        private void btnPickCs_Click(object sender, EventArgs e)
        {
            string cs = _service.GetCoordinateSystem();
            if (!string.IsNullOrEmpty(cs))
            {
                string oldCs = txtCoordinateSystem.Text;
                txtCoordinateSystem.Text = cs;
                if (_map.Extents != null && !string.IsNullOrEmpty(oldCs) && !string.IsNullOrEmpty(cs) && !oldCs.Equals(cs))
                {
                    //Transform current extents
                    try
                    {
                        var trans = _service.GetEditedResource().CurrentConnection.CoordinateSystemCatalog.CreateTransform(oldCs, cs);

                        var oldExt = _map.Extents;

                        double llx;
                        double lly;
                        double urx;
                        double ury;

                        trans.Transform(oldExt.MinX, oldExt.MinY, out llx, out lly);
                        trans.Transform(oldExt.MaxX, oldExt.MaxY, out urx, out ury);

                        _map.Extents.MinX = llx;
                        _map.Extents.MinY = lly;
                        _map.Extents.MaxX = urx;
                        _map.Extents.MaxY = ury;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format(Strings.CoordinateTransformationFailed, ex.Message));
                    }
                }
            }
        }

        private void btnSetZoom_Click(object sender, EventArgs e)
        {
            var diag = new ExtentCalculationDialog(_map);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var env = diag.Extents;
                if (env != null)
                {
                    _map.SetExtents(env.MinX, env.MinY, env.MaxX, env.MaxY);
                }
                else
                {
                    MessageBox.Show(Strings.ErrorMapExtentCalculationFailed, Strings.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
