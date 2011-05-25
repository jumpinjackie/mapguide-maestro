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
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.ObjectModels;

namespace SamplesWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string agent = ConfigurationManager.AppSettings["MapAgentUrl"];
            
            IServerConnection conn = ConnectionProviderRegistry.CreateConnection(
                "Maestro.Http",
                "Url", agent,
                "Username", "Anonymous",
                "Password", "");

            var mdfId = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";
            if (conn.ResourceService.ResourceExists(mdfId))
            {
                //Here's an example of pre-processing the WebLayout before loading it
                //in the AJAX viewer.

                //This technique can also be used to do things like:
                //
                // 1. Overriding the initial view 
                

                //Create a WebLayout. By default the version created will be 
                //the latest supported one on the mapguide server we've connected to. For example
                //connecting to MGOS 2.2 will create a version 1.1.0 WebLayout. All the known
                //resource versions have been registered on startup (see Global.asax.cs)
                IWebLayout wl = ObjectFactory.CreateWebLayout(conn, mdfId);

                //What is IWebLayout2? It is an extension of IWebLayout that supports the ping server property.
                //This is the interface equivalent of WebLayout 1.1.0 schema. Most schema revisions in MapGuide
                //are additive and incremental, and our Object Model interfaces follow the same pattern. All new
                //interfaces inherit from their ancestor interface
                //
                //Anyway, what we want to do is if we created a 1.1.0 WebLayout, is to switch on the ping server
                //property, thus preventing session expiry
                IWebLayout2 wl2 = wl as IWebLayout2;
                if (wl2 != null)
                    wl2.EnablePingServer = true;

                wl.Title = "Maestro API Web Samples";
                wl.TaskPane.InitialTask = "../SamplesWeb/Tasks/Home.aspx";

                string resId = "Session:" + conn.SessionID + "//Sheboygan.WebLayout";
                conn.ResourceService.SaveResourceAs(wl, resId);

                Response.Redirect("../mapviewernet/ajaxviewer.aspx?WEBLAYOUT=" + resId + "&SESSION=" + conn.SessionID);
            }
            else
            {
                Response.Write("Could not find map definition: " + mdfId + "<br/>");
                Response.Write("Please ensure the Sheboygan sample dataset has been loaded");
            }
        }
    }
}
