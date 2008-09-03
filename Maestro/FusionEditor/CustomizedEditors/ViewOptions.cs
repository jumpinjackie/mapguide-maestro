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
	public class ViewOptions : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.ComboBox DisplayUnits;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components = null;

		public ViewOptions()
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

				DisplayUnits.Text = GetSettingValue("DisplayUnits"); 
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
			this.DisplayUnits = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// DisplayUnits
			// 
			this.DisplayUnits.Items.AddRange(new object[] {
															  "meters",
															  "miles"});
			this.DisplayUnits.Location = new System.Drawing.Point(104, 10);
			this.DisplayUnits.Name = "DisplayUnits";
			this.DisplayUnits.Size = new System.Drawing.Size(504, 21);
			this.DisplayUnits.TabIndex = 7;
			this.DisplayUnits.TextChanged += new System.EventHandler(this.DisplayUnits_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Display units";
			// 
			// ViewOptions
			// 
			this.Controls.Add(this.DisplayUnits);
			this.Controls.Add(this.label1);
			this.Name = "ViewOptions";
			this.Size = new System.Drawing.Size(616, 40);
			this.ResumeLayout(false);

		}
		#endregion

		private void DisplayUnits_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("DisplayUnits", DisplayUnits.Text);
		}
	}
}

