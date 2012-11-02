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
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.Diagnostics;
using System.IO;

#if LOCAL_API
namespace OSGeo.MapGuide.MaestroAPI.Local.Commands
#else
namespace OSGeo.MapGuide.MaestroAPI.Native.Commands
#endif
{
    public class LocalGetResourceContents : IGetResourceContents
    {
        private MgResourceService _resSvc;

#if LOCAL_API
        public LocalGetResourceContents(LocalConnection conn)
        {
            this.Parent = conn;
            var fact = new MgdServiceFactory();
            _resSvc = (MgResourceService)fact.CreateService(MgServiceType.ResourceService);
        }
#else 
        public LocalGetResourceContents(LocalNativeConnection conn)
        {
            this.Parent = conn;
            _resSvc = (MgResourceService)conn.Connection.CreateService(MgServiceType.ResourceService);
        }
#endif

        Dictionary<string, IResource> IGetResourceContents.Execute(IEnumerable<string> resourceIds)
        {
            //There is an implicit assumption here that all resource ids check out and that
            //there is no duplicates
            
            var resources = new Dictionary<string, IResource>();
            if (this.Parent.SiteVersion >= new Version(2, 2))
            {
                Trace.TraceInformation("[GetResources]: Using optimal code path provided by 2.2 Resource Service APIs");

                MgStringCollection ids = new MgStringCollection();
                foreach (var rid in resourceIds)
                {
                    ids.Add(rid);
                }
                //Use the magic of reflection to call newer APIs even though we're referencing
                //and older assembly
                System.Reflection.MethodInfo mi = _resSvc.GetType().GetMethod("GetResourceContents");
                MgStringCollection result = (MgStringCollection)mi.Invoke(_resSvc, new object[] { ids, null });

                int rcount = ids.GetCount();
                for (int i = 0; i < rcount; i++)
                {
                    var resId = ids.GetItem(i);
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(result.GetItem(i))))
                    {
                        ResourceTypes resType = ResourceIdentifier.GetResourceType(resId);

                        IResource r = ResourceTypeRegistry.Deserialize(resType, ms);
                        r.CurrentConnection = this.Parent;
                        r.ResourceID = resId;
                        resources.Add(resId, r);
                    }
                }
            }
            else
            {
                //TODO: Maybe use a ThreadPool in conjunction with cloned connections?
                Trace.TraceInformation("[GetResources]: Using non-optimal code path provided by pre-2.2 Resource Service APIs");
                foreach (var rid in resourceIds)
                {
                    resources.Add(rid, this.Parent.ResourceService.GetResource(rid));
                }
            }

            return resources;
        }

        public IServerConnection Parent
        {
            get;
            private set;
        }
    }
}
