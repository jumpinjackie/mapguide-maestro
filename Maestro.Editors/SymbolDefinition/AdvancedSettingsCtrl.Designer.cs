namespace Maestro.Editors.SymbolDefinition
{
    partial class AdvancedSettingsCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSettingsCtrl));
            this.chkEnableResizeBox = new System.Windows.Forms.CheckBox();
            this.grpResizeBox = new System.Windows.Forms.GroupBox();
            this.symYCoord = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symXCoord = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symGrowControl = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symHeight = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symWidth = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.contentPanel.SuspendLayout();
            this.grpResizeBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.grpResizeBox);
            this.contentPanel.Controls.Add(this.chkEnableResizeBox);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // chkEnableResizeBox
            // 
            resources.ApplyResources(this.chkEnableResizeBox, "chkEnableResizeBox");
            this.chkEnableResizeBox.Name = "chkEnableResizeBox";
            this.chkEnableResizeBox.UseVisualStyleBackColor = true;
            this.chkEnableResizeBox.CheckedChanged += new System.EventHandler(this.chkEnableResizeBox_CheckedChanged);
            // 
            // grpResizeBox
            // 
            this.grpResizeBox.Controls.Add(this.symYCoord);
            this.grpResizeBox.Controls.Add(this.symXCoord);
            this.grpResizeBox.Controls.Add(this.symGrowControl);
            this.grpResizeBox.Controls.Add(this.symHeight);
            this.grpResizeBox.Controls.Add(this.symWidth);
            this.grpResizeBox.Controls.Add(this.label5);
            this.grpResizeBox.Controls.Add(this.label4);
            this.grpResizeBox.Controls.Add(this.label3);
            this.grpResizeBox.Controls.Add(this.label2);
            this.grpResizeBox.Controls.Add(this.label1);
            resources.ApplyResources(this.grpResizeBox, "grpResizeBox");
            this.grpResizeBox.Name = "grpResizeBox";
            this.grpResizeBox.TabStop = false;
            // 
            // symYCoord
            // 
            resources.ApplyResources(this.symYCoord, "symYCoord");
            this.symYCoord.Name = "symYCoord";
            this.symYCoord.SupportedEnhancedDataTypes = null;
            this.symYCoord.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symYCoord.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symXCoord
            // 
            resources.ApplyResources(this.symXCoord, "symXCoord");
            this.symXCoord.Name = "symXCoord";
            this.symXCoord.SupportedEnhancedDataTypes = null;
            this.symXCoord.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symXCoord.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symGrowControl
            // 
            resources.ApplyResources(this.symGrowControl, "symGrowControl");
            this.symGrowControl.Name = "symGrowControl";
            this.symGrowControl.SupportedEnhancedDataTypes = null;
            this.symGrowControl.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symGrowControl.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symHeight
            // 
            resources.ApplyResources(this.symHeight, "symHeight");
            this.symHeight.Name = "symHeight";
            this.symHeight.SupportedEnhancedDataTypes = null;
            this.symHeight.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symHeight.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symWidth
            // 
            resources.ApplyResources(this.symWidth, "symWidth");
            this.symWidth.Name = "symWidth";
            this.symWidth.SupportedEnhancedDataTypes = null;
            this.symWidth.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symWidth.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // AdvancedSettingsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Advanced";
            this.Name = "AdvancedSettingsCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.grpResizeBox.ResumeLayout(false);
            this.grpResizeBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEnableResizeBox;
        private System.Windows.Forms.GroupBox grpResizeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private SymbolField symYCoord;
        private SymbolField symXCoord;
        private SymbolField symGrowControl;
        private SymbolField symHeight;
        private SymbolField symWidth;
    }
}
