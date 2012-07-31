namespace Maestro.Editors.Fusion
{
    partial class MapCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtMapId = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.txtViewScale = new System.Windows.Forms.TextBox();
            this.txtViewY = new System.Windows.Forms.TextBox();
            this.txtViewX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMapDefinition = new System.Windows.Forms.TextBox();
            this.btnBrowseMdf = new System.Windows.Forms.Button();
            this.chkSingleTiled = new System.Windows.Forms.CheckBox();
            this.lblSelColor = new System.Windows.Forms.Label();
            this.chkSelectionAsOverlay = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbSelectionColor = new Maestro.Editors.Common.ColorComboBox();
            this.grpCms = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkOsmMapnik = new System.Windows.Forms.CheckBox();
            this.chkOsmTransportMap = new System.Windows.Forms.CheckBox();
            this.chkOsmCycleMap = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkGoogTerrain = new System.Windows.Forms.CheckBox();
            this.chkGoogStreets = new System.Windows.Forms.CheckBox();
            this.chkGoogSatellite = new System.Windows.Forms.CheckBox();
            this.chkGoogHybrid = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkBingStreets = new System.Windows.Forms.CheckBox();
            this.chkBingSatellite = new System.Windows.Forms.CheckBox();
            this.chkBingHybrid = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkYahooStreets = new System.Windows.Forms.CheckBox();
            this.chkYahooSatellite = new System.Windows.Forms.CheckBox();
            this.chkYahooHybrid = new System.Windows.Forms.CheckBox();
            this.txtYahooApiKey = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpCms.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtMapId
            // 
            resources.ApplyResources(this.txtMapId, "txtMapId");
            this.txtMapId.Name = "txtMapId";
            this.txtMapId.TextChanged += new System.EventHandler(this.txtMapId_TextChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.chkOverride);
            this.groupBox1.Controls.Add(this.txtViewScale);
            this.groupBox1.Controls.Add(this.txtViewY);
            this.groupBox1.Controls.Add(this.txtViewX);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkOverride
            // 
            resources.ApplyResources(this.chkOverride, "chkOverride");
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.UseVisualStyleBackColor = true;
            this.chkOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
            // 
            // txtViewScale
            // 
            resources.ApplyResources(this.txtViewScale, "txtViewScale");
            this.txtViewScale.Name = "txtViewScale";
            // 
            // txtViewY
            // 
            resources.ApplyResources(this.txtViewY, "txtViewY");
            this.txtViewY.Name = "txtViewY";
            // 
            // txtViewX
            // 
            resources.ApplyResources(this.txtViewX, "txtViewX");
            this.txtViewX.Name = "txtViewX";
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
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtMapDefinition
            // 
            resources.ApplyResources(this.txtMapDefinition, "txtMapDefinition");
            this.txtMapDefinition.Name = "txtMapDefinition";
            this.txtMapDefinition.ReadOnly = true;
            // 
            // btnBrowseMdf
            // 
            resources.ApplyResources(this.btnBrowseMdf, "btnBrowseMdf");
            this.btnBrowseMdf.Name = "btnBrowseMdf";
            this.btnBrowseMdf.UseVisualStyleBackColor = true;
            this.btnBrowseMdf.Click += new System.EventHandler(this.btnBrowseMdf_Click);
            // 
            // chkSingleTiled
            // 
            resources.ApplyResources(this.chkSingleTiled, "chkSingleTiled");
            this.chkSingleTiled.Name = "chkSingleTiled";
            this.chkSingleTiled.UseVisualStyleBackColor = true;
            // 
            // lblSelColor
            // 
            resources.ApplyResources(this.lblSelColor, "lblSelColor");
            this.lblSelColor.Name = "lblSelColor";
            // 
            // chkSelectionAsOverlay
            // 
            resources.ApplyResources(this.chkSelectionAsOverlay, "chkSelectionAsOverlay");
            this.chkSelectionAsOverlay.Name = "chkSelectionAsOverlay";
            this.chkSelectionAsOverlay.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMapId);
            this.groupBox2.Controls.Add(this.chkSelectionAsOverlay);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbSelectionColor);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.lblSelColor);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkSingleTiled);
            this.groupBox2.Controls.Add(this.txtMapDefinition);
            this.groupBox2.Controls.Add(this.btnBrowseMdf);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cmbSelectionColor
            // 
            this.cmbSelectionColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbSelectionColor, "cmbSelectionColor");
            this.cmbSelectionColor.Name = "cmbSelectionColor";
            // 
            // grpCms
            // 
            resources.ApplyResources(this.grpCms, "grpCms");
            this.grpCms.Controls.Add(this.groupBox6);
            this.grpCms.Controls.Add(this.groupBox5);
            this.grpCms.Controls.Add(this.groupBox4);
            this.grpCms.Controls.Add(this.groupBox3);
            this.grpCms.Controls.Add(this.txtYahooApiKey);
            this.grpCms.Controls.Add(this.label7);
            this.grpCms.Name = "grpCms";
            this.grpCms.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chkOsmMapnik);
            this.groupBox6.Controls.Add(this.chkOsmTransportMap);
            this.groupBox6.Controls.Add(this.chkOsmCycleMap);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // chkOsmMapnik
            // 
            resources.ApplyResources(this.chkOsmMapnik, "chkOsmMapnik");
            this.chkOsmMapnik.Name = "chkOsmMapnik";
            this.chkOsmMapnik.UseVisualStyleBackColor = true;
            this.chkOsmMapnik.CheckedChanged += new System.EventHandler(this.chkOsmMapnik_CheckedChanged);
            // 
            // chkOsmTransportMap
            // 
            resources.ApplyResources(this.chkOsmTransportMap, "chkOsmTransportMap");
            this.chkOsmTransportMap.Name = "chkOsmTransportMap";
            this.chkOsmTransportMap.UseVisualStyleBackColor = true;
            this.chkOsmTransportMap.CheckedChanged += new System.EventHandler(this.chkOsmTransportMap_CheckedChanged);
            // 
            // chkOsmCycleMap
            // 
            resources.ApplyResources(this.chkOsmCycleMap, "chkOsmCycleMap");
            this.chkOsmCycleMap.Name = "chkOsmCycleMap";
            this.chkOsmCycleMap.UseVisualStyleBackColor = true;
            this.chkOsmCycleMap.CheckedChanged += new System.EventHandler(this.chkOsmCycleMap_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkGoogTerrain);
            this.groupBox5.Controls.Add(this.chkGoogStreets);
            this.groupBox5.Controls.Add(this.chkGoogSatellite);
            this.groupBox5.Controls.Add(this.chkGoogHybrid);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // chkGoogTerrain
            // 
            resources.ApplyResources(this.chkGoogTerrain, "chkGoogTerrain");
            this.chkGoogTerrain.Name = "chkGoogTerrain";
            this.chkGoogTerrain.UseVisualStyleBackColor = true;
            this.chkGoogTerrain.CheckedChanged += new System.EventHandler(this.chkGoogTerrain_CheckedChanged);
            // 
            // chkGoogStreets
            // 
            resources.ApplyResources(this.chkGoogStreets, "chkGoogStreets");
            this.chkGoogStreets.Name = "chkGoogStreets";
            this.chkGoogStreets.UseVisualStyleBackColor = true;
            this.chkGoogStreets.CheckedChanged += new System.EventHandler(this.chkGoogStreets_CheckedChanged);
            // 
            // chkGoogSatellite
            // 
            resources.ApplyResources(this.chkGoogSatellite, "chkGoogSatellite");
            this.chkGoogSatellite.Name = "chkGoogSatellite";
            this.chkGoogSatellite.UseVisualStyleBackColor = true;
            this.chkGoogSatellite.CheckedChanged += new System.EventHandler(this.chkGoogSatellite_CheckedChanged);
            // 
            // chkGoogHybrid
            // 
            resources.ApplyResources(this.chkGoogHybrid, "chkGoogHybrid");
            this.chkGoogHybrid.Name = "chkGoogHybrid";
            this.chkGoogHybrid.UseVisualStyleBackColor = true;
            this.chkGoogHybrid.CheckedChanged += new System.EventHandler(this.chkGoogHybrid_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkBingStreets);
            this.groupBox4.Controls.Add(this.chkBingSatellite);
            this.groupBox4.Controls.Add(this.chkBingHybrid);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // chkBingStreets
            // 
            resources.ApplyResources(this.chkBingStreets, "chkBingStreets");
            this.chkBingStreets.Name = "chkBingStreets";
            this.chkBingStreets.UseVisualStyleBackColor = true;
            this.chkBingStreets.CheckedChanged += new System.EventHandler(this.chkBingStreets_CheckedChanged);
            // 
            // chkBingSatellite
            // 
            resources.ApplyResources(this.chkBingSatellite, "chkBingSatellite");
            this.chkBingSatellite.Name = "chkBingSatellite";
            this.chkBingSatellite.UseVisualStyleBackColor = true;
            this.chkBingSatellite.CheckedChanged += new System.EventHandler(this.chkBingSatellite_CheckedChanged);
            // 
            // chkBingHybrid
            // 
            resources.ApplyResources(this.chkBingHybrid, "chkBingHybrid");
            this.chkBingHybrid.Name = "chkBingHybrid";
            this.chkBingHybrid.UseVisualStyleBackColor = true;
            this.chkBingHybrid.CheckedChanged += new System.EventHandler(this.chkBingHybrid_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkYahooStreets);
            this.groupBox3.Controls.Add(this.chkYahooSatellite);
            this.groupBox3.Controls.Add(this.chkYahooHybrid);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chkYahooStreets
            // 
            resources.ApplyResources(this.chkYahooStreets, "chkYahooStreets");
            this.chkYahooStreets.Name = "chkYahooStreets";
            this.chkYahooStreets.UseVisualStyleBackColor = true;
            this.chkYahooStreets.CheckedChanged += new System.EventHandler(this.chkYahooStreets_CheckedChanged);
            // 
            // chkYahooSatellite
            // 
            resources.ApplyResources(this.chkYahooSatellite, "chkYahooSatellite");
            this.chkYahooSatellite.Name = "chkYahooSatellite";
            this.chkYahooSatellite.UseVisualStyleBackColor = true;
            this.chkYahooSatellite.CheckedChanged += new System.EventHandler(this.chkYahooSatellite_CheckedChanged);
            // 
            // chkYahooHybrid
            // 
            resources.ApplyResources(this.chkYahooHybrid, "chkYahooHybrid");
            this.chkYahooHybrid.Name = "chkYahooHybrid";
            this.chkYahooHybrid.UseVisualStyleBackColor = true;
            this.chkYahooHybrid.CheckedChanged += new System.EventHandler(this.chkYahooHybrid_CheckedChanged);
            // 
            // txtYahooApiKey
            // 
            resources.ApplyResources(this.txtYahooApiKey, "txtYahooApiKey");
            this.txtYahooApiKey.Name = "txtYahooApiKey";
            this.txtYahooApiKey.TextChanged += new System.EventHandler(this.txtYahooApiKey_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // MapCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grpCms);
            this.Controls.Add(this.groupBox2);
            this.Name = "MapCtrl";
            resources.ApplyResources(this, "$this");
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpCms.ResumeLayout(false);
            this.grpCms.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMapId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtViewScale;
        private System.Windows.Forms.TextBox txtViewY;
        private System.Windows.Forms.TextBox txtViewX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMapDefinition;
        private System.Windows.Forms.Button btnBrowseMdf;
        private System.Windows.Forms.CheckBox chkOverride;
        private System.Windows.Forms.CheckBox chkSingleTiled;
        private System.Windows.Forms.Label lblSelColor;
        private Maestro.Editors.Common.ColorComboBox cmbSelectionColor;
        private System.Windows.Forms.CheckBox chkSelectionAsOverlay;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox grpCms;
        private System.Windows.Forms.CheckBox chkGoogHybrid;
        private System.Windows.Forms.CheckBox chkGoogSatellite;
        private System.Windows.Forms.CheckBox chkGoogStreets;
        private System.Windows.Forms.CheckBox chkYahooHybrid;
        private System.Windows.Forms.CheckBox chkYahooSatellite;
        private System.Windows.Forms.CheckBox chkYahooStreets;
        private System.Windows.Forms.CheckBox chkBingHybrid;
        private System.Windows.Forms.CheckBox chkBingSatellite;
        private System.Windows.Forms.CheckBox chkBingStreets;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtYahooApiKey;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkGoogTerrain;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chkOsmMapnik;
        private System.Windows.Forms.CheckBox chkOsmTransportMap;
        private System.Windows.Forms.CheckBox chkOsmCycleMap;
    }
}
