namespace Maestro.Editors.MapDefinition
{
    partial class ExtentCalculationDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtentCalculationDialog));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.grdCalculations = new System.Windows.Forms.DataGridView();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.prgCalculations = new System.Windows.Forms.ProgressBar();
            this.bgCalculation = new System.ComponentModel.BackgroundWorker();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCoordinateSystem = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCalculations)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
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
            // txtUpperY
            // 
            resources.ApplyResources(this.txtUpperY, "txtUpperY");
            this.txtUpperY.Name = "txtUpperY";
            this.txtUpperY.ReadOnly = true;
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
            this.txtUpperX.ReadOnly = true;
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
            this.txtLowerY.ReadOnly = true;
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
            this.txtLowerX.ReadOnly = true;
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
            // grdCalculations
            // 
            this.grdCalculations.AllowUserToAddRows = false;
            this.grdCalculations.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grdCalculations, "grdCalculations");
            this.grdCalculations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCalculations.Name = "grdCalculations";
            this.grdCalculations.ReadOnly = true;
            // 
            // btnAccept
            // 
            resources.ApplyResources(this.btnAccept, "btnAccept");
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // prgCalculations
            // 
            resources.ApplyResources(this.prgCalculations, "prgCalculations");
            this.prgCalculations.Name = "prgCalculations";
            // 
            // bgCalculation
            // 
            this.bgCalculation.WorkerReportsProgress = true;
            this.bgCalculation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgCalculation_DoWork);
            this.bgCalculation.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgCalculation_RunWorkerCompleted);
            this.bgCalculation.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgCalculation_ProgressChanged);
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.Name = "lblMessage";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtCoordinateSystem
            // 
            resources.ApplyResources(this.txtCoordinateSystem, "txtCoordinateSystem");
            this.txtCoordinateSystem.Name = "txtCoordinateSystem";
            this.txtCoordinateSystem.ReadOnly = true;
            // 
            // ExtentCalculationDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.txtCoordinateSystem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.prgCalculations);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.grdCalculations);
            this.Controls.Add(this.groupBox2);
            this.Name = "ExtentCalculationDialog";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCalculations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
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
        private System.Windows.Forms.DataGridView grdCalculations;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar prgCalculations;
        private System.ComponentModel.BackgroundWorker bgCalculation;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCoordinateSystem;

    }
}