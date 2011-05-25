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

namespace SamplesWeb.Tasks
{
    public partial class ListSelection : System.Web.UI.Page
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

            string xml = Request.Params["SELECTION"];

            //The map selection contains one or more layer selections
            //each containing a one or more sets of identity property values
            //(because a feature may have multiple identity properties)

            MapSelection selection = new MapSelection(rtMap, HttpUtility.UrlDecode(xml));
            if (selection.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < selection.Count; i++)
                {
                    MapSelection.LayerSelection layerSel = selection[i];
                    sb.Append("<p>Layer: " + layerSel.Layer.Name + " (" + layerSel.Count + " selected item)");
                    sb.Append("<table>");
                    
                    for (int j = 0; j < layerSel.Count; j++)
                    {
                        sb.Append("<tr>");
                        object[] values = layerSel[j];
                        for (int k = 0; k < values.Length; k++)
                        {
                            sb.Append("<td>");
                            sb.Append(values[k].ToString());
                            sb.Append("</td>");
                        }
                        sb.AppendFormat("<td><a href='FeatureInfo.aspx?MAPNAME={0}&SESSION={1}&LAYERID={2}&ID={3}'>More Info</a></td>",
                            rtMap.Name,
                            conn.SessionID,
                            layerSel.Layer.ObjectId,
                            HttpUtility.UrlEncode(layerSel.EncodeIDString(values)));
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    lblMessage.Text = "Showing IDs of selected features";

                    result.InnerHtml = sb.ToString();
                }
            }
            else
            {
                lblMessage.Text = "Nothing selected. Select some features first then run this sample again.";
            }
        }
    }
}
