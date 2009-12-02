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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlyoutEditor));
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
            resources.ApplyResources(this.NameBox, "NameBox");
            this.NameBox.Name = "NameBox";
            this.NameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ImageClassText
            // 
            resources.ApplyResources(this.ImageClassText, "ImageClassText");
            this.ImageClassText.Name = "ImageClassText";
            this.ImageClassText.TextChanged += new System.EventHandler(this.ImageClassText_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // ImageURLText
            // 
            resources.ApplyResources(this.ImageURLText, "ImageURLText");
            this.ImageURLText.Name = "ImageURLText";
            this.ImageURLText.TextChanged += new System.EventHandler(this.ImageURLText_TextChanged);
            // 
            // TooltipBox
            // 
            resources.ApplyResources(this.TooltipBox, "TooltipBox");
            this.TooltipBox.Name = "TooltipBox";
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
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

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
