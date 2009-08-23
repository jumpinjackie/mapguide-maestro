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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR
{
	/// <summary>
	/// Summary description for OGDI.
	/// </summary>
	public class OGDI : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Layername;
		private System.Windows.Forms.TextBox Dataset;
		private System.Windows.Forms.TextBox Family;
		private System.Windows.Forms.TextBox Driver;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
		private EditorInterface m_editor;

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

				if (!connectionstring.StartsWith("gltp:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("gltp:".Length);

				if (connectionstring.IndexOf(":") > 0)
				{
					string items = connectionstring.Substring(0, connectionstring.IndexOf(":"));
					connectionstring = connectionstring.Substring(items.Length + 1);
					if (items.StartsWith("//"))
						items = items.Substring(2);
					string[] parts = items.Split('/');
					Server.Text = GetDefaultValue(parts, 0, "localhost");
					Driver.Text = GetDefaultValue(parts, 1, "");
					Dataset.Text = GetDefaultValue(parts, 2, "");
						
				}
				else
				{
					Server.Text = "";
					Driver.Text = "";
					Dataset.Text = "";
				}

				string[] parts2 = connectionstring.Split(':');
				Layername.Text = GetDefaultValue(parts2, 0, "");
				Family.Text = GetDefaultValue(parts2, 1, "");
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
			sb.Append("gltp:");
			if (Server.Text.Trim().Length > 0)
			{
				sb.Append("//");
				sb.Append(Server.Text);
			}
			sb.Append("/");
			sb.Append(Driver.Text);
			sb.Append("/");
			sb.Append(Dataset.Text);
			sb.Append(":");
			sb.Append(Layername.Text);
			sb.Append(":");
			sb.Append(Family.Text);

			m_item.Parameter["DataSource"] = sb.ToString();
		}

		private string GetDefaultValue(string[] items, int index, string defaultValue)
		{
			if (items.Length > index && items[index].Trim().Length > 0)
				return items[index];
			else
				return defaultValue;
		}


		public OGDI()
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
			this.Layername = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.Dataset = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.Family = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Driver = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Server = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Layername
			// 
			this.Layername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Layername.Location = new System.Drawing.Point(112, 104);
			this.Layername.Name = "Layername";
			this.Layername.Size = new System.Drawing.Size(144, 20);
			this.Layername.TabIndex = 31;
			this.Layername.Text = "";
			this.Layername.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 104);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 16);
			this.label5.TabIndex = 30;
			this.label5.Text = "Layername";
			// 
			// Dataset
			// 
			this.Dataset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Dataset.Location = new System.Drawing.Point(112, 72);
			this.Dataset.Name = "Dataset";
			this.Dataset.Size = new System.Drawing.Size(144, 20);
			this.Dataset.TabIndex = 29;
			this.Dataset.Text = "";
			this.Dataset.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 28;
			this.label4.Text = "Dataset";
			// 
			// Family
			// 
			this.Family.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Family.Location = new System.Drawing.Point(112, 136);
			this.Family.Name = "Family";
			this.Family.Size = new System.Drawing.Size(144, 20);
			this.Family.TabIndex = 27;
			this.Family.Text = "";
			this.Family.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 26;
			this.label3.Text = "Family";
			// 
			// Driver
			// 
			this.Driver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Driver.Location = new System.Drawing.Point(112, 40);
			this.Driver.Name = "Driver";
			this.Driver.Size = new System.Drawing.Size(144, 20);
			this.Driver.TabIndex = 25;
			this.Driver.Text = "";
			this.Driver.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 24;
			this.label2.Text = "Driver";
			// 
			// Server
			// 
			this.Server.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Server.Location = new System.Drawing.Point(112, 8);
			this.Server.Name = "Server";
			this.Server.Size = new System.Drawing.Size(144, 20);
			this.Server.TabIndex = 23;
			this.Server.Text = "";
			this.Server.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 22;
			this.label1.Text = "Server";
			// 
			// OGDI
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(264, 168);
			this.Controls.Add(this.Layername);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.Dataset);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Family);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Driver);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Server);
			this.Controls.Add(this.label1);
			this.Name = "OGDI";
			this.Size = new System.Drawing.Size(264, 168);
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
	}
}
