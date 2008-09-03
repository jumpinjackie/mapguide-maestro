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
	/// Summary description for Calulated.
	/// </summary>
	public class Calculated : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox PropertyName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox PropertyDefinition;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public event FeatureSourceExtension.NameChangedDelegate NameChanged;

		private EditorInterface m_editor = null;

		private CalculatedPropertyType m_property;
		private bool m_isUpdating = false;

		public Calculated()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(CalculatedPropertyType property)
		{
			m_property = property;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;

				PropertyName.Text = m_property.Name;
				PropertyDefinition.Text = m_property.Expression;
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
			this.PropertyName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.PropertyDefinition = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// PropertyName
			// 
			this.PropertyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.PropertyName.Location = new System.Drawing.Point(160, 6);
			this.PropertyName.Name = "PropertyName";
			this.PropertyName.Size = new System.Drawing.Size(320, 20);
			this.PropertyName.TabIndex = 6;
			this.PropertyName.Text = "";
			this.PropertyName.TextChanged += new System.EventHandler(this.PropertyName_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Property name";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(208, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Calculated property definition";
			// 
			// PropertyDefinition
			// 
			this.PropertyDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.PropertyDefinition.Location = new System.Drawing.Point(8, 48);
			this.PropertyDefinition.Multiline = true;
			this.PropertyDefinition.Name = "PropertyDefinition";
			this.PropertyDefinition.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.PropertyDefinition.Size = new System.Drawing.Size(472, 184);
			this.PropertyDefinition.TabIndex = 7;
			this.PropertyDefinition.Text = "";
			this.PropertyDefinition.TextChanged += new System.EventHandler(this.PropertyDefinition_TextChanged);
			// 
			// Calculated
			// 
			this.Controls.Add(this.PropertyDefinition);
			this.Controls.Add(this.PropertyName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Calculated";
			this.Size = new System.Drawing.Size(488, 232);
			this.ResumeLayout(false);

		}
		#endregion

		private void PropertyDefinition_TextChanged(object sender, System.EventArgs e)
		{
			if (m_property == null || m_isUpdating)
				return;

			m_property.Expression = PropertyDefinition.Text;
			if (m_editor != null)
				m_editor.HasChanged();
		}

		private void PropertyName_TextChanged(object sender, System.EventArgs e)
		{
			if (m_property == null || m_isUpdating)
				return;

			m_property.Name = PropertyName.Text;
			if (NameChanged != null)
				NameChanged(m_property, m_property.Name);
			if (m_editor != null)
				m_editor.HasChanged();
		
		}
	}
}
