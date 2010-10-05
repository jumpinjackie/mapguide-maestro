namespace OSGeo.MapGuide.Maestro
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
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Display Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Version";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLastUpdated});
            this.statusStrip1.Location = new System.Drawing.Point(0, 311);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(620, 22);
            this.statusStrip1.TabIndex = 3;
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblServerDisplayName);
            this.groupBox1.Controls.Add(this.lblServerStatus);
            this.groupBox1.Controls.Add(this.lblServerVersion);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 97);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // lblServerDisplayName
            // 
            this.lblServerDisplayName.AutoSize = true;
            this.lblServerDisplayName.Location = new System.Drawing.Point(162, 25);
            this.lblServerDisplayName.Name = "lblServerDisplayName";
            this.lblServerDisplayName.Size = new System.Drawing.Size(0, 13);
            this.lblServerDisplayName.TabIndex = 3;
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(162, 50);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(0, 13);
            this.lblServerStatus.TabIndex = 4;
            // 
            // lblServerVersion
            // 
            this.lblServerVersion.AutoSize = true;
            this.lblServerVersion.Location = new System.Drawing.Point(162, 74);
            this.lblServerVersion.Name = "lblServerVersion";
            this.lblServerVersion.Size = new System.Drawing.Size(0, 13);
            this.lblServerVersion.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.lblOsVersion);
            this.groupBox2.Controls.Add(this.lblVirtMemTotal);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblVirtMemAvail);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lblPhysMemTotal);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblAvailPhysMem);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 181);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Operating System";
            // 
            // lblOsVersion
            // 
            this.lblOsVersion.AutoSize = true;
            this.lblOsVersion.Location = new System.Drawing.Point(18, 26);
            this.lblOsVersion.Name = "lblOsVersion";
            this.lblOsVersion.Size = new System.Drawing.Size(0, 13);
            this.lblOsVersion.TabIndex = 21;
            // 
            // lblVirtMemTotal
            // 
            this.lblVirtMemTotal.AutoSize = true;
            this.lblVirtMemTotal.Location = new System.Drawing.Point(182, 133);
            this.lblVirtMemTotal.Name = "lblVirtMemTotal";
            this.lblVirtMemTotal.Size = new System.Drawing.Size(0, 13);
            this.lblVirtMemTotal.TabIndex = 20;
            this.lblVirtMemTotal.Tag = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(18, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Total Virtual Memory";
            // 
            // lblVirtMemAvail
            // 
            this.lblVirtMemAvail.AutoSize = true;
            this.lblVirtMemAvail.Location = new System.Drawing.Point(182, 110);
            this.lblVirtMemAvail.Name = "lblVirtMemAvail";
            this.lblVirtMemAvail.Size = new System.Drawing.Size(0, 13);
            this.lblVirtMemAvail.TabIndex = 19;
            this.lblVirtMemAvail.Tag = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Available Virtual Memory";
            // 
            // lblPhysMemTotal
            // 
            this.lblPhysMemTotal.AutoSize = true;
            this.lblPhysMemTotal.Location = new System.Drawing.Point(182, 86);
            this.lblPhysMemTotal.Name = "lblPhysMemTotal";
            this.lblPhysMemTotal.Size = new System.Drawing.Size(0, 13);
            this.lblPhysMemTotal.TabIndex = 18;
            this.lblPhysMemTotal.Tag = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Total Physical Memory";
            // 
            // lblAvailPhysMem
            // 
            this.lblAvailPhysMem.AutoSize = true;
            this.lblAvailPhysMem.Location = new System.Drawing.Point(182, 62);
            this.lblAvailPhysMem.Name = "lblAvailPhysMem";
            this.lblAvailPhysMem.Size = new System.Drawing.Size(0, 13);
            this.lblAvailPhysMem.TabIndex = 17;
            this.lblAvailPhysMem.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Available Physical Memory";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBox3.Location = new System.Drawing.Point(306, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(302, 284);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Statistics";
            // 
            // lblUptime
            // 
            this.lblUptime.AutoSize = true;
            this.lblUptime.Location = new System.Drawing.Point(220, 254);
            this.lblUptime.Name = "lblUptime";
            this.lblUptime.Size = new System.Drawing.Size(0, 13);
            this.lblUptime.TabIndex = 13;
            // 
            // lblCpuUtil
            // 
            this.lblCpuUtil.AutoSize = true;
            this.lblCpuUtil.Location = new System.Drawing.Point(220, 118);
            this.lblCpuUtil.Name = "lblCpuUtil";
            this.lblCpuUtil.Size = new System.Drawing.Size(0, 13);
            this.lblCpuUtil.TabIndex = 16;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(18, 254);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(46, 13);
            this.label25.TabIndex = 12;
            this.label25.Text = "Uptime";
            // 
            // lblTotalOpsReceived
            // 
            this.lblTotalOpsReceived.AutoSize = true;
            this.lblTotalOpsReceived.Location = new System.Drawing.Point(220, 234);
            this.lblTotalOpsReceived.Name = "lblTotalOpsReceived";
            this.lblTotalOpsReceived.Size = new System.Drawing.Size(0, 13);
            this.lblTotalOpsReceived.TabIndex = 4;
            // 
            // lblAvgOpTime
            // 
            this.lblAvgOpTime.AutoSize = true;
            this.lblAvgOpTime.Location = new System.Drawing.Point(220, 96);
            this.lblAvgOpTime.Name = "lblAvgOpTime";
            this.lblAvgOpTime.Size = new System.Drawing.Size(0, 13);
            this.lblAvgOpTime.TabIndex = 15;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(18, 234);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(159, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Total Operations Received";
            // 
            // lblSiteQueueCount
            // 
            this.lblSiteQueueCount.AutoSize = true;
            this.lblSiteQueueCount.Location = new System.Drawing.Point(220, 73);
            this.lblSiteQueueCount.Name = "lblSiteQueueCount";
            this.lblSiteQueueCount.Size = new System.Drawing.Size(0, 13);
            this.lblSiteQueueCount.TabIndex = 14;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(18, 212);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(164, 13);
            this.label15.TabIndex = 13;
            this.label15.Text = "Total Operations Processed";
            // 
            // lblClientQueueCount
            // 
            this.lblClientQueueCount.AutoSize = true;
            this.lblClientQueueCount.Location = new System.Drawing.Point(220, 49);
            this.lblClientQueueCount.Name = "lblClientQueueCount";
            this.lblClientQueueCount.Size = new System.Drawing.Size(0, 13);
            this.lblClientQueueCount.TabIndex = 13;
            // 
            // lblTotalOpsProcessed
            // 
            this.lblTotalOpsProcessed.AutoSize = true;
            this.lblTotalOpsProcessed.Location = new System.Drawing.Point(220, 212);
            this.lblTotalOpsProcessed.Name = "lblTotalOpsProcessed";
            this.lblTotalOpsProcessed.Size = new System.Drawing.Size(0, 13);
            this.lblTotalOpsProcessed.TabIndex = 3;
            // 
            // lblAdminQueueCount
            // 
            this.lblAdminQueueCount.AutoSize = true;
            this.lblAdminQueueCount.Location = new System.Drawing.Point(220, 25);
            this.lblAdminQueueCount.Name = "lblAdminQueueCount";
            this.lblAdminQueueCount.Size = new System.Drawing.Size(0, 13);
            this.lblAdminQueueCount.TabIndex = 12;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(18, 189);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(110, 13);
            this.label16.TabIndex = 12;
            this.label16.Text = "Total Connections";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(18, 165);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(117, 13);
            this.label17.TabIndex = 11;
            this.label17.Text = "Active Connections";
            // 
            // lblTotalConnections
            // 
            this.lblTotalConnections.AutoSize = true;
            this.lblTotalConnections.Location = new System.Drawing.Point(220, 189);
            this.lblTotalConnections.Name = "lblTotalConnections";
            this.lblTotalConnections.Size = new System.Drawing.Size(0, 13);
            this.lblTotalConnections.TabIndex = 2;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(18, 141);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(126, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "Total Operation Time";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(18, 118);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "CPU Utilization";
            // 
            // lblActiveConnections
            // 
            this.lblActiveConnections.AutoSize = true;
            this.lblActiveConnections.Location = new System.Drawing.Point(220, 165);
            this.lblActiveConnections.Name = "lblActiveConnections";
            this.lblActiveConnections.Size = new System.Drawing.Size(0, 13);
            this.lblActiveConnections.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(18, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Avg. Operation Time";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(18, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(166, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Site Operation Queue Count";
            // 
            // lblTotalOpTime
            // 
            this.lblTotalOpTime.AutoSize = true;
            this.lblTotalOpTime.Location = new System.Drawing.Point(220, 141);
            this.lblTotalOpTime.Name = "lblTotalOpTime";
            this.lblTotalOpTime.Size = new System.Drawing.Size(0, 13);
            this.lblTotalOpTime.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(18, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(176, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Client Operation Queue Count";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(18, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(178, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "Admin Operation Queue Count";
            // 
            // ServerStatusMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 333);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerStatusMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Status";
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