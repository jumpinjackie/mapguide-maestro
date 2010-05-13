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
using System;
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI {
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute("FeatureSource", Namespace="", IsNullable=false)]
    public class FeatureSource {

		private ServerConnectionI m_serverConnection;

		/// <summary>
		/// Gets or sets the connection used in various operations performed on this object
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public ServerConnectionI CurrentConnection
		{
			get { return m_serverConnection; }
			set 
			{ 
				m_serverConnection = value;
				if (this.Extension != null)
					foreach(FeatureSourceTypeExtension ex in this.Extension)
						ex.Parent = this;
			}
		}

		public static readonly string SchemaName = "FeatureSource-1.0.0.xsd";
        
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
        
        private string m_provider;
        private NameValuePairTypeCollection m_parameter;
        private SpatialContextTypeCollection m_supplementalSpatialContextInfo;
        private string m_configurationDocument;
        private string m_longTransaction;
        private FeatureSourceTypeExtensionCollection m_extension;
        private string m_version;

        
        /// <remarks/>
        public string Provider {
            get {
                return this.m_provider;
            }
            set {
                this.m_provider = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Parameter")]
        public NameValuePairTypeCollection Parameter {
            get {
                return this.m_parameter;
            }
            set {
                this.m_parameter = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SupplementalSpatialContextInfo")]
        public SpatialContextTypeCollection SupplementalSpatialContextInfo {
            get {
                return this.m_supplementalSpatialContextInfo;
            }
            set {
                this.m_supplementalSpatialContextInfo = value;
            }
        }
        
        /// <remarks/>
        public string ConfigurationDocument {
            get {
                return this.m_configurationDocument;
            }
            set {
                this.m_configurationDocument = value;
            }
        }
        
        /// <remarks/>
        public string LongTransaction {
            get {
                return this.m_longTransaction;
            }
            set {
                this.m_longTransaction = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Extension")]
        public FeatureSourceTypeExtensionCollection Extension {
            get {
                return this.m_extension;
            }
            set {
                this.m_extension = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version {
            get {
                return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }

		public string[] GetIdentityProperties(string classname)
		{
			if (this.CurrentConnection == null)
				throw new System.Exception("No server set for object");
		
			return this.CurrentConnection.GetIdentityProperties(this.ResourceId, classname);
		}

		public FeatureSetReader SelectFeatures(string query)
		{
			return this.SelectFeatures(query, null);
		}

		public FeatureSetReader SelectFeatures(string query, string classname)
		{
			return SelectFeatures(query, classname, null);
		}

		public FeatureSetReader SelectFeatures(string query, string classname, string[] columns)
		{
            return SelectFeatures(query, classname, columns, null);
		}

        public FeatureSetReader SelectFeatures(string query, string classname, string[] columns, NameValueCollection computedProperties)
        {
            if (this.CurrentConnection == null)
                throw new System.Exception("No server set for object");
            else
            {
                /*if (this.ConfigurationDocument != null && this.ConfigurationDocument.Length > 0 && classname == null)
                {
                    System.IO.MemoryStream data = this.CurrentConnection.GetResourceData(this.ResourceId, this.ConfigurationDocument);
                    //TODO: Decode the xml config document to derive the class name, something like "Default:xxxx"
                    throw new System.MissingMethodException("Currently the method that derives the classname from the featuresource is not complete, please specify it manually");
                }*/

                if (classname == null || classname.Length == 0)
                {
                    try
                    {
                        FeatureSourceDescription items = this.DescribeSource();
                        if (items != null && items.Schemas != null && items.Schemas.Length > 0)
                            classname = items.Schemas[0].Name;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No classname supplied, and the attempt to derive it failed", ex);
                    }
                }

                return this.CurrentConnection.QueryFeatureSource(this.ResourceId, classname, query, columns, computedProperties);
            }
        }

        public Topology.Geometries.IEnvelope GetSpatialExtent(string schema, string geometry)
        {
            return this.GetSpatialExtent(schema, geometry, null);
        }

        public Topology.Geometries.IEnvelope GetSpatialExtent(string schema, string geometry, string filter)
        {
            if (this.CurrentConnection == null)
                throw new System.Exception("No server set for object");
            else
                return this.CurrentConnection.GetSpatialExtent(this.ResourceId, schema, geometry, filter);
        }

		public FdoSpatialContextList GetSpatialInfo()
		{
			if (this.CurrentConnection == null)
				throw new System.Exception("No server set for object");
			else
				return this.CurrentConnection.GetSpatialContextInfo(this.ResourceId, false);
		}

		public FeatureSourceDescription DescribeSource()
		{
			return DescribeSource("");
		}

		public FeatureSourceDescription DescribeSource(string schema)
		{
			if (this.CurrentConnection == null)
				throw new System.Exception("No server set for object");
			else
				return this.CurrentConnection.DescribeFeatureSource(this.ResourceId, schema);
		}

	}
    
    /// <remarks/>
    public class NameValuePairType {
        
        private string m_name;
        
        private string m_value;
        
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
        public string Value {
            get {
                return this.m_value;
            }
            set {
                this.m_value = value;
            }
        }
    }
    
    /// <remarks/>
    public class RelatePropertyType {
        
        private string m_featureClassProperty;
        
        private string m_attributeClassProperty;
        
        /// <remarks/>
        public string FeatureClassProperty {
            get {
                return this.m_featureClassProperty;
            }
            set {
                this.m_featureClassProperty = value;
            }
        }
        
        /// <remarks/>
        public string AttributeClassProperty {
            get {
                return this.m_attributeClassProperty;
            }
            set {
                this.m_attributeClassProperty = value;
            }
        }
    }
    
    /// <remarks/>
    public class AttributeRelateType {
        
        private RelatePropertyTypeCollection m_relateProperty;
        
        private string m_attributeClass;
        
        private string m_resourceId;
        
        private string m_name;
        
        private string m_attributeNameDelimiter;
        
        private RelateTypeEnum m_relateType;
        
        private bool m_relateTypeSpecified;
        
        private bool m_forceOneToOne;
        
        private bool m_forceOneToOneSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RelateProperty")]
        public RelatePropertyTypeCollection RelateProperty {
            get {
                return this.m_relateProperty;
            }
            set {
                this.m_relateProperty = value;
            }
        }
        
        /// <remarks/>
        public string AttributeClass {
            get {
                return this.m_attributeClass;
            }
            set {
                this.m_attributeClass = value;
            }
        }
        
        /// <remarks/>
        public string ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
        
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
        public string AttributeNameDelimiter {
            get {
                return this.m_attributeNameDelimiter;
            }
            set {
                this.m_attributeNameDelimiter = value;
            }
        }
        
        /// <remarks/>
        public RelateTypeEnum RelateType {
            get {
                return this.m_relateType;
            }
            set {
                this.m_relateType = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RelateTypeSpecified {
            get {
                return this.m_relateTypeSpecified;
            }
            set {
                this.m_relateTypeSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ForceOneToOne {
            get {
                return this.m_forceOneToOne;
            }
            set {
                this.m_forceOneToOne = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ForceOneToOneSpecified {
            get {
                return this.m_forceOneToOneSpecified;
            }
            set {
                this.m_forceOneToOneSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    public enum RelateTypeEnum {
        
        /// <remarks/>
        LeftOuter,
        
        /// <remarks/>
        RightOuter,
        
        /// <remarks/>
        Inner,
        
        /// <remarks/>
        Association,
    }
    
    /// <remarks/>
    public class CalculatedPropertyType {
        
        private string m_name;
        
        private string m_expression;
        
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
        public string Expression {
            get {
                return this.m_expression;
            }
            set {
                this.m_expression = value;
            }
        }
    }
    
    /// <remarks/>
    public class FeatureSourceTypeExtension {
        
		internal FeatureSource Parent
		{
			get { return m_parent; }
			set { m_parent = value; }
		}
		private FeatureSource m_parent;

        private CalculatedPropertyTypeCollection m_calculatedProperty;
        
        private AttributeRelateTypeCollection m_attributeRelate;
        
        private string m_name;
        
        private string m_featureClass;

        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalculatedProperty")]
        public CalculatedPropertyTypeCollection CalculatedProperty {
            get {
                return this.m_calculatedProperty;
            }
            set {
                this.m_calculatedProperty = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AttributeRelate")]
        public AttributeRelateTypeCollection AttributeRelate {
            get {
                return this.m_attributeRelate;
            }
            set {
                this.m_attributeRelate = value;
            }
        }
        
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
        public string FeatureClass {
            get {
                return this.m_featureClass;
            }
            set {
                this.m_featureClass = value;
            }
        }

		public FeatureSetReader SelectFeatures()
		{
			return SelectFeatures(null, null);
		}

		public FeatureSetReader SelectFeatures(string query)
		{
			return SelectFeatures(query, null);
		}


		public FeatureSetReader SelectFeatures(string query, string[] columns)
		{
			if (m_parent == null)
				throw new System.Exception("No parent set for extension");
			else
			{
				if (m_parent.CurrentConnection == null)
					throw new System.Exception("No server set for parent");
				else
					return m_parent.CurrentConnection.QueryFeatureSource(m_parent.ResourceId, this.FeatureClass, query, columns);
			}
		}
    }
    
    /// <remarks/>
    public class SpatialContextType {
        
        private string m_name;
        
        private string m_coordinateSystem;
        
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
        public string CoordinateSystem {
            get {
                return this.m_coordinateSystem;
            }
            set {
                this.m_coordinateSystem = value;
            }
        }
    }
    
    public class NameValuePairTypeCollection : System.Collections.CollectionBase {
        
		private System.Collections.Hashtable m_lookup;

        public NameValuePairType this[int idx] 
		{
            get {
                return ((NameValuePairType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }

		private void InitializeLookup()
		{
			m_lookup = new System.Collections.Hashtable();
			if (base.InnerList == null)
				return;
			foreach(NameValuePairType n in base.InnerList)
				m_lookup[n.Name] = n;
		}

        
		[System.Xml.Serialization.XmlIgnore()]
		public string this[string elementname]
		{
			get
			{
				if (m_lookup == null)
					InitializeLookup();
				return m_lookup.ContainsKey(elementname) ? ((NameValuePairType)m_lookup[elementname]).Value : null;
			}
			set
			{
				if (m_lookup == null)
					InitializeLookup();
				if (value == null)
				{
					if (m_lookup.ContainsKey(elementname))
					{
						for(int i = 0; i < base.InnerList.Count; i++)
							if (this[i].Value == elementname)
							{
								base.InnerList.RemoveAt(i);
								break;
							}
						m_lookup.Remove(elementname);
					}
				}
				else
				{
					if (m_lookup.ContainsKey(elementname))
						((NameValuePairType)m_lookup[elementname]).Value = value;
					else
					{
						NameValuePairType nv = new NameValuePairType();
						nv.Name = elementname;
						nv.Value = value;
						base.InnerList.Add(nv);
						m_lookup.Add(elementname, nv);
					}
				}


			}
		}

        public int Add(NameValuePairType value) 
			   {
            return base.InnerList.Add(value);
        }
    }
    
    public class SpatialContextTypeCollection : System.Collections.CollectionBase {
        
        public SpatialContextType this[int idx] {
            get {
                return ((SpatialContextType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(SpatialContextType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FeatureSourceTypeExtensionCollection : System.Collections.CollectionBase {
        
        public FeatureSourceTypeExtension this[int idx] {
            get {
                return ((FeatureSourceTypeExtension)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FeatureSourceTypeExtension value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class RelatePropertyTypeCollection : System.Collections.CollectionBase {
        
        public RelatePropertyType this[int idx] {
            get {
                return ((RelatePropertyType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(RelatePropertyType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class CalculatedPropertyTypeCollection : System.Collections.CollectionBase {
        
        public CalculatedPropertyType this[int idx] {
            get {
                return ((CalculatedPropertyType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(CalculatedPropertyType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class AttributeRelateTypeCollection : System.Collections.CollectionBase {
        
        public AttributeRelateType this[int idx] {
            get {
                return ((AttributeRelateType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(AttributeRelateType value) {
            return base.InnerList.Add(value);
        }
    }
}
