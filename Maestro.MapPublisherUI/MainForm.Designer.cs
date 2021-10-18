
namespace Maestro.MapPublisherUI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.wizardControl = new AeroWizard.WizardControl();
            this.wizardPage1 = new AeroWizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.httpLoginCtrl = new Maestro.Login.HttpLoginCtrl();
            this.lblIntroText = new System.Windows.Forms.Label();
            this.wizardPage2 = new AeroWizard.WizardPage();
            this.rdMrl = new System.Windows.Forms.RadioButton();
            this.rdLeaflet = new System.Windows.Forms.RadioButton();
            this.rdOpenLayers = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPage3 = new AeroWizard.WizardPage();
            this.externalBaseLayerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.lstExternalLayers = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddExternalBaseLayer = new System.Windows.Forms.ToolStripDropDownButton();
            this.openStreetMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stamenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bingMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customXYZTileSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDeleteExternalBaseLayer = new System.Windows.Forms.ToolStripButton();
            this.label3 = new System.Windows.Forms.Label();
            this.wizardPage4 = new AeroWizard.WizardPage();
            this.wizardPage5 = new AeroWizard.WizardPage();
            this.wizardPage6 = new AeroWizard.WizardPage();
            this.wizardPage7 = new AeroWizard.WizardPage();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl)).BeginInit();
            this.wizardPage1.SuspendLayout();
            this.wizardPage2.SuspendLayout();
            this.wizardPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.externalBaseLayerSplitContainer)).BeginInit();
            this.externalBaseLayerSplitContainer.Panel1.SuspendLayout();
            this.externalBaseLayerSplitContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl
            // 
            this.wizardControl.BackColor = System.Drawing.Color.White;
            this.wizardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardControl.Location = new System.Drawing.Point(0, 0);
            this.wizardControl.Name = "wizardControl";
            this.wizardControl.Pages.Add(this.wizardPage1);
            this.wizardControl.Pages.Add(this.wizardPage2);
            this.wizardControl.Pages.Add(this.wizardPage3);
            this.wizardControl.Pages.Add(this.wizardPage4);
            this.wizardControl.Pages.Add(this.wizardPage5);
            this.wizardControl.Pages.Add(this.wizardPage6);
            this.wizardControl.Pages.Add(this.wizardPage7);
            this.wizardControl.ShowProgressInTaskbarIcon = true;
            this.wizardControl.Size = new System.Drawing.Size(574, 415);
            this.wizardControl.TabIndex = 0;
            this.wizardControl.Title = "Maestro Static Map Publisher";
            // 
            // wizardPage1
            // 
            this.wizardPage1.AllowNext = false;
            this.wizardPage1.Controls.Add(this.label2);
            this.wizardPage1.Controls.Add(this.httpLoginCtrl);
            this.wizardPage1.Controls.Add(this.lblIntroText);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(527, 261);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Text = "Getting Started";
            this.wizardPage1.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPage1_Commit);
            this.wizardPage1.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPage1_Initialize);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "To get started, establish a connection to your MapGuide Server";
            // 
            // httpLoginCtrl
            // 
            this.httpLoginCtrl.Location = new System.Drawing.Point(20, 111);
            this.httpLoginCtrl.Name = "httpLoginCtrl";
            this.httpLoginCtrl.Password = "admin";
            this.httpLoginCtrl.Server = "http://localhost/mapguide/mapagent/mapagent.fcgi";
            this.httpLoginCtrl.SiteList = null;
            this.httpLoginCtrl.Size = new System.Drawing.Size(489, 133);
            this.httpLoginCtrl.StartingPoint = "Library://";
            this.httpLoginCtrl.TabIndex = 1;
            this.httpLoginCtrl.Username = "Administrator";
            this.httpLoginCtrl.EnableOk += new System.EventHandler(this.httpLoginCtrl_EnableOk);
            this.httpLoginCtrl.DisabledOk += new System.EventHandler(this.httpLoginCtrl_DisabledOk);
            // 
            // lblIntroText
            // 
            this.lblIntroText.Location = new System.Drawing.Point(17, 26);
            this.lblIntroText.Name = "lblIntroText";
            this.lblIntroText.Size = new System.Drawing.Size(492, 39);
            this.lblIntroText.TabIndex = 0;
            this.lblIntroText.Text = "The Maestro Static Map Publisher Wizard will guide you through the process of pub" +
    "lishing a map and/or tileset from your MapGuide Server as a publicly accessible " +
    "static web map.";
            // 
            // wizardPage2
            // 
            this.wizardPage2.Controls.Add(this.rdMrl);
            this.wizardPage2.Controls.Add(this.rdLeaflet);
            this.wizardPage2.Controls.Add(this.rdOpenLayers);
            this.wizardPage2.Controls.Add(this.label1);
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.Size = new System.Drawing.Size(527, 261);
            this.wizardPage2.TabIndex = 1;
            this.wizardPage2.Text = "Viewer Type";
            this.wizardPage2.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPage2_Initialize);
            // 
            // rdMrl
            // 
            this.rdMrl.AutoSize = true;
            this.rdMrl.Location = new System.Drawing.Point(54, 111);
            this.rdMrl.Name = "rdMrl";
            this.rdMrl.Size = new System.Drawing.Size(148, 19);
            this.rdMrl.TabIndex = 3;
            this.rdMrl.TabStop = true;
            this.rdMrl.Text = "mapguide-react-layout";
            this.rdMrl.UseVisualStyleBackColor = true;
            this.rdMrl.CheckedChanged += new System.EventHandler(this.rdMrl_CheckedChanged);
            // 
            // rdLeaflet
            // 
            this.rdLeaflet.AutoSize = true;
            this.rdLeaflet.Location = new System.Drawing.Point(54, 86);
            this.rdLeaflet.Name = "rdLeaflet";
            this.rdLeaflet.Size = new System.Drawing.Size(60, 19);
            this.rdLeaflet.TabIndex = 2;
            this.rdLeaflet.TabStop = true;
            this.rdLeaflet.Text = "Leaflet";
            this.rdLeaflet.UseVisualStyleBackColor = true;
            this.rdLeaflet.CheckedChanged += new System.EventHandler(this.rdLeaflet_CheckedChanged);
            // 
            // rdOpenLayers
            // 
            this.rdOpenLayers.AutoSize = true;
            this.rdOpenLayers.Location = new System.Drawing.Point(54, 61);
            this.rdOpenLayers.Name = "rdOpenLayers";
            this.rdOpenLayers.Size = new System.Drawing.Size(87, 19);
            this.rdOpenLayers.TabIndex = 1;
            this.rdOpenLayers.TabStop = true;
            this.rdOpenLayers.Text = "OpenLayers";
            this.rdOpenLayers.UseVisualStyleBackColor = true;
            this.rdOpenLayers.CheckedChanged += new System.EventHandler(this.rdOpenLayers_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(485, 34);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose the library that your viewer will use. Your choice of library will determi" +
    "ne the capabiltiies and feature set of your viewer application";
            // 
            // wizardPage3
            // 
            this.wizardPage3.Controls.Add(this.externalBaseLayerSplitContainer);
            this.wizardPage3.Controls.Add(this.label3);
            this.wizardPage3.Name = "wizardPage3";
            this.wizardPage3.Size = new System.Drawing.Size(527, 261);
            this.wizardPage3.TabIndex = 2;
            this.wizardPage3.Text = "External Base Layers";
            this.wizardPage3.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPage3_Initialize);
            // 
            // externalBaseLayerSplitContainer
            // 
            this.externalBaseLayerSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.externalBaseLayerSplitContainer.Location = new System.Drawing.Point(27, 54);
            this.externalBaseLayerSplitContainer.Name = "externalBaseLayerSplitContainer";
            // 
            // externalBaseLayerSplitContainer.Panel1
            // 
            this.externalBaseLayerSplitContainer.Panel1.Controls.Add(this.lstExternalLayers);
            this.externalBaseLayerSplitContainer.Panel1.Controls.Add(this.toolStrip1);
            this.externalBaseLayerSplitContainer.Size = new System.Drawing.Size(474, 185);
            this.externalBaseLayerSplitContainer.SplitterDistance = 158;
            this.externalBaseLayerSplitContainer.TabIndex = 1;
            // 
            // lstExternalLayers
            // 
            this.lstExternalLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstExternalLayers.FormattingEnabled = true;
            this.lstExternalLayers.ItemHeight = 15;
            this.lstExternalLayers.Location = new System.Drawing.Point(0, 25);
            this.lstExternalLayers.Name = "lstExternalLayers";
            this.lstExternalLayers.Size = new System.Drawing.Size(158, 160);
            this.lstExternalLayers.TabIndex = 1;
            this.lstExternalLayers.SelectedIndexChanged += new System.EventHandler(this.lstExternalLayers_SelectedIndexChanged);
            this.lstExternalLayers.SelectedValueChanged += new System.EventHandler(this.lstExternalLayers_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddExternalBaseLayer,
            this.btnDeleteExternalBaseLayer});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(158, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddExternalBaseLayer
            // 
            this.btnAddExternalBaseLayer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openStreetMapToolStripMenuItem,
            this.stamenToolStripMenuItem,
            this.bingMapsToolStripMenuItem,
            this.customXYZTileSetToolStripMenuItem});
            this.btnAddExternalBaseLayer.Image = ((System.Drawing.Image)(resources.GetObject("btnAddExternalBaseLayer.Image")));
            this.btnAddExternalBaseLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddExternalBaseLayer.Name = "btnAddExternalBaseLayer";
            this.btnAddExternalBaseLayer.Size = new System.Drawing.Size(58, 22);
            this.btnAddExternalBaseLayer.Text = "Add";
            // 
            // openStreetMapToolStripMenuItem
            // 
            this.openStreetMapToolStripMenuItem.Name = "openStreetMapToolStripMenuItem";
            this.openStreetMapToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openStreetMapToolStripMenuItem.Text = "OpenStreetMap";
            this.openStreetMapToolStripMenuItem.Click += new System.EventHandler(this.openStreetMapToolStripMenuItem_Click);
            // 
            // stamenToolStripMenuItem
            // 
            this.stamenToolStripMenuItem.Name = "stamenToolStripMenuItem";
            this.stamenToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.stamenToolStripMenuItem.Text = "Stamen";
            this.stamenToolStripMenuItem.Click += new System.EventHandler(this.stamenToolStripMenuItem_Click);
            // 
            // bingMapsToolStripMenuItem
            // 
            this.bingMapsToolStripMenuItem.Name = "bingMapsToolStripMenuItem";
            this.bingMapsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.bingMapsToolStripMenuItem.Text = "Bing Maps";
            this.bingMapsToolStripMenuItem.Click += new System.EventHandler(this.bingMapsToolStripMenuItem_Click);
            // 
            // customXYZTileSetToolStripMenuItem
            // 
            this.customXYZTileSetToolStripMenuItem.Name = "customXYZTileSetToolStripMenuItem";
            this.customXYZTileSetToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.customXYZTileSetToolStripMenuItem.Text = "Custom XYZ TileSet";
            this.customXYZTileSetToolStripMenuItem.Click += new System.EventHandler(this.customXYZTileSetToolStripMenuItem_Click);
            // 
            // btnDeleteExternalBaseLayer
            // 
            this.btnDeleteExternalBaseLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteExternalBaseLayer.Enabled = false;
            this.btnDeleteExternalBaseLayer.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteExternalBaseLayer.Image")));
            this.btnDeleteExternalBaseLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteExternalBaseLayer.Name = "btnDeleteExternalBaseLayer";
            this.btnDeleteExternalBaseLayer.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteExternalBaseLayer.Text = "Delete";
            this.btnDeleteExternalBaseLayer.Click += new System.EventHandler(this.btnDeleteExternalBaseLayer_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(340, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "What external base layers do you want available for this viewer?";
            // 
            // wizardPage4
            // 
            this.wizardPage4.Name = "wizardPage4";
            this.wizardPage4.Size = new System.Drawing.Size(527, 261);
            this.wizardPage4.TabIndex = 3;
            this.wizardPage4.Text = "MapGuide TileSets";
            this.wizardPage4.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPage4_Initialize);
            // 
            // wizardPage5
            // 
            this.wizardPage5.IsFinishPage = true;
            this.wizardPage5.Name = "wizardPage5";
            this.wizardPage5.ShowNext = false;
            this.wizardPage5.Size = new System.Drawing.Size(527, 261);
            this.wizardPage5.TabIndex = 4;
            this.wizardPage5.Text = "Overlay Layers";
            this.wizardPage5.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPage5_Initialize);
            // 
            // wizardPage6
            // 
            this.wizardPage6.Name = "wizardPage6";
            this.wizardPage6.Size = new System.Drawing.Size(527, 261);
            this.wizardPage6.TabIndex = 5;
            this.wizardPage6.Text = "Viewer Features";
            // 
            // wizardPage7
            // 
            this.wizardPage7.Name = "wizardPage7";
            this.wizardPage7.Size = new System.Drawing.Size(527, 261);
            this.wizardPage7.TabIndex = 6;
            this.wizardPage7.Text = "Review and Publish";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 415);
            this.Controls.Add(this.wizardControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl)).EndInit();
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            this.wizardPage2.ResumeLayout(false);
            this.wizardPage2.PerformLayout();
            this.wizardPage3.ResumeLayout(false);
            this.wizardPage3.PerformLayout();
            this.externalBaseLayerSplitContainer.Panel1.ResumeLayout(false);
            this.externalBaseLayerSplitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.externalBaseLayerSplitContainer)).EndInit();
            this.externalBaseLayerSplitContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl wizardControl;
        private AeroWizard.WizardPage wizardPage1;
        private AeroWizard.WizardPage wizardPage2;
        private AeroWizard.WizardPage wizardPage3;
        private AeroWizard.WizardPage wizardPage4;
        private AeroWizard.WizardPage wizardPage5;
        private System.Windows.Forms.Label lblIntroText;
        private System.Windows.Forms.RadioButton rdMrl;
        private System.Windows.Forms.RadioButton rdLeaflet;
        private System.Windows.Forms.RadioButton rdOpenLayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Login.HttpLoginCtrl httpLoginCtrl;
        private AeroWizard.WizardPage wizardPage6;
        private AeroWizard.WizardPage wizardPage7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer externalBaseLayerSplitContainer;
        private System.Windows.Forms.ListBox lstExternalLayers;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton btnAddExternalBaseLayer;
        private System.Windows.Forms.ToolStripMenuItem openStreetMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stamenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bingMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customXYZTileSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnDeleteExternalBaseLayer;
    }
}