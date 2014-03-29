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
using System.Diagnostics;

namespace Maestro.Editors.LayerDefinition.Vector.GridEditor
{
    internal partial class VectorScaleRangeGrid : UserControl
    {
        private VectorScaleRangeGrid()
        {
            InitializeComponent();
            _compositeRuleGrids = new Dictionary<ICompositeTypeStyle, RuleGridView>();
        }

        private bool _init = false;
        private VectorLayerStyleSectionCtrl _parent;
        private IVectorScaleRange _vsr;
        private IPointVectorStyle _pts;
        private ILineVectorStyle _lts;
        private IAreaVectorStyle _ats;
        private BindingList<ICompositeTypeStyle> _cts;
        private ILayerDefinition _editedLayer;

        public VectorScaleRangeGrid(IVectorScaleRange vsr, VectorLayerStyleSectionCtrl parent)
            : this()
        {
            try
            {
                _init = true;
                _parent = parent;
                _editedLayer = (ILayerDefinition)_parent.EditorService.GetEditedResource();
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
                    _cts = new BindingList<ICompositeTypeStyle>();
                    if (chkArea.Checked || chkLine.Checked || chkArea.Checked)
                    {
                        chkComposite.Checked = false;
                    }
                    else
                    {
                        foreach (var r in vsr2.CompositeStyle)
                        {
                            _cts.Add(r);
                        }
                        chkComposite.Checked = (vsr2.CompositeStyleCount > 0);
                    }
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

            SetCompositeUI();
        }

        private void SetCompositeUI()
        {
            var vsr2 = _vsr as IVectorScaleRange2;
            if (vsr2 == null) return;
            SetCompositeTabVisibility(chkComposite.Checked);
            vsr2.CompositeStyle = (chkComposite.Checked) ? _cts : null;

            lstStyles.DataSource = (chkComposite.Checked) ? _cts : null;

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var vsr2 = _vsr as IVectorScaleRange2;
            if (vsr2 != null)
            {
                _cts.Add(_editedLayer.CreateDefaultCompositeStyle());
                vsr2.CompositeStyle = _cts;
                _parent.RaiseResourceChanged();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var vsr2 = _vsr as IVectorScaleRange2;
            if (vsr2 != null)
            {
                var style = lstStyles.SelectedItem as ICompositeTypeStyle;
                if (style != null)
                {
                    _cts.Remove(style);
                    vsr2.CompositeStyle = _cts;
                    splitContainer1.Panel2.Controls.Clear();
                    _parent.RaiseResourceChanged();
                }
            }
        }

        private Dictionary<ICompositeTypeStyle, RuleGridView> _compositeRuleGrids;

        private void lstStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            var style = lstStyles.SelectedItem as ICompositeTypeStyle;
            if (style != null)
            {
                btnDelete.Enabled = true;
                if (!_compositeRuleGrids.ContainsKey(style))
                {
                    var grid = new RuleGridView();
                    grid.Init(_parent.EditorService, _vsr, style);
                    _compositeRuleGrids[style] = grid;
                }

                //Update offset
                if (lstStyles.SelectedIndex >= 0)
                {
                    int offset = 0;
                    for (int i = 0; i < lstStyles.SelectedIndex; i++)
                    {
                        offset += _cts[i].RuleCount;
                    }
                    _compositeRuleGrids[style].ThemeIndexOffest = offset;
                }

                SetActiveControl(_compositeRuleGrids[style]);
            }
        }

        private void SetActiveControl(RuleGridView activeGrid)
        {
            Debug.WriteLine(string.Format("Set active composite rule grid to: {0}", activeGrid.GetHashCode()));
            splitContainer1.Panel2.Controls.Clear();
            activeGrid.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(activeGrid);
        }
    }
}
