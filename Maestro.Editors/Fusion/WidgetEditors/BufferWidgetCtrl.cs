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
    [ToolboxItem(false)]
    internal partial class BufferWidgetCtrl : UserControl, IWidgetEditor, INotifyPropertyChanged
    {
        public BufferWidgetCtrl()
        {
            InitializeComponent();
        }

        private string[] _units = { "meters", "feet", "miles", "kilometers" }; //NOXLATE

        private IWidget _widget;

        public void Setup(IWidget widget, FlexibleLayoutEditorContext context, IEditorService edsvc)
        {
            _widget = widget;
            baseEditor.Setup(_widget, context, edsvc);

            TextBoxBinder.BindText(txtBorderColorInput, this, "BorderColorInput"); //NOXLATE
            TextBoxBinder.BindText(txtBufferDistance, this, "BufferDistance"); //NOXLATE
            TextBoxBinder.BindText(txtBufferDistanceInput, this, "BufferDistanceInput"); //NOXLATE
            TextBoxBinder.BindText(txtBufferUnitsInput, this, "BufferUnitsInput"); //NOXLATE
            TextBoxBinder.BindText(txtFillColorInput, this, "FillColorInput"); //NOXLATE
            TextBoxBinder.BindText(txtLayerName, this, "LayerName"); //NOXLATE
            TextBoxBinder.BindText(txtLayerNameInput, this, "LayerNameInput"); //NOXLATE
            TextBoxBinder.BindText(txtBufferUnits, this, "BufferUnits"); //NOXLATE

            cmbBorderColor.CurrentColor = this.BorderColor;
            cmbFillColor.CurrentColor = this.FillColor;
        }

        public Control Content
        {
            get { return this; }
        }

        public string LayerName
        {
            get { return _widget.GetValue("LayerName"); } //NOXLATE
            set
            {
                _widget.SetValue("LayerName", value); //NOXLATE
                OnPropertyChanged("LayerName"); //NOXLATE
            }
        }

        public string LayerNameInput
        {
            get { return _widget.GetValue("LayerNameInput"); } //NOXLATE
            set
            {
                _widget.SetValue("LayerNameInput", value); //NOXLATE
                OnPropertyChanged("LayerNameInput"); //NOXLATE
            }
        }

        public double BufferDistance
        {
            get
            {
                double value;
                if (double.TryParse(_widget.GetValue("BufferDistance"), out value)) //NOXLATE
                    return value;
                return 100.0;
            }
            set
            {
                _widget.SetValue("BufferDistance", value.ToString(CultureInfo.InvariantCulture)); //NOXLATE
                OnPropertyChanged("BufferDistance"); //NOXLATE
            }
        }

        public string BufferDistanceInput
        {
            get { return _widget.GetValue("BufferDistanceInput"); } //NOXLATE
            set
            {
                _widget.SetValue("BufferDistanceInput", value); //NOXLATE
                OnPropertyChanged("BufferDistanceInput"); //NOXLATE
            }
        }

        public string BufferUnits
        {
            get { return _widget.GetValue("BufferUnits"); } //NOXLATE
            set
            {
                _widget.SetValue("BufferUnits", value); //NOXLATE
                OnPropertyChanged("BufferUnits"); //NOXLATE
            }
        }

        public string BufferUnitsInput
        {
            get { return _widget.GetValue("BufferUnitsInput"); } //NOXLATE
            set
            {
                _widget.SetValue("BufferUnitsInput", value); //NOXLATE
                OnPropertyChanged("BufferUnitsInput"); //NOXLATE
            }
        }

        public Color BorderColor
        {
            get { return Utility.ParseHTMLColor(_widget.GetValue("BorderColor")); } //NOXLATE
            set
            {
                _widget.SetValue("BorderColor", Utility.SerializeHTMLColor(value, true)); //NOXLATE
                OnPropertyChanged("BorderColor"); //NOXLATE
            }
        }

        public string BorderColorInput
        {
            get { return _widget.GetValue("BorderColorInput"); } //NOXLATE
            set
            {
                _widget.SetValue("BorderColorInput", value); //NOXLATE
                OnPropertyChanged("BorderColorInput"); //NOXLATE
            }
        }

        public Color FillColor
        {
            get { return Utility.ParseHTMLColor(_widget.GetValue("FillColor")); } //NOXLATE
            set
            {
                _widget.SetValue("FillColor", Utility.SerializeHTMLColor(value, true)); //NOXLATE
                OnPropertyChanged("FillColor"); //NOXLATE
            }
        }

        public string FillColorInput
        {
            get { return _widget.GetValue("FillColorInput"); } //NOXLATE
            set
            {
                _widget.SetValue("FillColorInput", value); //NOXLATE
                OnPropertyChanged("FillColorInput"); //NOXLATE
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
            var item = GenericItemSelectionDialog.SelectItem(Properties.Resources.TitleBufferUnits, Properties.Resources.PromptSelectBufferUnits, _units);
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
