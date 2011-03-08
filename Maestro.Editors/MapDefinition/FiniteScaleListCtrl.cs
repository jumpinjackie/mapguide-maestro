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

namespace Maestro.Editors.MapDefinition
{
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

        public FiniteScaleListCtrl(IMapDefinition map)
            : this()
        {
            _map = map;
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
                case ListChangedType.ItemDeleted:
                    {
                        RemoveScaleFromMap(_scales[e.NewIndex]);
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
            _map.BaseMap.RemoveAllScales();
        }

        private void RemoveScaleFromMap(double scale)
        {
            _map.InitBaseMap();
            _map.BaseMap.RemoveFiniteDisplayScale(scale);
        }

        private void AddScaleToMap(double scale)
        {
            _map.InitBaseMap();
            _map.BaseMap.AddFiniteDisplayScale(scale);
        }

        private void btnGenerateScales_Click(object sender, EventArgs e)
        {
            if (lstDisplayScales.Items.Count > 0)
            {
                if (MessageBox.Show(this, Properties.Resources.OverwriteDisplayScales, Properties.Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
            }
        }

        private void btnRemoveScale_Click(object sender, EventArgs e)
        {
            if (lstDisplayScales.SelectedItem != null)
            {
                double scale = (double)lstDisplayScales.SelectedItem;
                _scales.Remove(scale);
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
                    }
                }
            }
        }
    }
}
