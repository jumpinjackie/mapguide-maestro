#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Editors.MapDefinition
{
    [ToolboxItem(true)]
    public partial class LiveMapDefinitionEditorCtrl : EditorBase
    {
        public LiveMapDefinitionEditorCtrl()
        {
            InitializeComponent();
        }

        public IEditorService EditorService
        {
            get;
            private set;
        }

        /// <summary>
        /// Synchronizes the internal Map Definition with the state of the Runtime Map.
        /// 
        /// Call this before attempting access to the internal Map Definition to ensure a consistent state
        /// </summary>
        public void SyncMap()
        {
            _rtMap.UpdateMapDefinition(_shadowCopy);
        }

        public IMapDefinition GetMapDefinition()
        {
            return _shadowCopy;
        }

        public bool ConvertTiledGroupsToNonTiled
        {
            get { return viewer.ConvertTiledGroupsToNonTiled; }
            set { viewer.ConvertTiledGroupsToNonTiled = value; }
        }

        public void RefreshMap()
        {
            viewer.RefreshMap();
        }

        private RuntimeMap _rtMap;
        private IMapDefinition _shadowCopy;
        private IMappingService _mapSvc;

        public override void Bind(IEditorService service)
        {
            this.EditorService = service;
            service.RegisterCustomNotifier(this);

            _shadowCopy = (IMapDefinition)service.GetEditedResource();
            _mapSvc = (IMappingService)_shadowCopy.CurrentConnection.GetService((int)ServiceType.Mapping);
            _rtMap = _mapSvc.CreateMap(_shadowCopy);

            viewer.LoadMap(_rtMap);
        }

        private void legendCtrl_NodeSelected(object sender, TreeNode node)
        {
            var layer = node.Tag as Maestro.MapViewer.Legend.LayerNodeMetadata;
            //Nothing to edit for theme rule nodes
            if (layer != null && layer.IsThemeRule)
                return;
            propGrid.SelectedObject = node.Tag;
        }
    }
}
