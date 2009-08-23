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
	/// Summary description for Join.
	/// </summary>
	public class Join : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BrowseFeatureSource;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox JoinName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox FeatureClass;
		private System.Windows.Forms.Label label4;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public event FeatureSourceExtension.NameChangedDelegate NameChanged;

		private EditorInterface m_editor = null;
		private EditorInterface m_browseEditor = null;

		private AttributeRelateType m_join;
		private System.Windows.Forms.TextBox FeatureSource;
		private System.Windows.Forms.CheckBox ForceOnToOne;
		private System.Windows.Forms.ComboBox RelationType;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel KeyWarning;
		private bool m_isUpdating = false;

		public Join()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			RelationType.Items.Clear();
			foreach(string s in Enum.GetNames(typeof(RelateTypeEnum)))
				RelationType.Items.Add(s);
		}

		public void SetItem(AttributeRelateType join, EditorInterface browseEditor)
		{
			m_join = join;
			m_browseEditor = browseEditor;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;

				JoinName.Text = m_join.Name;
				FeatureSource.Text = m_join.ResourceId;
				UpdateSchemaList();
				FeatureClass.SelectedIndex = FeatureClass.FindString(m_join.AttributeClass);
				RelationType.SelectedIndex = RelationType.FindStringExact(m_join.RelateType.ToString());
				ForceOnToOne.Checked = m_join.ForceOneToOne;
				KeyWarning.Visible = m_join.RelateProperty == null || m_join.RelateProperty.Count == 0;
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void UpdateSchemaList()
		{
			try
			{
				FeatureClass.BeginUpdate();
				FeatureClass.Items.Clear();

				if (m_join.ResourceId == null || m_join.ResourceId.Length == 0)
				{
					panel1.Enabled = false;
					return;
				}

				FeatureSource m_feature = m_browseEditor.CurrentConnection.GetFeatureSource(m_join.ResourceId);
				foreach(FeatureSourceDescription.FeatureSourceSchema fsc in m_feature.DescribeSource().Schemas)
					FeatureClass.Items.Add(fsc.Fullname);
				panel1.Enabled = true;
			}
			catch 
			{
				panel1.Enabled = false;
			}
			finally
			{
				try { FeatureClass.EndUpdate(); }
				catch {}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Join));
			this.FeatureSource = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BrowseFeatureSource = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ForceOnToOne = new System.Windows.Forms.CheckBox();
			this.RelationType = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.FeatureClass = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.JoinName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.KeyWarning = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.KeyWarning.SuspendLayout();
			this.SuspendLayout();
			// 
			// FeatureSource
			// 
			this.FeatureSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FeatureSource.Location = new System.Drawing.Point(152, 32);
			this.FeatureSource.Name = "FeatureSource";
			this.FeatureSource.ReadOnly = true;
			this.FeatureSource.Size = new System.Drawing.Size(320, 20);
			this.FeatureSource.TabIndex = 8;
			this.FeatureSource.Text = "";
			this.FeatureSource.TextChanged += new System.EventHandler(this.FeatureSource_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Join with featuresource";
			// 
			// BrowseFeatureSource
			// 
			this.BrowseFeatureSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseFeatureSource.Location = new System.Drawing.Point(480, 32);
			this.BrowseFeatureSource.Name = "BrowseFeatureSource";
			this.BrowseFeatureSource.Size = new System.Drawing.Size(24, 20);
			this.BrowseFeatureSource.TabIndex = 9;
			this.BrowseFeatureSource.Text = "...";
			this.BrowseFeatureSource.Click += new System.EventHandler(this.BrowseFeatureSource_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.KeyWarning);
			this.panel1.Controls.Add(this.ForceOnToOne);
			this.panel1.Controls.Add(this.RelationType);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.FeatureClass);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Enabled = false;
			this.panel1.Location = new System.Drawing.Point(8, 56);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(496, 176);
			this.panel1.TabIndex = 10;
			// 
			// ForceOnToOne
			// 
			this.ForceOnToOne.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ForceOnToOne.Location = new System.Drawing.Point(0, 56);
			this.ForceOnToOne.Name = "ForceOnToOne";
			this.ForceOnToOne.Size = new System.Drawing.Size(456, 16);
			this.ForceOnToOne.TabIndex = 12;
			this.ForceOnToOne.Text = "Force one-to-one relation";
			this.ForceOnToOne.CheckedChanged += new System.EventHandler(this.ForceOnToOne_CheckedChanged);
			// 
			// RelationType
			// 
			this.RelationType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RelationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RelationType.Location = new System.Drawing.Point(144, 24);
			this.RelationType.Name = "RelationType";
			this.RelationType.Size = new System.Drawing.Size(352, 21);
			this.RelationType.TabIndex = 11;
			this.RelationType.SelectedIndexChanged += new System.EventHandler(this.RelationType_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "Join type";
			// 
			// FeatureClass
			// 
			this.FeatureClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FeatureClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FeatureClass.Location = new System.Drawing.Point(144, 0);
			this.FeatureClass.Name = "FeatureClass";
			this.FeatureClass.Size = new System.Drawing.Size(352, 21);
			this.FeatureClass.TabIndex = 9;
			this.FeatureClass.SelectedIndexChanged += new System.EventHandler(this.FeatureClass_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Featureclass";
			// 
			// JoinName
			// 
			this.JoinName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.JoinName.Location = new System.Drawing.Point(152, 8);
			this.JoinName.Name = "JoinName";
			this.JoinName.Size = new System.Drawing.Size(352, 20);
			this.JoinName.TabIndex = 12;
			this.JoinName.Text = "";
			this.JoinName.TextChanged += new System.EventHandler(this.JoinName_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 16);
			this.label3.TabIndex = 11;
			this.label3.Text = "Join name";
			// 
			// KeyWarning
			// 
			this.KeyWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.KeyWarning.Controls.Add(this.label5);
			this.KeyWarning.Controls.Add(this.pictureBox1);
			this.KeyWarning.Location = new System.Drawing.Point(16, 88);
			this.KeyWarning.Name = "KeyWarning";
			this.KeyWarning.Size = new System.Drawing.Size(456, 16);
			this.KeyWarning.TabIndex = 13;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(16, 16);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(24, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(432, 23);
			this.label5.TabIndex = 1;
			this.label5.Text = "A join must have at least one key";
			// 
			// Join
			// 
			this.Controls.Add(this.JoinName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.BrowseFeatureSource);
			this.Controls.Add(this.FeatureSource);
			this.Controls.Add(this.label2);
			this.Name = "Join";
			this.Size = new System.Drawing.Size(512, 240);
			this.panel1.ResumeLayout(false);
			this.KeyWarning.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void JoinName_TextChanged(object sender, System.EventArgs e)
		{
			if (m_join == null || m_isUpdating)
				return;

			m_join.Name = JoinName.Text;
			if (NameChanged != null)
				NameChanged(m_join, m_join.Name);
			if (m_editor != null)
				m_editor.HasChanged();
		}

		private void FeatureSource_TextChanged(object sender, System.EventArgs e)
		{
			if (m_join == null || m_isUpdating)
				return;

			m_join.ResourceId = FeatureSource.Text;
			if (m_editor != null)
				m_editor.HasChanged();
		
		}

		private void RelationType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_join == null || m_isUpdating)
				return;

			m_join.RelateType = (RelateTypeEnum)Enum.Parse(typeof(RelateTypeEnum), RelationType.Text);
			m_join.RelateTypeSpecified = true;
			if (m_editor != null)
				m_editor.HasChanged();
		}

		private void ForceOnToOne_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_join == null || m_isUpdating)
				return;

			m_join.ForceOneToOne = ForceOnToOne.Checked;
			m_join.ForceOneToOneSpecified = true;
			if (m_editor != null)
				m_editor.HasChanged();
		}

		private void BrowseFeatureSource_Click(object sender, System.EventArgs e)
		{
			string item = m_browseEditor.BrowseResource("FeatureSource");
			if (item != null)
			{
				FeatureSource.Text = item;
				UpdateSchemaList();
			}
		}

		private void FeatureClass_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_join == null || m_isUpdating)
				return;

			m_join.AttributeClass = FeatureClass.Text;
			if (m_editor != null)
				m_editor.HasChanged();
		}
	}
}
