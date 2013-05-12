namespace Maestro.Editors.Fusion.WidgetEditors
{
    partial class GenericWidgetCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericWidgetCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnWidgetInfo = new System.Windows.Forms.Button();
            this.txtXmlContent = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnWidgetInfo
            // 
            resources.ApplyResources(this.btnWidgetInfo, "btnWidgetInfo");
            this.btnWidgetInfo.Name = "btnWidgetInfo";
            this.btnWidgetInfo.UseVisualStyleBackColor = true;
            this.btnWidgetInfo.Click += new System.EventHandler(this.btnWidgetInfo_Click);
            // 
            // txtXmlContent
            // 
            resources.ApplyResources(this.txtXmlContent, "txtXmlContent");
            this.txtXmlContent.IsReadOnly = false;
            this.txtXmlContent.Name = "txtXmlContent";
            this.txtXmlContent.TextChanged += new System.EventHandler(this.txtXmlContent_TextChanged);
            // 
            // GenericWidgetCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtXmlContent);
            this.Controls.Add(this.btnWidgetInfo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Name = "GenericWidgetCtrl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnWidgetInfo;
        private ICSharpCode.TextEditor.TextEditorControl txtXmlContent;
    }
}
