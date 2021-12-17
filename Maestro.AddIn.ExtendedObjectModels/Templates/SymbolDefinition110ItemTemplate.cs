#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using System;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    public class SimpleSymbolDefinitionItem110Template : ItemTemplate
    {
        public SimpleSymbolDefinitionItem110Template()
            : base(Strings.TPL_CATEGORY_MGOS20,
                   Res.marker,
                   Strings.TPL_SSD_DESC,
                   Strings.TPL_SSD_NAME,
                   ResourceTypes.SymbolDefinition.ToString(),
                   "Simple", //NOXLATE
                   new Version(1, 1, 0))
        { }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 0);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            return ObjectFactory.CreateSimpleSymbol(new Version(1, 1, 0), Strings.DefaultSymbolName, Strings.DefaultSymbolDescription);
        }
    }

    public class CompoundSymbolDefinition110ItemTemplate : ItemTemplate
    {
        public CompoundSymbolDefinition110ItemTemplate()
            : base(Strings.TPL_CATEGORY_MGOS20,
                   Res.marker,
                   Strings.TPL_CSD_DESC,
                   Strings.TPL_CSD_NAME,
                   ResourceTypes.SymbolDefinition.ToString(),
                   "Compound", //NOXLATE
                   new Version(1, 1, 0))
        { }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 0);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            return ObjectFactory.CreateCompoundSymbol(new Version(1, 1, 0), Strings.DefaultSymbolName, Strings.DefaultSymbolDescription);
        }
    }
}