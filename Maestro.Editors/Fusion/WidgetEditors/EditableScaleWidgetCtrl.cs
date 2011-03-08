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

namespace Maestro.Editors.Fusion.WidgetEditors
{
    public partial class EditableScaleWidgetCtrl : UserControl, IWidgetEditor
    {
        public EditableScaleWidgetCtrl()
        {
            InitializeComponent();
        }

        private IWidget _widget;

        public void Setup(IWidget widget, FlexibleLayoutEditorContext context, IEditorService edsvc)
        {
            _widget = widget;
            baseEditor.Setup(_widget, context, edsvc);

            numPrecision.Value = Convert.ToDecimal(_widget.GetValue("Precision"));
        }

        public Control Content
        {
            get { return this; }
        }

        private void numPrecision_ValueChanged(object sender, EventArgs e)
        {
            _widget.SetValue("Precision", numPrecision.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
