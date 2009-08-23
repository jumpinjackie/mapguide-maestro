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
	/// Summary description for CommandEditor.
	/// </summary>
	public class CommandEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TabControl tabControl;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private object m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
		private ResourceEditors.LayoutControls.BasicCommand basicCommand;
		private ResourceEditors.LayoutControls.FlyoutItem flyoutItem;
		private ResourceEditors.LayoutControls.SearchCommand searchCommand;
		private ResourceEditors.LayoutControls.InvokeURL invokeURL;
		private ResourceEditors.LayoutControls.TargetedCommand targetedCommand;
		private ResourceEditors.LayoutControls.PrintCommand printCommand;
		private ResourceEditors.LayoutControls.HelpCommand helpCommand;
		private ResourceEditors.LayoutControls.InvokeScript invokeScript;
		private System.Windows.Forms.TabPage basicPage;
		private System.Windows.Forms.TabPage extraPage;
		private LayoutEditor m_layoutEditor = null;


		public CommandEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			foreach(Control c in basicPage.Controls)
			{
				c.Dock = DockStyle.Fill;
				c.Visible = false;
			}

			foreach(Control c in extraPage.Controls)
			{
				c.Dock = DockStyle.Fill;
				c.Visible = false;
			}
		}

		public void SetItem(object command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
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
				if (m_command == null)
					return;

				Control extraItem = null;

				if (m_command as OSGeo.MapGuide.MaestroAPI.FlyoutItemType != null)
				{
					tabControl.Visible = false;
					basicCommand.Visible = true;
					flyoutItem.Visible = true;
					this.Visible = true;

					if (flyoutItem.Parent != this)
					{
						flyoutItem.Parent.Controls.Remove(flyoutItem);
						this.Controls.Add(flyoutItem);
					}
					flyoutItem.SetItem(m_command as OSGeo.MapGuide.MaestroAPI.FlyoutItemType, m_layout, m_editor, m_layoutEditor);
				}
				else
				{
					OSGeo.MapGuide.MaestroAPI.CommandItemType comt = m_command as OSGeo.MapGuide.MaestroAPI.CommandItemType;
					if (comt == null)
					{
						this.Visible = false;
						return;
					}

					OSGeo.MapGuide.MaestroAPI.CommandType ct = null;
					foreach(OSGeo.MapGuide.MaestroAPI.CommandType cmd in m_layout.CommandSet)
						if (cmd.Name == comt.Command)
						{
							ct = cmd;
							break;
						}

					if (ct == null)
					{
						this.Visible = false;
						return;
					}

					if (ct as OSGeo.MapGuide.MaestroAPI.HelpCommandType != null)
						extraItem = helpCommand;
					else if (ct as OSGeo.MapGuide.MaestroAPI.InvokeScriptCommandType != null)
						extraItem = invokeScript;
					else if (ct as OSGeo.MapGuide.MaestroAPI.InvokeURLCommandType != null)
						extraItem = invokeURL;
					else if (ct as OSGeo.MapGuide.MaestroAPI.PrintCommandType != null)
						extraItem = printCommand;
					else if (ct as OSGeo.MapGuide.MaestroAPI.SearchCommandType != null)
						extraItem = searchCommand;
					else if (ct as OSGeo.MapGuide.MaestroAPI.TargetedCommandType != null)
						extraItem = targetedCommand;

					flyoutItem.Visible = false;
					basicCommand.Visible = true;
					basicCommand.SetItem(m_command as OSGeo.MapGuide.MaestroAPI.CommandItemType, m_layout, m_editor, m_layoutEditor);

					if (extraItem == null)
					{
						tabControl.Visible = false;
						if (basicCommand.Parent != this)
						{
							basicCommand.Parent.Controls.Remove(basicCommand);
							basicCommand.Visible = true;
							this.Controls.Add(basicCommand);
						}
					}
					else
					{
						tabControl.Visible = true;
						if (basicCommand.Parent == this)
						{
							basicCommand.Parent.Controls.Remove(basicCommand);
							basicPage.Controls.Add(basicCommand);
						}

						foreach(Control c in extraPage.Controls)
							c.Visible = c == extraItem;

						extraPage.Visible = true;
						System.Reflection.MethodInfo mi = extraItem.GetType().GetMethod("SetItem");
						mi.Invoke(extraItem, new object[] {ct, m_layout, m_editor, m_layoutEditor});
					}
					this.Visible = true;
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.basicPage = new System.Windows.Forms.TabPage();
            this.flyoutItem = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.FlyoutItem();
            this.basicCommand = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.BasicCommand();
            this.extraPage = new System.Windows.Forms.TabPage();
            this.invokeScript = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.InvokeScript();
            this.helpCommand = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.HelpCommand();
            this.printCommand = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.PrintCommand();
            this.targetedCommand = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.TargetedCommand();
            this.invokeURL = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.InvokeURL();
            this.searchCommand = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.SearchCommand();
            this.tabControl.SuspendLayout();
            this.basicPage.SuspendLayout();
            this.extraPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.basicPage);
            this.tabControl.Controls.Add(this.extraPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(448, 269);
            this.tabControl.TabIndex = 0;
            // 
            // basicPage
            // 
            this.basicPage.Controls.Add(this.flyoutItem);
            this.basicPage.Controls.Add(this.basicCommand);
            this.basicPage.Location = new System.Drawing.Point(4, 22);
            this.basicPage.Name = "basicPage";
            this.basicPage.Size = new System.Drawing.Size(440, 243);
            this.basicPage.TabIndex = 0;
            this.basicPage.Text = "Basic properties";
            // 
            // flyoutItem
            // 
            this.flyoutItem.AutoScroll = true;
            this.flyoutItem.AutoScrollMinSize = new System.Drawing.Size(192, 160);
            this.flyoutItem.Location = new System.Drawing.Point(136, 8);
            this.flyoutItem.Name = "flyoutItem";
            this.flyoutItem.Size = new System.Drawing.Size(288, 168);
            this.flyoutItem.TabIndex = 1;
            // 
            // basicCommand
            // 
            this.basicCommand.AutoScroll = true;
            this.basicCommand.AutoScrollMinSize = new System.Drawing.Size(192, 232);
            this.basicCommand.Location = new System.Drawing.Point(8, 8);
            this.basicCommand.Name = "basicCommand";
            this.basicCommand.Size = new System.Drawing.Size(192, 232);
            this.basicCommand.TabIndex = 0;
            // 
            // extraPage
            // 
            this.extraPage.Controls.Add(this.invokeScript);
            this.extraPage.Controls.Add(this.helpCommand);
            this.extraPage.Controls.Add(this.printCommand);
            this.extraPage.Controls.Add(this.targetedCommand);
            this.extraPage.Controls.Add(this.invokeURL);
            this.extraPage.Controls.Add(this.searchCommand);
            this.extraPage.Location = new System.Drawing.Point(4, 22);
            this.extraPage.Name = "extraPage";
            this.extraPage.Size = new System.Drawing.Size(440, 486);
            this.extraPage.TabIndex = 1;
            this.extraPage.Text = "Extra properties";
            // 
            // invokeScript
            // 
            this.invokeScript.AutoScroll = true;
            this.invokeScript.AutoScrollMinSize = new System.Drawing.Size(192, 232);
            this.invokeScript.Location = new System.Drawing.Point(24, 272);
            this.invokeScript.Name = "invokeScript";
            this.invokeScript.Size = new System.Drawing.Size(112, 80);
            this.invokeScript.TabIndex = 5;
            // 
            // helpCommand
            // 
            this.helpCommand.AutoScroll = true;
            this.helpCommand.AutoScrollMinSize = new System.Drawing.Size(192, 96);
            this.helpCommand.Location = new System.Drawing.Point(168, 152);
            this.helpCommand.Name = "helpCommand";
            this.helpCommand.Size = new System.Drawing.Size(112, 96);
            this.helpCommand.TabIndex = 4;
            // 
            // printCommand
            // 
            this.printCommand.AutoScroll = true;
            this.printCommand.AutoScrollMinSize = new System.Drawing.Size(208, 104);
            this.printCommand.Location = new System.Drawing.Point(24, 152);
            this.printCommand.Name = "printCommand";
            this.printCommand.Size = new System.Drawing.Size(104, 104);
            this.printCommand.TabIndex = 3;
            // 
            // targetedCommand
            // 
            this.targetedCommand.AutoScroll = true;
            this.targetedCommand.AutoScrollMinSize = new System.Drawing.Size(192, 64);
            this.targetedCommand.Location = new System.Drawing.Point(264, 8);
            this.targetedCommand.Name = "targetedCommand";
            this.targetedCommand.Size = new System.Drawing.Size(120, 128);
            this.targetedCommand.TabIndex = 2;
            // 
            // invokeURL
            // 
            this.invokeURL.AutoScroll = true;
            this.invokeURL.AutoScrollMinSize = new System.Drawing.Size(192, 336);
            this.invokeURL.Location = new System.Drawing.Point(136, 8);
            this.invokeURL.Name = "invokeURL";
            this.invokeURL.Size = new System.Drawing.Size(112, 128);
            this.invokeURL.TabIndex = 1;
            // 
            // searchCommand
            // 
            this.searchCommand.AutoScroll = true;
            this.searchCommand.AutoScrollMinSize = new System.Drawing.Size(192, 408);
            this.searchCommand.Location = new System.Drawing.Point(16, 8);
            this.searchCommand.Name = "searchCommand";
            this.searchCommand.Size = new System.Drawing.Size(112, 128);
            this.searchCommand.TabIndex = 0;
            // 
            // CommandEditor
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(220, 269);
            this.Controls.Add(this.tabControl);
            this.Name = "CommandEditor";
            this.Size = new System.Drawing.Size(448, 269);
            this.tabControl.ResumeLayout(false);
            this.basicPage.ResumeLayout(false);
            this.extraPage.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
	}
}
