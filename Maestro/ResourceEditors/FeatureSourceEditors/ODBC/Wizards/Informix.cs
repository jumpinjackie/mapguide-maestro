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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Informix));
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
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // Host
            // 
            resources.ApplyResources(this.Host, "Host");
            this.Host.Name = "Host";
            this.Host.TextChanged += new System.EventHandler(this.PropertyText_Changed);
            // 
            // Server
            // 
            resources.ApplyResources(this.Server, "Server");
            this.Server.Name = "Server";
            this.Server.TextChanged += new System.EventHandler(this.PropertyText_Changed);
            // 
            // Service
            // 
            resources.ApplyResources(this.Service, "Service");
            this.Service.Name = "Service";
            this.Service.TextChanged += new System.EventHandler(this.PropertyText_Changed);
            // 
            // Database
            // 
            resources.ApplyResources(this.Database, "Database");
            this.Database.Name = "Database";
            this.Database.TextChanged += new System.EventHandler(this.PropertyText_Changed);
            // 
            // Protocol
            // 
            resources.ApplyResources(this.Protocol, "Protocol");
            this.Protocol.Items.AddRange(new object[] {
            resources.GetString("Protocol.Items")});
            this.Protocol.Name = "Protocol";
            this.Protocol.TextChanged += new System.EventHandler(this.PropertyText_Changed);
            // 
            // Informix
            // 
            resources.ApplyResources(this, "$this");
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
            this.ResumeLayout(false);
            this.PerformLayout();

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
