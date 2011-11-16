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
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace SamplesWeb.Tasks
{
    public partial class SetSelectedFeatures : System.Web.UI.Page
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

        protected void btnSelect_Click(object sender, EventArgs e)
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

            //Query using the user filter
            IFeatureReader reader = conn.FeatureService.QueryFeatureSource(
                                        rtLayer.FeatureSourceID,
                                        rtLayer.QualifiedClassName,
                                        txtFilter.Text);

            //Get the selection set
            MapSelection sel = new MapSelection(rtMap);
            MapSelection.LayerSelection layerSel = null;
            if (!sel.Contains(rtLayer))
            {
                sel.Add(rtLayer);   
            }
            layerSel = sel[rtLayer];

            //Clear any existing selections
            layerSel.Clear();

            //Populate selection set with query result
            int added = layerSel.AddFeatures(reader, -1);

            //Generate selection string
            string selXml = sel.ToXml();
            
            //Generate a client-side set selection and execute a "Zoom to Selection" afterwards
            Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "load",
                    "<script type=\"text/javascript\"> window.onload = function() { parent.parent.GetMapFrame().SetSelectionXML('" + selXml + "'); parent.parent.ExecuteMapAction(10); } </script>");


            lblMessage.Text = added + " features in " + rtLayer.Name + " selected";
        }
    }
}
