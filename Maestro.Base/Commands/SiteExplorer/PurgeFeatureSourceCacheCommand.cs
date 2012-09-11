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
using ICSharpCode.Core;
using Maestro.Base.Services;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class PurgeFeatureSourceCacheCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            if (wb != null)
            {
                if (wb.ActiveSiteExplorer != null)
                {
                    var items = wb.ActiveSiteExplorer.SelectedItems;
                    if (items.Length == 1)
                    {
                        var it = items[0];
                        if (it.ResourceType == ResourceTypes.FeatureSource.ToString() || it.ResourceType == ResourceTypes.LayerDefinition.ToString())
                        {
                            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                            var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);
                            var resId = it.ResourceId;

                            if (it.ResourceType == ResourceTypes.LayerDefinition.ToString())
                            {
                                var res = (ILayerDefinition)conn.ResourceService.GetResource(resId);
                                resId = res.SubLayer.ResourceId;
                            }

                            //If selected item is a Layer, it must be pointing to a Feature Source and not a Drawing Source
                            if (resId.EndsWith(ResourceTypes.FeatureSource.ToString()))
                            {
                                using (var st = conn.ResourceService.GetResourceXmlData(resId))
                                {
                                    //"Touching" the feature source is sufficient to invalidate any cached information about it
                                    conn.ResourceService.SetResourceXmlData(resId, st);
                                    MessageService.ShowMessage(string.Format(Strings.SchemaInformationPurged, resId));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
