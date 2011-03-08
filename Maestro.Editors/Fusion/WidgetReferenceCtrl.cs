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

namespace Maestro.Editors.Fusion
{
    public partial class WidgetReferenceCtrl : UserControl
    {
        private WidgetReferenceCtrl()
        {
            InitializeComponent();
        }

        private IWidgetItem _widgetRef;
        private IApplicationDefinition _flexLayout;
        private bool _init = true;

        public WidgetReferenceCtrl(IWidgetItem item, IEditorService edsvc)
            : this()
        {
            _widgetRef = item;
            _flexLayout = (IApplicationDefinition)edsvc.GetEditedResource();
            cmbWidgetRefs.DataSource = _flexLayout.GetAllReferenceableWidgetNames();

            cmbWidgetRefs.SelectedItem = item.Widget;
            _init = false;
        }

        private void cmbWidgetRefs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (cmbWidgetRefs.SelectedItem != null)
            {
                _widgetRef.Widget = cmbWidgetRefs.SelectedItem.ToString();
            }
        }
    }
}
