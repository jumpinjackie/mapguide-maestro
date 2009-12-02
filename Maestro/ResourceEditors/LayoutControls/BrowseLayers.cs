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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls
{
	/// <summary>
	/// Summary description for BrowseLayers.
	/// </summary>
	public class BrowseLayers : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Button SelectAllButton;
		private System.Windows.Forms.Button SelectNoneButton;
		private System.Windows.Forms.Button SelectInverseButton;
		public System.Windows.Forms.CheckedListBox LayerList;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BrowseLayers()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseLayers));
            this.LayerList = new System.Windows.Forms.CheckedListBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.SelectNoneButton = new System.Windows.Forms.Button();
            this.SelectInverseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LayerList
            // 
            resources.ApplyResources(this.LayerList, "LayerList");
            this.LayerList.Name = "LayerList";
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            // 
            // SelectAllButton
            // 
            resources.ApplyResources(this.SelectAllButton, "SelectAllButton");
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // SelectNoneButton
            // 
            resources.ApplyResources(this.SelectNoneButton, "SelectNoneButton");
            this.SelectNoneButton.Name = "SelectNoneButton";
            this.SelectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
            // 
            // SelectInverseButton
            // 
            resources.ApplyResources(this.SelectInverseButton, "SelectInverseButton");
            this.SelectInverseButton.Name = "SelectInverseButton";
            this.SelectInverseButton.Click += new System.EventHandler(this.SelectInverseButton_Click);
            // 
            // BrowseLayers
            // 
            this.AcceptButton = this.OKBtn;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.SelectInverseButton);
            this.Controls.Add(this.SelectNoneButton);
            this.Controls.Add(this.SelectAllButton);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.LayerList);
            this.Name = "BrowseLayers";
            this.ResumeLayout(false);

		}
		#endregion

		private void SelectAllButton_Click(object sender, System.EventArgs e)
		{
			for(int i = 0; i < LayerList.Items.Count; i++)
				LayerList.SetItemChecked(i, true);
		}

		private void SelectNoneButton_Click(object sender, System.EventArgs e)
		{
			for(int i = 0; i < LayerList.Items.Count; i++)
				LayerList.SetItemChecked(i, false);
		}

		private void SelectInverseButton_Click(object sender, System.EventArgs e)
		{
			for(int i = 0; i < LayerList.Items.Count; i++)
				LayerList.SetItemChecked(i, !LayerList.GetItemChecked(i));
		}

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}



	}
}
