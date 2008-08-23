namespace OSGeo.MapGuide.Maestro
{
    partial class ResourceProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceProperties));
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SecurityTab = new System.Windows.Forms.TabPage();
            this.UseInherited = new System.Windows.Forms.CheckBox();
            this.UsersAndGroups = new System.Windows.Forms.ListView();
            this.UserAndGroupImages = new System.Windows.Forms.ImageList(this.components);
            this.WMSTab = new System.Windows.Forms.TabPage();
            this.WFSTab = new System.Windows.Forms.TabPage();
            this.CustomTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SecurityTab.SuspendLayout();
            this.WMSTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 381);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(359, 40);
            this.panel1.TabIndex = 0;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.Location = new System.Drawing.Point(183, 8);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKBtn.Location = new System.Drawing.Point(95, 8);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SecurityTab);
            this.tabControl1.Controls.Add(this.WMSTab);
            this.tabControl1.Controls.Add(this.WFSTab);
            this.tabControl1.Controls.Add(this.CustomTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(359, 381);
            this.tabControl1.TabIndex = 1;
            // 
            // SecurityTab
            // 
            this.SecurityTab.Controls.Add(this.UseInherited);
            this.SecurityTab.Controls.Add(this.UsersAndGroups);
            this.SecurityTab.Location = new System.Drawing.Point(4, 22);
            this.SecurityTab.Name = "SecurityTab";
            this.SecurityTab.Padding = new System.Windows.Forms.Padding(3);
            this.SecurityTab.Size = new System.Drawing.Size(352, 342);
            this.SecurityTab.TabIndex = 0;
            this.SecurityTab.Text = "Security";
            this.SecurityTab.UseVisualStyleBackColor = true;
            // 
            // UseInherited
            // 
            this.UseInherited.AutoSize = true;
            this.UseInherited.Location = new System.Drawing.Point(16, 16);
            this.UseInherited.Name = "UseInherited";
            this.UseInherited.Size = new System.Drawing.Size(166, 17);
            this.UseInherited.TabIndex = 1;
            this.UseInherited.Text = "Use inherited security settings";
            this.UseInherited.UseVisualStyleBackColor = true;
            this.UseInherited.CheckedChanged += new System.EventHandler(this.UseInherited_CheckedChanged);
            // 
            // UsersAndGroups
            // 
            this.UsersAndGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.UsersAndGroups.Enabled = false;
            this.UsersAndGroups.Location = new System.Drawing.Point(16, 40);
            this.UsersAndGroups.Name = "UsersAndGroups";
            this.UsersAndGroups.Size = new System.Drawing.Size(320, 240);
            this.UsersAndGroups.SmallImageList = this.UserAndGroupImages;
            this.UsersAndGroups.TabIndex = 0;
            this.UsersAndGroups.UseCompatibleStateImageBehavior = false;
            this.UsersAndGroups.View = System.Windows.Forms.View.List;
            // 
            // UserAndGroupImages
            // 
            this.UserAndGroupImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("UserAndGroupImages.ImageStream")));
            this.UserAndGroupImages.TransparentColor = System.Drawing.Color.Transparent;
            this.UserAndGroupImages.Images.SetKeyName(0, "WriteUser.ico");
            this.UserAndGroupImages.Images.SetKeyName(1, "ReadOnlyUser.ico");
            this.UserAndGroupImages.Images.SetKeyName(2, "DenyUser.ico");
            this.UserAndGroupImages.Images.SetKeyName(3, "WriteGroup.ico");
            this.UserAndGroupImages.Images.SetKeyName(4, "ReadOnlyGroup.ico");
            this.UserAndGroupImages.Images.SetKeyName(5, "DenyGroup.ico");
            // 
            // WMSTab
            // 
            this.WMSTab.Controls.Add(this.groupBox2);
            this.WMSTab.Controls.Add(this.groupBox1);
            this.WMSTab.Location = new System.Drawing.Point(4, 22);
            this.WMSTab.Name = "WMSTab";
            this.WMSTab.Padding = new System.Windows.Forms.Padding(3);
            this.WMSTab.Size = new System.Drawing.Size(351, 355);
            this.WMSTab.TabIndex = 1;
            this.WMSTab.Text = "WMS";
            this.WMSTab.UseVisualStyleBackColor = true;
            // 
            // WFSTab
            // 
            this.WFSTab.Location = new System.Drawing.Point(4, 22);
            this.WFSTab.Name = "WFSTab";
            this.WFSTab.Padding = new System.Windows.Forms.Padding(3);
            this.WFSTab.Size = new System.Drawing.Size(351, 294);
            this.WFSTab.TabIndex = 2;
            this.WFSTab.Text = "WFS";
            this.WFSTab.UseVisualStyleBackColor = true;
            // 
            // CustomTab
            // 
            this.CustomTab.Location = new System.Drawing.Point(4, 22);
            this.CustomTab.Name = "CustomTab";
            this.CustomTab.Size = new System.Drawing.Size(351, 294);
            this.CustomTab.TabIndex = 3;
            this.CustomTab.Text = "Custom Metadata";
            this.CustomTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Metadata";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Keywords";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 24);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Queryable";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(8, 48);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(56, 17);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Visible";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Bounds";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(8, 72);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(64, 17);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "Opaque";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Abstract";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 160);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Location = new System.Drawing.Point(8, 176);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 128);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Functionality";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(88, 96);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(216, 20);
            this.textBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(80, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(248, 20);
            this.textBox2.TabIndex = 8;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(80, 40);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(248, 20);
            this.textBox3.TabIndex = 9;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(80, 64);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox4.Size = new System.Drawing.Size(248, 40);
            this.textBox4.TabIndex = 10;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(80, 112);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox5.Size = new System.Drawing.Size(248, 40);
            this.textBox5.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(304, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 20);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ResourceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 421);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "ResourceProperties";
            this.Text = "ResourceProperties";
            this.Load += new System.EventHandler(this.ResourceProperties_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.SecurityTab.ResumeLayout(false);
            this.SecurityTab.PerformLayout();
            this.WMSTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SecurityTab;
        private System.Windows.Forms.TabPage WMSTab;
        private System.Windows.Forms.TabPage WFSTab;
        private System.Windows.Forms.TabPage CustomTab;
        private System.Windows.Forms.CheckBox UseInherited;
        private System.Windows.Forms.ListView UsersAndGroups;
        private System.Windows.Forms.ImageList UserAndGroupImages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
    }
}