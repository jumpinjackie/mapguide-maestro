namespace Maestro.Editors.Fusion
{
    partial class ManageSettingsDialog
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
            grdSettings = new System.Windows.Forms.DataGridView();
            btnApplyAndClose = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            COL_KEY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            COL_VALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)grdSettings).BeginInit();
            SuspendLayout();
            // 
            // grdSettings
            // 
            grdSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { COL_KEY, COL_VALUE });
            grdSettings.Dock = System.Windows.Forms.DockStyle.Top;
            grdSettings.Location = new System.Drawing.Point(0, 0);
            grdSettings.Name = "grdSettings";
            grdSettings.RowTemplate.Height = 25;
            grdSettings.Size = new System.Drawing.Size(800, 392);
            grdSettings.TabIndex = 3;
            // 
            // btnApplyAndClose
            // 
            btnApplyAndClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnApplyAndClose.Location = new System.Drawing.Point(600, 415);
            btnApplyAndClose.Name = "btnApplyAndClose";
            btnApplyAndClose.Size = new System.Drawing.Size(107, 23);
            btnApplyAndClose.TabIndex = 5;
            btnApplyAndClose.Text = "Apply and Close";
            btnApplyAndClose.UseVisualStyleBackColor = true;
            btnApplyAndClose.Click += btnApplyAndClose_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(713, 415);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // COL_KEY
            // 
            COL_KEY.DataPropertyName = "Name";
            COL_KEY.HeaderText = "Name";
            COL_KEY.Name = "COL_KEY";
            COL_KEY.Width = 200;
            // 
            // COL_VALUE
            // 
            COL_VALUE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            COL_VALUE.DataPropertyName = "Value";
            COL_VALUE.HeaderText = "Value";
            COL_VALUE.Name = "COL_VALUE";
            // 
            // ManageSettingsDialog
            // 
            AcceptButton = btnApplyAndClose;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(800, 450);
            ControlBox = false;
            Controls.Add(btnApplyAndClose);
            Controls.Add(btnCancel);
            Controls.Add(grdSettings);
            Name = "ManageSettingsDialog";
            Text = "Manage Settings";
            ((System.ComponentModel.ISupportInitialize)grdSettings).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView grdSettings;
        private System.Windows.Forms.Button btnApplyAndClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_KEY;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_VALUE;
    }
}