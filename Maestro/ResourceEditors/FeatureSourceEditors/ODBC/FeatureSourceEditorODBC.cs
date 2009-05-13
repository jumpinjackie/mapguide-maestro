#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Specialized;
using OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for FeautreSourceEditorODBC.
	/// </summary>
	public class FeatureSourceEditorODBC : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox ConnectionType;
		private System.Windows.Forms.Label label2;
		private ResourceEditors.FeatureSourceEditors.ODBC.DSN dsn;
		private ResourceEditors.FeatureSourceEditors.ODBC.Managed managed;
		private ResourceEditors.FeatureSourceEditors.ODBC.Unmanaged unmanaged;
		private ResourceEditors.FeatureSourceEditors.ODBC.Wizard wizard;
		private System.Windows.Forms.Panel CustomPanel;
		private System.Windows.Forms.TextBox ConnectionString;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private System.Windows.Forms.Button RefreshButton;
		private System.Windows.Forms.Button ConfigureButton;
		private bool m_isUpdating = false;
		private Globalizator.Globalizator m_globalizor;

		public delegate void ConnectionStringUpdatedDelegate(string connectionString);

        private string m_wkt = null;
        System.Xml.XmlNode m_crsNode = null;

        /// <summary>
        /// The ODBC provider needs this stuff instead of the coordsys override :(.
        /// {0} is the name, {1} is the wkt, {2,3} and {4,5} is the bbox coordinates.
        /// </summary>
        private const string WKT_HEADER = "<gml:DerivedCRS gml:id=\"{0}\">" +
            "  <gml:remarks>{0}</gml:remarks>" +
            "  <gml:srsName>{0}</gml:srsName>" +
            "  <!-- TODO: Maestro does not know how to read the coordsys extent -->" +
            "  <gml:validArea>" +
            "    <gml:boundingBox>" +
            "      <gml:pos>{2} {3}</gml:pos>" +
            "      <gml:pos>{4} {5}</gml:pos>" +
            "    </gml:boundingBox>" +
            "  </gml:validArea>" +
            "  <gml:baseCRS>" +
            "    <fdo:WKTCRS gml:id=\"{0}\">" +
            "      <gml:srsName>{0}</gml:srsName>" +
            "      <fdo:WKT>{1}</fdo:WKT>" +
            "    </fdo:WKTCRS>" +
            "  </gml:baseCRS>" +
            "  <gml:definedByConversion xlink:href=\"http://fdo.osgeo.org/coord_conversions#identity\"/>" +
            "  <gml:derivedCRSType codeSpace=\"http://fdo.osgeo.org/crs_types\">geographic</gml:derivedCRSType>" +
            "  <gml:usesCS xlink:href=\"http://fdo.osgeo.org/cs#default_cartesian\" />" +
            "</gml:DerivedCRS>";

        private const string XML_WKT_WRAPPER = "<fdo:DataStore xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" " +
            "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
            "xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
            "xmlns:gml=\"http://www.opengis.net/gml\" " +
            "xmlns:fdo=\"http://fdo.osgeo.org/schemas\" " +
            "xmlns:fds=\"http://fdo.osgeo.org/schemas/fds\">{0}</fdo:DataStore>";

		private enum DisplayTypes : int
		{
			Managed,
			Unmanaged,
			DSN,
			Wizard,
			Custom
		}

		public class GeometryColumn
		{
			public string Name;
			public string XColumn;
			public string YColumn;
			public string ZColumn;
		}

		public FeatureSourceEditorODBC(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;
			dsn.SetItem(m_editor, m_feature);
			managed.SetItem(m_editor, m_feature);
			unmanaged.SetItem(m_editor, m_feature);
			wizard.SetItem(m_editor, m_feature);
			credentials.SetItem(m_editor, m_feature);

			foreach(Control c in CustomPanel.Controls)
			{
				c.Visible = false;
				c.Dock = DockStyle.Fill;
			}

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				dsn.UpdateDisplay();
				managed.UpdateDisplay();
				unmanaged.UpdateDisplay();
				wizard.UpdateDisplay();
				credentials.UpdateDisplay();

				//This is spaghetti code, yum!
				if ((m_feature.Parameter["DataSourceName"] == null || m_feature.Parameter["DataSourceName"].Length == 0) &&
					(m_feature.Parameter["ConnectionString"] == null || m_feature.Parameter["ConnectionString"].Length == 0))
					ConnectionType.SelectedIndex = (int)DisplayTypes.Managed;
                else if (m_feature.Parameter["DataSourceName"] != null && m_feature.Parameter["DataSourceName"].Length != 0)
					ConnectionType.SelectedIndex = (int)DisplayTypes.DSN;
				else if (m_feature.Parameter["ConnectionString"].IndexOf("%MG_DATA_FILE_PATH%") > 0)
					ConnectionType.SelectedIndex = (int)DisplayTypes.Managed;
				else if (m_feature.Parameter["ConnectionString"].IndexOf("%MG_DATA_PATH_ALIAS[") > 0)
					ConnectionType.SelectedIndex = (int)DisplayTypes.Unmanaged;
				else
				{
					NameValueCollection nv = ConnectionStringManager.SplitConnectionString(m_feature.Parameter["ConnectionString"]);
					if (nv["Provider"] != null && nv["Provider"].Length != 0)
						ConnectionType.SelectedIndex = (int)DisplayTypes.Wizard;
					else
					{
						NameValueCollection test = new NameValueCollection();
						ConnectionStringManager.InsertDefaultValues(test, nv["Driver"]);
						if (test.Count > 0)
							ConnectionType.SelectedIndex = (int)DisplayTypes.Wizard;
						else if (m_feature.Parameter["DataSourceName"] != null && m_feature.Parameter["DataSourceName"].IndexOf("Dbq=") >= 0)
							ConnectionType.SelectedIndex = (int)DisplayTypes.Unmanaged;
                        else if (m_feature.Parameter["ConnectionString"] != null && m_feature.Parameter["ConnectionString"].IndexOf("Dbq=") >= 0)
                            ConnectionType.SelectedIndex = (int)DisplayTypes.Unmanaged;
                        else
							ConnectionType.SelectedIndex = (int)DisplayTypes.Custom;
					}
				}

				if (m_feature.Parameter["ConnectionString"] == null || m_feature.Parameter["ConnectionString"].Length == 0)
					ConnectionString.Text = "";
				else
					ConnectionString.Text = m_feature.Parameter["ConnectionString"];

                m_crsNode = null;
                m_wkt = null;

                //The ODBC provider does not support this
                m_feature.SupplementalSpatialContextInfo = null;

                if (!string.IsNullOrEmpty(m_feature.ConfigurationDocument))
                {
                    try
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.Load(m_editor.CurrentConnection.GetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument));

                        System.Xml.XmlNamespaceManager nm = new System.Xml.XmlNamespaceManager(doc.NameTable);
                        nm.AddNamespace("fdo", "http://fdo.osgeo.org/schemas");
                        nm.AddNamespace("gml", "http://www.opengis.net/gml");
                        nm.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

                        m_crsNode = doc.SelectSingleNode("fdo:DataStore/gml:DerivedCRS", nm);
                        if (m_crsNode != null)
                        {
                            System.Xml.XmlNode wktNode = m_crsNode.SelectSingleNode("gml:baseCRS/fdo:WKTCRS/fdo:WKT", nm);
                            if (wktNode != null)
                                m_wkt = wktNode.InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format("Failed to read current coordsys configuration: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (m_wkt != null)
                {
                    m_feature.SupplementalSpatialContextInfo = new OSGeo.MapGuide.MaestroAPI.SpatialContextTypeCollection();
                    MaestroAPI.SpatialContextType cn = new OSGeo.MapGuide.MaestroAPI.SpatialContextType();
                    cn.CoordinateSystem = m_wkt;
                    cn.Name = "Default";
                    m_feature.SupplementalSpatialContextInfo.Add(cn);
                }

			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private FeatureSourceEditorODBC()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			m_globalizor = new Globalizator.Globalizator(this);
			string tmp = m_globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditorODBC.ConnectionType.Items");
			if (tmp != null && tmp.Trim().Length > 0)
			{
				ArrayList fix = new ArrayList();
				foreach(string s in tmp.Trim().Split('\n'))
					if (s.Trim().Length > 0)
						fix.Add(s.Trim());

				if (fix.Count == ConnectionType.Items.Count)
				{
					ConnectionType.Items.Clear();
					ConnectionType.Items.AddRange(fix.ToArray());
				}
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.ConnectionType = new System.Windows.Forms.ComboBox();
			this.CustomPanel = new System.Windows.Forms.Panel();
			this.dsn = new ResourceEditors.FeatureSourceEditors.ODBC.DSN();
			this.managed = new ResourceEditors.FeatureSourceEditors.ODBC.Managed();
			this.unmanaged = new ResourceEditors.FeatureSourceEditors.ODBC.Unmanaged();
			this.wizard = new ResourceEditors.FeatureSourceEditors.ODBC.Wizard();
			this.ConnectionString = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.credentials = new ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
			this.RefreshButton = new System.Windows.Forms.Button();
			this.ConfigureButton = new System.Windows.Forms.Button();
			this.CustomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Connection Type";
			// 
			// ConnectionType
			// 
			this.ConnectionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ConnectionType.Items.AddRange(new object[] {
																"Internal file (Managed)",
																"File on the server (Unmanaged)",
																"DSN (Named ODBC)",
																"Known database type",
																"Custom connectionstring"});
			this.ConnectionType.Location = new System.Drawing.Point(128, 0);
			this.ConnectionType.Name = "ConnectionType";
			this.ConnectionType.Size = new System.Drawing.Size(424, 21);
			this.ConnectionType.TabIndex = 1;
			this.ConnectionType.SelectedIndexChanged += new System.EventHandler(this.ConnectionType_SelectedIndexChanged);
			// 
			// CustomPanel
			// 
			this.CustomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.CustomPanel.Controls.Add(this.dsn);
			this.CustomPanel.Controls.Add(this.managed);
			this.CustomPanel.Controls.Add(this.unmanaged);
			this.CustomPanel.Controls.Add(this.wizard);
			this.CustomPanel.Location = new System.Drawing.Point(8, 32);
			this.CustomPanel.Name = "CustomPanel";
			this.CustomPanel.Size = new System.Drawing.Size(544, 120);
			this.CustomPanel.TabIndex = 2;
			// 
			// dsn
			// 
			this.dsn.Location = new System.Drawing.Point(8, 16);
			this.dsn.Name = "dsn";
			this.dsn.Size = new System.Drawing.Size(128, 96);
			this.dsn.TabIndex = 3;
			this.dsn.ConnectionStringUpdated += new ResourceEditors.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdated);
			// 
			// managed
			// 
			this.managed.Location = new System.Drawing.Point(144, 16);
			this.managed.Name = "managed";
			this.managed.Size = new System.Drawing.Size(128, 96);
			this.managed.TabIndex = 2;
			this.managed.ConnectionStringUpdated += new ResourceEditors.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdated);
			// 
			// unmanaged
			// 
			this.unmanaged.Location = new System.Drawing.Point(288, 16);
			this.unmanaged.Name = "unmanaged";
			this.unmanaged.Size = new System.Drawing.Size(112, 96);
			this.unmanaged.TabIndex = 1;
			this.unmanaged.ConnectionStringUpdated += new ResourceEditors.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdated);
			// 
			// wizard
			// 
			this.wizard.Location = new System.Drawing.Point(424, 16);
			this.wizard.Name = "wizard";
			this.wizard.Size = new System.Drawing.Size(104, 96);
			this.wizard.TabIndex = 0;
			this.wizard.ConnectionStringUpdated += new ResourceEditors.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdated);
			// 
			// ConnectionString
			// 
			this.ConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectionString.Location = new System.Drawing.Point(8, 352);
			this.ConnectionString.Multiline = true;
			this.ConnectionString.Name = "ConnectionString";
			this.ConnectionString.Size = new System.Drawing.Size(536, 56);
			this.ConnectionString.TabIndex = 3;
			this.ConnectionString.Text = "ConnectionString";
			this.ConnectionString.TextChanged += new System.EventHandler(this.ConnectionString_TextChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(8, 328);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Connection String";
			// 
			// credentials
			// 
			this.credentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.credentials.Location = new System.Drawing.Point(8, 160);
			this.credentials.Name = "credentials";
			this.credentials.Size = new System.Drawing.Size(544, 152);
			this.credentials.TabIndex = 5;
			this.credentials.ConnectionStringUpdated += new ResourceEditors.FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate(this.ConnectionStringUpdated);
			// 
			// RefreshButton
			// 
			this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.RefreshButton.Location = new System.Drawing.Point(424, 328);
			this.RefreshButton.Name = "RefreshButton";
			this.RefreshButton.Size = new System.Drawing.Size(120, 24);
			this.RefreshButton.TabIndex = 6;
			this.RefreshButton.Text = "Rebuild Schema";
			this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
			// 
			// ConfigureButton
			// 
			this.ConfigureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ConfigureButton.Location = new System.Drawing.Point(272, 328);
			this.ConfigureButton.Name = "ConfigureButton";
			this.ConfigureButton.Size = new System.Drawing.Size(136, 24);
			this.ConfigureButton.TabIndex = 7;
			this.ConfigureButton.Text = "Configure columns";
			this.ConfigureButton.Click += new System.EventHandler(this.ConfigureButton_Click);
			// 
			// FeatureSourceEditorODBC
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(560, 416);
			this.Controls.Add(this.ConfigureButton);
			this.Controls.Add(this.RefreshButton);
			this.Controls.Add(this.credentials);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ConnectionString);
			this.Controls.Add(this.CustomPanel);
			this.Controls.Add(this.ConnectionType);
			this.Controls.Add(this.label1);
			this.Name = "FeatureSourceEditorODBC";
			this.Size = new System.Drawing.Size(560, 416);
			this.CustomPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public object Resource
		{
			get { return m_feature; }
			set 
			{
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
				dsn.SetItem(m_editor, m_feature);
				managed.SetItem(m_editor, m_feature);
				unmanaged.SetItem(m_editor, m_feature);
				wizard.SetItem(m_editor, m_feature);
				credentials.SetItem(m_editor, m_feature);
				UpdateDisplay();
			}
		}

		public string ResourceId
		{
			get { return m_feature.ResourceId; }
			set { m_feature.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

		public bool Save(string savename)
		{

            if (m_feature.SupplementalSpatialContextInfo == null || m_feature.SupplementalSpatialContextInfo.Count == 0)
            {
                //Remove the CRSnode
                UpdateConfigWithCoordsys(null);
            }
            else
            {
                string wkt = m_feature.SupplementalSpatialContextInfo[0].CoordinateSystem;
                //m_feature.SupplementalSpatialContextInfo = null;

                string name = m_editor.CurrentConnection.CoordinateSystem.ConvertWktToCoordinateSystemCode(wkt);

                if (wkt != m_wkt)
                    //TODO: Figure out how to get the coordsys extent
                    UpdateConfigWithCoordsys(string.Format(System.Globalization.CultureInfo.InvariantCulture, WKT_HEADER, name, wkt, 0.0, 0.0, 0.0, 0.0));

            }
			return false;
		}

        private void UpdateConfigWithCoordsys(string coordsysFragment)
        {
            if (!string.IsNullOrEmpty(m_feature.ConfigurationDocument))
            {
                try
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(m_editor.CurrentConnection.GetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument));

                    System.Xml.XmlNamespaceManager nm = new System.Xml.XmlNamespaceManager(doc.NameTable);
                    nm.AddNamespace("fdo", "http://fdo.osgeo.org/schemas");
                    nm.AddNamespace("gml", "http://www.opengis.net/gml");
                    nm.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

                    System.Xml.XmlNode root = doc.SelectSingleNode("fdo:DataStore", nm);
                    System.Xml.XmlNode crsNode = root.SelectSingleNode("gml:DerivedCRS", nm);
                    if (crsNode != null)
                        root.RemoveChild(crsNode);

                    if (!string.IsNullOrEmpty(coordsysFragment))
                    {
                        string tmpXml = string.Format(XML_WKT_WRAPPER, coordsysFragment);
                        System.Xml.XmlDocument d2 = new System.Xml.XmlDocument();
                        d2.LoadXml(tmpXml);

                        root.InsertBefore(doc.ImportNode(d2.FirstChild.FirstChild, true), root.FirstChild);
                    }

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        doc.Save(ms);
                        m_editor.CurrentConnection.SetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument, OSGeo.MapGuide.MaestroAPI.ResourceDataType.Stream, ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Failed to read current coordsys configuration: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

		private void ConnectionStringUpdated(string connectionString)
		{
			try
			{
				m_isUpdating = true;
				ConnectionString.Text = connectionString;
				m_editor.HasChanged();
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void ConnectionType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Control sel = null;
			switch((DisplayTypes)ConnectionType.SelectedIndex)
			{
				case DisplayTypes.Managed:
					sel = managed;
					break;
				case DisplayTypes.Unmanaged:
					sel = unmanaged;
					break;
				case DisplayTypes.DSN:
					sel = dsn;
					break;
				case DisplayTypes.Wizard:
					sel = wizard;
					break;
				case DisplayTypes.Custom:
					break;
			}

			foreach(Control c in CustomPanel.Controls)
				c.Visible = c == sel;

		}

		private void ConnectionString_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			m_feature.Parameter["ConnectionString"] = ConnectionString.Text;
			m_feature.Parameter["Datasource"] = "";
			m_editor.HasChanged();
		}

		private void RefreshButton_Click(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			try
			{
				Hashtable geometry = new Hashtable();
				Hashtable keys = new Hashtable();
				//This is always a temp resource, so it is safe to update it
				if (m_feature.ConfigurationDocument != null && m_feature.ConfigurationDocument.Length != 0)
				{
					try
					{
						System.Xml.XmlDocument prev = new System.Xml.XmlDocument();
						prev.Load(m_editor.CurrentConnection.GetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument));
						geometry = GeometryColumns(prev);
						keys = KeyColumns(prev);
					}
					catch
					{
					}

					try
					{
						m_editor.CurrentConnection.DeleteResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument);
					}
					catch
					{
					}
				}

				m_feature.ConfigurationDocument = null;
				m_editor.CurrentConnection.SaveResource(m_feature);

				UpdateConfigDocument(GetCleanConfig(geometry, keys));

			}
			catch (Exception ex)
			{
				MessageBox.Show(this, string.Format("Failed to refresh schema: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateConfigDocument(System.Xml.XmlDocument config)
		{
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				OSGeo.MapGuide.MaestroAPI.Utf8XmlWriter wr = new OSGeo.MapGuide.MaestroAPI.Utf8XmlWriter(ms);
				config.Save(wr);
				System.IO.MemoryStream ms2 = OSGeo.MapGuide.MaestroAPI.Utility.RemoveUTF8BOM(ms);
				m_editor.CurrentConnection.SetResourceData(m_feature.ResourceId, "config", OSGeo.MapGuide.MaestroAPI.ResourceDataType.Stream, ms2);
				if (ms2 != ms)
					(ms2 as IDisposable).Dispose();
			}

			m_feature.ConfigurationDocument = "config";
			m_editor.CurrentConnection.SaveResource(m_feature);
			m_editor.HasChanged();
		}

		public Hashtable KeyColumns(System.Xml.XmlDocument doc)
		{
			Hashtable results = new Hashtable();
			if (doc["fdo:DataStore"] != null && doc["fdo:DataStore"]["xs:schema"] != null )
					foreach(System.Xml.XmlNode n in doc["fdo:DataStore"]["xs:schema"].ChildNodes)
						if (n.Name == "xs:element" && n.Attributes["name"] != null && n.FirstChild != null && n.FirstChild.Name == "xs:key")
							foreach(System.Xml.XmlNode nx in n.FirstChild.ChildNodes)
								if (nx.Name == "xs:field" && nx.Attributes["xpath"] != null)
								{
									results[n.Attributes["name"].Value] = nx.Attributes["xpath"].Value;
									break;
								}

			return results;
		}

		public Hashtable GeometryColumns(System.Xml.XmlDocument doc)
		{
			Hashtable results = new Hashtable();

			if (doc["fdo:DataStore"] != null && doc["fdo:DataStore"]["SchemaMapping"] != null)
					foreach(System.Xml.XmlNode n in doc["fdo:DataStore"]["SchemaMapping"].ChildNodes)
						if (n.Name == "complexType" && n["Table"] != null && n["Table"].Attributes["name"] != null)
						{
							string tablename = n["Table"].Attributes["name"].Value;

							foreach(System.Xml.XmlNode c in n.ChildNodes)
								if (c.Name == "element" && c.Attributes["name"] != null && c.Attributes["xColumnName"] != null && c.Attributes["yColumnName"] != null)
								{
									GeometryColumn gc = new GeometryColumn();
									gc.XColumn = c.Attributes["xColumnName"].Value;
									gc.YColumn = c.Attributes["yColumnName"].Value;
									if (c.Attributes["zColumnName"] != null)
										gc.ZColumn = c.Attributes["zColumnName"].Value;
									gc.Name = c.Attributes["name"].Value;

									if (!results.ContainsKey(tablename))
										results.Add(tablename, new ArrayList());
									results[tablename] = gc;
								}
						}

			return results;
		}

		private System.Xml.XmlDocument GetCleanConfig(Hashtable geometryColumns, Hashtable keyColumns)
		{
			System.IO.MemoryStream scm_ms = new System.IO.MemoryStream();
			string backSet = m_feature.ConfigurationDocument;
			try
			{
				m_feature.ConfigurationDocument = null;
				m_editor.CurrentConnection.SaveResource(m_feature);

				//This is used to read the schema in raw xml
				NameValueCollection param = new NameValueCollection();
				param.Add("OPERATION", "DESCRIBEFEATURESCHEMA");
				param.Add("VERSION", "1.0.0");
				param.Add("SESSION", m_editor.CurrentConnection.SessionID);
				param.Add("FORMAT", "text/xml");
				param.Add("RESOURCEID", m_feature.ResourceId);

				OSGeo.MapGuide.MaestroAPI.Utility.CopyStream(((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).ExecuteOperation(param), scm_ms);
			}
			finally
			{
				m_feature.ConfigurationDocument = backSet;
				m_editor.CurrentConnection.SaveResource(m_feature);
			}

			System.Xml.XmlDocument schema = new System.Xml.XmlDocument();
			scm_ms.Position = 0;
			schema.Load(scm_ms);

			if (schema.FirstChild.NodeType == System.Xml.XmlNodeType.XmlDeclaration)
				schema.RemoveChild(schema.FirstChild);

			if (schema.FirstChild.Attributes["targetNamespace"] == null)
				schema.FirstChild.Attributes.Append(schema.CreateAttribute("targetNamespace"));
			schema.FirstChild.Attributes["targetNamespace"].Value = "http://fdo.osgeo.org/schemas/feature/Default";
			if (schema.FirstChild.Attributes["xmlns:Default"] == null)
				schema.FirstChild.Attributes.Append(schema.CreateAttribute("xmlns:Default"));
			schema.FirstChild.Attributes["xmlns:Default"].Value = "http://fdo.osgeo.org/schemas/feature/Default";

            //Preserve the crs, if any
            if (m_crsNode != null)
                schema.FirstChild.AppendChild(m_crsNode.CloneNode(true));

			Hashtable approvedKeys = new Hashtable();

			foreach(System.Xml.XmlNode n in schema.FirstChild.ChildNodes)
				if (n.Name == "xs:element" && n.Attributes["name"] != null && n.Attributes["type"] != null)
					n.Attributes["type"].Value = "Default:" + n.Attributes["name"].Value + "Type";
				else if (n.Name == "xs:complexType" && n.Attributes["name"] != null && n.Attributes["name"].Value.EndsWith("Type"))
				{
					string tablename = n.Attributes["name"].Value.Substring(0, n.Attributes["name"].Value.Length - "Type".Length);
					if (geometryColumns.ContainsKey(tablename))
					{
						n.Attributes.Append(n.OwnerDocument.CreateAttribute("fdo:hasGeometry")).Value = "true";
						n.Attributes.Append(n.OwnerDocument.CreateAttribute("fdo:geometryName")).Value = ((GeometryColumn)geometryColumns[tablename]).Name;
						if (n.FirstChild != null && n.FirstChild.FirstChild != null & n.FirstChild.FirstChild.FirstChild != null)
						{
							//Extend the class type if needed
							if (n.FirstChild.Name == "xs:sequence")
							{
								string tmp = "<xs:complexContent><xs:extension base=\"fdo:ClassType\">" + n.InnerXml + "</xs:extension></xs:complexContent>";
								n.InnerXml = tmp;
							}

							System.Xml.XmlNode bas = n.FirstChild.FirstChild;
							if (bas.Name == "xs:extension" && bas.Attributes["base"] != null)
								bas.Attributes["base"].Value = "gml:AbstractFeatureType";

							System.Xml.XmlNode seq = n.FirstChild.FirstChild.FirstChild;
							if (seq.Name == "xs:sequence")
							{
								System.Xml.XmlNode g = seq.AppendChild(seq.OwnerDocument.CreateElement("xs:element", "http://www.w3.org/2001/XMLSchema"));
								g.Attributes.Append(g.OwnerDocument.CreateAttribute("name")).Value = ((GeometryColumn)geometryColumns[tablename]).Name;
								g.Attributes.Append(g.OwnerDocument.CreateAttribute("type")).Value = "gml:AbstractGeometryType";
								g.Attributes.Append(g.OwnerDocument.CreateAttribute("fdo:hasMeasure")).Value = "false";
								g.Attributes.Append(g.OwnerDocument.CreateAttribute("fdo:hasElevation")).Value = "false";
								//g.Attributes.Append(g.OwnerDocument.CreateAttribute("fdo:srsName")).Value = "";
								g.Attributes.Append(g.OwnerDocument.CreateAttribute("fdo:geometricTypes")).Value = "point";
								g.Attributes.Append(g.OwnerDocument.CreateAttribute("fdo:geometryTypes")).Value = "point multipoint";
							}
						}
					}
					else
						n.Attributes.Append(n.OwnerDocument.CreateAttribute("fdo:hasGeometry")).Value = "false";

					if (keyColumns.ContainsKey(tablename))
						if (n.FirstChild != null && n.FirstChild.FirstChild != null & n.FirstChild.FirstChild.FirstChild != null)
						{
                            //Extend the class type if needed
                            if (n.FirstChild.Name == "xs:sequence")
                            {
                                string tmp = "<xs:complexContent><xs:extension base=\"fdo:ClassType\">" + n.InnerXml + "</xs:extension></xs:complexContent>";
                                n.InnerXml = tmp;
                            }
                            
                            string colName = (string)keyColumns[tablename];
							System.Xml.XmlNode seq = n.FirstChild.FirstChild.FirstChild;

							if (seq.Name == "xs:sequence")
								foreach(System.Xml.XmlNode nx in seq.ChildNodes)
									if (nx.Name == "xs:element" && nx.Attributes["name"] != null && nx.Attributes["name"].Value == colName)
									{
										approvedKeys[tablename] = colName;
										break;
									}
						}
				}

			keyColumns = approvedKeys;

            Hashtable keySelectors = new Hashtable();
            foreach (string s in keyColumns.Keys)
                keySelectors.Add(s, null);

            //Find tables with existing keys
            foreach (System.Xml.XmlNode n in schema.FirstChild.ChildNodes)
                if (n.Name == "xs:element" && n.Attributes["name"] != null && n.Attributes["type"] != null && keyColumns.ContainsKey(n.Attributes["name"].Value))
                    keySelectors[n.Attributes["name"].Value] = n;

            //Create selectors for non-existing keys
            foreach(string s in keySelectors.Keys)
                if (keySelectors[s] == null)
            {
    				System.Xml.XmlNode selector = schema.FirstChild.AppendChild(schema.CreateElement("xs:element", "http://www.w3.org/2001/XMLSchema"));
                    selector.Attributes.Append(selector.OwnerDocument.CreateAttribute("name")).Value = s;
                    selector.Attributes.Append(selector.OwnerDocument.CreateAttribute("type")).Value = "Default:" + s + "Type";
                    selector.Attributes.Append(selector.OwnerDocument.CreateAttribute("abstract")).Value = "false";
                    selector.Attributes.Append(selector.OwnerDocument.CreateAttribute("substitutionGroup")).Value = "gml:_Feature";

                    System.Xml.XmlNode key = selector.AppendChild(selector.OwnerDocument.CreateElement("xs:key", "http://www.w3.org/2001/XMLSchema"));
                    key.Attributes.Append(key.OwnerDocument.CreateAttribute("name")).Value = s + "Key";

                    System.Xml.XmlNode sl = key.AppendChild(selector.OwnerDocument.CreateElement("xs:selector", "http://www.w3.org/2001/XMLSchema"));
                    sl.Attributes.Append(sl.OwnerDocument.CreateAttribute("xpath")).Value = ".//" + s;

                    System.Xml.XmlNode f = key.AppendChild(selector.OwnerDocument.CreateElement("xs:field", "http://www.w3.org/2001/XMLSchema"));
                    f.Attributes.Append(f.OwnerDocument.CreateAttribute("xpath")).Value = (string)keyColumns[s];
            }

			foreach(System.Xml.XmlNode n in schema.FirstChild.ChildNodes)
				if (n.Name == "xs:element" && n.Attributes["name"] != null && keyColumns.ContainsKey(n.Attributes["name"].Value))
					{
						ArrayList toRemove = new ArrayList();
						if (n.FirstChild != null && n.FirstChild.Name == "xs:key")
							foreach(System.Xml.XmlNode nx in n.FirstChild.ChildNodes)
								if (nx.Name == "xs:field" && nx.Attributes["xpath"] != null)
									toRemove.Add(nx);

						foreach(System.Xml.XmlNode nx in toRemove)
							nx.ParentNode.RemoveChild(nx);

						System.Xml.XmlNode f = n.FirstChild.AppendChild(n.OwnerDocument.CreateElement("xs:field", "http://www.w3.org/2001/XMLSchema"));
						f.Attributes.Append(f.OwnerDocument.CreateAttribute("xpath")).Value = (string)keyColumns[n.Attributes["name"].Value];
					}
				


			scm_ms.Position = 0;
			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription fsd = new OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription(scm_ms);

			//Using a temp document to avoid all that namespace clutter that .Net uses
			System.Xml.XmlDocument temp = new System.Xml.XmlDocument();
			temp.LoadXml("<temp></temp>");
			foreach(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm in fsd.Schemas)
			{
				System.Xml.XmlNode type = temp.FirstChild.AppendChild(temp.CreateElement("complexType"));
				type.Attributes.Append(temp.CreateAttribute("name")).Value = scm.Name + "Type";
				type.AppendChild(temp.CreateElement("Table")).Attributes.Append(temp.CreateAttribute("name")).Value = scm.Name;

				foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn fsc in scm.Columns)
				{
					System.Xml.XmlNode elm = type.AppendChild(temp.CreateElement("element"));
					elm.Attributes.Append(temp.CreateAttribute("name")).Value = fsc.Name;
					elm.AppendChild(temp.CreateElement("Column")).Attributes.Append(temp.CreateAttribute("name")).Value = fsc.Name;
				}

				if (geometryColumns.ContainsKey(scm.Name))
				{
					GeometryColumn gc = (GeometryColumn)geometryColumns[scm.Name];
					System.Xml.XmlNode elm = type.AppendChild(temp.CreateElement("element"));
					elm.Attributes.Append(temp.CreateAttribute("name")).Value = gc.Name;
					elm.Attributes.Append(temp.CreateAttribute("xColumnName")).Value = gc.XColumn;
					elm.Attributes.Append(temp.CreateAttribute("yColumnName")).Value = gc.YColumn;
					if(gc.ZColumn != null)
						elm.Attributes.Append(temp.CreateAttribute("zColumnName")).Value = gc.ZColumn;
				}
			}

			System.Xml.XmlDocument mapping = new System.Xml.XmlDocument();
			string mappingRoot = "<SchemaMapping xmlns:rdb=\"http://fdordbms.osgeo.org/schemas\" xmlns=\"http://fdoodbc.osgeo.org/schemas\" provider=\"\" name=\"Default\" >";
			mappingRoot += temp.FirstChild.InnerXml;
			mappingRoot += "</SchemaMapping>";
			mapping.LoadXml(mappingRoot);

			System.Xml.XmlNamespaceManager nm = new System.Xml.XmlNamespaceManager(mapping.NameTable);
			nm.AddNamespace("xmlns:rdb", "http://fdordbms.osgeo.org/schemas");
			nm.AddNamespace("", "http://fdoodbc.osgeo.org/schemas");
			OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider fpr = m_editor.CurrentConnection.GetFeatureProvider(m_feature.Provider);
			mapping.FirstChild.Attributes["provider"].Value = fpr.Name;

			System.Xml.XmlDocument config = new System.Xml.XmlDocument();
			/*System.Xml.XmlNamespaceManager nm = new System.Xml.XmlNamespaceManager(config.NameTable);
				nm.AddNamespace("xmlns:xs", "http://www.w3.org/2001/XMLSchema");
				nm.AddNamespace("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
				nm.AddNamespace("xmlns:xlink", "http://www.w3.org/1999/xlink");
				nm.AddNamespace("xmlns:gml", "http://www.opengis.net/gml");
				nm.AddNamespace("xmlns:fdo", "http://fdo.osgeo.org/schemas");
				nm.AddNamespace("xmlns:fds", "http://fdo.osgeo.org/schemas/fds");*/

			string fdoRoot = "<fdo:DataStore xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:gml=\"http://www.opengis.net/gml\" xmlns:fdo=\"http://fdo.osgeo.org/schemas\" xmlns:fds=\"http://fdo.osgeo.org/schemas/fds\">";
			fdoRoot += schema.FirstChild.OuterXml;
			fdoRoot += mapping.InnerXml;
			fdoRoot += "</fdo:DataStore>";

			config.LoadXml(fdoRoot);
			return config;
		}

		private void ConfigureButton_Click(object sender, System.EventArgs e)
		{
			GeometryColumnsEditor dlg = new GeometryColumnsEditor();

			Hashtable geometry = new Hashtable();
			Hashtable keys = new Hashtable();
            
            //Make sure there is no autogeneration, as that hides the properties
            m_feature.Parameter["GenerateDefaultGeometryProperty"] = "false";
            
            if (m_feature.ConfigurationDocument != null && m_feature.ConfigurationDocument.Length != 0)
			{
				try
				{
					System.Xml.XmlDocument prev = new System.Xml.XmlDocument();
					prev.Load(m_editor.CurrentConnection.GetResourceData(m_feature.ResourceId, m_feature.ConfigurationDocument));
					geometry = GeometryColumns(prev);
					keys = KeyColumns(prev);
				}
				catch(Exception ex)
				{
					MessageBox.Show(this, string.Format("Failed to read current column configuration: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}


			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription fsd = null;
			try
			{
				string backSet = m_feature.ConfigurationDocument;
				try
				{
					m_feature.ConfigurationDocument = null;
					m_editor.CurrentConnection.SaveResource(m_feature);
					fsd = m_editor.CurrentConnection.DescribeFeatureSource(m_feature.ResourceId);
				}
				finally
				{
					m_feature.ConfigurationDocument = backSet;
					m_editor.CurrentConnection.SaveResource(m_feature);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, string.Format("Failed to read current datasource layout: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			dlg.Setup(fsd, geometry, keys);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				UpdateConfigDocument(GetCleanConfig(dlg.GeometryColumns, dlg.KeyColumns));	
			}
		}

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) 
        {
            //Save as temp before validating, to include projection data
            Save(null);
            return true; 
        }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }

	}
}
