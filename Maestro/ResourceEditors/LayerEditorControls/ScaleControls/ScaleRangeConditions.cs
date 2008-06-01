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

                if (m_owner != null && m_owner.Globalizor != null)
                {
                    DisplayPoints.Text = m_owner.Globalizor.Translate("Display points");
                    DisplayLines.Text = m_owner.Globalizor.Translate("Display lines");
                    DisplayAreas.Text = m_owner.Globalizor.Translate("Display areas");
                }
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
                if (DisplayPoints.Tag != null)
                {
                    m_vsc.Items.Add(DisplayPoints.Tag);
                    DisplayPoints.Tag = null;
                }
                else
                {
                    PointTypeStyleType pst = new PointTypeStyleType();
                    if (m_vsc.Items == null)
                        m_vsc.Items = new System.Collections.ArrayList();
                    m_vsc.Items.Add(pst);
                    pointConditionList.SetItem(m_vsc, pst);
                }
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
                if (DisplayLines.Tag != null)
                {
                    m_vsc.Items.Add(DisplayLines.Tag);
                    DisplayLines.Tag = null;
                }
                else
                {
                    LineTypeStyleType lst = new LineTypeStyleType();
                    if (m_vsc.Items == null)
                        m_vsc.Items = new System.Collections.ArrayList();
                    m_vsc.Items.Add(lst);
                    lineConditionList.SetItem(m_vsc, lst);
                }
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
                if (DisplayAreas.Tag != null)
                {
                    m_vsc.Items.Add(DisplayAreas.Tag);
                    DisplayAreas.Tag = null;
                }
                else
                {
                    AreaTypeStyleType ast = new AreaTypeStyleType();
                    if (m_vsc.Items == null)
                        m_vsc.Items = new System.Collections.ArrayList();
                    m_vsc.Items.Add(ast);
                    areaConditionList.SetItem(m_vsc, ast);
                }
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
