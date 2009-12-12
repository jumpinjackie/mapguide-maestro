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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
{
    public partial class ScaleRangeConditions: UserControl
    {
        public event EventHandler ItemChanged;

        private VectorScaleRangeType m_vsc;
        private bool m_isUpdating = false;
        private bool m_hasUnsupportedItems = false;

        public bool HasUnsupportedItems { get { return m_hasUnsupportedItems; } }

        private VectorLayer m_owner;

        public VectorLayer Owner
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

        public ScaleRangeConditions()
        {
            InitializeComponent();
        }

        public void SetItem(VectorScaleRangeType vsc)
        {
            try
            {
                m_isUpdating = true;
                m_vsc = vsc;

                bool hasPoints = false;
                bool hasLines = false;
                bool hasAreas = false;
                m_hasUnsupportedItems = false;
                if (m_vsc.Items != null)
                    foreach (object o in m_vsc.Items)
                    {
                        if (o as PointTypeStyleType != null)
                        {
                            hasPoints = true;
                            pointConditionList.SetItem(m_vsc, o as PointTypeStyleType);
                        }
                        else if (o as LineTypeStyleType != null)
                        {
                            hasLines = true;
                            lineConditionList.SetItem(m_vsc, o as LineTypeStyleType);
                        }
                        else if (o as AreaTypeStyleType != null)
                        {
                            hasAreas = true;
                            areaConditionList.SetItem(m_vsc, o as AreaTypeStyleType);
                        }
                        else
                            m_hasUnsupportedItems = true;
                    }

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
                if (m_vsc.Items == null)
                    m_vsc.Items = new System.Collections.ArrayList();

                if (DisplayPoints.Tag != null)
                {
                    m_vsc.Items.Add(DisplayPoints.Tag);
                    DisplayPoints.Tag = null;
                }

                bool hasPoint = false;
                foreach (object o in m_vsc.Items)
                    if (o as PointTypeStyleType != null)
                        hasPoint = true;

                if (!hasPoint)
                {
                    PointTypeStyleType pst = new PointTypeStyleType();
                    m_vsc.Items.Add(pst);
                    pointConditionList.SetItem(m_vsc, pst);
                }

                pointConditionList.AddRule();

            }
            else
            {
                DisplayPoints.Tag = null;
                if (m_vsc.Items != null)
                    foreach (object o in m_vsc.Items)
                        if (o as PointTypeStyleType != null)
                        {
                            DisplayPoints.Tag = o;
                            m_vsc.Items.Remove(o);
                            break;
                        }
            }

            if (!m_isUpdating)
                SignalItemChanged();
        }

        private void DisplayLines_CheckedChanged(object sender, EventArgs e)
        {
            lineConditionList.Visible = DisplayLines.Checked;

            if (DisplayLines.Checked)
            {
                if (m_vsc.Items == null)
                    m_vsc.Items = new System.Collections.ArrayList();

                if (DisplayLines.Tag != null)
                {
                    m_vsc.Items.Add(DisplayLines.Tag);
                    DisplayLines.Tag = null;
                }

                bool hasLine = false;
                foreach (object o in m_vsc.Items)
                    if (o as LineTypeStyleType != null)
                        hasLine = true;

                if (!hasLine)
                {
                    LineTypeStyleType lst = new LineTypeStyleType();
                    m_vsc.Items.Add(lst);
                    lineConditionList.SetItem(m_vsc, lst);
                }

                lineConditionList.AddRule();
            }
            else
            {
                DisplayLines.Tag = null;
                if (m_vsc.Items != null)
                    foreach (object o in m_vsc.Items)
                        if (o as LineTypeStyleType != null)
                        {
                            DisplayLines.Tag = o;
                            m_vsc.Items.Remove(o);
                            break;
                        }
            }

            if (!m_isUpdating)
                SignalItemChanged();
        }

        private void DisplayAreas_CheckedChanged(object sender, EventArgs e)
        {
            areaConditionList.Visible = DisplayAreas.Checked;

            if (DisplayAreas.Checked)
            {
                if (m_vsc.Items == null)
                    m_vsc.Items = new System.Collections.ArrayList();

                if (DisplayAreas.Tag != null)
                {
                    m_vsc.Items.Add(DisplayAreas.Tag);
                    DisplayAreas.Tag = null;
                }

                bool hasArea = false;
                foreach (object o in m_vsc.Items)
                    if (o as AreaTypeStyleType != null)
                        hasArea = true;

                if (!hasArea)
                {
                    AreaTypeStyleType ast = new AreaTypeStyleType();
                    m_vsc.Items.Add(ast);
                    areaConditionList.SetItem(m_vsc, ast);
                }

                areaConditionList.AddRule();
            }
            else
            {
                DisplayAreas.Tag = null;
                if (m_vsc.Items != null)
                    foreach (object o in m_vsc.Items)
                        if (o as AreaTypeStyleType != null)
                        {
                            DisplayAreas.Tag = o;
                            m_vsc.Items.Remove(o);
                            break;
                        }
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
