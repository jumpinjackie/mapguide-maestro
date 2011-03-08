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

namespace Maestro.Editors.LayerDefinition.Vector
{
    [ToolboxItem(true)]
    internal partial class VectorLayerStyleSectionCtrl : EditorBindableCollapsiblePanel
    {
        public VectorLayerStyleSectionCtrl()
        {
            InitializeComponent();
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

            scaleRangeList.SetItem(_vl);
            scaleRangeList.ResizeAuto();

            EvaluateCommands();
        }

        public VectorLayerEditorCtrl Owner
        {
            set { scaleRangeList.Owner = value; }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddScaleRange(_factory.CreateVectorScaleRange());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (scaleRangeList.SelectedItem == null)
                return;
            IVectorScaleRange vsc = scaleRangeList.SelectedItem;
            scaleRangeList.RemoveScaleRange(scaleRangeList.SelectedItem);
            _vl.RemoveVectorScaleRange(scaleRangeList.SelectedItem);

            _edsvc.HasChanged();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (scaleRangeList.SelectedItem == null)
                return;

            AddScaleRange(scaleRangeList.SelectedItem.Clone());
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
            scaleRangeList.SetItem(_vl);
            scaleRangeList.ResizeAuto();

            _edsvc.HasChanged();
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

        private void AddScaleRange(IVectorScaleRange vsc)
        {
            if (_vl == null)
                return;

            _vl.AddVectorScaleRange(vsc);
            Control c = scaleRangeList.AddScaleRange(vsc);
            scaleRangeList.ResizeAuto();
            _edsvc.HasChanged();

            try { c.Focus(); }
            catch { }
        }

        private void scaleRangeList_ItemChanged(object sender, EventArgs e)
        {
            scaleRangeList.ResizeAuto();
            _edsvc.HasChanged();
        }

        private void scaleRangeList_SelectionChanged(object sender, EventArgs e)
        {
            EvaluateCommands();
        }

        private void EvaluateCommands()
        {
            btnDelete.Enabled = btnDuplicate.Enabled = scaleRangeList.SelectedItem != null;
            btnSort.Enabled = _vl.HasVectorScaleRanges();
        }
    }
}
