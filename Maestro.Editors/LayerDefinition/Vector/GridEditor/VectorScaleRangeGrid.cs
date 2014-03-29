#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.Editors.LayerDefinition.Vector.GridEditor
{
    internal partial class VectorScaleRangeGrid : UserControl
    {
        private VectorScaleRangeGrid()
        {
            InitializeComponent();
        }

        private bool _init = false;
        private VectorLayerStyleSectionCtrl _parent;
        private IVectorScaleRange _vsr;
        private IPointVectorStyle _pts;
        private ILineVectorStyle _lts;
        private IAreaVectorStyle _ats;
        private List<ICompositeTypeStyle> _cts;

        public VectorScaleRangeGrid(IVectorScaleRange vsr, VectorLayerStyleSectionCtrl parent)
            : this()
        {
            try
            {
                _init = true;
                _parent = parent;
                _vsr = vsr;
                _pts = _vsr.PointStyle;
                _lts = _vsr.LineStyle;
                _ats = _vsr.AreaStyle;

                chkPoints.Checked = (_pts != null);
                chkLine.Checked = (_lts != null);
                chkArea.Checked = (_ats != null);
                var vsr2 = _vsr as IVectorScaleRange2;
                if (vsr2 != null)
                {
                    _cts = new List<ICompositeTypeStyle>(vsr2.CompositeStyle);
                    chkComposite.Checked = (vsr2.CompositeStyleCount > 0);
                }
                else
                {
                    chkComposite.Visible = false;
                    SetCompositeTabVisibility(false);
                }

                SetPointUI();
                SetLineUI();
                SetAreaUI();
                SetCompositeUI();

                pointRuleGrid.Init(_parent.EditorService, _vsr, _vsr.PointStyle);
                lineRuleGrid.Init(_parent.EditorService, _vsr, _vsr.LineStyle);
                areaRuleGrid.Init(_parent.EditorService, _vsr, _vsr.AreaStyle);
            }
            finally
            {
                _init = false;
            }
        }

        private void chkPoints_CheckedChanged(object sender, EventArgs e)
        {
            if (_init) return;

            SetPointUI();
        }

        private void SetPointUI()
        {
            SetPointTabVisibility(chkPoints.Checked);
            _vsr.PointStyle = (chkPoints.Checked) ? _pts : null;
            pointRuleGrid.Init(_parent.EditorService, _vsr, _vsr.PointStyle);
            if (!_init)
                _parent.RaiseResourceChanged();
        }

        private void chkLine_CheckedChanged(object sender, EventArgs e)
        {
            if (_init) return;

            SetLineUI();
        }

        private void SetLineUI()
        {
            SetLineTabVisibility(chkLine.Checked);
            _vsr.LineStyle = (chkLine.Checked) ? _lts : null;
            lineRuleGrid.Init(_parent.EditorService, _vsr, _vsr.LineStyle);
            if (!_init)
                _parent.RaiseResourceChanged();
        }

        private void chkArea_CheckedChanged(object sender, EventArgs e)
        {
            if (_init) return;

            SetAreaUI();
        }

        private void SetAreaUI()
        {
            SetAreaTabVisibility(chkArea.Checked);
            _vsr.AreaStyle = (chkArea.Checked) ? _ats : null;
            areaRuleGrid.Init(_parent.EditorService, _vsr, _vsr.AreaStyle);
            if (!_init)
                _parent.RaiseResourceChanged();
        }

        private void SetPointTabVisibility(bool visible)
        {
            int idx = tabGeomStyles.TabPages.IndexOf(TAB_POINTS);
            if (visible)
            {
                if (idx < 0)
                    tabGeomStyles.TabPages.Insert(tabGeomStyles.TabPages.Count, TAB_POINTS);
            }
            else
            {
                if (idx >= 0)
                    tabGeomStyles.TabPages.RemoveAt(idx);
            }
        }

        private void chkComposite_CheckedChanged(object sender, EventArgs e)
        {
            if (_init) return;
        }

        private void SetCompositeUI()
        {
            var vsr2 = _vsr as IVectorScaleRange2;
            if (vsr2 == null) return;
            SetCompositeTabVisibility(chkComposite.Checked);
            vsr2.CompositeStyle = (chkComposite.Checked) ? _cts : null;

            if (!_init)
                _parent.RaiseResourceChanged();
        }

        private void SetCompositeTabVisibility(bool visible)
        {
            int idx = tabGeomStyles.TabPages.IndexOf(TAB_COMPOSITE);
            if (visible)
            {
                if (idx < 0)
                    tabGeomStyles.TabPages.Insert(tabGeomStyles.TabPages.Count, TAB_COMPOSITE);
            }
            else
            {
                if (idx >= 0)
                    tabGeomStyles.TabPages.RemoveAt(idx);
            }
        }

        private void SetLineTabVisibility(bool visible)
        {
            int idx = tabGeomStyles.TabPages.IndexOf(TAB_LINES);
            if (visible)
            {
                if (idx < 0)
                    tabGeomStyles.TabPages.Insert(tabGeomStyles.TabPages.Count, TAB_LINES);
            }
            else
            {
                if (idx >= 0)
                    tabGeomStyles.TabPages.RemoveAt(idx);
            }
        }

        private void SetAreaTabVisibility(bool visible)
        {
            int idx = tabGeomStyles.TabPages.IndexOf(TAB_AREAS);
            if (visible)
            {
                if (idx < 0)
                    tabGeomStyles.TabPages.Insert(tabGeomStyles.TabPages.Count, TAB_AREAS);
            }
            else
            {
                if (idx >= 0)
                    tabGeomStyles.TabPages.RemoveAt(idx);
            }
        }
    }
}
