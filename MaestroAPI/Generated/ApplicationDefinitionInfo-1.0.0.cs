#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI {
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class ApplicationDefinitionTemplateInfoSet {
        
        private ApplicationDefinitionTemplateInfoTypeCollection m_templateInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TemplateInfo")]
        public ApplicationDefinitionTemplateInfoTypeCollection TemplateInfo {
            get {
                return this.m_templateInfo;
            }
            set {
                this.m_templateInfo = value;
            }
        }
    }
    
    /// <remarks/>
    public class ApplicationDefinitionTemplateInfoType {
        
        private string m_name;
        
        private string m_locationUrl;
        
        private string m_description;
        
        private string m_previewImageUrl;
        
        private ApplicationDefinitionPanelTypeCollection m_panel;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string LocationUrl {
            get {
                return this.m_locationUrl;
            }
            set {
                this.m_locationUrl = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        public string PreviewImageUrl {
            get {
                return this.m_previewImageUrl;
            }
            set {
                this.m_previewImageUrl = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Panel")]
        public ApplicationDefinitionPanelTypeCollection Panel {
            get {
                return this.m_panel;
            }
            set {
                this.m_panel = value;
            }
        }
    }
    
    /// <remarks/>
    public class ApplicationDefinitionPanelType {
        
        private string m_name;
        
        private string m_label;
        
        private string m_description;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class ApplicationDefinitionWidgetInfoSet {
        
        private ApplicationDefinitionWidgetInfoTypeCollection m_widgetInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WidgetInfo")]
        public ApplicationDefinitionWidgetInfoTypeCollection WidgetInfo {
            get {
                return this.m_widgetInfo;
            }
            set {
                this.m_widgetInfo = value;
            }
        }
    }
    
    /// <remarks/>
    public class ApplicationDefinitionWidgetInfoType {
        
        private string m_type;
        
        private string m_localizedType;
        
        private string m_description;
        
        private string m_location;
        
        private string m_label;
        
        private string m_tooltip;
        
        private string m_statusText;
        
        private string m_imageUrl;
        
        private string m_imageClass;
        
        private bool m_standardUi;
        
        private bool m_standardUiSpecified;
        
        private StringCollection m_containableBy;
        
        private ApplicationDefinitionWidgetParameterTypeCollection m_parameter;
        
        /// <remarks/>
        public string Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
        
        /// <remarks/>
        public string LocalizedType {
            get {
                return this.m_localizedType;
            }
            set {
                this.m_localizedType = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        public string Location {
            get {
                return this.m_location;
            }
            set {
                this.m_location = value;
            }
        }
        
        /// <remarks/>
        public string Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        public string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                this.m_tooltip = value;
            }
        }
        
        /// <remarks/>
        public string StatusText {
            get {
                return this.m_statusText;
            }
            set {
                this.m_statusText = value;
            }
        }
        
        /// <remarks/>
        public string ImageUrl {
            get {
                return this.m_imageUrl;
            }
            set {
                this.m_imageUrl = value;
            }
        }
        
        /// <remarks/>
        public string ImageClass {
            get {
                return this.m_imageClass;
            }
            set {
                this.m_imageClass = value;
            }
        }
        
        /// <remarks/>
        public bool StandardUi {
            get {
                return this.m_standardUi;
            }
            set {
                this.m_standardUi = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StandardUiSpecified {
            get {
                return this.m_standardUiSpecified;
            }
            set {
                this.m_standardUiSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ContainableBy")]
        public StringCollection ContainableBy {
            get {
                return this.m_containableBy;
            }
            set {
                this.m_containableBy = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Parameter")]
        public ApplicationDefinitionWidgetParameterTypeCollection Parameter {
            get {
                return this.m_parameter;
            }
            set {
                this.m_parameter = value;
            }
        }
    }
    
    /// <remarks/>
    public class ApplicationDefinitionWidgetParameterType {
        
        private string m_name;
        
        private string m_description;
        
        private string m_type;
        
        private string m_label;
        
        private string m_min;
        
        private string m_max;
        
        private AllowedValueTypeCollection m_allowedValue;
        
        private string m_defaultValue;
        
        private bool m_isMandatory;
        
        private bool m_isMandatorySpecified;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        public string Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
        
        /// <remarks/>
        public string Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        public string Min {
            get {
                return this.m_min;
            }
            set {
                this.m_min = value;
            }
        }
        
        /// <remarks/>
        public string Max {
            get {
                return this.m_max;
            }
            set {
                this.m_max = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AllowedValue")]
        public AllowedValueTypeCollection AllowedValue {
            get {
                return this.m_allowedValue;
            }
            set {
                this.m_allowedValue = value;
            }
        }
        
        /// <remarks/>
        public string DefaultValue {
            get {
                return this.m_defaultValue;
            }
            set {
                this.m_defaultValue = value;
            }
        }
        
        /// <remarks/>
        public bool IsMandatory {
            get {
                return this.m_isMandatory;
            }
            set {
                this.m_isMandatory = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsMandatorySpecified {
            get {
                return this.m_isMandatorySpecified;
            }
            set {
                this.m_isMandatorySpecified = value;
            }
        }
    }
    
    /// <remarks/>
    public class AllowedValueType {
        
        private string m_name;
        
        private string m_label;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class ApplicationDefinitionContainerInfoSet {
        
        private ApplicationDefinitionContainerInfoTypeCollection m_containerInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ContainerInfo")]
        public ApplicationDefinitionContainerInfoTypeCollection ContainerInfo {
            get {
                return this.m_containerInfo;
            }
            set {
                this.m_containerInfo = value;
            }
        }
    }
    
    /// <remarks/>
    public class ApplicationDefinitionContainerInfoType {
        
        private string m_type;
        
        private string m_localizedType;
        
        private string m_description;
        
        private string m_previewImageUrl;
        
        /// <remarks/>
        public string Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
        
        /// <remarks/>
        public string LocalizedType {
            get {
                return this.m_localizedType;
            }
            set {
                this.m_localizedType = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        public string PreviewImageUrl {
            get {
                return this.m_previewImageUrl;
            }
            set {
                this.m_previewImageUrl = value;
            }
        }
    }
    
    public class ApplicationDefinitionTemplateInfoTypeCollection : System.Collections.CollectionBase {
        
        public ApplicationDefinitionTemplateInfoType this[int idx] {
            get {
                return ((ApplicationDefinitionTemplateInfoType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ApplicationDefinitionTemplateInfoType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ApplicationDefinitionPanelTypeCollection : System.Collections.CollectionBase {
        
        public ApplicationDefinitionPanelType this[int idx] {
            get {
                return ((ApplicationDefinitionPanelType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ApplicationDefinitionPanelType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ApplicationDefinitionWidgetInfoTypeCollection : System.Collections.CollectionBase {
        
        public ApplicationDefinitionWidgetInfoType this[int idx] {
            get {
                return ((ApplicationDefinitionWidgetInfoType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ApplicationDefinitionWidgetInfoType value) {
            return base.InnerList.Add(value);
        }
    }
    
   
    public class ApplicationDefinitionWidgetParameterTypeCollection : System.Collections.CollectionBase {
        
        public ApplicationDefinitionWidgetParameterType this[int idx] {
            get {
                return ((ApplicationDefinitionWidgetParameterType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ApplicationDefinitionWidgetParameterType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class AllowedValueTypeCollection : System.Collections.CollectionBase {
        
        public AllowedValueType this[int idx] {
            get {
                return ((AllowedValueType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(AllowedValueType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ApplicationDefinitionContainerInfoTypeCollection : System.Collections.CollectionBase {
        
        public ApplicationDefinitionContainerInfoType this[int idx] {
            get {
                return ((ApplicationDefinitionContainerInfoType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ApplicationDefinitionContainerInfoType value) {
            return base.InnerList.Add(value);
        }
    }
}
