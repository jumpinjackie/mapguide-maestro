namespace Maestro.AddIn.Rest.UI.Representation
{
    partial class CsvRepresentationCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CsvRepresentationCtrl));
            this.getMethodCtrl1 = new Maestro.AddIn.Rest.UI.Methods.GetMethodCtrl();
            this.permissionsCtrl1 = new Maestro.AddIn.Rest.UI.Representation.PermissionsCtrl();
            this.SuspendLayout();
            // 
            // getMethodCtrl1
            // 
            resources.ApplyResources(this.getMethodCtrl1, "getMethodCtrl1");
            this.getMethodCtrl1.Name = "getMethodCtrl1";
            // 
            // permissionsCtrl1
            // 
            resources.ApplyResources(this.permissionsCtrl1, "permissionsCtrl1");
            this.permissionsCtrl1.Name = "permissionsCtrl1";
            // 
            // CsvRepresentationCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.permissionsCtrl1);
            this.Controls.Add(this.getMethodCtrl1);
            this.Name = "CsvRepresentationCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private Methods.GetMethodCtrl getMethodCtrl1;
        private PermissionsCtrl permissionsCtrl1;
    }
}
