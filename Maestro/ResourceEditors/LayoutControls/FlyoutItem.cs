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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls
{
	/// <summary>
	/// Summary description for FlyoutItem.
	/// </summary>
	public class FlyoutItem : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.PictureBox ItemPictureDisabled;
		private System.Windows.Forms.PictureBox ItemPictureEnabled;
		private System.Windows.Forms.TextBox ItemIconDisabled;
		private System.Windows.Forms.TextBox ItemIconEnabled;
		private System.Windows.Forms.TextBox ItemDescription;
		private System.Windows.Forms.TextBox ItemTooltip;
		private System.Windows.Forms.TextBox ItemTitle;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.FlyoutItemType m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
		private LayoutEditor m_layoutEditor = null;

		public FlyoutItem()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(OSGeo.MapGuide.MaestroAPI.FlyoutItemType command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
		{
			m_command = command;
			m_layout = layout;
			m_layoutEditor = layoutEditor;
			m_editor = editor;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_command == null || m_layoutEditor == null)
					return;

				ItemTitle.Text = m_command.Label;
				ItemTooltip.Text = m_command.Tooltip;
				ItemDescription.Text = m_command.Description;
				ItemIconEnabled.Text = m_command.ImageURL;
				ItemIconDisabled.Text = m_command.DisabledImageURL;

				if (m_layoutEditor != null)
				{
					ItemPictureEnabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(m_command.ImageURL)];
					ItemPictureDisabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(m_command.DisabledImageURL)];
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlyoutItem));
            this.ItemPictureDisabled = new System.Windows.Forms.PictureBox();
            this.ItemPictureEnabled = new System.Windows.Forms.PictureBox();
            this.ItemIconDisabled = new System.Windows.Forms.TextBox();
            this.ItemIconEnabled = new System.Windows.Forms.TextBox();
            this.ItemDescription = new System.Windows.Forms.TextBox();
            this.ItemTooltip = new System.Windows.Forms.TextBox();
            this.ItemTitle = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ItemPictureDisabled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemPictureEnabled)).BeginInit();
            this.SuspendLayout();
            // 
            // ItemPictureDisabled
            // 
            this.ItemPictureDisabled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.ItemPictureDisabled, "ItemPictureDisabled");
            this.ItemPictureDisabled.Name = "ItemPictureDisabled";
            this.ItemPictureDisabled.TabStop = false;
            // 
            // ItemPictureEnabled
            // 
            this.ItemPictureEnabled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.ItemPictureEnabled, "ItemPictureEnabled");
            this.ItemPictureEnabled.Name = "ItemPictureEnabled";
            this.ItemPictureEnabled.TabStop = false;
            // 
            // ItemIconDisabled
            // 
            resources.ApplyResources(this.ItemIconDisabled, "ItemIconDisabled");
            this.ItemIconDisabled.Name = "ItemIconDisabled";
            this.ItemIconDisabled.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // ItemIconEnabled
            // 
            resources.ApplyResources(this.ItemIconEnabled, "ItemIconEnabled");
            this.ItemIconEnabled.Name = "ItemIconEnabled";
            this.ItemIconEnabled.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // ItemDescription
            // 
            resources.ApplyResources(this.ItemDescription, "ItemDescription");
            this.ItemDescription.Name = "ItemDescription";
            this.ItemDescription.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // ItemTooltip
            // 
            resources.ApplyResources(this.ItemTooltip, "ItemTooltip");
            this.ItemTooltip.Name = "ItemTooltip";
            this.ItemTooltip.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // ItemTitle
            // 
            resources.ApplyResources(this.ItemTitle, "ItemTitle");
            this.ItemTitle.Name = "ItemTitle";
            this.ItemTitle.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label19
            // 
            this.label19.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label20
            // 
            this.label20.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // label21
            // 
            this.label21.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label22
            // 
            this.label22.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // label23
            // 
            this.label23.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // FlyoutItem
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ItemPictureDisabled);
            this.Controls.Add(this.ItemPictureEnabled);
            this.Controls.Add(this.ItemIconDisabled);
            this.Controls.Add(this.ItemIconEnabled);
            this.Controls.Add(this.ItemDescription);
            this.Controls.Add(this.ItemTooltip);
            this.Controls.Add(this.ItemTitle);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Name = "FlyoutItem";
            ((System.ComponentModel.ISupportInitialize)(this.ItemPictureDisabled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemPictureEnabled)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void SomeProperty_Changed(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			m_command.Label = ItemTitle.Text;
			m_command.Tooltip = ItemTooltip.Text;
			m_command.Description = ItemDescription.Text;
			m_command.ImageURL = ItemIconEnabled.Text;
			m_command.DisabledImageURL = ItemIconDisabled.Text;

			if (m_layoutEditor != null)
			{
				ItemPictureEnabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(m_command.ImageURL)];
				ItemPictureDisabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(m_command.DisabledImageURL)];
				m_layoutEditor.NameHasChanged(m_command.Label);
			}

			m_editor.HasChanged();
		
		}
	}
}
