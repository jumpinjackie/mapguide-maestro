#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls
{
	/// <summary>
	/// Summary description for SchemaSelector.
	/// </summary>
	public class SchemaSelector : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ComboBox Geometry;
		private System.Windows.Forms.ComboBox Schema;
		private System.Windows.Forms.Label label2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public delegate void SchemaChangedDelegate(bool fromUser, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema);
		public delegate void GeometryChangedDelegate(bool fromUser, string geom);

		public event SchemaChangedDelegate SchemaChanged;
		public event GeometryChangedDelegate GeometryChanged;

		private bool inUpdate = false;
		private OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription m_schemas;
		private OSGeo.MapGuide.MaestroAPI.LayerDefinition m_layer;
		private EditorInterface m_editor;
		private Globalizator.Globalizator m_globalizor;
		private System.Windows.Forms.Label GeometryLabel;
		private bool m_isRaster = false;

		public SchemaSelector()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void UpdateDisplay()
		{
			if (inUpdate)
				return;
			try
			{
				inUpdate = true;

				if (m_layer == null)
					return;

				if (m_schemas != null)
				{
					Schema.Items.Clear();

					foreach(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm in m_schemas.Schemas)
						Schema.Items.Add(OSGeo.MapGuide.MaestroAPI.Utility.DecodeFDOName(scm.Fullname));

					string schemaName = "";
					if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
						schemaName =  (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType).FeatureName;
					else if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType != null)
						schemaName = (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType).FeatureName;

                    if (schemaName == null)
                        schemaName = "";

                    schemaName = OSGeo.MapGuide.MaestroAPI.Utility.DecodeFDOName(schemaName);
					Schema.SelectedIndex = Schema.FindString(schemaName);

					if (Schema.Items.Count > 0 && (Schema.Text != schemaName || Schema.SelectedIndex < 0))
					{
						Schema.SelectedIndex = 0;
						schemaName = (string)Schema.Items[0];
						if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
							(m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType).FeatureName = schemaName;
						else if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType != null)
							(m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType).FeatureName = schemaName;

						if (SchemaChanged != null && Schema.SelectedIndex >= 0)
							SchemaChanged(false, m_schemas[Schema.SelectedIndex]);
					}


					UpdateGeometry(false);
				}
				
			} 
			finally
			{
				inUpdate = false;
			}
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
			this.Geometry = new System.Windows.Forms.ComboBox();
			this.Schema = new System.Windows.Forms.ComboBox();
			this.GeometryLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Geometry
			// 
			this.Geometry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Geometry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Geometry.Location = new System.Drawing.Point(104, 32);
			this.Geometry.Name = "Geometry";
			this.Geometry.Size = new System.Drawing.Size(232, 21);
			this.Geometry.TabIndex = 11;
			this.Geometry.SelectedIndexChanged += new System.EventHandler(this.Geometry_SelectedIndexChanged);
			// 
			// Schema
			// 
			this.Schema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Schema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Schema.Location = new System.Drawing.Point(104, 0);
			this.Schema.Name = "Schema";
			this.Schema.Size = new System.Drawing.Size(232, 21);
			this.Schema.TabIndex = 10;
			this.Schema.SelectedIndexChanged += new System.EventHandler(this.Schema_SelectedIndexChanged);
			// 
			// GeometryLabel
			// 
			this.GeometryLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.GeometryLabel.Location = new System.Drawing.Point(0, 32);
			this.GeometryLabel.Name = "GeometryLabel";
			this.GeometryLabel.Size = new System.Drawing.Size(96, 16);
			this.GeometryLabel.TabIndex = 9;
			this.GeometryLabel.Text = "Geometry";
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Schema";
			// 
			// SchemaSelector
			// 
			this.Controls.Add(this.Geometry);
			this.Controls.Add(this.Schema);
			this.Controls.Add(this.GeometryLabel);
			this.Controls.Add(this.label2);
			this.Name = "SchemaSelector";
			this.Size = new System.Drawing.Size(336, 56);
			this.ResumeLayout(false);

		}
		#endregion

		private void Schema_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (inUpdate || m_layer == null || m_schemas == null)
				return;

			if (Schema.SelectedIndex >= 0)
			{
				if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
					(m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType).FeatureName = Schema.Text;
				else if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType != null)
					(m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType).FeatureName = Schema.Text;

				if (SchemaChanged != null)
					SchemaChanged(true, m_schemas[Schema.SelectedIndex]);

				UpdateGeometry(true);
				m_editor.HasChanged();
			}
			else
			{
				Geometry.Enabled = false;
			}


		}

		public bool IsRaster 
		{
			get { return m_isRaster; }
			set 
			{ 
				m_isRaster = value;
				GeometryLabel.Text = m_isRaster ? "Raster" : "Geometry";
			}
		}

		private void UpdateGeometry(bool fromUser)
		{
			Geometry.Items.Clear();

			if (Schema.SelectedIndex >= 0)
			{
				string geom = "";
				
				if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
					geom = (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType).Geometry;
				else if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType != null)
					geom = (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType).Geometry;

				Geometry.Enabled = true;

				foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn col in m_schemas[Schema.SelectedIndex].Columns)
				{
					if (!m_isRaster && col.Type == OSGeo.MapGuide.MaestroAPI.Utility.GeometryType)
						Geometry.Items.Add(col.Name);
                    else if (m_isRaster && col.Type == OSGeo.MapGuide.MaestroAPI.Utility.RasterType)
						Geometry.Items.Add(col.Name);
				}

				Geometry.SelectedIndex = Geometry.FindString(geom);
                if (Geometry.Items.Count > 0 && (Geometry.SelectedIndex < 0 || Geometry.Text != geom))
                {
                    Geometry.SelectedIndex = 0;
                    if (GeometryChanged != null)
                        GeometryChanged(true, Geometry.Text); 
                        //We select the default item, so notify the user that it was changed
                }

				if (geom != Geometry.Text)
				{
					if (GeometryChanged != null)
						GeometryChanged(false, Geometry.Text);

					if (fromUser)
						m_editor.HasChanged();
				}

			}
			else
			{
				Geometry.Enabled = false;
			}
		}

		private void Geometry_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (inUpdate || m_layer == null || m_schemas == null)
				return;

			if (Geometry.SelectedIndex >= 0)
			{
				if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
					(m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType).Geometry = Geometry.Text;
				else if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType != null)
					(m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType).Geometry = Geometry.Text;

				m_editor.HasChanged();

				if (GeometryChanged != null)
					GeometryChanged(true, Geometry.Text);
			}
		}

		public OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema CurrentSchema
		{
			get 
			{
				if (m_schemas == null || m_schemas.Schemas == null)
					return null;

				foreach(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm in m_schemas.Schemas)
					if (scm.FullnameDecoded == Schema.Text)
						return scm;

				return null;
			}
		}

		public void SetItem(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.LayerDefinition layer, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription schema, Globalizator.Globalizator globalizor)
		{
			m_editor = editor;
			m_layer = layer;
			m_schemas = schema;
			m_globalizor = globalizor;

			UpdateDisplay();
		}
	}
}
