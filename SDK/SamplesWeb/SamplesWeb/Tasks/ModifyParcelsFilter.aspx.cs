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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace SamplesWeb.Tasks
{
    public partial class ModifyParcelsFilter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string agent = ConfigurationManager.AppSettings["MapAgentUrl"];
                string mapName = Request.Params["MAPNAME"];
                string session = Request.Params["SESSION"];

                IServerConnection conn = ConnectionProviderRegistry.CreateConnection(
                    "Maestro.Http",
                    "Url", agent,
                    "SessionId", session);

                IMappingService mpSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                string rtMapId = "Session:" + conn.SessionID + "//" + mapName + ".Map";

                RuntimeMap rtMap = mpSvc.OpenMap(rtMapId);
                int layerIndex = rtMap.Layers.IndexOf("Parcels");
                RuntimeMapLayer layer = rtMap.Layers[layerIndex];

                //Here is now the layer replacement technique works:
                //
                //We take the Layer Definition content referenced by the old layer
                //Modify the filter in this content and save it to a new resource id
                //We then create a replacement layer that points to this new resource id
                //and set the public properties to be identical of the old layer.
                //
                //Finally we then remove the old layer and put the replacement layer in its
                //place, before saving the runtime map.

                ILayerDefinition ldf = (ILayerDefinition)conn.ResourceService.GetResource(layer.LayerDefinitionID);
                IVectorLayerDefinition vl = (IVectorLayerDefinition)ldf.SubLayer;

                //Sets the layer filter
                vl.Filter = "RNAME LIKE 'SCHMITT%'";
                if (Request.Params["RESET"] == "1")
                {
                    vl.Filter = "";
                }
                
                //Save this modified layer under a different resource id
                string ldfId = "Session:" + conn.SessionID + "//ParcelsFiltered.LayerDefinition";
                conn.ResourceService.SaveResourceAs(ldf, ldfId);
                //Note that SaveResourceAs does not modify the ResourceID of the resource we want to save
                //so we need to update it here
                ldf.ResourceID = ldfId;

                //Create our replacement layer and apply the same properties from the old one
                RuntimeMapLayer replace = mpSvc.CreateMapLayer(rtMap, ldf);
                replace.ExpandInLegend = layer.ExpandInLegend;
                replace.Group = layer.Group;
                replace.LegendLabel = layer.LegendLabel;
                replace.Name = layer.Name;
                replace.Selectable = layer.Selectable;
                replace.ShowInLegend = layer.ShowInLegend;
                replace.Visible = layer.Visible;

                //Remove the old layer and put the new layer at the same position (thus having the
                //same draw order)
                rtMap.Layers.RemoveAt(layerIndex);
                rtMap.Layers.Insert(layerIndex, replace);
                replace.ForceRefresh();

                rtMap.Save();

                if (Request.Params["RESET"] == "1")
                {
                    lblMessage.Text = "Layer filter has been reset";
                    resetLink.Visible = false;
                }
                else
                {
                    lblMessage.Text = "Layer filter has been set (to RNAME LIKE 'SCHMITT%')";
                    resetLink.Attributes["href"] = "ModifyParcelsFilter.aspx?MAPNAME=" + mapName + "&SESSION=" + session + "&RESET=1";
                }

                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "load",
                    "<script type=\"text/javascript\"> window.onload = function() { parent.parent.Refresh(); } </script>");
            }
        }
    }
}
