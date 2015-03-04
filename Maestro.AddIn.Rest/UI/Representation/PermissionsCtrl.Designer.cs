namespace Maestro.AddIn.Rest.UI.Representation
{
    partial class PermissionsCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionsCtrl));
            this.chkAllowRoles = new System.Windows.Forms.CheckBox();
            this.lstAllowGroups = new System.Windows.Forms.ListBox();
            this.chkAllowGroups = new System.Windows.Forms.CheckBox();
            this.chkAllowAnonymous = new System.Windows.Forms.CheckBox();
            this.txtRoles = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkAllowRoles
            // 
            resources.ApplyResources(this.chkAllowRoles, "chkAllowRoles");
            this.chkAllowRoles.Name = "chkAllowRoles";
            this.chkAllowRoles.UseVisualStyleBackColor = true;
            // 
            // lstAllowGroups
            // 
            resources.ApplyResources(this.lstAllowGroups, "lstAllowGroups");
            this.lstAllowGroups.FormattingEnabled = true;
            this.lstAllowGroups.Name = "lstAllowGroups";
            this.lstAllowGroups.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            // 
            // chkAllowGroups
            // 
            resources.ApplyResources(this.chkAllowGroups, "chkAllowGroups");
            this.chkAllowGroups.Name = "chkAllowGroups";
            this.chkAllowGroups.UseVisualStyleBackColor = true;
            // 
            // chkAllowAnonymous
            // 
            resources.ApplyResources(this.chkAllowAnonymous, "chkAllowAnonymous");
            this.chkAllowAnonymous.Name = "chkAllowAnonymous";
            this.chkAllowAnonymous.UseVisualStyleBackColor = true;
            // 
            // txtRoles
            // 
            resources.ApplyResources(this.txtRoles, "txtRoles");
            this.txtRoles.Name = "txtRoles";
            // 
            // PermissionsCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtRoles);
            this.Controls.Add(this.chkAllowRoles);
            this.Controls.Add(this.lstAllowGroups);
            this.Controls.Add(this.chkAllowGroups);
            this.Controls.Add(this.chkAllowAnonymous);
            this.Name = "PermissionsCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAllowRoles;
        private System.Windows.Forms.ListBox lstAllowGroups;
        private System.Windows.Forms.CheckBox chkAllowGroups;
        private System.Windows.Forms.CheckBox chkAllowAnonymous;
        private System.Windows.Forms.TextBox txtRoles;
    }
}
