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
using Maestro.Editors.Common;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
	/// <summary>
	/// Summary description for LineStyleEditor.
	/// </summary>
    [ToolboxItem(false)]
	internal class LineStyleEditor : System.Windows.Forms.UserControl
	{
        public ImageStylePicker fillCombo;
        public Label lblColor;
		private System.Windows.Forms.Label lblThickness;
		private System.Windows.Forms.Label lblFill;
		public System.Windows.Forms.CheckBox displayLine;
        private System.Windows.Forms.Panel panel1;
        public ComboBox thicknessCombo;
        public ColorExpressionField colorCombo;

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
            this.fillCombo = new Maestro.Editors.Common.ImageStylePicker();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblThickness = new System.Windows.Forms.Label();
            this.lblFill = new System.Windows.Forms.Label();
            this.displayLine = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.thicknessCombo = new System.Windows.Forms.ComboBox();
            this.colorCombo = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.ColorExpressionField();
            this.panel1.SuspendLayout();
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
            this.panel1.Controls.Add(this.thicknessCombo);
            this.panel1.Controls.Add(this.colorCombo);
            this.panel1.Controls.Add(this.fillCombo);
            this.panel1.Controls.Add(this.lblColor);
            this.panel1.Controls.Add(this.lblThickness);
            this.panel1.Controls.Add(this.lblFill);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // thicknessCombo
            // 
            resources.ApplyResources(this.thicknessCombo, "thicknessCombo");
            this.thicknessCombo.FormattingEnabled = true;
            this.thicknessCombo.Items.AddRange(new object[] {
            resources.GetString("thicknessCombo.Items")});
            this.thicknessCombo.Name = "thicknessCombo";
            // 
            // colorCombo
            // 
            resources.ApplyResources(this.colorCombo, "colorCombo");
            this.colorCombo.ColorExpression = "";
            this.colorCombo.Name = "colorCombo";
            this.colorCombo.RequestExpressionEditor += new System.EventHandler(this.colorCombo_RequestExpressionEditor);
            // 
            // LineStyleEditor
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.displayLine);
            this.Name = "LineStyleEditor";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.LineStyleEditor_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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

        public event EventHandler RequiresExpressionEditor;

        private void colorCombo_RequestExpressionEditor(object sender, EventArgs e)
        {
            var handler = this.RequiresExpressionEditor;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public string ColorExpression
        {
            get { return colorCombo.ColorExpression; }
            set { colorCombo.ColorExpression = value; }
        }
	}
}
