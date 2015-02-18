namespace Maestro.Editors.TileSetDefinition.Providers
{
    partial class GenericProviderCtrl
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
            this.grdSettings = new System.Windows.Forms.DataGridView();
            this.COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_VALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // grdSettings
            // 
            this.grdSettings.AllowUserToAddRows = false;
            this.grdSettings.AllowUserToDeleteRows = false;
            this.grdSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_NAME,
            this.COL_VALUE});
            this.grdSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSettings.Location = new System.Drawing.Point(0, 0);
            this.grdSettings.Name = "grdSettings";
            this.grdSettings.Size = new System.Drawing.Size(301, 150);
            this.grdSettings.TabIndex = 0;
            // 
            // COL_NAME
            // 
            this.COL_NAME.DataPropertyName = "Name";
            this.COL_NAME.HeaderText = "Name";
            this.COL_NAME.Name = "COL_NAME";
            this.COL_NAME.ReadOnly = true;
            this.COL_NAME.Width = 150;
            // 
            // COL_VALUE
            // 
            this.COL_VALUE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_VALUE.DataPropertyName = "Value";
            this.COL_VALUE.HeaderText = "Value";
            this.COL_VALUE.Name = "COL_VALUE";
            // 
            // GenericProviderCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdSettings);
            this.Name = "GenericProviderCtrl";
            this.Size = new System.Drawing.Size(301, 150);
            ((System.ComponentModel.ISupportInitialize)(this.grdSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdSettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_VALUE;
    }
}
