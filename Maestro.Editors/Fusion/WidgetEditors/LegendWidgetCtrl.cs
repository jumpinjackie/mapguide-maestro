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

namespace Maestro.Editors.Fusion.WidgetEditors
{
    [ToolboxItem(false)]
    internal partial class LegendWidgetCtrl : UserControl, IWidgetEditor
    {
        public LegendWidgetCtrl()
        {
            InitializeComponent();
        }

        private IWidget _widget;

        public void Setup(IWidget widget, FlexibleLayoutEditorContext context, IEditorService edsvc)
        {
            _widget = widget;
            baseEditor.Setup(_widget, context, edsvc);

            chkHideInvisibleLayers.Checked = Convert.ToBoolean(_widget.GetValue("HideInvisibleLayers")); //NOXLATE
            chkShowMapFolder.Checked = Convert.ToBoolean(_widget.GetValue("ShowMapFolder")); //NOXLATE
            chkShowRootFolder.Checked = Convert.ToBoolean(_widget.GetValue("ShowRootFolder")); //NOXLATE

            txtDisabledLayerIcon.Text = _widget.GetValue("DisabledLayerIcon"); //NOXLATE
            txtGroupInfoIcon.Text = _widget.GetValue("GroupInfoIcon"); //NOXLATE
            txtLayerDwfIcon.Text = _widget.GetValue("LayerDWFIcon"); //NOXLATE
            txtLayerInfoIcon.Text = _widget.GetValue("LayerInfoIcon"); //NOXLATE
            txtLayerRasterIcon.Text = _widget.GetValue("LayerRasterIcon"); //NOXLATE
            txtLayerThemeIcon.Text = _widget.GetValue("LayerThemeIcon"); //NOXLATE
            txtRootFolderIcon.Text = _widget.GetValue("RootFolderIcon"); //NOXLATE
        }

        public Control Content
        {
            get { return this; }
        }

        private void chkHideInvisibleLayers_CheckedChanged(object sender, EventArgs e)
        {
            _widget.SetValue("HideInvisibleLayers", chkHideInvisibleLayers.Checked.ToString()); //NOXLATE
        }

        private void chkShowRootFolder_CheckedChanged(object sender, EventArgs e)
        {
            _widget.SetValue("ShowRootFolder", chkShowRootFolder.Checked.ToString()); //NOXLATE
        }

        private void chkShowMapFolder_CheckedChanged(object sender, EventArgs e)
        {
            _widget.SetValue("ShowMapFolder", chkShowMapFolder.Checked.ToString()); //NOXLATE
        }

        private void txtLayerRasterIcon_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("LayerRasterIcon", txtLayerRasterIcon.Text); //NOXLATE
        }

        private void txtLayerDwfIcon_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("LayerDWFIcon", txtLayerDwfIcon.Text); //NOXLATE
        }

        private void txtLayerThemeIcon_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("LayerThemeIcon", txtLayerThemeIcon.Text); //NOXLATE
        }

        private void txtDisabledLayerIcon_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("DisabledLayerIcon", txtDisabledLayerIcon.Text); //NOXLATE
        }

        private void txtLayerInfoIcon_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("LayerInfoIcon", txtLayerInfoIcon.Text); //NOXLATE
        }

        private void txtGroupInfoIcon_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("GroupInfoIcon", txtGroupInfoIcon.Text); //NOXLATE
        }

        private void txtRootFolderIcon_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("RootFolderIcon", txtRootFolderIcon.Text); //NOXLATE
        }
    }
}
