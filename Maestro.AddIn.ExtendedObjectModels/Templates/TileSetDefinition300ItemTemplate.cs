#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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
using Maestro.Base.Templates;
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class TileSetDefinition300ItemTemplate : ItemTemplate
    {
        public TileSetDefinition300ItemTemplate()
        {
            Category = Strings.TPL_CATEGORY_MGOS30;
            Icon = Res.grid;
            Description = Strings.TPL_TSD_300_DESC;
            Name = Strings.TPL_TSD_300_NAME;
            ResourceType = ResourceTypes.TileSetDefinition.ToString();
        }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(3, 0);
            }
        }

        public override IResource CreateItem(string startPoint, OSGeo.MapGuide.MaestroAPI.IServerConnection conn)
        {
            var cmd = (IGetTileProviders)conn.CreateCommand((int)CommandType.GetTileProviders);
            var providers = cmd.Execute();
            var item = GenericItemSelectionDialog.SelectItem(Strings.SelectTileProvider, Strings.SelectTileProvider, providers.TileProvider, "DisplayName", "Name");
            if (item != null)
            {
                ITileSetDefinition tsd = ObjectFactory.CreateTileSetDefinition(new Version(3, 0, 0));
                switch (item.Name)
                {
                    case "Default":
                        tsd.SetDefaultProviderParameters(300, 300, string.Empty, new double[0]);
                        break;
                    case "XYZ":
                        tsd.SetXYZProviderParameters();
                        break;
                    default:
                        return null;
                }
                return tsd;
            }
            return null;
        }
    }
}
