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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Editors.LayerDefinition.Vector.StyleEditors;
using OSGeo.MapGuide.MaestroAPI.Schema;
using Maestro.Editors.LayerDefinition.Vector.Scales;
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Editors.LayerDefinition.Vector.Thematics;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.LayerDefinition.Vector.GridEditor
{
    [ToolboxItem(false)]
    internal partial class RuleGridView : UserControl
    {
        public RuleGridView()
        {
            _init = true;
            InitializeComponent();
        }

        private void InitGrid(bool bComposite)
        {
            if (bComposite)
                _rules = new BindingList<IRuleModel>();
            else
                _rules = new BindingList<ILabeledRuleModel>();
            _rules.ListChanged += OnRuleListChanged;
            grdRules.DataSource = _rules;
            var col = grdRules.Columns["LegendLabel"];
            if (col != null)
                col.ReadOnly = false;
            grdRules.RowsAdded += OnGridRowsAdded;
        }

        void OnGridRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (_init)
                return;

            if (e.RowIndex >= 0)
            {
                var row = grdRules.Rows[e.RowIndex];
                var rule = (IRuleModel)row.DataBoundItem;
                //Need to sync back to session repo because the rule has been added to the in-memory resource,
                //but not to the session copy.
                _edSvc.SyncSessionCopy();
                UpdateRulePreviewAsync(rule);
            }

            btnExplodeTheme.Enabled = (grdRules.Rows.Count > 1);
        }

        private void OnRuleListChanged(object sender, ListChangedEventArgs e)
        {
            if (_init)
                return;

            switch(e.ListChangedType)
            {
                case ListChangedType.ItemChanged:
                    if (e.PropertyDescriptor.Name != "Style" && e.PropertyDescriptor.Name != "Label")
                        _edSvc.HasChanged();
                    break;
                case ListChangedType.ItemMoved:
                    break;
            }
        }

        private bool _init = false;
        private IBindingList _rules;
        private IVectorStyle _style;
        private IVectorScaleRange _parentScaleRange;
        private IEditorService _edSvc;
        private ILayerDefinition _editedLayer;

        public void Init(IEditorService edSvc, IVectorScaleRange parentRange, IVectorStyle style)
        {
            try
            {
                _init = true;
                _edSvc = edSvc;
                _style = style;
                InitGrid(style is ICompositeTypeStyle);
                _editedLayer = (ILayerDefinition)_edSvc.GetEditedResource();
                _parentScaleRange = parentRange;
                ReSyncRules(style);
            }
            finally
            {
                _init = false;
            }
        }

        private void ReSyncRules(IVectorStyle style)
        {
            _rules.Clear();
            if (style != null)
            {
                switch (style.StyleType)
                {
                    case StyleType.Point:
                        {
                            for (int i = 0; i < style.RuleCount; i++)
                            {
                                IPointRule pr = (IPointRule)style.GetRuleAt(i);
                                _rules.Add(new PointRuleModel(pr, i));
                            }
                        }
                        break;
                    case StyleType.Line:
                        {
                            for (int i = 0; i < style.RuleCount; i++)
                            {
                                ILineRule lr = (ILineRule)style.GetRuleAt(i);
                                _rules.Add(new LineRuleModel(lr, i));
                            }
                        }
                        break;
                    case StyleType.Area:
                        {
                            for (int i = 0; i < style.RuleCount; i++)
                            {
                                IAreaRule ar = (IAreaRule)style.GetRuleAt(i);
                                _rules.Add(new AreaRuleModel(ar, i));
                            }
                        }
                        break;
                }
            }
            else
            {
                _rules.Clear();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_rules.Count > 0)
            {
                GenerateStylePreviewsForVisibleRows(false);
            }
        }

        private IEnumerable<DataGridViewRow> GetVisibleRuleRows()
        {
            var visRowCount = grdRules.DisplayedRowCount(true);
            if (grdRules.FirstDisplayedCell == null || visRowCount == 0)
                yield break;
            var firstDisplayedRowIndex = grdRules.FirstDisplayedCell.RowIndex;
            var lastvibileRowIndex = (firstDisplayedRowIndex + visRowCount) - 1;
            for (int rowIndex = firstDisplayedRowIndex; rowIndex <= lastvibileRowIndex; rowIndex++)
            {
                var row = grdRules.Rows[rowIndex];
                var cells = row.Cells;
                foreach (DataGridViewCell cell in cells)
                {
                    if (cell.Displayed)
                    {
                        yield return row;
                        break;
                    }
                }
            }
        }

        private void btnRefreshStylePreviews_Click(object sender, EventArgs e)
        {
            GenerateStylePreviewsForVisibleRows(false);
        }

        private void GenerateStylePreviewsForVisibleRows(bool bOnlyForNewlyVisibleRows)
        {
            List<DataGridViewRow> visibleRows = new List<DataGridViewRow>(GetVisibleRuleRows());
            List<RuleModel> visibleRules = new List<RuleModel>();
            foreach (var row in visibleRows)
            {
                RuleModel rule = (RuleModel)row.DataBoundItem;
                //TODO: Only check for un-drawn rules based on bOnlyForNewlyVisibleRows. Right now it will re-draw all rules in view
                visibleRules.Add(rule);
            }

            UpdateRulePreviewsAsync(visibleRules);
        }

        private void UpdateRulePreviewsAsync(IEnumerable<IRuleModel> visibleRules)
        {
            int? styleType = null;
            switch (_style.StyleType)
            {
                case StyleType.Point:
                    styleType = 1;
                    break;
                case StyleType.Line:
                    styleType = 2;
                    break;
                case StyleType.Area:
                    styleType = 3;
                    break;
                case StyleType.Composite:
                    styleType = 4;
                    break;
            }
            var scale = 0.0;
            //The min scale is part of the scale range, max scale is not
            if (_parentScaleRange.MinScale.HasValue)
                scale = _parentScaleRange.MinScale.Value;
            var layerId = _edSvc.EditedResourceID;
            var editedRes = _edSvc.GetEditedResource();
            var conn = editedRes.CurrentConnection;
            var mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);

            BusyWaitDialog.Run(Strings.UpdatingStylePreviews, () =>
            { //Background thread worker
                var icons = new Dictionary<int, Image>();
                foreach (var rule in visibleRules)
                {
                    var img = mapSvc.GetLegendImage(scale, layerId, rule.Index, styleType.Value, 50, 16, "PNG");
                    icons[rule.Index] = img;
                }
                return icons;
            }, (res, ex) =>
            { //Run completion
                if (ex != null)
                {
                    ErrorDialog.Show(ex);
                }
                else
                {
                    if (res != null)
                    {
                        var icons = (Dictionary<int, Image>)res;
                        //We're back on the GUI thread now
                        foreach (var rule in visibleRules)
                        {
                            if (icons.ContainsKey(rule.Index))
                            {
                                rule.SetRuleStylePreview(icons[rule.Index]);
                            }
                        }
                    }
                }
            });
        }

        private void UpdateRulePreviewAsync(IRuleModel rule)
        {
            UpdateRulePreviewsAsync(new IRuleModel[] { rule });
        }

        private void grdRules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0) {
                var rule = (RuleModel)grdRules.Rows[e.RowIndex].DataBoundItem;
                var cell = grdRules.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var col = cell.OwningColumn;
                //NOTE: These are the property names of RuleModel. If those properties are ever changed be sure to update this as well
                switch (col.Name)
                {
                    case "Style":
                        EditRuleStyle(rule);
                        break;
                    case "Label":
                        EditLabelStyle(rule);
                        break;
                    case "LegendLabel":
                        grdRules.CurrentCell = cell;
                        grdRules.BeginEdit(true);
                        break;
                    case "Filter":
                        {
                            var expr = rule.Filter;
                            var newExpr = _edSvc.EditExpression(expr, GetLayerClass(), GetLayerProvider(), GetFeatureSource(), true);
                            if (newExpr != null)
                            {
                                rule.Filter = newExpr;
                            }
                        }
                        break;
                }
            }
        }

        private ITextSymbol m_origLabel;
        private ITextSymbol m_editLabel;

        private void EditLabelStyle(RuleModel rule)
        {
            PointRuleModel pr = rule as PointRuleModel;
            LineRuleModel lr = rule as LineRuleModel;
            AreaRuleModel ar = rule as AreaRuleModel;

            UserControl uc = null;
            /*
            if (m_owner.SelectedClass == null)
            {
                MessageBox.Show(Strings.NoFeatureClassAssigned);
                return;
            }*/
            var previewScale = 0.0;
            if (_parentScaleRange.MinScale.HasValue)
                previewScale = _parentScaleRange.MinScale.Value;
            ILayerStylePreviewable prev = new LayerStylePreviewable(_edSvc.EditedResourceID,
                                                                    previewScale,
                                                                    80,
                                                                    40,
                                                                    "PNG", //NOXLATE
                                                                    rule.Index);

            //TODO: This is obviously a mess and could do with some future cleanup, but the idea here should be
            //easy to understand. Each primitive basic style (that's not a label) has 3 actions.
            // - Commit (When user clicks OK on dialog)
            // - Rollback (When user clicks Cancel on dialog)
            // - Edit Commit (When user invokes refresh)
            //Those that support GETLEGENDIMAGE-based previews will be passed an edit commit action. Invoking the
            //edit commit action will update the session-based layer with this edit-copy rule, allowing for the changes
            //to be reflected when we do the GETLEGENDIMAGE call
            //
            //Labels are exempt as those previews can be sufficiently simulated with System.Drawing API
            var vl = (IVectorLayerDefinition)_editedLayer.SubLayer;
            ClassDefinition clsDef = GetLayerClass();
            Action commit = null;
            Action rollback = null;

            ITextSymbol labelToEdit = null;
            if (pr != null)
            {
                labelToEdit = pr.GetLabelStyle();
            }
            else if (lr != null)
            {
                labelToEdit = lr.GetLabelStyle();
            }
            else if (ar != null)
            {
                labelToEdit = ar.GetLabelStyle();
            }

            m_origLabel = labelToEdit;
            m_editLabel = (labelToEdit == null) ? null : (ITextSymbol)labelToEdit.Clone();

            uc = new FontStyleEditor(_edSvc, clsDef, vl.ResourceId);
            ((FontStyleEditor)uc).Item = m_editLabel;

            if (uc != null)
            {
                EditorTemplateForm dlg = new EditorTemplateForm();
                dlg.ItemPanel.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
                dlg.RefreshSize();
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    if (commit != null)
                    {
                        commit.Invoke();
                    }

                    ITextSymbol editedLabel = ((FontStyleEditor)uc).Item;
                    if (pr != null)
                    {
                        pr.SetLabelStyle(editedLabel);
                        _edSvc.HasChanged();
                    }
                    else if (lr != null)
                    {
                        lr.SetLabelStyle(editedLabel);
                        _edSvc.HasChanged();
                    }
                    else if (ar != null)
                    {
                        ar.SetLabelStyle(editedLabel);
                        _edSvc.HasChanged();
                    }
                }
                else if (res == DialogResult.Cancel)
                {
                    if (rollback != null)
                        rollback.Invoke();
                }
            }
        }

        private IPointSymbolization2D m_origPoint;
        private IPointSymbolization2D m_editPoint;

        private IAreaSymbolizationFill m_origArea;
        private IAreaSymbolizationFill m_editArea;

        private IEnumerable<IStroke> m_origLine;
        private IList<IStroke> m_editLine;

        private void EditRuleStyle(RuleModel rule)
        {
            PointRuleModel pr = rule as PointRuleModel;
            LineRuleModel lr = rule as LineRuleModel;
            AreaRuleModel ar = rule as AreaRuleModel;

            UserControl uc = null;
            /*
            if (m_owner.SelectedClass == null)
            {
                MessageBox.Show(Strings.NoFeatureClassAssigned);
                return;
            }*/
            var previewScale = 0.0;
            if (_parentScaleRange.MinScale.HasValue)
                previewScale = _parentScaleRange.MinScale.Value;
            ILayerStylePreviewable prev = new LayerStylePreviewable(_edSvc.EditedResourceID,
                                                                    previewScale,
                                                                    80,
                                                                    40,
                                                                    "PNG", //NOXLATE
                                                                    rule.Index);

            //TODO: This is obviously a mess and could do with some future cleanup, but the idea here should be
            //easy to understand. Each primitive basic style (that's not a label) has 3 actions.
            // - Commit (When user clicks OK on dialog)
            // - Rollback (When user clicks Cancel on dialog)
            // - Edit Commit (When user invokes refresh)
            //Those that support GETLEGENDIMAGE-based previews will be passed an edit commit action. Invoking the
            //edit commit action will update the session-based layer with this edit-copy rule, allowing for the changes
            //to be reflected when we do the GETLEGENDIMAGE call
            //
            //Labels are exempt as those previews can be sufficiently simulated with System.Drawing API
            var vl = (IVectorLayerDefinition)_editedLayer.SubLayer;
            ClassDefinition clsDef = GetLayerClass();
            Action commit = null;
            Action rollback = null;

            if (pr != null)
            {
                var sym = pr.GetSymbolizationStyle();

                m_origPoint = sym;
                m_editPoint = (sym == null) ? null : (IPointSymbolization2D)sym.Clone();

                var pfse = new PointFeatureStyleEditor(_edSvc, clsDef, vl.ResourceId, pr.Style, prev);
                uc = pfse;
                pfse.Item = m_editPoint;

                Action editCommit = () =>
                {
                    m_editPoint = pfse.Item;
                    pr.SetSymbolizationStyle(m_editPoint);
                };
                pfse.SetEditCommit(editCommit);
                commit = () =>
                {
                    pr.SetSymbolizationStyle(pfse.Item);
                    _edSvc.HasChanged();
                    UpdateRulePreviewAsync(pr);
                };
                rollback = () =>
                {
                    pr.SetSymbolizationStyle(m_origPoint);
                };
            }
            else if (lr != null)
            {
                var lineSym = lr.GetSymbolizationStyle();
                var strokes = lineSym.Strokes;
                m_origLine = strokes;
                m_editLine = (strokes == null) ? new List<IStroke>() : LayerElementCloningUtil.CloneStrokes(strokes);

                var lfse = new LineFeatureStyleEditor(_edSvc, clsDef, vl.ResourceId, _editedLayer, prev);
                uc = lfse;
                lfse.Item = m_editLine;

                Action editCommit = () =>
                {
                    m_editLine = lfse.Item;
                    lineSym.SetStrokes(m_editLine);
                };
                lfse.SetEditCommit(editCommit);
                commit = () =>
                {
                    lineSym.SetStrokes(lfse.Item);
                    _edSvc.HasChanged();
                    UpdateRulePreviewAsync(lr);
                };
                rollback = () =>
                {
                    lineSym.SetStrokes(m_origLine);
                };
            }
            else if (ar != null)
            {
                var area = ar.GetSymbolizationStyle();

                m_origArea = area;
                m_editArea = (area == null) ? null : (IAreaSymbolizationFill)area.Clone();

                var afse = new AreaFeatureStyleEditor(_edSvc, clsDef, vl.ResourceId, prev);
                uc = afse;
                afse.Item = m_editArea;

                Action editCommit = () =>
                {
                    m_editArea = afse.Item;
                    ar.SetSymbolizationStyle(m_editArea);
                };
                commit = () =>
                {
                    ar.SetSymbolizationStyle(afse.Item);
                    _edSvc.HasChanged();
                    UpdateRulePreviewAsync(ar);
                };
                rollback = () =>
                {
                    ar.SetSymbolizationStyle(m_origArea);
                };
                afse.SetEditCommit(editCommit);
            }

            if (uc != null)
            {
                EditorTemplateForm dlg = new EditorTemplateForm();
                dlg.ItemPanel.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
                dlg.RefreshSize();
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    if (commit != null)
                    {
                        commit.Invoke();
                    }

                    if (pr != null)
                    {
                        _edSvc.HasChanged();
                    }
                    else if (lr != null)
                    {
                        _edSvc.HasChanged();
                    }
                    else if (ar != null)
                    {
                        _edSvc.HasChanged();
                    }
                }
                else if (res == DialogResult.Cancel)
                {
                    if (rollback != null)
                        rollback.Invoke();
                }
            }
        }

        private ClassDefinition _layerClass;

        private ClassDefinition GetLayerClass()
        {
            if (_layerClass == null) 
            {
                IVectorLayerDefinition vl = (IVectorLayerDefinition)_editedLayer.SubLayer;
                _layerClass = _edSvc.FeatureService.GetClassDefinition(vl.ResourceId, vl.FeatureName);
            }
            return _layerClass;
        }

        private string GetFeatureSource()
        {
            IVectorLayerDefinition vl = (IVectorLayerDefinition)_editedLayer.SubLayer;
            return vl.ResourceId;
        }

        private string _providerName;

        private string GetLayerProvider()
        {
            if (_providerName == null)
            {
                IVectorLayerDefinition vl = (IVectorLayerDefinition)_editedLayer.SubLayer;
                IFeatureSource fs = (IFeatureSource)_edSvc.ResourceService.GetResource(vl.ResourceId);
                _providerName = fs.Provider;
            }
            return _providerName;
        }

        private void btnAddRule_Click(object sender, EventArgs e)
        {
            switch(_style.StyleType)
            {
                case StyleType.Point:
                    {
                        var rule = _editedLayer.CreateDefaultPointRule();
                        ((IPointVectorStyle)_style).AddRule(rule);
                        var model = new PointRuleModel(rule, _style.RuleCount - 1);
                        _rules.Add(model);
                    }
                    break;
                case StyleType.Line:
                    {
                        var rule = _editedLayer.CreateDefaultLineRule();
                        ((ILineVectorStyle)_style).AddRule(rule);
                        var model = new LineRuleModel(rule, _style.RuleCount - 1);
                        _rules.Add(model);
                    }
                    break;
                case StyleType.Area:
                    {
                        var rule = _editedLayer.CreateDefaultAreaRule();
                        ((IAreaVectorStyle)_style).AddRule(rule);
                        var model = new AreaRuleModel(rule, _style.RuleCount - 1);
                        _rules.Add(model);
                    }
                    break;
                case StyleType.Composite:
                    {
                        var rule = _editedLayer.CreateDefaultCompositeRule();
                        ((ICompositeTypeStyle)_style).AddCompositeRule(rule);
                        var model = new CompositeRuleModel(rule, _style.RuleCount - 1);
                        _rules.Add(model);
                    }
                    break;
            }
        }

        private void btnDeleteRule_Click(object sender, EventArgs e)
        {
            List<RuleModel> remove = new List<RuleModel>();
            foreach (DataGridViewRow row in grdRules.SelectedRows)
                remove.Add((RuleModel)row.DataBoundItem);

            if (remove.Count > 0)
            {
                try
                {
                    _init = true;
                    remove.Sort((a, b) =>
                    {
                        return a.Index.CompareTo(b.Index);
                    });
                    //Remove in reverse order
                    for (int i = remove.Count - 1; i >= 0; i--)
                    {
                        switch (_style.StyleType)
                        {
                            case StyleType.Point:
                                {
                                    var pts = ((IPointVectorStyle)_style);
                                    var pr = pts.GetRuleAt(remove[i].Index);
                                    pts.RemoveRule(pr);
                                }
                                break;
                            case StyleType.Line:
                                {
                                    var lts = ((ILineVectorStyle)_style);
                                    var lr = lts.GetRuleAt(remove[i].Index);
                                    lts.RemoveRule(lr);
                                }
                                break;
                            case StyleType.Area:
                                {
                                    var ats = ((IAreaVectorStyle)_style);
                                    var ar = ats.GetRuleAt(remove[i].Index);
                                    ats.RemoveRule(ar);
                                }
                                break;
                            case StyleType.Composite:
                                {
                                    var cts = ((ICompositeTypeStyle)_style);
                                    var cr = cts.GetRuleAt(remove[i].Index);
                                    cts.RemoveCompositeRule(cr);
                                }
                                break;
                        }
                    }
                }
                finally 
                {
                    //TODO: Yes, we're going thermonuclear, simply because the RuleModel's indexes may be out of sync as a result
                    //of removing rules, so we have to be sure these indexes are correct
                    ReSyncRules(_style);
                    _init = false;
                    GenerateStylePreviewsForVisibleRows(false);
                    _edSvc.HasChanged();
                }
            }
        }

        private void grdRules_Scroll(object sender, ScrollEventArgs e)
        {
            if (btnAutoRefresh.Checked)
            {
                GenerateStylePreviewsForVisibleRows(true);
            }
        }

        private void grdRules_SelectionChanged(object sender, EventArgs e)
        {
            btnDeleteRule.Enabled = (grdRules.SelectedRows.Count > 0);
        }

        private void btnCreateTheme_Click(object sender, EventArgs e)
        {
            ThemeCreator dlg = new ThemeCreator(_edSvc, _editedLayer, GetLayerClass(), _style);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _init = true;
                    ReSyncRules(_style);
                    _edSvc.SyncSessionCopy();
                    GenerateStylePreviewsForVisibleRows(false);
                }
                finally
                {
                    _init = false;
                    _edSvc.HasChanged();
                }
            }
        }

        private void btnExplodeTheme_Click(object sender, EventArgs e)
        {
            var diag = new ExplodeThemeDialog(_edSvc, _parentScaleRange, _style, _editedLayer);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var options = new ExplodeThemeOptions()
                {
                    ActiveStyle = _style,
                    FolderId = diag.CreateInFolder,
                    Layer = _editedLayer,
                    LayerNameFormat = diag.LayerNameFormat,
                    LayerPrefix = diag.LayerPrefix,
                    Range = _parentScaleRange
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
