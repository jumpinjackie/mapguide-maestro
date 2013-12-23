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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using Maestro.Editors.Common.Expression;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// An expression editor dialog
    /// </summary>
    public partial class ExpressionEditor : Form, IExpressionEditor
    {
        private ClassDefinition _cls;
        private IFeatureService _featSvc;
        private string m_featureSource = null;
        private FdoProviderCapabilities _caps;
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

        /// <summary>
        /// Initializes the dialog.
        /// </summary>
        /// <param name="featSvc">The feature service.</param>
        /// <param name="caps">The provider capabilities.</param>
        /// <param name="cls">The class definition.</param>
        /// <param name="featuresSourceId">The features source id.</param>
        /// <param name="attachStylizationFunctions">if set to <c>true</c> stylization functions are also attached</param>
        public void Initialize(IFeatureService featSvc, FdoProviderCapabilities caps, ClassDefinition cls, string featuresSourceId, bool attachStylizationFunctions)
        {
            try
            {
                _cls = cls;
                _featSvc = featSvc;
                m_featureSource = featuresSourceId;
                _caps = caps;

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

                //Functions
                SortedList<string, FdoProviderCapabilitiesExpressionFunctionDefinition> sortedFuncs = new SortedList<string, FdoProviderCapabilitiesExpressionFunctionDefinition>();
                foreach (FdoProviderCapabilitiesExpressionFunctionDefinition func in caps.Expression.FunctionDefinitionList)
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

                foreach (FdoProviderCapabilitiesExpressionFunctionDefinition func in sortedFuncs.Values)
                {
                    string name = func.Name;
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = name;
                    btn.Text = name;
                    btn.ToolTipText = func.Description;

                    string fmt = "{0}({1})"; //NOXLATE
                    List<string> args = new List<string>();
                    foreach (FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition argDef in func.ArgumentDefinitionList)
                    {
                        args.Add(argDef.Name.Trim());
                    }
                    string expr = string.Format(fmt, name, FdoExpressionCompletionDataProvider.StringifyFunctionArgs(args));
                    btn.Click += delegate
                    {
                        InsertText(expr);
                    };
                    btnFunctions.DropDown.Items.Add(btn);
                }

                //Spatial Operators
                foreach (FdoProviderCapabilitiesFilterOperation op in caps.Filter.Spatial)
                {
                    string name = op.ToString().ToUpper();
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = btn.Text = btn.ToolTipText = op.ToString();
                    btn.Click += delegate
                    {
                        InsertSpatialFilter(name);
                    };
                    btnSpatial.DropDown.Items.Add(btn);
                }

                //Distance Operators
                foreach (FdoProviderCapabilitiesFilterOperation1 op in caps.Filter.Distance)
                {
                    string name = op.ToString().ToUpper();
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = btn.Text = btn.ToolTipText = op.ToString();
                    btn.Click += delegate
                    {
                        InsertSpatialFilter(name);
                    };
                    btnDistance.DropDown.Items.Add(btn);
                }

                //Conditional Operators
                foreach (FdoProviderCapabilitiesFilterOperation op in caps.Filter.Condition)
                {
                    string name = op.ToString().ToUpper();
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = btn.Text = btn.ToolTipText = op.ToString();
                    btn.Click += delegate
                    {
                        InsertSpatialFilter(name);
                    };
                    btnCondition.DropDown.Items.Add(btn);
                }
            }
            catch
            {
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        bool OnEditorDialogKeyPress(Keys keyData)
        {
            if (_editor.ProcessKeyPress(keyData))
                return true;

            if (keyData == Keys.Back)
                StripKey();

            return false;
        }

        bool OnEditorKeyPress(char ch)
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
                    using (var rdr = _featSvc.AggregateQueryFeatureSource(m_featureSource, _cls.QualifiedName, filter, new System.Collections.Specialized.NameValueCollection() { 
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
                            using (var rd = _featSvc.QueryFeatureSource(m_featureSource, _cls.QualifiedName, filter, new string[] { ColumnName.Text }))
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
    }
}