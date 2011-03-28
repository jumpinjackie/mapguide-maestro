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
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI;
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Base.Services
{
    using Res = Properties.Resources;
    using Maestro.Base.Templates;
    using System.IO;

    public class NewItemTemplateService : ServiceBase
    {
        private Dictionary<string, List<ItemTemplate>> _templates;

        public override void Initialize()
        {
            base.Initialize();
            _templates = new Dictionary<string, List<ItemTemplate>>();

            var tpls = AddInTree.BuildItems<ItemTemplate>("/Maestro/NewItemTemplates", this);
            foreach (var tp in tpls)
            {
                if (!_templates.ContainsKey(tp.Category))
                    _templates[tp.Category] = new List<ItemTemplate>();

                _templates[tp.Category].Add(tp);
                LoggingService.Info("Registered default template: " + tp.GetType()); //LOCALIZE
            }

            if (!_templates.ContainsKey(Res.TPL_CATEGORY_USERDEF))
                _templates[Res.TPL_CATEGORY_USERDEF] = new List<ItemTemplate>();

            UserItemTemplate [] utpls = ScanUserTemplates();
            foreach (var ut in utpls)
            {
                _templates[Res.TPL_CATEGORY_USERDEF].Add(ut);
                LoggingService.Info("Adding user template: " + ut.TemplatePath);
            }
            LoggingService.Info("Initialized: New Item Template Service"); //LOCALIZE
        }

        private UserItemTemplate[] ScanUserTemplates()
        {
            List<UserItemTemplate> tpls = new List<UserItemTemplate>();
            
            //TODO: Store path in preferences
            string userTplPath = Path.Combine(FileUtility.ApplicationRootPath, "UserTemplates");
            if (Directory.Exists(userTplPath))
            {
                foreach (string file in Directory.GetFiles(userTplPath))
                {
                    try
                    {
                        var tpl = new UserItemTemplate(file);
                        tpls.Add(tpl);
                    }
                    catch (Exception ex)
                    {
                        LoggingService.Info("Could not load user template: " + file); //LOCALIZE
                    }
                }
            }
            return tpls.ToArray();
        }

        public string[] GetCategories()
        {
            List<string> values = new List<string>(_templates.Keys);
            return values.ToArray();
        }

        public class TemplateSet
        {
            private Dictionary<string, List<ItemTemplate>> _templates;

            internal TemplateSet(IEnumerable<ItemTemplate> templates)
            {
                _templates = new Dictionary<string,List<ItemTemplate>>();
                foreach (var tpl in templates)
                {
                    if (!_templates.ContainsKey(tpl.Category))
                        _templates[tpl.Category] = new List<ItemTemplate>();

                    _templates[tpl.Category].Add(tpl);
                }
                foreach (var key in _templates.Keys)
                {
                    _templates[key].Sort();
                }
            }

            public IEnumerable<string> GetCategories() { return _templates.Keys; }

            public IEnumerable<ItemTemplate> GetTemplatesForCategory(string category)
            {
                return _templates[category];
            }
        }

        public TemplateSet GetItemTemplates(string[] categories, Version siteVersion)
        {
            List<ItemTemplate> templates = new List<ItemTemplate>();
            foreach (var cat in categories)
            {
                if (_templates.ContainsKey(cat))
                {
                    var matches = new List<ItemTemplate>();
                    foreach (var tpl in _templates[cat])
                    {
                        if (siteVersion >= tpl.MinimumSiteVersion)
                            matches.Add(tpl);
                    }
                    templates.AddRange(matches);
                }
            }
            return new TemplateSet(templates.ToArray());
        }
    }
}
