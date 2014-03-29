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
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System.Collections;
using Maestro.Editors.Common;
using System.Globalization;
using Maestro.Editors.LayerDefinition.Vector.Scales;
using Maestro.Editors.LayerDefinition.Vector.StyleEditors;
using Maestro.Editors.LayerDefinition.Vector.GridEditor;

namespace Maestro.Editors.LayerDefinition.Vector
{
    [ToolboxItem(false)]
    //[ToolboxItem(true)]
    internal partial class VectorLayerStyleSectionCtrl : EditorBindableCollapsiblePanel
    {
        public VectorLayerStyleSectionCtrl()
        {
            InitializeComponent();
            cmbMinScale.Items.Add("0");
            cmbMaxScale.Items.Add(Strings.Infinity);
            lstScaleRanges.DataSource = _scales;
        }

        private ILayerElementFactory _factory;
        private IVectorLayerDefinition _vl;
        private IEditorService _edsvc;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);
            var res = (ILayerDefinition)service.GetEditedResource();
            _vl = (IVectorLayerDefinition)res.SubLayer;
            _factory = (ILayerElementFactory)res;
            BindScaleList(_vl.VectorScaleRange);
            EvaluateCommands();
        }

        internal IEditorService EditorService { get { return _edsvc; } }

        public VectorLayerEditorCtrl Owner { get; internal set; }

        public ILayerElementFactory Factory { get { return _factory; } }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddScaleRange(new VectorScaleRange() { Item = _factory.CreateVectorScaleRange() });
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var vsc = lstScaleRanges.SelectedItem as VectorScaleRange;
            if (vsc != null)
            {
                RemoveScaleRange(vsc);
            }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            var vsc = lstScaleRanges.SelectedItem as VectorScaleRange;
            if (vsc != null)
            {
                var clone = vsc.Clone();
                AddScaleRange(clone);
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (_vl == null)
                return;

            if (!_vl.HasVectorScaleRanges())
                return;

            List<IVectorScaleRange> ranges = new List<IVectorScaleRange>();
            foreach (IVectorScaleRange sc in _vl.VectorScaleRange)
            {
                ranges.Add(sc);
            }

            ranges.Sort(new ScaleRangeSorter());

            _vl.RemoveAllScaleRanges();
            foreach (IVectorScaleRange sc in ranges)
            {
                _vl.AddVectorScaleRange(sc);
            }

            //Refresh display
            BindScaleList(_vl.VectorScaleRange);

            _edsvc.HasChanged();
        }

        private BindingList<VectorScaleRange> _scales = new BindingList<VectorScaleRange>();

        private void BindScaleList(IEnumerable<IVectorScaleRange> scales)
        {
            _scales.Clear();
            foreach (var scale in scales)
            {
                _scales.Add(new VectorScaleRange() { Item = scale });
            }
        }

        class VectorScaleRange : INotifyPropertyChanged
        {
            public IVectorScaleRange Item { get; set; }

            public string ScaleDisplayString
            {
                get
                {
                    return string.Format("{0} : {1}",
                        this.Item.MinScale.HasValue ? this.Item.MinScale.Value.ToString(CultureInfo.InvariantCulture) : "0",
                        this.Item.MaxScale.HasValue ? this.Item.MaxScale.Value.ToString(CultureInfo.InvariantCulture) : Strings.Infinity);
                }
            }

            public bool SupportsElevation
            {
                get
                {
                    var it = this.Item as IVectorScaleRange2;
                    return (it != null);
                }
            }

            public double? MinScale
            {
                get
                {
                    return this.Item.MinScale;
                }
                set
                {
                    this.Item.MinScale = value;
                    OnPropertyChanged("ScaleDisplayString");
                }
            }

            public double? MaxScale
            {
                get
                {
                    return this.Item.MaxScale;
                }
                set
                {
                    this.Item.MaxScale = value;
                    OnPropertyChanged("ScaleDisplayString");
                }
            }

            internal VectorScaleRange Clone()
            {
                return new VectorScaleRange() { Item = this.Item.Clone() };
            }

            private void OnPropertyChanged(string name)
            {
                var handler = this.PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(name));
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        /// <summary>
        /// Sort helper used to sort the scale ranges
        /// </summary>
        private class ScaleRangeSorter : IComparer, IComparer<IVectorScaleRange>
        {
            public int Compare(object x, object y)
            {
                if (x is IVectorScaleRange && y is IVectorScaleRange)
                {
                    IVectorScaleRange vx = (IVectorScaleRange)x;
                    IVectorScaleRange vy = (IVectorScaleRange)y;

                    double minX = vx.MinScale.HasValue ? vx.MinScale.Value : 0;
                    double maxX = vx.MaxScale.HasValue ? vx.MaxScale.Value : double.MaxValue;
                    double minY = vy.MinScale.HasValue ? vy.MinScale.Value : 0;
                    double maxY = vy.MaxScale.HasValue ? vy.MaxScale.Value : double.MaxValue;

                    if (minX == minY)
                        if (maxX == maxY)
                            return 0;
                        else
                            return maxX > maxY ? 1 : -1;
                    else
                        return minX > minY ? 1 : -1;
                }
                else
                    return 0;
            }

            public int Compare(IVectorScaleRange x, IVectorScaleRange y)
            {
                return this.Compare((object)x, (object)y);
            }
        }

        private void RemoveScaleRange(VectorScaleRange vsc)
        {
            _scales.Remove(vsc);
            _vl.RemoveVectorScaleRange(vsc.Item);
            if (_rangeCtrls.ContainsKey(vsc))
            {
                _rangeCtrls[vsc].Dispose();
                _rangeCtrls.Remove(vsc);
            }
        }

        private void AddScaleRange(VectorScaleRange vsc)
        {
            _scales.Add(vsc);
            _vl.AddVectorScaleRange(vsc.Item);
            
        }

        private void EvaluateCommands()
        {
            btnDelete.Enabled = btnDuplicate.Enabled = lstScaleRanges.SelectedItem != null;
            btnSort.Enabled = _vl.HasVectorScaleRanges();
        }

        private Dictionary<VectorScaleRange, Control> _rangeCtrls = new Dictionary<VectorScaleRange, Control>();

        private Control _activeScaleRangeCtrl;

        private void lstScaleRanges_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vsc = lstScaleRanges.SelectedItem as VectorScaleRange;
            if (vsc != null)
            {
                try
                {
                    _update = true;

                    if (vsc.Item.MinScale.HasValue)
                    {
                        cmbMinScale.Text = vsc.Item.MinScale.Value.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        cmbMinScale.SelectedIndex = 0;
                        cmbMinScale.Text = cmbMinScale.SelectedItem.ToString();
                    }

                    if (vsc.Item.MaxScale.HasValue)
                    {
                        cmbMaxScale.Text = vsc.Item.MaxScale.Value.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        cmbMaxScale.SelectedIndex = 0;
                        cmbMaxScale.Text = cmbMaxScale.SelectedItem.ToString();
                    }

                    grpScaleRange.Text = string.Format("{0} ({1})", Strings.ScaleRange, vsc.ScaleDisplayString);
                    grpScaleRange.Controls.Clear();

                    btnKmlElevation.Enabled = vsc.SupportsElevation;

                    Control ctrl = null;
                    if (!_rangeCtrls.ContainsKey(vsc))
                    {
                        if (LayerEditorSettings.UseGridEditor)
                            ctrl = new VectorScaleRangeGrid(vsc.Item, this);
                        else
                            ctrl = new VectorScaleRangeCtrl(vsc.Item, this);
                        ctrl.Dock = DockStyle.Fill;
                        _rangeCtrls[vsc] = ctrl;
                    }
                    else
                    {
                        ctrl = _rangeCtrls[vsc];
                    }
                    grpScaleRange.Controls.Add(ctrl);
                    _activeScaleRangeCtrl = ctrl;
                }
                finally
                {
                    _update = false;
                }
            }
        }

        private bool _update = false;

        private void cmbMinScale_TextChanged(object sender, EventArgs e)
        {
            if (_update) return;
            errorProvider.Clear();

            var vsc = lstScaleRanges.SelectedItem as VectorScaleRange;
            if (vsc != null)
            {
                if (cmbMinScale.Text == "0")
                {
                    vsc.MinScale = null;
                }
                else
                {
                    double o;
                    if (double.TryParse(cmbMinScale.Text, System.Globalization.NumberStyles.Number, System.Threading.Thread.CurrentThread.CurrentUICulture, out o))
                    {
                        vsc.MinScale = o;
                        errorProvider.SetError(cmbMinScale, null);
                    }
                    else
                    {
                        errorProvider.SetError(cmbMinScale, Strings.InvalidValueError);
                    }
                }
                OnResourceChanged();
            }
        }

        private void cmbMaxScale_TextChanged(object sender, EventArgs e)
        {
            if (_update) return;
            errorProvider.Clear();

            var vsc = lstScaleRanges.SelectedItem as VectorScaleRange;
            if (vsc != null)
            {
                if (cmbMaxScale.Text == Strings.Infinity)
                {
                    vsc.MaxScale = null;
                }
                else
                {
                    double o;
                    if (double.TryParse(cmbMaxScale.Text, System.Globalization.NumberStyles.Number, System.Threading.Thread.CurrentThread.CurrentUICulture, out o))
                    {
                        vsc.MaxScale = o;
                        errorProvider.SetError(cmbMaxScale, null);
                    }
                    else
                    {
                        errorProvider.SetError(cmbMaxScale, Strings.InvalidValueError);
                    }
                }
                OnResourceChanged();
            }
        }

        private void btnKmlElevation_Click(object sender, EventArgs e)
        {
            var vsc = lstScaleRanges.SelectedItem as VectorScaleRange;
            if (vsc != null && vsc.SupportsElevation)
            {
                if (new ElevationDialog(_edsvc, (IVectorScaleRange2)vsc.Item, Owner.FeatureSourceId, Owner.SelectedClass, Owner.GetFdoProvider()).ShowDialog() == DialogResult.OK)
                {
                    OnResourceChanged();
                }
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
