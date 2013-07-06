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
using System.Collections.Specialized;
using ObjCommon = OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Provides services for accessing, querying and inspecting feature sources
    /// </summary>
    /// <remarks>
    /// Note that <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConnection"/> provides
    /// built-in access to resource and feature services. Using the <see cref="M:OSGeo.MapGuide.MaestroAPI.IServerConnection.GetService"/>
    /// method is not necessary
    /// </remarks>
    public interface IFeatureService : IService
    {
        /// <summary>
        /// Gets the capabilities of the specified provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        OSGeo.MapGuide.ObjectModels.Capabilities.FdoProviderCapabilities GetProviderCapabilities(string provider);

        /// <summary>
        /// Gets an array of all registered providers
        /// </summary>
        ObjCommon.FeatureProviderRegistryFeatureProvider[] FeatureProviders { get; }

        /// <summary>
        /// Tests the specified connection settings
        /// </summary>
        /// <param name="providername"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string TestConnection(string providername, NameValueCollection parameters);

        /// <summary>
        /// Tests the connection settings of the specified feature source
        /// </summary>
        /// <param name="featureSourceId"></param>
        /// <returns></returns>
        string TestConnection(string featureSourceId);

        /// <summary>
        /// Removes the version numbers from a providername
        /// </summary>
        /// <param name="providername">The name of the provider, with or without version numbers</param>
        /// <returns>The provider name without version numbers</returns>
        string RemoveVersionFromProviderName(string providername);

        /// <summary>
        /// Gets the possible values for a given connection property
        /// </summary>
        /// <param name="providerName">The FDO provider name</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="partialConnectionString">A partial connection string if certain providers require such information</param>
        /// <returns>A list of possible values for the given property</returns>
        string[] GetConnectionPropertyValues(string providerName, string propertyName, string partialConnectionString);

        /// <summary>
        /// Returns an installed provider, given the name of the provider
        /// </summary>
        /// <param name="providername">The name of the provider</param>
        /// <returns>The first matching provider or null</returns>
        ObjCommon.FeatureProviderRegistryFeatureProvider GetFeatureProvider(string providername);

        /// <summary>
        /// Executes a SQL query
        /// </summary>
        /// <remarks>
        /// No validation is done on the SQL query string. The calling application should validate the SQL string to ensure
        /// that it does not contain any malicious operations.
        /// </remarks>
        /// <param name="featureSourceID">The Feature Source ID</param>
        /// <param name="sql">The SQL query string</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IReader"/> containing the results of the query</returns>
        IReader ExecuteSqlQuery(string featureSourceID, string sql);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="filter">The FDO filter string that determines what features will be returned</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing the results of the query</returns>
        IFeatureReader QueryFeatureSource(string resourceID, string className, string filter);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing the results of the query</returns>
        IFeatureReader QueryFeatureSource(string resourceID, string className);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="filter">The FDO filter string that determines what features will be returned</param>
        /// <param name="propertyNames">A list of properties that are to be returned in the query result</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing the results of the query</returns>
        IFeatureReader QueryFeatureSource(string resourceID, string className, string filter, string[] propertyNames);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="filter">The FDO filter string that determines what features will be returned</param>
        /// <param name="propertyNames">A list of properties that are to be returned in the query result</param>
        /// <param name="computedProperties">A list of name/value pairs that contain the alias (name) for an FDO expression (value)</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing the results of the query</returns>
        IFeatureReader QueryFeatureSource(string resourceID, string className, string filter, string[] propertyNames, NameValueCollection computedProperties);

        /// <summary>
        /// Executes an aggregate query on the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="filter">The FDO filter string that determines what features will be returned</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IReader"/> containing the results of the query</returns>
        IReader AggregateQueryFeatureSource(string resourceID, string className, string filter);

        /// <summary>
        /// Executes an aggregate query on the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="filter">The FDO filter string that determines what features will be returned</param>
        /// <param name="propertyNames">An array of property names to include in the result</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IReader"/> containing the results of the query</returns>
        IReader AggregateQueryFeatureSource(string resourceID, string className, string filter, string[] propertyNames);

        /// <summary>
        /// Executes an aggregate query on the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="filter">The FDO filter string that determines what features will be returned</param>
        /// <param name="aggregateFunctions">A list of name/value pairs that contain the alias (name) for an FDO aggregate expression (value)</param>
        /// <returns>A <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IReader"/> containing the results of the query</returns>
        IReader AggregateQueryFeatureSource(string resourceID, string className, string filter, NameValueCollection aggregateFunctions);

        /// <summary>
        /// Gets the geometric extent of the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="geometryPropertyName">The geometry property to get the geometric extent of</param>
        /// <returns></returns>
        ObjCommon.IEnvelope GetSpatialExtent(string resourceID, string className, string geometryPropertyName);

        /// <summary>
        /// Gets the geometric extent of the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="geometryPropertyName">The geometry property to get the geometric extent of</param>
        /// <param name="filter">The FDO filter string that determines what features will be returned</param>
        /// <returns></returns>
        ObjCommon.IEnvelope GetSpatialExtent(string resourceID, string className, string geometryPropertyName, string filter);

        /// <summary>
        /// Gets the geometric extent of the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name</param>
        /// <param name="geometryPropertyName">The geometry property to get the geometric extent of</param>
        /// <param name="allowFallbackToContextInformation">If true, the geometric extent of the feature class's spatial context will be used in the event that computing the extents fails</param>
        /// <exception cref="T:OSGeo.MapGuide.MaestroAPI.Exceptions.NullExtentException">Thrown if the geometric extent is null</exception>
        /// <returns></returns>
        ObjCommon.IEnvelope GetSpatialExtent(string resourceID, string className, string geometryPropertyName, bool allowFallbackToContextInformation);

        /// <summary>
        /// Describes the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <remarks>
        /// If you only need to list schemas and class names, use the respective <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetSchemas" /> and
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetClassNames" /> methods. Using this API will have a noticeable performance impact on 
        /// really large datastores (whose size is in the 100s of classes).
        /// </remarks>
        /// <returns></returns>
        FeatureSourceDescription DescribeFeatureSource(string resourceID);

        /// <summary>
        /// Describes the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="schema">The name of the schema to describe</param>
        /// <remarks>
        /// If you only need to list schemas and class names, use the respective <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetSchemas" /> and
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetClassNames" /> methods. Using this API will have a noticeable performance impact on 
        /// really large datastores (whose size is in the 100s of classes).
        /// </remarks>
        /// <returns></returns>
        FeatureSchema DescribeFeatureSource(string resourceID, string schema);

        /// <summary>
        /// Describes the specified feature source restricted to only the specified schema and the specified class names
        /// </summary>
        /// <param name="resourceID">The feature source id</param>
        /// <param name="schema">The schema name</param>
        /// <param name="classNames">The array of class names to include in the resulting schema</param>
        /// <remarks>
        /// If you only need to list schemas and class names, use the respective <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetSchemas" /> and
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetClassNames" /> methods. Using this API will have a noticeable performance impact on 
        /// really large datastores (whose size is in the 100s of classes).
        /// </remarks>
        /// <returns></returns>
        FeatureSchema DescribeFeatureSourcePartial(string resourceID, string schema, string [] classNames);

        /// <summary>
        /// Gets the specified class definition
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="className">The feature class name. You can pass a qualified class name to be explicit about which class definition you are after</param>
        /// <returns></returns>
        ClassDefinition GetClassDefinition(string resourceID, string className);

        /// <summary>
        /// Get the spatial context information for the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="activeOnly">If true, will only return the active spatial context</param>
        /// <returns></returns>
        ObjCommon.FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly);

        /// <summary>
        /// Gets the names of the identity properties from a feature
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="classname">The classname of the feature, including schema</param>
        /// <returns>A string array with the found identities</returns>
        string[] GetIdentityProperties(string resourceID, string classname);

        /// <summary>
        /// Enumerates all the data stores and if they are FDO enabled for the specified provider and partial connection string
        /// </summary>
        /// <param name="providerName">The FDO provider name</param>
        /// <param name="partialConnString">The partial connection string. Certain providers require a partial conection string in order to be able to enumerate data stores</param>
        /// <returns></returns>
        OSGeo.MapGuide.ObjectModels.Common.DataStoreList EnumerateDataStores(string providerName, string partialConnString);

        /// <summary>
        /// Gets an array of schema names from the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <returns></returns>
        string[] GetSchemas(string resourceID);

        /// <summary>
        /// Gets an array of qualified feature class names from the specified feature source
        /// </summary>
        /// <param name="resourceID">The feature source id</param>
        /// <param name="schemaName">
        /// The name of the schema whose class names are to be returned. If null, class names from all schemas in the feature source
        /// are returned
        /// </param>
        /// <returns></returns>
        string[] GetClassNames(string resourceID, string schemaName);

        /// <summary>
        /// Gets the long transactions for the specified feature source
        /// </summary>
        /// <param name="resourceID">The Feature Source ID</param>
        /// <param name="activeOnly">If true, will only return active long transactions</param>
        /// <returns></returns>
        ILongTransactionList GetLongTransactions(string resourceID, bool activeOnly);

        /// <summary>
        /// Gets the schema mappings for the given FDO provider. These mappings form the basis for a custom configuration document
        /// for a feature source that supports configuration
        /// </summary>
        /// <param name="provider">The FDO provider</param>
        /// <param name="partialConnString">The connection string</param>
        /// <returns></returns>
        ConfigurationDocument GetSchemaMapping(string provider, string partialConnString);
    }
}
