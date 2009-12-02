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
	/// Summary description for WidgetEntry.
	/// </summary>
	public class WidgetEntry : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox WidgetCombo;
		private WidgetItemType m_wi;
		private bool m_isUpdating = false;
		public event ValueChangedDelegate ValueChanged;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WidgetEntry()
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

		public void SetupCombos(WidgetType[] widgets)
		{
			m_wi = null;
			WidgetCombo.DataSource = null;
			WidgetCombo.DisplayMember = "Name";
			WidgetCombo.ValueMember = "Name";
			WidgetCombo.DataSource = new ArrayList(widgets);
		}


		public void SetItem(WidgetItemType wi)
		{
			try
			{
				m_isUpdating = true;
				m_wi = wi;
				WidgetCombo.SelectedIndex = WidgetCombo.FindString(m_wi.Widget);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WidgetEntry));
            this.label1 = new System.Windows.Forms.Label();
            this.WidgetCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // WidgetCombo
            // 
            this.WidgetCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.WidgetCombo, "WidgetCombo");
            this.WidgetCombo.Name = "WidgetCombo";
            this.WidgetCombo.SelectedIndexChanged += new System.EventHandler(this.WidgetCombo_SelectedIndexChanged);
            // 
            // WidgetEntry
            // 
            this.Controls.Add(this.WidgetCombo);
            this.Controls.Add(this.label1);
            this.Name = "WidgetEntry";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

		}
		#endregion

		private void WidgetCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_wi == null)
				return;
			m_wi.Widget = WidgetCombo.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_wi);
		}
	}
}
