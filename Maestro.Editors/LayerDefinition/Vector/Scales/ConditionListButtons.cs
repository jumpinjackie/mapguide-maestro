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
using Maestro.Editors.LayerDefinition.Vector.StyleEditors;
using Maestro.Shared.UI;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    [ToolboxItem(false)]
    internal partial class ConditionListButtons : UserControl
    {
        private IPointVectorStyle m_point;
        private ILineVectorStyle m_line;
        private IAreaVectorStyle m_area;
        private ICompositeTypeStyle m_comp;

        private IVectorScaleRange m_parent;
        private object m_lastSelection;
        private object m_currentSelection;

        public event EventHandler ItemChanged;

        private VectorLayerEditorCtrl m_owner;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        public ConditionListButtons()
        {
            this.InitializeComponent();
            conditionList.SelectionChanged += new EventHandler(conditionList_SelectionChanged);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        public void SetItem(IVectorScaleRange parent, ICompositeTypeStyle comp)
        {
            SetItemInternal(parent, comp);
            conditionList.SetItem(parent, comp);
        }

        private void SetItemInternal(IVectorScaleRange parent, object item)
        {
            m_parent = parent;
            m_point = item as IPointVectorStyle;
            m_line = item as ILineVectorStyle;
            m_area = item as IAreaVectorStyle;
            m_comp = item as ICompositeTypeStyle;

            var ar2 = m_area as IAreaVectorStyle2;
            var pt2 = m_point as IPointVectorStyle2;
            var ln2 = m_line as ILineVectorStyle2;
            var cm2 = m_comp as ICompositeTypeStyle2;

            //Check if we're working with a 1.3.0 schema
            ShowInLegend.Enabled = (ar2 != null || pt2 != null || ln2 != null || cm2 != null);
            btnExplodeTheme.Enabled = (m_comp == null);
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
                int idx = m_point.RuleCount;
                conditionList.AddRuleControl(prt, idx).Focus();
                m_point.AddRule(prt);
            }
            else if (m_line != null)
            {
                ILineRule lrt = _factory.CreateDefaultLineRule();
                int idx = m_line.RuleCount;
                conditionList.AddRuleControl(lrt, idx).Focus();
                m_line.AddRule(lrt);
            }
            else if (m_area != null)
            {
                IAreaRule art = _factory.CreateDefaultAreaRule();
                int idx = m_area.RuleCount;
                conditionList.AddRuleControl(art, idx).Focus();
                m_area.AddRule(art);
            }
            else if (m_comp != null)
            {
                ICompositeRule cr = _factory.CreateDefaultCompositeRule();
                int idx = m_area.RuleCount;
                conditionList.AddRuleControl(cr, idx).Focus();
                m_comp.AddCompositeRule(cr);
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
                if (string.IsNullOrEmpty(vl.FeatureName))
                {
                    MessageBox.Show(Strings.NoFeatureClassAssigned);
                    return;
                }
                var cls = m_owner.Editor.FeatureService.GetClassDefinition(vl.ResourceId, vl.FeatureName);
                if (cls == null)
                {
                    MessageBox.Show(string.Format(Strings.FeatureClassNotFound, vl.FeatureName));
                    return;
                }
                ThemeCreator dlg = new ThemeCreator(
                    m_owner.Editor, 
                    layer,
                    m_owner.SelectedClass, 
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
                MessageBox.Show(this, string.Format(Strings.GenericError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void CopyRuleButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;

            if (conditionList.SelectedItem == null)
                return;

            object rule = Utility.XmlDeepCopy(conditionList.SelectedItem);

            int idx = -1;
            if (m_point != null)
            {
                idx = m_point.RuleCount;
                m_point.AddRule((IPointRule)rule);
            }
            else if (m_line != null)
            {
                idx = m_line.RuleCount;
                m_line.AddRule((ILineRule)rule);
            }
            else if (m_area != null)
            {
                idx = m_area.RuleCount;
                m_area.AddRule((IAreaRule)rule);
            }
            else
                return;

            conditionList.AddRuleControl(rule, idx).Focus();

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
            var ar2 = m_area as IAreaVectorStyle2;
            var pt2 = m_point as IPointVectorStyle2;
            var ln2 = m_line as ILineVectorStyle2;
            var cs2 = m_comp as ICompositeTypeStyle2;

            if (ar2 != null)
            {
                ar2.ShowInLegend = ShowInLegend.Checked;
                m_owner.FlagDirty();
            }
            else if (pt2 != null)
            {
                pt2.ShowInLegend = ShowInLegend.Checked;
                m_owner.FlagDirty();
            }
            else if (ln2 != null)
            {
                ln2.ShowInLegend = ShowInLegend.Checked;
                m_owner.FlagDirty();
            }
            else if (cs2 != null)
            {
                cs2.ShowInLegend = ShowInLegend.Checked;
                m_owner.FlagDirty();
            }
        }

        private void btnExplodeTheme_Click(object sender, EventArgs e)
        {
            var ed = m_owner.EditorService;
            var layer = ed.GetEditedResource() as ILayerDefinition;
            var style = m_point as IVectorStyle ?? m_line as IVectorStyle ?? m_area as IVectorStyle;
            var diag = new ExplodeThemeDialog(ed, m_parent, style, layer);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var options = new ExplodeThemeOptions()
                {
                    ActiveStyle = style,
                    FolderId = diag.CreateInFolder,
                    Layer = layer,
                    LayerNameFormat = diag.LayerNameFormat,
                    LayerPrefix = diag.LayerPrefix,
                    Range = m_parent
                };

                var progress = new ProgressDialog();
                var worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                progress.RunOperationAsync(null, ExplodeThemeWorker, options);
            }
        }

        private static object ExplodeThemeWorker(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
            var options = (ExplodeThemeOptions)args[0];
            LengthyOperationProgressCallBack cb = (s, cbArgs) =>
            {
                worker.ReportProgress(cbArgs.Progress, cbArgs.StatusMessage);
            };
            Utility.ExplodeThemeIntoFilteredLayers(options, cb);
            return true;
        }
    }
}
