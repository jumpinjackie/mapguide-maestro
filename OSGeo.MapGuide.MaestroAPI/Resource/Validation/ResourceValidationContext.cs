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
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Schema;

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

        private IResourceService _resSvc;
        private IFeatureService _featSvc;

        public ResourceValidationContext(IResourceService resSvc, IFeatureService featSvc)
        {
            _resSvc = resSvc;
            _featSvc = featSvc;
            _validated = new Dictionary<string, string>();
            _resources = new Dictionary<string, IResource>();
            _schemas = new Dictionary<string, FeatureSourceDescription>();
            _spatialContexts = new Dictionary<string, FdoSpatialContextList>();
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

        public FdoSpatialContextList GetSpatialContexts(string resourceId)
        {
            if (_spatialContexts.ContainsKey(resourceId))
            {
                Trace.TraceInformation("Fetching cached spatial contexts of: " + resourceId);
                return _spatialContexts[resourceId];
            }

            var scList = _featSvc.GetSpatialContextInfo(resourceId, false);
            _spatialContexts[resourceId] = scList;

            return scList;
        }

        public FeatureSourceDescription DescribeFeatureSource(string resourceId)
        {
            if (_schemas.ContainsKey(resourceId))
            {
                Trace.TraceInformation("Fetching cached schema of: " + resourceId);
                return _schemas[resourceId];
            }

            var desc = _featSvc.DescribeFeatureSource(resourceId);
            _schemas[resourceId] = desc;

            return desc;
        }

        public IResource GetResource(string resourceId)
        {
            if (_resources.ContainsKey(resourceId))
            {
                Trace.TraceInformation("Fetching cached copy of: " + resourceId);
                return _resources[resourceId];
            }

            var res = _resSvc.GetResource(resourceId);
            _resources[resourceId] = res;

            return res;
        }

        public bool IsAlreadyValidated(string resourceId)
        {
            var res = _validated.ContainsKey(resourceId);

            if (res)
                Trace.TraceInformation("Skipping validation: " + resourceId);

            return res;
        }

        public void MarkValidated(string resourceId)
        {
            _validated[resourceId] = resourceId;

            Trace.TraceInformation("Validated: " + resourceId);
        }
    }
}
