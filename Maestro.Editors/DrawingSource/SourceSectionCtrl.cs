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
using Maestro.Shared.UI;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.DrawingSource;

namespace Maestro.Editors.DrawingSource
{
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class SourceSectionCtrl : EditorBindableCollapsiblePanel
    {
        public SourceSectionCtrl()
        {
            InitializeComponent();
        }

        private IDrawingSource _dws;
        private IEditorService _edSvc;

        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _edSvc.RegisterCustomNotifier(this);
            _dws = (IDrawingSource)service.GetEditedResource();

            resDataCtrl.Init(service);
            resDataCtrl.DataListChanged += new EventHandler(OnResourceDataListChanged);
            resDataCtrl.ResourceDataMarked += new ResourceDataSelectionEventHandler(OnResourceDataMarked);
            TextBoxBinder.BindText(txtSourceCs, _dws, "CoordinateSpace"); //NOXLATE
            MarkSelected();
        }

        void OnResourceDataListChanged(object sender, EventArgs e)
        {
            OnResourceChanged();
        }

        private void MarkSelected()
        {
            var file = _dws.SourceName;
            if (!string.IsNullOrEmpty(file))
            {
                resDataCtrl.MarkedFile = file;
            }
        }

        void OnResourceDataMarked(object sender, string dataName)
        {
            _dws.SourceName = dataName;

            //Re-calc sheet extents for this DWF
            using (new WaitCursor(this))
            {
                _dws.RemoveAllSheets();
                _edSvc.SyncSessionCopy();

                //Re-populate sheets
                _dws.RegenerateSheetList();

                _edSvc.SyncSessionCopy();
                //Re-calc extents
                _dws.UpdateExtents();
                OnResourceChanged();
            }
        }

        private void btnBrowseCs_Click(object sender, EventArgs e)
        {
            var cs = _edSvc.GetCoordinateSystem();
            if (!string.IsNullOrEmpty(cs))
            {
                _dws.CoordinateSpace = cs;
            }
        }
    }
}
