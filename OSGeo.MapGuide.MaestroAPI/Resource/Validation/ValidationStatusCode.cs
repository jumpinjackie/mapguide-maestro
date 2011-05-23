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
    public enum ValidationStatusCode : int
    {
        Dummy = 0,

        #region information
        Info_FeatureSource_NoPrimaryKey = 1001,

        Info_DrawingSource_NoCoordinateSpace = 1011,

        Info_MapDefinition_GroupMissingLabelInformation = 1101,
        Info_MapDefinition_GroupHasDefaultLabel,
        Info_MapDefinition_MultipleSpatialContexts,

        Info_LayerDefinition_ScaleRangeOverlap = 1201,
        #endregion

        #region warnings
        Warning_General_ValidationWarning = 3001,
        Warning_General_NoRegisteredValidatorForResource,

        Warning_FeatureSource_NoSpatialContext = 3101,
        Warning_FeatureSource_EmptySpatialContext,
        Warning_FeatureSource_DefaultSpatialContext,
        Warning_FeatureSource_NoSchemasFound,

        Warning_LoadProcedure_DwgNotSupported = 3201,
        Warning_LoadProcedure_RasterNotSupported,
        Warning_LoadProcedure_Sdf2OptionsNotSupported,
        Warning_LoadProcedure_GeneralizationNotSupported,
        Warning_LoadProcedure_ConvertToSdf3NotSupported,
        Warning_LoadProcedure_SourceFileNotFound,

        Warning_MapDefinition_DuplicateLayerName = 3301,
        Warning_MapDefinition_LayerMissingLegendLabel,
        Warning_MapDefinition_MissingSpatialContext,
        Warning_MapDefinition_LayerReprojection,
        Warning_MapDefinition_DataOutsideMapBounds,
        
        Warning_Fusion_InitialViewOutsideMapExtents = 3401,
        Warning_Fusion_MapCoordSysIncompatibleWithCommericalLayers,

        Warning_LayerDefinition_UnsupportedLayerType = 3501,
        Warning_LayerDefinition_MultipleGridScaleRanges,

        Warning_WebLayout_InitialViewOutsideMapExtents = 3601,
        #endregion

        #region errors
        Error_General_ValidationError = 5001,

        Error_FeatureSource_ConnectionTestFailed = 5101,
        Error_FeatureSource_SpatialContextReadError,
        Error_FeatureSource_SchemaReadError,

        Error_MapDefinition_NoFiniteDisplayScales = 5201,
        Error_MapDefinition_RasterReprojection,
        Error_MapDefinition_ResourceRead,
        Error_MapDefinition_FeatureSourceRead,
        Error_MapDefinition_LayerRead,
        Error_MapDefinition_LayerWithNonExistentGroup,
        Error_MapDefinition_GroupWithNonExistentGroup,

        Error_Fusion_MissingMap = 5301,
        Error_Fusion_InvalidMap,
        Error_Fusion_MapValidationError,

        Error_LayerDefinition_Generic = 5401,
        Error_LayerDefinition_GeometryNotFound,
        Error_LayerDefinition_ClassNotFound,
        Error_LayerDefinition_FeatureSourceLoadError,
        Error_LayerDefinition_DrawingSourceSheetLayerNotFound,
        Error_LayerDefinition_DrawingSourceSheetNotFound,
        Error_LayerDefinition_DrawingSourceError,
        Error_LayerDefinition_NoGridScaleRanges,
        Error_LayerDefinition_MinMaxScaleSwapped,
        Error_LayerDefinition_MissingScaleRanges,
        Error_LayerDefinition_MissingGeometry,
        Error_LayerDefinition_MissingFeatureSource,
        Error_LayerDefinition_LayerNull,

        Error_WebLayout_Generic = 5501,
        Error_WebLayout_NonExistentToolbarCommandReference,
        Error_WebLayout_NonExistentTaskPaneCommandReference,
        Error_WebLayout_NonExistentCommandReference,
        Error_WebLayout_DuplicateSearchCommandResultColumn,
        Error_WebLayout_DuplicateCommandName,
        Error_WebLayout_MissingMap,

        Error_DrawingSource_NoSourceDwf = 5601,
        #endregion
    }
}
