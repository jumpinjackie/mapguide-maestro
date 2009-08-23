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
		private Globalizator.Globalizator m_globalizor;

		public FeatureSourceEditorGdal()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			m_globalizor = new Globalizator.Globalizator(this);
			simple.Visible = composite.Visible = false;
			simple.Dock = composite.Dock = DockStyle.Fill;
		}

		public FeatureSourceEditorGdal(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;
			simple.SetItem(m_editor, m_feature, m_globalizor);
			composite.SetItem(m_editor, m_feature, m_globalizor);

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
			this.toolTips = new System.Windows.Forms.ToolTip(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.TypeCombo = new System.Windows.Forms.ComboBox();
			this.contentPanel = new System.Windows.Forms.Panel();
			this.simple = new ResourceEditors.FeatureSourceEditors.Gdal.Simple();
			this.composite = new ResourceEditors.FeatureSourceEditors.Gdal.Composite();
			this.contentPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Type";
			// 
			// TypeCombo
			// 
			this.TypeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TypeCombo.Items.AddRange(new object[] {
														   "Single file or folder",
														   "Composite"});
			this.TypeCombo.Location = new System.Drawing.Point(104, 8);
			this.TypeCombo.Name = "TypeCombo";
			this.TypeCombo.Size = new System.Drawing.Size(272, 21);
			this.TypeCombo.TabIndex = 1;
			this.TypeCombo.SelectedIndexChanged += new System.EventHandler(this.TypeCombo_SelectedIndexChanged);
			// 
			// contentPanel
			// 
			this.contentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.contentPanel.Controls.Add(this.composite);
			this.contentPanel.Controls.Add(this.simple);
			this.contentPanel.Location = new System.Drawing.Point(8, 40);
			this.contentPanel.Name = "contentPanel";
			this.contentPanel.Size = new System.Drawing.Size(368, 296);
			this.contentPanel.TabIndex = 2;
			// 
			// simple
			// 
			this.simple.AutoScroll = true;
			this.simple.AutoScrollMinSize = new System.Drawing.Size(216, 40);
			this.simple.Location = new System.Drawing.Point(16, 16);
			this.simple.Name = "simple";
			this.simple.Size = new System.Drawing.Size(216, 40);
			this.simple.TabIndex = 0;
			// 
			// composite
			// 
			this.composite.Location = new System.Drawing.Point(8, 80);
			this.composite.Name = "composite";
			this.composite.Size = new System.Drawing.Size(344, 120);
			this.composite.TabIndex = 1;
			// 
			// FeatureSourceEditorGdal
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(384, 32);
			this.Controls.Add(this.contentPanel);
			this.Controls.Add(this.TypeCombo);
			this.Controls.Add(this.label1);
			this.Name = "FeatureSourceEditorGdal";
			this.Size = new System.Drawing.Size(384, 344);
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
				simple.SetItem(m_editor, m_feature, m_globalizor);
				composite.SetItem(m_editor, m_feature, m_globalizor);
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
