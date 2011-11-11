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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.Editors.LayerDefinition
{
    /// <summary>
    /// Editor control for Grid (Raster) Layer Definitions
    /// </summary>
    public partial class RasterLayerEditorCtrl : EditorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RasterLayerEditorCtrl()
        {
            InitializeComponent();
        }

        private IRasterLayerDefinition _rldf;
        private IEditorService _edsvc;

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _rldf = (IRasterLayerDefinition)((ILayerDefinition)_edsvc.GetEditedResource()).SubLayer;
            _edsvc.RegisterCustomNotifier(this);
            rasterLayerSettingsSectionCtrl.Bind(_edsvc);
            rasterLayerVisibilitySectionCtrl.Bind(_edsvc);
            rasterLayerAdvancedSectionCtrl.Bind(_edsvc);
        }
    }
}
