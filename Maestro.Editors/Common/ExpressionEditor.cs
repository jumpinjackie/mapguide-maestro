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

#endregion Disclaimer / License

using ICSharpCode.TextEditor;
using Maestro.Editors.Common.Expression;
using Maestro.Editors.Generic.XmlEditor;
using Maestro.Editors.LayerDefinition.Vector.Thematics;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.FDO.Expressions;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// An expression editor dialog
    /// </summary>
    public partial class ExpressionEditor : Form, IExpressionEditor, ITextInserter, IExpressionErrorSource
    {
        private ClassDefinition _cls;
        private IEditorService _edSvc;
        private string m_featureSource = null;
        private IFdoProviderCapabilities _caps;
        private ITextEditor _editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEditor"/> class.
        /// </summary>
        internal ExpressionEditor()
        {
            InitializeComponent();
            ExpressionText.SetHighlighting("FDO");
            _editor = TextEditorFactory.CreateEditor(ExpressionText);
            _editor.KeyPress += OnEditorKeyPress;
            _editor.DialogKeyPress += OnEditorDialogKeyPress;
            _contextualBuffer = new StringBuilder();
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Form.Load event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            _editor.SetParent(ExpressionText);
            base.OnLoad(e);
        }

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>The expression.</value>
        public string Expression
        {
            get { return ExpressionText.Text; }
            set { ExpressionText.Text = value; }
        }

        private ExpressionEditorMode _mode;

        /// <summary>
        /// Initializes the dialog.
        /// </summary>
        /// <param name="edSvc">The editor service.</param>
        /// <param name="caps">The provider capabilities.</param>
        /// <param name="cls">The class definition.</param>
        /// <param name="featureSourceId">The FeatureSource id.</param>
        /// <param name="mode">The editor mode</param>
        /// <param name="attachStylizationFunctions">if set to <c>true</c> stylization functions are also attached</param>
        public void Initialize(IEditorService edSvc, IFdoProviderCapabilities caps, ClassDefinition cls, string featureSourceId, ExpressionEditorMode mode, bool attachStylizationFunctions)
        {
            try
            {
                _mode = mode;
                _cls = cls;
                _edSvc = edSvc;
                m_featureSource = featureSourceId;
                _caps = caps;

                btnTools.Enabled = attachStylizationFunctions;

                SortedList<string, PropertyDefinition> sortedCols = new SortedList<string, PropertyDefinition>();
                foreach (var col in _cls.Properties)
                {
                    sortedCols.Add(col.Name, col);
                }

                ColumnName.Items.Clear();
                ColumnName.Tag = sortedCols;

                foreach (var col in sortedCols.Values)
                {
                    string name = col.Name;
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = name;
                    btn.Text = name;
                    btn.Click += delegate
                    {
                        InsertText(name);
                    };
                    btnProperties.DropDown.Items.Add(btn);

                    ColumnName.Items.Add(name);
                }

                if (ColumnName.Items.Count > 0)
                    ColumnName.SelectedIndex = 0;

                LoadFunctions(caps, attachStylizationFunctions);
            }
            catch
            {
            }
        }

        internal static IFdoFunctionDefintionSignature[] MakeUniqueSignatures(IFdoFunctionDefintion func)
        {
            var dict = new Dictionary<string, IFdoFunctionDefintionSignature>();
            foreach (var sig in func.Signatures)
            {
                string fmt = "{0}({1})"; //NOXLATE
                List<string> args = new List<string>();
                foreach (var argDef in sig.Arguments)
                {
                    args.Add(argDef.Name.Trim());
                }
                string expr = string.Format(fmt, func.Name, FdoExpressionCompletionDataProvider.StringifyFunctionArgs(args));
                if (!dict.ContainsKey(expr))
                    dict[expr] = sig;
            }
            return dict.Values.ToArray();
        }

        private void LoadFunctions(IFdoProviderCapabilities caps, bool attachStylizationFunctions)
        {
            //Functions
            var sortedFuncs = new SortedList<string, IFdoFunctionDefintion>();
            foreach (var func in caps.Expression.SupportedFunctions)
            {
                sortedFuncs.Add(func.Name, func);
            }

            if (attachStylizationFunctions)
            {
                foreach (var func in Utility.GetStylizationFunctions())
                {
                    sortedFuncs.Add(func.Name, func);
                }
            }

            foreach (var func in sortedFuncs.Values)
            {
                string name = func.Name;
                string desc = func.Description;

                ToolStripItemCollection parent = btnFunctions.DropDown.Items;
                var sigs = MakeUniqueSignatures(func);
                if (sigs.Length > 1)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = string.Format(Strings.MultiSigFunction, name, sigs.Length);
                    btn.Text = string.Format(Strings.MultiSigFunction, name, sigs.Length);
                    btn.ToolTipText = desc;

                    btnFunctions.DropDown.Items.Add(btn);
                    parent = btn.DropDown.Items;
                }

                foreach (var sig in sigs)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = name;
                    btn.ToolTipText = desc;

                    string fmt = "{0}({1})"; //NOXLATE
                    List<string> args = new List<string>();
                    foreach (var argDef in sig.Arguments)
                    {
                        args.Add(argDef.Name.Trim());
                    }
                    string expr = string.Format(fmt, name, FdoExpressionCompletionDataProvider.StringifyFunctionArgs(args));
                    btn.Text = expr;
                    btn.Click += (s, e) =>
                    {
                        InsertText(expr);
                    };
                    parent.Add(btn);
                }
            }

            //Spatial Operators
            foreach (var op in caps.Filter.SpatialOperations)
            {
                string name = op.ToUpper();
                ToolStripButton btn = new ToolStripButton();
                btn.Name = btn.Text = btn.ToolTipText = op;
                btn.Click += (s, e) =>
                {
                    InsertSpatialFilter(name);
                };
                btnSpatial.DropDown.Items.Add(btn);
            }

            //Distance Operators
            foreach (var op in caps.Filter.DistanceOperations)
            {
                string name = op.ToUpper();
                ToolStripButton btn = new ToolStripButton();
                btn.Name = btn.Text = btn.ToolTipText = op;
                btn.Click += (s, e) =>
                {
                    InsertSpatialFilter(name);
                };
                btnDistance.DropDown.Items.Add(btn);
            }

            //Conditional Operators
            foreach (var op in caps.Filter.ConditionTypes)
            {
                string name = op.ToUpper();
                ToolStripButton btn = new ToolStripButton();
                btn.Name = btn.Text = btn.ToolTipText = op;
                btn.Click += (s, e) =>
                {
                    InsertSpatialFilter(name);
                };
                btnCondition.DropDown.Items.Add(btn);
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool OnEditorDialogKeyPress(Keys keyData)
        {
            if (_editor.ProcessKeyPress(keyData))
                return true;

            if (keyData == Keys.Back)
                StripKey();

            return false;
        }

        private bool OnEditorKeyPress(char ch)
        {
            if (Char.IsLetter(ch))
            {
                ShowAutoComplete(ch);
            }
            return false;
        }

        private StringBuilder _contextualBuffer;

        private void StripKey()
        {
            if (_contextualBuffer.Length == 0)
            {
                //this.HideBox();
            }
            else
            {
                _contextualBuffer.Remove(_contextualBuffer.Length - 1, 1);
                System.Diagnostics.Debug.WriteLine("Contextual buffer: " + _contextualBuffer);
                //if (_contextualBuffer.Length == 0)
                //this.HideBox();
            }
        }

        internal void AppendKey(Keys keyData)
        {
            _contextualBuffer.Append(Convert.ToChar((int)keyData));
            System.Diagnostics.Debug.WriteLine("Contextual buffer: " + _contextualBuffer);
        }

        private void ShowAutoComplete(char ch)
        {
            var provider = new FdoExpressionCompletionDataProvider(_cls, _caps);
            _editor.ShowCompletionWindow(provider, ch);
        }

        private void ColumnName_Click(object sender, EventArgs e)
        {
        }

        private void ColumnName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColumnValue.Enabled = false;
            LookupValues.Enabled = ColumnName.SelectedIndex >= 0;
        }

        private void LookupValues_Click(object sender, EventArgs e)
        {
            //Use UNIQUE() method first. This should work in most cases
            using (new WaitCursor(this))
            {
                string filter = null;
                var expr = "UNIQUE(" + ColumnName.Text + ")"; //NOXLATE
                bool bFallback = false;
                ColumnValue.Items.Clear();
                ColumnValue.Tag = null;
                try
                {
                    using (var rdr = _edSvc.CurrentConnection.FeatureService.AggregateQueryFeatureSource(m_featureSource, _cls.QualifiedName, filter, new System.Collections.Specialized.NameValueCollection() {
                            { "UNIQ_VALS", expr } //NOXLATE
                        }))
                    {
                        ColumnValue.Tag = rdr.GetPropertyType("UNIQ_VALS"); //NOXLATE
                        while (rdr.ReadNext())
                        {
                            if (!rdr.IsNull("UNIQ_VALS")) //NOXLATE
                            {
                                object value = rdr["UNIQ_VALS"]; //NOXLATE
                                ColumnValue.Items.Add(value);
                            }
                        }
                        rdr.Close();
                    }
                }
                catch
                {
                    ColumnValue.Items.Clear();
                    bFallback = true;
                }
                if (!bFallback)
                {
                    ColumnValue.Enabled = true;
                    ColumnValue.SelectedIndex = -1;
                    ColumnValue.DroppedDown = true;
                    return;
                }

                try
                {
                    SortedList<string, PropertyDefinition> cols = (SortedList<string, PropertyDefinition>)ColumnName.Tag;
                    PropertyDefinition col = cols[ColumnName.Text];

                    bool retry = true;
                    Exception rawEx = null;

                    SortedList<string, string> values = new SortedList<string, string>();
                    bool hasNull = false;

                    while (retry)
                    {
                        try
                        {
                            retry = false;
                            using (var rd = _edSvc.CurrentConnection.FeatureService.QueryFeatureSource(m_featureSource, _cls.QualifiedName, filter, new string[] { ColumnName.Text }))
                            {
                                while (rd.ReadNext())
                                {
                                    if (rd.IsNull(ColumnName.Text))
                                        hasNull = true;
                                    else
                                        values[Convert.ToString(rd[ColumnName.Text], System.Globalization.CultureInfo.InvariantCulture)] = null;
                                }
                                rd.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (filter == null && ex.Message.IndexOf("MgNullPropertyValueException") >= 0) //NOXLATE
                            {
                                hasNull = true;
                                rawEx = ex;
                                retry = true;
                                filter = ColumnName.Text + " != NULL"; //NOXLATE
                            }
                            else if (rawEx != null)
                                throw rawEx;
                            else
                                throw;
                        }
                    }

                    ColumnValue.Items.Clear();
                    if (hasNull)
                        ColumnValue.Items.Add("NULL"); //NOXLATE

                    foreach (string s in values.Keys)
                        ColumnValue.Items.Add(s);

                    ColumnValue.Tag = col.Type;

                    if (ColumnValue.Items.Count == 0)
                        MessageBox.Show(this, Strings.NoColumnValuesError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        ColumnValue.Enabled = true;
                        ColumnValue.SelectedIndex = -1;
                        ColumnValue.DroppedDown = true;
                    }
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    MessageBox.Show(this, string.Format(Strings.ColumnValueError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ColumnValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColumnValue.SelectedIndex >= 0)
            {
                var tag = ColumnValue.Tag;
                if (tag != null)
                {
                    if (ColumnValue.Tag == typeof(string) && (ColumnValue.SelectedIndex != 0 || ColumnValue.Text != "NULL")) //NOXLATE
                    {
                        InsertText("'" + ColumnValue.Text + "'"); //NOXLATE
                    }
                    else
                    {
                        if (tag is PropertyValueType && (PropertyValueType)tag == PropertyValueType.String)
                            InsertText("'" + ColumnValue.Text + "'"); //NOXLATE
                        else
                            InsertText(ColumnValue.Text);
                    }
                }
                else
                {
                    InsertText(ColumnValue.Text);
                }
            }
        }

        private void InsertSpatialFilter(string text)
        {
            InsertText("[geometry] " + text + " GeomFromText('geometry wkt')");
        }

        private void InsertText(string text)
        {
            ExpressionText.ActiveTextAreaControl.TextArea.InsertString(text);
        }

        private void insertThemeExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var theme = new ThemeCreator(_edSvc, _cls, this))
            {
                theme.ShowDialog();
            }
        }

        void ITextInserter.InsertText(string text)
        {
            this.InsertText(text);
        }

        private void insertARGBColorExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ColorDialog())
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var c = picker.Color;
                    this.InsertText(string.Format("ARGB({0}, {1}, {2}, {3})", c.A, c.R, c.G, c.B));
                }
            }
        }

        private void insertHTMLCOLORExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ColorDialog())
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var c = picker.Color;
                    this.InsertText(string.Format("HTMLCOLOR({0}, {1}, {2})", c.R, c.G, c.B));
                }
            }
        }

        private void buildAndInsertLOOKUPExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> propNames = new List<string>();
            foreach (var prop in _cls.Properties)
            {
                propNames.Add(prop.Name);
            }
            using (var picker = new LookupExpressionBuilder(propNames.ToArray()))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.InsertText(picker.GetExpression());
                }
            }
        }

        private void buildAndInsertRANGEExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> propNames = new List<string>();
            foreach (var prop in _cls.Properties)
            {
                propNames.Add(prop.Name);
            }
            using (var picker = new RangeExpressionBuilder(propNames.ToArray()))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.InsertText(picker.GetExpression());
                }
            }
        }

        void IExpressionErrorSource.SetCursor(int line, int col)
        {
            ExpressionText.ActiveTextAreaControl.Caret.Line = line;
            ExpressionText.ActiveTextAreaControl.Caret.Column = col;
        }

        void IExpressionErrorSource.HighlightToken(string token)
        {
            bool silent = false;
            if (string.IsNullOrEmpty(token))
            {
                if (!silent)
                    MessageBox.Show(Strings.TextNoStringSpecifiedToLookFor);
                return;
            }
            var search = new TextEditorSearcher();
            search.Document = ExpressionText.Document;
            search.LookFor = token; // txtLookFor.Text;
            search.MatchCase = false;
            search.MatchWholeWordOnly = true;

            bool bLoopedAround;
            TextRange range = search.FindNext(0, false, out bLoopedAround);
            if (range != null)
                SelectResult(range);
            else if (!silent)
                MessageBox.Show(Strings.TextNoStringSpecifiedToLookFor);
        }

        private void SelectResult(TextRange range)
        {
            TextLocation p1 = ExpressionText.Document.OffsetToPosition(range.Offset);
            TextLocation p2 = ExpressionText.Document.OffsetToPosition(range.Offset + range.Length);
            ExpressionText.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
            ExpressionText.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
            // Also move the caret to the end of the selection, because when the user
            // presses F3, the caret is where we start searching next time.
            ExpressionText.ActiveTextAreaControl.Caret.Position =
                ExpressionText.Document.OffsetToPosition(range.Offset + range.Length);
        }

        private FdoExpressionValidator _validator = new FdoExpressionValidator();

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mode == ExpressionEditorMode.Filter)
                {
                    FdoFilter filter = FdoFilter.Parse(ExpressionText.Text);
                    _validator.ValidateFilter(filter, _cls, _caps);
                    MessageBox.Show(Strings.FilterIsValid);
                }
                else //Expression
                {
                    FdoExpression expr = FdoExpression.Parse(ExpressionText.Text);
                    _validator.ValidateExpression(expr, _cls, _caps);
                    MessageBox.Show(Strings.ExprIsValid);
                }
            }
            catch (FdoExpressionValidationException ex)
            {
                new ExpressionParseErrorDialog(ex, this).ShowDialog();
            }
            catch (FdoMalformedExpressionException ex)
            {
                new MalformedExpressionDialog(ex, this).ShowDialog();
            }
        }

        private void viewParsedExpressionFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mode == ExpressionEditorMode.Filter)
                {
                    FdoFilter filter = FdoFilter.Parse(ExpressionText.Text);
                    new ExpressionDisplayDialog(filter).ShowDialog();
                }
                else //Expression
                {
                    FdoExpression expr = FdoExpression.Parse(ExpressionText.Text);
                    new ExpressionDisplayDialog(expr).ShowDialog();
                }
            }
            catch (FdoParseException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}