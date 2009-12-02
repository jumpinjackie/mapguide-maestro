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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WidgetEditor));
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
            resources.ApplyResources(this.TypeCombo, "TypeCombo");
            this.TypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeCombo.Name = "TypeCombo";
            this.TypeCombo.SelectedIndexChanged += new System.EventHandler(this.TypeCombo_SelectedIndexChanged);
            // 
            // NameText
            // 
            resources.ApplyResources(this.NameText, "NameText");
            this.NameText.Name = "NameText";
            this.NameText.TextChanged += new System.EventHandler(this.NameText_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // LocationText
            // 
            resources.ApplyResources(this.LocationText, "LocationText");
            this.LocationText.Name = "LocationText";
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
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

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
