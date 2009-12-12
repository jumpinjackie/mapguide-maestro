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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// Summary description for LineStyleEditor.
	/// </summary>
	public class LineStyleEditor : System.Windows.Forms.UserControl
	{
        public ResourceEditors.GeometryStyleEditors.ImageStylePicker fillCombo;
        public Label lblColor;
		private System.Windows.Forms.Label lblThickness;
		private System.Windows.Forms.Label lblFill;
		public System.Windows.Forms.CheckBox displayLine;
		private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.NumericUpDown thicknessUpDown;
        private Label label1;
        public ColorComboWithTransparency colorCombo;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LineStyleEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineStyleEditor));
            this.fillCombo = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ImageStylePicker();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblThickness = new System.Windows.Forms.Label();
            this.lblFill = new System.Windows.Forms.Label();
            this.displayLine = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.thicknessUpDown = new System.Windows.Forms.NumericUpDown();
            this.colorCombo = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboWithTransparency();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // fillCombo
            // 
            resources.ApplyResources(this.fillCombo, "fillCombo");
            this.fillCombo.DisplayMember = "Name";
            this.fillCombo.Name = "fillCombo";
            this.fillCombo.TextWidth = 40;
            this.fillCombo.ValueMember = "Name";
            // 
            // lblColor
            // 
            resources.ApplyResources(this.lblColor, "lblColor");
            this.lblColor.Name = "lblColor";
            // 
            // lblThickness
            // 
            resources.ApplyResources(this.lblThickness, "lblThickness");
            this.lblThickness.Name = "lblThickness";
            // 
            // lblFill
            // 
            resources.ApplyResources(this.lblFill, "lblFill");
            this.lblFill.Name = "lblFill";
            // 
            // displayLine
            // 
            this.displayLine.Checked = true;
            this.displayLine.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.displayLine, "displayLine");
            this.displayLine.Name = "displayLine";
            this.displayLine.CheckedChanged += new System.EventHandler(this.displayLine_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.colorCombo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.thicknessUpDown);
            this.panel1.Controls.Add(this.fillCombo);
            this.panel1.Controls.Add(this.lblColor);
            this.panel1.Controls.Add(this.lblThickness);
            this.panel1.Controls.Add(this.lblFill);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // thicknessUpDown
            // 
            resources.ApplyResources(this.thicknessUpDown, "thicknessUpDown");
            this.thicknessUpDown.Name = "thicknessUpDown";
            // 
            // colorCombo
            // 
            resources.ApplyResources(this.colorCombo, "colorCombo");
            this.colorCombo.CurrentColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorCombo.Name = "colorCombo";
            // 
            // LineStyleEditor
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.displayLine);
            this.Name = "LineStyleEditor";
            resources.ApplyResources(this, "$this");
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
            panel1.Enabled = displayLine.Checked;
		}

	}
}
