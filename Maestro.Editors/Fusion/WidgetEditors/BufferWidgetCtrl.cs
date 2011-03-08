#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Shared.UI;
using Maestro.Editors.Common;

namespace Maestro.Editors.Fusion.WidgetEditors
{
    public partial class BufferWidgetCtrl : UserControl, IWidgetEditor, INotifyPropertyChanged
    {
        public BufferWidgetCtrl()
        {
            InitializeComponent();
        }

        private string[] _units = { "meters", "feet", "miles", "kilometers" };

        private IWidget _widget;

        public void Setup(IWidget widget, FlexibleLayoutEditorContext context, IEditorService edsvc)
        {
            _widget = widget;
            baseEditor.Setup(_widget, context, edsvc);

            TextBoxBinder.BindText(txtBorderColorInput, this, "BorderColorInput");
            TextBoxBinder.BindText(txtBufferDistance, this, "BufferDistance");
            TextBoxBinder.BindText(txtBufferDistanceInput, this, "BufferDistanceInput");
            TextBoxBinder.BindText(txtBufferUnitsInput, this, "BufferUnitsInput");
            TextBoxBinder.BindText(txtFillColorInput, this, "FillColorInput");
            TextBoxBinder.BindText(txtLayerName, this, "LayerName");
            TextBoxBinder.BindText(txtLayerNameInput, this, "LayerNameInput");
            TextBoxBinder.BindText(txtBufferUnits, this, "BufferUnits");

            cmbBorderColor.CurrentColor = this.BorderColor;
            cmbFillColor.CurrentColor = this.FillColor;
        }

        public Control Content
        {
            get { return this; }
        }

        public string LayerName
        {
            get { return _widget.GetValue("LayerName"); }
            set
            {
                _widget.SetValue("LayerName", value);
                OnPropertyChanged("LayerName");
            }
        }

        public string LayerNameInput
        {
            get { return _widget.GetValue("LayerNameInput"); }
            set
            {
                _widget.SetValue("LayerNameInput", value);
                OnPropertyChanged("LayerNameInput");
            }
        }

        public double BufferDistance
        {
            get
            {
                double value;
                if (double.TryParse(_widget.GetValue("BufferDistance"), out value))
                    return value;
                return 100.0;
            }
            set
            {
                _widget.SetValue("BufferDistance", value.ToString(CultureInfo.InvariantCulture));
                OnPropertyChanged("BufferDistance");
            }
        }

        public string BufferDistanceInput
        {
            get { return _widget.GetValue("BufferDistanceInput"); }
            set
            {
                _widget.SetValue("BufferDistanceInput", value);
                OnPropertyChanged("BufferDistanceInput");
            }
        }

        public string BufferUnits
        {
            get { return _widget.GetValue("BufferUnits"); }
            set
            {
                _widget.SetValue("BufferUnits", value);
                OnPropertyChanged("BufferUnits");
            }
        }

        public string BufferUnitsInput
        {
            get { return _widget.GetValue("BufferUnitsInput"); }
            set
            {
                _widget.SetValue("BufferUnitsInput", value);
                OnPropertyChanged("BufferUnitsInput");
            }
        }

        public Color BorderColor
        {
            get { return Utility.ParseHTMLColor(_widget.GetValue("BorderColor")); }
            set
            {
                _widget.SetValue("BorderColor", Utility.SerializeHTMLColor(value, true));
                OnPropertyChanged("BorderColor");
            }
        }

        public string BorderColorInput
        {
            get { return _widget.GetValue("BorderColorInput"); }
            set
            {
                _widget.SetValue("BorderColorInput", value);
                OnPropertyChanged("BorderColorInput");
            }
        }

        public Color FillColor
        {
            get { return Utility.ParseHTMLColor(_widget.GetValue("FillColor")); }
            set
            {
                _widget.SetValue("FillColor", Utility.SerializeHTMLColor(value, true));
                OnPropertyChanged("FillColor");
            }
        }

        public string FillColorInput
        {
            get { return _widget.GetValue("FillColorInput"); }
            set
            {
                _widget.SetValue("FillColorInput", value);
                OnPropertyChanged("FillColorInput");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void btnBrowseBufferUnits_Click(object sender, EventArgs e)
        {
            var item = GenericItemSelectionDialog.SelectItem("Buffer Units", "Select Buffer Units", _units);
            if (item != null)
            {
                txtBufferUnits.Text = item;
            }
        }

        private void cmbBorderColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BorderColor = cmbBorderColor.CurrentColor;
        }

        private void cmbFillColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillColor = cmbFillColor.CurrentColor;
        }
    }
}
