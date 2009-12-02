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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapLayerGroupProperties));
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
            resources.ApplyResources(this, "$this");
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
