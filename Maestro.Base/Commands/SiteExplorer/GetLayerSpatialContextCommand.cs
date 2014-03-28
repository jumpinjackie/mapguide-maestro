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
#endregion
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Editors.FeatureSource;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class GetLayerSpatialContextCommand : AbstractMenuCommand
    {
        class SpatialContextNotFoundException : Exception
        {
            public SpatialContextNotFoundException() { }
            public SpatialContextNotFoundException(string message) : base(message) { }
            public SpatialContextNotFoundException(string message, Exception inner) : base(message, inner) { }
            protected SpatialContextNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

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
                        if (it.ResourceType == ResourceTypes.LayerDefinition.ToString())
                        {
                            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                            var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);
                            BusyWaitDialog.Run(Strings.RetrievingSpatialContextForLayer, 
                            () => {
                                var resId = it.ResourceId;
                                var ldf = (ILayerDefinition)conn.ResourceService.GetResource(resId);

                                //If selected item is a Layer, it must be pointing to a Feature Source and not a Drawing Source
                                if (ldf.SubLayer.ResourceId.EndsWith(ResourceTypes.FeatureSource.ToString()))
                                {
                                    var ltype = ldf.SubLayer.LayerType;
                                    if (ltype == LayerType.Vector ||
                                        ltype == LayerType.Raster)
                                    {
                                        var sc = ldf.GetSpatialContext();
                                        if (sc == null)
                                        {
                                            if (ltype == LayerType.Vector)
                                            {
                                                IVectorLayerDefinition vl = (IVectorLayerDefinition)ldf.SubLayer;
                                                throw new SpatialContextNotFoundException(string.Format(Strings.GeometryPropertyNotFound, vl.Geometry));
                                            }
                                            else //Raster
                                            {
                                                IRasterLayerDefinition rl = (IRasterLayerDefinition)ldf.SubLayer;
                                                throw new SpatialContextNotFoundException(string.Format(Strings.RasterPropertyNotFound, rl.Geometry));
                                            }
                                        }
                                        return sc;
                                    }
                                    else
                                    {
                                        throw new SpatialContextNotFoundException(string.Format(Strings.NonApplicableLayerType, ldf.SubLayer.LayerType));
                                    }
                                }
                                else
                                {
                                    throw new SpatialContextNotFoundException(string.Format(Strings.NonApplicableLayerType, ldf.SubLayer.LayerType));
                                }
                            }, (res, ex) => {
                                if (ex != null)
                                {
                                    var nf = ex as SpatialContextNotFoundException;
                                    if (nf != null)
                                        MessageService.ShowMessage(nf.Message);
                                    else
                                        ErrorDialog.Show(ex);
                                }
                                else
                                {
                                    var sc = res as IFdoSpatialContext;
                                    if (sc != null)
                                    {
                                        new SpatialContextInfoDialog(sc).ShowDialog();
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }

        static IFdoSpatialContext FindSpatialContext(FdoSpatialContextList spatialContexts, string scName)
        {
            foreach (IFdoSpatialContext sc in spatialContexts.SpatialContext)
            {
                if (sc.Name == scName)
                    return sc;
            }
            return null;
        }
    }
}
