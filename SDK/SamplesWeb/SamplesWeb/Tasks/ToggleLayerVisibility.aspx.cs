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

namespace SamplesWeb.Tasks
{
    public partial class ToggleLayerVisibility : System.Web.UI.Page
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

            RuntimeMapLayer layer = rtMap.GetLayerByName(Request.Params["LAYERNAME"]);
            if (layer != null)
            {
                layer.Visible = !layer.Visible;

                rtMap.Save(); //Always save changes after modifying

                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "load",
                    "<script type=\"text/javascript\"> window.onload = function() { parent.parent.Refresh(); } </script>");


                lblMessage.Text = "Layer (" + layer.Name + ") visible: " + layer.Visible;
            }
            else
            {
                lblMessage.Text = "Layer (" + layer.Name + ") not found!";
            }
        }
    }
}
