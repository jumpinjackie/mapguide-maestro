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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.Fusion
{
    /// <summary>
    /// A helper class for widget editors
    /// </summary>
    public class FlexibleLayoutEditorContext
    {
        private IFusionService _service;

        private IApplicationDefinitionWidgetInfoSet _widgetSet;
        private IApplicationDefinitionTemplateInfoSet _templateSet;
        private IApplicationDefinitionContainerInfoSet _containerSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public FlexibleLayoutEditorContext(IFusionService service)
        {
            _service = service;

            _widgetSet = _service.GetApplicationWidgets();
            _templateSet = _service.GetApplicationTemplates();
            _containerSet = _service.GetApplicationContainers();
        }

        /// <summary>
        /// Gets information about a particular widget (by name)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IWidgetInfo GetWidgetInfo(string name)
        {
            Check.NotEmpty(name, "name"); //NOXLATE
            foreach (var wgt in _widgetSet.WidgetInfo)
            {
                if (name.Equals(wgt.Type))
                    return wgt;
            }
            return null;
        }

        /// <summary>
        /// Gets information about all widgets
        /// </summary>
        /// <returns></returns>
        public IWidgetInfo[] GetAllWidgets()
        {
            return new List<IWidgetInfo>(_widgetSet.WidgetInfo).ToArray();
        }

        /// <summary>
        /// Gets information about a specific container
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IApplicationDefinitionContainerInfo GetContainerInfo(string name)
        {
            Check.NotEmpty(name, "name"); //NOXLATE
            foreach (var cnt in _containerSet.ContainerInfo)
            {
                if (name.Equals(cnt.Type))
                    return cnt;
            }
            return null;
        }

        /// <summary>
        /// Gets information about a specific template
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IApplicationDefinitionTemplateInfo GetTemplateInfo(string name)
        {
            Check.NotEmpty(name, "name"); //NOXLATE
            foreach (var tpl in _templateSet.TemplateInfo)
            {
                if (name.Equals(tpl.Name))
                    return tpl;
            }
            return null;
        }

        internal string[] GetDockableWidgetNames(IApplicationDefinition appDef)
        {
            //Key the dockable types
            var dict = new Dictionary<string, IWidgetInfo>();
            foreach (var winfo in GetAllWidgets())
            {
                if (winfo.StandardUi)
                {
                    dict[winfo.Type] = winfo;
                }
            }

            var result = new List<string>();
            foreach (var wset in appDef.WidgetSets)
            {
                foreach (var wgt in wset.Widgets)
                {
                    if (dict.ContainsKey(wgt.Type))
                        result.Add(wgt.Name);
                }
            }
            return result.ToArray();
        }

        internal bool IsWidgetDockable(string widgetType)
        {
            foreach (var winfo in _widgetSet.WidgetInfo)
            {
                if (winfo.Type == widgetType)
                    return winfo.StandardUi;
            }
            return false;
        }
    }
}
