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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls
{
	/// <summary>
	/// Summary description for PrintCommand.
	/// </summary>
	public class PrintCommand : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ListBox Layouts;
        private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;

		private OSGeo.MapGuide.MaestroAPI.PrintCommandType m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
        private ToolStrip toolStrip;
        private ToolStripButton AddPrintLayout;
        private ToolStripButton RemovePrintLayout;
		private LayoutEditor m_layoutEditor = null;

		public PrintCommand()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(OSGeo.MapGuide.MaestroAPI.PrintCommandType command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintCommand));
            this.Layouts = new System.Windows.Forms.ListBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.AddPrintLayout = new System.Windows.Forms.ToolStripButton();
            this.RemovePrintLayout = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Layouts
            // 
            this.Layouts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Layouts.IntegralHeight = false;
            this.Layouts.Location = new System.Drawing.Point(0, 49);
            this.Layouts.Name = "Layouts";
            this.Layouts.Size = new System.Drawing.Size(208, 82);
            this.Layouts.TabIndex = 0;
            this.Layouts.SelectedIndexChanged += new System.EventHandler(this.Layouts_SelectedIndexChanged);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Display these printlayouts";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddPrintLayout,
            this.RemovePrintLayout});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(208, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // AddPrintLayout
            // 
            this.AddPrintLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddPrintLayout.Image = ((System.Drawing.Image)(resources.GetObject("AddPrintLayout.Image")));
            this.AddPrintLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddPrintLayout.Name = "AddPrintLayout";
            this.AddPrintLayout.Size = new System.Drawing.Size(23, 22);
            this.AddPrintLayout.Text = "toolStripButton1";
            this.AddPrintLayout.Click += new System.EventHandler(this.AddPrintLayout_Click);
            // 
            // RemovePrintLayout
            // 
            this.RemovePrintLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemovePrintLayout.Enabled = false;
            this.RemovePrintLayout.Image = ((System.Drawing.Image)(resources.GetObject("RemovePrintLayout.Image")));
            this.RemovePrintLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemovePrintLayout.Name = "RemovePrintLayout";
            this.RemovePrintLayout.Size = new System.Drawing.Size(23, 22);
            this.RemovePrintLayout.Text = "toolStripButton2";
            this.RemovePrintLayout.Click += new System.EventHandler(this.RemovePrintLayout_Click);
            // 
            // PrintCommand
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(208, 104);
            this.Controls.Add(this.Layouts);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.label1);
            this.Name = "PrintCommand";
            this.Size = new System.Drawing.Size(208, 131);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Layouts_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			RemovePrintLayout.Enabled = Layouts.SelectedItems.Count == 1;
		}

        private void AddPrintLayout_Click(object sender, EventArgs e)
        {
            string resource = m_editor.BrowseResource("PrintLayout");
            if (resource != null)
            {
                if (m_command.PrintLayout == null)
                    m_command.PrintLayout = new OSGeo.MapGuide.MaestroAPI.ResourceReferenceTypeCollection();
                bool existing = false;
                foreach (OSGeo.MapGuide.MaestroAPI.ResourceReferenceType rf in m_command.PrintLayout)
                    if (rf.ResourceId == resource)
                    {
                        existing = true;
                        break;
                    }

                if (!existing)
                {
                    OSGeo.MapGuide.MaestroAPI.ResourceReferenceType rf = new OSGeo.MapGuide.MaestroAPI.ResourceReferenceType();
                    rf.ResourceId = resource;
                    m_command.PrintLayout.Add(rf);
                    Layouts.Items.Add(resource);
                }
            }
        }

        private void RemovePrintLayout_Click(object sender, EventArgs e)
        {
            if (Layouts.SelectedItems.Count == 1)
            {
                for (int i = 0; i < m_command.PrintLayout.Count; i++)
                    if (m_command.PrintLayout[i].ResourceId == (string)Layouts.SelectedItems[0])
                    {
                        m_command.PrintLayout.RemoveAt(i);
                        break;
                    }
                Layouts.Items.Remove(Layouts.SelectedItems[0]);
            }
        }
	}
}
