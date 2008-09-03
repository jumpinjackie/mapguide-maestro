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
	/// Summary description for BasicCommand.
	/// </summary>
	public class BasicCommand : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.PictureBox ItemPictureDisabled;
		private System.Windows.Forms.PictureBox ItemPictureEnabled;
		private System.Windows.Forms.TextBox ItemIconDisabled;
		private System.Windows.Forms.TextBox ItemIconEnabled;
		private System.Windows.Forms.TextBox ItemDescription;
		private System.Windows.Forms.TextBox ItemTooltip;
		private System.Windows.Forms.TextBox ItemTitle;
		private System.Windows.Forms.TextBox ItemName;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label ItemNameLabel;
		private System.Windows.Forms.ComboBox ItemAction;
		private System.Windows.Forms.Label ItemActionLabel;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.CommandItemType m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
		private LayoutEditor m_layoutEditor = null;

		public BasicCommand()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(OSGeo.MapGuide.MaestroAPI.CommandItemType command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
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

				ItemAction.Items.Clear();

				foreach(string key in m_layoutEditor.BuiltInCommands.Keys)
					ItemAction.Items.Add(key);

				OSGeo.MapGuide.MaestroAPI.CommandType ct = null;
				foreach(OSGeo.MapGuide.MaestroAPI.CommandType cmd in m_layout.CommandSet)
					if (cmd.Name == m_command.Command)
					{
						ct = cmd;
						break;
					}

				if (ct as OSGeo.MapGuide.MaestroAPI.BasicCommandType != null)
				{
					ItemActionLabel.Visible = ItemAction.Visible = true;
					foreach(string key in m_layoutEditor.BuiltInCommands.Keys)
					{
						DataRow dr = (DataRow)m_layoutEditor.BuiltInCommands[key];
						if (dr["Action"].ToString().Replace(" ", "") == (ct as OSGeo.MapGuide.MaestroAPI.BasicCommandType).Action.ToString())
						{
							ItemAction.SelectedIndex = ItemAction.FindStringExact(dr["Command"].ToString());
							break;
						}
					}
				}
				else
				{
					ItemActionLabel.Visible = ItemAction.Visible = false;
				}

				ItemName.Text = ct.Name;
				ItemTitle.Text = ct.Label;
				ItemTooltip.Text = ct.Tooltip;
				ItemDescription.Text = ct.Description;
				ItemIconEnabled.Text = ct.ImageURL;
				ItemIconDisabled.Text = ct.DisabledImageURL;

				if (m_layoutEditor != null)
				{
					ItemPictureEnabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(ct.ImageURL)];
					ItemPictureDisabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(ct.DisabledImageURL)];
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
			this.ItemName = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.ItemNameLabel = new System.Windows.Forms.Label();
			this.ItemAction = new System.Windows.Forms.ComboBox();
			this.ItemActionLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ItemPictureDisabled
			// 
			this.ItemPictureDisabled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ItemPictureDisabled.Location = new System.Drawing.Point(96, 200);
			this.ItemPictureDisabled.Name = "ItemPictureDisabled";
			this.ItemPictureDisabled.Size = new System.Drawing.Size(20, 20);
			this.ItemPictureDisabled.TabIndex = 31;
			this.ItemPictureDisabled.TabStop = false;
			// 
			// ItemPictureEnabled
			// 
			this.ItemPictureEnabled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ItemPictureEnabled.Location = new System.Drawing.Point(96, 168);
			this.ItemPictureEnabled.Name = "ItemPictureEnabled";
			this.ItemPictureEnabled.Size = new System.Drawing.Size(20, 20);
			this.ItemPictureEnabled.TabIndex = 30;
			this.ItemPictureEnabled.TabStop = false;
			// 
			// ItemIconDisabled
			// 
			this.ItemIconDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemIconDisabled.Location = new System.Drawing.Point(120, 200);
			this.ItemIconDisabled.Name = "ItemIconDisabled";
			this.ItemIconDisabled.Size = new System.Drawing.Size(64, 20);
			this.ItemIconDisabled.TabIndex = 29;
			this.ItemIconDisabled.Text = "";
			this.ItemIconDisabled.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemIconEnabled
			// 
			this.ItemIconEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemIconEnabled.Location = new System.Drawing.Point(120, 168);
			this.ItemIconEnabled.Name = "ItemIconEnabled";
			this.ItemIconEnabled.Size = new System.Drawing.Size(64, 20);
			this.ItemIconEnabled.TabIndex = 28;
			this.ItemIconEnabled.Text = "";
			this.ItemIconEnabled.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemDescription
			// 
			this.ItemDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemDescription.Location = new System.Drawing.Point(96, 136);
			this.ItemDescription.Name = "ItemDescription";
			this.ItemDescription.Size = new System.Drawing.Size(88, 20);
			this.ItemDescription.TabIndex = 27;
			this.ItemDescription.Text = "";
			this.ItemDescription.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemTooltip
			// 
			this.ItemTooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemTooltip.Location = new System.Drawing.Point(96, 104);
			this.ItemTooltip.Name = "ItemTooltip";
			this.ItemTooltip.Size = new System.Drawing.Size(88, 20);
			this.ItemTooltip.TabIndex = 26;
			this.ItemTooltip.Text = "";
			this.ItemTooltip.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemTitle
			// 
			this.ItemTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemTitle.Location = new System.Drawing.Point(96, 72);
			this.ItemTitle.Name = "ItemTitle";
			this.ItemTitle.Size = new System.Drawing.Size(88, 20);
			this.ItemTitle.TabIndex = 25;
			this.ItemTitle.Text = "";
			this.ItemTitle.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// ItemName
			// 
			this.ItemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemName.Location = new System.Drawing.Point(96, 40);
			this.ItemName.Name = "ItemName";
			this.ItemName.Size = new System.Drawing.Size(88, 20);
			this.ItemName.TabIndex = 24;
			this.ItemName.Text = "";
			this.ItemName.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// label19
			// 
			this.label19.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label19.Location = new System.Drawing.Point(8, 200);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(80, 16);
			this.label19.TabIndex = 23;
			this.label19.Text = "Icon disabled";
			// 
			// label20
			// 
			this.label20.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label20.Location = new System.Drawing.Point(8, 168);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(80, 16);
			this.label20.TabIndex = 22;
			this.label20.Text = "Icon enabled";
			// 
			// label21
			// 
			this.label21.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label21.Location = new System.Drawing.Point(8, 136);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(80, 16);
			this.label21.TabIndex = 21;
			this.label21.Text = "Description";
			// 
			// label22
			// 
			this.label22.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label22.Location = new System.Drawing.Point(8, 104);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(80, 16);
			this.label22.TabIndex = 20;
			this.label22.Text = "Tooltip";
			// 
			// label23
			// 
			this.label23.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label23.Location = new System.Drawing.Point(8, 72);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(80, 16);
			this.label23.TabIndex = 19;
			this.label23.Text = "Title";
			// 
			// ItemNameLabel
			// 
			this.ItemNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ItemNameLabel.Location = new System.Drawing.Point(8, 40);
			this.ItemNameLabel.Name = "ItemNameLabel";
			this.ItemNameLabel.Size = new System.Drawing.Size(80, 16);
			this.ItemNameLabel.TabIndex = 18;
			this.ItemNameLabel.Text = "Name";
			// 
			// ItemAction
			// 
			this.ItemAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ItemAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItemAction.Location = new System.Drawing.Point(96, 8);
			this.ItemAction.Name = "ItemAction";
			this.ItemAction.Size = new System.Drawing.Size(88, 21);
			this.ItemAction.TabIndex = 17;
			this.ItemAction.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			this.ItemAction.SelectedIndexChanged += new System.EventHandler(this.ItemAction_SelectedIndexChanged);
			// 
			// ItemActionLabel
			// 
			this.ItemActionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ItemActionLabel.Location = new System.Drawing.Point(8, 8);
			this.ItemActionLabel.Name = "ItemActionLabel";
			this.ItemActionLabel.Size = new System.Drawing.Size(80, 16);
			this.ItemActionLabel.TabIndex = 16;
			this.ItemActionLabel.Text = "Action";
			// 
			// BasicCommand
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(192, 232);
			this.Controls.Add(this.ItemPictureDisabled);
			this.Controls.Add(this.ItemPictureEnabled);
			this.Controls.Add(this.ItemIconDisabled);
			this.Controls.Add(this.ItemIconEnabled);
			this.Controls.Add(this.ItemDescription);
			this.Controls.Add(this.ItemTooltip);
			this.Controls.Add(this.ItemTitle);
			this.Controls.Add(this.ItemName);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.ItemNameLabel);
			this.Controls.Add(this.ItemAction);
			this.Controls.Add(this.ItemActionLabel);
			this.Name = "BasicCommand";
			this.Size = new System.Drawing.Size(192, 232);
			this.ResumeLayout(false);

		}
		#endregion

		private void SomeProperty_Changed(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;


			OSGeo.MapGuide.MaestroAPI.CommandType ct = null;
			foreach(OSGeo.MapGuide.MaestroAPI.CommandType cmd in m_layout.CommandSet)
				if (cmd.Name == m_command.Command)
				{
					ct = cmd;
					break;
				}

			if (ct as OSGeo.MapGuide.MaestroAPI.BasicCommandType != null)
			{
				if (ItemAction.Text != "")
					(ct as OSGeo.MapGuide.MaestroAPI.BasicCommandType).Action = (OSGeo.MapGuide.MaestroAPI.BasicCommandActionType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.BasicCommandActionType), ItemAction.Text.Replace(" ", ""), true);
			}

			bool allowRename = true;
				foreach(OSGeo.MapGuide.MaestroAPI.CommandType cmd2 in m_layout.CommandSet)
					if (cmd2.Name == ItemName.Text && ct != cmd2)
					{
						MessageBox.Show(this, m_layoutEditor.Globalizor.Translate("Cannot rename the command, because that would create two commands with the same name."), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						allowRename = false;
						break;
					}

			if (allowRename)
			{
				ct.Name = ItemName.Text;
				m_command.Command = ItemName.Text;
				if (m_layoutEditor != null)
					m_layoutEditor.NameHasChanged(ct.Name);
			}

			ct.Label = ItemTitle.Text;
			ct.Tooltip = ItemTooltip.Text;
			ct.Description = ItemDescription.Text;
			ct.ImageURL = ItemIconEnabled.Text;
			ct.DisabledImageURL = ItemIconDisabled.Text;

			if (m_layoutEditor != null)
			{
				ItemPictureEnabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(ct.ImageURL)];
				ItemPictureDisabled.Image = LayoutEditor.LoadedImageList.Images[m_layoutEditor.FindImageIndex(ct.DisabledImageURL)];
			}

			m_editor.HasChanged();
		}

		private void ItemAction_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

	}
}
