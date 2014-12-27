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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Provides the means for the resource validation system to avoid validating
    /// already validated resources
    /// </summary>
    public class ResourceValidationContext
    {
        private Dictionary<string, string> _validated;
        private Dictionary<string, IResource> _resources;
        private Dictionary<string, FeatureSourceDescription> _schemas;
        private Dictionary<string, FdoSpatialContextList> _spatialContexts;

        private IServerConnection _conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceValidationContext"/> class.
        /// </summary>
        /// <param name="conn">The server connection</param>
        public ResourceValidationContext(IServerConnection conn)
        {
            _conn = conn;
            _validated = new Dictionary<string, string>();
            _resources = new Dictionary<string, IResource>();
            _schemas = new Dictionary<string, FeatureSourceDescription>();
            _spatialContexts = new Dictionary<string, FdoSpatialContextList>();
        }

        internal IServerConnection Connection
        {
            get { return _conn; }
        }

        /// <summary>
        /// Clears all cached items and validated resources
        /// </summary>
        public void Reset()
        {
            _validated.Clear();
            _resources.Clear();
            _schemas.Clear();
            _spatialContexts.Clear();
        }

        /// <summary>
        /// Gets the spatial contexts.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <returns></returns>
        public FdoSpatialContextList GetSpatialContexts(string resourceId)
        {
            if (_spatialContexts.ContainsKey(resourceId))
            {
                Trace.TraceInformation("Fetching cached spatial contexts of: " + resourceId); //NOXLATE
                return _spatialContexts[resourceId];
            }

            var scList = _conn.FeatureService.GetSpatialContextInfo(resourceId, false);
            _spatialContexts[resourceId] = scList;

            return scList;
        }

        /// <summary>
        /// Describes the feature source
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <returns></returns>
        public FeatureSourceDescription DescribeFeatureSource(string resourceId)
        {
            if (_schemas.ContainsKey(resourceId))
            {
                Trace.TraceInformation("Fetching cached schema of: " + resourceId); //NOXLATE
                return _schemas[resourceId];
            }

            var desc = _conn.FeatureService.DescribeFeatureSource(resourceId);
            _schemas[resourceId] = desc;

            return desc;
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <returns></returns>
        public IResource GetResource(string resourceId)
        {
            if (_resources.ContainsKey(resourceId))
            {
                Trace.TraceInformation("Fetching cached copy of: " + resourceId); //NOXLATE
                return _resources[resourceId];
            }

            var res = _conn.ResourceService.GetResource(resourceId);
            _resources[resourceId] = res;

            return res;
        }

        /// <summary>
        /// Determines whether the specified resource has already been validated
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>
        ///   <c>true</c> if [the specified resource has already been validated]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAlreadyValidated(string resourceId)
        {
            var res = _validated.ContainsKey(resourceId);

            if (res)
                Trace.TraceInformation("Skipping validation: " + resourceId); //NOXLATE

            return res;
        }

        /// <summary>
        /// Marks the specified resource id as being validated.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        public void MarkValidated(string resourceId)
        {
            _validated[resourceId] = resourceId;

            Trace.TraceInformation("Validated: " + resourceId); //NOXLATE
        }

        /// <summary>
        /// Gets whether the specified resource exists
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <returns></returns>
        public bool ResourceExists(string resourceId)
        {
            return _conn.ResourceService.ResourceExists(resourceId);
        }
    }
}