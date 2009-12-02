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
	public class ZoomToSelection : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.ComboBox ZoomFactor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox MaximumZoomDimension;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.IContainer components = null;

		public ZoomToSelection()
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

				MaximumZoomDimension.Text = GetSettingValue("MaximumZoomDimension"); 
				ZoomFactor.Text = GetSettingValue("ZoomFactor"); 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZoomToSelection));
            this.ZoomFactor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MaximumZoomDimension = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ZoomFactor
            // 
            resources.ApplyResources(this.ZoomFactor, "ZoomFactor");
            this.ZoomFactor.Items.AddRange(new object[] {
            resources.GetString("ZoomFactor.Items"),
            resources.GetString("ZoomFactor.Items1"),
            resources.GetString("ZoomFactor.Items2"),
            resources.GetString("ZoomFactor.Items3")});
            this.ZoomFactor.Name = "ZoomFactor";
            this.ZoomFactor.TextChanged += new System.EventHandler(this.ZoomFactor_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // MaximumZoomDimension
            // 
            resources.ApplyResources(this.MaximumZoomDimension, "MaximumZoomDimension");
            this.MaximumZoomDimension.Name = "MaximumZoomDimension";
            this.MaximumZoomDimension.TextChanged += new System.EventHandler(this.MaximumZoomDimension_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ZoomToSelection
            // 
            this.Controls.Add(this.ZoomFactor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MaximumZoomDimension);
            this.Controls.Add(this.label2);
            this.Name = "ZoomToSelection";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void MaximumZoomDimension_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MaximumZoomDimension", MaximumZoomDimension.Text);
		}

		private void ZoomFactor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("ZoomFactor", ZoomFactor.Text);
		}
	}
}

