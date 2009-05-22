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
	/// Summary description for FeatureSourceEditorMSSQLSpatial.
	/// </summary>
	public class FeatureSourceEditorKingKML : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		private System.ComponentModel.IContainer components;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.ToolTip toolTips;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox FilePathType;
		private ResourceEditors.FeatureSourceEditors.KingKML.Unmanaged unmanaged;
		private ResourceEditors.FeatureSourceEditors.KingKML.Managed managed;
		private System.Windows.Forms.Panel providerpanel;
		private bool m_isUpdating = false;
		private Globalizator.Globalizator m_globalizor;

		public FeatureSourceEditorKingKML()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			foreach(Control c in providerpanel.Controls)
			{
				c.Visible = false;
				c.Dock = DockStyle.Fill;
			}
			m_globalizor = new Globalizator.Globalizator(this);
			string tmp = m_globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditorKingKML.FilePathType.Items");
			if (tmp != null && tmp.Trim().Length > 0)
			{
				ArrayList fix = new ArrayList();
				foreach(string s in tmp.Trim().Split('\n'))
					if (s.Trim().Length > 0)
						fix.Add(s.Trim());

				if (fix.Count == FilePathType.Items.Count)
				{
					FilePathType.Items.Clear();
					FilePathType.Items.AddRange(fix.ToArray());
				}
			}
		}

		public FeatureSourceEditorKingKML(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;

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
			this.FilePathType = new System.Windows.Forms.ComboBox();
			this.providerpanel = new System.Windows.Forms.Panel();
			this.managed = new ResourceEditors.FeatureSourceEditors.KingKML.Managed();
			this.unmanaged = new ResourceEditors.FeatureSourceEditors.KingKML.Unmanaged();
			this.providerpanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "File location";
			// 
			// FilePathType
			// 
			this.FilePathType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FilePathType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FilePathType.Items.AddRange(new object[] {
															  "Internal file (Managed)",
															  "File on the server (Unmanaged)"});
			this.FilePathType.Location = new System.Drawing.Point(136, 0);
			this.FilePathType.Name = "FilePathType";
			this.FilePathType.Size = new System.Drawing.Size(304, 21);
			this.FilePathType.TabIndex = 1;
			this.FilePathType.SelectedIndexChanged += new System.EventHandler(this.FilePathType_SelectedIndexChanged);
			// 
			// providerpanel
			// 
			this.providerpanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.providerpanel.Controls.Add(this.managed);
			this.providerpanel.Controls.Add(this.unmanaged);
			this.providerpanel.Location = new System.Drawing.Point(8, 32);
			this.providerpanel.Name = "providerpanel";
			this.providerpanel.Size = new System.Drawing.Size(432, 216);
			this.providerpanel.TabIndex = 2;
			// 
			// managed
			// 
			this.managed.Location = new System.Drawing.Point(16, 16);
			this.managed.Name = "managed";
			this.managed.Size = new System.Drawing.Size(136, 144);
			this.managed.TabIndex = 2;
			// 
			// unmanaged
			// 
			this.unmanaged.Location = new System.Drawing.Point(136, 16);
			this.unmanaged.Name = "unmanaged";
			this.unmanaged.Size = new System.Drawing.Size(224, 144);
			this.unmanaged.TabIndex = 1;
			// 
			// FeatureSourceEditorKingKML
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(248, 128);
			this.Controls.Add(this.providerpanel);
			this.Controls.Add(this.FilePathType);
			this.Controls.Add(this.label1);
			this.Name = "FeatureSourceEditorKingKML";
			this.Size = new System.Drawing.Size(448, 256);
			this.providerpanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public object Resource
		{
			get { return m_feature; }
			set 
			{
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
				managed.SetItem(m_editor, m_feature);
				unmanaged.SetItem(m_editor, m_feature);
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
				if (m_feature == null || m_feature.Parameter == null)
					return;

                if (m_feature.Parameter["File"] == null || m_feature.Parameter["File"].Trim().Length == 0 || m_feature.Parameter["File"].IndexOf("%MG_DATA_FILE_PATH%") >= 0)
					FilePathType.SelectedIndex = 0;
				else
					FilePathType.SelectedIndex = 1;
				
				managed.UpdateDisplay();
				unmanaged.UpdateDisplay();
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public bool Save(string savename)
		{
			return false;
		}

		private void FilePathType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (FilePathType.SelectedIndex == 0)
			{
				managed.UpdateDisplay();
				managed.Visible = true;
				unmanaged.Visible = false;
			}
			else
			{
				unmanaged.UpdateDisplay();
				unmanaged.Visible = true;
				managed.Visible = false;
			}
		}

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    
    }
}
