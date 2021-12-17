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
using System;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class MapDefinition240ItemTemplate : ItemTemplate
    {
        public MapDefinition240ItemTemplate()
            : base(Strings.TPL_CATEGORY_MGOS24,
                   Res.map,
                   Strings.TPL_MDF_240_DESC,
                   Strings.TPL_MDF_240_NAME,
                   ResourceTypes.MapDefinition.ToString(),
                   null,
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
            return ObjectFactory.CreateMapDefinition(new Version(2, 4, 0), string.Empty);
        }
    }
}