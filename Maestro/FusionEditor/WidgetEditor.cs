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
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor
{
	/// <summary>
	/// Summary description for WidgetEditor.
	/// </summary>
	public class WidgetEditor : BasisWidgetEditor
	{
		private System.Windows.Forms.ComboBox TypeCombo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox NameText;

		private System.Windows.Forms.TextBox LocationText;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WidgetEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		public void SetupCombos(OSGeo.MapGuide.MaestroAPI.ApplicationDefinitionWidgetInfoSet wi)
		{
			m_w = null;
			TypeCombo.DataSource = null;
			TypeCombo.DisplayMember = "Type";
			TypeCombo.ValueMember = "Type";
			TypeCombo.DataSource = new ArrayList(wi.WidgetInfo);
		}

		public override void SetItem(WidgetType w)
		{
			try
			{
				m_isUpdating = true;
				m_w = w;
				NameText.Text = w.Name;
				LocationText.Text = w.Location;
				TypeCombo.SelectedIndex = TypeCombo.FindString(w.Type);
			}
			finally
			{
				m_isUpdating = false;
			}
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.TypeCombo = new System.Windows.Forms.ComboBox();
			this.NameText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.LocationText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// TypeCombo
			// 
			this.TypeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TypeCombo.Location = new System.Drawing.Point(96, 32);
			this.TypeCombo.Name = "TypeCombo";
			this.TypeCombo.Size = new System.Drawing.Size(288, 21);
			this.TypeCombo.TabIndex = 11;
			this.TypeCombo.SelectedIndexChanged += new System.EventHandler(this.TypeCombo_SelectedIndexChanged);
			// 
			// NameText
			// 
			this.NameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.NameText.Location = new System.Drawing.Point(96, 8);
			this.NameText.Name = "NameText";
			this.NameText.Size = new System.Drawing.Size(288, 20);
			this.NameText.TabIndex = 9;
			this.NameText.Text = "textBox1";
			this.NameText.TextChanged += new System.EventHandler(this.NameText_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Type";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Position";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Name";
			// 
			// LocationText
			// 
			this.LocationText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LocationText.Location = new System.Drawing.Point(96, 56);
			this.LocationText.Name = "LocationText";
			this.LocationText.Size = new System.Drawing.Size(288, 20);
			this.LocationText.TabIndex = 23;
			this.LocationText.Text = "LocationText";
			this.LocationText.TextChanged += new System.EventHandler(this.LocationText_TextChanged);
			// 
			// WidgetEditor
			// 
			this.Controls.Add(this.LocationText);
			this.Controls.Add(this.TypeCombo);
			this.Controls.Add(this.NameText);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "WidgetEditor";
			this.Size = new System.Drawing.Size(392, 80);
			this.ResumeLayout(false);

		}
		#endregion

		private void NameText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			m_w.Name = NameText.Text;
			RaiseValueChanged();
		}

		private void TypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			m_w.Type = TypeCombo.Text;		
			RaiseValueChanged();
		}

		private void LocationText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;
			m_w.Location = LocationText.Text;
			RaiseValueChanged();
		}


	}
}
