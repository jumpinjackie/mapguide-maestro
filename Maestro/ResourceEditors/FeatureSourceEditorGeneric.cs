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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for DataSourceEditor.
	/// </summary>
	public class FeatureSourceEditorGeneric : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.DataGrid properties;
		private System.Data.DataSet PropertyDataSet;
		private System.Data.DataTable PropertyTable;
		private System.Data.DataColumn PropertyColumn;
		private System.Data.DataColumn ValueColumn;
		private OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProvider m_provider;
		private System.Windows.Forms.DataGridTableStyle PropertiesTableStyle;
		private System.Windows.Forms.DataGridTextBoxColumn PropertyStyleColumn;
		private System.Windows.Forms.DataGridTextBoxColumn ValueStyleColumn;
		private System.Data.DataColumn RealNameColumn;
		private Hashtable propertyMap = null;
		private ResourceEditors.ResourceDataEditor resourceDataEditor;
		private bool inUpdate = false;

		public FeatureSourceEditorGeneric(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;
			m_provider = m_editor.CurrentConnection.GetFeatureProvider(m_feature.Provider);

			UpdateDisplay();
		}


		public void UpdateDisplay()
		{
			if (inUpdate)
				return;

			try
			{
				inUpdate = true;

				PropertyTable.Rows.Clear();
				propertyMap = new Hashtable();

				if (m_provider == null)
				{
					if (m_feature.Parameter == null || m_feature.Parameter.Count == 0)
						throw new Exception(Strings.FeatureSourceEditorGeneric.MissingProviderError);


					foreach(OSGeo.MapGuide.MaestroAPI.NameValuePairType props in m_feature.Parameter)
					{

						System.Data.DataRow row = PropertyTable.NewRow();
						row["Property"] = props.Name;
						row["Value"] = props.Value;
						row["RealName"] = props.Name;
						PropertyTable.Rows.Add(row);
						propertyMap.Add(props.Name, row);
					}

					
				}
				else
				{
					foreach(OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProviderConnectionProperty property in m_provider.ConnectionProperties)
					{
						System.Data.DataRow row = PropertyTable.NewRow();
						row["Property"] = property.LocalizedName;
						row["Value"] = property.DefaultValue;
						row["RealName"] = property.Name;
						PropertyTable.Rows.Add(row);
						propertyMap.Add(property.Name, row);
					}

					if (m_feature.Parameter == null || m_feature.Parameter.Count == 0)
					{
						m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();
						foreach(OSGeo.MapGuide.MaestroAPI.FeatureProviderRegistryFeatureProviderConnectionProperty property in m_provider.ConnectionProperties)
						{
							OSGeo.MapGuide.MaestroAPI.NameValuePairType v = new OSGeo.MapGuide.MaestroAPI.NameValuePairType();
							v.Name = property.Name;
							v.Value = property.DefaultValue;
							m_feature.Parameter.Add(v);
						}

					}
					else
						foreach(OSGeo.MapGuide.MaestroAPI.NameValuePairType props in m_feature.Parameter)
							if (propertyMap[props.Name] != null)
								((System.Data.DataRow)propertyMap[props.Name])["Value"] = props.Value;
				}

				if (!m_editor.Existing)
					m_feature.Provider = m_editor.CurrentConnection.RemoveVersionFromProviderName(m_provider.Name);

				resourceDataEditor.ResourceExists = true; //m_editor.Existing;
				resourceDataEditor.Editor = m_editor;
				resourceDataEditor.ResourceID = m_feature.ResourceId;
			} 
			finally 
			{
				inUpdate = false;
			}
		}

		private FeatureSourceEditorGeneric()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			PropertyTable.RowChanged += new DataRowChangeEventHandler(PropertyTable_RowChanged);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceEditorGeneric));
            this.properties = new System.Windows.Forms.DataGrid();
            this.PropertyTable = new System.Data.DataTable();
            this.PropertyColumn = new System.Data.DataColumn();
            this.ValueColumn = new System.Data.DataColumn();
            this.RealNameColumn = new System.Data.DataColumn();
            this.PropertiesTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.PropertyStyleColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            this.ValueStyleColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            this.PropertyDataSet = new System.Data.DataSet();
            this.resourceDataEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.ResourceDataEditor();
            ((System.ComponentModel.ISupportInitialize)(this.properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // properties
            // 
            resources.ApplyResources(this.properties, "properties");
            this.properties.CaptionVisible = false;
            this.properties.DataMember = "";
            this.properties.DataSource = this.PropertyTable;
            this.properties.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.properties.Name = "properties";
            this.properties.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.PropertiesTableStyle});
            // 
            // PropertyTable
            // 
            this.PropertyTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.PropertyColumn,
            this.ValueColumn,
            this.RealNameColumn});
            this.PropertyTable.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "Property"}, true)});
            this.PropertyTable.PrimaryKey = new System.Data.DataColumn[] {
        this.PropertyColumn};
            this.PropertyTable.TableName = "Properties";
            // 
            // PropertyColumn
            // 
            this.PropertyColumn.AllowDBNull = false;
            this.PropertyColumn.Caption = "Property";
            this.PropertyColumn.ColumnName = "Property";
            this.PropertyColumn.DefaultValue = "";
            // 
            // ValueColumn
            // 
            this.ValueColumn.Caption = "Value";
            this.ValueColumn.ColumnName = "Value";
            this.ValueColumn.DefaultValue = "";
            // 
            // RealNameColumn
            // 
            this.RealNameColumn.Caption = "RealName";
            this.RealNameColumn.ColumnName = "RealName";
            // 
            // PropertiesTableStyle
            // 
            this.PropertiesTableStyle.AllowSorting = false;
            this.PropertiesTableStyle.DataGrid = this.properties;
            this.PropertiesTableStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.PropertyStyleColumn,
            this.ValueStyleColumn});
            this.PropertiesTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.PropertiesTableStyle.MappingName = "Properties";
            this.PropertiesTableStyle.RowHeadersVisible = false;
            // 
            // PropertyStyleColumn
            // 
            this.PropertyStyleColumn.Format = "";
            this.PropertyStyleColumn.FormatInfo = null;
            resources.ApplyResources(this.PropertyStyleColumn, "PropertyStyleColumn");
            this.PropertyStyleColumn.ReadOnly = true;
            // 
            // ValueStyleColumn
            // 
            this.ValueStyleColumn.Format = "";
            this.ValueStyleColumn.FormatInfo = null;
            resources.ApplyResources(this.ValueStyleColumn, "ValueStyleColumn");
            // 
            // PropertyDataSet
            // 
            this.PropertyDataSet.DataSetName = "PropertyDataSet";
            this.PropertyDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.PropertyDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.PropertyTable});
            // 
            // resourceDataEditor
            // 
            resources.ApplyResources(this.resourceDataEditor, "resourceDataEditor");
            this.resourceDataEditor.Editor = null;
            this.resourceDataEditor.Name = "resourceDataEditor";
            this.resourceDataEditor.ResourceExists = false;
            this.resourceDataEditor.ResourceID = null;
            this.resourceDataEditor.UseTemporaryResource = false;
            this.resourceDataEditor.ResourceDataChanged += new System.EventHandler(this.resourceDataEditor_ResourceDataChanged);
            // 
            // FeatureSourceEditorGeneric
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.resourceDataEditor);
            this.Controls.Add(this.properties);
            this.Name = "FeatureSourceEditorGeneric";
            this.Load += new System.EventHandler(this.FeatureSourceEditorGeneric_Load);
            ((System.ComponentModel.ISupportInitialize)(this.properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataSet)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void FeatureSourceEditorGeneric_Load(object sender, System.EventArgs e)
		{
		
		}


		public object Resource
		{
			get { return m_feature; }
			set 
			{ 
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
				UpdateDisplay();
			}
		}

		public string ResourceId
		{
			get { return m_feature.ResourceId; }
			set { m_feature.ResourceId = value; }
		}

		private void PropertyTable_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (inUpdate)
				return;

			Hashtable prop = new Hashtable();
			foreach(OSGeo.MapGuide.MaestroAPI.NameValuePairType props in m_feature.Parameter)
				if (propertyMap[props.Name] != null)
					props.Value = (string)((System.Data.DataRow)propertyMap[props.Name])["Value"];

			m_editor.HasChanged();
		}

		public bool Preview()
		{
			return false;
		}

		public bool Save(string savename)
		{
			try { resourceDataEditor.Focus(); }
			catch { }
			return false;
		}

		private void TestConnectionResult_TextChanged(object sender, System.EventArgs e)
		{
		
		}

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return m_editor.CurrentConnection.SupportsResourcePreviews; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }

        private void resourceDataEditor_ResourceDataChanged(object sender, EventArgs e)
        {
            m_editor.HasChanged();
        }
    }
}
