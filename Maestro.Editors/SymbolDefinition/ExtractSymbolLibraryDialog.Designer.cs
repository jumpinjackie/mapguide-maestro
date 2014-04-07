namespace Maestro.Editors.SymbolDefinition
{
    partial class ExtractSymbolLibraryDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractSymbolLibraryDialog));
            this.btnSource = new System.Windows.Forms.Button();
            this.grpAvailableSymbols = new System.Windows.Forms.GroupBox();
            this.lstSymbols = new System.Windows.Forms.ListView();
            this.txtSymbolLibrary = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTarget = new System.Windows.Forms.Button();
            this.txtTargetFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpAvailableSymbols.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSource
            // 
            resources.ApplyResources(this.btnSource, "btnSource");
            this.btnSource.Name = "btnSource";
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
            // 
            // grpAvailableSymbols
            // 
            resources.ApplyResources(this.grpAvailableSymbols, "grpAvailableSymbols");
            this.grpAvailableSymbols.Controls.Add(this.lstSymbols);
            this.grpAvailableSymbols.Name = "grpAvailableSymbols";
            this.grpAvailableSymbols.TabStop = false;
            // 
            // lstSymbols
            // 
            resources.ApplyResources(this.lstSymbols, "lstSymbols");
            this.lstSymbols.Name = "lstSymbols";
            this.lstSymbols.UseCompatibleStateImageBehavior = false;
            this.lstSymbols.View = System.Windows.Forms.View.Tile;
            this.lstSymbols.SelectedIndexChanged += new System.EventHandler(this.lstSymbols_SelectedIndexChanged);
            // 
            // txtSymbolLibrary
            // 
            resources.ApplyResources(this.txtSymbolLibrary, "txtSymbolLibrary");
            this.txtSymbolLibrary.Name = "txtSymbolLibrary";
            this.txtSymbolLibrary.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnTarget
            // 
            resources.ApplyResources(this.btnTarget, "btnTarget");
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.UseVisualStyleBackColor = true;
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // txtTargetFolder
            // 
            resources.ApplyResources(this.txtTargetFolder, "txtTargetFolder");
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ExtractSymbolLibraryDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTarget);
            this.Controls.Add(this.txtTargetFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSource);
            this.Controls.Add(this.grpAvailableSymbols);
            this.Controls.Add(this.txtSymbolLibrary);
            this.Controls.Add(this.label1);
            this.Name = "ExtractSymbolLibraryDialog";
            this.grpAvailableSymbols.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSource;
        private System.Windows.Forms.GroupBox grpAvailableSymbols;
        private System.Windows.Forms.ListView lstSymbols;
        private System.Windows.Forms.TextBox txtSymbolLibrary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTarget;
        private System.Windows.Forms.TextBox txtTargetFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}