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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.PostGIS
{
	/// <summary>
	/// Summary description for FeatureSourceEditorPostGIS.
	/// </summary>
	public class FeatureSourceEditorPostGIS : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		private System.ComponentModel.IContainer components;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Port;
		private System.Windows.Forms.Label label2;
		private bool m_isUpdating = false;
		private System.Windows.Forms.ToolTip toolTips;
        private TextBox Datastore;
        private Label label4;

		public FeatureSourceEditorPostGIS()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
        }

		public FeatureSourceEditorPostGIS(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;

            if (!m_editor.Existing && string.IsNullOrEmpty(m_feature.Parameter["DataStore"]))
                m_feature.Parameter["DataStore"] = "public";

			UpdateDisplay();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceEditorPostGIS));
            this.credentials = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
            this.Database = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.Datastore = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // credentials
            // 
            resources.ApplyResources(this.credentials, "credentials");
            this.credentials.Name = "credentials";
            this.credentials.CredentialsChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
            // 
            // Database
            // 
            resources.ApplyResources(this.Database, "Database");
            this.Database.Name = "Database";
            this.toolTips.SetToolTip(this.Database, resources.GetString("Database.ToolTip"));
            this.Database.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Server
            // 
            resources.ApplyResources(this.Server, "Server");
            this.Server.Name = "Server";
            this.toolTips.SetToolTip(this.Server, resources.GetString("Server.ToolTip"));
            this.Server.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Port
            // 
            resources.ApplyResources(this.Port, "Port");
            this.Port.Name = "Port";
            this.toolTips.SetToolTip(this.Port, resources.GetString("Port.ToolTip"));
            this.Port.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Datastore
            // 
            resources.ApplyResources(this.Datastore, "Datastore");
            this.Datastore.Name = "Datastore";
            this.toolTips.SetToolTip(this.Datastore, resources.GetString("Datastore.ToolTip"));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // FeatureSourceEditorPostGIS
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Datastore);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Database);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.credentials);
            this.Name = "FeatureSourceEditorPostGIS";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		public string ResourceId
		{
			get { return m_feature.ResourceId; }
			set { m_feature.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
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

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				
				if (m_feature == null || m_feature.Parameter == null)
				{
					this.Enabled = false;
					return;
				}

				this.Enabled = true;

				// Split service string of possible formats:
				// - database - use default localhost and port (DEF_PGPORT_STR)
				// - database@host - use default port
				// - database@host:port

				string service = m_feature.Parameter["Service"];
				if (service == null)
					service = "";

				string[] parts = service.Split('@', ':');
				Database.Text = parts[0];
				if (parts.Length > 1)
				{
					Server.Text = parts[1];
					if (parts.Length > 2)
						Port.Text = parts[2];
					else
						Port.Text = "";
				}
				else
				{
					Server.Text = "";
					Port.Text = "";
				}

                Datastore.Text = m_feature.Parameter["DataStore"];
				credentials.SetCredentials(m_feature.Parameter["Username"], m_feature.Parameter["Password"]);
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public bool Save(string savename)
		{
			return false;
		}

		private void SomeProperty_Changed(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;
	
			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			// Split service string of possible formats:
			// - database - use default localhost and port (DEF_PGPORT_STR)
			// - database@host - use default port
			// - database@host:port

			string service = Database.Text;
			if (Server.Text.Trim().Length != 0)
			{
				service += "@" + Server.Text;
				if (Port.Text.Trim().Length != 0)
					service += ":" + Port.Text;
			}

			m_feature.Parameter["Service"] = service;
            m_feature.Parameter["DataStore"] = Datastore.Text;
			m_editor.HasChanged();
		
		}

		private void credentials_CredentialsChanged(string username, string password)
		{
			if (m_feature == null || m_isUpdating)
				return;
	
			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["Username"] = username;
			m_feature.Parameter["Password"] = password;
			m_editor.HasChanged();
		
		}

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return m_editor.CurrentConnection.SupportsResourcePreviews; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    }
}
