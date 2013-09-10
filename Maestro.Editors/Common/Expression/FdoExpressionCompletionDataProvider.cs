#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Maestro.Editors.Common.Expression
{
    //NOTE:
    //
    //It seems the auto-complete capabilities of the ICSharpCode.TextEditor assume object-oriented languages or languages
    //that involve a member access operator (., -> or anything similar), so we have to use a custom ICompletionData that
    //compensates for the lack of such contexts
    //
    //NOTE/TODO:
    //Auto-completions are currently case-sensitive and will only trigger on the correct case.

    internal class FdoExpressionCompletionDataProvider : ICompletionDataProvider
    {
        private ClassDefinition _klass;
        private FdoProviderCapabilities _caps;

        public FdoExpressionCompletionDataProvider(ClassDefinition cls, FdoProviderCapabilities caps)
        {
            _klass = cls;
            _caps = caps;
            this.DefaultIndex = 0;
            this.PreSelection = null;
            this.ImageList = new System.Windows.Forms.ImageList();
            this.ImageList.Images.Add(Properties.Resources.block);
            this.ImageList.Images.Add(Properties.Resources.property);
            this.ImageList.Images.Add(Properties.Resources.funnel);
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get;
            private set;
        }

        public string PreSelection
        {
            get;
            private set;
        }

        public int DefaultIndex
        {
            get;
            private set;
        }

        public bool InsertSpace
        {
            get;
            set;
        }

        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            CompletionDataProviderKeyResult res;
            if (key == ' ' && this.InsertSpace)
            {
                this.InsertSpace = false; // insert space only once
                res = CompletionDataProviderKeyResult.BeforeStartKey;
            }
            else if (char.IsLetterOrDigit(key) || key == '_')
            {
                this.InsertSpace = false; // don't insert space if user types normally
                res = CompletionDataProviderKeyResult.NormalKey;
            }
            else
            {
                // do not reset insertSpace when doing an insertion!
                res = CompletionDataProviderKeyResult.InsertionKey;
            }
            return res;
        }

        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            if (this.InsertSpace)
            {
                textArea.Document.Insert(insertionOffset++, " ");
            }
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);

            var res = data.InsertAction(textArea, key);
            var fdoComp = (FdoCompletionData)data;
            if (fdoComp.ImageIndex == 0 && fdoComp.AppendText.Length > 2) //Function and not an empty function call
            {
                //Rewind caret so it is at the start of the function call (at first parameter)
                var offset = textArea.Caret.Offset;
                offset -= (fdoComp.AppendText.Length - 1);
                textArea.Caret.Position = textArea.Document.OffsetToPosition(offset);
                textArea.SelectionManager.ClearSelection();
                textArea.SelectionManager.SetSelection(textArea.Caret.Position, textArea.Document.OffsetToPosition(offset + (fdoComp.HighlightLength - 1)));
            }
            return res;
        }

        /// <summary>
        /// Gets the line of text up to the cursor position.
        /// </summary>
        string GetLineText(TextArea textArea)
        {
            LineSegment lineSegment = textArea.Document.GetLineSegmentForOffset(textArea.Caret.Offset);
            return textArea.Document.GetText(lineSegment);
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            return GenerateCompletionData((GetLineText(textArea) + charTyped).Trim());
        }

        class FdoCompletionData : DefaultCompletionData
        {
            private string _insertText;
            private string _appendText;
            private int _highlightLength = 0;

            public int HighlightLength { get { return _highlightLength; } }

            public string InsertText { get { return _insertText; } }

            public string AppendText { get { return _appendText; } }

            public FdoCompletionData(string prefix, string text, string description, int imageIndex)
                : base(text, description, imageIndex)
            {
                _insertText = text.Substring(prefix.Length);
                _appendText = string.Empty;
            }

            public FdoCompletionData(string prefix, string text, string description, string appendText, int highlightLength, int imageIndex)
                : this(prefix, text, description, imageIndex)
            {
                _appendText = appendText;
                _highlightLength = highlightLength;
            }

            public override bool InsertAction(TextArea textArea, char ch)
            {
                textArea.InsertString(_insertText + _appendText);
                return false;
            }
        }

        private ICompletionData[] GenerateCompletionData(string line)
        {
            Debug.WriteLine("FDO auto-complete: " + line);
            List<DefaultCompletionData> items = new List<DefaultCompletionData>();
            string name = GetName(line);
            if (!String.IsNullOrEmpty(name))
            {
                try
                {
                    foreach (var func in GetMatchingFdoFunctions(name))
                    {
                        var member = CreateFdoFunctionDescriptor(func);
                        int highlightLength = 0;
                        if (func.ArgumentDefinitionList.Count > 0)
                        {
                            highlightLength = func.ArgumentDefinitionList[0].Name.Length + 2; // [ and ]
                        }
                        items.Add(new FdoCompletionData(name, member.Name, member.Description, member.AppendText, highlightLength, 0));
                    }
                    foreach (var member in GetMatchingClassProperties(name))
                    {
                        items.Add(new FdoCompletionData(name, member.Name, member.Description, 1));
                    }
                    foreach (var member in GetMatchingFdoConditions(name))
                    {
                        if (string.IsNullOrEmpty(member.AppendText))
                            items.Add(new FdoCompletionData(name, member.Name, member.Description, 2));
                        else
                            items.Add(new FdoCompletionData(name, member.Name, member.Description, member.AppendText, member.AppendText.Length - 1, 2));
                    }
                    foreach (var member in GetMatchingFdoOperators(name))
                    {
                        if (string.IsNullOrEmpty(member.AppendText))
                            items.Add(new FdoCompletionData(name, member.Name, member.Description, 2));
                        else
                            items.Add(new FdoCompletionData(name, member.Name, member.Description, member.AppendText, 0, 2));
                    }
                    items.Sort((a, b) => { return a.Text.CompareTo(b.Text); });
                }
                catch
                {
                    // Do nothing.
                }
            }
            return items.ToArray();
        }

        class Descriptor
        {
            public string Name;
            public string Description;
            public string AppendText;
        }

        private IEnumerable<FdoProviderCapabilitiesExpressionFunctionDefinition> GetMatchingFdoFunctions(string name)
        {
            foreach (var func in _caps.Expression.FunctionDefinitionList.Concat(Utility.GetStylizationFunctions()))
            {
                if (func.Name.StartsWith(name))
                    yield return func;
            }
        }

        private IEnumerable<Descriptor> GetMatchingFdoConditions(string name)
        {
            foreach (var cond in _caps.Filter.Condition)
            {
                if (cond.ToString().ToUpper().StartsWith(name))
                {
                    var desc = CreateFdoConditionDescriptor(cond);
                    if (desc != null)
                        yield return desc;
                }
            }
        }

        private Descriptor CreateFdoConditionDescriptor(FdoProviderCapabilitiesFilterType cond)
        {
            if (cond == FdoProviderCapabilitiesFilterType.Null)
            {
                return new Descriptor()
                {
                    Name = cond.ToString().ToUpper(),
                    Description = "[property] NULL" //NOXLATE
                };
            }
            else if (cond == FdoProviderCapabilitiesFilterType.In)
            {
                return new Descriptor()
                {
                    Name = cond.ToString().ToUpper(),
                    Description = "[property] IN ([value1], [value2], ..., [valueN])", //NOXLATE
                    AppendText = " ([value1], [value2])" //NOXLATE
                };
            }
            else if (cond == FdoProviderCapabilitiesFilterType.Like)
            {
                return new Descriptor()
                {
                    Name = cond.ToString().ToUpper(),
                    Description = "[property] LIKE [string value]", //NOXLATE
                    AppendText = " [string value]" //NOXLATE
                };
            }
            return null; //Handled by operators
        }

        private static Descriptor CreateBinaryDistanceOperator(string opName)
        {
            return new Descriptor()
            {
                Name = opName.ToUpper(),
                Description = "[property] " + opName + " [number]", //NOXLATE
                AppendText = " [number]" //NOXLATE
            };
        }

        private static Descriptor CreateBinarySpatialOperator(string opName)
        {
            return new Descriptor()
            {
                Name = opName.ToUpper(),
                Description = "[geometry] " + opName + " GeomFromText('geometry wkt')", //NOXLATE
                AppendText = " GeomFromText('geometry wkt')" //NOXLATE
            };
        }

        private IEnumerable<Descriptor> GetMatchingFdoOperators(string name)
        {
            foreach (var op in _caps.Filter.Distance)
            {
                var opName = op.ToString().ToUpper();
                if (opName.StartsWith(name))
                    yield return CreateBinaryDistanceOperator(opName);
            }
            foreach (var op in _caps.Filter.Spatial)
            {
                var opName = op.ToString().ToUpper();
                if (opName.StartsWith(name))
                    yield return CreateBinarySpatialOperator(opName);
            }
        }

        private IEnumerable<Descriptor> GetMatchingClassProperties(string name)
        {
            foreach (var prop in _klass.Properties)
            {
                if (prop.Name.StartsWith(name))
                    yield return CreatePropertyDescriptor(prop);
            }
        }

        private Descriptor CreateFdoFunctionDescriptor(FdoProviderCapabilitiesExpressionFunctionDefinition func)
        {
            var desc = new Descriptor();
            desc.Name = func.Name;
            string fmt = "{0}({1})"; //NOXLATE
            List<string> args = new List<string>();
            foreach (FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition argDef in func.ArgumentDefinitionList)
            {
                args.Add(argDef.Name.Trim());
            }
            string argsStr = StringifyFunctionArgs(args);
            string expr = string.Format(fmt, func.Name, argsStr); //NOXLATE
            desc.Description = string.Format(Strings.ExprEditorFunctionDesc, expr, func.Description, func.ReturnType, Environment.NewLine);
            desc.AppendText = "(" + argsStr + ")";
            return desc;
        }

        internal static string StringifyFunctionArgs(List<string> args)
        {
            string argsStr = args.Count > 0 ? "[" + string.Join("], [", args.ToArray()) + "]" : string.Empty; //NOXLATE
            return argsStr;
        }

        private static Descriptor CreatePropertyDescriptor(PropertyDefinition prop)
        {
            var desc = new Descriptor();
            desc.Name = prop.Name;

            if (prop.Type == PropertyDefinitionType.Geometry)
            {
                var g = (GeometricPropertyDefinition)prop;
                desc.Description = string.Format(Strings.FsPreview_GeometryPropertyNodeTooltip,
                    g.Name,
                    g.Description,
                    g.GeometryTypesToString(),
                    g.IsReadOnly,
                    g.HasElevation,
                    g.HasMeasure,
                    g.SpatialContextAssociation,
                    Environment.NewLine);
            }
            else if (prop.Type == PropertyDefinitionType.Data)
            {
                var d = (DataPropertyDefinition)prop;
                desc.Description = string.Format(Strings.FsPreview_DataPropertyNodeTooltip,
                    d.Name,
                    d.Description,
                    d.DataType.ToString(),
                    d.IsNullable,
                    d.IsReadOnly,
                    d.Length,
                    d.Precision,
                    d.Scale,
                    Environment.NewLine);
            }
            else if (prop.Type == PropertyDefinitionType.Raster)
            {
                var r = (RasterPropertyDefinition)prop;
                desc.Description = string.Format(Strings.FsPreview_RasterPropertyNodeTooltip,
                    r.Name,
                    r.Description,
                    r.IsNullable,
                    r.DefaultImageXSize,
                    r.DefaultImageYSize,
                    r.SpatialContextAssociation,
                    Environment.NewLine);
            }
            else
            {
                desc.Description = string.Format(Strings.FsPreview_GenericPropertyTooltip,
                    prop.Name,
                    prop.Type.ToString(),
                    Environment.NewLine);
            }

            return desc;
        }

        private string GetName(string text)
        {
            int startIndex = text.LastIndexOfAny(new char[] { ' ', '+', '/', '*', '-', '%', '=', '>', '<', '&', '|', '^', '~', '(', ',', ')' }); //NOXLATE
            string res = text.Substring(startIndex + 1);
            Debug.WriteLine("Evaluating FDO auto-complete options for: " + res);
            return res;
        }
    }
}
