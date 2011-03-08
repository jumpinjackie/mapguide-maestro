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
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Editors.LayerDefinition.Vector.Thematics;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class ConditionListButtons : UserControl
    {
        private IPointVectorStyle m_point;
        private ILineVectorStyle m_line;
        private IAreaVectorStyle m_area;

        private IVectorScaleRange m_parent;
        private object m_lastSelection;
        private object m_currentSelection;

        public event EventHandler ItemChanged;

        private VectorLayerEditorCtrl m_owner;

        public VectorLayerEditorCtrl Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                conditionList.Owner = m_owner;
            }
        }

        private ILayerElementFactory _factory;

        internal ConditionListButtons()
        {
            InitializeComponent();
            conditionList.SelectionChanged += new EventHandler(conditionList_SelectionChanged);
        }

        internal ILayerElementFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        private void conditionList_SelectionChanged(object sender, EventArgs e)
        {
            m_lastSelection = m_currentSelection;
            m_currentSelection = conditionList.SelectedItem;
            CopyRuleButton.Enabled = MoveRuleUpButton.Enabled = MoveRuleDownButton.Enabled = m_currentSelection != null || m_lastSelection != null;        
        }

        public void SetItem(IVectorScaleRange parent, IPointVectorStyle point)
        {
            SetItemInternal(parent, point);
            conditionList.SetItem(parent, point);
        }

        public void SetItem(IVectorScaleRange parent, ILineVectorStyle line)
        {
            SetItemInternal(parent, line);
            conditionList.SetItem(parent, line);
        }

        public void SetItem(IVectorScaleRange parent, IAreaVectorStyle area)
        {
            SetItemInternal(parent, area);
            conditionList.SetItem(parent, area);
        }


        private void SetItemInternal(IVectorScaleRange parent, object item)
        {
            m_parent = parent;
            m_point = item as IPointVectorStyle;
            m_line = item as ILineVectorStyle;
            m_area = item as IAreaVectorStyle;

        }

        public void AddRule()
        {
            AddRuleButton_Click(null, null);
        }

        private void AddRuleButton_Click(object sender, EventArgs e)
        {
            if (m_point != null)
            {
                IPointRule prt = _factory.CreateDefaultPointRule();
                conditionList.AddRuleControl(prt).Focus();
                m_point.AddRule(prt);
            }
            else if (m_line != null)
            {
                ILineRule lrt = _factory.CreateDefaultLineRule();
                conditionList.AddRuleControl(lrt).Focus();
                m_line.AddRule(lrt);
            }
            else if (m_area != null)
            {
                IAreaRule art = _factory.CreateDefaultAreaRule();
                conditionList.AddRuleControl(art).Focus();
                m_area.AddRule(art);

            }

            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void CreateThemeButton_Click(object sender, EventArgs e)
        {
            try
            {
                object owner = null;

                if (m_point != null)
                    owner = m_point;
                else if (m_line != null)
                    owner = m_line;
                else if (m_area != null)
                    owner = m_area;

                ILayerDefinition layer = (ILayerDefinition)m_owner.Editor.GetEditedResource();
                IVectorLayerDefinition vl = (IVectorLayerDefinition)layer.SubLayer;
                var cls = m_owner.Editor.FeatureService.GetClassDefinition(vl.ResourceId, vl.FeatureName);
                ThemeCreator dlg = new ThemeCreator(
                    m_owner.Editor, 
                    layer,
                    m_owner.Schema, 
                    owner);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var area = owner as IAreaVectorStyle;
                    var point = owner as IPointVectorStyle;
                    var line = owner as ILineVectorStyle;
                    if (area != null)
                        SetItem(m_parent, area);
                    else if (point != null)
                        SetItem(m_parent, point);
                    else if (line != null)
                        SetItem(m_parent, line);

                    m_owner.HasChanged();
                    m_owner.UpdateDisplay();
                }
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                m_owner.SetLastException(ex);
                MessageBox.Show(this, string.Format(Properties.Resources.GenericError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void CopyRuleButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;

            if (conditionList.SelectedItem == null)
                return;

            object rule = Utility.XmlDeepCopy(conditionList.SelectedItem);


            if (m_point != null)
            {
                m_point.AddRule((IPointRule)rule);
            }
            else if (m_line != null)
            {
                m_line.AddRule((ILineRule)rule);
            }
            else if (m_area != null)
            {
                m_area.AddRule((IAreaRule)rule);
            }
            else
                return;

            conditionList.AddRuleControl(rule).Focus();

            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void MoveRuleUpButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;
            conditionList.MoveSelectedRule(false);
            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void MoveRuleDownButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;
            conditionList.MoveSelectedRule(true);
            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void conditionList_ItemChanged(object sender, EventArgs e)
        {
            if (ItemChanged != null)
                ItemChanged(sender, null);
        }


        public void ResizeAuto()
        {
            this.Height = this.GetPreferedHeight();
        }

        public int GetPreferedHeight()
        {
            return panel1.Height + conditionList.GetPreferedHeight() + 24;
        }

        private void ShowInLegend_CheckedChanged(object sender, EventArgs e)
        {
            
        }

    }
}
