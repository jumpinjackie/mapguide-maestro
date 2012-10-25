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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using Res = Maestro.Base.Properties.Resources;
using System.IO;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Base.Templates
{
    internal class UserItemTemplate : ItemTemplate
    {
        private IResource _res;

        public UserItemTemplate(string templatePath)
            : this(Strings.TPL_USER_DEFINED, templatePath)
        { }

        public UserItemTemplate(string description, string templatePath)
            : this(Path.GetFileNameWithoutExtension(templatePath), description, templatePath)
        { }

        public string TemplatePath
        {
            get;
            private set;
        }

        public UserItemTemplate(string name, string description, string templatePath)
        {
            this.Description = description;

            this.Category = Strings.TPL_CATEGORY_USERDEF;
            this.Icon = Res.document;
            this.Name = name;
            this.TemplatePath = templatePath;

            _res = ResourceTypeRegistry.Deserialize(File.ReadAllText(templatePath));
            this.ResourceType = _res.ResourceType.ToString();
        }

        public override IResource CreateItem(string startPoint, IServerConnection conn)
        {
            IResource res = (IResource)_res.Clone();
            res.CurrentConnection = conn;
            return res;
        }
    }
}
