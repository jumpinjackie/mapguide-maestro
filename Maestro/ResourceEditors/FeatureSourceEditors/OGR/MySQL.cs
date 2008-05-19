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
	/// Summary description for MySQL.
	/// </summary>
	public class MySQL : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox Tables;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Port;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label7;
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

				if (!connectionstring.StartsWith("MYSQL:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("MYSQL:".Length);

				if (connectionstring.IndexOf(",") > 0)
				{
					Database.Text = connectionstring.Substring(0, connectionstring.IndexOf(","));
					connectionstring = connectionstring.Substring(Database.Text.Length + 1);
				}
				else
					Database.Text = "";
                				
				NameValueCollection items = FeatureSourceEditors.ODBC.ConnectionStringManager.SplitConnectionString(connectionstring, ',');
				Server.Text = GetDefaultValue(items["host"], "");
				m_username = GetDefaultValue(items["user"], "");
				m_password = GetDefaultValue(items["password"], "");
				Port.Text = GetDefaultValue(items["port"], "3306");
				Tables.Text = GetDefaultValue(items["tables"], "");

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
			sb.Append("MYSQL:");
			sb.Append(Database.Text);
			sb.Append(",user=");
			sb.Append(m_username);
			sb.Append(",password=");
			sb.Append(m_password);
			sb.Append(",host=");
			sb.Append(Server.Text);

			if (Port.Text.Trim().Length > 0)
			{
				sb.Append(",port=");
				sb.Append(Port.Text);
			}

			if (Tables.Text.Trim().Length > 0)
			{
				sb.Append(",tables=");
				sb.Append(Tables.Text);
			}

			m_item.Parameter["DataSource"] = sb.ToString();
		}

		private string GetDefaultValue(string item, string defaultValue)
		{
			if (item != null && item.Trim().Length > 0)
				return item;
			else
				return defaultValue;
		}

		public MySQL()
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
			this.Tables = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.Database = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Port = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Server = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.credentials = new ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
			this.SuspendLayout();
			// 
			// Tables
			// 
			this.Tables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Tables.Location = new System.Drawing.Point(112, 104);
			this.Tables.Name = "Tables";
			this.Tables.Size = new System.Drawing.Size(192, 20);
			this.Tables.TabIndex = 23;
			this.Tables.Text = "";
			this.Tables.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 104);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(96, 16);
			this.label6.TabIndex = 22;
			this.label6.Text = "Tables";
			// 
			// Database
			// 
			this.Database.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Database.Location = new System.Drawing.Point(112, 72);
			this.Database.Name = "Database";
			this.Database.Size = new System.Drawing.Size(192, 20);
			this.Database.TabIndex = 17;
			this.Database.Text = "";
			this.Database.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Database";
			// 
			// Port
			// 
			this.Port.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Port.Location = new System.Drawing.Point(112, 40);
			this.Port.Name = "Port";
			this.Port.Size = new System.Drawing.Size(192, 20);
			this.Port.TabIndex = 15;
			this.Port.Text = "";
			this.Port.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Port";
			// 
			// Server
			// 
			this.Server.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Server.Location = new System.Drawing.Point(112, 8);
			this.Server.Name = "Server";
			this.Server.Size = new System.Drawing.Size(192, 20);
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
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(112, 128);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(192, 56);
			this.label7.TabIndex = 24;
			this.label7.Text = "Tables are optional, but will speed up initial connection. Enter a list of table " +
				"names, seperated by semicolons. Ea.: table1;table2";
			// 
			// credentials
			// 
			this.credentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.credentials.AutoScroll = true;
			this.credentials.AutoScrollMinSize = new System.Drawing.Size(264, 152);
			this.credentials.Location = new System.Drawing.Point(8, 192);
			this.credentials.Name = "credentials";
			this.credentials.Size = new System.Drawing.Size(296, 152);
			this.credentials.TabIndex = 25;
			this.credentials.CredentialsChanged += new ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
			// 
			// MySQL
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(312, 352);
			this.Controls.Add(this.credentials);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.Tables);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.Database);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Port);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Server);
			this.Controls.Add(this.label1);
			this.Name = "MySQL";
			this.Size = new System.Drawing.Size(312, 352);
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
