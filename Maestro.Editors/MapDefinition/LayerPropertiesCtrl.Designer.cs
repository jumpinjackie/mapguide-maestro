namespace Maestro.Editors.MapDefinition
{
    partial class LayerPropertiesCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerPropertiesCtrl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtResourceId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSelectable = new System.Windows.Forms.CheckBox();
            this.chkExpanded = new System.Windows.Forms.CheckBox();
            this.chkLegendVisible = new System.Windows.Forms.CheckBox();
            this.chkVisible = new System.Windows.Forms.CheckBox();
            this.txtLegendLabel = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtResourceId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkSelectable);
            this.groupBox1.Controls.Add(this.chkExpanded);
            this.groupBox1.Controls.Add(this.chkLegendVisible);
            this.groupBox1.Controls.Add(this.chkVisible);
            this.groupBox1.Controls.Add(this.txtLegendLabel);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtResourceId
            // 
            resources.ApplyResources(this.txtResourceId, "txtResourceId");
            this.txtResourceId.Name = "txtResourceId";
            this.txtResourceId.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkSelectable
            // 
            resources.ApplyResources(this.chkSelectable, "chkSelectable");
            this.chkSelectable.Name = "chkSelectable";
            this.chkSelectable.UseVisualStyleBackColor = true;
            // 
            // chkExpanded
            // 
            resources.ApplyResources(this.chkExpanded, "chkExpanded");
            this.chkExpanded.Name = "chkExpanded";
            this.chkExpanded.UseVisualStyleBackColor = true;
            // 
            // chkLegendVisible
            // 
            resources.ApplyResources(this.chkLegendVisible, "chkLegendVisible");
            this.chkLegendVisible.Name = "chkLegendVisible";
            this.chkLegendVisible.UseVisualStyleBackColor = true;
            // 
            // chkVisible
            // 
            resources.ApplyResources(this.chkVisible, "chkVisible");
            this.chkVisible.Name = "chkVisible";
            this.chkVisible.UseVisualStyleBackColor = true;
            // 
            // txtLegendLabel
            // 
            resources.ApplyResources(this.txtLegendLabel, "txtLegendLabel");
            this.txtLegendLabel.Name = "txtLegendLabel";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
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
            // LayerPropertiesCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Name = "LayerPropertiesCtrl";
            resources.ApplyResources(this, "$this");
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkExpanded;
        private System.Windows.Forms.CheckBox chkLegendVisible;
        private System.Windows.Forms.CheckBox chkVisible;
        private System.Windows.Forms.TextBox txtLegendLabel;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSelectable;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtResourceId;
        private System.Windows.Forms.Label label3;
    }
}
