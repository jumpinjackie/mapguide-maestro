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
	public class Select : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox QueryActiveLayer;
		private System.Windows.Forms.TextBox Tolerance;
		private System.Windows.Forms.ComboBox SelectionType;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.IContainer components = null;

		public Select()
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

				try { QueryActiveLayer.Checked = bool.Parse(GetSettingValue("QueryActiveLayer")); }
				catch {}
				Tolerance.Text = GetSettingValue("Tolerance"); 
				SelectionType.Text = GetSettingValue("SelectionType"); 
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
			this.QueryActiveLayer = new System.Windows.Forms.CheckBox();
			this.Tolerance = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SelectionType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// QueryActiveLayer
			// 
			this.QueryActiveLayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.QueryActiveLayer.Location = new System.Drawing.Point(8, 8);
			this.QueryActiveLayer.Name = "QueryActiveLayer";
			this.QueryActiveLayer.Size = new System.Drawing.Size(600, 16);
			this.QueryActiveLayer.TabIndex = 24;
			this.QueryActiveLayer.Text = "Query active layer";
			this.QueryActiveLayer.CheckedChanged += new System.EventHandler(this.QueryActiveLayer_CheckedChanged);
			// 
			// Tolerance
			// 
			this.Tolerance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Tolerance.Location = new System.Drawing.Point(144, 32);
			this.Tolerance.Name = "Tolerance";
			this.Tolerance.Size = new System.Drawing.Size(464, 20);
			this.Tolerance.TabIndex = 23;
			this.Tolerance.Text = "";
			this.Tolerance.TextChanged += new System.EventHandler(this.Tolerance_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 22;
			this.label1.Text = "Tolerance";
			// 
			// SelectionType
			// 
			this.SelectionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SelectionType.Items.AddRange(new object[] {
															   "CONTAINS",
															   "DISJOINT",
															   "INTERSECTS",
															   "WITHIN",
															   "INSIDE",
															   "ENVELOPEINTERSECTS"});
			this.SelectionType.Location = new System.Drawing.Point(144, 56);
			this.SelectionType.Name = "SelectionType";
			this.SelectionType.Size = new System.Drawing.Size(464, 21);
			this.SelectionType.TabIndex = 32;
			this.SelectionType.TextChanged += new System.EventHandler(this.SelectionType_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 31;
			this.label2.Text = "Selection type";
			// 
			// Select
			// 
			this.Controls.Add(this.SelectionType);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.QueryActiveLayer);
			this.Controls.Add(this.Tolerance);
			this.Controls.Add(this.label1);
			this.Name = "Select";
			this.Size = new System.Drawing.Size(616, 96);
			this.ResumeLayout(false);

		}
		#endregion

		private void QueryActiveLayer_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("QueryActiveLayer", QueryActiveLayer.Checked.ToString().ToLower());
		}

		private void Tolerance_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Tolerance", Tolerance.Text);
		}

		private void SelectionType_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("SelectionType", SelectionType.Text);
		}
	}
}

