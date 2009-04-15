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
	/// Summary description for FeatureSourceEditorMySQL.
	/// </summary>
	public class FeatureSourceEditorMySQL : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		private System.ComponentModel.IContainer components;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Server;
        private System.Windows.Forms.Label label1;
		private bool m_isUpdating = false;
		private System.Windows.Forms.ToolTip toolTips;
		private Globalizator.Globalizator m_globalizor = null;

		public FeatureSourceEditorMySQL()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			m_globalizor = new Globalizator.Globalizator(this);

			toolTips.SetToolTip(Server, m_globalizor.Translate("Enter server name here, or leave blank for the default, which is localhost"));
			toolTips.SetToolTip(Database, m_globalizor.Translate("Enter the database name here"));
		}

		public FeatureSourceEditorMySQL(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;

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
            this.credentials = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
            this.Database = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // credentials
            // 
            this.credentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.credentials.AutoScroll = true;
            this.credentials.AutoScrollMinSize = new System.Drawing.Size(264, 152);
            this.credentials.Location = new System.Drawing.Point(8, 72);
            this.credentials.Name = "credentials";
            this.credentials.Size = new System.Drawing.Size(304, 152);
            this.credentials.TabIndex = 0;
            this.credentials.CredentialsChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
            // 
            // Database
            // 
            this.Database.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Database.Location = new System.Drawing.Point(112, 40);
            this.Database.Name = "Database";
            this.Database.Size = new System.Drawing.Size(200, 20);
            this.Database.TabIndex = 31;
            this.Database.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "Database";
            // 
            // Server
            // 
            this.Server.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Server.Location = new System.Drawing.Point(112, 8);
            this.Server.Name = "Server";
            this.Server.Size = new System.Drawing.Size(200, 20);
            this.Server.TabIndex = 29;
            this.Server.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "Server";
            // 
            // FeatureSourceEditorMySQL
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(320, 232);
            this.Controls.Add(this.Database);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.credentials);
            this.Name = "FeatureSourceEditorMySQL";
            this.Size = new System.Drawing.Size(320, 232);
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
                Server.Text = m_feature.Parameter["Service"];
                Database.Text = m_feature.Parameter["DataStore"];

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

			m_feature.Parameter["Service"] = Server.Text;
            m_feature.Parameter["DataStore"] = Database.Text;
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
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    }
}
