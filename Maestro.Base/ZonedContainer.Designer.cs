namespace Maestro.Base
{
    partial class ZonedContainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZonedContainer));
            this.topContainer = new System.Windows.Forms.SplitContainer();
            this.leftZone = new System.Windows.Forms.TabControl();
            this.leftImgList = new System.Windows.Forms.ImageList(this.components);
            this.docRightContainer = new System.Windows.Forms.SplitContainer();
            this.docBottomContainer = new System.Windows.Forms.SplitContainer();
            this.documentTabs = new System.Windows.Forms.TabControl();
            this.docImgList = new System.Windows.Forms.ImageList(this.components);
            this.bottomZone = new System.Windows.Forms.TabControl();
            this.bottomImgList = new System.Windows.Forms.ImageList(this.components);
            this.rightZone = new System.Windows.Forms.TabControl();
            this.rightImgList = new System.Windows.Forms.ImageList(this.components);
            this.topContainer.Panel1.SuspendLayout();
            this.topContainer.Panel2.SuspendLayout();
            this.topContainer.SuspendLayout();
            this.docRightContainer.Panel1.SuspendLayout();
            this.docRightContainer.Panel2.SuspendLayout();
            this.docRightContainer.SuspendLayout();
            this.docBottomContainer.Panel1.SuspendLayout();
            this.docBottomContainer.Panel2.SuspendLayout();
            this.docBottomContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // topContainer
            // 
            resources.ApplyResources(this.topContainer, "topContainer");
            this.topContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.topContainer.Name = "topContainer";
            // 
            // topContainer.Panel1
            // 
            this.topContainer.Panel1.Controls.Add(this.leftZone);
            // 
            // topContainer.Panel2
            // 
            this.topContainer.Panel2.Controls.Add(this.docRightContainer);
            // 
            // leftZone
            // 
            resources.ApplyResources(this.leftZone, "leftZone");
            this.leftZone.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.leftZone.HotTrack = true;
            this.leftZone.ImageList = this.leftImgList;
            this.leftZone.Name = "leftZone";
            this.leftZone.SelectedIndex = 0;
            this.leftZone.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ZoneDrawItem);
            this.leftZone.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ZoneMouseClick);
            this.leftZone.TabIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            this.leftZone.SelectedIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            // 
            // leftImgList
            // 
            this.leftImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.leftImgList, "leftImgList");
            this.leftImgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // docRightContainer
            // 
            resources.ApplyResources(this.docRightContainer, "docRightContainer");
            this.docRightContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.docRightContainer.Name = "docRightContainer";
            // 
            // docRightContainer.Panel1
            // 
            this.docRightContainer.Panel1.Controls.Add(this.docBottomContainer);
            resources.ApplyResources(this.docRightContainer.Panel1, "docRightContainer.Panel1");
            // 
            // docRightContainer.Panel2
            // 
            this.docRightContainer.Panel2.Controls.Add(this.rightZone);
            resources.ApplyResources(this.docRightContainer.Panel2, "docRightContainer.Panel2");
            // 
            // docBottomContainer
            // 
            resources.ApplyResources(this.docBottomContainer, "docBottomContainer");
            this.docBottomContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.docBottomContainer.Name = "docBottomContainer";
            // 
            // docBottomContainer.Panel1
            // 
            this.docBottomContainer.Panel1.Controls.Add(this.documentTabs);
            resources.ApplyResources(this.docBottomContainer.Panel1, "docBottomContainer.Panel1");
            // 
            // docBottomContainer.Panel2
            // 
            this.docBottomContainer.Panel2.Controls.Add(this.bottomZone);
            resources.ApplyResources(this.docBottomContainer.Panel2, "docBottomContainer.Panel2");
            // 
            // documentTabs
            // 
            resources.ApplyResources(this.documentTabs, "documentTabs");
            this.documentTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.documentTabs.HotTrack = true;
            this.documentTabs.ImageList = this.docImgList;
            this.documentTabs.Name = "documentTabs";
            this.documentTabs.SelectedIndex = 0;
            this.documentTabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ZoneDrawItem);
            this.documentTabs.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ZoneMouseClick);
            this.documentTabs.TabIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            this.documentTabs.SelectedIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            // 
            // docImgList
            // 
            this.docImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.docImgList, "docImgList");
            this.docImgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // bottomZone
            // 
            resources.ApplyResources(this.bottomZone, "bottomZone");
            this.bottomZone.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.bottomZone.HotTrack = true;
            this.bottomZone.ImageList = this.bottomImgList;
            this.bottomZone.Name = "bottomZone";
            this.bottomZone.SelectedIndex = 0;
            this.bottomZone.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ZoneDrawItem);
            this.bottomZone.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ZoneMouseClick);
            this.bottomZone.TabIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            this.bottomZone.SelectedIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            // 
            // bottomImgList
            // 
            this.bottomImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.bottomImgList, "bottomImgList");
            this.bottomImgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // rightZone
            // 
            resources.ApplyResources(this.rightZone, "rightZone");
            this.rightZone.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.rightZone.HotTrack = true;
            this.rightZone.ImageList = this.rightImgList;
            this.rightZone.Name = "rightZone";
            this.rightZone.SelectedIndex = 0;
            this.rightZone.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ZoneDrawItem);
            this.rightZone.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ZoneMouseClick);
            this.rightZone.TabIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            this.rightZone.SelectedIndexChanged += new System.EventHandler(this.ZoneTabSelectedIndexChanged);
            // 
            // rightImgList
            // 
            this.rightImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.rightImgList, "rightImgList");
            this.rightImgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ZonedContainer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.topContainer);
            this.Name = "ZonedContainer";
            resources.ApplyResources(this, "$this");
            this.topContainer.Panel1.ResumeLayout(false);
            this.topContainer.Panel2.ResumeLayout(false);
            this.topContainer.ResumeLayout(false);
            this.docRightContainer.Panel1.ResumeLayout(false);
            this.docRightContainer.Panel2.ResumeLayout(false);
            this.docRightContainer.ResumeLayout(false);
            this.docBottomContainer.Panel1.ResumeLayout(false);
            this.docBottomContainer.Panel2.ResumeLayout(false);
            this.docBottomContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer topContainer;
        private System.Windows.Forms.SplitContainer docRightContainer;
        private System.Windows.Forms.SplitContainer docBottomContainer;
        private System.Windows.Forms.TabControl documentTabs;
        private System.Windows.Forms.TabControl leftZone;
        private System.Windows.Forms.TabControl bottomZone;
        private System.Windows.Forms.TabControl rightZone;
        private System.Windows.Forms.ImageList leftImgList;
        private System.Windows.Forms.ImageList rightImgList;
        private System.Windows.Forms.ImageList docImgList;
        private System.Windows.Forms.ImageList bottomImgList;
    }
}
