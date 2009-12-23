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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR
{
	/// <summary>
	/// Summary description for FeatureSourceEditorOGR.
	/// </summary>
	public class FeatureSourceEditorOGR : System.Windows.Forms.UserControl, IResourceEditorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceEditorOGR));
            this.ConnectionType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProviderPanel = new System.Windows.Forms.Panel();
            this.fme = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.FME();
            this.informix = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.Informix();
            this.ogdi = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.OGDI();
            this.grass = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.Grass();
            this.arcSDE = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.ArcSDE();
            this.dods = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.DODS();
            this.custom = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.Custom();
            this.mySQL = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.MySQL();
            this.oracle = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.Oracle();
            this.odbc = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.ODBC();
            this.postGIS = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.PostGIS();
            this.managed = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.Managed();
            this.unmanaged = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR.Unmanaged();
            this.WriteProtected = new System.Windows.Forms.CheckBox();
            this.ProviderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectionType
            // 
            resources.ApplyResources(this.ConnectionType, "ConnectionType");
            this.ConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConnectionType.Items.AddRange(new object[] {
            resources.GetString("ConnectionType.Items"),
            resources.GetString("ConnectionType.Items1"),
            resources.GetString("ConnectionType.Items2"),
            resources.GetString("ConnectionType.Items3"),
            resources.GetString("ConnectionType.Items4"),
            resources.GetString("ConnectionType.Items5"),
            resources.GetString("ConnectionType.Items6"),
            resources.GetString("ConnectionType.Items7"),
            resources.GetString("ConnectionType.Items8"),
            resources.GetString("ConnectionType.Items9"),
            resources.GetString("ConnectionType.Items10"),
            resources.GetString("ConnectionType.Items11"),
            resources.GetString("ConnectionType.Items12")});
            this.ConnectionType.Name = "ConnectionType";
            this.ConnectionType.SelectedIndexChanged += new System.EventHandler(this.ConnectionType_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ProviderPanel
            // 
            resources.ApplyResources(this.ProviderPanel, "ProviderPanel");
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
            this.ProviderPanel.Name = "ProviderPanel";
            // 
            // fme
            // 
            resources.ApplyResources(this.fme, "fme");
            this.fme.Name = "fme";
            // 
            // informix
            // 
            resources.ApplyResources(this.informix, "informix");
            this.informix.Name = "informix";
            // 
            // ogdi
            // 
            resources.ApplyResources(this.ogdi, "ogdi");
            this.ogdi.Name = "ogdi";
            // 
            // grass
            // 
            resources.ApplyResources(this.grass, "grass");
            this.grass.Name = "grass";
            // 
            // arcSDE
            // 
            resources.ApplyResources(this.arcSDE, "arcSDE");
            this.arcSDE.Name = "arcSDE";
            // 
            // dods
            // 
            resources.ApplyResources(this.dods, "dods");
            this.dods.Name = "dods";
            // 
            // custom
            // 
            resources.ApplyResources(this.custom, "custom");
            this.custom.Name = "custom";
            // 
            // mySQL
            // 
            resources.ApplyResources(this.mySQL, "mySQL");
            this.mySQL.Name = "mySQL";
            // 
            // oracle
            // 
            resources.ApplyResources(this.oracle, "oracle");
            this.oracle.Name = "oracle";
            // 
            // odbc
            // 
            resources.ApplyResources(this.odbc, "odbc");
            this.odbc.Name = "odbc";
            // 
            // postGIS
            // 
            resources.ApplyResources(this.postGIS, "postGIS");
            this.postGIS.Name = "postGIS";
            // 
            // managed
            // 
            resources.ApplyResources(this.managed, "managed");
            this.managed.Name = "managed";
            // 
            // unmanaged
            // 
            resources.ApplyResources(this.unmanaged, "unmanaged");
            this.unmanaged.Name = "unmanaged";
            // 
            // WriteProtected
            // 
            resources.ApplyResources(this.WriteProtected, "WriteProtected");
            this.WriteProtected.Name = "WriteProtected";
            this.WriteProtected.CheckedChanged += new System.EventHandler(this.WriteProtected_CheckedChanged);
            // 
            // FeatureSourceEditorOGR
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.WriteProtected);
            this.Controls.Add(this.ProviderPanel);
            this.Controls.Add(this.ConnectionType);
            this.Controls.Add(this.label1);
            this.Name = "FeatureSourceEditorOGR";
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
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    
    }
}
