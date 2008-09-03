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
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor
{
	/// <summary>
	/// Summary description for FlyoutEditor.
	/// </summary>
	public class FlyoutEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox NameBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox TooltipBox;
		private System.Windows.Forms.TextBox ImageURLText;
		private System.Windows.Forms.TextBox ImageClassText;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private FlyoutItemType m_w = null;
		private bool m_isUpdating = false;
		public event ValueChangedDelegate ValueChanged;

		public FlyoutEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		public void SetItem(FlyoutItemType w)
		{
			try
			{
				m_isUpdating = true;
				m_w = w;
				NameBox.Text = w.Label;
				TooltipBox.Text = w.Tooltip;
				ImageURLText.Text = w.ImageUrl;
				ImageClassText.Text = w.ImageClass;
			}
			finally
			{
				m_isUpdating = false;
			}
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.NameBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.ImageClassText = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.ImageURLText = new System.Windows.Forms.TextBox();
			this.TooltipBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// NameBox
			// 
			this.NameBox.Location = new System.Drawing.Point(100, 10);
			this.NameBox.Name = "NameBox";
			this.NameBox.Size = new System.Drawing.Size(288, 20);
			this.NameBox.TabIndex = 9;
			this.NameBox.Text = "textBox1";
			this.NameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 58);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Image url";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Tooltip";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Name";
			// 
			// ImageClassText
			// 
			this.ImageClassText.Location = new System.Drawing.Point(100, 80);
			this.ImageClassText.Name = "ImageClassText";
			this.ImageClassText.Size = new System.Drawing.Size(288, 20);
			this.ImageClassText.TabIndex = 13;
			this.ImageClassText.Text = "textBox1";
			this.ImageClassText.TextChanged += new System.EventHandler(this.ImageClassText_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(84, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Image class";
			// 
			// ImageURLText
			// 
			this.ImageURLText.Location = new System.Drawing.Point(100, 56);
			this.ImageURLText.Name = "ImageURLText";
			this.ImageURLText.Size = new System.Drawing.Size(288, 20);
			this.ImageURLText.TabIndex = 14;
			this.ImageURLText.Text = "textBox1";
			this.ImageURLText.TextChanged += new System.EventHandler(this.ImageURLText_TextChanged);
			// 
			// TooltipBox
			// 
			this.TooltipBox.Location = new System.Drawing.Point(100, 32);
			this.TooltipBox.Name = "TooltipBox";
			this.TooltipBox.Size = new System.Drawing.Size(288, 20);
			this.TooltipBox.TabIndex = 15;
			this.TooltipBox.Text = "textBox1";
			this.TooltipBox.TextChanged += new System.EventHandler(this.TooltipBox_TextChanged);
			// 
			// FlyoutEditor
			// 
			this.Controls.Add(this.TooltipBox);
			this.Controls.Add(this.ImageURLText);
			this.Controls.Add(this.ImageClassText);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.NameBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "FlyoutEditor";
			this.Size = new System.Drawing.Size(392, 112);
			this.ResumeLayout(false);

		}
		#endregion

		private void NameBox_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			m_w.Label = NameBox.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_w);
		}

		private void TooltipBox_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			m_w.Tooltip = TooltipBox.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_w);
		}

		private void ImageURLText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			m_w.ImageUrl = ImageURLText.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_w);
		}

		private void ImageClassText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			m_w.ImageClass = ImageClassText.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_w);
		}
	}
}
