#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Collections;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.MaestroAPI;
using System.Reflection;

namespace Maestro.Editors.SymbolDefinition
{
    /// <summary>
    /// A UI control for editing symbol definition settings
    /// </summary>
    [ToolboxItem(true)]
    [Designer(typeof(SymbolFieldDesigner))]
    public partial class SymbolField : UserControl
    {
        private class SymbolFieldDesigner : ControlDesigner
        {
            public override IList SnapLines
            {
                get
                {
                    /* Code from above */
                    IList snapLines = base.SnapLines;

                    // *** This will need to be modified to match your user control
                    SymbolField control = Control as SymbolField;
                    if (control == null) { return snapLines; }

                    Control el = control.combo;
                    if (el == null) { return snapLines; }

                    // *** This will need to be modified to match the item in your user control
                    // This is the control in your UC that you want SnapLines for the entire UC
                    IDesigner designer = TypeDescriptor.CreateDesigner(
                        el, typeof(IDesigner));
                    if (designer == null) { return snapLines; }

                    // *** This will need to be modified to match the item in your user control
                    designer.Initialize(el);

                    using (designer)
                    {
                        ControlDesigner boxDesigner = designer as ControlDesigner;
                        if (boxDesigner == null) { return snapLines; }

                        foreach (SnapLine line in boxDesigner.SnapLines)
                        {
                            if (line.SnapLineType == SnapLineType.Baseline)
                            {
                                // *** This will need to be modified to match the item in your user control
                                snapLines.Add(new SnapLine(SnapLineType.Baseline,
                                    line.Offset + el.Top,
                                    line.Filter, line.Priority));
                                break;
                            }
                        }
                    }

                    return snapLines;
                }

            }
        }

        /// <summary>
        /// A method for presenting symbol parameters for a given symbol field
        /// </summary>
        /// <param name="sender">The sender.</param>
        public delegate void BrowseEventHandler(SymbolField sender);

        /// <summary>
        /// Occurs when a request has been made to present symbol parameters for selection
        /// </summary>
        public event BrowseEventHandler RequestBrowse;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolField"/> class.
        /// </summary>
        public SymbolField()
        {
            InitializeComponent();
            combo.TextChanged += new EventHandler(combo_TextChanged);
        }

        void combo_TextChanged(object sender, EventArgs e)
        {
            OnContentChanged();
        }

        /// <summary>
        /// Occurs when [content changed].
        /// </summary>
        public event EventHandler ContentChanged;

        //Important: We need to set this otherwise the Winforms Designer will put you in a world of pain
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Content
        {
            get { return combo.Text; }
            set 
            {
                if (combo.Text != value)
                {
                    combo.Text = value;
                    OnContentChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the enum type. Only set this if the underlying symbol property is an enum type
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type EnumType
        {
            get;
            private set;
        }

        /// <summary>
        /// Sets this field to explicitly take enum values for input
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        public void SetEnumMode<TEnum>()
        {
            this.EnumType = typeof(TEnum);
            this.Items = GetItems<TEnum>(false);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Sets this field for boolean input
        /// </summary>
        public void SetBooleanMode()
        {
            this.EnumType = null;
            this.Items = new string[]
            {
                true.ToString().ToLower(),
                false.ToString().ToLower()
            };
        }

        private void OnContentChanged()
        {
            if (_isBinding)
                return;

            if (_suppressProperty)
                return;

            //Update databound property before raising event
            if (_boundObjectType != null && _boundObject != null && _boundProperty != null)
            {
                try
                {
                    _suppressUI = true;
                    if (string.IsNullOrEmpty(this.Content))
                    {
                        _boundProperty.SetValue(_boundObject, null, null);
                    }
                    else
                    {
                        if (this.EnumType != null)
                        {
                            _boundProperty.SetValue(_boundObject, Enum.Parse(this.EnumType, this.Content), null);
                        }
                        else
                        {
                            _boundProperty.SetValue(_boundObject, this.Content, null);
                        }
                    }
                }
                finally
                {
                    _suppressUI = false;
                }
            }

            var handler = this.ContentChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private string[] _items;

        //Important: We need to set this otherwise the Winforms Designer will put you in a world of pain
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] Items
        {
            get { return _items; }
            set
            {
                _items = value;
                combo.Items.Clear();
                foreach (var it in value)
                {
                    combo.Items.Add(it);
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var handler = this.RequestBrowse;
            if (handler != null)
                handler(this);
        }

        //Important: We need to set this otherwise the Winforms Designer will put you in a world of pain
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[Description("Indicates the parameter data type(s) that are suitable for this field. Leave empty for all types")]
        //public DataType[] SupportedDataTypes { get; set; }

        //Important: We need to set this otherwise the Winforms Designer will put you in a world of pain
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// Gets or sets the supported enhanced data types.
        /// </summary>
        /// <value>
        /// The supported enhanced data types.
        /// </value>
        [Description("Indicates the parameter data type(s) that are suitable for this field. Leave empty for all types")]
        public DataType2[] SupportedEnhancedDataTypes { get; set; }

        /// <summary>
        /// Gets the items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetItems<T>()
        {
            return GetItems<T>(true);
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetItems<T>(bool quote)
        {
            var values = Enum.GetValues(typeof(T));
            var items = new string[values.Length];
            int i = 0;
            if (quote)
            {
                foreach (var val in values)
                {
                    items[i] = "'" + val + "'";
                    i++;
                }
            }
            else
            {
                foreach (var val in values)
                {
                    items[i] = val + "";
                    i++;
                }
            }
            return items;
        }

        //This code below attempts to approximate data binding
        private bool _suppressUI = false;           //Used to prevent a stack overflow due to a continuous UI -> Property -> UI -> ... chain
        private bool _suppressProperty = false;     //Used to prevent a stack overflow due to a continuous UI -> Property -> UI -> ... chain
        private bool _isBinding = false;

        private object _boundObject;
        private Type _boundObjectType;
        private PropertyInfo _boundProperty;

        /// <summary>
        /// Binds the specified data source.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="propertyName">Name of the property.</param>
        public void Bind(object dataSource, string propertyName)
        {
            Check.NotNull(dataSource, "dataSource");
            Check.NotEmpty(propertyName, "member");
            try
            {
                _isBinding = true;

                if (_boundObject != null)
                {
                    var i = _boundObject as INotifyPropertyChanged;
                    if (i != null)
                        i.PropertyChanged -= OnBoundObjectPropertyChanged;
                }

                _boundObject = dataSource;
                _boundObjectType = _boundObject.GetType();
                _boundProperty = _boundObjectType.GetProperty(propertyName);

                if (_boundProperty == null)
                    throw new InvalidOperationException("Could not find property " + _boundProperty + " of databound object (" + _boundObjectType.Name + ")"); //LOCALIZEME

                //Set initial value
                var val = _boundProperty.GetValue(_boundObject, null);
                if (val != null)
                    this.Content = val.ToString();
                else
                    this.Content = null;

                var inp = _boundObject as INotifyPropertyChanged;
                if (inp != null)
                {
                    inp.PropertyChanged += OnBoundObjectPropertyChanged;
                }
            }
            finally
            {
                _isBinding = false;
            }
        }

        void OnBoundObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_isBinding)
                return;

            if (_suppressUI)
                return;

            if (_boundProperty.Name != e.PropertyName)
                return;

            try
            {
                _suppressProperty = true;
                var val = _boundProperty.GetValue(_boundObject, null);
                if (val != null)
                    this.Content = val.ToString();
                else
                    this.Content = null;
            }
            finally
            {
                _suppressProperty = false;
            }
        }
    }
}
