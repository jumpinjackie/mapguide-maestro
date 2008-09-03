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
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions
{
	/// <summary>
	/// Summary description for Key.
	/// </summary>
	public class Key : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private FeatureSourceDescription.FeatureSourceSchema m_primary;
		private FeatureSourceDescription.FeatureSourceSchema m_secondary;
		private RelatePropertyType m_key;
		private System.Windows.Forms.ComboBox SecondaryKey;
		private System.Windows.Forms.ComboBox PrimaryKey;
		private bool m_isUpdating = false;
		private EditorInterface m_editor = null;

		public event FeatureSourceExtension.NameChangedDelegate NameChanged;

		public Key()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(FeatureSourceDescription.FeatureSourceSchema primary, FeatureSourceDescription.FeatureSourceSchema secondary, RelatePropertyType key)
		{
			m_primary = primary;
			m_secondary = secondary;
			m_key = key;

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;

				try
				{
					PrimaryKey.BeginUpdate();
					PrimaryKey.Items.Clear();
					if (m_primary != null)
					{
						PrimaryKey.Enabled = true;
						foreach(FeatureSetColumn fsc in m_primary.Columns)
                            if (fsc.Type != OSGeo.MapGuide.MaestroAPI.Utility.RasterType && fsc.Type != OSGeo.MapGuide.MaestroAPI.Utility.GeometryType)
								PrimaryKey.Items.Add(fsc.Name);
					}
					else
						PrimaryKey.Enabled = false;
				}
				finally
				{
					try { PrimaryKey.EndUpdate(); }
					catch {}
				}

				try
				{
					SecondaryKey.BeginUpdate();
					SecondaryKey.Items.Clear();
					if (m_secondary != null)
					{
						SecondaryKey.Enabled = true;
						foreach(FeatureSetColumn fsc in m_secondary.Columns)
                            if (fsc.Type != OSGeo.MapGuide.MaestroAPI.Utility.RasterType && fsc.Type != OSGeo.MapGuide.MaestroAPI.Utility.GeometryType)
								SecondaryKey.Items.Add(fsc.Name);
					}
					else
						SecondaryKey.Enabled = false;
				}
				finally
				{
					try { SecondaryKey.EndUpdate(); }
					catch {}
				}

				if (PrimaryKey.Enabled)
					PrimaryKey.SelectedIndex = PrimaryKey.FindString(m_key.FeatureClassProperty);
				if (SecondaryKey.Enabled)
					SecondaryKey.SelectedIndex = SecondaryKey.FindString(m_key.AttributeClassProperty);
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
			this.SecondaryKey = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.PrimaryKey = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// SecondaryKey
			// 
			this.SecondaryKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SecondaryKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecondaryKey.Location = new System.Drawing.Point(160, 32);
			this.SecondaryKey.Name = "SecondaryKey";
			this.SecondaryKey.Size = new System.Drawing.Size(320, 21);
			this.SecondaryKey.TabIndex = 7;
			this.SecondaryKey.SelectedIndexChanged += new System.EventHandler(this.SecondaryKey_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Left column";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Right column";
			// 
			// PrimaryKey
			// 
			this.PrimaryKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.PrimaryKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PrimaryKey.Location = new System.Drawing.Point(160, 8);
			this.PrimaryKey.Name = "PrimaryKey";
			this.PrimaryKey.Size = new System.Drawing.Size(320, 21);
			this.PrimaryKey.TabIndex = 8;
			this.PrimaryKey.SelectedIndexChanged += new System.EventHandler(this.PrimaryKey_SelectedIndexChanged);
			// 
			// Key
			// 
			this.Controls.Add(this.PrimaryKey);
			this.Controls.Add(this.SecondaryKey);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Key";
			this.Size = new System.Drawing.Size(488, 56);
			this.ResumeLayout(false);

		}
		#endregion

		private void PrimaryKey_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_key == null || m_isUpdating)
				return;

			m_key.FeatureClassProperty = PrimaryKey.Text;
			if (NameChanged != null)
				NameChanged(m_key, m_key.FeatureClassProperty + " : " + m_key.AttributeClassProperty);
			if (m_editor != null)
				m_editor.HasChanged();
		}

		private void SecondaryKey_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_key == null || m_isUpdating)
				return;

			m_key.AttributeClassProperty = SecondaryKey.Text;
			if (NameChanged != null)
				NameChanged(m_key, m_key.FeatureClassProperty + " : " + m_key.AttributeClassProperty);
			if (m_editor != null)
				m_editor.HasChanged();
		}
	}
}
