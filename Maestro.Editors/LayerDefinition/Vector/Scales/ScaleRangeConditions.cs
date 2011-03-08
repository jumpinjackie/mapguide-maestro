#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class ScaleRangeConditions: UserControl
    {
        public event EventHandler ItemChanged;

        private IVectorScaleRange m_vsc;
        private bool m_isUpdating = false;
        private bool m_hasUnsupportedItems = false;

        public bool HasUnsupportedItems { get { return m_hasUnsupportedItems; } }

        private VectorLayerEditorCtrl m_owner;

        public VectorLayerEditorCtrl Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                pointConditionList.Owner = m_owner;
                lineConditionList.Owner = m_owner;
                areaConditionList.Owner = m_owner;
            }
        }

        internal ScaleRangeConditions()
        {
            InitializeComponent();
        }

        private ILayerElementFactory _factory;

        internal ILayerElementFactory Factory
        {
            get { return _factory; }
            set
            {
                _factory = value;
                lineConditionList.Factory = value;
                pointConditionList.Factory = value;
                areaConditionList.Factory = value;
            }
        }

        public void SetItem(IVectorScaleRange vsc)
        {
            try
            {
                m_isUpdating = true;
                m_vsc = vsc;

                bool hasPoints = false;
                bool hasLines = false;
                bool hasAreas = false;
                m_hasUnsupportedItems = false;

                var ps = m_vsc.PointStyle;
                var ls = m_vsc.LineStyle;
                var ars = m_vsc.AreaStyle;
                if (ps != null)
                {
                    hasPoints = true;
                    pointConditionList.SetItem(m_vsc, ps);
                }
                if (ls != null)
                {
                    hasLines = true;
                    lineConditionList.SetItem(m_vsc, ls);
                }
                if (ars != null)
                {
                    hasAreas = true;
                    areaConditionList.SetItem(m_vsc, ars);
                }

                m_hasUnsupportedItems = !hasPoints && !hasLines && !hasAreas;

                DisplayPoints.Checked = hasPoints;
                DisplayLines.Checked = hasLines;
                DisplayAreas.Checked = hasAreas;
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private void DisplayPoints_CheckedChanged(object sender, EventArgs e)
        {
            pointConditionList.Visible = DisplayPoints.Checked;

            if (DisplayPoints.Checked)
            {
                if (DisplayPoints.Tag != null)
                {
                    m_vsc.PointStyle = (IPointVectorStyle)DisplayPoints.Tag;
                    DisplayPoints.Tag = null;
                }

                bool hasPoint = m_vsc.PointStyle != null;

                if (!hasPoint)
                {
                    var pst = _factory.CreateDefaultPointStyle();
                    m_vsc.PointStyle = pst;
                    pointConditionList.SetItem(m_vsc, pst);
                }

                pointConditionList.AddRule();

            }
            else
            {
                DisplayPoints.Tag = m_vsc.PointStyle;
                m_vsc.PointStyle = null;
            }

            if (!m_isUpdating)
                SignalItemChanged();
        }

        private void DisplayLines_CheckedChanged(object sender, EventArgs e)
        {
            lineConditionList.Visible = DisplayLines.Checked;

            if (DisplayLines.Checked)
            {
                if (DisplayLines.Tag != null)
                {
                    m_vsc.LineStyle = (ILineVectorStyle)DisplayLines.Tag;
                    DisplayLines.Tag = null;
                }

                bool hasLine = m_vsc.LineStyle != null;

                if (!hasLine)
                {
                    var ls = _factory.CreateDefaultLineStyle();
                    m_vsc.LineStyle = ls;
                    lineConditionList.SetItem(m_vsc, ls);
                }

                lineConditionList.AddRule();
            }
            else
            {
                DisplayLines.Tag = m_vsc.LineStyle;
                m_vsc.LineStyle = null;
            }

            if (!m_isUpdating)
                SignalItemChanged();
        }

        private void DisplayAreas_CheckedChanged(object sender, EventArgs e)
        {
            areaConditionList.Visible = DisplayAreas.Checked;

            if (DisplayAreas.Checked)
            {
                if (DisplayAreas.Tag != null)
                {
                    m_vsc.AreaStyle = (IAreaVectorStyle)DisplayAreas.Tag;
                    DisplayAreas.Tag = null;
                }

                bool hasArea = m_vsc.AreaStyle != null;

                if (!hasArea)
                {
                    var ast = _factory.CreateDefaultAreaStyle();
                    m_vsc.AreaStyle = ast;
                    areaConditionList.SetItem(m_vsc, ast);
                }

                areaConditionList.AddRule();
            }
            else
            {
                DisplayAreas.Tag = m_vsc.AreaStyle;
                m_vsc.AreaStyle = null;
            }

            if (!m_isUpdating)
                SignalItemChanged();
        }

        private void SignalItemChanged()
        {
            if (ItemChanged != null)
                ItemChanged(m_vsc, null);
        }

        private void pointConditionList_ItemChanged(object sender, EventArgs e)
        {
            SignalItemChanged();
        }

        private void lineConditionList_ItemChanged(object sender, EventArgs e)
        {
            SignalItemChanged();
        }

        private void areaConditionList_ItemChanged(object sender, EventArgs e)
        {
            SignalItemChanged();
        }

        public int GetPreferedHeight()
        {
            int minHeight = DisplayPoints.Height + DisplayLines.Height + DisplayAreas.Height + splitter1.Height + splitter2.Height;

            if (DisplayPoints.Checked)
                minHeight += pointConditionList.GetPreferedHeight();
            if (DisplayLines.Checked)
                minHeight += lineConditionList.GetPreferedHeight();
            if (DisplayAreas.Checked)
                minHeight += areaConditionList.GetPreferedHeight();

            return minHeight;
        }

        public void ResizeAuto()
        {
            pointConditionList.ResizeAuto();
            lineConditionList.ResizeAuto();
            areaConditionList.ResizeAuto();
            this.Height = this.GetPreferedHeight();
        }
    }
}
