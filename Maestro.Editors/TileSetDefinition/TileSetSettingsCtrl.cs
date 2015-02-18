#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

#endregion Disclaimer / License
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.ObjectModels.Common;
using Maestro.Editors.TileSetDefinition.Providers;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;

namespace Maestro.Editors.TileSetDefinition
{
    [ToolboxItem(false)]
    internal partial class TileSetSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public TileSetSettingsCtrl()
        {
            InitializeComponent();
        }

        private bool _init = false;
        private ITileSetDefinition _tsd;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            try
            {
                _init = true;
                _tsd = (ITileSetDefinition)service.GetEditedResource();
                var cmd = (IGetTileProviders)service.CurrentConnection.CreateCommand((int)CommandType.GetTileProviders);
                var providers = cmd.Execute();

                var provider = providers.TileProvider.FirstOrDefault(p => p.Name == _tsd.TileStoreParameters.TileProvider);
                if (provider != null)
                {
                    txtProvider.Text = provider.DisplayName;
                    LoadProviderCtrl(provider);
                }
            }
            finally
            {
                _init = false;
            }
        }

        private void LoadProviderCtrl(TileProvider provider)
        {
            var ctrl = MakeProviderCtrl(provider);
            ctrl.Dock = DockStyle.Fill;
            grpSettings.Controls.Clear();
            grpSettings.Controls.Add(ctrl);
        }

        private Control MakeProviderCtrl(TileProvider provider)
        {
            return new GenericProviderCtrl(provider, _tsd, OnResourceChanged);
        }
    }
}
