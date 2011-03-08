namespace Maestro.Editors.Diagnostics
{
    partial class ServerStatusMonitor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerStatusMonitor));
            this.pollTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblLastUpdated = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblServerDisplayName = new System.Windows.Forms.Label();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.lblServerVersion = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblOsVersion = new System.Windows.Forms.Label();
            this.lblVirtMemTotal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblVirtMemAvail = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPhysMemTotal = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAvailPhysMem = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblUptime = new System.Windows.Forms.Label();
            this.lblCpuUtil = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblTotalOpsReceived = new System.Windows.Forms.Label();
            this.lblAvgOpTime = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblSiteQueueCount = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblClientQueueCount = new System.Windows.Forms.Label();
            this.lblTotalOpsProcessed = new System.Windows.Forms.Label();
            this.lblAdminQueueCount = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblTotalConnections = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblActiveConnections = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTotalOpTime = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pollTimer
            // 
            this.pollTimer.Interval = 5000;
            this.pollTimer.Tick += new System.EventHandler(this.pollTimer_Tick);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLastUpdated});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.Name = "lblLastUpdated";
            resources.ApplyResources(this.lblLastUpdated, "lblLastUpdated");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblServerDisplayName);
            this.groupBox1.Controls.Add(this.lblServerStatus);
            this.groupBox1.Controls.Add(this.lblServerVersion);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblServerDisplayName
            // 
            resources.ApplyResources(this.lblServerDisplayName, "lblServerDisplayName");
            this.lblServerDisplayName.Name = "lblServerDisplayName";
            // 
            // lblServerStatus
            // 
            resources.ApplyResources(this.lblServerStatus, "lblServerStatus");
            this.lblServerStatus.Name = "lblServerStatus";
            // 
            // lblServerVersion
            // 
            resources.ApplyResources(this.lblServerVersion, "lblServerVersion");
            this.lblServerVersion.Name = "lblServerVersion";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.lblOsVersion);
            this.groupBox2.Controls.Add(this.lblVirtMemTotal);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblVirtMemAvail);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lblPhysMemTotal);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblAvailPhysMem);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // lblOsVersion
            // 
            resources.ApplyResources(this.lblOsVersion, "lblOsVersion");
            this.lblOsVersion.Name = "lblOsVersion";
            // 
            // lblVirtMemTotal
            // 
            resources.ApplyResources(this.lblVirtMemTotal, "lblVirtMemTotal");
            this.lblVirtMemTotal.Name = "lblVirtMemTotal";
            this.lblVirtMemTotal.Tag = "";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // lblVirtMemAvail
            // 
            resources.ApplyResources(this.lblVirtMemAvail, "lblVirtMemAvail");
            this.lblVirtMemAvail.Name = "lblVirtMemAvail";
            this.lblVirtMemAvail.Tag = "";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblPhysMemTotal
            // 
            resources.ApplyResources(this.lblPhysMemTotal, "lblPhysMemTotal");
            this.lblPhysMemTotal.Name = "lblPhysMemTotal";
            this.lblPhysMemTotal.Tag = "";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // lblAvailPhysMem
            // 
            resources.ApplyResources(this.lblAvailPhysMem, "lblAvailPhysMem");
            this.lblAvailPhysMem.Name = "lblAvailPhysMem";
            this.lblAvailPhysMem.Tag = "";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.lblUptime);
            this.groupBox3.Controls.Add(this.lblCpuUtil);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.lblTotalOpsReceived);
            this.groupBox3.Controls.Add(this.lblAvgOpTime);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.lblSiteQueueCount);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.lblClientQueueCount);
            this.groupBox3.Controls.Add(this.lblTotalOpsProcessed);
            this.groupBox3.Controls.Add(this.lblAdminQueueCount);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.lblTotalConnections);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.lblActiveConnections);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lblTotalOpTime);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // lblUptime
            // 
            resources.ApplyResources(this.lblUptime, "lblUptime");
            this.lblUptime.Name = "lblUptime";
            // 
            // lblCpuUtil
            // 
            resources.ApplyResources(this.lblCpuUtil, "lblCpuUtil");
            this.lblCpuUtil.Name = "lblCpuUtil";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // lblTotalOpsReceived
            // 
            resources.ApplyResources(this.lblTotalOpsReceived, "lblTotalOpsReceived");
            this.lblTotalOpsReceived.Name = "lblTotalOpsReceived";
            // 
            // lblAvgOpTime
            // 
            resources.ApplyResources(this.lblAvgOpTime, "lblAvgOpTime");
            this.lblAvgOpTime.Name = "lblAvgOpTime";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // lblSiteQueueCount
            // 
            resources.ApplyResources(this.lblSiteQueueCount, "lblSiteQueueCount");
            this.lblSiteQueueCount.Name = "lblSiteQueueCount";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // lblClientQueueCount
            // 
            resources.ApplyResources(this.lblClientQueueCount, "lblClientQueueCount");
            this.lblClientQueueCount.Name = "lblClientQueueCount";
            // 
            // lblTotalOpsProcessed
            // 
            resources.ApplyResources(this.lblTotalOpsProcessed, "lblTotalOpsProcessed");
            this.lblTotalOpsProcessed.Name = "lblTotalOpsProcessed";
            // 
            // lblAdminQueueCount
            // 
            resources.ApplyResources(this.lblAdminQueueCount, "lblAdminQueueCount");
            this.lblAdminQueueCount.Name = "lblAdminQueueCount";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // lblTotalConnections
            // 
            resources.ApplyResources(this.lblTotalConnections, "lblTotalConnections");
            this.lblTotalConnections.Name = "lblTotalConnections";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lblActiveConnections
            // 
            resources.ApplyResources(this.lblActiveConnections, "lblActiveConnections");
            this.lblActiveConnections.Name = "lblActiveConnections";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // lblTotalOpTime
            // 
            resources.ApplyResources(this.lblTotalOpTime, "lblTotalOpTime");
            this.lblTotalOpTime.Name = "lblTotalOpTime";
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
            // ServerStatusMonitor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerStatusMonitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerStatusMonitor_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer pollTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdated;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblServerDisplayName;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.Label lblServerVersion;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblCpuUtil;
        private System.Windows.Forms.Label lblTotalOpsReceived;
        private System.Windows.Forms.Label lblAvgOpTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblSiteQueueCount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblClientQueueCount;
        private System.Windows.Forms.Label lblTotalOpsProcessed;
        private System.Windows.Forms.Label lblAdminQueueCount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblTotalConnections;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblActiveConnections;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblTotalOpTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblUptime;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lblOsVersion;
        private System.Windows.Forms.Label lblVirtMemTotal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblVirtMemAvail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblPhysMemTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblAvailPhysMem;
        private System.Windows.Forms.Label label4;
    }
}