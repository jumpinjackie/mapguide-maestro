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
using System.Xml;
using System.Collections;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Gdal
{
	/// <summary>
	/// This class contains all code that manipulates the configuration document.
	/// It will run in a seperate thread.
	/// This is a .Net adaption of the PHP script:
	/// http://www.jasonbirch.com/fdogdal/rasterconfig.phps
	/// </summary>
	public class ConfigUpdater
	{

		//These 4 strings are from a script by Jason Birch:
		//http://www.jasonbirch.com/fdogdal/rasterconfig.phps

		// Config file Feature template:  filename, filename, MinX, MinY, MaxX, MaxY
		private const string TEMPLATE_FEAT = "<Feature name=\"{0}\"><Band name=\"RGB\" number=\"1\"><Image frame=\"1\" name=\"{1}\"><Bounds><MinX>{2}</MinX><MinY>{3}</MinY><MaxX>{4}</MaxX><MaxY>{5}</MaxY></Bounds></Image></Band></Feature>";

		//Config file location entry: imagepath, features
		private const string TEMPLATE_LOC = "<Location name=\"{0}\">{1}</Location>";

		// Config file SchemaMapping template: feature list
		private const string TEMPLATE_SMAP = "<SchemaMapping provider=\"OSGeo.Gdal.3.2\" name=\"default\" xmlns=\"http://fdogrfp.osgeo.org/schemas\"><complexType name=\"defaultType\"><complexType name=\"RasterTypeType\"><RasterDefinition name=\"images\">{0}</RasterDefinition></complexType></complexType></SchemaMapping>";

		// This should really come from GetSchemaMapping, but it's broken:  minX, minY, maxX, maxY
		private const string TEMPLATE_CFG = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><fdo:DataStore xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:gml=\"http://www.opengis.net/gml\" xmlns:fdo=\"http://fdo.osgeo.org/schemas\" xmlns:fds=\"http://fdo.osgeo.org/schemas/fds\"><gml:DerivedCRS gml:id=\"Default\"><gml:metaDataProperty><gml:GenericMetaData><fdo:SCExtentType>dynamic</fdo:SCExtentType><fdo:XYTolerance>0.001000</fdo:XYTolerance><fdo:ZTolerance>0.001000</fdo:ZTolerance></gml:GenericMetaData></gml:metaDataProperty><gml:remarks>System generated default FDO Spatial Context</gml:remarks><gml:srsName>Default</gml:srsName><gml:validArea><gml:boundingBox><gml:pos>{0} {1}</gml:pos><gml:pos>{2} {3}</gml:pos></gml:boundingBox></gml:validArea><gml:baseCRS>" + 
			"<fdo:WKTCRS gml:id=\"Default\"><gml:srsName>Default</gml:srsName><fdo:WKT>LOCAL_CS[\"*XY-MT*\",LOCAL_DATUM[\"*X-Y*\",10000],UNIT[\"Meter\", 1],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]</fdo:WKT></fdo:WKTCRS></gml:baseCRS><gml:definedByConversion xlink:href=\"http://fdo.osgeo.org/coord_conversions#identity\"/><gml:derivedCRSType codeSpace=\"http://fdo.osgeo.org/crs_types\">geographic</gml:derivedCRSType><gml:usesCS xlink:href=\"http://fdo.osgeo.org/cs#default_cartesian\"/></gml:DerivedCRS><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:fdo=\"http://fdo.osgeo.org/schemas\" xmlns:gml=\"http://www.opengis.net/gml\" xmlns:default=\"http://fdo.osgeo.org/schemas/feature/default\" targetNamespace=\"http://fdo.osgeo.org/schemas/feature/default\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:element name=\"default\" type=\"default:defaultType\" abstract=\"false\" substitutionGroup=\"gml:_Feature\"><xs:key name=\"defaultKey\"><xs:selector xpath=\".//default\"/>" + 
			"<xs:field xpath=\"FeatId\"/></xs:key></xs:element><xs:complexType name=\"defaultType\" abstract=\"false\" fdo:hasGeometry=\"false\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:complexContent><xs:extension base=\"gml:AbstractFeatureType\"><xs:sequence><xs:element name=\"FeatId\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:simpleType><xs:restriction base=\"xs:string\"><xs:maxLength value=\"256\"/></xs:restriction></xs:simpleType></xs:element><xs:element name=\"Image\" type=\"fdo:RasterPropertyType\" fdo:defaultImageXSize=\"1024\" fdo:defaultImageYSize=\"1024\" fdo:srsName=\"Default\"><xs:annotation>" + 
			"<xs:appinfo source=\"http://fdo.osgeo.org/schemas\"><fdo:DefaultDataModel dataModelType=\"Bitonal\" dataType=\"Unknown\" organization=\"Pixel\" bitsPerPixel=\"1\" tileSizeX=\"256\" tileSizeY=\"256\"/></xs:appinfo></xs:annotation></xs:element></xs:sequence></xs:extension></xs:complexContent></xs:complexType></xs:schema><SchemaMapping xmlns=\"http://fdogrfp.osgeo.org/schemas\" provider=\"OSGeo.Gdal.3.2\" name=\"default\"></SchemaMapping></fdo:DataStore>";


		private string[] m_files;
		private string[] m_remove;
		private string[] m_add;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;

		private EditorInterface m_editor;

		public ConfigUpdater(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
		{
			m_editor = editor;
			m_feature = feature;
		}

		public DialogResult UpdateItems(string[] newfiles, string[] toremove)
		{
			m_remove = toremove;
			m_add = newfiles;

			return m_editor.LengthyOperation(this, this.GetType().GetMethod("Runner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
		}

		private string[] SplitPath(string input)
		{
			int ix = input.LastIndexOf("\\");
			if (ix < 0)
				ix = input.LastIndexOf("/");

			string relname = input.Substring(ix + 1);
			string folder = input.Substring(0, ix + 1);

			return new string[] {folder, relname};
		}

		private bool Runner(string oldpath, string newpath, OSGeo.MapGuide.MaestroAPI.LengthyOperationCallBack callback, OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack progress)
		{

			OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs args = new OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs(null);
			Hashtable toplevels = new Hashtable();

			ArrayList validitems = new ArrayList();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			double minx, maxx, miny, maxy;

			//Keep previous
			Hashtable files = GetFilelist();
			
			//Remove deleted
			foreach(string s in m_remove)
			{
				if (files.ContainsKey(s))
					files.Remove(s);
			}

			foreach(string s in m_add)
			{
				if (files.ContainsKey(s))
					files.Remove(s);
			}

			foreach(string s in files.Keys)
			{
				string[] items = SplitPath(s);
				if (!toplevels.ContainsKey(items[0]))
					toplevels[items[0]] = new System.Text.StringBuilder();
							
				((System.Text.StringBuilder)toplevels[items[0]]).Append(files[s]);
			}

			GetBounds(toplevels, out minx, out maxx, out miny, out maxy);


			//Add new items
			if (m_add.Length > 0)
			{
				OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressArgs la = new OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressArgs("Processing files...", -1);

				if (progress != null)
					progress(this, la);
				if (la.Cancel)
					return false;

				la.Progress = 100;
				if (progress != null)
					progress(this, la);
				if (la.Cancel)
					return false;

				OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem[] vi = new OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem[m_add.Length];

				for(int i = 0; i < vi.Length; i++)
					vi[i] = new OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem(m_add[i]);

				args = new OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs(vi);

				if (callback != null)
					callback(this, args);

				if (args.Cancel)
					return false;

				if (args.Index > args.Items.Length)
					return true;

				if (args.Items.Length == 0)
					return true;

				do
				{
					OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem item = args.Items[args.Index];
					item.Status = OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.Pending;

					if (callback != null)
					{
						callback(this, args);
						if (args.Cancel) 
							return false;
					}

					try
					{
                        string tempname = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.FeatureSource, m_editor.CurrentConnection.SessionID);
						OSGeo.MapGuide.MaestroAPI.FeatureSource fs = new OSGeo.MapGuide.MaestroAPI.FeatureSource();
						fs.CurrentConnection = m_editor.CurrentConnection;
						fs.Provider = m_feature.Provider;
						fs.ConfigurationDocument = null;
						fs.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();
						fs.Parameter["DefaultRasterFileLocation"] = item.Itempath;

						try
						{
							m_editor.CurrentConnection.SaveResourceAs(fs, tempname);

							OSGeo.MapGuide.MaestroAPI.FdoSpatialContextList lst = m_editor.CurrentConnection.GetSpatialContextInfo(tempname, false);
							if (lst.SpatialContext != null && lst.SpatialContext.Count != 0 && lst.SpatialContext[0].Extent != null)
							{
                                double local_minx = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.X, System.Globalization.CultureInfo.InvariantCulture);
                                double local_miny = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.Y, System.Globalization.CultureInfo.InvariantCulture);

                                double local_maxx = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.X, System.Globalization.CultureInfo.InvariantCulture);
                                double local_maxy = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.Y, System.Globalization.CultureInfo.InvariantCulture);

								minx = Math.Min(minx, local_minx); 
								miny = Math.Min(miny, local_miny); 
								maxx = Math.Max(maxx, local_maxx); 
								maxy = Math.Max(maxy, local_maxy); 

								string[] items = SplitPath(item.Itempath);
							
								if (!toplevels.ContainsKey(items[0]))
									toplevels[items[0]] = new System.Text.StringBuilder();
							
								((System.Text.StringBuilder)toplevels[items[0]]).Append(string.Format(System.Globalization.CultureInfo.InvariantCulture, TEMPLATE_FEAT, System.IO.Path.GetFileNameWithoutExtension(items[1]), items[1], local_minx, local_miny, local_maxx, local_maxy)); 
							}
						}
						finally
						{
							try {m_editor.CurrentConnection.DeleteResource(tempname); }
							catch {}
						}
				
						item.Status = OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.Success;
						validitems.Add(item.Itempath);
					}
					catch (Exception ex)
					{
						string s = ex.Message;
						item.Status = OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.Failure;
					}

					if (callback != null)
					{
						callback(this, args);
						if (args.Cancel) 
							return false;
					}
				
					args.Index++;
				} while(!args.Cancel && args.Index < args.Items.Length);
			}

			//All done, if we have any working data, build the resource
			if (!args.Cancel)
			{
				if (m_feature.ConfigurationDocument != null && m_feature.ConfigurationDocument.Trim().Length == 0)
					m_feature.ConfigurationDocument = null;

				System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
				foreach(string s in toplevels.Keys)
					sb2.Append(string.Format(TEMPLATE_LOC, s, ((System.Text.StringBuilder)toplevels[s]).ToString()));

				string schemaXml = string.Format(TEMPLATE_SMAP, sb2.ToString());
				string configdoc = string.Format(System.Globalization.CultureInfo.InvariantCulture, TEMPLATE_CFG, minx, miny, maxx, maxy);

				System.Xml.XmlDocument doc1 = new System.Xml.XmlDocument();
				System.Xml.XmlDocument doc2 = new System.Xml.XmlDocument();
			
				doc1.LoadXml(configdoc);
				doc2.LoadXml(schemaXml);

				System.Xml.XmlNode newNode = doc1.ImportNode(doc2.DocumentElement, true);
				System.Xml.XmlNode oldNode = doc1.GetElementsByTagName("SchemaMapping")[0];
				oldNode.ParentNode.ReplaceChild(newNode, oldNode);

				if (doc1.FirstChild as System.Xml.XmlDeclaration != null)
					doc1.RemoveChild(doc1.FirstChild);

				doc1.PrependChild(doc1.CreateXmlDeclaration("1.0", "utf-8", null));
			
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				doc1.Save(ms);
				ms = OSGeo.MapGuide.MaestroAPI.Utility.RemoveUTF8BOM(ms);
				ms.Position = 0;

				string docname = m_feature.ConfigurationDocument == null ? "config" : m_feature.ConfigurationDocument;
				m_editor.CurrentConnection.SetResourceData(m_feature.ResourceId, docname, OSGeo.MapGuide.MaestroAPI.ResourceDataType.Stream, ms);

				m_feature.ConfigurationDocument = docname;
				m_editor.CurrentConnection.SaveResource(m_feature);
				m_files = (string[])validitems.ToArray(typeof(string));
			}

			m_editor.HasChanged();
			return !args.Cancel;
		}

		private void GetBounds(Hashtable files, out double minx, out double maxx, out double miny, out double maxy)
		{
			minx = miny = double.MaxValue;
			maxx = maxy = double.MinValue;

			XmlDocument doc = new XmlDocument();
			foreach(System.Text.StringBuilder sb in files.Values)
			{
				doc.LoadXml("<test xmlns=\"http://fdogrfp.osgeo.org/schemas\">" + sb.ToString() + "</test>");
				XmlNamespaceManager nm = new XmlNamespaceManager(doc.NameTable);
				nm.AddNamespace("e", "http://fdogrfp.osgeo.org/schemas");

				foreach(XmlNode n in doc.SelectNodes("e:test/e:Feature/e:Band/e:Image/e:Bounds", nm))
				{
						double coord;
						if (n["MinX"] != null && double.TryParse(n["MinX"].InnerText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out coord))
						{
							minx = Math.Min(coord, minx);
							maxx = Math.Max(coord, maxx);
						}

						if (n["MaxX"] != null && double.TryParse(n["MaxX"].InnerText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out coord))
						{
							minx = Math.Min(coord, minx);
							maxx = Math.Max(coord, maxx);
						}

						if (n["MinY"] != null && double.TryParse(n["MinY"].InnerText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out coord))
						{
							miny = Math.Min(coord, miny);
							maxy = Math.Max(coord, maxy);
						}

						if (n["MaxY"] != null && double.TryParse(n["MaxY"].InnerText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out coord))
						{
							miny = Math.Min(coord, miny);
							maxy = Math.Max(coord, maxy);
						}
				}
			}
		}

		public Hashtable GetFilelist()
		{
			Hashtable filenames = new Hashtable();
			//Try to read the existing data resource and filelist
			if (m_feature.ConfigurationDocument != null && m_feature.ConfigurationDocument.Trim().Length != 0)
			{
				try
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(m_editor.CurrentConnection.GetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument));
					XmlNamespaceManager nm = new XmlNamespaceManager(doc.NameTable);
					nm.AddNamespace("e", "http://fdogrfp.osgeo.org/schemas");
					nm.AddNamespace("gml", "http://www.opengis.net/gml");

					//Not sure why some have double "complexType" entries
					ArrayList totalNodes = new ArrayList();
					foreach(XmlNode n in doc["fdo:DataStore"].SelectNodes("e:SchemaMapping/e:complexType/e:complexType/e:RasterDefinition/e:Location", nm))
						totalNodes.Add(n);
					foreach(XmlNode n in doc["fdo:DataStore"].SelectNodes("e:SchemaMapping/e:complexType/e:RasterDefinition/e:Location", nm))
						totalNodes.Add(n);

					foreach(XmlNode n in totalNodes)
						foreach(XmlNode img in n.SelectNodes("e:Feature", nm))
						{
							string fullname = n.Attributes["name"].Value;
							if (fullname.IndexOf("\\") > 0 && !fullname.EndsWith("\\"))
								fullname += "\\";
							else if (fullname.IndexOf("/") > 0 && !fullname.EndsWith("/"))
								fullname += "/";

							if (img["Band"] == null || img["Band"]["Image"] == null)
								continue;

							XmlNode inner = img["Band"]["Image"];

							fullname += inner.Attributes["name"].Value;
							filenames[fullname] = img.OuterXml;

						}
				}
				catch
				{
					filenames = new Hashtable();
				}
			}

			return filenames;
		}

	}
}
