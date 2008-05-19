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
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for MapLayerProperties.
	/// </summary>
	public class MapLayerProperties : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox txtLayername;
		private System.Windows.Forms.CheckBox chkLayerShowInLegend;
		private System.Windows.Forms.CheckBox chkLayerVisible;
		private System.Windows.Forms.CheckBox chkLayerSelectable;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.CheckBox chkLayerExpand;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtLayerLegendLabel;
		private System.Windows.Forms.Button btnSelectLayer;
		private System.Windows.Forms.TextBox txtLayerResource;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.MapLayerType m_layer;
		private System.Windows.Forms.Button btnEdit;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;

		public event System.EventHandler LayerPropertiesChanged;

		public MapLayerProperties()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		internal void SelectLayerItem(OSGeo.MapGuide.MaestroAPI.MapLayerType layer, EditorInterface editor)
		{
			m_layer = layer;
			m_editor = editor;
			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			if (m_isUpdating)
				return;

			try
			{
				m_isUpdating = true;
				if (m_layer == null)
				{
					this.Visible = false;
				}
				else
				{
					this.Visible = true;
					txtLayerResource.Text = m_layer.ResourceId;
					txtLayername.Text = m_layer.Name;
					txtLayerLegendLabel.Text = m_layer.LegendLabel;
					chkLayerVisible.Checked = m_layer.Visible;
					chkLayerSelectable.Checked = m_layer.Selectable;
					chkLayerShowInLegend.Checked = m_layer.ShowInLegend;
					chkLayerExpand.Checked = m_layer.ExpandInLegend;
				}
			}
			finally
			{
				m_isUpdating = false;
			}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtLayername = new System.Windows.Forms.TextBox();
			this.chkLayerShowInLegend = new System.Windows.Forms.CheckBox();
			this.chkLayerVisible = new System.Windows.Forms.CheckBox();
			this.chkLayerSelectable = new System.Windows.Forms.CheckBox();
			this.btnEdit = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.chkLayerExpand = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.txtLayerLegendLabel = new System.Windows.Forms.TextBox();
			this.btnSelectLayer = new System.Windows.Forms.Button();
			this.txtLayerResource = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtLayername
			// 
			this.txtLayername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLayername.Location = new System.Drawing.Point(128, 40);
			this.txtLayername.Name = "txtLayername";
			this.txtLayername.Size = new System.Drawing.Size(160, 20);
			this.txtLayername.TabIndex = 38;
			this.txtLayername.Text = "";
			this.txtLayername.TextChanged += new System.EventHandler(this.txtLayername_TextChanged);
			// 
			// chkLayerShowInLegend
			// 
			this.chkLayerShowInLegend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chkLayerShowInLegend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkLayerShowInLegend.Location = new System.Drawing.Point(16, 160);
			this.chkLayerShowInLegend.Name = "chkLayerShowInLegend";
			this.chkLayerShowInLegend.Size = new System.Drawing.Size(272, 16);
			this.chkLayerShowInLegend.TabIndex = 35;
			this.chkLayerShowInLegend.Text = "Layer is shown in the map legend";
			this.chkLayerShowInLegend.CheckedChanged += new System.EventHandler(this.chkLayerShowInLegend_CheckedChanged);
			// 
			// chkLayerVisible
			// 
			this.chkLayerVisible.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chkLayerVisible.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkLayerVisible.Location = new System.Drawing.Point(16, 112);
			this.chkLayerVisible.Name = "chkLayerVisible";
			this.chkLayerVisible.Size = new System.Drawing.Size(272, 16);
			this.chkLayerVisible.TabIndex = 32;
			this.chkLayerVisible.Text = "Layer is visible at startup (when in display range)";
			this.chkLayerVisible.CheckedChanged += new System.EventHandler(this.chkLayerVisible_CheckedChanged);
			// 
			// chkLayerSelectable
			// 
			this.chkLayerSelectable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chkLayerSelectable.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkLayerSelectable.Location = new System.Drawing.Point(16, 136);
			this.chkLayerSelectable.Name = "chkLayerSelectable";
			this.chkLayerSelectable.Size = new System.Drawing.Size(272, 16);
			this.chkLayerSelectable.TabIndex = 33;
			this.chkLayerSelectable.Text = "Features on layer are selectable (if visible)";
			this.chkLayerSelectable.CheckedChanged += new System.EventHandler(this.chkLayerSelectable_CheckedChanged);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnEdit.Location = new System.Drawing.Point(166, 216);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(128, 32);
			this.btnEdit.TabIndex = 41;
			this.btnEdit.Text = "Edit layer";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// label12
			// 
			this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label12.Location = new System.Drawing.Point(16, 72);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(96, 16);
			this.label12.TabIndex = 31;
			this.label12.Text = "Legend label";
			// 
			// chkLayerExpand
			// 
			this.chkLayerExpand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chkLayerExpand.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkLayerExpand.Location = new System.Drawing.Point(16, 184);
			this.chkLayerExpand.Name = "chkLayerExpand";
			this.chkLayerExpand.Size = new System.Drawing.Size(272, 16);
			this.chkLayerExpand.TabIndex = 34;
			this.chkLayerExpand.Text = "Layer is expanded in legend (if themed)";
			this.chkLayerExpand.CheckedChanged += new System.EventHandler(this.chkLayerExpand_CheckedChanged);
			// 
			// label9
			// 
			this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label9.Location = new System.Drawing.Point(16, 8);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(88, 16);
			this.label9.TabIndex = 28;
			this.label9.Text = "Resource";
			// 
			// label10
			// 
			this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label10.Location = new System.Drawing.Point(16, 40);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(88, 16);
			this.label10.TabIndex = 29;
			this.label10.Text = "Name";
			// 
			// txtLayerLegendLabel
			// 
			this.txtLayerLegendLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLayerLegendLabel.Location = new System.Drawing.Point(128, 72);
			this.txtLayerLegendLabel.Name = "txtLayerLegendLabel";
			this.txtLayerLegendLabel.Size = new System.Drawing.Size(160, 20);
			this.txtLayerLegendLabel.TabIndex = 40;
			this.txtLayerLegendLabel.Text = "textBox10";
			this.txtLayerLegendLabel.TextChanged += new System.EventHandler(this.txtLayerLegendLabel_TextChanged);
			// 
			// btnSelectLayer
			// 
			this.btnSelectLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectLayer.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSelectLayer.Location = new System.Drawing.Point(264, 8);
			this.btnSelectLayer.Name = "btnSelectLayer";
			this.btnSelectLayer.Size = new System.Drawing.Size(24, 20);
			this.btnSelectLayer.TabIndex = 37;
			this.btnSelectLayer.Text = "...";
			this.btnSelectLayer.Click += new System.EventHandler(this.btnSelectLayer_Click);
			// 
			// txtLayerResource
			// 
			this.txtLayerResource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLayerResource.Location = new System.Drawing.Point(128, 8);
			this.txtLayerResource.Name = "txtLayerResource";
			this.txtLayerResource.ReadOnly = true;
			this.txtLayerResource.Size = new System.Drawing.Size(136, 20);
			this.txtLayerResource.TabIndex = 36;
			this.txtLayerResource.Text = "";
			// 
			// MapLayerProperties
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(300, 304);
			this.Controls.Add(this.txtLayername);
			this.Controls.Add(this.chkLayerShowInLegend);
			this.Controls.Add(this.chkLayerVisible);
			this.Controls.Add(this.chkLayerSelectable);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.chkLayerExpand);
			this.Controls.Add(this.txtLayerLegendLabel);
			this.Controls.Add(this.btnSelectLayer);
			this.Controls.Add(this.txtLayerResource);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label10);
			this.Name = "MapLayerProperties";
			this.Size = new System.Drawing.Size(304, 304);
			this.ResumeLayout(false);

		}
		#endregion

		private void txtLayername_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_layer != null)
				m_layer.Name = txtLayername.Text;

			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void txtLayerLegendLabel_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_layer != null)
				m_layer.LegendLabel = txtLayerLegendLabel.Text;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void chkLayerVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_layer != null)
				m_layer.Visible = chkLayerVisible.Checked;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void chkLayerSelectable_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_layer != null)
				m_layer.Selectable = chkLayerSelectable.Checked;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void chkLayerShowInLegend_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_layer != null)
				m_layer.ShowInLegend = chkLayerShowInLegend.Checked;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void chkLayerExpand_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_layer != null)
				m_layer.ExpandInLegend = chkLayerExpand.Checked;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			m_editor.EditItem(m_layer.ResourceId);
		}

		private void btnSelectLayer_Click(object sender, System.EventArgs e)
		{
			string resid = m_editor.BrowseResource("LayerDefinition");
			if (resid != null)
			{
				m_layer.ResourceId = resid;
				txtLayerResource.Text = resid;
				if (LayerPropertiesChanged != null)
					LayerPropertiesChanged(sender, e);
			}
		}

	}
}
