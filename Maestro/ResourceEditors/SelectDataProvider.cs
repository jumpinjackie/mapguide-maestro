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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectDataProvider));
            this.ProviderList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProviderComments = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProviderList
            // 
            resources.ApplyResources(this.ProviderList, "ProviderList");
            this.ProviderList.Name = "ProviderList";
            this.ProviderList.SelectedIndexChanged += new System.EventHandler(this.ProviderList_SelectedIndexChanged);
            this.ProviderList.DoubleClick += new System.EventHandler(this.ProviderList_DoubleClick);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ProviderComments
            // 
            resources.ApplyResources(this.ProviderComments, "ProviderComments");
            this.ProviderComments.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ProviderComments.Name = "ProviderComments";
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SelectDataProvider
            // 
            this.AcceptButton = this.OKBtn;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.ProviderComments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProviderList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectDataProvider";
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
