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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Print));
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
            resources.ApplyResources(this.PageTitle, "PageTitle");
            this.PageTitle.Name = "PageTitle";
            this.PageTitle.TextChanged += new System.EventHandler(this.PageTitle_TextChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // ShowNorthArrow
            // 
            resources.ApplyResources(this.ShowNorthArrow, "ShowNorthArrow");
            this.ShowNorthArrow.Name = "ShowNorthArrow";
            this.ShowNorthArrow.CheckedChanged += new System.EventHandler(this.ShowNorthArrow_CheckedChanged);
            // 
            // ShowLegend
            // 
            resources.ApplyResources(this.ShowLegend, "ShowLegend");
            this.ShowLegend.Name = "ShowLegend";
            this.ShowLegend.CheckedChanged += new System.EventHandler(this.ShowLegend_CheckedChanged);
            // 
            // ShowTitle
            // 
            resources.ApplyResources(this.ShowTitle, "ShowTitle");
            this.ShowTitle.Name = "ShowTitle";
            this.ShowTitle.CheckedChanged += new System.EventHandler(this.ShowTitle_CheckedChanged);
            // 
            // ShowPrintUI
            // 
            resources.ApplyResources(this.ShowPrintUI, "ShowPrintUI");
            this.ShowPrintUI.Name = "ShowPrintUI";
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
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

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

