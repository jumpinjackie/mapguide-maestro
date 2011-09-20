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
	/// Summary description for FillStyleEditor.
	/// </summary>
    [ToolboxItem(false)]
	internal class FillStyleEditor : System.Windows.Forms.UserControl
    {
		public ImageStylePicker fillCombo;

		public System.Windows.Forms.CheckBox displayFill;
        private System.Windows.Forms.Label lblBackground;
        public Label lblForeground;
        private System.Windows.Forms.Label lblFill;
        public ColorExpressionField foregroundColor;
        public ColorExpressionField backgroundColor;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FillStyleEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
        
            fillCombo.Items.Clear();
            fillCombo.Items.AddRange(FeaturePreviewRender.FillImages);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FillStyleEditor));
            this.lblBackground = new System.Windows.Forms.Label();
            this.lblForeground = new System.Windows.Forms.Label();
            this.lblFill = new System.Windows.Forms.Label();
            this.fillCombo = new Maestro.Editors.Common.ImageStylePicker();
            this.displayFill = new System.Windows.Forms.CheckBox();
            this.foregroundColor = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.ColorExpressionField();
            this.backgroundColor = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.ColorExpressionField();
            this.SuspendLayout();
            // 
            // lblBackground
            // 
            resources.ApplyResources(this.lblBackground, "lblBackground");
            this.lblBackground.Name = "lblBackground";
            // 
            // lblForeground
            // 
            resources.ApplyResources(this.lblForeground, "lblForeground");
            this.lblForeground.Name = "lblForeground";
            // 
            // lblFill
            // 
            resources.ApplyResources(this.lblFill, "lblFill");
            this.lblFill.Name = "lblFill";
            // 
            // fillCombo
            // 
            resources.ApplyResources(this.fillCombo, "fillCombo");
            this.fillCombo.DisplayMember = "Name";
            this.fillCombo.Name = "fillCombo";
            this.fillCombo.TextWidth = 50;
            this.fillCombo.ValueMember = "Name";
            // 
            // displayFill
            // 
            this.displayFill.Checked = true;
            this.displayFill.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.displayFill, "displayFill");
            this.displayFill.Name = "displayFill";
            this.displayFill.CheckedChanged += new System.EventHandler(this.displayFill_CheckedChanged);
            // 
            // foregroundColor
            // 
            resources.ApplyResources(this.foregroundColor, "foregroundColor");
            this.foregroundColor.ColorExpression = "";
            this.foregroundColor.Name = "foregroundColor";
            this.foregroundColor.RequestExpressionEditor += new System.EventHandler(this.foregroundColor_RequestExpressionEditor);
            // 
            // backgroundColor
            // 
            resources.ApplyResources(this.backgroundColor, "backgroundColor");
            this.backgroundColor.ColorExpression = "";
            this.backgroundColor.Name = "backgroundColor";
            this.backgroundColor.RequestExpressionEditor += new System.EventHandler(this.backgroundColor_RequestExpressionEditor);
            // 
            // FillStyleEditor
            // 
            this.Controls.Add(this.backgroundColor);
            this.Controls.Add(this.foregroundColor);
            this.Controls.Add(this.displayFill);
            this.Controls.Add(this.fillCombo);
            this.Controls.Add(this.lblBackground);
            this.Controls.Add(this.lblForeground);
            this.Controls.Add(this.lblFill);
            this.Name = "FillStyleEditor";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.FillStyleEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void FillStyleEditor_Load(object sender, System.EventArgs e)
		{
		}

		private void displayFill_CheckedChanged(object sender, System.EventArgs e)
		{
			lblFill.Enabled = 
			lblForeground.Enabled = 
			lblBackground.Enabled = 
			fillCombo.Enabled =
			foregroundColor.Enabled =
			backgroundColor.Enabled = 
				displayFill.Checked;
		}

        public event EventHandler ForegroundRequiresExpression;
        public event EventHandler BackgroundRequiresExpression;


        private void foregroundColor_RequestExpressionEditor(object sender, EventArgs e)
        {
            var handler = this.ForegroundRequiresExpression;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void backgroundColor_RequestExpressionEditor(object sender, EventArgs e)
        {
            var handler = this.BackgroundRequiresExpression;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
	}
}
