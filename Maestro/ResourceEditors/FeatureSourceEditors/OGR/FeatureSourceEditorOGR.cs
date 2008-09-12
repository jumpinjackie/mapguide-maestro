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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for FeatureSourceEditorOGR.
	/// </summary>
	public class FeatureSourceEditorOGR : System.Windows.Forms.UserControl, ResourceEditor
	{
		private System.Windows.Forms.ComboBox ConnectionType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel ProviderPanel;
		private ResourceEditors.FeatureSourceEditors.OGR.Custom custom;
		private ResourceEditors.FeatureSourceEditors.OGR.MySQL mySQL;
		private ResourceEditors.FeatureSourceEditors.OGR.Oracle oracle;
		private ResourceEditors.FeatureSourceEditors.OGR.ODBC odbc;
		private ResourceEditors.FeatureSourceEditors.OGR.PostGIS postGIS;
		private ResourceEditors.FeatureSourceEditors.OGR.Managed managed;
		private ResourceEditors.FeatureSourceEditors.OGR.Unmanaged unmanaged;
		private ResourceEditors.FeatureSourceEditors.OGR.DODS dods;
		private ResourceEditors.FeatureSourceEditors.OGR.ArcSDE arcSDE;
		private ResourceEditors.FeatureSourceEditors.OGR.Grass grass;
		private ResourceEditors.FeatureSourceEditors.OGR.OGDI ogdi;
		private ResourceEditors.FeatureSourceEditors.OGR.Informix informix;
		private ResourceEditors.FeatureSourceEditors.OGR.FME fme;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.CheckBox WriteProtected;
		private bool m_isUpdating = false;
		private Globalizator.Globalizator m_globalizor = null;


		private enum ProviderTypes
		{
			Managed,
			Unmanaged,
			DODS,
			ArcSDE,
			GRASS,
			OGDI,
			Informix,
			PostGIS,
			MySQL,
			ODBC,
			Oracle,
			FME,
			Custom
		}

		public FeatureSourceEditorOGR(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;

			SetItem();
			UpdateDisplay();
		}

		private void SetItem()
		{
			managed.SetItem(m_editor, m_feature);
			unmanaged.SetItem(m_editor, m_feature);
			arcSDE.SetItem(m_editor, m_feature);
			postGIS.SetItem(m_editor, m_feature);
			odbc.SetItem(m_editor, m_feature);
			grass.SetItem(m_editor, m_feature);
			oracle.SetItem(m_editor, m_feature);
			mySQL.SetItem(m_editor, m_feature);
			ogdi.SetItem(m_editor, m_feature);
			custom.SetItem(m_editor, m_feature);
			dods.SetItem(m_editor, m_feature);
			informix.SetItem(m_editor, m_feature);
			fme.SetItem(m_editor, m_feature);

			SelectBestFit();
		}

		private void SelectBestFit()
		{
			ProviderTypes item = ProviderTypes.Custom;
			if (m_feature != null)
			{
				string constr = m_feature.Parameter["DataSource"];
				if (constr == null)
					constr = "";

				constr = constr.Trim();

				if (constr.IndexOf("%MG_DATA_PATH%") >= 0)
					item = ProviderTypes.Managed;
				else
				{
					int index = constr.IndexOf(":");
					if (index < 0)
						item = ProviderTypes.Managed;
					else if (index == 0)
						item = ProviderTypes.Custom;
					else if (index == 1)
						item = ProviderTypes.Unmanaged;
					else
						switch(constr.Substring(0, index).ToUpper())
						{
							case "DODS":
								item = ProviderTypes.DODS;
								break;
							case "SDE":
								item = ProviderTypes.ArcSDE;
								break;
							case "GRASS":
								item = ProviderTypes.GRASS;
								break;
							case "MYSQL":
								item = ProviderTypes.MySQL;
								break;
							case "GLTP":
								item = ProviderTypes.OGDI;
								break;
							case "ODBC":
								item = ProviderTypes.ODBC;
								break;
							case "OCI":
								item = ProviderTypes.Oracle;
								break;
							case "PG":
								item = ProviderTypes.PostGIS;
								break;
							case "IDB":
								item = ProviderTypes.Informix;
								break;
							default:
								item = ProviderTypes.FME;
								break;
						}
				}
					
			}

			ConnectionType.SelectedIndex = (int)item;
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_feature != null && m_feature.Parameter != null && m_feature.Parameter["ReadOnly"] != null)
					WriteProtected.Checked = m_feature.Parameter["ReadOnly"].Trim().ToUpper() == "TRUE";
				else
					WriteProtected.Checked = true;

				foreach(UserControl c in ProviderPanel.Controls)
					if (c.Visible)
					{
						System.Reflection.MethodInfo mi = c.GetType().GetMethod("UpdateDisplay");
						if (mi != null)
							mi.Invoke(c, null);
					}
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public FeatureSourceEditorOGR()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			foreach(UserControl uc in ProviderPanel.Controls)
			{
				uc.Visible = false;
				uc.Dock = DockStyle.Fill;
			}

			m_globalizor = new Globalizator.Globalizator(this);

			string tmp = m_globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditorOGR.ConnectionType.Items");
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
			this.ConnectionType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ProviderPanel = new System.Windows.Forms.Panel();
			this.fme = new ResourceEditors.FeatureSourceEditors.OGR.FME();
			this.informix = new ResourceEditors.FeatureSourceEditors.OGR.Informix();
			this.ogdi = new ResourceEditors.FeatureSourceEditors.OGR.OGDI();
			this.grass = new ResourceEditors.FeatureSourceEditors.OGR.Grass();
			this.arcSDE = new ResourceEditors.FeatureSourceEditors.OGR.ArcSDE();
			this.dods = new ResourceEditors.FeatureSourceEditors.OGR.DODS();
			this.custom = new ResourceEditors.FeatureSourceEditors.OGR.Custom();
			this.mySQL = new ResourceEditors.FeatureSourceEditors.OGR.MySQL();
			this.oracle = new ResourceEditors.FeatureSourceEditors.OGR.Oracle();
			this.odbc = new ResourceEditors.FeatureSourceEditors.OGR.ODBC();
			this.postGIS = new ResourceEditors.FeatureSourceEditors.OGR.PostGIS();
			this.managed = new ResourceEditors.FeatureSourceEditors.OGR.Managed();
			this.unmanaged = new ResourceEditors.FeatureSourceEditors.OGR.Unmanaged();
			this.WriteProtected = new System.Windows.Forms.CheckBox();
			this.ProviderPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ConnectionType
			// 
			this.ConnectionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ConnectionType.Items.AddRange(new object[] {
																"Internal file (Managed)",
																"File on the server (Unmanaged)",
																"DODS",
																"ArcSDE",
																"GRASS",
																"OGDI",
																"Informix",
																"PostGIS",
																"MySQL",
																"ODBC",
																"Oracle",
																"FME",
																"Custom connectionstring"});
			this.ConnectionType.Location = new System.Drawing.Point(128, 0);
			this.ConnectionType.Name = "ConnectionType";
			this.ConnectionType.Size = new System.Drawing.Size(440, 21);
			this.ConnectionType.TabIndex = 3;
			this.ConnectionType.SelectedIndexChanged += new System.EventHandler(this.ConnectionType_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Connection Type";
			// 
			// ProviderPanel
			// 
			this.ProviderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ProviderPanel.Controls.Add(this.fme);
			this.ProviderPanel.Controls.Add(this.informix);
			this.ProviderPanel.Controls.Add(this.ogdi);
			this.ProviderPanel.Controls.Add(this.grass);
			this.ProviderPanel.Controls.Add(this.arcSDE);
			this.ProviderPanel.Controls.Add(this.dods);
			this.ProviderPanel.Controls.Add(this.custom);
			this.ProviderPanel.Controls.Add(this.mySQL);
			this.ProviderPanel.Controls.Add(this.oracle);
			this.ProviderPanel.Controls.Add(this.odbc);
			this.ProviderPanel.Controls.Add(this.postGIS);
			this.ProviderPanel.Controls.Add(this.managed);
			this.ProviderPanel.Controls.Add(this.unmanaged);
			this.ProviderPanel.Location = new System.Drawing.Point(8, 48);
			this.ProviderPanel.Name = "ProviderPanel";
			this.ProviderPanel.Size = new System.Drawing.Size(560, 448);
			this.ProviderPanel.TabIndex = 4;
			// 
			// fme
			// 
			this.fme.Location = new System.Drawing.Point(144, 400);
			this.fme.Name = "fme";
			this.fme.Size = new System.Drawing.Size(144, 32);
			this.fme.TabIndex = 12;
			// 
			// informix
			// 
			this.informix.Location = new System.Drawing.Point(384, 296);
			this.informix.Name = "informix";
			this.informix.Size = new System.Drawing.Size(152, 80);
			this.informix.TabIndex = 11;
			// 
			// ogdi
			// 
			this.ogdi.Location = new System.Drawing.Point(384, 200);
			this.ogdi.Name = "ogdi";
			this.ogdi.Size = new System.Drawing.Size(152, 80);
			this.ogdi.TabIndex = 10;
			// 
			// grass
			// 
			this.grass.Location = new System.Drawing.Point(384, 104);
			this.grass.Name = "grass";
			this.grass.Size = new System.Drawing.Size(152, 80);
			this.grass.TabIndex = 9;
			// 
			// arcSDE
			// 
			this.arcSDE.AutoScroll = true;
			this.arcSDE.AutoScrollMinSize = new System.Drawing.Size(312, 296);
			this.arcSDE.Location = new System.Drawing.Point(384, 8);
			this.arcSDE.Name = "arcSDE";
			this.arcSDE.Size = new System.Drawing.Size(152, 80);
			this.arcSDE.TabIndex = 8;
			// 
			// dods
			// 
			this.dods.Location = new System.Drawing.Point(200, 296);
			this.dods.Name = "dods";
			this.dods.Size = new System.Drawing.Size(184, 80);
			this.dods.TabIndex = 7;
			// 
			// custom
			// 
			this.custom.Location = new System.Drawing.Point(16, 296);
			this.custom.Name = "custom";
			this.custom.Size = new System.Drawing.Size(176, 80);
			this.custom.TabIndex = 6;
			// 
			// mySQL
			// 
			this.mySQL.Location = new System.Drawing.Point(208, 200);
			this.mySQL.Name = "mySQL";
			this.mySQL.Size = new System.Drawing.Size(176, 88);
			this.mySQL.TabIndex = 5;
			// 
			// oracle
			// 
			this.oracle.Location = new System.Drawing.Point(24, 208);
			this.oracle.Name = "oracle";
			this.oracle.Size = new System.Drawing.Size(168, 80);
			this.oracle.TabIndex = 4;
			// 
			// odbc
			// 
			this.odbc.Location = new System.Drawing.Point(216, 112);
			this.odbc.Name = "odbc";
			this.odbc.Size = new System.Drawing.Size(168, 80);
			this.odbc.TabIndex = 3;
			// 
			// postGIS
			// 
			this.postGIS.Location = new System.Drawing.Point(24, 104);
			this.postGIS.Name = "postGIS";
			this.postGIS.Size = new System.Drawing.Size(176, 88);
			this.postGIS.TabIndex = 2;
			// 
			// managed
			// 
			this.managed.Location = new System.Drawing.Point(200, 8);
			this.managed.Name = "managed";
			this.managed.Size = new System.Drawing.Size(176, 88);
			this.managed.TabIndex = 1;
			// 
			// unmanaged
			// 
			this.unmanaged.Location = new System.Drawing.Point(16, 8);
			this.unmanaged.Name = "unmanaged";
			this.unmanaged.Size = new System.Drawing.Size(176, 80);
			this.unmanaged.TabIndex = 0;
			// 
			// WriteProtected
			// 
			this.WriteProtected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.WriteProtected.Location = new System.Drawing.Point(128, 24);
			this.WriteProtected.Name = "WriteProtected";
			this.WriteProtected.Size = new System.Drawing.Size(440, 16);
			this.WriteProtected.TabIndex = 5;
			this.WriteProtected.Text = "Data source is write protected";
			this.WriteProtected.CheckedChanged += new System.EventHandler(this.WriteProtected_CheckedChanged);
			// 
			// FeatureSourceEditorOGR
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(416, 264);
			this.Controls.Add(this.WriteProtected);
			this.Controls.Add(this.ProviderPanel);
			this.Controls.Add(this.ConnectionType);
			this.Controls.Add(this.label1);
			this.Name = "FeatureSourceEditorOGR";
			this.Size = new System.Drawing.Size(576, 504);
			this.ProviderPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ConnectionType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UserControl selected = null;
			switch((ProviderTypes)ConnectionType.SelectedIndex)
			{
				case ProviderTypes.Managed:
					selected = managed;
					break;
				case ProviderTypes.Unmanaged:
					selected = unmanaged;
					break;
				case ProviderTypes.DODS:
					selected = dods;
					break;
				case ProviderTypes.ArcSDE:
					selected = arcSDE;
					break;
				case ProviderTypes.GRASS:
					selected = grass;
					break;
				case ProviderTypes.OGDI:
					selected = ogdi;
					break;
				case ProviderTypes.Informix:
					selected = informix;
					break;
				case ProviderTypes.PostGIS:
					selected = postGIS;
					break;
				case ProviderTypes.MySQL:
					selected = mySQL;
					break;
				case ProviderTypes.ODBC:
					selected = odbc;
					break;
				case ProviderTypes.Oracle:
					selected = oracle;
					break;
				case ProviderTypes.FME:
					selected = fme;
					break;
				case ProviderTypes.Custom:
					selected = custom;
					break;
			}

			if (selected != null)
			{
				System.Reflection.MethodInfo mi = selected.GetType().GetMethod("UpdateDisplay");
				if (mi != null)
					mi.Invoke(selected, null);
			}

			foreach(UserControl c in ProviderPanel.Controls)
				c.Visible = c == selected;
		}

		public object Resource
		{
			get { return m_feature; }
			set 
			{
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
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
			return false;
		}

		private void WriteProtected_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_feature == null)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["ReadOnly"] = WriteProtected.Checked ? "TRUE" : "FALSE";
		}

        public bool Profile() { return true; }
        public bool ValidateResource() { return true; }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return false; } }
        public bool SupportsProfiling { get { return false; } }
    
    }
}
