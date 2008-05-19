#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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

	public delegate void ValueChangedDelegate(object sender, object item);

	/// <summary>
	/// Summary description for ContainerEditor.
	/// </summary>
	public class ContainerEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox NameBox;
		private System.Windows.Forms.ComboBox PositionCombo;
		private System.Windows.Forms.ComboBox TypeCombo;

		private bool m_isUpdating = false;
		private ContainerType m_ct;
		public event ValueChangedDelegate ValueChanged;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ContainerEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(ContainerType ct)
		{
			try
			{
				m_isUpdating = true;
				m_ct = ct;
				NameBox.Text = ct.Name;
				PositionCombo.SelectedIndex = PositionCombo.FindString(ct.Position);
				TypeCombo.SelectedIndex = TypeCombo.FindString(ct.Type);
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public void SetupCombos(OSGeo.MapGuide.MaestroAPI.ApplicationDefinitionContainerInfoSet co)
		{
			m_ct = null;
			TypeCombo.DataSource = null;
			TypeCombo.DisplayMember = "Type";
			TypeCombo.ValueMember = "Type";
			TypeCombo.DataSource = new ArrayList(co.ContainerInfo);
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
			this.label3 = new System.Windows.Forms.Label();
			this.NameBox = new System.Windows.Forms.TextBox();
			this.PositionCombo = new System.Windows.Forms.ComboBox();
			this.TypeCombo = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Position";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Type";
			// 
			// NameBox
			// 
			this.NameBox.Location = new System.Drawing.Point(104, 8);
			this.NameBox.Name = "NameBox";
			this.NameBox.Size = new System.Drawing.Size(288, 20);
			this.NameBox.TabIndex = 3;
			this.NameBox.Text = "textBox1";
			this.NameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
			// 
			// PositionCombo
			// 
			this.PositionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PositionCombo.Items.AddRange(new object[] {
															   "",
															   "top",
															   "left",
															   "bottom",
															   "right"});
			this.PositionCombo.Location = new System.Drawing.Point(104, 32);
			this.PositionCombo.Name = "PositionCombo";
			this.PositionCombo.Size = new System.Drawing.Size(288, 21);
			this.PositionCombo.TabIndex = 4;
			this.PositionCombo.SelectedIndexChanged += new System.EventHandler(this.PositionCombo_SelectedIndexChanged);
			// 
			// TypeCombo
			// 
			this.TypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TypeCombo.Location = new System.Drawing.Point(104, 56);
			this.TypeCombo.Name = "TypeCombo";
			this.TypeCombo.Size = new System.Drawing.Size(288, 21);
			this.TypeCombo.TabIndex = 5;
			this.TypeCombo.SelectedIndexChanged += new System.EventHandler(this.TypeCombo_SelectedIndexChanged);
			// 
			// ContainerEditor
			// 
			this.Controls.Add(this.TypeCombo);
			this.Controls.Add(this.PositionCombo);
			this.Controls.Add(this.NameBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "ContainerEditor";
			this.Size = new System.Drawing.Size(392, 88);
			this.ResumeLayout(false);

		}
		#endregion

		private void NameBox_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_ct == null)
				return;

			m_ct.Name = NameBox.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_ct);
		}

		private void PositionCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_ct == null)
				return;

			m_ct.Position = PositionCombo.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_ct);
		}

		private void TypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_ct == null)
				return;

			m_ct.Type = TypeCombo.Text;
			if (ValueChanged != null)
				ValueChanged(this, m_ct);
		}
	}
}
