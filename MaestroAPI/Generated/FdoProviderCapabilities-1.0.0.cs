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
namespace OSGeo.MapGuide.MaestroAPI {
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute("FeatureProviderCapabilities", Namespace="", IsNullable=false)]
    public class FdoProviderCapabilities {
        
		public static readonly string SchemaName = "FdoProviderCapabilities-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        private FdoProviderCapabilitiesProvider m_provider;
        
        private FdoProviderCapabilitiesConnection m_connection;
        
        private FdoProviderCapabilitiesSchema m_schema;
        
        private FdoProviderCapabilitiesCommand m_command;
        
        private FdoProviderCapabilitiesFilter m_filter;
        
        private FdoProviderCapabilitiesExpression m_expression;
        
        private FdoProviderCapabilitiesRaster m_raster;
        
        private FdoProviderCapabilitiesTopology m_topology;
        
        private FdoProviderCapabilitiesGeometry m_geometry;
        
        /// <remarks/>
        public FdoProviderCapabilitiesProvider Provider {
            get {
                return this.m_provider;
            }
            set {
                this.m_provider = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesConnection Connection {
            get {
                return this.m_connection;
            }
            set {
                this.m_connection = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesSchema Schema {
            get {
                return this.m_schema;
            }
            set {
                this.m_schema = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesCommand Command {
            get {
                return this.m_command;
            }
            set {
                this.m_command = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesFilter Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesExpression Expression {
            get {
                return this.m_expression;
            }
            set {
                this.m_expression = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesRaster Raster {
            get {
                return this.m_raster;
            }
            set {
                this.m_raster = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesTopology Topology {
            get {
                return this.m_topology;
            }
            set {
                this.m_topology = value;
            }
        }
        
        /// <remarks/>
        public FdoProviderCapabilitiesGeometry Geometry {
            get {
                return this.m_geometry;
            }
            set {
                this.m_geometry = value;
            }
        }
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesProvider {
        
        private string m_name;
        
        private string m_value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
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
    public class FdoProviderCapabilitiesGeometry {
        
        private FdoProviderCapabilitiesGeometryTypeCollection m_types;
        
        private FdoProviderCapabilitiesGeometryType1Collection m_components;
        
        private string m_dimensionality;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable=false)]
        public FdoProviderCapabilitiesGeometryTypeCollection Types {
            get {
                return this.m_types;
            }
            set {
                this.m_types = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable=false)]
        public FdoProviderCapabilitiesGeometryType1Collection Components {
            get {
                return this.m_components;
            }
            set {
                this.m_components = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Dimensionality {
            get {
                return this.m_dimensionality;
            }
            set {
                this.m_dimensionality = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesGeometryType {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        Point,
        
        /// <remarks/>
        LineString,
        
        /// <remarks/>
        Polygon,
        
        /// <remarks/>
        MultiPoint,
        
        /// <remarks/>
        MultiLineString,
        
        /// <remarks/>
        MultiPolygon,
        
        /// <remarks/>
        MultiGeometry,
        
        /// <remarks/>
        CurveString,
        
        /// <remarks/>
        CurvePolygon,
        
        /// <remarks/>
        MultiCurveString,
        
        /// <remarks/>
        MultiCurvePolygon,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="FdoProviderCapabilitiesGeometryLineType")]
    public enum FdoProviderCapabilitiesGeometryType1 {
        
        /// <remarks/>
        LinearRing,
        
        /// <remarks/>
        ArcSegment,
        
        /// <remarks/>
        LinearSegment,
        
        /// <remarks/>
        CurveRing,
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesTopology {
        
        private bool m_supportsTopology;
        
        private bool m_supportsTopologicalHierarchy;
        
        private bool m_breaksCurveCrossingsAutomatically;
        
        private bool m_activatesTopologyByArea;
        
        private bool m_constrainsFeatureMovements;
        
        /// <remarks/>
        public bool SupportsTopology {
            get {
                return this.m_supportsTopology;
            }
            set {
                this.m_supportsTopology = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsTopologicalHierarchy {
            get {
                return this.m_supportsTopologicalHierarchy;
            }
            set {
                this.m_supportsTopologicalHierarchy = value;
            }
        }
        
        /// <remarks/>
        public bool BreaksCurveCrossingsAutomatically {
            get {
                return this.m_breaksCurveCrossingsAutomatically;
            }
            set {
                this.m_breaksCurveCrossingsAutomatically = value;
            }
        }
        
        /// <remarks/>
        public bool ActivatesTopologyByArea {
            get {
                return this.m_activatesTopologyByArea;
            }
            set {
                this.m_activatesTopologyByArea = value;
            }
        }
        
        /// <remarks/>
        public bool ConstrainsFeatureMovements {
            get {
                return this.m_constrainsFeatureMovements;
            }
            set {
                this.m_constrainsFeatureMovements = value;
            }
        }
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesRaster {
        
        private bool m_supportsRaster;
        
        private bool m_supportsStitching;
        
        private bool m_supportsSubsampling;
        
        /// <remarks/>
        public bool SupportsRaster {
            get {
                return this.m_supportsRaster;
            }
            set {
                this.m_supportsRaster = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsStitching {
            get {
                return this.m_supportsStitching;
            }
            set {
                this.m_supportsStitching = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsSubsampling {
            get {
                return this.m_supportsSubsampling;
            }
            set {
                this.m_supportsSubsampling = value;
            }
        }
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition {
        
        private string m_name;
        
        private string m_description;
        
        private FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType m_dataType;
        
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
        public FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType DataType {
            get {
                return this.m_dataType;
            }
            set {
                this.m_dataType = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType {
        
        /// <remarks/>
        Boolean,
        
        /// <remarks/>
        Byte,
        
        /// <remarks/>
        DateTime,
        
        /// <remarks/>
        Decimal,
        
        /// <remarks/>
        Double,
        
        /// <remarks/>
        Int16,
        
        /// <remarks/>
        Int32,
        
        /// <remarks/>
        Int64,
        
        /// <remarks/>
        Single,
        
        /// <remarks/>
        String,
        
        /// <remarks/>
        BLOB,
        
        /// <remarks/>
        CLOB,
        
        /// <remarks/>
        UniqueID,

        /// <remarks/>
        Void,
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesExpressionFunctionDefinition {
        
        private string m_name;
        
        private string m_description;
        
        private string m_returnType;
        
        private FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionCollection m_argumentDefinitionList;
        
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
        public string ReturnType {
            get {
                return this.m_returnType;
            }
            set {
                this.m_returnType = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ArgumentDefinition", IsNullable=false)]
        public FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionCollection ArgumentDefinitionList {
            get {
                return this.m_argumentDefinitionList;
            }
            set {
                this.m_argumentDefinitionList = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesExpressionFunctionDefinitionReturnType {
        
        /// <remarks/>
        Boolean,
        
        /// <remarks/>
        Byte,
        
        /// <remarks/>
        DateTime,
        
        /// <remarks/>
        Decimal,
        
        /// <remarks/>
        Double,
        
        /// <remarks/>
        Int16,
        
        /// <remarks/>
        Int32,
        
        /// <remarks/>
        Int64,
        
        /// <remarks/>
        Single,
        
        /// <remarks/>
        String,
        
        /// <remarks/>
        BLOB,
        
        /// <remarks/>
        CLOB,
        
        /// <remarks/>
        UniqueID,
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesExpression {
        
        private FdoProviderCapabilitiesExpressionNameCollection m_type;
        
        private FdoProviderCapabilitiesExpressionFunctionDefinitionCollection m_functionDefinitionList;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Name", IsNullable=false)]
        public FdoProviderCapabilitiesExpressionNameCollection Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FunctionDefinition", IsNullable=false)]
        public FdoProviderCapabilitiesExpressionFunctionDefinitionCollection FunctionDefinitionList {
            get {
                return this.m_functionDefinitionList;
            }
            set {
                this.m_functionDefinitionList = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesExpressionName {
        
        /// <remarks/>
        Basic,
        
        /// <remarks/>
        Function,
        
        /// <remarks/>
        Parameter,
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesFilter {
        
        private FdoProviderCapabilitiesFilterTypeCollection m_condition;
        
        private FdoProviderCapabilitiesFilterOperationCollection m_spatial;
        
        private FdoProviderCapabilitiesFilterOperation1Collection m_distance;
        
        private bool m_supportsGeodesicDistance;
        
        private bool m_supportsNonLiteralGeometricOperations;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable=false)]
        public FdoProviderCapabilitiesFilterTypeCollection Condition {
            get {
                return this.m_condition;
            }
            set {
                this.m_condition = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Operation", IsNullable=false)]
        public FdoProviderCapabilitiesFilterOperationCollection Spatial {
            get {
                return this.m_spatial;
            }
            set {
                this.m_spatial = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Operation", IsNullable=false)]
        public FdoProviderCapabilitiesFilterOperation1Collection Distance {
            get {
                return this.m_distance;
            }
            set {
                this.m_distance = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsGeodesicDistance {
            get {
                return this.m_supportsGeodesicDistance;
            }
            set {
                this.m_supportsGeodesicDistance = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsNonLiteralGeometricOperations {
            get {
                return this.m_supportsNonLiteralGeometricOperations;
            }
            set {
                this.m_supportsNonLiteralGeometricOperations = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesFilterType {
        
        /// <remarks/>
        Comparison,
        
        /// <remarks/>
        Like,
        
        /// <remarks/>
        In,
        
        /// <remarks/>
        Null,
        
        /// <remarks/>
        Spatial,
        
        /// <remarks/>
        Distance,
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesFilterOperation {
        
        /// <remarks/>
        Contains,
        
        /// <remarks/>
        Crosses,
        
        /// <remarks/>
        Disjoint,
        
        /// <remarks/>
        Equals,
        
        /// <remarks/>
        Intersects,
        
        /// <remarks/>
        Overlaps,
        
        /// <remarks/>
        Touches,
        
        /// <remarks/>
        Within,
        
        /// <remarks/>
        CoveredBy,
        
        /// <remarks/>
        Inside,
        
        /// <remarks/>
        EnvelopeIntersects,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="FdoProviderCapabilitiesFilterGeometryOperation")]
    public enum FdoProviderCapabilitiesFilterOperation1 {
        
        /// <remarks/>
        Beyond,
        
        /// <remarks/>
        Within,
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesCommand {
        
        private FdoProviderCapabilitiesCommandNameCollection m_supportedCommands;
        
        private bool m_supportsParameters;
        
        private bool m_supportsTimeout;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Name", IsNullable=false)]
        public FdoProviderCapabilitiesCommandNameCollection SupportedCommands {
            get {
                return this.m_supportedCommands;
            }
            set {
                this.m_supportedCommands = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsParameters {
            get {
                return this.m_supportsParameters;
            }
            set {
                this.m_supportsParameters = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsTimeout {
            get {
                return this.m_supportsTimeout;
            }
            set {
                this.m_supportsTimeout = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesCommandName {
        
        /// <remarks/>
        Select,
        
        /// <remarks/>
        Insert,
        
        /// <remarks/>
        Delete,
        
        /// <remarks/>
        Update,
        
        /// <remarks/>
        DescribeSchema,
        
        /// <remarks/>
        ApplySchema,
        
        /// <remarks/>
        DestroySchema,
        
        /// <remarks/>
        ActivateSpatialContext,
        
        /// <remarks/>
        CreateSpatialContext,
        
        /// <remarks/>
        DestroySpatialContext,
        
        /// <remarks/>
        GetSpatialContexts,
        
        /// <remarks/>
        CreateMeasureUnit,
        
        /// <remarks/>
        DestroyMeasureUnit,
        
        /// <remarks/>
        GetMeasureUnits,
        
        /// <remarks/>
        SQLCommand,
        
        /// <remarks/>
        AcquireLock,
        
        /// <remarks/>
        GetLockInfo,
        
        /// <remarks/>
        GetLockedObjects,
        
        /// <remarks/>
        GetLockOwners,
        
        /// <remarks/>
        ReleaseLock,
        
        /// <remarks/>
        ActivateLongTransaction,
        
        /// <remarks/>
        CommitLongTransaction,
        
        /// <remarks/>
        CreateLongTransaction,
        
        /// <remarks/>
        GetLongTransactions,
        
        /// <remarks/>
        FreezeLongTransaction,
        
        /// <remarks/>
        RollbackLongTransaction,
        
        /// <remarks/>
        ActivateLongTransactionCheckpoint,
        
        /// <remarks/>
        CreateLongTransactionCheckpoint,
        
        /// <remarks/>
        GetLongTransactionCheckpoints,
        
        /// <remarks/>
        RollbackLongTransactionCheckpoint,
        
        /// <remarks/>
        ChangeLongTransactionPrivileges,
        
        /// <remarks/>
        GetLongTransactionPrivileges,
        
        /// <remarks/>
        ChangeLongTransactionSet,
        
        /// <remarks/>
        GetLongTransactionsInSet,
        
        /// <remarks/>
        FirstProviderCommand,
        
        /// <remarks/>
        DeactivateLongTransaction,
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesSchema {
        
        private FdoProviderCapabilitiesSchemaTypeCollection m_class;
        
        private FdoProviderCapabilitiesSchemaType1Collection m_data;
        
        private bool m_supportsInheritance;
        
        private bool m_supportsMultipleSchemas;
        
        private bool m_supportsObjectProperties;
        
        private bool m_supportsAssociationProperties;
        
        private bool m_supportsSchemaOverrides;
        
        private bool m_supportsNetworkModel;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable=false)]
        public FdoProviderCapabilitiesSchemaTypeCollection Class {
            get {
                return this.m_class;
            }
            set {
                this.m_class = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable=false)]
        public FdoProviderCapabilitiesSchemaType1Collection Data {
            get {
                return this.m_data;
            }
            set {
                this.m_data = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsInheritance {
            get {
                return this.m_supportsInheritance;
            }
            set {
                this.m_supportsInheritance = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsMultipleSchemas {
            get {
                return this.m_supportsMultipleSchemas;
            }
            set {
                this.m_supportsMultipleSchemas = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsObjectProperties {
            get {
                return this.m_supportsObjectProperties;
            }
            set {
                this.m_supportsObjectProperties = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsAssociationProperties {
            get {
                return this.m_supportsAssociationProperties;
            }
            set {
                this.m_supportsAssociationProperties = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsSchemaOverrides {
            get {
                return this.m_supportsSchemaOverrides;
            }
            set {
                this.m_supportsSchemaOverrides = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsNetworkModel {
            get {
                return this.m_supportsNetworkModel;
            }
            set {
                this.m_supportsNetworkModel = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesSchemaType {
        
        /// <remarks/>
        Class,
        
        /// <remarks/>
        FeatureClass,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="FdoProviderCapabilitiesSchemaDataType")]
    public enum FdoProviderCapabilitiesSchemaType1 {
        
        /// <remarks/>
        Boolean,
        
        /// <remarks/>
        Byte,
        
        /// <remarks/>
        DateTime,
        
        /// <remarks/>
        Decimal,
        
        /// <remarks/>
        Double,
        
        /// <remarks/>
        Int16,
        
        /// <remarks/>
        Int32,
        
        /// <remarks/>
        Int64,
        
        /// <remarks/>
        Single,
        
        /// <remarks/>
        String,
        
        /// <remarks/>
        BLOB,
        
        /// <remarks/>
        CLOB,
        
        /// <remarks/>
        UniqueID,
    }
    
    /// <remarks/>
    public class FdoProviderCapabilitiesConnection {
        
        private FdoProviderCapabilitiesConnectionThreadCapability m_threadCapability;
        
        private FdoProviderCapabilitiesConnectionTypeCollection m_spatialContextExtent;
        
        private bool m_supportsLocking;
        
        private bool m_supportsTimeout;
        
        private bool m_supportsTransactions;
        
        private bool m_supportsLongTransactions;
        
        private bool m_supportsSQL;
        
        private bool m_supportsConfiguration;
        
        /// <remarks/>
        public FdoProviderCapabilitiesConnectionThreadCapability ThreadCapability {
            get {
                return this.m_threadCapability;
            }
            set {
                this.m_threadCapability = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable=false)]
        public FdoProviderCapabilitiesConnectionTypeCollection SpatialContextExtent {
            get {
                return this.m_spatialContextExtent;
            }
            set {
                this.m_spatialContextExtent = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsLocking {
            get {
                return this.m_supportsLocking;
            }
            set {
                this.m_supportsLocking = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsTimeout {
            get {
                return this.m_supportsTimeout;
            }
            set {
                this.m_supportsTimeout = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsTransactions {
            get {
                return this.m_supportsTransactions;
            }
            set {
                this.m_supportsTransactions = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsLongTransactions {
            get {
                return this.m_supportsLongTransactions;
            }
            set {
                this.m_supportsLongTransactions = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsSQL {
            get {
                return this.m_supportsSQL;
            }
            set {
                this.m_supportsSQL = value;
            }
        }
        
        /// <remarks/>
        public bool SupportsConfiguration {
            get {
                return this.m_supportsConfiguration;
            }
            set {
                this.m_supportsConfiguration = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesConnectionThreadCapability {
        
        /// <remarks/>
        SingleThreaded,
        
        /// <remarks/>
        PerConnectionThreaded,
        
        /// <remarks/>
        PerCommandThreaded,
        
        /// <remarks/>
        MultiThreaded,
    }
    
    /// <remarks/>
    public enum FdoProviderCapabilitiesConnectionType {
        
        /// <remarks/>
        Static,
        
        /// <remarks/>
        Dynamic,
    }
    
    public class FdoProviderCapabilitiesGeometryTypeCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesGeometryType this[int idx] {
            get {
                return ((FdoProviderCapabilitiesGeometryType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesGeometryType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesGeometryType1Collection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesGeometryType1 this[int idx] {
            get {
                return ((FdoProviderCapabilitiesGeometryType1)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesGeometryType1 value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition this[int idx] {
            get {
                return ((FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesExpressionNameCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesExpressionName this[int idx] {
            get {
                return ((FdoProviderCapabilitiesExpressionName)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesExpressionName value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesExpressionFunctionDefinitionCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesExpressionFunctionDefinition this[int idx] {
            get {
                return ((FdoProviderCapabilitiesExpressionFunctionDefinition)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesExpressionFunctionDefinition value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesFilterTypeCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesFilterType this[int idx] {
            get {
                return ((FdoProviderCapabilitiesFilterType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesFilterType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesFilterOperationCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesFilterOperation this[int idx] {
            get {
                return ((FdoProviderCapabilitiesFilterOperation)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesFilterOperation value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesFilterOperation1Collection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesFilterOperation1 this[int idx] {
            get {
                return ((FdoProviderCapabilitiesFilterOperation1)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesFilterOperation1 value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesCommandNameCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesCommandName this[int idx] {
            get {
                return ((FdoProviderCapabilitiesCommandName)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesCommandName value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesSchemaTypeCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesSchemaType this[int idx] {
            get {
                return ((FdoProviderCapabilitiesSchemaType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesSchemaType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesSchemaType1Collection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesSchemaType1 this[int idx] {
            get {
                return ((FdoProviderCapabilitiesSchemaType1)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesSchemaType1 value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FdoProviderCapabilitiesConnectionTypeCollection : System.Collections.CollectionBase {
        
        public FdoProviderCapabilitiesConnectionType this[int idx] {
            get {
                return ((FdoProviderCapabilitiesConnectionType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoProviderCapabilitiesConnectionType value) {
            return base.InnerList.Add(value);
        }
    }
}
