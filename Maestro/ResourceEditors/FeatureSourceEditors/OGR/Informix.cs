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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Specialized;
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR
{
	/// <summary>
	/// Summary description for Informix.
	/// </summary>
	public class Informix : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox Table;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private EditorInterface m_editor;


		private string m_username;
		private string m_password;

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item)
		{
			m_editor = editor;
			m_item = item;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if (m_item == null)
				return;

			try
			{
				m_isUpdating = true;
				string connectionstring = m_item.Parameter["DataSource"];
				if (connectionstring == null)
					connectionstring = "";

				if (!connectionstring.StartsWith("IDB:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("IDB:".Length);

				while(connectionstring.IndexOf("  ") >= 0)
					connectionstring = connectionstring.Replace("  ", " ");
                				
				NameValueCollection items = FeatureSourceEditors.ODBC.ConnectionStringManager.SplitConnectionString(connectionstring, ' ');
				Server.Text = GetDefaultValue(items["server"], "localhost");
				Database.Text = GetDefaultValue(items["dbname"], "");
				m_username = GetDefaultValue(items["user"], "");
				m_password = GetDefaultValue(items["pass"], "");
				Table.Text = GetDefaultValue(items["table"], "");
				credentials.SetCredentials(m_username, m_password);
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void UpdateConnectionString()
		{
			if (m_item == null) 
				return;

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
	
			if (Database.Text.Trim().Length > 0)
			{
				sb.Append(" dbname=");
				sb.Append(Database.Text);
			}

			if (Server.Text.Trim().Length > 0)
			{
				sb.Append(" server=");
				sb.Append(Server.Text);
			}

			if (m_username != null && m_username.Trim().Length > 0)
			{
				sb.Append(" user=");
				sb.Append(m_username);
			}

			if (m_password != null && m_password.Trim().Length > 0)
			{
				sb.Append(" pass=");
				sb.Append(m_password);
			}

			if (Table.Text.Trim().Length > 0)
			{
				sb.Append(" table=");
				sb.Append(Table.Text);
			}

			m_item.Parameter["DataSource"] = "IDB:" + sb.ToString().Trim();
		}

		private string GetDefaultValue(string item, string defaultValue)
		{
			if (item != null && item.Trim().Length > 0)
				return item;
			else
				return defaultValue;
		}

		public Informix()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
			this.Table = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Database = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Server = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.credentials = new ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
			this.SuspendLayout();
			// 
			// Table
			// 
			this.Table.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Table.Location = new System.Drawing.Point(112, 72);
			this.Table.Name = "Table";
			this.Table.Size = new System.Drawing.Size(160, 20);
			this.Table.TabIndex = 17;
			this.Table.Text = "";
			this.Table.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Table";
			// 
			// Database
			// 
			this.Database.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Database.Location = new System.Drawing.Point(112, 40);
			this.Database.Name = "Database";
			this.Database.Size = new System.Drawing.Size(160, 20);
			this.Database.TabIndex = 15;
			this.Database.Text = "";
			this.Database.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Database";
			// 
			// Server
			// 
			this.Server.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Server.Location = new System.Drawing.Point(112, 8);
			this.Server.Name = "Server";
			this.Server.Size = new System.Drawing.Size(160, 20);
			this.Server.TabIndex = 13;
			this.Server.Text = "";
			this.Server.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 12;
			this.label1.Text = "Server";
			// 
			// credentials
			// 
			this.credentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.credentials.AutoScroll = true;
			this.credentials.AutoScrollMinSize = new System.Drawing.Size(264, 152);
			this.credentials.Location = new System.Drawing.Point(8, 104);
			this.credentials.Name = "credentials";
			this.credentials.Size = new System.Drawing.Size(264, 152);
			this.credentials.TabIndex = 18;
			this.credentials.CredentialsChanged += new ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
			// 
			// Informix
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(280, 264);
			this.Controls.Add(this.credentials);
			this.Controls.Add(this.Table);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Database);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Server);
			this.Controls.Add(this.label1);
			this.Name = "Informix";
			this.Size = new System.Drawing.Size(280, 264);
			this.ResumeLayout(false);

		}
		#endregion

		private void SomeProperty_Change(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_item == null)
				return;

			if (m_item.Parameter == null)
				m_item.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			UpdateConnectionString();
			m_editor.HasChanged();
		
		}

		private void credentials_CredentialsChanged(string username, string password)
		{
			if (m_isUpdating || m_item == null)
				return;

			m_username = username;
			m_password = password;

			SomeProperty_Change(null, null);
		}
	}
}
