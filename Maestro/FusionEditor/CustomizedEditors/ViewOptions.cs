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
	public class ViewOptions : FusionEditor.BasisWidgetEditor
    {
        private CheckBox DisplayUnits;
		private System.ComponentModel.IContainer components = null;

		public ViewOptions()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
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

                DisplayUnits.Checked = GetSettingValue("DisplayUnits") == null || GetSettingValue("DisplayUnits").Trim().ToLower() == "true";
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewOptions));
            this.DisplayUnits = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // DisplayUnits
            // 
            resources.ApplyResources(this.DisplayUnits, "DisplayUnits");
            this.DisplayUnits.Name = "DisplayUnits";
            this.DisplayUnits.UseVisualStyleBackColor = true;
            this.DisplayUnits.CheckedChanged += new System.EventHandler(this.DisplayUnits_CheckedChanged);
            // 
            // ViewOptions
            // 
            this.Controls.Add(this.DisplayUnits);
            this.Name = "ViewOptions";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void DisplayUnits_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating || m_w == null)
                return;

            SetSettingValue("DisplayUnits", DisplayUnits.Checked ? "true" : "false");
        }
	}
}

