#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// A validation status code that is attached to a specific validation issue, allowing for programmatic handling
    /// of certain validation issues should they occur
    /// </summary>
    public enum ValidationStatusCode : int
    {
        /// <summary>
        /// Placeholder
        /// </summary>
        Dummy = 0,

        #region information
        /// <summary>
        /// No primary key found in feature source. Does affect layer selection if a layer is based on this feature source
        /// </summary>
        Info_FeatureSource_NoPrimaryKey = 1001,

        /// <summary>
        /// No coordinate system found in drawing source. Affects re-projection if referencing layer is housed in a Map Definition
        /// </summary>
        Info_DrawingSource_NoCoordinateSpace = 1011,

        /// <summary>
        /// A layer group has no label. Results in no text when displayed in the viewer legend
        /// </summary>
        Info_MapDefinition_GroupMissingLabelInformation = 1101,
        
        /// <summary>
        /// A layer group has a default label assigned to it.
        /// </summary>
        Info_MapDefinition_GroupHasDefaultLabel,

        /// <summary>
        /// 
        /// </summary>
        Info_MapDefinition_MultipleSpatialContexts,

        /// <summary>
        /// One or more scale ranges overlap. Can affect presentation of data. 
        /// </summary>
        Info_LayerDefinition_ScaleRangeOverlap = 1201,

        /// <summary>
        /// A referenced symbol definition contains irrelevant usage contexts for this particular layer
        /// </summary>
        Info_LayerDefinition_IrrelevantUsageContext,
        #endregion

        #region warnings
        /// <summary>
        /// Placeholder validation warning
        /// </summary>
        Warning_General_ValidationWarning = 3001,

        /// <summary>
        /// No validation was found for the specified resource and/or version
        /// </summary>
        Warning_General_NoRegisteredValidatorForResource,

        /// <summary>
        /// Feature Source has no spatial context. Affects re-projection if referencing layer is housed in a Map Definition
        /// </summary>
        Warning_FeatureSource_NoSpatialContext = 3101,
        
        /// <summary>
        /// Feature Source has an empty spatial context
        /// </summary>
        Warning_FeatureSource_EmptySpatialContext,

        /// <summary>
        /// Feature Source has a spatial context with what appears to be system-generated bounds. This usually makes extent queries or
        /// operations that rely on a data store's extents (eg. Layer Previews) unreliable
        /// </summary>
        Warning_FeatureSource_DefaultSpatialContext,

        /// <summary>
        /// No feature schemas found in feature source. Layers referencing this feature source have nothing to show and style.
        /// </summary>
        Warning_FeatureSource_NoSchemasFound,

        /// <summary>
        /// A mapagent error occurred during Feature Source validation. Most likely because of timeout. As a result, the full validity of the
        /// Feature Source cannot be determined
        /// </summary>
        Warning_FeatureSource_Validation_Timeout,

        /// <summary>
        /// Due to a limitation in the public APIs provided by MapGuide. Feature Sources with encrypted security credentials (MG_USER_CREDENTIALS)
        /// cannot be packaged by Maestro's packager without discarding the MG_USER_CREDENTIALS element. The workaround is to either re-secure these
        /// feature sources after loading the Maestro-created package, or to use the official packaging method which will preserve MG_USER_CREDENTIALS
        /// in its encrypted state
        /// </summary>
        Warning_FeatureSource_Cannot_Package_Secured_Credentials,
        
        /// <summary>
        /// A Feature Source whose provider contains username/password connection parameters contains login credentials in plaintext. This is not secure
        /// as such Feature Sources can be accessed by the Anonymous MapGuide user account. It is strongly advised that such Feature Sources can be 
        /// re-secured with MapGuide/Infrastructure Studio or with Maestro 4.0.3 (or newer)
        /// </summary>
        Warning_FeatureSource_Plaintext_Credentials,

        /// <summary>
        /// An Extended Feature Class does not specify a join prefix. This will cause collisions if the primary and secondary classes have one or more identical
        /// property names
        /// </summary>
        Warning_FeatureSource_EmptyJoinPrefix,

        /// <summary>
        /// An Extended Feature Class does not specify 
        /// </summary>
        Warning_FeatureSource_Potential_Bad_Join_Performance,

        /// <summary>
        /// DWG Load Procedures not supported. This is a limitation of Maestro
        /// </summary>
        Warning_LoadProcedure_DwgNotSupported = 3201,

        /// <summary>
        /// Raster Load Procedures not supported. This is a limitation of Maestro
        /// </summary>
        Warning_LoadProcedure_RasterNotSupported,

        /// <summary>
        /// SDF2 options not supported for a SDF Load Procedure. This is a limitation of Maestro
        /// </summary>
        Warning_LoadProcedure_Sdf2OptionsNotSupported,

        /// <summary>
        /// Generalization options not supported for file-based Load Procedures. This is a limitation of Maestro
        /// </summary>
        Warning_LoadProcedure_GeneralizationNotSupported,

        /// <summary>
        /// Convert to SDF (3.0) option not supported for file-based Load Procedures. This is a limitation of Maestro
        /// </summary>
        Warning_LoadProcedure_ConvertToSdf3NotSupported,

        /// <summary>
        /// A source file referenced in a Load Procedure could not be found. This can happen if executing this Load Procedure on a different client machine.
        /// </summary>
        Warning_LoadProcedure_SourceFileNotFound,

        /// <summary>
        /// A layer in a Map Definition has no legend label. Results in not text displayed in the viewer legend.
        /// </summary>
        Warning_MapDefinition_LayerMissingLegendLabel = 3301,

        /// <summary>
        /// Could not find a spatial context for a referenced layer definition in the map definition
        /// </summary>
        Warning_MapDefinition_MissingSpatialContext,

        /// <summary>
        /// One or more layers in a Map Definition have different coordinate system than the one declared in the Map Definition. This will normally incur a
        /// minor performance penalty when rendering due to on-the-fly reprojection of data. In some cases, this is unavoidable.
        /// </summary>
        Warning_MapDefinition_LayerReprojection,

        /// <summary>
        /// One or more layers in a Map Definition has data that lies outside of the extents in the Map Definition. This normally means the user would have
        /// to manually pan outside the extents to see this data.
        /// </summary>
        Warning_MapDefinition_DataOutsideMapBounds,

        /// <summary>
        /// The Map Definition does not have a coordinate system
        /// </summary>
        Warning_MapDefinition_MissingCoordinateSystem,

        /// <summary>
        /// The Map Definition contains a referenced feature source that has a null extent (usually caused by having no data in the feature source)
        /// </summary>
        Warning_MapDefinition_FeatureSourceWithNullExtent,

        /// <summary>
        /// The specified initial view parameters lie outside the referenced Map Definition's extents. Usually means you will see nothing when the Fusion viewer loads.
        /// </summary>
        Warning_Fusion_InitialViewOutsideMapExtents = 3401,

        /// <summary>
        /// The specified map definition's coordinate system is not WGS84.PseudoMercator. This is a requirement for integrating with Google/Yahoo/Bing commerical layers
        /// </summary>
        Warning_Fusion_MapCoordSysIncompatibleWithCommericalLayers,

        /// <summary>
        /// The referenced widget has no label, which may cause display problems if there is no icon specified
        /// </summary>
        Warning_Fusion_NoLabelOnWidget,

        /// <summary>
        /// A toolbar or container contains a reference to a non-UI widget
        /// </summary>
        Warning_Fusion_NonStandardUiWidgetAttachedToContainer,

        /// <summary>
        /// Unrecognised layer type
        /// </summary>
        Warning_LayerDefinition_UnsupportedLayerType = 3501,

        /// <summary>
        /// Multiple raster scale ranges were found
        /// </summary>
        Warning_LayerDefinition_MultipleGridScaleRanges,

        /// <summary>
        /// A scale range was found in a Layer Definition which has a composite style defined along side a point, line or area style. In such cases, the 
        /// composite style will always take precedence and the point/line/area style will have no effect
        /// </summary>
        Warning_LayerDefinition_CompositeStyleDefinedAlongsideBasicStyle,

        /// <summary>
        /// A parameter override has been specified for a parameter that does not exist
        /// </summary>
        Warning_LayerDefinition_SymbolParameterOverrideToNonExistentParameter,

        /// <summary>
        /// The web layout's initial view lies outside the referenced map definition's extents. Usually means you will see nothing when the AJAX viewer loads.
        /// </summary>
        Warning_WebLayout_InitialViewOutsideMapExtents = 3601,

        /// <summary>
        /// The simple symbol definition contains a symbol parameter that is not referenced
        /// anywhere within the definition
        /// </summary>
        Warning_SymbolDefinition_SymbolParameterNotUsed = 3701,
        #endregion

        #region errors
        /// <summary>
        /// General validation error that couldn't be categorized
        /// </summary>
        Error_General_ValidationError = 5001,

        /// <summary>
        /// One or more connection parameters for the feature source are invalid.
        /// </summary>
        Error_FeatureSource_ConnectionTestFailed = 5101,

        /// <summary>
        /// Unclassified error when reading spatial contexts
        /// </summary>
        Error_FeatureSource_SpatialContextReadError,

        /// <summary>
        /// Unclassified error when describing a schema
        /// </summary>
        Error_FeatureSource_SchemaReadError,

        /// <summary>
        /// The validator found the %MG_USERNAME% and %MG_PASSWORD% placeholder tokens in the Feature Source content, but could not find the
        /// matching MG_USER_CREDENTIALS resource data item that contains the encrypted credentials
        /// </summary>
        Error_FeatureSource_SecuredCredentialTokensWithoutSecuredCredentialData,

        /// <summary>
        /// No finite display scales defined for a map definition that contains tiled layers.
        /// </summary>
        Error_MapDefinition_NoFiniteDisplayScales = 5201,
        
        /// <summary>
        /// A raster layer in a Map Definition has a different coordinate system from the one that is declared in the
        /// Map Definition and the MapGuide Server we're connecting to does not support the raster re-projection feature (MGOS 2.0 or earlier)
        /// </summary>
        Error_MapDefinition_RasterReprojection,

        /// <summary>
        /// Unclassified error when reading a resource
        /// </summary>
        Error_MapDefinition_ResourceRead,

        /// <summary>
        /// Unclassified error when reading a feature source
        /// </summary>
        Error_MapDefinition_FeatureSourceRead,

        /// <summary>
        /// Unclassified error when reading a layer definition
        /// </summary>
        Error_MapDefinition_LayerRead,

        /// <summary>
        /// A layer belongs to a layer group that doesn't exist
        /// </summary>
        Error_MapDefinition_LayerWithNonExistentGroup,
        
        /// <summary>
        /// A layer group belongs to a layer group that doesn't exist
        /// </summary>
        Error_MapDefinition_GroupWithNonExistentGroup,

        /// <summary>
        /// A layer with an already existing name was found. Layer names must be unique
        /// </summary>
        Error_MapDefinition_DuplicateLayerName = 3301,

        /// <summary>
        /// The Fusion Application Definition has no Maps or Map Groups
        /// </summary>
        Error_Fusion_MissingMap = 5301,

        /// <summary>
        /// The referenced Map definition doesn't exist
        /// </summary>
        Error_Fusion_InvalidMap,

        /// <summary>
        /// Unclassified error validating the referenced map definition
        /// </summary>
        Error_Fusion_MapValidationError,

        /// <summary>
        /// A toolbar or container contains a reference to a non-existent widget
        /// </summary>
        Error_Fusion_InvalidWidgetReference,

        /// <summary>
        /// Unclassifed validation error
        /// </summary>
        Error_LayerDefinition_Generic = 5401,

        /// <summary>
        /// The specified geometry property does not exist in the specified feature class
        /// </summary>
        Error_LayerDefinition_GeometryNotFound,

        /// <summary>
        /// The specified feature class does not exist in the specified feature source
        /// </summary>
        Error_LayerDefinition_ClassNotFound,

        /// <summary>
        /// Unclassified error loading the referenced feature source
        /// </summary>
        Error_LayerDefinition_FeatureSourceLoadError,

        /// <summary>
        /// A specified drawing source sheet layer does not exist in the specified sheet of the specified Drawing Source
        /// </summary>
        Error_LayerDefinition_DrawingSourceSheetLayerNotFound,

        /// <summary>
        /// The specified sheet does not exist in the specified Drawing Source.
        /// </summary>
        Error_LayerDefinition_DrawingSourceSheetNotFound,

        /// <summary>
        /// Unclassified error validating or loading the referenced Drawing Source
        /// </summary>
        Error_LayerDefinition_DrawingSourceError,

        /// <summary>
        /// No Grid scale ranges were found in this Raster Layer
        /// </summary>
        Error_LayerDefinition_NoGridScaleRanges,

        /// <summary>
        /// The min scale and max scale values are swapped.
        /// </summary>
        Error_LayerDefinition_MinMaxScaleSwapped,

        /// <summary>
        /// The vector layer has no scale ranges.
        /// </summary>
        Error_LayerDefinition_MissingScaleRanges,

        /// <summary>
        /// There is no specified geometry property (as opposed to a geometry property specified, but doesn't exist)
        /// </summary>
        Error_LayerDefinition_MissingGeometry,

        /// <summary>
        /// There is no specified feature source (as opposed to a feature source specified, but doesn't exist)
        /// </summary>
        Error_LayerDefinition_MissingFeatureSource,

        /// <summary>
        /// Cannot determine the layer sub-type
        /// </summary>
        Error_LayerDefinition_LayerNull,

        /// <summary>
        /// The Layer Definition contains a composite rule pointing to a non-existent symbol definition
        /// </summary>
        Error_LayerDefinition_SymbolDefintionReferenceNotFound,

        /// <summary>
        /// Unclassified validation error
        /// </summary>
        Error_WebLayout_Generic = 5501,

        /// <summary>
        /// A toolbar item references a command that doesn't exist
        /// </summary>
        Error_WebLayout_NonExistentToolbarCommandReference,

        /// <summary>
        /// A task pane item references a command that doesn't exist
        /// </summary>
        Error_WebLayout_NonExistentTaskPaneCommandReference,

        /// <summary>
        /// A context menu item references a command that doesn't exist
        /// </summary>
        Error_WebLayout_NonExistentContextMenuCommandReference,

        /// <summary>
        /// A search result column references the same feature property.
        /// </summary>
        Error_WebLayout_DuplicateSearchCommandResultColumn,

        /// <summary>
        /// One or more commands have the same name
        /// </summary>
        Error_WebLayout_DuplicateCommandName,

        /// <summary>
        /// No Map Definition specified.
        /// </summary>
        Error_WebLayout_MissingMap,

        /// <summary>
        /// Source DWF file not specified
        /// </summary>
        Error_DrawingSource_NoSourceDwf = 5601,

        /// <summary>
        /// The simple symbol definition has no geometry usage contexts
        /// </summary>
        Error_SymbolDefinition_NoGeometryUsageContexts = 5701,

        /// <summary>
        /// The simple symbol definition contains an image graphic that references
        /// a non-existent resource id
        /// </summary>
        Error_SymbolDefinition_ImageGraphicReferenceResourceIdNotFound,

        /// <summary>
        /// The simple symbol definition contains an image graphic that references
        /// a non-existent resource data item
        /// </summary>
        Error_SymbolDefinition_ImageGraphicReferenceResourceDataNotFound,
        #endregion
    }
}
