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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for FeaturesourceEditorSQLite.
	/// </summary>
	public class FeatureSourceEditorSQLite : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private bool m_isUpdating = false;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox SQLiteType;
		private System.Windows.Forms.Panel panel1;
		private ResourceEditors.FeatureSourceEditors.SQLite.Unmanaged UnmanagedEditor;
        private ResourceEditors.FeatureSourceEditors.SQLite.Managed ManagedEditor;
		private Globalizator.Globalizator m_globalizor;

		public FeatureSourceEditorSQLite(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;
			ManagedEditor.SetItem(editor, feature);
			UnmanagedEditor.SetItem(editor, feature);

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;

				if (m_feature.Parameter == null)
					m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();
				
				if (m_feature.Parameter["File"] == null || m_feature.Parameter["File"].Trim().Length == 0 || m_feature.Parameter["File"].StartsWith("%MG_DATA_FILE_PATH%"))
					SQLiteType.SelectedIndex = 0;
				else
					SQLiteType.SelectedIndex = 1;

				ManagedEditor.UpdateDisplay();
				UnmanagedEditor.UpdateDisplay();
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private FeatureSourceEditorSQLite()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			ManagedEditor.Dock = DockStyle.Fill;
			UnmanagedEditor.Dock = DockStyle.Fill;
			m_globalizor = new Globalizator.Globalizator(this);
			string tmp = m_globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditorSQLite.SQLiteType.Items");
			if (tmp != null && tmp.Trim().Length > 0)
			{
				ArrayList fix = new ArrayList();
				foreach(string s in tmp.Trim().Split('\n'))
					if (s.Trim().Length > 0)
						fix.Add(s.Trim());

				if (fix.Count == SQLiteType.Items.Count)
				{
					SQLiteType.Items.Clear();
					SQLiteType.Items.AddRange(fix.ToArray());
				}
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
            this.label1 = new System.Windows.Forms.Label();
            this.SQLiteType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.UnmanagedEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.SQLite.Unmanaged();
            this.ManagedEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.SQLite.Managed();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "File location type";
            // 
            // SQLiteType
            // 
            this.SQLiteType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SQLiteType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SQLiteType.Items.AddRange(new object[] {
            "Inside the MapGuide server (Managed)",
            "On the server file system (Unmanaged)"});
            this.SQLiteType.Location = new System.Drawing.Point(120, 0);
            this.SQLiteType.Name = "SQLiteType";
            this.SQLiteType.Size = new System.Drawing.Size(536, 21);
            this.SQLiteType.TabIndex = 3;
            this.SQLiteType.SelectedIndexChanged += new System.EventHandler(this.SQLiteType_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.UnmanagedEditor);
            this.panel1.Controls.Add(this.ManagedEditor);
            this.panel1.Location = new System.Drawing.Point(8, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 200);
            this.panel1.TabIndex = 4;
            // 
            // UnmanagedEditor
            // 
            this.UnmanagedEditor.AutoScroll = true;
            this.UnmanagedEditor.AutoScrollMinSize = new System.Drawing.Size(248, 24);
            this.UnmanagedEditor.Location = new System.Drawing.Point(336, 8);
            this.UnmanagedEditor.Name = "UnmanagedEditor";
            this.UnmanagedEditor.Size = new System.Drawing.Size(288, 120);
            this.UnmanagedEditor.TabIndex = 1;
            this.UnmanagedEditor.Visible = false;
            // 
            // ManagedEditor
            // 
            this.ManagedEditor.AutoScroll = true;
            this.ManagedEditor.AutoScrollMinSize = new System.Drawing.Size(216, 88);
            this.ManagedEditor.Location = new System.Drawing.Point(16, 8);
            this.ManagedEditor.Name = "ManagedEditor";
            this.ManagedEditor.Size = new System.Drawing.Size(224, 136);
            this.ManagedEditor.TabIndex = 0;
            this.ManagedEditor.Visible = false;
            // 
            // FeatureSourceEditorSQLite
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(280, 160);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SQLiteType);
            this.Controls.Add(this.label1);
            this.Name = "FeatureSourceEditorSQLite";
            this.Size = new System.Drawing.Size(664, 240);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public object Resource
		{
			get { return m_feature; }
			set 
			{
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
				ManagedEditor.SetItem(m_editor, m_feature);
				UnmanagedEditor.SetItem(m_editor, m_feature);
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

		private void SQLiteType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (SQLiteType.SelectedIndex <= 0)
			{
				ManagedEditor.Visible = true;
				UnmanagedEditor.Visible = false;
				ManagedEditor.SetItem(m_editor, m_feature);
			}
			else
			{
				ManagedEditor.Visible = false;
				UnmanagedEditor.Visible = true;
				UnmanagedEditor.SetItem(m_editor, m_feature);
			}

			if (m_isUpdating || m_feature == null)
				return;
		}

		public bool Save(string savename)
		{
			return false;
		}

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    }
}
