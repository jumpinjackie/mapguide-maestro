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
using System.Collections.Specialized;
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Wizards
{
	/// <summary>
	/// Summary description for Informix.
	/// </summary>
	public class Informix : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox Host;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.TextBox Service;
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.ComboBox Protocol;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ResourceEditors.EditorInterface m_editor = null;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item = null;
		private bool m_isUpdating = false;
		public event FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate ConnectionStringUpdated;

		public Informix()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

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
				NameValueCollection nv = ConnectionStringManager.SplitConnectionString(m_item.Parameter["ConnectionString"]);
				ConnectionStringManager.InsertDefaultValues(nv, "{INFORMIX 3.30 32 BIT}");
				Host.Text = nv["Host"];
				Server.Text = nv["Server"];
				Service.Text = nv["Service"];
				Protocol.Text = nv["Protocol"];
				Database.Text = nv["Database"];
			}
			finally
			{
				m_isUpdating = false;
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.Host = new System.Windows.Forms.TextBox();
			this.Server = new System.Windows.Forms.TextBox();
			this.Service = new System.Windows.Forms.TextBox();
			this.Database = new System.Windows.Forms.TextBox();
			this.Protocol = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Host";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Server";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Service";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Protocol";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 104);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Database";
			// 
			// Host
			// 
			this.Host.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Host.Location = new System.Drawing.Point(152, 8);
			this.Host.Name = "Host";
			this.Host.Size = new System.Drawing.Size(200, 20);
			this.Host.TabIndex = 5;
			this.Host.Text = "textBox1";
			this.Host.TextChanged += new System.EventHandler(this.PropertyText_Changed);
			// 
			// Server
			// 
			this.Server.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Server.Location = new System.Drawing.Point(152, 32);
			this.Server.Name = "Server";
			this.Server.Size = new System.Drawing.Size(200, 20);
			this.Server.TabIndex = 6;
			this.Server.Text = "textBox2";
			this.Server.TextChanged += new System.EventHandler(this.PropertyText_Changed);
			// 
			// Service
			// 
			this.Service.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Service.Location = new System.Drawing.Point(152, 56);
			this.Service.Name = "Service";
			this.Service.Size = new System.Drawing.Size(200, 20);
			this.Service.TabIndex = 7;
			this.Service.Text = "textBox3";
			this.Service.TextChanged += new System.EventHandler(this.PropertyText_Changed);
			// 
			// Database
			// 
			this.Database.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Database.Location = new System.Drawing.Point(152, 104);
			this.Database.Name = "Database";
			this.Database.Size = new System.Drawing.Size(200, 20);
			this.Database.TabIndex = 9;
			this.Database.Text = "textBox5";
			this.Database.TextChanged += new System.EventHandler(this.PropertyText_Changed);
			// 
			// Protocol
			// 
			this.Protocol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Protocol.Items.AddRange(new object[] {
														  "olsoctcp"});
			this.Protocol.Location = new System.Drawing.Point(152, 80);
			this.Protocol.Name = "Protocol";
			this.Protocol.Size = new System.Drawing.Size(200, 21);
			this.Protocol.TabIndex = 10;
			this.Protocol.Text = "olsoctcp";
			this.Protocol.TextChanged += new System.EventHandler(this.PropertyText_Changed);
			// 
			// Informix
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(360, 128);
			this.Controls.Add(this.Protocol);
			this.Controls.Add(this.Database);
			this.Controls.Add(this.Service);
			this.Controls.Add(this.Server);
			this.Controls.Add(this.Host);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Informix";
			this.Size = new System.Drawing.Size(360, 128);
			this.ResumeLayout(false);

		}
		#endregion

		private void PropertyText_Changed(object sender, System.EventArgs e)
		{
			if (m_item == null || m_isUpdating)
				return;

			NameValueCollection prev = ConnectionStringManager.SplitConnectionString(m_item.Parameter["ConnectionString"]);
			NameValueCollection nv = new NameValueCollection();
			nv["Dsn"] = "''";
			nv["Driver"] = "{INFORMIX 3.30 32 BIT}";
			nv["Host"] = Host.Text;
			nv["Server"] = Server.Text;
			nv["Service"] = Service.Text;
			nv["Protocol"] = Protocol.Text;
			nv["Database"] = Database.Text;
			ConnectionStringManager.MergeCredentialsIntoConnectionString(prev, nv);

			m_item.Parameter["ConnectionString"] = ConnectionStringManager.JoinConnectionString(nv);

			if (ConnectionStringUpdated != null)
				ConnectionStringUpdated(m_item.Parameter["ConnectionString"]);

			m_editor.HasChanged();
		}
	}
}
