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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using System.Text;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace SamplesWeb.Tasks
{
    public partial class ToggleParcelsLayer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string agent = ConfigurationManager.AppSettings["MapAgentUrl"];

            IServerConnection conn = ConnectionProviderRegistry.CreateConnection(
                "Maestro.Http",
                "Url", agent,
                "SessionId", Request.Params["SESSION"]);

            IMappingService mpSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string rtMapId = "Session:" + conn.SessionID + "//" + Request.Params["MAPNAME"] + ".Map";

            RuntimeMap rtMap = mpSvc.OpenMap(rtMapId);

            RuntimeMapLayer parcels = rtMap.Layers["Parcels"];
            if (parcels != null)
            {
                rtMap.Layers.Remove(parcels);

                rtMap.Save();

                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "load",
                    "<script type=\"text/javascript\"> window.onload = function() { parent.parent.Refresh(); } </script>");

                lblMessage.Text = "Parcels layer removed";
            }
            else
            {
                string groupName = "Municipal";
                RuntimeMapGroup group = rtMap.Groups[groupName];
                if (group == null)
                {
                    group = mpSvc.CreateMapGroup(rtMap, groupName);
                    rtMap.Groups.Add(group);
                    throw new Exception("Layer group not found");
                }

                ILayerDefinition layerDef = (ILayerDefinition)conn.ResourceService.GetResource("Library://Samples/Sheboygan/Layers/Parcels.LayerDefinition");
                RuntimeMapLayer layer = mpSvc.CreateMapLayer(rtMap, layerDef);

                layer.Group = group.Name;
                layer.LegendLabel = "Parcels";
                layer.ShowInLegend = true;
                layer.ExpandInLegend = true;
                layer.Selectable = true;
                layer.Visible = true;

                //Set it to be drawn above islands.
                //In terms of draw order, it goes [0...n] -> [TopMost ... Bottom]
                //So for a layer to be drawn above something else, its draw order must be
                //less than that particular layer.

                int index = rtMap.Layers.IndexOf("Islands");
                rtMap.Layers.Insert(index, layer);

                rtMap.Save();

                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "load",
                    "<script type=\"text/javascript\"> window.onload = function() { parent.parent.Refresh(); } </script>");

                lblMessage.Text = "Parcels layer added again";
            }

            rtMap = mpSvc.OpenMap(rtMapId);
            DumpMap(rtMap);
        }

        //This method dumps the runtime state of the map. I personally
        //used this method to debug this sample as I was developing it.
        //
        //It's been kept here for reference.
        private void DumpMap(RuntimeMap rtMap)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<p>Debugging</p>");
            sb.Append("Name: " + rtMap.Name + "<br/>");
            sb.Append("Layers: <br/>");
            sb.Append("<ul>");
            foreach (var layer in rtMap.Layers)
            {
                sb.Append("<li>Name: " + layer.Name + " (Selectable: " + layer.Selectable + ", Visible: " + layer.Visible + ")<br/>");
                sb.Append("Group: " + layer.Group + "<br/>");
                sb.Append("Draw Order: " + layer.DisplayOrder + "</li>");
            }
            sb.Append("</ul>");

            debug.InnerHtml = sb.ToString();
        }
    }
}
