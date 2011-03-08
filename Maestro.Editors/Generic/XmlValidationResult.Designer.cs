namespace Maestro.Editors.Generic
{
    partial class XmlValidationResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XmlValidationResult));
            this.btnOK = new System.Windows.Forms.Button();
            this.grdMessages = new System.Windows.Forms.DataGridView();
            this.COL_ICON = new System.Windows.Forms.DataGridViewImageColumn();
            this.COL_MESSAGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grdMessages
            // 
            this.grdMessages.AllowUserToAddRows = false;
            this.grdMessages.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grdMessages, "grdMessages");
            this.grdMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_ICON,
            this.COL_MESSAGE});
            this.grdMessages.Name = "grdMessages";
            this.grdMessages.ReadOnly = true;
            this.grdMessages.RowHeadersVisible = false;
            // 
            // COL_ICON
            // 
            this.COL_ICON.DataPropertyName = "Icon";
            resources.ApplyResources(this.COL_ICON, "COL_ICON");
            this.COL_ICON.Name = "COL_ICON";
            this.COL_ICON.ReadOnly = true;
            // 
            // COL_MESSAGE
            // 
            this.COL_MESSAGE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_MESSAGE.DataPropertyName = "Message";
            resources.ApplyResources(this.COL_MESSAGE, "COL_MESSAGE");
            this.COL_MESSAGE.Name = "COL_MESSAGE";
            this.COL_MESSAGE.ReadOnly = true;
            // 
            // XmlValidationResult
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grdMessages);
            this.Controls.Add(this.btnOK);
            this.Name = "XmlValidationResult";
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView grdMessages;
        private System.Windows.Forms.DataGridViewImageColumn COL_ICON;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_MESSAGE;
    }
}