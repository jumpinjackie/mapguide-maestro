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
	/// Summary description for FeatureSourceEditorMSSQLSpatial.
	/// </summary>
	public class FeatureSourceEditorGdal : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		private System.ComponentModel.IContainer components;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.ToolTip toolTips;
		private bool m_isUpdating = false;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox TypeCombo;
		private System.Windows.Forms.Panel contentPanel;
		private ResourceEditors.FeatureSourceEditors.Gdal.Simple simple;
		private ResourceEditors.FeatureSourceEditors.Gdal.Composite composite;

		public FeatureSourceEditorGdal()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			simple.Visible = composite.Visible = false;
			simple.Dock = composite.Dock = DockStyle.Fill;
		}

		public FeatureSourceEditorGdal(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;
			simple.SetItem(m_editor, m_feature);
			composite.SetItem(m_editor, m_feature);

			UpdateDisplay();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceEditorGdal));
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.TypeCombo = new System.Windows.Forms.ComboBox();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.composite = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Gdal.Composite();
            this.simple = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Gdal.Simple();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // TypeCombo
            // 
            resources.ApplyResources(this.TypeCombo, "TypeCombo");
            this.TypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeCombo.Items.AddRange(new object[] {
            resources.GetString("TypeCombo.Items"),
            resources.GetString("TypeCombo.Items1")});
            this.TypeCombo.Name = "TypeCombo";
            this.TypeCombo.SelectedIndexChanged += new System.EventHandler(this.TypeCombo_SelectedIndexChanged);
            // 
            // contentPanel
            // 
            resources.ApplyResources(this.contentPanel, "contentPanel");
            this.contentPanel.Controls.Add(this.composite);
            this.contentPanel.Controls.Add(this.simple);
            this.contentPanel.Name = "contentPanel";
            // 
            // composite
            // 
            resources.ApplyResources(this.composite, "composite");
            this.composite.Name = "composite";
            // 
            // simple
            // 
            resources.ApplyResources(this.simple, "simple");
            this.simple.Name = "simple";
            // 
            // FeatureSourceEditorGdal
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.TypeCombo);
            this.Controls.Add(this.label1);
            this.Name = "FeatureSourceEditorGdal";
            this.contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public object Resource
		{
			get { return m_feature; }
			set 
			{
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
				simple.SetItem(m_editor, m_feature);
				composite.SetItem(m_editor, m_feature);
				UpdateDisplay();
			}
		}

		public string ResourceId
		{
			get { return m_feature.ResourceId; }
			set { m_feature.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_feature == null)
				{
					simple.Visible = composite.Visible = false;
					return;
				}

				if (m_feature.ConfigurationDocument == null || m_feature.ConfigurationDocument.Trim().Length == 0)
					TypeCombo.SelectedIndex = 0;
				else
					TypeCombo.SelectedIndex = 1;

				TypeCombo_SelectedIndexChanged(null, null);
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public bool Save(string savename)
		{
            if (TypeCombo.SelectedIndex == 0)
            {
                //Clear the config document
                if (m_feature.Parameter == null)
                    m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

                m_feature.ConfigurationDocument = null;
                m_feature.Parameter["DefaultRasterFileLocation"] = simple.Filepath.Text;
            }

			return false;
		}

		private void TypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null)
				return;

			if (TypeCombo.SelectedIndex == 0)
			{
				bool wasVisible = simple.Visible;
				simple.Visible = true;
				composite.Visible = false;
                if (!wasVisible)
                {
                    simple.UpdateDisplay();
                    if (!m_isUpdating)
                        m_editor.HasChanged();
                }
			}
			else
			{
				bool wasVisible = composite.Visible;
				simple.Visible = false;
				composite.Visible = true;
                if (!wasVisible)
                {
                    composite.UpdateDisplay();
                    if (!m_isUpdating)
                        m_editor.HasChanged();
                }
			}
		}

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }

	}
}
