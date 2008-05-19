#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for Program.
	/// </summary>
	public class Program
	{
		[STAThread()]
		public static void Main(string[] args)
		{
			//Test of the LocalNativeConnection... it needs some work :(
			/*OSGeo.MapGuide.MaestroAPI.LocalNativeConnection con = new OSGeo.MapGuide.MaestroAPI.LocalNativeConnection(@"C:\Programmer\MapGuideOpenSource2.0\WebServerExtensions\www\webconfig.ini", "Administrator", "admin", null);
			OSGeo.MapGuide.MaestroAPI.MapDefinition mdef = con.GetMapDefinition("Library://Allerod importeret/Map1.MapDefinition");
			string sid = "Session:" + con.SessionID + "//test.Map";
			con.CreateRuntimeMap(sid, mdef.ResourceId);
			OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap rtmap = con.GetRuntimeMap(sid);*/


			//Test of Mono serializers
			/*System.Xml.Serialization.XmlSerializer xs1 = new System.Xml.Serialization.XmlSerializer(typeof(OSGeo.MapGuide.MaestroAPI.LayerDefinition));
			System.Xml.Serialization.XmlSerializer xs2 = new System.Xml.Serialization.XmlSerializer(typeof(OSGeo.MapGuide.MaestroAPI.WebLayout));
			System.Xml.Serialization.XmlSerializer xs3 = new System.Xml.Serialization.XmlSerializer(typeof(OSGeo.MapGuide.MaestroAPI.MapDefinition));
			System.Xml.Serialization.XmlSerializer xs4 = new System.Xml.Serialization.XmlSerializer(typeof(OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType));*/

			try
			{
				Application.EnableVisualStyles();
				Application.DoEvents();
				Globalizator.Globalizator.InitializeResourceManager();
				Application.Run(new FormMain());
			}
			catch(Exception ex)
			{
				MessageBox.Show("A serious error occured: " + ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
