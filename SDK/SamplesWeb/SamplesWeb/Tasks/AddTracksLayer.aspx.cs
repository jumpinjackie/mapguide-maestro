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
using OSGeo.MapGuide.MaestroAPI.Mapping;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels;
using System.Drawing;

namespace SamplesWeb.Tasks
{
    public partial class AddTracksLayer : System.Web.UI.Page
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

            RuntimeMapLayer tracks = rtMap.Layers["Tracks"];
            if (tracks != null)
            {
                lblMessage.Text = "Tracks layer already added";
            }
            else
            {
                string groupName = "Transportation";
                RuntimeMapGroup group = rtMap.Groups[groupName];
                if (group == null)
                {
                    group = mpSvc.CreateMapGroup(rtMap, groupName);
                    rtMap.Groups.Add(group);
                }

                //For some reason, the Sheboygan sample data does not have a Rail
                //Layer Definition, so what better time to show how to create a
                //layer dynamically :)

                //Our Feature Source
                string fsId = "Library://Samples/Sheboygan/Data/Rail.FeatureSource";

                //The place we'll store the layer definition
                string layerId = "Session:" + conn.SessionID + "//Rail.LayerDefinition";

                CreateTracksLayer(conn, fsId, layerId);

                ILayerDefinition layerDef = (ILayerDefinition)conn.ResourceService.GetResource(layerId);
                RuntimeMapLayer layer = mpSvc.CreateMapLayer(rtMap, layerDef);

                layer.Group = groupName;
                layer.Name = "Tracks";
                layer.LegendLabel = "Tracks";
                layer.ShowInLegend = true;
                layer.ExpandInLegend = true;
                layer.Selectable = true;
                layer.Visible = true;

                //Set it to be drawn above districts.
                //In terms of draw order, it goes [0...n] -> [TopMost ... Bottom]
                //So for a layer to be drawn above something else, its draw order must be
                //less than that particular layer.

                int index = rtMap.Layers.IndexOf("Districts");
                rtMap.Layers.Insert(index, layer);

                rtMap.Save();

                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "load",
                    "<script type=\"text/javascript\"> window.onload = function() { parent.parent.Refresh(); } </script>");

                lblMessage.Text = "Tracks layer added again";
            }

            rtMap = mpSvc.OpenMap(rtMapId);
            DumpMap(rtMap);
        }

        private static void CreateTracksLayer(IServerConnection conn, string resId, string layerId)
        {
            //We use the ObjectFactory class to create our layer
            ILayerDefinition ldf = ObjectFactory.CreateDefaultLayer(conn, LayerType.Vector);
            IVectorLayerDefinition vldf = (IVectorLayerDefinition)ldf.SubLayer;

            //Set feature source
            vldf.ResourceId = resId;

            //Set the feature class
            vldf.FeatureName = "SHP_Schema:Rail";

            //Set the designated geometry
            vldf.Geometry = "SHPGEOM";

            //Get the first vector scale range. This will have been created for us and is 0 to infinity
            IVectorScaleRange vsr = vldf.GetScaleRangeAt(0);

            //Get the line style
            ILineVectorStyle lstyle = vsr.LineStyle;

            //Get the first rule (a created one will only have one)
            ILineRule rule = lstyle.GetRuleAt(0);

            //What are we doing here? We're checking if this vector scale range is a
            //IVectorScaleRange2 instance. If it is, it means this layer definition
            //has a composite style attached, which takes precedence over point/area/line
            //styles. We don't want this, so this removes the composite styles if they
            //exist.
            IVectorScaleRange2 vsr2 = vsr as IVectorScaleRange2;
            if (vsr2 != null)
                vsr2.CompositeStyle = null;

            //There's only one stroke here, but iteration is the only
            //way to go through
            foreach (var stroke in rule.Strokes)
            {
                //Set color to red
                stroke.Color = "ffff0000";
            }

            //Now save it
            conn.ResourceService.SaveResourceAs(ldf, layerId);
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
                sb.Append("Label: " + layer.LegendLabel + "<br/>");
                sb.Append("Group: " + layer.Group + "<br/>");
                sb.Append("Draw Order: " + layer.DisplayOrder + "</li>");
            }
            sb.Append("</ul>");

            debug.InnerHtml = sb.ToString();
        }
    }
}
