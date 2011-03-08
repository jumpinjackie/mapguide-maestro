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
using ICSharpCode.Core;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Base.UI;
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class RepointCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);
            if (exp.SelectedItems.Length == 1)
            {
                var selected = exp.SelectedItems[0];
                var resId = new ResourceIdentifier(selected.ResourceId);
                if (resId.ResourceType == OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefinition)
                {
                    DoRepointLayer(wb, conn, resId);
                }
                else if (resId.ResourceType == OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition)
                {
                    DoRepointMap(wb, conn, resId);
                }
                else
                {
                    MessageService.ShowMessage(Properties.Resources.ResourceNotRepointable);
                }
            }
        }

        private void DoRepointMap(Workbench wb, OSGeo.MapGuide.MaestroAPI.IServerConnection conn, ResourceIdentifier resId)
        {
            var diag = new RepointerDialog(resId, conn.ResourceService);
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string srcMap = diag.Source;
                string dstMap = diag.Target;

                var deps = diag.Dependents;

                ProgressDialog.DoBackgroundWork worker = (wk, e, args) =>
                {
                    int updated = 0;
                    int total = deps.Count;
                    wk.ReportProgress(0, Properties.Resources.ProgressUpdatingReferences);
                    foreach (var dep in deps)
                    {
                        //Only web and flexible layouts depend on maps
                        var rt = ResourceIdentifier.GetResourceType(dep);
                        Debug.Assert(rt == ResourceTypes.WebLayout || rt == ResourceTypes.ApplicationDefinition);

                        bool changed = false;

                        IResource res = null;
                        if (rt == ResourceTypes.WebLayout)
                        {
                            IWebLayout wl = (IWebLayout)conn.ResourceService.GetResource(dep);
                            if (wl.Map.ResourceId.Equals(srcMap))
                            {
                                wl.Map.ResourceId = dstMap;
                                changed = true;
                                res = wl;
                            }
                        }
                        else //Flexible layout
                        {
                            IApplicationDefinition appDef = (IApplicationDefinition)conn.ResourceService.GetResource(dep);
                            foreach (var mg in appDef.MapSet.MapGroups)
                            {
                                foreach (var map in mg.Map)
                                {
                                    if (map.Type.Equals("MapGuide"))
                                    {
                                        string mdfId = map.GetMapDefinition();
                                        if (mdfId.Equals(srcMap))
                                        {
                                            map.SetMapDefinition(dstMap);
                                            changed = true;
                                            res = appDef;
                                        }
                                    }
                                }
                            }
                        }

                        //If it wasn't changed why did the resource service
                        //list this map as a dependent resource?
                        if (changed)
                        {
                            Debug.Assert(res != null);
                            conn.ResourceService.SaveResource(res);
                            updated++;

                            wk.ReportProgress((updated / total) * 100);
                        }
                    }
                    return updated;
                };
                var prd = new ProgressDialog();
                int result = (int)prd.RunOperationAsync(wb, worker);
                MessageService.ShowMessage(string.Format(Properties.Resources.ResourcesRepointed, result, dstMap));
            }
        }

        private static void DoRepointLayer(Workbench wb, OSGeo.MapGuide.MaestroAPI.IServerConnection conn, ResourceIdentifier resId)
        {
            var diag = new RepointerDialog(resId, conn.ResourceService);
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string srcLayer = diag.Source;
                string targetLayer = diag.Target;

                var deps = diag.Dependents;

                ProgressDialog.DoBackgroundWork worker = (wk, e, args) =>
                {
                    int updated = 0;
                    int total = deps.Count;
                    wk.ReportProgress(0, Properties.Resources.ProgressUpdatingReferences);
                    foreach (var dep in deps)
                    {
                        //Only maps depend on layers
                        Debug.Assert(ResourceIdentifier.GetResourceType(dep) == OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition);

                        IMapDefinition mdf = (IMapDefinition)conn.ResourceService.GetResource(dep);
                        bool changed = false;
                        //Find all references to source and replace with target
                        foreach (var layer in mdf.MapLayer)
                        {
                            if (layer.ResourceId.Equals(srcLayer))
                            {
                                layer.ResourceId = targetLayer;
                                changed = true;
                            }
                        }
                        if (mdf.BaseMap != null)
                        {
                            foreach (var group in mdf.BaseMap.BaseMapLayerGroup)
                            {
                                foreach (var layer in group.BaseMapLayer)
                                {
                                    if (layer.ResourceId.Equals(srcLayer))
                                    {
                                        layer.ResourceId = targetLayer;
                                        changed = true;
                                    }
                                }
                            }
                        }

                        //If it wasn't changed why did the resource service
                        //list this map as a dependent resource?
                        if (changed)
                        {
                            conn.ResourceService.SaveResource(mdf);
                            updated++;

                            wk.ReportProgress((updated / total) * 100);
                        }
                    }
                    return updated;
                };
                var prd = new ProgressDialog();
                int result = (int)prd.RunOperationAsync(wb, worker);
                MessageService.ShowMessage(string.Format(Properties.Resources.ResourcesRepointed, result, targetLayer));
            }
        }
    }
}
