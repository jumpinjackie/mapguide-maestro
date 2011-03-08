namespace Maestro.Editors.Fusion.WidgetEditors
{
    partial class InvokeUrlWidgetCtrl
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
            this.baseEditor = new Maestro.Editors.Fusion.WidgetEditors.WidgetEditorBase();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdParams = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.chkDisableEmpty = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdParams)).BeginInit();
            this.SuspendLayout();
            // 
            // baseEditor
            // 
            this.baseEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.baseEditor.Location = new System.Drawing.Point(0, 0);
            this.baseEditor.Name = "baseEditor";
            this.baseEditor.Size = new System.Drawing.Size(446, 112);
            this.baseEditor.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtUrl);
            this.groupBox1.Controls.Add(this.txtTarget);
            this.groupBox1.Controls.Add(this.grdParams);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkDisableEmpty);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(4, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 201);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Invoke Url";
            // 
            // grdParams
            // 
            this.grdParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdParams.Location = new System.Drawing.Point(19, 107);
            this.grdParams.Name = "grdParams";
            this.grdParams.Size = new System.Drawing.Size(397, 75);
            this.grdParams.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Additional Parameters";
            // 
            // chkDisableEmpty
            // 
            this.chkDisableEmpty.AutoSize = true;
            this.chkDisableEmpty.Location = new System.Drawing.Point(89, 71);
            this.chkDisableEmpty.Name = "chkDisableEmpty";
            this.chkDisableEmpty.Size = new System.Drawing.Size(149, 17);
            this.chkDisableEmpty.TabIndex = 2;
            this.chkDisableEmpty.Text = "Disable If Selection Empty";
            this.chkDisableEmpty.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Url";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target";
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(89, 19);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(327, 20);
            this.txtTarget.TabIndex = 5;
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(89, 45);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(327, 20);
            this.txtUrl.TabIndex = 6;
            // 
            // InvokeUrlWidgetCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.baseEditor);
            this.Name = "InvokeUrlWidgetCtrl";
            this.Size = new System.Drawing.Size(446, 323);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdParams)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WidgetEditorBase baseEditor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDisableEmpty;
        private System.Windows.Forms.DataGridView grdParams;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtTarget;
    }
}
