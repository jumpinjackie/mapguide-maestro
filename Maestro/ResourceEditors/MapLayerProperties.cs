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
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for MapLayerProperties.
	/// </summary>
	public class MapLayerProperties : System.Windows.Forms.UserControl
    {
        public TextBox txtLayername;
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

        private OSGeo.MapGuide.MaestroAPI.BaseMapLayerType m_layer;
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

        internal void SelectLayerItem(OSGeo.MapGuide.MaestroAPI.BaseMapLayerType layer, EditorInterface editor)
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
                    if (m_layer as MaestroAPI.MapLayerType != null)
                    {
                        chkLayerVisible.Visible = true;
                        chkLayerVisible.Checked = (m_layer as MaestroAPI.MapLayerType).Visible;
                    }
                    else
                        chkLayerVisible.Visible = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapLayerProperties));
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
            resources.ApplyResources(this.txtLayername, "txtLayername");
            this.txtLayername.Name = "txtLayername";
            this.txtLayername.TextChanged += new System.EventHandler(this.txtLayername_TextChanged);
            // 
            // chkLayerShowInLegend
            // 
            resources.ApplyResources(this.chkLayerShowInLegend, "chkLayerShowInLegend");
            this.chkLayerShowInLegend.Name = "chkLayerShowInLegend";
            this.chkLayerShowInLegend.CheckedChanged += new System.EventHandler(this.chkLayerShowInLegend_CheckedChanged);
            // 
            // chkLayerVisible
            // 
            resources.ApplyResources(this.chkLayerVisible, "chkLayerVisible");
            this.chkLayerVisible.Name = "chkLayerVisible";
            this.chkLayerVisible.CheckedChanged += new System.EventHandler(this.chkLayerVisible_CheckedChanged);
            // 
            // chkLayerSelectable
            // 
            resources.ApplyResources(this.chkLayerSelectable, "chkLayerSelectable");
            this.chkLayerSelectable.Name = "chkLayerSelectable";
            this.chkLayerSelectable.CheckedChanged += new System.EventHandler(this.chkLayerSelectable_CheckedChanged);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label12
            // 
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // chkLayerExpand
            // 
            resources.ApplyResources(this.chkLayerExpand, "chkLayerExpand");
            this.chkLayerExpand.Name = "chkLayerExpand";
            this.chkLayerExpand.CheckedChanged += new System.EventHandler(this.chkLayerExpand_CheckedChanged);
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // txtLayerLegendLabel
            // 
            resources.ApplyResources(this.txtLayerLegendLabel, "txtLayerLegendLabel");
            this.txtLayerLegendLabel.Name = "txtLayerLegendLabel";
            this.txtLayerLegendLabel.TextChanged += new System.EventHandler(this.txtLayerLegendLabel_TextChanged);
            // 
            // btnSelectLayer
            // 
            resources.ApplyResources(this.btnSelectLayer, "btnSelectLayer");
            this.btnSelectLayer.Name = "btnSelectLayer";
            this.btnSelectLayer.Click += new System.EventHandler(this.btnSelectLayer_Click);
            // 
            // txtLayerResource
            // 
            resources.ApplyResources(this.txtLayerResource, "txtLayerResource");
            this.txtLayerResource.Name = "txtLayerResource";
            this.txtLayerResource.ReadOnly = true;
            // 
            // MapLayerProperties
            // 
            resources.ApplyResources(this, "$this");
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
            this.ResumeLayout(false);
            this.PerformLayout();

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

			if (m_layer as MaestroAPI.MapLayerType != null)
				(m_layer as MaestroAPI.MapLayerType).Visible = chkLayerVisible.Checked;
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
