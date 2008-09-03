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
			this.SuspendLayout();
			// 
			// ItemPictureDisabled
			// 
			this.ItemPictureDisabled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ItemPictureDisabled.Location = new System.Drawing.Point(96, 136);
			this.ItemPictureDisabled.Name = "ItemPictureDisabled";
			this.ItemPictureDisabled.Size = new System.Drawing.Size(20, 20);
			this.ItemPictureDisabled.TabIndex = 43;
			this.ItemPictureDisabled.TabStop = false;
			// 
			// ItemPictureEnabled
			// 
			this.ItemPictureEnabled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ItemPictureEnabled.Location = new System.Drawing.Point(96, 104);
			this.ItemPictureEnabled.Name = "ItemPictureEnabled";
			this.ItemPictureEnabled.Size = new System.Drawing.Size(20, 20);
			this.ItemPictureEnabled.TabIndex = 42;
			this.ItemPictureEnabled.TabStop = false;
			// 
			// ItemIconDisabled
			// 
			this.ItemIconDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemIconDisabled.Location = new System.Drawing.Point(120, 136);
			this.ItemIconDisabled.Name = "ItemIconDisabled";
			this.ItemIconDisabled.Size = new System.Drawing.Size(64, 20);
			this.ItemIconDisabled.TabIndex = 41;
			this.ItemIconDisabled.Text = "";
			this.ItemIconDisabled.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemIconEnabled
			// 
			this.ItemIconEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemIconEnabled.Location = new System.Drawing.Point(120, 104);
			this.ItemIconEnabled.Name = "ItemIconEnabled";
			this.ItemIconEnabled.Size = new System.Drawing.Size(64, 20);
			this.ItemIconEnabled.TabIndex = 40;
			this.ItemIconEnabled.Text = "";
			this.ItemIconEnabled.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemDescription
			// 
			this.ItemDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemDescription.Location = new System.Drawing.Point(96, 72);
			this.ItemDescription.Name = "ItemDescription";
			this.ItemDescription.Size = new System.Drawing.Size(88, 20);
			this.ItemDescription.TabIndex = 39;
			this.ItemDescription.Text = "";
			this.ItemDescription.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemTooltip
			// 
			this.ItemTooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemTooltip.Location = new System.Drawing.Point(96, 40);
			this.ItemTooltip.Name = "ItemTooltip";
			this.ItemTooltip.Size = new System.Drawing.Size(88, 20);
			this.ItemTooltip.TabIndex = 38;
			this.ItemTooltip.Text = "";
			this.ItemTooltip.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemTitle
			// 
			this.ItemTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemTitle.Location = new System.Drawing.Point(96, 8);
			this.ItemTitle.Name = "ItemTitle";
			this.ItemTitle.Size = new System.Drawing.Size(88, 20);
			this.ItemTitle.TabIndex = 37;
			this.ItemTitle.Text = "";
			this.ItemTitle.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// label19
			// 
			this.label19.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label19.Location = new System.Drawing.Point(8, 136);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(80, 16);
			this.label19.TabIndex = 36;
			this.label19.Text = "Icon disabled";
			// 
			// label20
			// 
			this.label20.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label20.Location = new System.Drawing.Point(8, 104);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(80, 16);
			this.label20.TabIndex = 35;
			this.label20.Text = "Icon enabled";
			// 
			// label21
			// 
			this.label21.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label21.Location = new System.Drawing.Point(8, 72);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(80, 16);
			this.label21.TabIndex = 34;
			this.label21.Text = "Description";
			// 
			// label22
			// 
			this.label22.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label22.Location = new System.Drawing.Point(8, 40);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(80, 16);
			this.label22.TabIndex = 33;
			this.label22.Text = "Tooltip";
			// 
			// label23
			// 
			this.label23.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label23.Location = new System.Drawing.Point(8, 8);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(80, 16);
			this.label23.TabIndex = 32;
			this.label23.Text = "Title";
			// 
			// FlyoutItem
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(192, 160);
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
			this.Size = new System.Drawing.Size(192, 160);
			this.ResumeLayout(false);

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
