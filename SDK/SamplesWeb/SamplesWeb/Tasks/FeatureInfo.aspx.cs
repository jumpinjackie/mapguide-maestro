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
using System.Collections.Generic;
using System.Text;

namespace SamplesWeb.Tasks
{
    public partial class FeatureInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string agent = ConfigurationManager.AppSettings["MapAgentUrl"];
            string mapName = Request.Params["MAPNAME"];
            string layerId = Request.Params["LAYERID"];
            string id = HttpUtility.UrlDecode(Request.Params["ID"]);

            IServerConnection conn = ConnectionProviderRegistry.CreateConnection(
                "Maestro.Http",
                "Url", agent,
                "SessionId", Request.Params["SESSION"]);

            IMappingService mpSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string rtMapId = "Session:" + conn.SessionID + "//" + mapName + ".Map";

            RuntimeMap rtMap = mpSvc.OpenMap(rtMapId);
            RuntimeMapLayer layer = rtMap.Layers.GetByObjectId(layerId);

            //The values returned are in the same order as the array from the IdentityProperties
            object[] values = layer.ParseSelectionValues(id);
            PropertyInfo[] idProps = layer.IdentityProperties;

            //Having decoded the identity property values and knowing what names they are from the
            //RuntimeMapLayer, construct the selection filter based on these values.
            //
            //This sample assumes the Sheboygan dataset and so all identity property values are 
            //known to be only numeric or strings. If this is not the case for you, use the Type
            //property in PropertyInfo to determine how to construct the filter
            string[] conditions = new string[idProps.Length];
            for (int i = 0; i < idProps.Length; i++)
            {
                conditions[i] = idProps[i].Name + " = " + values[i].ToString();
            }
            //OR all the conditions together to form our final filter
            string selFilter = string.Join(" OR ", conditions);

            //Execute the query
            IFeatureReader reader = conn.FeatureService.QueryFeatureSource(
                                        layer.FeatureSourceID,
                                        layer.QualifiedClassName,
                                        selFilter);

            //Use a StringBuilder because we are doing a lot of concatentation here
            StringBuilder sb = new StringBuilder();

            //Collect the field names
            string[] fieldNames = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fieldNames[i] = reader.GetName(i);
            }

            int count = 0;

            //Write out the attribute table
            while (reader.ReadNext())
            {
                sb.Append("<table border='1'>");
                for (int i = 0; i < fieldNames.Length; i++)
                {
                    //Just like the MgFeatureReader, you must test for null before
                    //attempting extraction of values, but unlike MgFeatureReader this
                    //offers an indexer property that returns System.Object which allows
                    //a nice and easy way to string coerce all property values.
                    sb.Append("<tr>");
                    sb.Append("<td><strong>" + fieldNames[i] + "</strong></td>");
                    sb.Append("<td>" + (reader.IsNull(i) ? "(null)" : reader[i]) + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                count++;
            }
            content.InnerHtml = sb.ToString();
            lblMessage.Text = "Showing attributes of " + count + " features";
            reader.Close();
        }
    }
}
