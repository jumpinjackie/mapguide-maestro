#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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

	public class UnitTest
	{

		public static void TestSerializer()
		{
			OSGeo.MapGuide.MaestroAPI.XmlSerializer.XmlSerializerEngine eng = new OSGeo.MapGuide.MaestroAPI.XmlSerializer.XmlSerializerEngine();
			OSGeo.MapGuide.MaestroAPI.XmlEntities.Envelope envp = new OSGeo.MapGuide.MaestroAPI.XmlEntities.Envelope();
			envp.LowerLeftCoordinate = new OSGeo.MapGuide.MaestroAPI.XmlEntities.Coordinate();
			envp.LowerLeftCoordinate.X = 1;
			envp.LowerLeftCoordinate.Y = 2;
			envp.UpperRightCoordinate = new OSGeo.MapGuide.MaestroAPI.XmlEntities.Coordinate();
			envp.UpperRightCoordinate.X = 3;
			envp.UpperRightCoordinate.Y = 4;

			System.Xml.XmlDocument doc = eng.Serialize(envp, new Version(1,0,0,0));
			string v = doc.OuterXml;

			object o = eng.Deserialize(doc, envp.GetType(), new Version(1,0,0,0));
			string v2 = eng.Serialize(o, new Version(1,0,0,0)).OuterXml;
			if (v != v2)
				throw new Exception("Bad serializer!");

			OSGeo.MapGuide.MaestroAPI.XmlEntities.DataStoreList lst = new OSGeo.MapGuide.MaestroAPI.XmlEntities.DataStoreList();
			lst.DataStore = new OSGeo.MapGuide.MaestroAPI.XmlEntities.DataStoreItemCollection();
			lst.DataStore.Add(new OSGeo.MapGuide.MaestroAPI.XmlEntities.DataStoreItem());
			lst.DataStore.Add(new OSGeo.MapGuide.MaestroAPI.XmlEntities.DataStoreItem());
			lst.DataStore[0].FdoEnabled = false;
			lst.DataStore[0].Name = "test";
			lst.DataStore[1].FdoEnabled = true;
			lst.DataStore[1].Name = "fest";

			doc = eng.Serialize(lst, new Version(1,0,0,0));
			v = doc.OuterXml;
			o = eng.Deserialize(doc, lst.GetType(), null, new Version(1,0,0,0));
			v2 = eng.Serialize(o, new Version(1,0,0,0)).OuterXml;
			if (v != v2)
				throw new Exception("Bad serializer!");


			System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(envp.GetType());
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			sr.Serialize(ms, envp);
			ms.Position = 0;
			doc.Load(ms);
			string v3 = doc.OuterXml;
			if (v != v3)
				throw new Exception("Non conformant serializer!");


		}

		public static void SaveXml(string lib, string clas, string filter)
		{
			OSGeo.MapGuide.MaestroAPI.ServerConnection con = new OSGeo.MapGuide.MaestroAPI.ServerConnection(new Uri("http://localhost/mapguide/mapagent/mapagent.fcgi"), "Administrator", "admin", "da");
			OSGeo.MapGuide.MaestroAPI.FeatureSetReader rd = con.QueryFeatureSource(lib, clas, filter, new string[] {"EJD_NR", "FID"});
			using (System.IO.FileStream f = System.IO.File.Open("dump.txt", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(f, System.Text.Encoding.Default))
				while(rd.Read())
					foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn col in rd.Columns)
						sw.WriteLine(col.Name + ": " + rd.Row[col.Name].ToString());
		}

		public static void TestRuntimeMap(string sessionID, string mapName)
		{

			OSGeo.MapGuide.MaestroAPI.ServerConnection con = new OSGeo.MapGuide.MaestroAPI.ServerConnection(new Uri("http://localhost/mapguide/mapagent/mapagent.fcgi"), sessionID, "da");
			string mapid = con.GetResourceIdentifier(mapName, OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition, true);
			mapid = mapid.Substring(0, mapid.Length - "Definition".Length);
			OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap map = con.GetRuntimeMap(mapid);
			con.SaveRuntimeMap(mapid, map);


			/*
			OSGeo.MapGuide.MaestroAPI.ServerConnection c = new OSGeo.MapGuide.MaestroAPI.ServerConnection(new Uri("http://localhost/mapguide/mapagent/mapagent.fcgi"), sessionID, "da");
			System.IO.MemoryStream ms = c.GetResourceData("Session:" + sessionID + "//" + mapName + ".Map", "RuntimeData");

			OSGeo.MapGuide.MaestroAPI.Map m = new OSGeo.MapGuide.MaestroAPI.Map();
			m.Deserialize(new OSGeo.MapGuide.MaestroAPI.BinarySerializer.MgBinaryDeserializer(ms, c.SiteVersion));

			System.IO.MemoryStream ms2 = new System.IO.MemoryStream();
			m.Serialize(new OSGeo.MapGuide.MaestroAPI.BinarySerializer.MgBinarySerializer(ms2, c.SiteVersion));

			if (ms.Length != ms2.Length)
				throw new Exception("Reserialization changed length");
			else
			{
				ms.Position = 0;
				ms2.Position = 0;
				for(int i =0 ;i<ms.Length;i++)
				{
					int a = ms.ReadByte();
					int b = ms2.ReadByte();
					if (a != b)
						Console.WriteLine("Reserialization differs at byte: " + i.ToString() + ", before: " + a.ToString() + ", after: " + b.ToString() );
				}
			}

			ms2.Position = 0;
			m.Deserialize(new OSGeo.MapGuide.MaestroAPI.BinarySerializer.MgBinaryDeserializer(ms2, c.SiteVersion));

			if (c.SiteVersion >= OSGeo.MapGuide.MaestroAPI.SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideOS1_2))
			{
				System.IO.MemoryStream ms4 = c.GetResourceData("Session:" + sessionID + "//" + mapName + ".Map", "LayerGroupData");
				m.DeserializeLayerData(new OSGeo.MapGuide.MaestroAPI.BinarySerializer.MgBinaryDeserializer(ms4, c.SiteVersion));

				System.IO.MemoryStream ms5 = new System.IO.MemoryStream();
				m.SerializeLayerData(new OSGeo.MapGuide.MaestroAPI.BinarySerializer.MgBinarySerializer(ms5, c.SiteVersion));

				ms5.Position = 0;
				m.DeserializeLayerData(new OSGeo.MapGuide.MaestroAPI.BinarySerializer.MgBinaryDeserializer(ms5, c.SiteVersion));

				if (ms4.Length != ms5.Length)
					throw new Exception("Reserialization (layer) changed length");
				else
				{
					ms4.Position = 0;
					ms5.Position = 0;
					for(int i =0 ;i<ms.Length;i++)
					{
						int a = ms.ReadByte();
						int b = ms2.ReadByte();
						if (a != b)
							Console.WriteLine("Reserialization (layer) differs at byte: " + i.ToString() + ", before: " + a.ToString() + ", after: " + b.ToString() );
					}
				}

				m.SerializeLayerData(new OSGeo.MapGuide.MaestroAPI.BinarySerializer.MgBinarySerializer(ms5, c.SiteVersion));
				c.SetResourceData("Session:" + sessionID + "//" + mapName + ".Map", "LayerGroupData", "Stream", ms5);
			}

			ms2.Position = 0;
			c.SetResourceData("Session:" + sessionID + "//" + mapName + ".Map", "RuntimeData", "Stream", ms2);
			*/
		}

		public static void ReadTestFile(System.Type type, string filename)
		{
			try
			{
				object o = null;
				System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(type);
				using (System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
				{
					OSGeo.MapGuide.MaestroAPI.XMLValidator m_validator = new OSGeo.MapGuide.MaestroAPI.XMLValidator();

					System.Reflection.FieldInfo fi = type.GetField("SchemaName", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public );
					if (fi == null)
						throw new Exception("Type " + type + ", does not contain Schema Info");

					string xsd = (string)fi.GetValue(null);

					using (System.IO.FileStream xsd_fs = System.IO.File.Open(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Schemas"), xsd), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
					{
						m_validator.Validate(fs, System.Xml.Schema.XmlSchema.Read(xsd_fs, null));

						fs.Position = 0;
						o = sr.Deserialize(fs);
					}
				}

				MessageBox.Show(o.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public static void Test()
		{
			//OSGeo.MapGuide.MaestroAPI.ServerConnection.testread();
			//OSGeo.MapGuide.MaestroAPI.ServerConnection c = new OSGeo.MapGuide.MaestroAPI.ServerConnection(new Uri("http://localhost/mapguide/mapagent/mapagent.fcgi"), "9bf09e30-ffff-ffff-8000-02004c4f4f50_en", null);
			//c.GetMap("Session:9bf09e30-ffff-ffff-8000-02004c4f4f50_en//Map1.Map");

			OSGeo.MapGuide.MaestroAPI.ServerConnection c = new OSGeo.MapGuide.MaestroAPI.ServerConnection(new Uri("http://localhost/mapguide/mapagent/mapagent.fcgi"), "Administrator", "admin", "da");
			OSGeo.MapGuide.MaestroAPI.ResourceList l = c.GetRepositoryResources();
			foreach(object ix in l.Items)
			{
				if (ix.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
				{
					OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument i = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ix;
					if (i.ResourceId.EndsWith("FeatureSource"))
					{
						OSGeo.MapGuide.MaestroAPI.FeatureSource fs = c.GetFeatureSource(i.ResourceId);

						if (fs.Extension.Count == 0)
						{
							Console.WriteLine("No extensions for " + i.ResourceId + "\nTODO: Extract this info from the config document");
							
							//fs.SelectFeatures("");
						}
						else
						{

							foreach(OSGeo.MapGuide.MaestroAPI.FeatureSourceTypeExtension x in fs.Extension)
							{
								Console.Write("Reading fs: " + fs.ResourceId + ", " + x.Name);

								try
								{
									OSGeo.MapGuide.MaestroAPI.FeatureSetReader rdx = x.SelectFeatures("");
									while(rdx.Read())
									{
										foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn col in rdx.Columns)
											if (col.Type == typeof(OSGeo.MapGuide.MaestroAPI.Geometry.Geometry))
											{
												OSGeo.MapGuide.MaestroAPI.Geometry.Geometry g = (OSGeo.MapGuide.MaestroAPI.Geometry.Geometry)rdx.Row[rdx.Columns.Length-1];
												string s = OSGeo.MapGuide.MaestroAPI.Geometry.WKTWriter.Serialize(g);
												OSGeo.MapGuide.MaestroAPI.Geometry.Geometry gx = OSGeo.MapGuide.MaestroAPI.Geometry.WKTReader.Deserialize(s);
											}
									}
									Console.WriteLine(" - Done");
								}
								catch(Exception ex)
								{
									Console.WriteLine(" - Failed: " + ex.Message);
								}
							}
						}
					}
				}
			}
		}
	}
}