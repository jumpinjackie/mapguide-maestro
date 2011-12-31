namespace Maestro.Editors.SymbolDefinition.GraphicsEditors
{
    partial class TextDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextDialog));
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.symFontUnderlined = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symFontItalic = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symFontBold = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symFontFamily = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.lnkSelectFont = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.symPositionY = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symPositionX = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symAngle = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symHeightScalable = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symHeight = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.symAlignmentJustification = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symAlignmentVertical = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symAlignmentHorizontal = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.grpTextFrame = new System.Windows.Forms.GroupBox();
            this.symOffsetY = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symOffsetX = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symFillColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.chkTextFrame = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.symGhostColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symTextColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineSpacing = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnContent = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpTextFrame.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.symFontUnderlined);
            this.groupBox1.Controls.Add(this.symFontItalic);
            this.groupBox1.Controls.Add(this.symFontBold);
            this.groupBox1.Controls.Add(this.symFontFamily);
            this.groupBox1.Controls.Add(this.lnkSelectFont);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // symFontUnderlined
            // 
            resources.ApplyResources(this.symFontUnderlined, "symFontUnderlined");
            this.symFontUnderlined.Name = "symFontUnderlined";
            this.symFontUnderlined.SupportedEnhancedDataTypes = null;
            this.symFontUnderlined.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symFontUnderlined.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symFontItalic
            // 
            resources.ApplyResources(this.symFontItalic, "symFontItalic");
            this.symFontItalic.Name = "symFontItalic";
            this.symFontItalic.SupportedEnhancedDataTypes = null;
            this.symFontItalic.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symFontItalic.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symFontBold
            // 
            resources.ApplyResources(this.symFontBold, "symFontBold");
            this.symFontBold.Name = "symFontBold";
            this.symFontBold.SupportedEnhancedDataTypes = null;
            this.symFontBold.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symFontBold.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symFontFamily
            // 
            resources.ApplyResources(this.symFontFamily, "symFontFamily");
            this.symFontFamily.Name = "symFontFamily";
            this.symFontFamily.SupportedEnhancedDataTypes = null;
            this.symFontFamily.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symFontFamily.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // lnkSelectFont
            // 
            resources.ApplyResources(this.lnkSelectFont, "lnkSelectFont");
            this.lnkSelectFont.Name = "lnkSelectFont";
            this.lnkSelectFont.TabStop = true;
            this.lnkSelectFont.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectFont_LinkClicked);
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
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtContent
            // 
            resources.ApplyResources(this.txtContent, "txtContent");
            this.txtContent.Name = "txtContent";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.symPositionY);
            this.groupBox2.Controls.Add(this.symPositionX);
            this.groupBox2.Controls.Add(this.symAngle);
            this.groupBox2.Controls.Add(this.symHeightScalable);
            this.groupBox2.Controls.Add(this.symHeight);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // symPositionY
            // 
            resources.ApplyResources(this.symPositionY, "symPositionY");
            this.symPositionY.Name = "symPositionY";
            this.symPositionY.SupportedEnhancedDataTypes = null;
            this.symPositionY.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symPositionY.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symPositionX
            // 
            resources.ApplyResources(this.symPositionX, "symPositionX");
            this.symPositionX.Name = "symPositionX";
            this.symPositionX.SupportedEnhancedDataTypes = null;
            this.symPositionX.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symPositionX.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symAngle
            // 
            resources.ApplyResources(this.symAngle, "symAngle");
            this.symAngle.Name = "symAngle";
            this.symAngle.SupportedEnhancedDataTypes = null;
            this.symAngle.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symAngle.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symHeightScalable
            // 
            resources.ApplyResources(this.symHeightScalable, "symHeightScalable");
            this.symHeightScalable.Name = "symHeightScalable";
            this.symHeightScalable.SupportedEnhancedDataTypes = null;
            this.symHeightScalable.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symHeightScalable.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symHeight
            // 
            resources.ApplyResources(this.symHeight, "symHeight");
            this.symHeight.Name = "symHeight";
            this.symHeight.SupportedEnhancedDataTypes = null;
            this.symHeight.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symHeight.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.symAlignmentJustification);
            this.groupBox3.Controls.Add(this.symAlignmentVertical);
            this.groupBox3.Controls.Add(this.symAlignmentHorizontal);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // symAlignmentJustification
            // 
            resources.ApplyResources(this.symAlignmentJustification, "symAlignmentJustification");
            this.symAlignmentJustification.Name = "symAlignmentJustification";
            this.symAlignmentJustification.SupportedEnhancedDataTypes = null;
            this.symAlignmentJustification.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symAlignmentJustification.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symAlignmentVertical
            // 
            resources.ApplyResources(this.symAlignmentVertical, "symAlignmentVertical");
            this.symAlignmentVertical.Name = "symAlignmentVertical";
            this.symAlignmentVertical.SupportedEnhancedDataTypes = null;
            this.symAlignmentVertical.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symAlignmentVertical.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symAlignmentHorizontal
            // 
            resources.ApplyResources(this.symAlignmentHorizontal, "symAlignmentHorizontal");
            this.symAlignmentHorizontal.Name = "symAlignmentHorizontal";
            this.symAlignmentHorizontal.SupportedEnhancedDataTypes = null;
            this.symAlignmentHorizontal.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symAlignmentHorizontal.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // grpTextFrame
            // 
            this.grpTextFrame.Controls.Add(this.symOffsetY);
            this.grpTextFrame.Controls.Add(this.symOffsetX);
            this.grpTextFrame.Controls.Add(this.symFillColor);
            this.grpTextFrame.Controls.Add(this.symLineColor);
            this.grpTextFrame.Controls.Add(this.label17);
            this.grpTextFrame.Controls.Add(this.label16);
            this.grpTextFrame.Controls.Add(this.label15);
            this.grpTextFrame.Controls.Add(this.label14);
            resources.ApplyResources(this.grpTextFrame, "grpTextFrame");
            this.grpTextFrame.Name = "grpTextFrame";
            this.grpTextFrame.TabStop = false;
            // 
            // symOffsetY
            // 
            resources.ApplyResources(this.symOffsetY, "symOffsetY");
            this.symOffsetY.Name = "symOffsetY";
            this.symOffsetY.SupportedEnhancedDataTypes = null;
            this.symOffsetY.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symOffsetY.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symOffsetX
            // 
            resources.ApplyResources(this.symOffsetX, "symOffsetX");
            this.symOffsetX.Name = "symOffsetX";
            this.symOffsetX.SupportedEnhancedDataTypes = null;
            this.symOffsetX.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symOffsetX.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symFillColor
            // 
            resources.ApplyResources(this.symFillColor, "symFillColor");
            this.symFillColor.Name = "symFillColor";
            this.symFillColor.SupportedEnhancedDataTypes = null;
            this.symFillColor.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symFillColor.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineColor
            // 
            resources.ApplyResources(this.symLineColor, "symLineColor");
            this.symLineColor.Name = "symLineColor";
            this.symLineColor.SupportedEnhancedDataTypes = null;
            this.symLineColor.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineColor.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // chkTextFrame
            // 
            resources.ApplyResources(this.chkTextFrame, "chkTextFrame");
            this.chkTextFrame.Name = "chkTextFrame";
            this.chkTextFrame.UseVisualStyleBackColor = true;
            this.chkTextFrame.CheckedChanged += new System.EventHandler(this.chkTextFrame_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.symGhostColor);
            this.groupBox5.Controls.Add(this.symTextColor);
            this.groupBox5.Controls.Add(this.symLineSpacing);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label20);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // symGhostColor
            // 
            resources.ApplyResources(this.symGhostColor, "symGhostColor");
            this.symGhostColor.Name = "symGhostColor";
            this.symGhostColor.SupportedEnhancedDataTypes = null;
            this.symGhostColor.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symGhostColor.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symTextColor
            // 
            resources.ApplyResources(this.symTextColor, "symTextColor");
            this.symTextColor.Name = "symTextColor";
            this.symTextColor.SupportedEnhancedDataTypes = null;
            this.symTextColor.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symTextColor.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineSpacing
            // 
            resources.ApplyResources(this.symLineSpacing, "symLineSpacing");
            this.symLineSpacing.Name = "symLineSpacing";
            this.symLineSpacing.SupportedEnhancedDataTypes = null;
            this.symLineSpacing.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineSpacing.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnContent
            // 
            resources.ApplyResources(this.btnContent, "btnContent");
            this.btnContent.Name = "btnContent";
            this.btnContent.UseVisualStyleBackColor = true;
            this.btnContent.Click += new System.EventHandler(this.btnContent_Click);
            // 
            // TextDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnContent);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.chkTextFrame);
            this.Controls.Add(this.grpTextFrame);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "TextDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpTextFrame.ResumeLayout(false);
            this.grpTextFrame.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel lnkSelectFont;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox grpTextFrame;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkTextFrame;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label7;
        private SymbolField symFontUnderlined;
        private SymbolField symFontItalic;
        private SymbolField symFontBold;
        private SymbolField symFontFamily;
        private SymbolField symPositionY;
        private SymbolField symPositionX;
        private SymbolField symAngle;
        private SymbolField symHeightScalable;
        private SymbolField symHeight;
        private SymbolField symAlignmentJustification;
        private SymbolField symAlignmentVertical;
        private SymbolField symAlignmentHorizontal;
        private SymbolField symOffsetY;
        private SymbolField symOffsetX;
        private SymbolField symFillColor;
        private SymbolField symLineColor;
        private SymbolField symGhostColor;
        private SymbolField symTextColor;
        private SymbolField symLineSpacing;
        private System.Windows.Forms.Button btnContent;
    }
}