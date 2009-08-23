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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for SelectDataProvider.
	/// </summary>
	public class SelectDataProvider : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox ProviderList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label ProviderComments;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ArrayList m_providers;
		private OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider m_selected;
		private Globalizator.Globalizator m_globalizor = null;


		public SelectDataProvider(OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection)
			: this()
		{
			m_providers = new ArrayList();
			ProviderList.Items.Clear();

			foreach(OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider provider in connection.FeatureProviders)
			{
				m_providers.Add(provider);
				ProviderList.Items.Add(provider.DisplayName);
			}
		}

		public OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider SelectedProvider
		{
			get {return m_selected;}
		}

		protected SelectDataProvider()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			m_globalizor = new  Globalizator.Globalizator(this);
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
            this.ProviderList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProviderComments = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProviderList
            // 
            this.ProviderList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProviderList.Location = new System.Drawing.Point(16, 32);
            this.ProviderList.Name = "ProviderList";
            this.ProviderList.Size = new System.Drawing.Size(232, 212);
            this.ProviderList.TabIndex = 0;
            this.ProviderList.SelectedIndexChanged += new System.EventHandler(this.ProviderList_SelectedIndexChanged);
            this.ProviderList.DoubleClick += new System.EventHandler(this.ProviderList_DoubleClick);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Providers on server:";
            // 
            // ProviderComments
            // 
            this.ProviderComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProviderComments.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ProviderComments.Location = new System.Drawing.Point(16, 248);
            this.ProviderComments.Name = "ProviderComments";
            this.ProviderComments.Size = new System.Drawing.Size(232, 40);
            this.ProviderComments.TabIndex = 2;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKBtn.Enabled = false;
            this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.OKBtn.Location = new System.Drawing.Point(40, 304);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(80, 32);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CancelBtn.Location = new System.Drawing.Point(144, 304);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(80, 32);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SelectDataProvider
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(266, 352);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.ProviderComments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProviderList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectDataProvider";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Data Provider";
            this.ResumeLayout(false);

		}
		#endregion

		private void CancelButton_Click(object sender, System.EventArgs e)
		{
			m_selected = null;
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void ProviderList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			OKBtn.Enabled = (ProviderList.SelectedIndex >= 0);
			if (OKBtn.Enabled)
				ProviderComments.Text = ((OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider)m_providers[ProviderList.SelectedIndex]).Description;
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			m_selected = (OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider)m_providers[ProviderList.SelectedIndex];
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void ProviderList_DoubleClick(object sender, System.EventArgs e)
		{
			if (OKBtn.Enabled)
				OKBtn.PerformClick();
		}
	}
}
