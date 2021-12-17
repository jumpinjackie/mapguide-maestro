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

using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using System;
using Res = Maestro.Base.Properties.Resources;

namespace Maestro.Base.Templates
{
    internal class FeatureSourceItemTemplate : ItemTemplate
    {
        public FeatureSourceItemTemplate()
            : base(Strings.TPL_CATEGORY_DEFAULT,
                   Res.database_share,
                   Strings.TPL_FS_DESC,
                   Strings.TPL_FS_NAME,
                   ResourceTypes.FeatureSource.ToString(),
                   null,
                   new Version(1, 0, 0))
        { }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            var provider = GenericItemSelectionDialog.SelectItem(
                Strings.SelectFdoProvider,
                Strings.SelectFdoProvider,
                conn.FeatureService.FeatureProviders,
                "DisplayName", //NOXLATE
                "Name"); //NOXLATE

            if (provider != null)
            {
                return ObjectFactory.CreateFeatureSource(Utility.StripVersionFromProviderName(provider.Name));
            }
            return null;
        }
    }
}