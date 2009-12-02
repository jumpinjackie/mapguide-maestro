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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    public partial class FiniteDisplayScales : UserControl
    {
        private MaestroAPI.MapDefinition m_map;
        private EditorInterface m_editor;

        public FiniteDisplayScales()
        {
            InitializeComponent();
        }

        internal void SetItem(EditorInterface editor, MaestroAPI.MapDefinition map)
        {
            m_map = map;
            m_editor = editor;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            lstScales.Items.Clear();
            if (m_map.BaseMapDefinition != null && m_map.BaseMapDefinition.FiniteDisplayScale != null)
                foreach (double d in m_map.BaseMapDefinition.FiniteDisplayScale)
                    lstScales.Items.Add(d.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture));

            listviewCount.Text = string.Format(Strings.FiniteDisplayScales.ScaleCountLabel, lstScales.Items.Count);
        }

        private void linearGeneration_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (m_map.BaseMapDefinition == null)
                m_map.BaseMapDefinition = new OSGeo.MapGuide.MaestroAPI.MapDefinitionTypeBaseMapDefinition();
            if (m_map.BaseMapDefinition.FiniteDisplayScale == null)
                m_map.BaseMapDefinition.FiniteDisplayScale = new OSGeo.MapGuide.MaestroAPI.DoubleCollection();

            
            if (m_map.BaseMapDefinition.FiniteDisplayScale.Count != 0)
                if (MessageBox.Show(this, Strings.FiniteDisplayScales.OverwriteConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                    return;

            OSGeo.MapGuide.MaestroAPI.DoubleCollection vals = new OSGeo.MapGuide.MaestroAPI.DoubleCollection();
            if (linearGeneration.Checked)
            {
                double inc = (double)(maxScale.Value - minScale.Value) / (double)scaleCount.Value;
                double cur = (double)minScale.Value;
                for (int i = 0; i < scaleCount.Value; i++)
                {
                    vals.Add(cur);
                    cur += inc;
                }

                //In case the rounding sucks
                vals[(int)(scaleCount.Value - 1)] = (double)maxScale.Value;
            }
            else if (ExponentialGeneration.Checked)
            {
                double b = Math.Pow((double)(maxScale.Value - minScale.Value), 1 / (double)(scaleCount.Value));
                double cur = (double)minScale.Value;
                for (int i = 0; i < scaleCount.Value; i++)
                {
                    vals.Add(cur);
                    cur = ((double)maxScale.Value) / Math.Pow(b, (int)scaleCount.Value - i - 1) + (double)minScale.Value;
                }
                //m_map.BaseMapDefinition.FiniteDisplayScale[scaleCount.Value - 1] = maxScale;
            } 
            else
            {
                vals.Clear();
                double span = (double)maxScale.Value - (double)minScale.Value;
                double b = Math.Log10((double)maxScale.Value / (double)(scaleCount.Value - 1));

                for (int i = 0; i < scaleCount.Value; i++)
                    vals.Add(Math.Pow(i, b) + (double)minScale.Value);
            }

            if (RegularRounding.Checked || PrettyRounding.Checked)
                for (int i = 0; i < vals.Count; i++)
                    vals[i] = Math.Round(vals[i]);

            if (PrettyRounding.Checked)
                for (int i = 0; i < vals.Count; i++)
                {
                    int group = (int)Math.Floor(Math.Log10(vals[i]));
                    group--;

                    vals[i] = Math.Round(Math.Round((vals[i] / Math.Pow(10, group))) * Math.Pow(10, group));
                }

            m_map.BaseMapDefinition.FiniteDisplayScale = vals;
            m_editor.HasChanged();
            
            //Remove duplicates and sort
            SortCollection();
            UpdateDisplay();
        }

        private void scaleCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lstScales_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            double d;
            if (!double.TryParse(e.Label, System.Globalization.NumberStyles.Float, System.Threading.Thread.CurrentThread.CurrentUICulture, out d))
            {
                e.CancelEdit = true;
                return;
            }

            //e.Label = d.ToString(m_globalizor.Culture);
            m_editor.HasChanged();
            //SortCollection();
            //This crashes it
            //UpdateDisplay();
        }

        private void lstScales_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveScaleButton.Enabled = lstScales.SelectedItems.Count > 0;
        }

        private void RemoveScaleButton_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem lvi in lstScales.SelectedItems)
                for(int i = 0; i < m_map.BaseMapDefinition.FiniteDisplayScale.Count; i++)
                    if (m_map.BaseMapDefinition.FiniteDisplayScale[i].ToString(System.Threading.Thread.CurrentThread.CurrentUICulture) == lvi.Text)
                    {
                        m_editor.HasChanged();
                        m_map.BaseMapDefinition.FiniteDisplayScale.RemoveAt(i);
                        break;
                    }

            SortCollection();
            UpdateDisplay();
        }

        private void SortCollection()
        {
            SortedList<double, double> lst = new SortedList<double, double>();
            foreach (double d in m_map.BaseMapDefinition.FiniteDisplayScale)
                if (!lst.ContainsKey(d))
                    lst.Add(d, d);

            m_map.BaseMapDefinition.FiniteDisplayScale.Clear();
            foreach (double d in lst.Keys)
                m_map.BaseMapDefinition.FiniteDisplayScale.Add(d);

        }

        private void AddScaleButton_Click(object sender, EventArgs e)
        {
            if (m_map.BaseMapDefinition == null)
                m_map.BaseMapDefinition = new OSGeo.MapGuide.MaestroAPI.MapDefinitionTypeBaseMapDefinition();
            if (m_map.BaseMapDefinition.FiniteDisplayScale == null)
                m_map.BaseMapDefinition.FiniteDisplayScale = new OSGeo.MapGuide.MaestroAPI.DoubleCollection();

            double n;
            if (m_map.BaseMapDefinition.FiniteDisplayScale.Count == 0)
                n = 100;
            else
                n = m_map.BaseMapDefinition.FiniteDisplayScale[m_map.BaseMapDefinition.FiniteDisplayScale.Count - 1] * 2;

            ListViewItem lvi = lstScales.Items.Add(n.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture));
            lstScales.SelectedItems.Clear();
            lstScales.EnsureVisible(lvi.Index);
            lvi.Selected = true;
            lvi.BeginEdit();
        }

        private void RefreshScalesButton_Click(object sender, EventArgs e)
        {
            SortCollection();
            UpdateDisplay();
        }

        private void editScalesButton_Click(object sender, EventArgs e)
        {
            LayerEditorControls.EditScales dlg = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.EditScales();

            System.Text.StringBuilder sb = new StringBuilder();

            if (m_map.BaseMapDefinition != null && m_map.BaseMapDefinition.FiniteDisplayScale != null)
                foreach (double d in m_map.BaseMapDefinition.FiniteDisplayScale)
                    sb.Append(d.ToString(System.Globalization.CultureInfo.CurrentUICulture) + "\r\n");

            dlg.textBox1.Text = sb.ToString();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (m_map.BaseMapDefinition == null)
                    m_map.BaseMapDefinition = new OSGeo.MapGuide.MaestroAPI.MapDefinitionTypeBaseMapDefinition();
                m_map.BaseMapDefinition.FiniteDisplayScale = new OSGeo.MapGuide.MaestroAPI.DoubleCollection();

                using(System.IO.StringReader sr = new System.IO.StringReader(dlg.textBox1.Text))
                    while (sr.Peek() != -1)
                    {
                        string s = sr.ReadLine().Trim();
                        double d;
                        if (double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out d))
                            m_map.BaseMapDefinition.FiniteDisplayScale.Add(d);
                    }
                m_editor.HasChanged();
                SortCollection();
                UpdateDisplay();
            }
        }   
    }
}
