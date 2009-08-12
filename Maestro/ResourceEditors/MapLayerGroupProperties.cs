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
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for MapLayerGroupProperties.
	/// </summary>
	public class MapLayerGroupProperties : System.Windows.Forms.UserControl
    {
        public TextBox txtLayername;
		private System.Windows.Forms.CheckBox chkLayerShowInLegend;
		private System.Windows.Forms.CheckBox chkLayerVisible;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.CheckBox chkLayerExpand;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtLayerLegendLabel;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private OSGeo.MapGuide.MaestroAPI.MapLayerGroupCommonType m_group = null;
		private bool m_isUpdating = false;

		public event System.EventHandler LayerPropertiesChanged;
        public event GroupRenameDelegate GroupRenamed;

        public delegate void GroupRenameDelegate(object sender, GroupRenameEventArgs args);

        public class GroupRenameEventArgs : System.EventArgs
        {
            public GroupRenameEventArgs(string previousName, string newName, OSGeo.MapGuide.MaestroAPI.MapLayerGroupCommonType group)
            {
                this.PreviousName = previousName;
                this.NewName = newName;
                this.Group = group;
            }

            public string NewName;
            public string PreviousName;
            public OSGeo.MapGuide.MaestroAPI.MapLayerGroupCommonType Group;
        }

		public MapLayerGroupProperties()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			

		}

        internal void SelectLayerItem(OSGeo.MapGuide.MaestroAPI.MapLayerGroupCommonType group, EditorInterface editor)
		{
			if (m_isUpdating)
				return;

			try
			{
				m_isUpdating = true;

				m_group = group;
				if (group == null)
				{
					this.Visible = false;
				}
				else
				{
					this.Visible = true;
					txtLayername.Text = group.Name;
					txtLayerLegendLabel.Text = group.LegendLabel;
                    chkLayerVisible.Checked = group.Visible;

					chkLayerShowInLegend.Checked = group.ShowInLegend;
					chkLayerExpand.Checked = group.ExpandInLegend;
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
            this.label12 = new System.Windows.Forms.Label();
            this.chkLayerExpand = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLayerLegendLabel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLayername
            // 
            this.txtLayername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLayername.Location = new System.Drawing.Point(128, 8);
            this.txtLayername.Name = "txtLayername";
            this.txtLayername.Size = new System.Drawing.Size(160, 20);
            this.txtLayername.TabIndex = 47;
            this.txtLayername.TextChanged += new System.EventHandler(this.txtLayername_TextChanged);
            // 
            // chkLayerShowInLegend
            // 
            this.chkLayerShowInLegend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLayerShowInLegend.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkLayerShowInLegend.Location = new System.Drawing.Point(16, 104);
            this.chkLayerShowInLegend.Name = "chkLayerShowInLegend";
            this.chkLayerShowInLegend.Size = new System.Drawing.Size(272, 16);
            this.chkLayerShowInLegend.TabIndex = 46;
            this.chkLayerShowInLegend.Text = "Group is shown in the map legend";
            this.chkLayerShowInLegend.CheckedChanged += new System.EventHandler(this.chkLayerShowInLegend_CheckedChanged);
            // 
            // chkLayerVisible
            // 
            this.chkLayerVisible.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLayerVisible.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkLayerVisible.Location = new System.Drawing.Point(16, 80);
            this.chkLayerVisible.Name = "chkLayerVisible";
            this.chkLayerVisible.Size = new System.Drawing.Size(272, 16);
            this.chkLayerVisible.TabIndex = 44;
            this.chkLayerVisible.Text = "Group is visible at startup";
            this.chkLayerVisible.CheckedChanged += new System.EventHandler(this.chkLayerVisible_CheckedChanged);
            // 
            // label12
            // 
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label12.Location = new System.Drawing.Point(16, 40);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(96, 16);
            this.label12.TabIndex = 43;
            this.label12.Text = "Legend label";
            // 
            // chkLayerExpand
            // 
            this.chkLayerExpand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLayerExpand.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkLayerExpand.Location = new System.Drawing.Point(16, 128);
            this.chkLayerExpand.Name = "chkLayerExpand";
            this.chkLayerExpand.Size = new System.Drawing.Size(272, 16);
            this.chkLayerExpand.TabIndex = 45;
            this.chkLayerExpand.Text = "Group is expanded in legend";
            this.chkLayerExpand.CheckedChanged += new System.EventHandler(this.chkLayerExpand_CheckedChanged);
            // 
            // label10
            // 
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label10.Location = new System.Drawing.Point(16, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 16);
            this.label10.TabIndex = 41;
            this.label10.Text = "Name";
            // 
            // txtLayerLegendLabel
            // 
            this.txtLayerLegendLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLayerLegendLabel.Location = new System.Drawing.Point(128, 40);
            this.txtLayerLegendLabel.Name = "txtLayerLegendLabel";
            this.txtLayerLegendLabel.Size = new System.Drawing.Size(160, 20);
            this.txtLayerLegendLabel.TabIndex = 49;
            this.txtLayerLegendLabel.TextChanged += new System.EventHandler(this.txtLayerLegendLabel_TextChanged);
            // 
            // MapLayerGroupProperties
            // 
            this.Controls.Add(this.txtLayername);
            this.Controls.Add(this.chkLayerShowInLegend);
            this.Controls.Add(this.chkLayerVisible);
            this.Controls.Add(this.chkLayerExpand);
            this.Controls.Add(this.txtLayerLegendLabel);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Name = "MapLayerGroupProperties";
            this.Size = new System.Drawing.Size(304, 184);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void txtLayername_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

            if (m_group != null)
            {
                string prev= m_group.Name;
                m_group.Name = txtLayername.Text;

                if (GroupRenamed != null)
                    GroupRenamed(this, new GroupRenameEventArgs(prev, m_group.Name, m_group));

                if (LayerPropertiesChanged != null)
                    LayerPropertiesChanged(sender, e);
            }
		
		}

		private void txtLayerLegendLabel_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_group != null)
				m_group.LegendLabel = txtLayerLegendLabel.Text;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void chkLayerVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_group != null)
				m_group.Visible = chkLayerVisible.Checked;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void chkLayerShowInLegend_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_group != null)
				if (m_isUpdating)
					return;

			m_group.ShowInLegend = chkLayerVisible.Checked;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);
		}

		private void chkLayerExpand_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (m_group != null)
				m_group.ExpandInLegend = chkLayerExpand.Checked;
			if (LayerPropertiesChanged != null)
				LayerPropertiesChanged(sender, e);		
		}
	}
}
