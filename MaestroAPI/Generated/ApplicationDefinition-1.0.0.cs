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
using System.Xml;
namespace OSGeo.MapGuide.MaestroAPI.ApplicationDefinition {
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute("ApplicationDefinition", Namespace="", IsNullable=false)]
    public class ApplicationDefinitionType {
        
		public static readonly string SchemaName = "ApplicationDefinition-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

		private string m_resourceId;
		[System.Xml.Serialization.XmlIgnore()]
		public string ResourceId 
		{ 
			get { return m_resourceId; } 
			set { m_resourceId = value; } 
		}

        private ServerConnectionI m_serverConnection;

        /// <summary>
        /// Gets or sets the connection used in various operations performed on this object
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public ServerConnectionI CurrentConnection
        {
            get { return m_serverConnection; }
            set { m_serverConnection = value; }
        }

        private string m_title;
        
        private string m_templateUrl;
        
        private MapGroupTypeCollection m_mapSet;
        
        private WidgetSetTypeCollection m_widgetSet;
        
        private CustomContentType m_extension;
        
        /// <remarks/>
        public string Title {
            get {
                return this.m_title;
            }
            set {
                this.m_title = value;
            }
        }
        
        /// <remarks/>
        public string TemplateUrl {
            get {
                return this.m_templateUrl;
            }
            set {
                this.m_templateUrl = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("MapGroup", IsNullable=false)]
        public MapGroupTypeCollection MapSet {
            get {
                return this.m_mapSet;
            }
            set {
                this.m_mapSet = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WidgetSet")]
        public WidgetSetTypeCollection WidgetSet {
            get {
                return this.m_widgetSet;
            }
            set {
                this.m_widgetSet = value;
            }
        }
        
        /// <remarks/>
        public CustomContentType Extension {
            get {
                return this.m_extension;
            }
            set {
                this.m_extension = value;
            }
        }

		/// <summary>
		/// Gets the Application Document, used to create new extension elements
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public XmlDocument ApplicationDocument
		{
			get 
			{
				XmlDocument appDoc = null;

                if (this.Extension.Any != null && this.Extension.Any.Length > 0)
					appDoc = this.Extension.Any[0].OwnerDocument;
				else if (this.MapSet != null)
					foreach(MapGroupType mgt in this.MapSet)
					{
						if (mgt.Extension != null && mgt.Extension.Any != null && mgt.Extension.Any.Length > 0)
						{
							appDoc = mgt.Extension.Any[0].OwnerDocument;
							break;
						}

						if (mgt.Map != null)
							foreach (OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType mtx in mgt.Map)
								if (mtx.Extension != null && mtx.Extension.Any != null && mtx.Extension.Any.Length > 0)
								{
									appDoc = mtx.Extension.Any[0].OwnerDocument;
									break;
								}
						if (appDoc != null)
							break;
					}

				if (appDoc == null)
					appDoc = new System.Xml.XmlDocument();

				return appDoc;
			}
		}
    }
    
    /// <remarks/>
    public class MapGroupType {
        
        private MapViewType m_initialView;
        
        private MapTypeCollection m_map;
        
        private CustomContentType m_extension;
        
        private string m_id;
        
        /// <remarks/>
        public MapViewType InitialView {
            get {
                return this.m_initialView;
            }
            set {
                this.m_initialView = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Map")]
        public MapTypeCollection Map {
            get {
                return this.m_map;
            }
            set {
                this.m_map = value;
            }
        }
        
        /// <remarks/>
        public CustomContentType Extension {
            get {
                return this.m_extension;
            }
            set {
                this.m_extension = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id {
            get {
                return this.m_id;
            }
            set {
                this.m_id = value;
            }
        }
    }
    
    /// <remarks/>
    public class MapViewType {
        
        private System.Double m_centerX;
        
        private System.Double m_centerY;
        
        private System.Double m_scale;
        
        /// <remarks/>
        public System.Double CenterX {
            get {
                return this.m_centerX;
            }
            set {
                this.m_centerX = value;
            }
        }
        
        /// <remarks/>
        public System.Double CenterY {
            get {
                return this.m_centerY;
            }
            set {
                this.m_centerY = value;
            }
        }
        
        /// <remarks/>
        public System.Double Scale {
            get {
                return this.m_scale;
            }
            set {
                this.m_scale = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UiWidgetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MapWidgetType))]
    public class WidgetType {
        
        private string m_name;
        
        private string m_type;
        
        private string m_location;
        
        private CustomContentType m_extension;
        
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
        public string Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
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
        public CustomContentType Extension {
            get {
                return this.m_extension;
            }
            set {
                this.m_extension = value;
            }
        }
    }
    
    /// <remarks/>
    public class CustomContentType {
        

		public CustomContentType()
		{
            m_any = new XmlElement[0];
		}

		public CustomContentType(XmlElementCollection col)
		{
			if (col != null)
			{
				m_any = new System.Xml.XmlElement[col.Count];
				for(int i = 0; i < col.Count; i++)
					m_any[i] = col[i];
				InitializeLookup();
			}
		}

        private System.Xml.XmlElement[] m_any;
		private System.Collections.Hashtable m_lookup;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.m_any;
            }
            set {
                this.m_any = value;
            }
        }

		[System.Xml.Serialization.XmlIgnore()]
		public string this[string elementname]
		{
			get
			{
				if (m_lookup == null)
					InitializeLookup();
				return m_lookup.ContainsKey(elementname) ? ((XmlNode)m_lookup[elementname]).InnerXml : null;
			}
			set
			{
				if (m_lookup == null)
					InitializeLookup();
                if (m_any == null)
                    m_any = new XmlElement[0];

				if (value == null)
				{
					if (m_lookup.ContainsKey(elementname))
					{
						for(int i = 0; i < m_any.Length; i++)
							if (m_any[i].Name == elementname)
							{
								System.Xml.XmlElement[] n = new System.Xml.XmlElement[m_any.Length - 1];
								for(int j = 0; j < n.Length; j++)
									n[j] = m_any[j >= i ? j + 1 : j];
									
								m_any = n;
								break;
							}
						m_lookup.Remove(elementname);
					}
				}
				else
				{
					if (m_lookup.ContainsKey(elementname))
						((XmlNode)m_lookup[elementname]).InnerXml = value;
					else
					{
                        if (m_any == null)
                            m_any = new XmlElement[0];

						XmlDocument doc = m_any.Length == 0 ? new XmlDocument() : m_any[0].OwnerDocument;


						XmlElement m = doc.CreateElement(elementname);
						m.InnerXml = value;
						System.Xml.XmlElement[] n = new System.Xml.XmlElement[m_any.Length + 1];
						System.Array.Copy(this.m_any, 0, n, 0, this.m_any.Length);
						m_any = n;
						m_any[m_any.Length - 1] =  m;
						m_lookup.Add(elementname, m);
					}
				}


			}
		}

		private void InitializeLookup()
		{
			m_lookup = new System.Collections.Hashtable();
			if (m_any == null)
				return;
			foreach(XmlNode n in m_any)
				m_lookup[n.Name] = n;
		}
    }
    
    /// <remarks/>
    public class UiWidgetType : WidgetType {
        
        private string m_imageUrl;
        
        private string m_imageClass;
        
        private string m_label;
        
        private string m_tooltip;
        
        private string m_statusText;
        
        private string m_disabled;
        
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
        public string Disabled {
            get {
                return this.m_disabled;
            }
            set {
                this.m_disabled = value;
            }
        }
    }
    
    /// <remarks/>
    public class MapWidgetType : WidgetType {
        
        private string m_mapId;
        
        /// <remarks/>
        public string MapId {
            get {
                return this.m_mapId;
            }
            set {
                this.m_mapId = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WidgetItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FlyoutItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SeparatorItemType))]
    public abstract class UiItemType {
        
        private UiItemFunctionType m_function;
        
        /// <remarks/>
        public UiItemFunctionType Function {
            get {
                return this.m_function;
            }
            set {
                this.m_function = value;
            }
        }
    }
    
    /// <remarks/>
    public enum UiItemFunctionType {
        
        /// <remarks/>
        Separator,
        
        /// <remarks/>
        Widget,
        
        /// <remarks/>
        Flyout,
    }
    
    /// <remarks/>
    public class WidgetItemType : UiItemType {
        
        private string m_widget;
        
        /// <remarks/>
        public string Widget {
            get {
                return this.m_widget;
            }
            set {
                this.m_widget = value;
            }
        }
    }
    
    /// <remarks/>
    public class FlyoutItemType : UiItemType {
        
        private string m_label;
        
        private string m_tooltip;
        
        private string m_imageUrl;
        
        private string m_imageClass;
        
        private UiItemTypeCollection m_item;
        
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
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public UiItemTypeCollection Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
    /// <remarks/>
    public class SeparatorItemType : UiItemType {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UiItemContainerType))]
    public class ContainerType {
        
        private string m_name;
        
        private string m_type;
        
        private string m_position;
        
        private CustomContentType m_extension;
        
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
        public string Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
        
        /// <remarks/>
        public string Position {
            get {
                return this.m_position;
            }
            set {
                this.m_position = value;
            }
        }
        
        /// <remarks/>
        public CustomContentType Extension {
            get {
                return this.m_extension;
            }
            set {
                this.m_extension = value;
            }
        }
    }
    
    /// <remarks/>
    public class UiItemContainerType : ContainerType {
        
        private UiItemTypeCollection m_item;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public UiItemTypeCollection Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
    /// <remarks/>
    public class WidgetSetType {
        
        private ContainerTypeCollection m_container;
        
        private MapWidgetType m_mapWidget;
        
        private WidgetTypeCollection m_widget;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Container")]
        public ContainerTypeCollection Container {
            get {
                return this.m_container;
            }
            set {
                this.m_container = value;
            }
        }
        
        /// <remarks/>
        public MapWidgetType MapWidget {
            get {
                return this.m_mapWidget;
            }
            set {
                this.m_mapWidget = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Widget")]
        public WidgetTypeCollection Widget {
            get {
                return this.m_widget;
            }
            set {
                this.m_widget = value;
            }
        }
    }
    
    /// <remarks/>
    public class MapType {
        
        private string m_type;
        
        private string m_singleTile;
        
        private CustomContentType m_extension;
        
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
        public string SingleTile {
            get {
                return this.m_singleTile;
            }
            set {
                this.m_singleTile = value;
            }
        }
        
        /// <remarks/>
        public CustomContentType Extension {
            get {
                return this.m_extension;
            }
            set {
                this.m_extension = value;
            }
        }
    }
    
    public class MapGroupTypeCollection : System.Collections.CollectionBase {
        
        public MapGroupType this[int idx] {
            get {
                return ((MapGroupType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(MapGroupType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class WidgetSetTypeCollection : System.Collections.CollectionBase {
        
        public WidgetSetType this[int idx] {
            get {
                return ((WidgetSetType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(WidgetSetType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class MapTypeCollection : System.Collections.CollectionBase {
        
        public MapType this[int idx] {
            get {
                return ((MapType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(MapType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class XmlElementCollection : System.Collections.CollectionBase {
        
        public XmlElement this[int idx] {
            get {
                return ((XmlElement)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(XmlElement value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class UiItemTypeCollection : System.Collections.CollectionBase {
        
        public UiItemType this[int idx] {
            get {
                return ((UiItemType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(UiItemType value) {
            return base.InnerList.Add(value);
        }

		public void Insert(int index, UiItemType value)
		{
			base.InnerList.Insert(index, value);
		}

		public int IndexOf(UiItemType value)
		{
			return base.InnerList.IndexOf(value);
		}

    }
    
    public class ContainerTypeCollection : System.Collections.CollectionBase {
        
        public ContainerType this[int idx] {
            get {
                return ((ContainerType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ContainerType value) {
            return base.InnerList.Add(value);
        }

		public void Insert(int index, ContainerType value)
		{
			base.InnerList.Insert(index, value);
		}

		public int IndexOf(ContainerType value)
		{
			return base.InnerList.IndexOf(value);
		}

		public bool Contains(ContainerType value)
		{
			return base.InnerList.Contains(value);
		}
    }
    
    public class WidgetTypeCollection : System.Collections.CollectionBase {
        
        public WidgetType this[int idx] {
            get {
                return ((WidgetType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }

		public WidgetType FindByType(string type)
		{
			foreach(WidgetType t in base.InnerList)
				if (t.Type == type)
					return t;
			return null;
		}

		public WidgetType FindByName(string name)
		{
			foreach(WidgetType t in base.InnerList)
				if (t.Name == name)
					return t;
			return null;
		}

        public int Add(WidgetType value) 
		{
            return base.InnerList.Add(value);
        }
    }
}
