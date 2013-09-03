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
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;

namespace Maestro.Editors.MapDefinition
{
    [ToolboxItem(false)]
    internal partial class FiniteScaleListCtrl : UserControl
    {
        internal FiniteScaleListCtrl()
        {
            InitializeComponent();
            cmbMethod.DataSource = Enum.GetValues(typeof(ScaleGenerationMethod));
            cmbRounding.DataSource = Enum.GetValues(typeof(ScaleRoundingMethod));

            cmbMethod.SelectedIndex = 0;
            cmbRounding.SelectedIndex = 0;
            _scales = new BindingList<double>();

            lstDisplayScales.DataSource = _scales;
        }

        private BindingList<double> _scales;

        private IMapDefinition _map;
        private IEditorService _edSvc;

        public FiniteScaleListCtrl(IMapDefinition map, IEditorService editorSvc)
            : this()
        {
            _map = map;
            _edSvc = editorSvc;
            //Init scale list
            if (_map.BaseMap != null)
            {
                foreach (var scale in _map.BaseMap.FiniteDisplayScale)
                {
                    _scales.Add(scale);
                }
            }
            //Now wire change events
            _scales.ListChanged += new ListChangedEventHandler(OnScaleListChanged);
        }

        void OnScaleListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    {
                        AddScaleToMap(_scales[e.NewIndex]);
                    }
                    break;
                case ListChangedType.Reset:
                    {
                        ClearScales();
                    }
                    break;
            }
        }

        private void ClearScales()
        {
            _map.InitBaseMap();
            _map.RemoveAllFiniteDisplayScales(true);
            _edSvc.MarkDirty();
        }

        private void RemoveScaleFromMap(double scale)
        {
            _scales.Remove(scale);
            _map.RemoveFiniteDisplayScale(scale, true);
            _edSvc.MarkDirty();
        }

        private void AddScaleToMap(double scale)
        {
            _map.InitBaseMap();
            _map.AddFiniteDisplayScale(scale);
            _edSvc.MarkDirty();
        }

        private void btnGenerateScales_Click(object sender, EventArgs e)
        {
            if (numScales.Value == 0)
            {
                MessageBox.Show(this, Strings.NoScalesToGenerate, Strings.TitleError, MessageBoxButtons.OK);
                return;
            }

            if (lstDisplayScales.Items.Count > 0)
            {
                if (MessageBox.Show(this, Strings.OverwriteDisplayScales, Strings.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            using (new WaitCursor(this))
            {
                var values = ScaleListGenerator.GenerateScales(
                    (double)numMinScale.Value,
                    (double)numMaxScale.Value,
                    (ScaleGenerationMethod)cmbMethod.SelectedItem,
                    (ScaleRoundingMethod)cmbRounding.SelectedItem,
                    (int)numScales.Value);

                _scales.Clear();
                foreach (var s in values)
                {
                    _scales.Add(s);
                }
                _edSvc.MarkDirty();
            }
        }

        private void btnRemoveScale_Click(object sender, EventArgs e)
        {
            if (lstDisplayScales.SelectedItem != null)
            {
                double scale = (double)lstDisplayScales.SelectedItem;
                RemoveScaleFromMap(scale);
            }
        }

        private void lstDisplayScales_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveScale.Enabled = (lstDisplayScales.SelectedItem != null);
        }

        private void btnEditScalesManually_Click(object sender, EventArgs e)
        {
            using (var dlg = new ManualScaleEditor(_scales))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    using (new WaitCursor(this))
                    {
                        _scales.Clear();
                        foreach (double scale in dlg.Scales)
                        {
                            _scales.Add(scale);
                        }
                        _edSvc.MarkDirty();
                    }
                }
            }
        }

        static double[] CMS_SCALE_LIST = { 
            1128.49722,
            2256.9944399999999,
            4513.9888799999999,
            9027.9777610000001,
            18055.95552,
            36111.911039999999,
            72223.822090000001,
            144447.64420000001,
            288895.28840000002,
            577790.57669999998,
            1155581.1529999999,
            2311162.307,
            4622324.6140000001,
            9244649.227,
            18489298.449999999,
            36978596.909999996,
            73957193.819999993,
            147914387.59999999,
            295828775.30000001,
            591657550.5
        };

        private void btnCmsScaleList_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Strings.ConfirmGoogleScaleList, Strings.Confirm, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _scales.Clear();
                foreach (var scale in CMS_SCALE_LIST)
                {
                    _scales.Add(scale);
                }
                _edSvc.MarkDirty();
            }
        }
    }
}
