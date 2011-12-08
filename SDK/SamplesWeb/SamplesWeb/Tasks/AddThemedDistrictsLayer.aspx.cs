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
using OSGeo.MapGuide.ObjectModels;
using System.Drawing;
using System.Collections.Specialized;
using OSGeo.MapGuide.MaestroAPI.Feature;
using System.Text;

namespace SamplesWeb.Tasks
{
    public partial class AddThemedDistrictsLayer : System.Web.UI.Page
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

            RuntimeMapLayer tracks = rtMap.Layers["ThemedDistricts"];
            if (tracks != null)
            {
                lblMessage.Text = "Themed districts layer already added";
            }
            else
            {
                //Add our themed districts layer

                //Our Feature Source
                string fsId = "Library://Samples/Sheboygan/Data/VotingDistricts.FeatureSource";

                //The place we'll store the layer definition
                string layerId = "Session:" + conn.SessionID + "//ThemedVotingDistricts.LayerDefinition";

                CreateDistrictsLayer(conn, fsId, layerId);

                ILayerDefinition layerDef = (ILayerDefinition)conn.ResourceService.GetResource(layerId);
                RuntimeMapLayer layer = mpSvc.CreateMapLayer(rtMap, layerDef);

                layer.Name = "ThemedDistricts";
                layer.Group = "";
                layer.LegendLabel = "Themed Districts";
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

                lblMessage.Text = "Themed districts layer added";
            }

            rtMap = mpSvc.OpenMap(rtMapId);
            DumpMap(rtMap);
        }

        private void CreateDistrictsLayer(IServerConnection conn, string resId, string layerId)
        {
            //We use the ObjectFactory class to create our layer
            ILayerDefinition ldf = ObjectFactory.CreateDefaultLayer(conn, LayerType.Vector);
            IVectorLayerDefinition vldf = (IVectorLayerDefinition)ldf.SubLayer;

            //Set feature source
            vldf.ResourceId = resId;

            //Set the feature class
            vldf.FeatureName = "SDF_2_Schema:VotingDistricts";

            //Set the designated geometry
            vldf.Geometry = "Data";

            //Get the first vector scale range. This will have been created for us and is 0 to infinity
            IVectorScaleRange vsr = vldf.GetScaleRangeAt(0);

            //What are we doing here? We're checking if this vector scale range is a
            //IVectorScaleRange2 instance. If it is, it means this layer definition
            //has a composite style attached, which takes precedence over point/area/line
            //styles. We don't want this, so this removes the composite styles if they
            //exist.
            IVectorScaleRange2 vsr2 = vsr as IVectorScaleRange2;
            if (vsr2 != null)
                vsr2.CompositeStyle = null;

            //Get the area style
            IAreaVectorStyle astyle = vsr.AreaStyle;
            //Remove the default rule
            astyle.RemoveAllRules();

            IFeatureService featSvc = conn.FeatureService;
            //Generate a random color for each distinct feature id
            //Perform a distinct value query
            IReader valueReader = featSvc.AggregateQueryFeatureSource(resId, "SDF_2_Schema:VotingDistricts", null, new NameValueCollection()
            {
                { "Value", "UNIQUE(Autogenerated_SDF_ID)" } //UNIQUE() is the aggregate function that collects all distinct values of FeatId
            });

            while (valueReader.ReadNext())
            {
                //The parent Layer Definition provides all the methods needed to create the necessary child elements
                IAreaRule rule = ldf.CreateDefaultAreaRule();
                //Set the filter for this rule
                rule.Filter = "Autogenerated_SDF_ID = " + valueReader["Value"].ToString();
                //IReader allows object access by name in case you don't care to determine the data type
                rule.LegendLabel = valueReader["Value"].ToString();
                //Assign a random color fill
                rule.AreaSymbolization2D.Fill.ForegroundColor = Utility.SerializeHTMLColor(RandomColor(), true);
                //Add this rule
                astyle.AddRule(rule);
            }
            valueReader.Close();

            //Now save it
            conn.ResourceService.SaveResourceAs(ldf, layerId);
        }

        Random rand = new Random();

        Color RandomColor()
        {
            return Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
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
