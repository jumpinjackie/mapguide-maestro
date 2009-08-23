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
namespace OSGeo.MapGuide.MaestroAPI 
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class LoadProcedure : LoadProcedureTypeType {
	
		public static readonly string SchemaName = "LoadProcedure-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

	}
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LoadProcedure))]
    public class LoadProcedureTypeType {
        
        private LoadProcedureType m_item;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ShpLoadProcedure", typeof(ShpLoadProcedureType))]
        [System.Xml.Serialization.XmlElementAttribute("DwfLoadProcedure", typeof(DwfLoadProcedureType))]
        [System.Xml.Serialization.XmlElementAttribute("SdfLoadProcedure", typeof(SdfLoadProcedureType))]
        [System.Xml.Serialization.XmlElementAttribute("RasterLoadProcedure", typeof(RasterLoadProcedureType))]
        [System.Xml.Serialization.XmlElementAttribute("DwgLoadProcedure", typeof(DwgLoadProcedureType))]
        public LoadProcedureType Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
    /// <remarks/>
    public class ShpLoadProcedureType : LoadProcedureType {
        
        private System.Double m_generalization;
        
        private bool m_convertToSdf;
        
        /// <remarks/>
        public System.Double Generalization {
            get {
                return this.m_generalization;
            }
            set {
                this.m_generalization = value;
            }
        }
        
        /// <remarks/>
        public bool ConvertToSdf {
            get {
                return this.m_convertToSdf;
            }
            set {
                this.m_convertToSdf = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SdfLoadProcedureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DwfLoadProcedureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RasterLoadProcedureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DwgLoadProcedureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ShpLoadProcedureType))]
    public class LoadProcedureType {
        
        private StringCollection m_sourceFile;
        
        private string m_rootPath;
        
        private string m_coordinateSystem;
        
        private bool m_generateSpatialDataSources;
        
        private string m_spatialDataSourcesPath;
        
        private string m_spatialDataSourcesFolder;
        
        private bool m_generateLayers;
        
        private string m_layersPath;
        
        private string m_layersFolder;
        
        private bool m_generateMaps;
        
        private bool m_generateMapsSpecified;
        
        private string m_mapsPath;
        
        private string m_mapsFolder;
        
        private bool m_generateSymbolLibraries;
        
        private bool m_generateSymbolLibrariesSpecified;
        
        private string m_symbolLibrariesPath;
        
        private string m_symbolLibrariesFolder;
        
        private StringCollection m_resourceId;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SourceFile")]
        public StringCollection SourceFile {
            get {
                return this.m_sourceFile;
            }
            set {
                this.m_sourceFile = value;
            }
        }
        
        /// <remarks/>
        public string RootPath {
            get {
                return this.m_rootPath;
            }
            set {
                this.m_rootPath = value;
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
        
        /// <remarks/>
        public bool GenerateSpatialDataSources {
            get {
                return this.m_generateSpatialDataSources;
            }
            set {
                this.m_generateSpatialDataSources = value;
            }
        }
        
        /// <remarks/>
        public string SpatialDataSourcesPath {
            get {
                return this.m_spatialDataSourcesPath;
            }
            set {
                this.m_spatialDataSourcesPath = value;
            }
        }
        
        /// <remarks/>
        public string SpatialDataSourcesFolder {
            get {
                return this.m_spatialDataSourcesFolder;
            }
            set {
                this.m_spatialDataSourcesFolder = value;
            }
        }
        
        /// <remarks/>
        public bool GenerateLayers {
            get {
                return this.m_generateLayers;
            }
            set {
                this.m_generateLayers = value;
            }
        }
        
        /// <remarks/>
        public string LayersPath {
            get {
                return this.m_layersPath;
            }
            set {
                this.m_layersPath = value;
            }
        }
        
        /// <remarks/>
        public string LayersFolder {
            get {
                return this.m_layersFolder;
            }
            set {
                this.m_layersFolder = value;
            }
        }
        
        /// <remarks/>
        public bool GenerateMaps {
            get {
                return this.m_generateMaps;
            }
            set {
                this.m_generateMaps = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GenerateMapsSpecified {
            get {
                return this.m_generateMapsSpecified;
            }
            set {
                this.m_generateMapsSpecified = value;
            }
        }
        
        /// <remarks/>
        public string MapsPath {
            get {
                return this.m_mapsPath;
            }
            set {
                this.m_mapsPath = value;
            }
        }
        
        /// <remarks/>
        public string MapsFolder {
            get {
                return this.m_mapsFolder;
            }
            set {
                this.m_mapsFolder = value;
            }
        }
        
        /// <remarks/>
        public bool GenerateSymbolLibraries {
            get {
                return this.m_generateSymbolLibraries;
            }
            set {
                this.m_generateSymbolLibraries = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GenerateSymbolLibrariesSpecified {
            get {
                return this.m_generateSymbolLibrariesSpecified;
            }
            set {
                this.m_generateSymbolLibrariesSpecified = value;
            }
        }
        
        /// <remarks/>
        public string SymbolLibrariesPath {
            get {
                return this.m_symbolLibrariesPath;
            }
            set {
                this.m_symbolLibrariesPath = value;
            }
        }
        
        /// <remarks/>
        public string SymbolLibrariesFolder {
            get {
                return this.m_symbolLibrariesFolder;
            }
            set {
                this.m_symbolLibrariesFolder = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ResourceId")]
        public StringCollection ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
    }
    
    /// <remarks/>
    public class GeoReferenceOverrideType {
        
        private string m_sourceFile;
        
        private System.Double m_locationX;
        
        private System.Double m_locationY;
        
        private System.Double m_scaleX;
        
        private System.Double m_scaleY;
        
        /// <remarks/>
        public string SourceFile {
            get {
                return this.m_sourceFile;
            }
            set {
                this.m_sourceFile = value;
            }
        }
        
        /// <remarks/>
        public System.Double LocationX {
            get {
                return this.m_locationX;
            }
            set {
                this.m_locationX = value;
            }
        }
        
        /// <remarks/>
        public System.Double LocationY {
            get {
                return this.m_locationY;
            }
            set {
                this.m_locationY = value;
            }
        }
        
        /// <remarks/>
        public System.Double ScaleX {
            get {
                return this.m_scaleX;
            }
            set {
                this.m_scaleX = value;
            }
        }
        
        /// <remarks/>
        public System.Double ScaleY {
            get {
                return this.m_scaleY;
            }
            set {
                this.m_scaleY = value;
            }
        }
    }
    
    /// <remarks/>
    public class DwgDriveAliasType {
        
        private string m_name;
        
        private string m_path;
        
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
        public string Path {
            get {
                return this.m_path;
            }
            set {
                this.m_path = value;
            }
        }
    }
    
    /// <remarks/>
    public class DwgComponentType {
        
        private DwgComponentTypeType m_type;
        
        private string m_name;
        
        private string m_dwgHandle;
        
        private bool m_selected;
        
        private DwgComponentTypeCollection m_children;
        
        /// <remarks/>
        public DwgComponentTypeType Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
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
        public string DwgHandle {
            get {
                return this.m_dwgHandle;
            }
            set {
                this.m_dwgHandle = value;
            }
        }
        
        /// <remarks/>
        public bool Selected {
            get {
                return this.m_selected;
            }
            set {
                this.m_selected = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Children")]
        public DwgComponentTypeCollection Children {
            get {
                return this.m_children;
            }
            set {
                this.m_children = value;
            }
        }
    }
    
    /// <remarks/>
    public enum DwgComponentTypeType {
        
        /// <remarks/>
        Dwg,
        
        /// <remarks/>
        Map,
        
        /// <remarks/>
        Group,
        
        /// <remarks/>
        Element,
        
        /// <remarks/>
        Layer,
        
        /// <remarks/>
        QueryCategory,
        
        /// <remarks/>
        Query,
        
        /// <remarks/>
        ThematicMap,
        
        /// <remarks/>
        Theme,
    }
    
    /// <remarks/>
    public class SdfLoadProcedureType : LoadProcedureType {
        
        private System.Double m_generalization;
        
        private SdfKeyTreatmentType m_sdfKeyTreatment;
        
        /// <remarks/>
        public System.Double Generalization {
            get {
                return this.m_generalization;
            }
            set {
                this.m_generalization = value;
            }
        }
        
        /// <remarks/>
        public SdfKeyTreatmentType SdfKeyTreatment {
            get {
                return this.m_sdfKeyTreatment;
            }
            set {
                this.m_sdfKeyTreatment = value;
            }
        }
    }
    
    /// <remarks/>
    public enum SdfKeyTreatmentType {
        
        /// <remarks/>
        AutogenerateAll,
        
        /// <remarks/>
        DiscardDuplicates,
        
        /// <remarks/>
        MergeDuplicates,
    }
    
    /// <remarks/>
    public class DwfLoadProcedureType : LoadProcedureType {
    }
    
    /// <remarks/>
    public class RasterLoadProcedureType : LoadProcedureType {
        
        private string m_featureSourceName;
        
        private System.Double m_subsampleFactor;
        
        private OverlapTreatmentType m_overlapTreatmentType;
        
        private GeoReferenceOverrideTypeCollection m_geoReferenceOverride;
        
        /// <remarks/>
        public string FeatureSourceName {
            get {
                return this.m_featureSourceName;
            }
            set {
                this.m_featureSourceName = value;
            }
        }
        
        /// <remarks/>
        public System.Double SubsampleFactor {
            get {
                return this.m_subsampleFactor;
            }
            set {
                this.m_subsampleFactor = value;
            }
        }
        
        /// <remarks/>
        public OverlapTreatmentType OverlapTreatmentType {
            get {
                return this.m_overlapTreatmentType;
            }
            set {
                this.m_overlapTreatmentType = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GeoReferenceOverride")]
        public GeoReferenceOverrideTypeCollection GeoReferenceOverride {
            get {
                return this.m_geoReferenceOverride;
            }
            set {
                this.m_geoReferenceOverride = value;
            }
        }
    }
    
    /// <remarks/>
    public enum OverlapTreatmentType {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        ForegroundOn,
        
        /// <remarks/>
        ForegroundOff,
    }
    
    /// <remarks/>
    public class DwgLoadProcedureType : LoadProcedureType {
        
        private DwgComponentTypeCollection m_components;
        
        private DwgResourceTypeType m_resourceType;
        
        private bool m_combineComponents;
        
        private System.Double m_generalization;
        
        private bool m_closedPolylinesToPolygons;
        
        private DwgDriveAliasTypeCollection m_dwgDriveAlias;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Component", IsNullable=false)]
        public DwgComponentTypeCollection Components {
            get {
                return this.m_components;
            }
            set {
                this.m_components = value;
            }
        }
        
        /// <remarks/>
        public DwgResourceTypeType ResourceType {
            get {
                return this.m_resourceType;
            }
            set {
                this.m_resourceType = value;
            }
        }
        
        /// <remarks/>
        public bool CombineComponents {
            get {
                return this.m_combineComponents;
            }
            set {
                this.m_combineComponents = value;
            }
        }
        
        /// <remarks/>
        public System.Double Generalization {
            get {
                return this.m_generalization;
            }
            set {
                this.m_generalization = value;
            }
        }
        
        /// <remarks/>
        public bool ClosedPolylinesToPolygons {
            get {
                return this.m_closedPolylinesToPolygons;
            }
            set {
                this.m_closedPolylinesToPolygons = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DwgDriveAlias")]
        public DwgDriveAliasTypeCollection DwgDriveAlias {
            get {
                return this.m_dwgDriveAlias;
            }
            set {
                this.m_dwgDriveAlias = value;
            }
        }
    }
    
    /// <remarks/>
    public enum DwgResourceTypeType {
        
        /// <remarks/>
        Feature,
        
        /// <remarks/>
        Drawing,
    }
        
    
    public class DwgComponentTypeCollection : System.Collections.CollectionBase {
        
        public DwgComponentType this[int idx] {
            get {
                return ((DwgComponentType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(DwgComponentType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class GeoReferenceOverrideTypeCollection : System.Collections.CollectionBase {
        
        public GeoReferenceOverrideType this[int idx] {
            get {
                return ((GeoReferenceOverrideType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(GeoReferenceOverrideType value) {
            return base.InnerList.Add(value);
        }
    }
        
    public class DwgDriveAliasTypeCollection : System.Collections.CollectionBase {
        
        public DwgDriveAliasType this[int idx] {
            get {
                return ((DwgDriveAliasType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(DwgDriveAliasType value) {
            return base.InnerList.Add(value);
        }
    }
}
