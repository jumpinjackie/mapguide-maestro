#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class WatermarkDefinitionSimple240ItemTemplate : ItemTemplate
    {
        public WatermarkDefinitionSimple240ItemTemplate()
            : base(Strings.TPL_CATEGORY_MGOS24,
                   Res.water,
                   Strings.TPL_WDFS_240_DESC,
                   Strings.TPL_WDFS_240_NAME,
                   ResourceTypes.LayerDefinition.ToString(),
                   "Simple", //NOXLATE
                   new Version(2, 4, 0))
        { }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 4);
            }
        }

        public override IResource CreateItem(string startPoint, OSGeo.MapGuide.MaestroAPI.IServerConnection conn)
        {
            return ObjectFactory.CreateWatermark(SymbolDefinitionType.Simple, new Version(2, 4, 0));
        }
    }

    internal class WatermarkDefinitionCompound240ItemTemplate : ItemTemplate
    {
        public WatermarkDefinitionCompound240ItemTemplate()
            : base(Strings.TPL_CATEGORY_MGOS24,
                   Res.water,
                   Strings.TPL_WDFC_240_DESC,
                   Strings.TPL_WDFC_240_NAME,
                   ResourceTypes.LayerDefinition.ToString(),
                   "Compound", //NOXLATE
                   new Version(2, 4, 0))
        { }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 4);
            }
        }

        public override IResource CreateItem(string startPoint, OSGeo.MapGuide.MaestroAPI.IServerConnection conn)
        {
            return ObjectFactory.CreateWatermark(SymbolDefinitionType.Compound, new Version(2, 4, 0));
        }
    }
}