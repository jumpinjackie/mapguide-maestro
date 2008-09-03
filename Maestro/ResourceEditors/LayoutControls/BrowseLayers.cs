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
		private Globalizator.Globalizator m_globalizor;

		public BrowseLayers()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new Globalizator.Globalizator(this);
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
			this.LayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LayerList.IntegralHeight = false;
			this.LayerList.Location = new System.Drawing.Point(8, 8);
			this.LayerList.Name = "LayerList";
			this.LayerList.Size = new System.Drawing.Size(360, 264);
			this.LayerList.TabIndex = 0;
			// 
			// OKBtn
			// 
			this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.OKBtn.Location = new System.Drawing.Point(88, 312);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(88, 24);
			this.OKBtn.TabIndex = 1;
			this.OKBtn.Text = "OK";
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(200, 312);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(88, 24);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "Cancel";
			// 
			// SelectAllButton
			// 
			this.SelectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.SelectAllButton.Location = new System.Drawing.Point(8, 272);
			this.SelectAllButton.Name = "SelectAllButton";
			this.SelectAllButton.Size = new System.Drawing.Size(88, 24);
			this.SelectAllButton.TabIndex = 3;
			this.SelectAllButton.Text = "Select all";
			this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
			// 
			// SelectNoneButton
			// 
			this.SelectNoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.SelectNoneButton.Location = new System.Drawing.Point(96, 272);
			this.SelectNoneButton.Name = "SelectNoneButton";
			this.SelectNoneButton.Size = new System.Drawing.Size(88, 24);
			this.SelectNoneButton.TabIndex = 4;
			this.SelectNoneButton.Text = "Select none";
			this.SelectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
			// 
			// SelectInverseButton
			// 
			this.SelectInverseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.SelectInverseButton.Location = new System.Drawing.Point(184, 272);
			this.SelectInverseButton.Name = "SelectInverseButton";
			this.SelectInverseButton.Size = new System.Drawing.Size(88, 24);
			this.SelectInverseButton.TabIndex = 5;
			this.SelectInverseButton.Text = "Select inverse";
			this.SelectInverseButton.Click += new System.EventHandler(this.SelectInverseButton_Click);
			// 
			// BrowseLayers
			// 
			this.AcceptButton = this.OKBtn;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(376, 349);
			this.Controls.Add(this.SelectInverseButton);
			this.Controls.Add(this.SelectNoneButton);
			this.Controls.Add(this.SelectAllButton);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.LayerList);
			this.MinimumSize = new System.Drawing.Size(288, 192);
			this.Name = "BrowseLayers";
			this.Text = "Select layers";
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
