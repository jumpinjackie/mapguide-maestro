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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    [ToolboxItem(false)]
    internal partial class VectorScaleRangeCtrl : UserControl
    {
        private IPointVectorStyle _pts;
        private ILineVectorStyle _lns;
        private IAreaVectorStyle _ars;
        private BindingList<ICompositeTypeStyle> _cts;

        private VectorLayerStyleSectionCtrl _parent;

        private IVectorScaleRange _vsr;
        private bool _init;

        public VectorScaleRangeCtrl(IVectorScaleRange vsr, VectorLayerStyleSectionCtrl parent)
        {
            InitializeComponent();
            _init = true;

            try
            {
                _parent = parent;
                _vsr = vsr;

                _pts = vsr.PointStyle;
                _lns = vsr.LineStyle;
                _ars = vsr.AreaStyle;

                chkPoints.Checked = true;
                chkLine.Checked = true;
                chkArea.Checked = true;
                chkComposite.Checked = true;

                chkPoints.Checked = (_pts != null);
                chkLine.Checked = (_lns != null);
                chkArea.Checked = (_ars != null);

                pointList.Owner = parent.Owner;
                lineList.Owner = parent.Owner;
                areaList.Owner = parent.Owner;

                pointList.Factory = parent.Factory;
                lineList.Factory = parent.Factory;
                areaList.Factory = parent.Factory;

                if (_pts == null)
                    _pts = parent.Factory.CreateDefaultPointStyle();

                chkDisplayAsText.Checked = _pts.DisplayAsText;
                chkAllowOverpost.Checked = _pts.AllowOverpost;

                if (_lns == null)
                    _lns = parent.Factory.CreateDefaultLineStyle();

                if (_ars == null)
                    _ars = parent.Factory.CreateDefaultAreaStyle();

                pointList.SetItem(vsr, _pts);
                lineList.SetItem(vsr, _lns);
                areaList.SetItem(vsr, _ars);

                if (_pts.RuleCount == 0)
                    pointList.AddRule();

                if (_lns.RuleCount == 0)
                    lineList.AddRule();

                if (_ars.RuleCount == 0)
                    areaList.AddRule();

                var vsr2 = vsr as IVectorScaleRange2;
                if (vsr2 != null)
                {
                    _cts = new BindingList<ICompositeTypeStyle>();
                    foreach (var c in vsr2.CompositeStyle)
                        _cts.Add(c);

                    chkComposite.Checked = (_cts.Count > 0);
                    
                    compList.Owner = parent.Owner;
                    compList.Factory = parent.Factory;
                    if (_cts.Count == 0)
                        _cts.Add(parent.Factory.CreateDefaultCompositeStyle());

                    compList.LoadStyles(vsr2, _cts);
                }
                else
                {
                    chkComposite.Checked = false;
                }
                chkComposite.Enabled = (vsr2 != null);
            }
            finally
            {
                _init = false;
            }
        }

        private void chkDisplayAsText_CheckedChanged(object sender, EventArgs e)
        {
            if (_init) return;
            _pts.DisplayAsText = chkDisplayAsText.Checked;
            _parent.RaiseResourceChanged();
        }

        private void chkAllowOverpost_CheckedChanged(object sender, EventArgs e)
        {
            if (_init) return;
            _pts.AllowOverpost = chkAllowOverpost.Checked;
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

        private void chkPoints_CheckedChanged(object sender, EventArgs e)
        {
            SetPointTabVisibility(chkPoints.Checked);
            if (_init) return;

            _vsr.PointStyle = (chkPoints.Checked) ? _pts : null;
            _parent.RaiseResourceChanged();
        }

        private void chkLine_CheckedChanged(object sender, EventArgs e)
        {
            SetLineTabVisibility(chkLine.Checked);
            if (_init) return;

            _vsr.LineStyle = (chkLine.Checked) ? _lns : null;
            _parent.RaiseResourceChanged();
        }

        private void chkArea_CheckedChanged(object sender, EventArgs e)
        {
            SetAreaTabVisibility(chkArea.Checked);
            if (_init) return;

            _vsr.AreaStyle = (chkArea.Checked) ? _ars : null;
            _parent.RaiseResourceChanged();
        }

        private void chkComposite_CheckedChanged(object sender, EventArgs e)
        {
            SetCompositeTabVisibility(chkComposite.Checked);
            if (_init) return;
            var vsr2 = _vsr as IVectorScaleRange2;
            if (vsr2 == null) return;

            vsr2.CompositeStyle = (chkComposite.Checked) ? _cts : null;
            _parent.RaiseResourceChanged();
        }

        private void OnItemChanged(object sender, EventArgs e)
        {
            _parent.RaiseResourceChanged();
        }
    }
}
