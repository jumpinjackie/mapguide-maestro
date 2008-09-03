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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Gdal
{
	/// <summary>
	/// Summary description for AddFolder.
	/// </summary>
	public class AddFolder : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel ButtonPanel;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Button OKBtn;
		public System.Windows.Forms.TextBox FileList;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddFolder()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ButtonPanel = new System.Windows.Forms.Panel();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.OKBtn = new System.Windows.Forms.Button();
			this.FileList = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ButtonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonPanel
			// 
			this.ButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonPanel.Controls.Add(this.CancelBtn);
			this.ButtonPanel.Controls.Add(this.OKBtn);
			this.ButtonPanel.Location = new System.Drawing.Point(10, 36);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(500, 24);
			this.ButtonPanel.TabIndex = 17;
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelBtn.Location = new System.Drawing.Point(258, 0);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(96, 24);
			this.CancelBtn.TabIndex = 13;
			this.CancelBtn.Text = "Cancel";
			// 
			// OKBtn
			// 
			this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.OKBtn.Enabled = false;
			this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKBtn.Location = new System.Drawing.Point(146, 0);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(96, 24);
			this.OKBtn.TabIndex = 12;
			this.OKBtn.Text = "OK";
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// FileList
			// 
			this.FileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FileList.Location = new System.Drawing.Point(104, 8);
			this.FileList.Name = "FileList";
			this.FileList.Size = new System.Drawing.Size(400, 20);
			this.FileList.TabIndex = 16;
			this.FileList.Text = "";
			this.FileList.TextChanged += new System.EventHandler(this.FileList_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 16);
			this.label1.TabIndex = 15;
			this.label1.Text = "Folder path";
			// 
			// AddFolder
			// 
			this.AcceptButton = this.OKBtn;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 69);
			this.Controls.Add(this.ButtonPanel);
			this.Controls.Add(this.FileList);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddFolder";
			this.Text = "Add folder";
			this.ButtonPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void FileList_TextChanged(object sender, System.EventArgs e)
		{
			OKBtn.Enabled = FileList.Text.Trim().Length > 0;
		}
	}
}
