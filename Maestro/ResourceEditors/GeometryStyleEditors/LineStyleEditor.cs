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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// Summary description for LineStyleEditor.
	/// </summary>
	public class LineStyleEditor : System.Windows.Forms.UserControl
	{
		public ResourceEditors.GeometryStyleEditors.ImageStylePicker fillCombo;

		public ResourceEditors.GeometryStyleEditors.ColorComboBox colorCombo;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.Label lblThickness;
		private System.Windows.Forms.Label lblFill;
		public System.Windows.Forms.CheckBox displayLine;
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.NumericUpDown thicknessUpDown;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LineStyleEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            colorCombo.AllowTransparent = false;
            colorCombo.ResetColors();

            fillCombo.Items.Clear();
            fillCombo.Items.AddRange(FeaturePreviewRender.LineStyles);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LineStyleEditor));
			this.colorCombo = new ResourceEditors.GeometryStyleEditors.ColorComboBox();
			this.fillCombo = new ResourceEditors.GeometryStyleEditors.ImageStylePicker();
			this.lblColor = new System.Windows.Forms.Label();
			this.lblThickness = new System.Windows.Forms.Label();
			this.lblFill = new System.Windows.Forms.Label();
			this.displayLine = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.thicknessUpDown = new System.Windows.Forms.NumericUpDown();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.thicknessUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// colorCombo
			// 
			this.colorCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));

			this.colorCombo.Location = new System.Drawing.Point(120, 64);
			this.colorCombo.Name = "colorCombo";
			this.colorCombo.Size = new System.Drawing.Size(312, 21);
			this.colorCombo.TabIndex = 11;
			// 
			// fillCombo
			// 
			this.fillCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.fillCombo.DisplayMember = "Name";
			this.fillCombo.Location = new System.Drawing.Point(120, 0);
			this.fillCombo.Name = "fillCombo";
			this.fillCombo.Size = new System.Drawing.Size(312, 21);
			this.fillCombo.TabIndex = 9;
			this.fillCombo.TextWidth = 40;
			this.fillCombo.ValueMember = "Name";
			// 
			// lblColor
			// 
			this.lblColor.Location = new System.Drawing.Point(0, 64);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(96, 16);
			this.lblColor.TabIndex = 8;
			this.lblColor.Text = "Color";
			// 
			// lblThickness
			// 
			this.lblThickness.Location = new System.Drawing.Point(0, 32);
			this.lblThickness.Name = "lblThickness";
			this.lblThickness.Size = new System.Drawing.Size(96, 16);
			this.lblThickness.TabIndex = 7;
			this.lblThickness.Text = "Thickness";
			// 
			// lblFill
			// 
			this.lblFill.Location = new System.Drawing.Point(0, 8);
			this.lblFill.Name = "lblFill";
			this.lblFill.Size = new System.Drawing.Size(96, 16);
			this.lblFill.TabIndex = 6;
			this.lblFill.Text = "Line style";
			// 
			// displayLine
			// 
			this.displayLine.Checked = true;
			this.displayLine.CheckState = System.Windows.Forms.CheckState.Checked;
			this.displayLine.Dock = System.Windows.Forms.DockStyle.Top;
			this.displayLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.displayLine.Location = new System.Drawing.Point(0, 0);
			this.displayLine.Name = "displayLine";
			this.displayLine.Size = new System.Drawing.Size(432, 16);
			this.displayLine.TabIndex = 14;
			this.displayLine.Text = "Display edge";
			this.displayLine.CheckedChanged += new System.EventHandler(this.displayLine_CheckedChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.thicknessUpDown);
			this.panel1.Controls.Add(this.fillCombo);
			this.panel1.Controls.Add(this.lblColor);
			this.panel1.Controls.Add(this.lblThickness);
			this.panel1.Controls.Add(this.lblFill);
			this.panel1.Controls.Add(this.colorCombo);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(432, 88);
			this.panel1.TabIndex = 15;
			// 
			// thicknessUpDown
			// 
			this.thicknessUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.thicknessUpDown.Location = new System.Drawing.Point(120, 32);
			this.thicknessUpDown.Name = "thicknessUpDown";
			this.thicknessUpDown.Size = new System.Drawing.Size(312, 20);
			this.thicknessUpDown.TabIndex = 12;
			// 
			// LineStyleEditor
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.displayLine);
			this.Name = "LineStyleEditor";
			this.Size = new System.Drawing.Size(432, 104);
			this.Load += new System.EventHandler(this.LineStyleEditor_Load);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.thicknessUpDown)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void LineStyleEditor_Load(object sender, System.EventArgs e)
		{
		}

		private void displayLine_CheckedChanged(object sender, System.EventArgs e)
		{
			lblFill.Enabled =
			lblThickness.Enabled =
			lblColor.Enabled = 
			fillCombo.Enabled = 
			thicknessUpDown.Enabled = 
			colorCombo.Enabled = 
				displayLine.Checked;

		
		}

	}
}
