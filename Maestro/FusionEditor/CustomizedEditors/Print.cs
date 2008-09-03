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
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors
{
	public class Print : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox PageTitle;
		private System.Windows.Forms.CheckBox ShowNorthArrow;
		private System.Windows.Forms.CheckBox ShowLegend;
		private System.Windows.Forms.CheckBox ShowTitle;
		private System.Windows.Forms.CheckBox ShowPrintUI;
		private System.ComponentModel.IContainer components = null;

		public Print()
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

				PageTitle.Text = GetSettingValue("PageTitle"); 
				try { ShowLegend.Checked = bool.Parse(GetSettingValue("ShowLegend")); } 
				catch {}
				try { ShowNorthArrow.Checked = bool.Parse(GetSettingValue("ShowNorthArrow")); } 
				catch {}
				try { ShowPrintUI.Checked = bool.Parse(GetSettingValue("ShowPrintUI")); } 
				catch {}
				try { ShowTitle.Checked = bool.Parse(GetSettingValue("ShowTitle")); } 
				catch {}
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
			this.PageTitle = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ShowNorthArrow = new System.Windows.Forms.CheckBox();
			this.ShowLegend = new System.Windows.Forms.CheckBox();
			this.ShowTitle = new System.Windows.Forms.CheckBox();
			this.ShowPrintUI = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// PageTitle
			// 
			this.PageTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.PageTitle.Location = new System.Drawing.Point(136, 10);
			this.PageTitle.Name = "PageTitle";
			this.PageTitle.Size = new System.Drawing.Size(472, 20);
			this.PageTitle.TabIndex = 23;
			this.PageTitle.Text = "";
			this.PageTitle.TextChanged += new System.EventHandler(this.PageTitle_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 10);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 16);
			this.label5.TabIndex = 22;
			this.label5.Text = "Page title";
			// 
			// ShowNorthArrow
			// 
			this.ShowNorthArrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ShowNorthArrow.Location = new System.Drawing.Point(8, 56);
			this.ShowNorthArrow.Name = "ShowNorthArrow";
			this.ShowNorthArrow.Size = new System.Drawing.Size(600, 16);
			this.ShowNorthArrow.TabIndex = 25;
			this.ShowNorthArrow.Text = "Show Root Folder";
			this.ShowNorthArrow.CheckedChanged += new System.EventHandler(this.ShowNorthArrow_CheckedChanged);
			// 
			// ShowLegend
			// 
			this.ShowLegend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ShowLegend.Location = new System.Drawing.Point(8, 32);
			this.ShowLegend.Name = "ShowLegend";
			this.ShowLegend.Size = new System.Drawing.Size(600, 16);
			this.ShowLegend.TabIndex = 24;
			this.ShowLegend.Text = "Hide invisible layers";
			this.ShowLegend.CheckedChanged += new System.EventHandler(this.ShowLegend_CheckedChanged);
			// 
			// ShowTitle
			// 
			this.ShowTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ShowTitle.Location = new System.Drawing.Point(8, 104);
			this.ShowTitle.Name = "ShowTitle";
			this.ShowTitle.Size = new System.Drawing.Size(600, 16);
			this.ShowTitle.TabIndex = 27;
			this.ShowTitle.Text = "Show Root Folder";
			this.ShowTitle.CheckedChanged += new System.EventHandler(this.ShowTitle_CheckedChanged);
			// 
			// ShowPrintUI
			// 
			this.ShowPrintUI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ShowPrintUI.Location = new System.Drawing.Point(8, 80);
			this.ShowPrintUI.Name = "ShowPrintUI";
			this.ShowPrintUI.Size = new System.Drawing.Size(600, 16);
			this.ShowPrintUI.TabIndex = 26;
			this.ShowPrintUI.Text = "Hide invisible layers";
			this.ShowPrintUI.CheckedChanged += new System.EventHandler(this.ShowPrintUI_CheckedChanged);
			// 
			// Print
			// 
			this.Controls.Add(this.ShowTitle);
			this.Controls.Add(this.ShowPrintUI);
			this.Controls.Add(this.ShowNorthArrow);
			this.Controls.Add(this.ShowLegend);
			this.Controls.Add(this.PageTitle);
			this.Controls.Add(this.label5);
			this.Name = "Print";
			this.Size = new System.Drawing.Size(616, 128);
			this.ResumeLayout(false);

		}
		#endregion

		private void PageTitle_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("PageTitle", PageTitle.Text);
		}

		private void ShowLegend_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("ShowLegend", ShowLegend.Checked.ToString().ToLower());
		}

		private void ShowNorthArrow_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("ShowNorthArrow", ShowNorthArrow.Checked.ToString().ToLower());
		}

		private void ShowPrintUI_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("ShowPrintUI", ShowPrintUI.Checked.ToString().ToLower());
		}

		private void ShowTitle_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("ShowTitle", ShowTitle.Checked.ToString().ToLower());
		}
	}
}

