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
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Text;

namespace SamplesWeb.Tasks
{
    public partial class LayerInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string agent = ConfigurationManager.AppSettings["MapAgentUrl"];
                MAPNAME.Value = Request.Params["MAPNAME"];
                SESSION.Value = Request.Params["SESSION"];

                IServerConnection conn = ConnectionProviderRegistry.CreateConnection(
                    "Maestro.Http",
                    "Url", agent,
                    "SessionId", SESSION.Value);

                IMappingService mpSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                string rtMapId = "Session:" + conn.SessionID + "//" + MAPNAME.Value + ".Map";

                RuntimeMap rtMap = mpSvc.OpenMap(rtMapId);
                foreach (RuntimeMapLayer rtLayer in rtMap.Layers)
                {
                    ddlLayers.Items.Add(new ListItem(rtLayer.Name, rtLayer.ObjectId));
                }
            }
        }

        protected void btnDescribe_Click(object sender, EventArgs e)
        {
            string agent = ConfigurationManager.AppSettings["MapAgentUrl"];
            MAPNAME.Value = Request.Params["MAPNAME"];
            SESSION.Value = Request.Params["SESSION"];

            IServerConnection conn = ConnectionProviderRegistry.CreateConnection(
                "Maestro.Http",
                "Url", agent,
                "SessionId", SESSION.Value);

            IMappingService mpSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string rtMapId = "Session:" + conn.SessionID + "//" + MAPNAME.Value + ".Map";

            RuntimeMap rtMap = mpSvc.OpenMap(rtMapId);

            //Get the selected layer
            RuntimeMapLayer rtLayer = rtMap.Layers.GetByObjectId(ddlLayers.SelectedValue);

            //Get the class definition
            ClassDefinition clsDef = conn.FeatureService.GetClassDefinition(rtLayer.FeatureSourceID, rtLayer.QualifiedClassName);

            StringBuilder sb = new StringBuilder();

            sb.Append("<h3>Layer Properties</h3><hr/>");
            sb.Append("<p>Name: " + rtLayer.Name + "</p>");
            sb.Append("<p>Legend Label: " + rtLayer.LegendLabel + "</p>");
            sb.Append("<p>Display Level: " + rtLayer.DisplayOrder + "</p>");
            sb.Append("<p>Expand In Legend: " + rtLayer.ExpandInLegend + "</p>");
            sb.Append("<p>Show In Legend: " + rtLayer.ShowInLegend + "</p>");
            sb.Append("<p>Visible: " + rtLayer.Visible + "</p>");
            sb.Append("<p>Layer Definition: " + rtLayer.LayerDefinitionID + "</p>");
            sb.Append("<p>Has Tooltips: " + rtLayer.HasTooltips + "</p>");
            sb.Append("<p>Filter: " + rtLayer.Filter + "</p>");

            sb.Append("<h3>Class Definition</h3><hr/>");

            sb.Append("<p>Schema: " + clsDef.QualifiedName.Split(':')[0] + "</p>");
            sb.Append("<p>Class Name: " + clsDef.Name + "</p>");
            sb.Append("<strong>Properties (* indicates identity):</strong>");
            sb.Append("<ul>");
            for (int i = 0; i < clsDef.Properties.Count; i++)
            {
                PropertyDefinition prop = clsDef.Properties[i];
                bool isIdentity = false;

                if (prop.Type == PropertyDefinitionType.Data)
                {
                    isIdentity = clsDef.IdentityProperties.Contains((DataPropertyDefinition)prop);
                }
                string name = (isIdentity ? "* " + prop.Name : prop.Name);
                sb.AppendFormat("<li><p>Name: {0}</p><p>Type: {1}</p></li>", name, prop.Type.ToString());
            }
            sb.Append("</ul>");

            classDef.InnerHtml = sb.ToString();
        }
    }
}
