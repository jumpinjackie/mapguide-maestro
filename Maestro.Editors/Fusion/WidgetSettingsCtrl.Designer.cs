namespace Maestro.Editors.Fusion
{
    partial class WidgetSettingsCtrl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WidgetSettingsCtrl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trvWidgets = new System.Windows.Forms.TreeView();
            this.widgetImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddWidget = new System.Windows.Forms.ToolStripDropDownButton();
            this.widgetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flyoutMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddContainer = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveWidget = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnManageWidgets = new System.Windows.Forms.ToolStripButton();
            this.propertiesPanel = new System.Windows.Forms.Panel();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.propertiesPanel);
            this.contentPanel.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.trvWidgets);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // trvWidgets
            // 
            resources.ApplyResources(this.trvWidgets, "trvWidgets");
            this.trvWidgets.ImageList = this.widgetImageList;
            this.trvWidgets.Name = "trvWidgets";
            this.trvWidgets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvWidgets_AfterSelect);
            // 
            // widgetImageList
            // 
            this.widgetImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("widgetImageList.ImageStream")));
            this.widgetImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.widgetImageList.Images.SetKeyName(0, "application.png");
            this.widgetImageList.Images.SetKeyName(1, "gear.png");
            this.widgetImageList.Images.SetKeyName(2, "ui-separator.png");
            this.widgetImageList.Images.SetKeyName(3, "ui-menu.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddWidget,
            this.btnAddContainer,
            this.btnRemoveWidget,
            this.toolStripSeparator1,
            this.btnMoveUp,
            this.btnMoveDown,
            this.toolStripSeparator2,
            this.btnManageWidgets});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddWidget
            // 
            this.btnAddWidget.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddWidget.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.widgetToolStripMenuItem,
            this.separatorToolStripMenuItem,
            this.flyoutMenuToolStripMenuItem});
            this.btnAddWidget.Image = global::Maestro.Editors.Properties.Resources.gear__plus;
            resources.ApplyResources(this.btnAddWidget, "btnAddWidget");
            this.btnAddWidget.Name = "btnAddWidget";
            // 
            // widgetToolStripMenuItem
            // 
            this.widgetToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.gear;
            this.widgetToolStripMenuItem.Name = "widgetToolStripMenuItem";
            resources.ApplyResources(this.widgetToolStripMenuItem, "widgetToolStripMenuItem");
            this.widgetToolStripMenuItem.Click += new System.EventHandler(this.widgetToolStripMenuItem_Click);
            // 
            // separatorToolStripMenuItem
            // 
            this.separatorToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.ui_separator;
            this.separatorToolStripMenuItem.Name = "separatorToolStripMenuItem";
            resources.ApplyResources(this.separatorToolStripMenuItem, "separatorToolStripMenuItem");
            this.separatorToolStripMenuItem.Click += new System.EventHandler(this.separatorToolStripMenuItem_Click);
            // 
            // flyoutMenuToolStripMenuItem
            // 
            this.flyoutMenuToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.ui_menu;
            this.flyoutMenuToolStripMenuItem.Name = "flyoutMenuToolStripMenuItem";
            resources.ApplyResources(this.flyoutMenuToolStripMenuItem, "flyoutMenuToolStripMenuItem");
            this.flyoutMenuToolStripMenuItem.Click += new System.EventHandler(this.flyoutMenuToolStripMenuItem_Click);
            // 
            // btnAddContainer
            // 
            this.btnAddContainer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddContainer.Image = global::Maestro.Editors.Properties.Resources.application__plus;
            resources.ApplyResources(this.btnAddContainer, "btnAddContainer");
            this.btnAddContainer.Name = "btnAddContainer";
            this.btnAddContainer.Click += new System.EventHandler(this.btnAddContainer_Click);
            // 
            // btnRemoveWidget
            // 
            this.btnRemoveWidget.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnRemoveWidget, "btnRemoveWidget");
            this.btnRemoveWidget.Image = global::Maestro.Editors.Properties.Resources.gear__minus;
            this.btnRemoveWidget.Name = "btnRemoveWidget";
            this.btnRemoveWidget.Click += new System.EventHandler(this.btnRemoveWidget_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnManageWidgets
            // 
            this.btnManageWidgets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManageWidgets.Image = global::Maestro.Editors.Properties.Resources.gear;
            resources.ApplyResources(this.btnManageWidgets, "btnManageWidgets");
            this.btnManageWidgets.Name = "btnManageWidgets";
            this.btnManageWidgets.Click += new System.EventHandler(this.btnManageWidgets_Click);
            // 
            // propertiesPanel
            // 
            resources.ApplyResources(this.propertiesPanel, "propertiesPanel");
            this.propertiesPanel.Name = "propertiesPanel";
            // 
            // WidgetSettingsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Widgets";
            this.Name = "WidgetSettingsCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel propertiesPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView trvWidgets;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton btnAddWidget;
        private System.Windows.Forms.ToolStripButton btnAddContainer;
        private System.Windows.Forms.ToolStripButton btnRemoveWidget;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnManageWidgets;
        private System.Windows.Forms.ImageList widgetImageList;
        private System.Windows.Forms.ToolStripMenuItem widgetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem separatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flyoutMenuToolStripMenuItem;
    }
}
