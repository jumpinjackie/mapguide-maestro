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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace OSGeo.MapGuide.MaestroAPI.Capability
{
    /// <summary>
    /// Base connection capabilitiy class
    /// </summary>
    public abstract class ConnectionCapabilities : IConnectionCapabilities
    {
        /// <summary>
        /// The parent connection
        /// </summary>
        protected IServerConnection _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionCapabilities"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected ConnectionCapabilities(IServerConnection parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Gets the highest supported resource version.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public virtual Version GetMaxSupportedResourceVersion(ResourceTypes resourceType)
        {
            Version ver = new Version(1, 0, 0);
            switch (resourceType)
            {
                case ResourceTypes.ApplicationDefinition:
                    if (!SupportsFusion())
                        throw new UnsupportedResourceTypeException(ResourceTypes.ApplicationDefinition);
                    break;
                case ResourceTypes.WatermarkDefinition:
                    ver = GetMaxWatermarkDefinitionVersion();
                    break;
                case ResourceTypes.MapDefinition:
                    ver = GetMaxMapDefinitionVersion();
                    break;
                case ResourceTypes.LayerDefinition:
                    ver = GetMaxLayerDefinitionVersion();
                    break;
                case ResourceTypes.LoadProcedure:
                    ver = GetMaxLoadProcedureVersion();
                    break;
                case ResourceTypes.WebLayout:
                    ver = GetMaxWebLayoutVersion();
                    break;
                case ResourceTypes.SymbolDefinition:
                    if (!SupportsAdvancedSymbols())
                        throw new UnsupportedResourceTypeException(ResourceTypes.SymbolDefinition);
                    else
                        ver = GetMaxSymbolDefinitionVersion();
                    break;
            }
            return ver;
        }

        /// <summary>
        /// Supportses the advanced symbols.
        /// </summary>
        /// <returns></returns>
        protected virtual bool SupportsAdvancedSymbols()
        {
            return (_parent.SiteVersion >= new Version(1, 2));
        }

        /// <summary>
        /// Supportses the fusion.
        /// </summary>
        /// <returns></returns>
        protected virtual bool SupportsFusion()
        {
            return (_parent.SiteVersion >= new Version(2, 0));
        }

        /// <summary>
        /// Gets the max watermark definition version
        /// </summary>
        /// <returns></returns>
        protected virtual Version GetMaxWatermarkDefinitionVersion()
        {
            if (_parent.SiteVersion >= new Version(2, 4))
                return new Version(2, 4, 0);
            return new Version(2, 3, 0);
        }

        /// <summary>
        /// Gets the max load procedure version.
        /// </summary>
        /// <returns></returns>
        protected virtual Version GetMaxLoadProcedureVersion()
        {
            if (_parent.SiteVersion >= new Version(2, 2))
                return new Version(2, 2, 0);

            if (_parent.SiteVersion >= new Version(2, 0))
                return new Version(1, 1, 0);

            return new Version(1, 0, 0);
        }

        /// <summary>
        /// Gets the max symbol definition version.
        /// </summary>
        /// <returns></returns>
        protected virtual Version GetMaxSymbolDefinitionVersion()
        {
            if (_parent.SiteVersion >= new Version(2, 4))
                return new Version(2, 4, 0);
            if (_parent.SiteVersion >= new Version(2, 0))
                return new Version(1, 1, 0);

            return new Version(1, 0, 0);
        }

        /// <summary>
        /// Gets the max web layout version.
        /// </summary>
        /// <returns></returns>
        protected virtual Version GetMaxWebLayoutVersion()
        {
            if (_parent.SiteVersion >= new Version(2, 4))
                return new Version(2, 4, 0);
            if (_parent.SiteVersion >= new Version(2, 2))
                return new Version(1, 1, 0);
            return new Version(1, 0, 0);
        }

        /// <summary>
        /// Gets the max map definition version
        /// </summary>
        /// <returns></returns>
        protected virtual Version GetMaxMapDefinitionVersion()
        {
            if (_parent.SiteVersion >= new Version(2, 4))
                return new Version(2, 4, 0);
            if (_parent.SiteVersion >= new Version(2, 3))
                return new Version(2, 3, 0);

            return new Version(1, 0, 0);
        }

        /// <summary>
        /// Gets the max layer definition version.
        /// </summary>
        /// <returns></returns>
        protected virtual Version GetMaxLayerDefinitionVersion()
        {
            if (_parent.SiteVersion >= new Version(2, 4))
                return new Version(2, 4, 0);
            if (_parent.SiteVersion >= new Version(2, 3))
                return new Version(2, 3, 0);
            if (_parent.SiteVersion >= new Version(2, 1))
                return new Version(1, 3, 0);
            if (_parent.SiteVersion >= new Version(2, 0))
                return new Version(1, 2, 0);
            if (_parent.SiteVersion >= new Version(1, 2))
                return new Version(1, 1, 0);
            
            return new Version(1, 0, 0);
        }

        /// <summary>
        /// Gets an array of supported commands
        /// </summary>
        /// <value></value>
        public abstract int[] SupportedCommands
        {
            get;
        }

        /// <summary>
        /// Gets an array of supported services
        /// </summary>
        /// <value></value>
        public virtual int[] SupportedServices
        {
            get
            {
                if (_parent.SiteVersion >= new Version(2, 0))
                {
                    return new int[] {
                        (int)ServiceType.Resource,
                        (int)ServiceType.Feature,
                        (int)ServiceType.Fusion,
                        (int)ServiceType.Mapping,
                        (int)ServiceType.Tile,
                        (int)ServiceType.Drawing,
                        (int)ServiceType.Site
                    };
                }
                else //Fusion doesn't exist pre-2.0
                {
                    return new int[] {
                        (int)ServiceType.Resource,
                        (int)ServiceType.Feature,
                        (int)ServiceType.Mapping,
                        (int)ServiceType.Tile,
                        (int)ServiceType.Drawing,
                        (int)ServiceType.Site
                    };
                }
            }
        }

        /// <summary>
        /// Indicates whether web-based previewing capabilities are possible with this connection
        /// </summary>
        /// <value></value>
        public abstract bool SupportsResourcePreviews
        {
            get;
        }

        /// <summary>
        /// Indicates whether the current connection can be used between multiple threads
        /// </summary>
        /// <value></value>
        public abstract bool IsMultithreaded
        {
            get;
        }

        /// <summary>
        /// Indicates if this current connection supports the specified resource type
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public virtual bool IsSupportedResourceType(string resourceType)
        {
            Check.NotEmpty(resourceType, "resourceType"); //NOXLATE
            var ver = _parent.SiteVersion;
            var rt = (ResourceTypes)Enum.Parse(typeof(ResourceTypes), resourceType);
            switch (rt)
            {
                case ResourceTypes.ApplicationDefinition: //Introduced in 2.0.0
                    return (ver >= new Version(2, 0));
                case ResourceTypes.SymbolDefinition: //Introduced in 1.2.0
                    return (ver >= new Version(1, 2));
            }

            return true;
        }

        /// <summary>
        /// Indicates if this current connection supports the specified resource type
        /// </summary>
        /// <param name="resType"></param>
        /// <returns></returns>
        public bool IsSupportedResourceType(ResourceTypes resType)
        {
            return IsSupportedResourceType(resType.ToString());
        }

        /// <summary>
        /// Gets whether this connection supports publishing resources for WFS
        /// </summary>
        public virtual bool SupportsWfsPublishing
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether this connection supports publishing resources for WMS
        /// </summary>
        public virtual bool SupportsWmsPublishing
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether this connection supports resource reference tracking
        /// </summary>
        public virtual bool SupportsResourceReferences
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether this connection supports resource security
        /// </summary>
        public virtual bool SupportsResourceSecurity
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether this connection supports the concept of resource headers
        /// </summary>
        public virtual bool SupportsResourceHeaders
        {
            get { return true; }
        }
    }
}
