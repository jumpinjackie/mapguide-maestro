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
	/// Summary description for WidgetEditorUI.
	/// </summary>
	public class WidgetEditorUI : BasisWidgetEditor
	{
		private System.Windows.Forms.TextBox TooltipText;
		private System.Windows.Forms.CheckBox DisabledCheck;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox ImageClassText;
		private System.Windows.Forms.TextBox ImageURLText;
		private System.Windows.Forms.TextBox LabelText;
		private System.Windows.Forms.TextBox StatusText;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public WidgetEditorUI()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public override void SetItem(WidgetType w)
		{
			m_w = w;
			if (w.GetType() == typeof(UiWidgetType))
			{
				this.Visible = true;
				if (((UiWidgetType)w).Disabled == null)
					((UiWidgetType)w).Disabled = false.ToString();

				DisabledCheck.Checked = bool.Parse(((UiWidgetType)w).Disabled);
				ImageClassText.Text = ((UiWidgetType)w).ImageClass;
				ImageURLText.Text = ((UiWidgetType)w).ImageUrl;
				LabelText.Text = ((UiWidgetType)w).Label;
				StatusText.Text = ((UiWidgetType)w).StatusText;
				TooltipText.Text = ((UiWidgetType)w).Tooltip;
			}
			else
				this.Visible = false;
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

		private void DisabledCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			((UiWidgetType)m_w).Disabled = DisabledCheck.Checked.ToString().ToLower();
			RaiseValueChanged();
		}

		private void ImageClassText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			((UiWidgetType)m_w).ImageClass = ImageClassText.Text;
			RaiseValueChanged();
		}

		private void ImageURLText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			((UiWidgetType)m_w).ImageUrl = ImageURLText.Text;
			RaiseValueChanged();
		}

		private void LabelText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			((UiWidgetType)m_w).Label = LabelText.Text;
			RaiseValueChanged();
		}

		private void StatusText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			((UiWidgetType)m_w).StatusText = StatusText.Text;
			RaiseValueChanged();
		}

		private void TooltipText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			((UiWidgetType)m_w).Tooltip = TooltipText.Text;
			RaiseValueChanged();
		}



		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.TooltipText = new System.Windows.Forms.TextBox();
			this.DisabledCheck = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.ImageClassText = new System.Windows.Forms.TextBox();
			this.ImageURLText = new System.Windows.Forms.TextBox();
			this.LabelText = new System.Windows.Forms.TextBox();
			this.StatusText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// TooltipText
			// 
			this.TooltipText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TooltipText.Location = new System.Drawing.Point(100, 128);
			this.TooltipText.Name = "TooltipText";
			this.TooltipText.Size = new System.Drawing.Size(288, 20);
			this.TooltipText.TabIndex = 33;
			this.TooltipText.Text = "textBox1";
			this.TooltipText.TextChanged += new System.EventHandler(this.TooltipText_TextChanged);
			// 
			// DisabledCheck
			// 
			this.DisabledCheck.Location = new System.Drawing.Point(100, 8);
			this.DisabledCheck.Name = "DisabledCheck";
			this.DisabledCheck.Size = new System.Drawing.Size(112, 16);
			this.DisabledCheck.TabIndex = 23;
			this.DisabledCheck.Text = "Disabled";
			this.DisabledCheck.TextChanged += new System.EventHandler(this.DisabledCheck_CheckedChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 24;
			this.label4.Text = "Image class";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(4, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 16);
			this.label5.TabIndex = 25;
			this.label5.Text = "Image url";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(4, 104);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 27;
			this.label6.Text = "Status text";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(4, 80);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(88, 16);
			this.label7.TabIndex = 26;
			this.label7.Text = "Label";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(4, 128);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 16);
			this.label8.TabIndex = 28;
			this.label8.Text = "Tooltip";
			// 
			// ImageClassText
			// 
			this.ImageClassText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ImageClassText.Location = new System.Drawing.Point(100, 32);
			this.ImageClassText.Name = "ImageClassText";
			this.ImageClassText.Size = new System.Drawing.Size(288, 20);
			this.ImageClassText.TabIndex = 29;
			this.ImageClassText.Text = "textBox1";
			this.ImageClassText.TextChanged += new System.EventHandler(this.ImageClassText_TextChanged);
			// 
			// ImageURLText
			// 
			this.ImageURLText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ImageURLText.Location = new System.Drawing.Point(100, 56);
			this.ImageURLText.Name = "ImageURLText";
			this.ImageURLText.Size = new System.Drawing.Size(288, 20);
			this.ImageURLText.TabIndex = 30;
			this.ImageURLText.Text = "textBox1";
			this.ImageURLText.TextChanged += new System.EventHandler(this.ImageURLText_TextChanged);
			// 
			// LabelText
			// 
			this.LabelText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LabelText.Location = new System.Drawing.Point(100, 80);
			this.LabelText.Name = "LabelText";
			this.LabelText.Size = new System.Drawing.Size(288, 20);
			this.LabelText.TabIndex = 31;
			this.LabelText.Text = "textBox1";
			this.LabelText.TextChanged += new System.EventHandler(this.LabelText_TextChanged);
			// 
			// StatusText
			// 
			this.StatusText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.StatusText.Location = new System.Drawing.Point(100, 104);
			this.StatusText.Name = "StatusText";
			this.StatusText.Size = new System.Drawing.Size(288, 20);
			this.StatusText.TabIndex = 32;
			this.StatusText.Text = "textBox1";
			this.StatusText.TextChanged += new System.EventHandler(this.StatusText_TextChanged);
			// 
			// WidgetEditorUI
			// 
			this.Controls.Add(this.TooltipText);
			this.Controls.Add(this.DisabledCheck);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.ImageClassText);
			this.Controls.Add(this.ImageURLText);
			this.Controls.Add(this.LabelText);
			this.Controls.Add(this.StatusText);
			this.Name = "WidgetEditorUI";
			this.Size = new System.Drawing.Size(392, 160);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
