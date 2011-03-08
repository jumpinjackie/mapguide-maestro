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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.Editors.LayerDefinition.Drawing
{
    [ToolboxItem(true)]
    internal partial class DrawingLayerSettingsCtrl : CollapsiblePanel,IEditorBindable
    {
        public DrawingLayerSettingsCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;
        private IDrawingLayerDefinition _dlayer;

        public void Bind(IEditorService service)
        {
            _service = service;
            _service.RegisterCustomNotifier(this);

            var lyr = service.GetEditedResource() as ILayerDefinition;
            Debug.Assert(lyr != null);

            _dlayer = lyr.SubLayer as IDrawingLayerDefinition;
            Debug.Assert(_dlayer != null);

            TextBoxBinder.BindText(txtDrawingSource, _dlayer, "ResourceId");

            var sheets = _service.DrawingService.EnumerateDrawingSections(_dlayer.ResourceId);
            cmbSheet.DisplayMember = "Title";
            cmbSheet.ValueMember = "Name";
            ComboBoxBinder.BindSelectedIndexChanged(cmbSheet, "SelectedValue", _dlayer, "Sheet");
            cmbSheet.DataSource = sheets.Section;

            var minBinding = new Binding("Text", _dlayer, "MinScale");
            var maxBinding = new Binding("Text", _dlayer, "MaxScale");

            minBinding.Format += (sender, ce) =>
            {
                if (ce.DesiredType != typeof(string)) return;

                ce.Value = Convert.ToDouble(ce.Value);
            };
            minBinding.Parse += (sender, ce) =>
            {
                if (ce.DesiredType != typeof(double)) return;
                double val;
                if (!double.TryParse(ce.Value.ToString(), out val))
                    return;

                ce.Value = val;
            };
            maxBinding.Format += (sender, ce) =>
            {
                if (ce.DesiredType != typeof(string)) return;

                ce.Value = Convert.ToDouble(ce.Value);
            };
            maxBinding.Parse += (sender, ce) =>
            {
                if (ce.DesiredType != typeof(double)) return;
                double val;
                if (!double.TryParse(ce.Value.ToString(), out val))
                    return;

                ce.Value = val;
            };

            TextBoxBinder.BindText(txtMinScale, minBinding);
            TextBoxBinder.BindText(txtMaxScale, maxBinding);

            //This is not the root object so no change listeners have been subscribed
            _dlayer.PropertyChanged += (sender, e) => { OnResourceChanged(); };
        }

        private void OnResourceChanged()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;

        private void cmbSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListDwfLayers.Items.Clear();

            var layers = _service.DrawingService.EnumerateDrawingLayers(_dlayer.ResourceId, _dlayer.Sheet);
            if (string.IsNullOrEmpty(_dlayer.LayerFilter)) //All layers
            {
                foreach (var lyr in layers)
                {
                    chkListDwfLayers.Items.Add(lyr, true);
                }
            }
            else 
            {
                string[] visible = _dlayer.LayerFilter.Split(',');
                foreach (var lyr in layers)
                {
                    if (Array.IndexOf<string>(visible, lyr) >= 0)
                        chkListDwfLayers.Items.Add(lyr, true);
                    else
                        chkListDwfLayers.Items.Add(lyr, false);
                }
            }
        }

        private bool IsAllLayersChecked()
        {
            return chkListDwfLayers.CheckedIndices.Count == chkListDwfLayers.Items.Count;
        }

        private string GetLayerFilter()
        {
            if (chkListDwfLayers.CheckedIndices.Count == 0)
            {
                return string.Empty;
            }
            else if (IsAllLayersChecked())
            {
                return null;
            }
            else
            {
                var list = new List<string>();
                foreach (var obj in chkListDwfLayers.CheckedItems)
                {
                    list.Add(obj.ToString());
                }
                return string.Join(",", list.ToArray());
            }
        }

        private void chkListDwfLayers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _dlayer.LayerFilter = GetLayerFilter();
        }

        private void lnkCheckAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < chkListDwfLayers.Items.Count; i++)
            {
                chkListDwfLayers.SetItemChecked(i, true);
            }
            Debug.Assert(chkListDwfLayers.CheckedIndices.Count == chkListDwfLayers.Items.Count);
            _dlayer.LayerFilter = GetLayerFilter();
        }
    }
}
