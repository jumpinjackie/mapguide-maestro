#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License


namespace OSGeo.MapGuide.ObjectModels.Capabilities
{
    /// <summary>
    /// Describes command capabilities for an FDO provider
    /// </summary>
    public interface IFdoCommandCapabilities
    {
        /// <summary>
        /// The array of supported commands
        /// </summary>
        string[] SupportedCommands { get; }

        /// <summary>
        /// Indicates if parameters are supported
        /// </summary>
        bool SupportsParameters { get; }

        /// <summary>
        /// Indicates if timeout is supported
        /// </summary>
        bool SupportsTimeout { get; }
    }

    /// <summary>
    /// Describes connection capabilities for an FDO provider
    /// </summary>
    public interface IFdoConnectionCapabilities
    {
        /// <summary>
        /// The array of supported spatial context extent types
        /// </summary>
        string[] SupportedSpatialContextExtentTypes { get; }

        /// <summary>
        /// Indicates if XML configuration documents are supported
        /// </summary>
        bool SupportsConfiguration { get; }

        /// <summary>
        /// Indicates if locking is supported
        /// </summary>
        bool SupportsLocking { get; }

        /// <summary>
        /// Indicates if long transactions are supported
        /// </summary>
        bool SupportsLongTransactions { get; }

        /// <summary>
        /// Indicates if SQL commands are supported
        /// </summary>
        bool SupportsSQL { get; }

        /// <summary>
        /// Indicates if timeout is supported
        /// </summary>
        bool SupportsTimeout { get; }

        /// <summary>
        /// Indicates if transactions are supported
        /// </summary>
        bool SupportsTransactions { get; }

        /// <summary>
        /// Gets the thread capability of this connection
        /// </summary>
        string ThreadCapability { get; }
    }

    /// <summary>
    /// Describes the FDO expression capabilities of the FDO provider
    /// </summary>
    public interface IFdoExpressionCapabilities
    {
        /// <summary>
        /// Gets an array of supported FDO functions
        /// </summary>
        IFdoFunctionDefintion[] SupportedFunctions { get; }

        /// <summary>
        /// Gets an array of supported FDO expression types
        /// </summary>
        string[] ExpressionTypes { get; }
    }

    /// <summary>
    /// Describes an argument of an FDO function
    /// </summary>
    public interface IFdoFunctionArgumentDefinition
    {
        /// <summary>
        /// The name of the argument
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the argument
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Describes an FDO function signature
    /// </summary>
    public interface IFdoFunctionDefintionSignature
    {
        /// <summary>
        /// The array of arguments in this function signature
        /// </summary>
        IFdoFunctionArgumentDefinition[] Arguments { get; }

        /// <summary>
        /// Gets the return type of this signature
        /// </summary>
        string ReturnType { get; }
    }

    /// <summary>
    /// Describes an FDO function
    /// </summary>
    public interface IFdoFunctionDefintion
    {
        /// <summary>
        /// The name of the function
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the function
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The array of supported signatures
        /// </summary>
        IFdoFunctionDefintionSignature[] Signatures { get; }
    }

    /// <summary>
    /// Describes the filter capabilities of the FDO provider
    /// </summary>
    public interface IFdoFilterCapabilities
    {
        /// <summary>
        /// The array of supported condition types
        /// </summary>
        string[] ConditionTypes { get; }

        /// <summary>
        /// The array of supported distance operations
        /// </summary>
        string[] DistanceOperations { get; }

        /// <summary>
        /// The array of supported spatial operations
        /// </summary>
        string[] SpatialOperations { get; }

        /// <summary>
        /// Indicates if geodesic distances are supported
        /// </summary>
        bool SupportsGeodesicDistance { get; }

        /// <summary>
        /// Indicates if non-literal geometric operations are supported
        /// </summary>
        bool SupportsNonLiteralGeometricOperations { get; }
    }

    /// <summary>
    /// Describes the geometry capabilities of the FDO provider
    /// </summary>
    public interface IFdoGeometryCapabilities
    {
        /// <summary>
        /// The array of supported geometry component types
        /// </summary>
        string[] SupportedComponentTypes { get; }

        /// <summary>
        /// The supported dimensionality
        /// </summary>
        string Dimensionality { get; }

        /// <summary>
        /// The array of supported geometry types
        /// </summary>
        string[] SupportedGeometryTypes { get; }
    }

    /// <summary>
    /// Describes the raster capabilities of the FDO provider
    /// </summary>
    public interface IFdoRasterCapabilities
    {
        /// <summary>
        /// Indicates if rasters are supported
        /// </summary>
        bool SupportsRaster { get; }

        /// <summary>
        /// Indicates if stitching is supported
        /// </summary>
        bool SupportsStitching { get; }

        /// <summary>
        /// Indicates if sub-sampling is supported
        /// </summary>
        bool SupportsSubsampling { get; }
    }
    
    /// <summary>
    /// Describes the schema capabilities of the FDO provider
    /// </summary>
    public interface IFdoSchemaCapabilities
    {
        /// <summary>
        /// The array of supported class definition types
        /// </summary>
        string[] SupportedClassTypes { get; }

        /// <summary>
        /// The array of supported data types
        /// </summary>
        string[] SupportedDataTypes { get; }

        /// <summary>
        /// Indicates if association properties are supported
        /// </summary>
        bool SupportsAssociationProperties { get; }

        /// <summary>
        /// Indicates if inheritance is supported
        /// </summary>
        bool SupportsInheritance { get; }

        /// <summary>
        /// Indicates if multiple-schemas is supported
        /// </summary>
        bool SupportsMultipleSchemas { get; }

        /// <summary>
        /// Indicates if network model is supported
        /// </summary>
        bool SupportsNetworkModel { get; }

        /// <summary>
        /// Indicates if object properties is supported
        /// </summary>
        bool SupportsObjectProperties { get; }

        /// <summary>
        /// Indicates if schema overrides is supported
        /// </summary>
        bool SupportsSchemaOverrides { get; }
    }

    /// <summary>
    /// Describes capabilities of the FDO provider
    /// </summary>
    public interface IFdoProviderCapabilities
    {
        /// <summary>
        /// Command capabilities
        /// </summary>
        IFdoCommandCapabilities Command { get; }

        /// <summary>
        /// Connection capabilities
        /// </summary>
        IFdoConnectionCapabilities Connection { get; }

        /// <summary>
        /// Expression capabilities
        /// </summary>
        IFdoExpressionCapabilities Expression { get; }

        /// <summary>
        /// Filter capabilities
        /// </summary>
        IFdoFilterCapabilities Filter { get; }

        /// <summary>
        /// Geometry capabilities
        /// </summary>
        IFdoGeometryCapabilities Geometry { get; }

        /// <summary>
        /// Raster capabilities
        /// </summary>
        IFdoRasterCapabilities Raster { get; }

        /// <summary>
        /// Schema capabilities
        /// </summary>
        IFdoSchemaCapabilities Schema { get; }
    }
}