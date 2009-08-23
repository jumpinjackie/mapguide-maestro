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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for TestForm.
	/// </summary>
	public class TestForm : System.Windows.Forms.Form
	{
		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_con = null;

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox textBox4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(24, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(96, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "Administrator";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(24, 56);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(96, 20);
			this.textBox2.TabIndex = 1;
			this.textBox2.Text = "admin";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(136, 32);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(144, 40);
			this.button1.TabIndex = 2;
			this.button1.Text = "Log in";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(24, 8);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(256, 20);
			this.textBox3.TabIndex = 3;
			this.textBox3.Text = "http://localhost/mapguide/mapagent/mapagent.fcgi";
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(24, 112);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(240, 121);
			this.listBox1.TabIndex = 4;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(144, 80);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(120, 32);
			this.button2.TabIndex = 5;
			this.button2.Text = "Get Feature Providers";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(24, 264);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(112, 32);
			this.button3.TabIndex = 6;
			this.button3.Text = "Get map";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(24, 240);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(240, 20);
			this.textBox4.TabIndex = 7;
			this.textBox4.Text = "Allerod importeret/Map1";
			// 
			// TestForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 398);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			m_con = new OSGeo.MapGuide.MaestroAPI.HttpServerConnection(new Uri(textBox3.Text), textBox1.Text, textBox2.Text, "da", true);
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if (m_con == null)
			{
				MessageBox.Show("Not connected");
				return;
			}

			listBox1.Items.Clear();
			foreach(OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider fp in m_con.FeatureProviders)
				listBox1.Items.Add(fp.DisplayName);
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			string resid = new MaestroAPI.ResourceIdentifier(textBox4.Text, OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition);
			OSGeo.MapGuide.MaestroAPI.MapDefinition m = m_con.GetMapDefinition(resid);

			listBox1.Items.Clear();
			foreach(OSGeo.MapGuide.MaestroAPI.MapLayerType l in m.Layers)
				listBox1.Items.Add(l.LegendLabel);
		}
	}
}
