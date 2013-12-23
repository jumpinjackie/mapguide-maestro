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
using Maestro.Editors.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;

#pragma warning disable 1591

namespace Maestro.Base.Editor
{
    /// <summary>
    /// A specialized editor for Load Procedure resources.
    /// </summary>
    /// <remarks>
    /// Although public, this class is undocumented and reserved for internal use by built-in Maestro AddIns
    /// </remarks>
    public partial class LoadProcedureEditor : EditorContentBase
    {
        public LoadProcedureEditor()
        {
            InitializeComponent();
        }

        protected override void Bind(Maestro.Editors.IEditorService service)
        {
            panelBody.Controls.Clear();
            var lp = (ILoadProcedure)service.GetEditedResource();
            //DWG and Raster load procedures cannot be edited with this editor so load placeholder
            if (lp.SubType.Type == LoadType.Dwg ||
                lp.SubType.Type == LoadType.Raster)
            {
                var ctrl = new UnsupportedEditorControl();
                ctrl.Dock = DockStyle.Fill;
                panelBody.Controls.Add(ctrl);
            }
            else
            {
                var lpEditor = new LoadProcedureEditorCtrl();
                lpEditor.Dock = DockStyle.Fill;
                panelBody.Controls.Add(lpEditor);

                lpEditor.Bind(service);
            }
        }

        public override Icon ViewIcon
        {
            get
            {
                return Properties.Resources.icon_document;
            }
        }
    }
}
