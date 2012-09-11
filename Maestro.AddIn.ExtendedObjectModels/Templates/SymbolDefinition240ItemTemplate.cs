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
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Base.Templates;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    public class SimpleSymbolDefinitionItem240Template : ItemTemplate
    {
        public SimpleSymbolDefinitionItem240Template()
        {
            Category = Strings.TPL_CATEGORY_MGOS24;
            Icon = Res.marker;
            Description = Strings.TPL_SSD_240_DESC;
            Name = Strings.TPL_SSD_240_NAME;
            ResourceType = ResourceTypes.SymbolDefinition.ToString();
        }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 4);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            return ObjectFactory.CreateSimpleSymbol(conn, new Version(2, 4, 0), Strings.DefaultSymbolName, Strings.DefaultSymbolDescription);
        }
    }

    public class CompoundSymbolDefinition240ItemTemplate : ItemTemplate
    {
        public CompoundSymbolDefinition240ItemTemplate()
        {
            Category = Strings.TPL_CATEGORY_MGOS24;
            Icon = Res.marker;
            Description = Strings.TPL_CSD_240_DESC;
            Name = Strings.TPL_CSD_240_NAME;
            ResourceType = ResourceTypes.SymbolDefinition.ToString();
        }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 4);
            }
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            return ObjectFactory.CreateCompoundSymbol(conn, new Version(2, 4, 0), Strings.DefaultSymbolName, Strings.DefaultSymbolDescription);
        }
    }
}
