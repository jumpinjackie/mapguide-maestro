#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for LayerEditor.
	/// </summary>
	public class LayerEditor : System.Windows.Forms.UserControl, IResourceEditorControl
	{

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FeatureSource;

		private OSGeo.MapGuide.MaestroAPI.LayerDefinition m_layer;
		private bool inUpdate = false;
		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription m_schema = null;

		
		private System.Data.DataSet PropertyDataset;
		private System.Data.DataTable ViewerPropertiesTable;
		private System.Data.DataColumn PropertyColumnVisible;
		private System.Data.DataColumn PropertyColumnDisplay;
		private System.Data.DataColumn PropertyColumnName;
		private System.Data.DataSet DisplayRanges;
		private System.Data.DataTable DisplayRangesTable;
		private System.Data.DataColumn FromScaleColumn;
		private System.Data.DataColumn ToScaleColumn;
		private System.Data.DataColumn StylizationColumn;
		private System.Windows.Forms.Button BrowseFeatureResource;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		private System.Data.DataColumn dataColumn3;
		private System.Windows.Forms.Panel EditorPanel;
		private ResourceEditors.LayerEditorControls.RasterLayer rasterLayer;
        private Label label2;
        private ComboBox LayerDefinitionVersion;
        private ResourceEditors.LayerEditorControls.VectorLayer vectorLayer;

		
		public LayerEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_layer = new OSGeo.MapGuide.MaestroAPI.LayerDefinition();

			vectorLayer.SetItem(m_editor, m_layer, null);
			rasterLayer.SetItem(m_editor, m_layer, null);

            //TODO: Do we want to limit the user here?
            /*if (m_editor.CurrentConnection.SiteVersion < OSGeo.MapGuide.MaestroAPI.SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideEP2010))
                LayerDefinitionVersion.Items.Remove("1.3.0");

            if (m_editor.CurrentConnection.SiteVersion < OSGeo.MapGuide.MaestroAPI.SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideOS1_2))
                LayerDefinitionVersion.Items.Remove("1.2.0");*/

			UpdateDisplay();
		}

		private void RefreshSchema(string resourceID)
		{
			try
			{
				m_schema = null;
				if (resourceID == null || resourceID == "" || resourceID.Equals(Strings.LayerEditor.SelectFeatureSourceHint))
					return;

				m_schema = m_editor.CurrentConnection.DescribeFeatureSource(resourceID);
				bool isRaster = false;
				foreach(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema sc in m_schema.Schemas)
					foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn fsc in sc.Columns)
						if (fsc.Type == typeof(Bitmap))
						{
							isRaster = true;
							break;
						}

				if (isRaster && m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				{
					OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld = new OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType();
					gld.ResourceId = resourceID;
					gld.GridScaleRange = new OSGeo.MapGuide.MaestroAPI.GridScaleRangeTypeCollection();
					OSGeo.MapGuide.MaestroAPI.GridScaleRangeType gl = new OSGeo.MapGuide.MaestroAPI.GridScaleRangeType();
					gld.GridScaleRange.Add(gl);
					gl.RebuildFactor = 1;
					m_layer.Item = gld;
				}
				else if (!isRaster && m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType == null)
				{
					m_layer.Item = new OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType();
					m_layer.Item.ResourceId = resourceID;
				}

                if (isRaster)
                    rasterLayer.SetItem(m_editor, m_layer, m_schema);
                else
                    vectorLayer.SetItem(m_editor, m_layer, m_schema);
			}
			catch (Exception ex)
			{
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(Strings.LayerEditor.SchemaReadError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
			}

		}

		public LayerEditor(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			m_layer = editor.CurrentConnection.GetLayerDefinition(resourceID);

			if (m_layer.Item != null)
			{
				try 
				{
					inUpdate = true;
					FeatureSource.Text = m_layer.Item.ResourceId;
					RefreshSchema(m_layer.Item.ResourceId);
				}
				finally
				{
					inUpdate = false;
				}
			}

			if (m_schema != null)
			{
				vectorLayer.SetItem(m_editor, m_layer, m_schema);
				rasterLayer.SetItem(m_editor, m_layer, m_schema);
			}

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if (inUpdate)
				return;
			try
			{
				inUpdate = true;

				if (m_layer != null && m_layer.Item != null && m_layer.Item.ResourceId != null && m_layer.Item.ResourceId.Length > 0)
					FeatureSource.Text = m_layer.Item.ResourceId;
				else
					FeatureSource.Text = Strings.LayerEditor.SelectFeatureSourceHint;

				if (m_schema == null)
				{
					vectorLayer.Visible = false;
					rasterLayer.Visible = false;
					return;
				}

				if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
				{
					OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;
					FeatureSource.Text = vl.ResourceId;

					vectorLayer.Visible = true;
					rasterLayer.Visible = false;
					vectorLayer.UpdateDisplay();
				}
				else if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType != null)
				{
					vectorLayer.Visible = false;
					rasterLayer.Visible = true;
					rasterLayer.UpdateDisplay();
				}
				else
				{
					vectorLayer.Visible = false;
					rasterLayer.Visible = false;
				}

                if (m_layer.version == "1.0.0")
                {
                    m_layer.version = "1.1.0";
                    m_editor.HasChanged();
                }
                LayerDefinitionVersion.SelectedIndex = LayerDefinitionVersion.FindStringExact(m_layer.version);

			} 
			finally
			{
				inUpdate = false;
			}
		}

		private System.ComponentModel.IContainer components;

		protected LayerEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			vectorLayer.Visible = false;
			rasterLayer.Visible = false;

			int scrollW = Math.Max(vectorLayer.AutoScrollMinSize.Width, rasterLayer.AutoScrollMinSize.Width);
			int scrollH =  Math.Max(vectorLayer.AutoScrollMinSize.Height, rasterLayer.AutoScrollMinSize.Height);
			EditorPanel.AutoScrollMinSize = new Size(scrollW, scrollH);
			EditorPanel.AutoScroll = vectorLayer.AutoScroll || rasterLayer.AutoScroll;

			vectorLayer.Dock = rasterLayer.Dock = DockStyle.Fill;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.ViewerPropertiesTable = new System.Data.DataTable();
            this.PropertyColumnVisible = new System.Data.DataColumn();
            this.PropertyColumnDisplay = new System.Data.DataColumn();
            this.PropertyColumnName = new System.Data.DataColumn();
            this.FeatureSource = new System.Windows.Forms.TextBox();
            this.BrowseFeatureResource = new System.Windows.Forms.Button();
            this.PropertyDataset = new System.Data.DataSet();
            this.DisplayRangesTable = new System.Data.DataTable();
            this.FromScaleColumn = new System.Data.DataColumn();
            this.ToScaleColumn = new System.Data.DataColumn();
            this.StylizationColumn = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.DisplayRanges = new System.Data.DataSet();
            this.EditorPanel = new System.Windows.Forms.Panel();
            this.rasterLayer = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.RasterLayer();
            this.vectorLayer = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.VectorLayer();
            this.label2 = new System.Windows.Forms.Label();
            this.LayerDefinitionVersion = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ViewerPropertiesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRangesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRanges)).BeginInit();
            this.EditorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ViewerPropertiesTable
            // 
            this.ViewerPropertiesTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.PropertyColumnVisible,
            this.PropertyColumnDisplay,
            this.PropertyColumnName});
            this.ViewerPropertiesTable.TableName = "ViewerPropertiesTable";
            // 
            // PropertyColumnVisible
            // 
            this.PropertyColumnVisible.Caption = "Visible";
            this.PropertyColumnVisible.ColumnName = "Visible";
            this.PropertyColumnVisible.DataType = typeof(bool);
            this.PropertyColumnVisible.DefaultValue = false;
            // 
            // PropertyColumnDisplay
            // 
            this.PropertyColumnDisplay.Caption = "Display";
            this.PropertyColumnDisplay.ColumnName = "Display";
            // 
            // PropertyColumnName
            // 
            this.PropertyColumnName.Caption = "Name";
            this.PropertyColumnName.ColumnName = "Name";
            // 
            // FeatureSource
            // 
            resources.ApplyResources(this.FeatureSource, "FeatureSource");
            this.FeatureSource.Name = "FeatureSource";
            this.FeatureSource.ReadOnly = true;
            this.FeatureSource.TextChanged += new System.EventHandler(this.FeatureSource_TextChanged);
            // 
            // BrowseFeatureResource
            // 
            resources.ApplyResources(this.BrowseFeatureResource, "BrowseFeatureResource");
            this.BrowseFeatureResource.Name = "BrowseFeatureResource";
            this.BrowseFeatureResource.Click += new System.EventHandler(this.BrowseFeatureResource_Click);
            // 
            // PropertyDataset
            // 
            this.PropertyDataset.DataSetName = "ViewerProperties";
            this.PropertyDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.PropertyDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.ViewerPropertiesTable});
            // 
            // DisplayRangesTable
            // 
            this.DisplayRangesTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.FromScaleColumn,
            this.ToScaleColumn,
            this.StylizationColumn,
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3});
            this.DisplayRangesTable.TableName = "DisplayRangesTable";
            // 
            // FromScaleColumn
            // 
            this.FromScaleColumn.Caption = "From";
            this.FromScaleColumn.ColumnName = "FromScale";
            // 
            // ToScaleColumn
            // 
            this.ToScaleColumn.Caption = "To";
            this.ToScaleColumn.ColumnName = "ToScale";
            // 
            // StylizationColumn
            // 
            this.StylizationColumn.Caption = "Stylization";
            this.StylizationColumn.ColumnName = "Stylization";
            this.StylizationColumn.DataType = typeof(object);
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "PreviewPoint";
            this.dataColumn1.DataType = typeof(object);
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "PreviewLine";
            this.dataColumn2.DataType = typeof(object);
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "PreviewArea";
            this.dataColumn3.DataType = typeof(object);
            // 
            // DisplayRanges
            // 
            this.DisplayRanges.DataSetName = "DisplayRangesDataSet";
            this.DisplayRanges.Locale = new System.Globalization.CultureInfo("da-DK");
            this.DisplayRanges.Tables.AddRange(new System.Data.DataTable[] {
            this.DisplayRangesTable});
            // 
            // EditorPanel
            // 
            resources.ApplyResources(this.EditorPanel, "EditorPanel");
            this.EditorPanel.Controls.Add(this.rasterLayer);
            this.EditorPanel.Controls.Add(this.vectorLayer);
            this.EditorPanel.Name = "EditorPanel";
            // 
            // rasterLayer
            // 
            resources.ApplyResources(this.rasterLayer, "rasterLayer");
            this.rasterLayer.Name = "rasterLayer";
            // 
            // vectorLayer
            // 
            resources.ApplyResources(this.vectorLayer, "vectorLayer");
            this.vectorLayer.Name = "vectorLayer";
            this.vectorLayer.Resource = null;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // LayerDefinitionVersion
            // 
            this.LayerDefinitionVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LayerDefinitionVersion.FormattingEnabled = true;
            this.LayerDefinitionVersion.Items.AddRange(new object[] {
            resources.GetString("LayerDefinitionVersion.Items"),
            resources.GetString("LayerDefinitionVersion.Items1"),
            resources.GetString("LayerDefinitionVersion.Items2")});
            resources.ApplyResources(this.LayerDefinitionVersion, "LayerDefinitionVersion");
            this.LayerDefinitionVersion.Name = "LayerDefinitionVersion";
            this.LayerDefinitionVersion.SelectedIndexChanged += new System.EventHandler(this.LayerDefinitionVersion_SelectedIndexChanged);
            // 
            // LayerEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.LayerDefinitionVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EditorPanel);
            this.Controls.Add(this.FeatureSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BrowseFeatureResource);
            this.Name = "LayerEditor";
            ((System.ComponentModel.ISupportInitialize)(this.ViewerPropertiesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRangesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRanges)).EndInit();
            this.EditorPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private void FeatureSource_TextChanged(object sender, System.EventArgs e)
		{
			if (inUpdate)
				return;

			RefreshSchema(FeatureSource.Text);

			if (m_layer.Item != null)
			{
				m_layer.Item.ResourceId = FeatureSource.Text;
				m_editor.HasChanged();
				UpdateDisplay();
			}
		}

		public object Resource
		{
			get { return m_layer; }
			set 
			{
				string prevFdef = m_layer == null || m_layer.Item == null ? null : m_layer.Item.ResourceId;
				m_layer = (OSGeo.MapGuide.MaestroAPI.LayerDefinition)value;
				string nFdef = m_layer == null || m_layer.Item == null ? null : m_layer.Item.ResourceId;
				if (prevFdef != nFdef)
					RefreshSchema(nFdef);
				
				vectorLayer.SetItem(m_editor, m_layer, m_schema);
				rasterLayer.SetItem(m_editor, m_layer, m_schema);
				UpdateDisplay();
			}
		}


		public string ResourceId
		{
			get { return m_layer.ResourceId; }
			set { m_layer.ResourceId = value; }
		}

		public bool Preview()
		{
			try
			{
                string templayer = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefiniton, m_editor.CurrentConnection.SessionID);

                string tempmap = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition, m_editor.CurrentConnection.SessionID);


                MaestroAPI.MapDefinition map = new OSGeo.MapGuide.MaestroAPI.MapDefinition();
                map.BackgroundColor = Color.White;

                try
                {
                    Topology.Geometries.IEnvelope env = m_layer.GetSpatialExtent(true);
                    map.Extents = new OSGeo.MapGuide.MaestroAPI.Box2DType();
                    map.Extents.MinX = env.MinX;
                    map.Extents.MinY = env.MinY;
                    map.Extents.MaxX = env.MaxX;
                    map.Extents.MaxY = env.MaxY;
                }
                catch (Exception ex)
                {
                    m_editor.SetLastException(ex);
                }

                MaestroAPI.MapLayerType l = new OSGeo.MapGuide.MaestroAPI.MapLayerType();
                l.Visible = true;
                l.ShowInLegend = true;
                l.ExpandInLegend = true;
                l.Selectable = true;
                if (string.IsNullOrEmpty(m_layer.ResourceId))
                    l.LegendLabel = Strings.LayerEditor.DefaultLegendLabel;
                else
                    l.LegendLabel = new MaestroAPI.ResourceIdentifier(m_layer.ResourceId).Name;
                l.Name = l.LegendLabel;
                l.ResourceId = templayer;
                map.Layers = new OSGeo.MapGuide.MaestroAPI.MapLayerTypeCollection();
                map.Layers.Add(l);

                m_editor.CurrentConnection.SaveResourceAs(m_layer, templayer);

                //Try to infer CS from layer
                var ldf = (LayerDefinition)m_editor.CurrentConnection.GetResource(templayer);
                var feats = (FeatureSource)m_editor.CurrentConnection.GetResource(ldf.Item.ResourceId);
                var scList = feats.GetSpatialInfo();
                if (scList.SpatialContext.Count > 0)
                {
                    string wkt = string.Empty;
                    foreach (FdoSpatialContextListSpatialContext sc in scList.SpatialContext)
                    {
                        if (sc.IsActive)
                        {
                            wkt = sc.CoordinateSystemWkt;
                            continue;
                        }
                    }
                    if (string.IsNullOrEmpty(wkt))
                        map.CoordinateSystem = scList.SpatialContext[0].CoordinateSystemWkt;

                    if (!string.IsNullOrEmpty(wkt))
                        map.CoordinateSystem = wkt;
                }

                m_editor.CurrentConnection.SaveResourceAs(map, tempmap);

                if (m_editor.UseFusionPreview)
                {
                    string templayout = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, m_editor.CurrentConnection.SessionID);
                    
                    MaestroAPI.ApplicationDefinition.ApplicationDefinitionType layout;
                    if (System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, "Preview layout.ApplicationDefinition")))
                    {
                        using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(Application.StartupPath, "Preview layout.ApplicationDefinition"), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                            layout = (MaestroAPI.ApplicationDefinition.ApplicationDefinitionType)m_editor.CurrentConnection.DeserializeObject(typeof(OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType), fs);

                    }
                    else
                        layout = (MaestroAPI.ApplicationDefinition.ApplicationDefinitionType)m_editor.CurrentConnection.DeserializeObject(typeof(OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType), System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "Preview layout.ApplicationDefinition"));

                    if (layout.MapSet == null)
                        layout.MapSet = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapGroupTypeCollection();
                    if (layout.MapSet.Count == 0)
                        layout.MapSet.Add(new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapGroupType());

                    if (layout.MapSet[0].Map == null)
                        layout.MapSet[0].Map = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapTypeCollection();

                    if (layout.MapSet[0].Map.Count == 0)
                        layout.MapSet[0].Map.Add(new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType());

                    if (string.IsNullOrEmpty(layout.MapSet[0].Map[0].SingleTile))
                        layout.MapSet[0].Map[0].SingleTile = "true";
                    layout.MapSet[0].Map[0].Type = "MapGuide";
                    
                    if (layout.MapSet[0].Map[0].Extension == null)
                        layout.MapSet[0].Map[0].Extension = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.CustomContentType();

                    if (layout.MapSet[0].Map[0].Extension.Any == null || layout.MapSet[0].Map[0].Extension.Any.Length == 0)
                        layout.MapSet[0].Map[0].Extension.Any = new System.Xml.XmlElement[1];
                    layout.MapSet[0].Map[0].Extension.Any[0] = layout.ApplicationDocument.CreateElement("ResourceId");
                    layout.MapSet[0].Map[0].Extension.Any[0].InnerText = tempmap;


                    string url = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).BaseURL;
                    if (string.IsNullOrEmpty(layout.TemplateUrl))
                        layout.TemplateUrl = "fusion/templates/mapguide/aqua/index.html";

                    m_editor.CurrentConnection.SaveResourceAs(layout, templayout);

                    url += layout.TemplateUrl;

                    if (!url.EndsWith("?"))
                        url += "?";

                    url += "ApplicationDefinition=" + System.Web.HttpUtility.UrlEncode(templayout) + "&SESSION=" + System.Web.HttpUtility.UrlEncode(m_editor.CurrentConnection.SessionID);

                    m_editor.OpenUrl(url);

                }
                else
                {
                    string templayout = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, m_editor.CurrentConnection.SessionID);

                    MaestroAPI.WebLayout layout;
                    if (System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, "Preview layout.WebLayout")))
                    {
                        using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(Application.StartupPath, "Preview layout.WebLayout"), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                            layout = (MaestroAPI.WebLayout)m_editor.CurrentConnection.DeserializeObject(typeof(OSGeo.MapGuide.MaestroAPI.WebLayout), fs);

                    }
                    else
                        layout = (MaestroAPI.WebLayout)m_editor.CurrentConnection.DeserializeObject(typeof(OSGeo.MapGuide.MaestroAPI.WebLayout), System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "Preview layout.WebLayout"));

                    layout.Map.ResourceId = tempmap;
                    m_editor.CurrentConnection.SaveResourceAs(layout, templayout);

                    string url = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).BaseURL;

                    url += "mapviewerajax/?WEBLAYOUT=" + System.Web.HttpUtility.UrlEncode(templayout) + "&SESSION=" + System.Web.HttpUtility.UrlEncode(m_editor.CurrentConnection.SessionID);

                    m_editor.OpenUrl(url);
                }

			}
			catch(Exception ex)
			{
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(Strings.LayerEditor.MapPreviewError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

            return true;
		}


		public bool Save(string savename)
		{
			return false;
		}

		private void BrowseFeatureResource_Click(object sender, System.EventArgs e)
		{
			string item = m_editor.BrowseResource("FeatureSource");
			if (item != null)
				if (FeatureSource.Text != item)
					FeatureSource.Text = item;
				else
					FeatureSource_TextChanged(sender, e);
		}
    
        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return m_editor.CurrentConnection.SupportsResourcePreviews; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return true; } }

        private void LayerDefinitionVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_layer == null || inUpdate || LayerDefinitionVersion.SelectedIndex < 0)
                return;

            m_layer.ConvertLayerDefinitionToVersion(new Version(LayerDefinitionVersion.Text));
            m_editor.HasChanged();
        }
    }
}
