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
using System.Text;
using Maestro.Base.Templates;
using Res = Maestro.AddIn.ExtendedObjectModels.Properties.Resources;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.AddIn.ExtendedObjectModels.Templates
{
    internal class WebLayout110ItemTemplate : ItemTemplate
    {
        public WebLayout110ItemTemplate()
        {
            Category = Res.TPL_CATEGORY_MGOS22;
            Icon = Res.application_browser;
            Description = Res.TPL_WL_110_DESC;
            Name = Res.TPL_WL_110_NAME;
            ResourceType = ResourceTypes.WebLayout.ToString();
        }

        public override Version MinimumSiteVersion
        {
            get
            {
                return new Version(2, 2);
            }
        }

        public override IResource CreateItem(IServerConnection conn)
        {
            return ObjectFactory.CreateWebLayout(conn, new Version(1, 1, 0), string.Empty);
        }
    }
}
