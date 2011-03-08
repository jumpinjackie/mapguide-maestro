namespace Maestro.Editors.LoadProcedure
{
    partial class ExecuteCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecuteCtrl));
            this.btnExecute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            resources.ApplyResources(this.btnExecute, "btnExecute");
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // ExecuteCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnExecute);
            this.Name = "ExecuteCtrl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
    }
}
