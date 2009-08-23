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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions
{
	/// <summary>
	/// Summary description for Extension.
	/// </summary>
	public class Extension : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox ExtensionName;
		private System.Windows.Forms.ComboBox ExtensionSchema;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public event FeatureSourceExtension.NameChangedDelegate NameChanged;
		private EditorInterface m_editor = null;

		private FeatureSourceTypeExtension m_extension;
		private bool m_isUpdating = false;
		private FeatureSourceDescription.FeatureSourceSchema[] m_fsd;

		public Extension()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(FeatureSourceDescription.FeatureSourceSchema[] fsd, FeatureSourceTypeExtension extension)
		{
			m_fsd = fsd;
			m_extension = extension;

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				try
				{
					ExtensionSchema.BeginUpdate();
					ExtensionSchema.Items.Clear();
					foreach(FeatureSourceDescription.FeatureSourceSchema fsc in m_fsd)
						ExtensionSchema.Items.Add(fsc.Fullname);
					ExtensionSchema.Enabled = true;
				}
				catch 
				{
					ExtensionSchema.Enabled = false;
				}
				finally
				{
					try { ExtensionSchema.EndUpdate(); }
					catch {}
				}


				ExtensionSchema.SelectedIndex = ExtensionSchema.FindString(m_extension.FeatureClass);
				ExtensionName.Text = m_extension.Name;
			}
			finally
			{
				m_isUpdating = false;
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
			this.label2 = new System.Windows.Forms.Label();
			this.ExtensionName = new System.Windows.Forms.TextBox();
			this.ExtensionSchema = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Feature class to extend";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Extension name";
			// 
			// ExtensionName
			// 
			this.ExtensionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ExtensionName.Location = new System.Drawing.Point(160, 8);
			this.ExtensionName.Name = "ExtensionName";
			this.ExtensionName.Size = new System.Drawing.Size(320, 20);
			this.ExtensionName.TabIndex = 2;
			this.ExtensionName.Text = "";
			this.ExtensionName.TextChanged += new System.EventHandler(this.ExtensionName_TextChanged);
			// 
			// ExtensionSchema
			// 
			this.ExtensionSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ExtensionSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ExtensionSchema.Location = new System.Drawing.Point(160, 32);
			this.ExtensionSchema.Name = "ExtensionSchema";
			this.ExtensionSchema.Size = new System.Drawing.Size(320, 21);
			this.ExtensionSchema.TabIndex = 3;
			this.ExtensionSchema.SelectedIndexChanged += new System.EventHandler(this.ExtensionSchema_SelectedIndexChanged);
			// 
			// Extension
			// 
			this.Controls.Add(this.ExtensionSchema);
			this.Controls.Add(this.ExtensionName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Extension";
			this.Size = new System.Drawing.Size(488, 56);
			this.ResumeLayout(false);

		}
		#endregion

		private void ExtensionSchema_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_extension == null || m_isUpdating)
				return;

			m_extension.FeatureClass = ExtensionSchema.Text;
			if (m_editor != null)
				m_editor.HasChanged();
		}

		private void ExtensionName_TextChanged(object sender, System.EventArgs e)
		{
			if (m_extension == null || m_isUpdating)
				return;

			m_extension.Name = ExtensionName.Text;
			if (NameChanged != null)
				NameChanged(m_extension, m_extension.Name);
			if (m_editor != null)
				m_editor.HasChanged();
		}
	}
}
