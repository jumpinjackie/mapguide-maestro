using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    partial class FillStyleEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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

        public Maestro.Editors.Common.ImageStylePicker fillCombo;
        public System.Windows.Forms.CheckBox displayFill;
        private System.Windows.Forms.Label lblBackground;
        public System.Windows.Forms.Label lblForeground;
        private System.Windows.Forms.Label lblFill;
        public ColorExpressionField foregroundColor;
        public ColorExpressionField backgroundColor;
    }
}
