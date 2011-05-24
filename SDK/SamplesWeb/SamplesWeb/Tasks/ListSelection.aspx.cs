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
                    sb.Append("<p>Layer: " + layerSel.Layer.Name + "(" + layerSel.Count + ")");
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
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    lblMessage.Text = "Showing IDs of selected features";

                    result.InnerHtml = sb.ToString();
                }
            }
            else
            {
                lblMessage.Text = "Nothing selected";
            }
        }
    }
}
