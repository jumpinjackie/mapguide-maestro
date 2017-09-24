#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Http.Commands
{
    internal class HttpGetResourceContents : IGetResourceContents
    {
        readonly IResourceService _resSvc;

        public HttpGetResourceContents(IResourceService resSvc)
        {
            _resSvc = resSvc;
        }

        public IDictionary<string, IResource> Execute(IEnumerable<string> resourceIds)
        {
            var completed = new ConcurrentDictionary<string, IResource>();

            List<string> workItems = new List<string>(resourceIds);

            var errors = new ConcurrentBag<Exception>();

            Parallel.ForEach(workItems, (resId) => 
            {
                try
                {
                    IResource res = _resSvc.GetResource(resId);
                    completed[resId] = res;
                }
                catch (Exception ex)
                {
                    //TODO: Should probably pair with original resource id
                    errors.Add(ex);
                }
            });

            if (errors.Any())
            {
                throw new AggregateException(errors);
            }

            return completed;
        }

        public IServerConnection Parent { get; }
    }
}