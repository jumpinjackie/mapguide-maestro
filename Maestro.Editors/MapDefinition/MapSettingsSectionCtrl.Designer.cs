namespace Maestro.Editors.MapDefinition
{
    partial class MapSettingsSectionCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSettingsSectionCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSetZoom = new System.Windows.Forms.Button();
            this.txtUpperY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUpperX = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLowerY = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLowerX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtCoordinateSystem = new System.Windows.Forms.TextBox();
            this.btnPickCs = new System.Windows.Forms.Button();
            this.cmbBackgroundColor = new Maestro.Editors.Common.ColorComboBox();
            this.contentPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.cmbBackgroundColor);
            this.contentPanel.Controls.Add(this.btnPickCs);
            this.contentPanel.Controls.Add(this.txtCoordinateSystem);
            this.contentPanel.Controls.Add(this.txtDescription);
            this.contentPanel.Controls.Add(this.groupBox2);
            this.contentPanel.Controls.Add(this.label3);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.btnSetZoom);
            this.groupBox2.Controls.Add(this.txtUpperY);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtUpperX);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtLowerY);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtLowerX);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btnSetZoom
            // 
            resources.ApplyResources(this.btnSetZoom, "btnSetZoom");
            this.btnSetZoom.Name = "btnSetZoom";
            this.btnSetZoom.Click += new System.EventHandler(this.btnSetZoom_Click);
            // 
            // txtUpperY
            // 
            resources.ApplyResources(this.txtUpperY, "txtUpperY");
            this.txtUpperY.Name = "txtUpperY";
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // txtUpperX
            // 
            resources.ApplyResources(this.txtUpperX, "txtUpperX");
            this.txtUpperX.Name = "txtUpperX";
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txtLowerY
            // 
            resources.ApplyResources(this.txtLowerY, "txtLowerY");
            this.txtLowerY.Name = "txtLowerY";
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtLowerX
            // 
            resources.ApplyResources(this.txtLowerX, "txtLowerX");
            this.txtLowerX.Name = "txtLowerX";
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // txtDescription
            // 
            resources.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Name = "txtDescription";
            // 
            // txtCoordinateSystem
            // 
            resources.ApplyResources(this.txtCoordinateSystem, "txtCoordinateSystem");
            this.txtCoordinateSystem.Name = "txtCoordinateSystem";
            this.txtCoordinateSystem.ReadOnly = true;
            // 
            // btnPickCs
            // 
            resources.ApplyResources(this.btnPickCs, "btnPickCs");
            this.btnPickCs.Name = "btnPickCs";
            this.btnPickCs.UseVisualStyleBackColor = true;
            this.btnPickCs.Click += new System.EventHandler(this.btnPickCs_Click);
            // 
            // cmbBackgroundColor
            // 
            this.cmbBackgroundColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBackgroundColor, "cmbBackgroundColor");
            this.cmbBackgroundColor.Name = "cmbBackgroundColor";
            // 
            // MapSettingsSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Map Settings";
            this.Name = "MapSettingsSectionCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCoordinateSystem;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSetZoom;
        private System.Windows.Forms.TextBox txtUpperY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUpperX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLowerY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLowerX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnPickCs;
        private Maestro.Editors.Common.ColorComboBox cmbBackgroundColor;
    }
}
