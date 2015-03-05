namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class SymbolInstancesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolInstancesDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.grpComponents = new System.Windows.Forms.GroupBox();
            this.lstInstances = new System.Windows.Forms.ListView();
            this.imgPreviews = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.referenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inlineCompoundSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inlineSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solidFillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textLabelPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textLabelLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textLabelPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.inlineSimpleSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnSaveExternal = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditInstanceProperties = new System.Windows.Forms.ToolStripButton();
            this.btnEditComponent = new System.Windows.Forms.ToolStripButton();
            this.grpProperties = new System.Windows.Forms.GroupBox();
            this.grdOverrides = new System.Windows.Forms.DataGridView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnAddProperty = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEditProperty = new System.Windows.Forms.ToolStripDropDownButton();
            this.viaEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viaExpressionEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDeleteProperty = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPropertyInfo = new System.Windows.Forms.ToolStripButton();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.symPreview = new System.Windows.Forms.PictureBox();
            this.btnEditAsXml = new System.Windows.Forms.Button();
            this.grpComponents.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.grpProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOverrides)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.grpPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.symPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpComponents
            // 
            resources.ApplyResources(this.grpComponents, "grpComponents");
            this.grpComponents.Controls.Add(this.lstInstances);
            this.grpComponents.Controls.Add(this.toolStrip1);
            this.grpComponents.Name = "grpComponents";
            this.grpComponents.TabStop = false;
            // 
            // lstInstances
            // 
            resources.ApplyResources(this.lstInstances, "lstInstances");
            this.lstInstances.LargeImageList = this.imgPreviews;
            this.lstInstances.MultiSelect = false;
            this.lstInstances.Name = "lstInstances";
            this.lstInstances.ShowGroups = false;
            this.lstInstances.SmallImageList = this.imgPreviews;
            this.lstInstances.UseCompatibleStateImageBehavior = false;
            this.lstInstances.View = System.Windows.Forms.View.List;
            this.lstInstances.SelectedIndexChanged += new System.EventHandler(this.lstInstances_SelectedIndexChanged);
            // 
            // imgPreviews
            // 
            this.imgPreviews.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imgPreviews, "imgPreviews");
            this.imgPreviews.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.btnSaveExternal,
            this.toolStripSeparator2,
            this.btnEditInstanceProperties,
            this.btnEditComponent});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.referenceToolStripMenuItem,
            this.inlineCompoundSymbolToolStripMenuItem,
            this.inlineSymbolToolStripMenuItem});
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            // 
            // referenceToolStripMenuItem
            // 
            this.referenceToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.arrow;
            this.referenceToolStripMenuItem.Name = "referenceToolStripMenuItem";
            resources.ApplyResources(this.referenceToolStripMenuItem, "referenceToolStripMenuItem");
            this.referenceToolStripMenuItem.Click += new System.EventHandler(this.referenceToolStripMenuItem_Click);
            // 
            // inlineCompoundSymbolToolStripMenuItem
            // 
            this.inlineCompoundSymbolToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.marker;
            this.inlineCompoundSymbolToolStripMenuItem.Name = "inlineCompoundSymbolToolStripMenuItem";
            resources.ApplyResources(this.inlineCompoundSymbolToolStripMenuItem, "inlineCompoundSymbolToolStripMenuItem");
            this.inlineCompoundSymbolToolStripMenuItem.Click += new System.EventHandler(this.inlineCompoundSymbolToolStripMenuItem_Click);
            // 
            // inlineSymbolToolStripMenuItem
            // 
            this.inlineSymbolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solidFillToolStripMenuItem,
            this.lineToolStripMenuItem,
            this.pointToolStripMenuItem,
            this.textLabelPointToolStripMenuItem,
            this.textLabelLineToolStripMenuItem,
            this.textLabelPolygonToolStripMenuItem,
            this.toolStripSeparator4,
            this.inlineSimpleSymbolToolStripMenuItem});
            this.inlineSymbolToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.marker;
            this.inlineSymbolToolStripMenuItem.Name = "inlineSymbolToolStripMenuItem";
            resources.ApplyResources(this.inlineSymbolToolStripMenuItem, "inlineSymbolToolStripMenuItem");
            // 
            // solidFillToolStripMenuItem
            // 
            this.solidFillToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer_shape;
            this.solidFillToolStripMenuItem.Name = "solidFillToolStripMenuItem";
            resources.ApplyResources(this.solidFillToolStripMenuItem, "solidFillToolStripMenuItem");
            this.solidFillToolStripMenuItem.Click += new System.EventHandler(this.solidFillToolStripMenuItem_Click);
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer_shape_line;
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            resources.ApplyResources(this.lineToolStripMenuItem, "lineToolStripMenuItem");
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.lineToolStripMenuItem_Click);
            // 
            // pointToolStripMenuItem
            // 
            this.pointToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer_small;
            this.pointToolStripMenuItem.Name = "pointToolStripMenuItem";
            resources.ApplyResources(this.pointToolStripMenuItem, "pointToolStripMenuItem");
            this.pointToolStripMenuItem.Click += new System.EventHandler(this.pointToolStripMenuItem_Click);
            // 
            // textLabelPointToolStripMenuItem
            // 
            this.textLabelPointToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.edit;
            this.textLabelPointToolStripMenuItem.Name = "textLabelPointToolStripMenuItem";
            resources.ApplyResources(this.textLabelPointToolStripMenuItem, "textLabelPointToolStripMenuItem");
            this.textLabelPointToolStripMenuItem.Click += new System.EventHandler(this.textLabelPointToolStripMenuItem_Click);
            // 
            // textLabelLineToolStripMenuItem
            // 
            this.textLabelLineToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.edit;
            this.textLabelLineToolStripMenuItem.Name = "textLabelLineToolStripMenuItem";
            resources.ApplyResources(this.textLabelLineToolStripMenuItem, "textLabelLineToolStripMenuItem");
            this.textLabelLineToolStripMenuItem.Click += new System.EventHandler(this.textLabelLineToolStripMenuItem_Click);
            // 
            // textLabelPolygonToolStripMenuItem
            // 
            this.textLabelPolygonToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.edit;
            this.textLabelPolygonToolStripMenuItem.Name = "textLabelPolygonToolStripMenuItem";
            resources.ApplyResources(this.textLabelPolygonToolStripMenuItem, "textLabelPolygonToolStripMenuItem");
            this.textLabelPolygonToolStripMenuItem.Click += new System.EventHandler(this.textLabelPolygonToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // inlineSimpleSymbolToolStripMenuItem
            // 
            this.inlineSimpleSymbolToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.marker;
            this.inlineSimpleSymbolToolStripMenuItem.Name = "inlineSimpleSymbolToolStripMenuItem";
            resources.ApplyResources(this.inlineSimpleSymbolToolStripMenuItem, "inlineSimpleSymbolToolStripMenuItem");
            this.inlineSimpleSymbolToolStripMenuItem.Click += new System.EventHandler(this.inlineSimpleSymbolToolStripMenuItem_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSaveExternal
            // 
            resources.ApplyResources(this.btnSaveExternal, "btnSaveExternal");
            this.btnSaveExternal.Image = global::Maestro.Editors.Properties.Resources.disk;
            this.btnSaveExternal.Name = "btnSaveExternal";
            this.btnSaveExternal.Click += new System.EventHandler(this.btnSaveExternal_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnEditInstanceProperties
            // 
            resources.ApplyResources(this.btnEditInstanceProperties, "btnEditInstanceProperties");
            this.btnEditInstanceProperties.Image = global::Maestro.Editors.Properties.Resources.document__pencil;
            this.btnEditInstanceProperties.Name = "btnEditInstanceProperties";
            this.btnEditInstanceProperties.Click += new System.EventHandler(this.btnEditInstanceProperties_Click);
            // 
            // btnEditComponent
            // 
            resources.ApplyResources(this.btnEditComponent, "btnEditComponent");
            this.btnEditComponent.Image = global::Maestro.Editors.Properties.Resources.document__pencil;
            this.btnEditComponent.Name = "btnEditComponent";
            this.btnEditComponent.Click += new System.EventHandler(this.btnEditComponent_Click);
            // 
            // grpProperties
            // 
            resources.ApplyResources(this.grpProperties, "grpProperties");
            this.grpProperties.Controls.Add(this.grdOverrides);
            this.grpProperties.Controls.Add(this.toolStrip2);
            this.grpProperties.Name = "grpProperties";
            this.grpProperties.TabStop = false;
            // 
            // grdOverrides
            // 
            this.grdOverrides.AllowUserToAddRows = false;
            this.grdOverrides.AllowUserToDeleteRows = false;
            this.grdOverrides.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.grdOverrides, "grdOverrides");
            this.grdOverrides.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdOverrides.Name = "grdOverrides";
            this.grdOverrides.SelectionChanged += new System.EventHandler(this.grdOverrides_SelectionChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddProperty,
            this.btnEditProperty,
            this.btnDeleteProperty,
            this.toolStripSeparator3,
            this.btnPropertyInfo});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // btnAddProperty
            // 
            this.btnAddProperty.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.refreshToolStripMenuItem});
            this.btnAddProperty.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAddProperty, "btnAddProperty");
            this.btnAddProperty.Name = "btnAddProperty";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.arrow_circle_135;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            resources.ApplyResources(this.refreshToolStripMenuItem, "refreshToolStripMenuItem");
            // 
            // btnEditProperty
            // 
            this.btnEditProperty.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viaEditorToolStripMenuItem,
            this.viaExpressionEditorToolStripMenuItem});
            resources.ApplyResources(this.btnEditProperty, "btnEditProperty");
            this.btnEditProperty.Image = global::Maestro.Editors.Properties.Resources.document__pencil;
            this.btnEditProperty.Name = "btnEditProperty";
            // 
            // viaEditorToolStripMenuItem
            // 
            this.viaEditorToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.application_form;
            this.viaEditorToolStripMenuItem.Name = "viaEditorToolStripMenuItem";
            resources.ApplyResources(this.viaEditorToolStripMenuItem, "viaEditorToolStripMenuItem");
            this.viaEditorToolStripMenuItem.Click += new System.EventHandler(this.viaEditorToolStripMenuItem_Click);
            // 
            // viaExpressionEditorToolStripMenuItem
            // 
            this.viaExpressionEditorToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.sum;
            this.viaExpressionEditorToolStripMenuItem.Name = "viaExpressionEditorToolStripMenuItem";
            resources.ApplyResources(this.viaExpressionEditorToolStripMenuItem, "viaExpressionEditorToolStripMenuItem");
            this.viaExpressionEditorToolStripMenuItem.Click += new System.EventHandler(this.viaExpressionEditorToolStripMenuItem_Click);
            // 
            // btnDeleteProperty
            // 
            resources.ApplyResources(this.btnDeleteProperty, "btnDeleteProperty");
            this.btnDeleteProperty.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDeleteProperty.Name = "btnDeleteProperty";
            this.btnDeleteProperty.Click += new System.EventHandler(this.btnDeleteProperty_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // btnPropertyInfo
            // 
            resources.ApplyResources(this.btnPropertyInfo, "btnPropertyInfo");
            this.btnPropertyInfo.Image = global::Maestro.Editors.Properties.Resources.information;
            this.btnPropertyInfo.Name = "btnPropertyInfo";
            this.btnPropertyInfo.Click += new System.EventHandler(this.btnPropertyInfo_Click);
            // 
            // grpPreview
            // 
            resources.ApplyResources(this.grpPreview, "grpPreview");
            this.grpPreview.Controls.Add(this.lnkRefresh);
            this.grpPreview.Controls.Add(this.symPreview);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.TabStop = false;
            // 
            // lnkRefresh
            // 
            resources.ApplyResources(this.lnkRefresh, "lnkRefresh");
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
            // 
            // symPreview
            // 
            resources.ApplyResources(this.symPreview, "symPreview");
            this.symPreview.Name = "symPreview";
            this.symPreview.TabStop = false;
            this.symPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPicture_Paint);
            // 
            // btnEditAsXml
            // 
            resources.ApplyResources(this.btnEditAsXml, "btnEditAsXml");
            this.btnEditAsXml.Name = "btnEditAsXml";
            this.btnEditAsXml.UseVisualStyleBackColor = true;
            this.btnEditAsXml.Click += new System.EventHandler(this.btnEditAsXml_Click);
            // 
            // SymbolInstancesDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpProperties);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.btnEditAsXml);
            this.Controls.Add(this.grpComponents);
            this.Controls.Add(this.btnClose);
            this.Name = "SymbolInstancesDialog";
            this.grpComponents.ResumeLayout(false);
            this.grpComponents.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grpProperties.ResumeLayout(false);
            this.grpProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOverrides)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.grpPreview.ResumeLayout(false);
            this.grpPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.symPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox grpComponents;
        private System.Windows.Forms.ListView lstInstances;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem referenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inlineCompoundSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ImageList imgPreviews;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.PictureBox symPreview;
        private System.Windows.Forms.Button btnEditAsXml;
        private System.Windows.Forms.LinkLabel lnkRefresh;
        private System.Windows.Forms.GroupBox grpProperties;
        private System.Windows.Forms.DataGridView grdOverrides;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripDropDownButton btnAddProperty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnDeleteProperty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnEditComponent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnPropertyInfo;
        private System.Windows.Forms.ToolStripMenuItem inlineSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solidFillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem inlineSimpleSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textLabelPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textLabelLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textLabelPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnEditInstanceProperties;
        private System.Windows.Forms.ToolStripDropDownButton btnEditProperty;
        private System.Windows.Forms.ToolStripMenuItem viaEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viaExpressionEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnSaveExternal;
    }
}