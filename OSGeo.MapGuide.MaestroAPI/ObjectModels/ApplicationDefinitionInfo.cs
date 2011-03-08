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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.ApplicationDefinition_1_0_0
{
    using OSGeo.MapGuide.MaestroAPI;
    using System.Xml.Serialization;

    partial class ApplicationDefinitionWidgetInfoSet : IApplicationDefinitionWidgetInfoSet
    {
        [XmlIgnore]
        IEnumerable<IWidgetInfo> IApplicationDefinitionWidgetInfoSet.WidgetInfo
        {
            get 
            {
                foreach (var w in this.WidgetInfo)
                {
                    yield return w;
                }
            }
        }
    }

    partial class ApplicationDefinitionContainerInfoSet : IApplicationDefinitionContainerInfoSet
    {
        [XmlIgnore]
        IEnumerable<IApplicationDefinitionContainerInfo> IApplicationDefinitionContainerInfoSet.ContainerInfo
        {
            get 
            {
                foreach (var c in this.ContainerInfo)
                {
                    yield return c;
                }
            }
        }
    }

    partial class ApplicationDefinitionTemplateInfoSet : IApplicationDefinitionTemplateInfoSet
    {
        [XmlIgnore]
        IEnumerable<IApplicationDefinitionTemplateInfo> IApplicationDefinitionTemplateInfoSet.TemplateInfo
        {
            get 
            {
                foreach (var t in this.TemplateInfo)
                {
                    yield return t;
                }
            }
        }
    }

    partial class AllowedValueType : IAllowedValue
    {

    }

    partial class ApplicationDefinitionTemplateInfoType : IApplicationDefinitionTemplateInfo
    {
        [XmlIgnore]
        IEnumerable<IApplicationDefinitionPanel> IApplicationDefinitionTemplateInfo.Panels
        {
            get 
            {
                foreach (var p in this.Panel)
                {
                    yield return p;
                }
            }
        }
    }

    partial class ApplicationDefinitionContainerInfoType : IApplicationDefinitionContainerInfo
    {

    }

    partial class ApplicationDefinitionPanelType : IApplicationDefinitionPanel
    {

    }

    partial class ApplicationDefinitionWidgetInfoType : IWidgetInfo
    {
        [XmlIgnore]
        string[] IWidgetInfo.ContainableBy
        {
            get { return this.ContainableBy.ToArray(); }
        }

        [XmlIgnore]
        public IWidgetParameter[] Parameters
        {
            get 
            {
                List<IWidgetParameter> param = new List<IWidgetParameter>();
                foreach (var p in this.Parameter)
                {
                    param.Add(p);
                }
                return param.ToArray();
            }
        }
    }

    partial class ApplicationDefinitionWidgetParameterType : IWidgetParameter
    {
        [XmlIgnore]
        IAllowedValue[] IWidgetParameter.AllowedValue
        {
            get 
            {
                List<IAllowedValue> values = new List<IAllowedValue>();
                foreach (var v in this.AllowedValue)
                {
                    values.Add(v);
                }
                return values.ToArray();
            }
        }
    }
}
