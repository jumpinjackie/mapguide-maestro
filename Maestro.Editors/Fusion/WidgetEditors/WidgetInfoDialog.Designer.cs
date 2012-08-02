namespace Maestro.Editors.Fusion.WidgetEditors
{
    partial class WidgetInfoDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WidgetInfoDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grdExtensionProperties = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdExtensionProperties)).BeginInit();
            this.SuspendLayout();
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
            // grdExtensionProperties
            // 
            this.grdExtensionProperties.AllowUserToAddRows = false;
            this.grdExtensionProperties.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grdExtensionProperties, "grdExtensionProperties");
            this.grdExtensionProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdExtensionProperties.Name = "grdExtensionProperties";
            this.grdExtensionProperties.ReadOnly = true;
            // 
            // WidgetInfoDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.grdExtensionProperties);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "WidgetInfoDialog";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.grdExtensionProperties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grdExtensionProperties;
    }
}