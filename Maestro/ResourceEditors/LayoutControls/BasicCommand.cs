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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicCommand));
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
            // ItemName
            // 
            resources.ApplyResources(this.ItemName, "ItemName");
            this.ItemName.Name = "ItemName";
            this.ItemName.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
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
            // ItemNameLabel
            // 
            this.ItemNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.ItemNameLabel, "ItemNameLabel");
            this.ItemNameLabel.Name = "ItemNameLabel";
            // 
            // ItemAction
            // 
            resources.ApplyResources(this.ItemAction, "ItemAction");
            this.ItemAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemAction.Name = "ItemAction";
            this.ItemAction.SelectedIndexChanged += new System.EventHandler(this.ItemAction_SelectedIndexChanged);
            this.ItemAction.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // ItemActionLabel
            // 
            this.ItemActionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.ItemActionLabel, "ItemActionLabel");
            this.ItemActionLabel.Name = "ItemActionLabel";
            // 
            // BasicCommand
            // 
            resources.ApplyResources(this, "$this");
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
						MessageBox.Show(this, Strings.BasicCommand.DuplicateRenameError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
