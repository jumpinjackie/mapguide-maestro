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
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors
{
	public class Legend : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.TextBox RootFolderIcon;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox DisabledLayerIcon;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox LayerRasterIcon;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox LayerThemeIcon;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox GroupInfoIcon;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox LayerInfoIcon;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox HideInvisibleLayers;
		private System.Windows.Forms.CheckBox ShowRootFolder;
		private System.ComponentModel.IContainer components = null;

		public Legend()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public override void SetItem(WidgetType w)
		{
			try
			{
				m_isUpdating = true;
				m_w = w;
				this.Enabled = m_w != null;

				try { HideInvisibleLayers.Checked = bool.Parse(GetSettingValue("HideInvisibleLayers")); }
				catch { }
				try { ShowRootFolder.Checked = bool.Parse(GetSettingValue("ShowRootFolder")); }
				catch { }

				DisabledLayerIcon.Text = GetSettingValue("DisabledLayerIcon");
				RootFolderIcon.Text = GetSettingValue("RootFolderIcon");
				LayerThemeIcon.Text = GetSettingValue("LayerThemeIcon");
				LayerRasterIcon.Text = GetSettingValue("LayerRasterIcon");
				LayerInfoIcon.Text = GetSettingValue("LayerInfoIcon");
				GroupInfoIcon.Text = GetSettingValue("GroupInfoIcon");
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.RootFolderIcon = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.DisabledLayerIcon = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.LayerRasterIcon = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.LayerThemeIcon = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.GroupInfoIcon = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.LayerInfoIcon = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.HideInvisibleLayers = new System.Windows.Forms.CheckBox();
			this.ShowRootFolder = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// RootFolderIcon
			// 
			this.RootFolderIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RootFolderIcon.Location = new System.Drawing.Point(144, 80);
			this.RootFolderIcon.Name = "RootFolderIcon";
			this.RootFolderIcon.Size = new System.Drawing.Size(464, 20);
			this.RootFolderIcon.TabIndex = 11;
			this.RootFolderIcon.Text = "";
			this.RootFolderIcon.TextChanged += new System.EventHandler(this.RootFolderIcon_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Root folder icon";
			// 
			// DisabledLayerIcon
			// 
			this.DisabledLayerIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.DisabledLayerIcon.Location = new System.Drawing.Point(144, 56);
			this.DisabledLayerIcon.Name = "DisabledLayerIcon";
			this.DisabledLayerIcon.Size = new System.Drawing.Size(464, 20);
			this.DisabledLayerIcon.TabIndex = 9;
			this.DisabledLayerIcon.Text = "";
			this.DisabledLayerIcon.TextChanged += new System.EventHandler(this.DisabledLayerIcon_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Disabled layer icon";
			// 
			// LayerRasterIcon
			// 
			this.LayerRasterIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LayerRasterIcon.Location = new System.Drawing.Point(144, 128);
			this.LayerRasterIcon.Name = "LayerRasterIcon";
			this.LayerRasterIcon.Size = new System.Drawing.Size(464, 20);
			this.LayerRasterIcon.TabIndex = 15;
			this.LayerRasterIcon.Text = "";
			this.LayerRasterIcon.TextChanged += new System.EventHandler(this.LayerRasterIcon_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 128);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 16);
			this.label3.TabIndex = 14;
			this.label3.Text = "Raster layer icon";
			// 
			// LayerThemeIcon
			// 
			this.LayerThemeIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LayerThemeIcon.Location = new System.Drawing.Point(144, 104);
			this.LayerThemeIcon.Name = "LayerThemeIcon";
			this.LayerThemeIcon.Size = new System.Drawing.Size(464, 20);
			this.LayerThemeIcon.TabIndex = 13;
			this.LayerThemeIcon.Text = "";
			this.LayerThemeIcon.TextChanged += new System.EventHandler(this.LayerThemeIcon_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Themed layer icon";
			// 
			// GroupInfoIcon
			// 
			this.GroupInfoIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.GroupInfoIcon.Location = new System.Drawing.Point(144, 176);
			this.GroupInfoIcon.Name = "GroupInfoIcon";
			this.GroupInfoIcon.Size = new System.Drawing.Size(464, 20);
			this.GroupInfoIcon.TabIndex = 19;
			this.GroupInfoIcon.Text = "";
			this.GroupInfoIcon.TextChanged += new System.EventHandler(this.GroupInfoIcon_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 176);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 16);
			this.label5.TabIndex = 18;
			this.label5.Text = "Group info icon";
			// 
			// LayerInfoIcon
			// 
			this.LayerInfoIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LayerInfoIcon.Location = new System.Drawing.Point(144, 152);
			this.LayerInfoIcon.Name = "LayerInfoIcon";
			this.LayerInfoIcon.Size = new System.Drawing.Size(464, 20);
			this.LayerInfoIcon.TabIndex = 17;
			this.LayerInfoIcon.Text = "";
			this.LayerInfoIcon.TextChanged += new System.EventHandler(this.LayerInfoIcon_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 152);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(136, 16);
			this.label6.TabIndex = 16;
			this.label6.Text = "Layer info icon";
			// 
			// HideInvisibleLayers
			// 
			this.HideInvisibleLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.HideInvisibleLayers.Location = new System.Drawing.Point(8, 8);
			this.HideInvisibleLayers.Name = "HideInvisibleLayers";
			this.HideInvisibleLayers.Size = new System.Drawing.Size(600, 16);
			this.HideInvisibleLayers.TabIndex = 20;
			this.HideInvisibleLayers.Text = "Hide invisible layers";
			this.HideInvisibleLayers.CheckedChanged += new System.EventHandler(this.HideInvisibleLayers_CheckedChanged);
			// 
			// ShowRootFolder
			// 
			this.ShowRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ShowRootFolder.Location = new System.Drawing.Point(8, 32);
			this.ShowRootFolder.Name = "ShowRootFolder";
			this.ShowRootFolder.Size = new System.Drawing.Size(600, 16);
			this.ShowRootFolder.TabIndex = 21;
			this.ShowRootFolder.Text = "Show Root Folder";
			this.ShowRootFolder.CheckedChanged += new System.EventHandler(this.ShowRootFolder_CheckedChanged);
			// 
			// Legend
			// 
			this.Controls.Add(this.ShowRootFolder);
			this.Controls.Add(this.HideInvisibleLayers);
			this.Controls.Add(this.GroupInfoIcon);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.LayerInfoIcon);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.LayerRasterIcon);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.LayerThemeIcon);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.RootFolderIcon);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.DisabledLayerIcon);
			this.Controls.Add(this.label1);
			this.Name = "Legend";
			this.Size = new System.Drawing.Size(616, 208);
			this.ResumeLayout(false);

		}
		#endregion

		private void HideInvisibleLayers_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("HideInvisibleLayers", HideInvisibleLayers.Checked.ToString().ToLower());
		}

		private void ShowRootFolder_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("ShowRootFolder", ShowRootFolder.Checked.ToString().ToLower());
		}

		private void DisabledLayerIcon_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("DisabledLayerIcon", DisabledLayerIcon.Text);
		}

		private void RootFolderIcon_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("RootFolderIcon", RootFolderIcon.Text);
		}

		private void LayerThemeIcon_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("LayerThemeIcon", LayerThemeIcon.Text);
		}

		private void LayerRasterIcon_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("LayerRasterIcon", LayerRasterIcon.Text);
		}

		private void LayerInfoIcon_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("LayerInfoIcon", LayerInfoIcon.Text);
		}

		private void GroupInfoIcon_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("GroupInfoIcon", GroupInfoIcon.Text);
		}
	}
}

