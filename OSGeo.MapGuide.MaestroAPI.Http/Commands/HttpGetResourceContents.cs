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
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.Net;
using System.Threading;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Commands;

namespace OSGeo.MapGuide.MaestroAPI.Http.Commands
{
    public class HttpGetResourceContents : IGetResourceContents
    {
        private readonly IResourceService _resSvc;

        public HttpGetResourceContents(IResourceService resSvc)
        {
            _resSvc = resSvc;
            _completed = new Dictionary<string, IResource>();
        }

        private Dictionary<string, IResource> _completed;

        private readonly object SyncRoot = new object();

        private void PutCompleted(IResource res)
        {
            lock (SyncRoot)
            {
                _completed.Add(res.ResourceID, res);
            }
        }

        public Dictionary<string, IResource> Execute(IEnumerable<string> resourceIds)
        {
            _completed.Clear();
            List<string> workItems = new List<string>(resourceIds);
            int completed = 0;

            foreach (var resId in workItems)
            {
                //Closures referencing iterator variables are bad mmkay?
                string rid = resId;

                //NOTE: Multi-threaded code is my weakness. So I wouldn't be surprised
                //if this has some subtle bug due to multi-threading. However, I have
                //stuck to basic rules of thumb in implementing this (ie. Do not let threads
                //manipulate shared state!). The whole code path of the 
                //IResourceService.GetResource() implementation does not touch any shared 
                //state. So I say with minor confidence that this should work without problems.
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    try
                    {
                        IResource res = _resSvc.GetResource(rid);
                        PutCompleted(res);
                    }
                    finally
                    {
                        Interlocked.Increment(ref completed);
                    }
                });
            }

            //Wait until all completed
            while (completed < workItems.Count)
                Thread.Sleep(20);

            return _completed;
        }

        public IServerConnection Parent
        {
            get;
            private set;
        }
    }
}
