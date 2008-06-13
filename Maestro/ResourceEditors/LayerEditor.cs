#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for LayerEditor.
	/// </summary>
	public class LayerEditor : System.Windows.Forms.UserControl, ResourceEditor
	{

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FeatureSource;

		private OSGeo.MapGuide.MaestroAPI.LayerDefinition m_layer;
		private bool inUpdate = false;
		private Globalizator.Globalizator m_globalizor = null;
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
        private ResourceEditors.LayerEditorControls.VectorLayer vectorLayer;

		
		public LayerEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_layer = new OSGeo.MapGuide.MaestroAPI.LayerDefinition();

			vectorLayer.SetItem(m_editor, m_layer, null, m_globalizor);
			rasterLayer.SetItem(m_editor, m_layer, null, m_globalizor);

			UpdateDisplay();
		}

		private void RefreshSchema(string resourceID)
		{
			try
			{
				m_schema = null;
				if (resourceID == null || resourceID == "" || resourceID.Equals(m_globalizor.Translate("< Select a featuresource >")))
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
					rasterLayer.SetItem(m_editor, m_layer, m_schema, m_globalizor);
				else
					vectorLayer.SetItem(m_editor, m_layer, m_schema, m_globalizor);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, m_globalizor.Translate(string.Format("Failed to read schema from data source.\nThe operation gave the error message: {0}", ex.Message)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
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
				vectorLayer.SetItem(m_editor, m_layer, m_schema, m_globalizor);
				rasterLayer.SetItem(m_editor, m_layer, m_schema, m_globalizor);
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
					FeatureSource.Text = m_globalizor.Translate("< Select a featuresource >");

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
			m_globalizor = new Globalizator.Globalizator(this);
	
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
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Feature resource";
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
            this.FeatureSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FeatureSource.Location = new System.Drawing.Point(112, 0);
            this.FeatureSource.Name = "FeatureSource";
            this.FeatureSource.ReadOnly = true;
            this.FeatureSource.Size = new System.Drawing.Size(352, 20);
            this.FeatureSource.TabIndex = 2;
            this.FeatureSource.Text = "< Select a featuresource >";
            this.FeatureSource.TextChanged += new System.EventHandler(this.FeatureSource_TextChanged);
            // 
            // BrowseFeatureResource
            // 
            this.BrowseFeatureResource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseFeatureResource.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseFeatureResource.Location = new System.Drawing.Point(464, 0);
            this.BrowseFeatureResource.Name = "BrowseFeatureResource";
            this.BrowseFeatureResource.Size = new System.Drawing.Size(24, 20);
            this.BrowseFeatureResource.TabIndex = 14;
            this.BrowseFeatureResource.Text = "...";
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
            this.EditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.EditorPanel.Controls.Add(this.rasterLayer);
            this.EditorPanel.Controls.Add(this.vectorLayer);
            this.EditorPanel.Location = new System.Drawing.Point(0, 32);
            this.EditorPanel.Name = "EditorPanel";
            this.EditorPanel.Size = new System.Drawing.Size(504, 776);
            this.EditorPanel.TabIndex = 15;
            // 
            // rasterLayer
            // 
            this.rasterLayer.Location = new System.Drawing.Point(240, 24);
            this.rasterLayer.Name = "rasterLayer";
            this.rasterLayer.Size = new System.Drawing.Size(176, 232);
            this.rasterLayer.TabIndex = 1;
            // 
            // vectorLayer
            // 
            this.vectorLayer.Location = new System.Drawing.Point(24, 32);
            this.vectorLayer.Name = "vectorLayer";
            this.vectorLayer.Resource = null;
            this.vectorLayer.Size = new System.Drawing.Size(184, 216);
            this.vectorLayer.TabIndex = 0;
            // 
            // LayerEditor
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(496, 800);
            this.Controls.Add(this.EditorPanel);
            this.Controls.Add(this.FeatureSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BrowseFeatureResource);
            this.Name = "LayerEditor";
            this.Size = new System.Drawing.Size(504, 808);
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
				
				vectorLayer.SetItem(m_editor, m_layer, m_schema, m_globalizor);
				rasterLayer.SetItem(m_editor, m_layer, m_schema, m_globalizor);
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
                string templayer = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefiniton, true);

                string tempmap = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition, true);


                MaestroAPI.MapDefinition map = new OSGeo.MapGuide.MaestroAPI.MapDefinition();
                map.BackgroundColor = Color.White;

                OSGeo.MapGuide.MaestroAPI.FdoSpatialContextList lst = m_editor.CurrentConnection.GetSpatialContextInfo(m_layer.Item.ResourceId, false);
                
                if (lst.SpatialContext != null && lst.SpatialContext.Count >= 1)
                {
                    map.Extents = new OSGeo.MapGuide.MaestroAPI.Box2DType();
                    map.Extents.MinX = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.X,  System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    map.Extents.MinY = double.Parse(lst.SpatialContext[0].Extent.LowerLeftCoordinate.Y,  System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);;
                    map.Extents.MaxX = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.X,  System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);;
                    map.Extents.MaxY = double.Parse(lst.SpatialContext[0].Extent.UpperRightCoordinate.Y, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture); ;
                }

                MaestroAPI.MapLayerType l = new OSGeo.MapGuide.MaestroAPI.MapLayerType();
                l.Visible = true;
                l.ShowInLegend = true;
                l.ExpandInLegend = true;
                l.Selectable = true;
                if (string.IsNullOrEmpty(m_layer.ResourceId))
                    l.LegendLabel = "Layer";
                else
                    l.LegendLabel = m_editor.CurrentConnection.GetResourceName(m_layer.ResourceId, false);
                l.Name = l.LegendLabel;
                l.ResourceId = templayer;
                map.Layers = new OSGeo.MapGuide.MaestroAPI.MapLayerTypeCollection();
                map.Layers.Add(l);

                m_editor.CurrentConnection.SaveResourceAs(m_layer, templayer);
                m_editor.CurrentConnection.SaveResourceAs(map, tempmap);

                if (m_editor.UseFusionPreview)
                {
                    string templayout = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, true);
                    
                    MaestroAPI.ApplicationDefinition.ApplicationDefinitionType layout;
                    if (System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, "Preview layout.ApplicationDefinition")))
                    {
                        using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(Application.StartupPath, "Preview layout.ApplicationDefinition"), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                            layout = (MaestroAPI.ApplicationDefinition.ApplicationDefinitionType)m_editor.CurrentConnection.DeserializeObject(typeof(OSGeo.MapGuide.MaestroAPI.WebLayout), fs);

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
                    string templayout = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, true);

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
				MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed while creating map preview: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
	}
}
