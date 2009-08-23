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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OSGeo.MapGuide.Maestro.ResourceEditors;
using System.Collections.Specialized;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC
{
	/// <summary>
	/// Summary description for GeometryColumnsEditor.
	/// </summary>
	public class GeometryColumnsEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel TablePanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox KeyColumn;
		private System.Windows.Forms.ComboBox XColumn;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox YColumn;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox ZColumn;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.ComponentModel.IContainer components;

		private Hashtable m_geom = null;
		private Hashtable m_keys = null;
		private System.Windows.Forms.Panel PropertyPanel;
		private System.Windows.Forms.ImageList ListImages;
		private System.Windows.Forms.GroupBox GeometryPanel;
		private System.Windows.Forms.CheckBox GeometryEnabled;
		private System.Windows.Forms.ListView TableList;
		private bool m_isUpdating = false;

		public GeometryColumnsEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GeometryColumnsEditor));
			this.panel1 = new System.Windows.Forms.Panel();
			this.PropertyPanel = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.TablePanel = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.KeyColumn = new System.Windows.Forms.ComboBox();
			this.ListImages = new System.Windows.Forms.ImageList(this.components);
			this.XColumn = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.YColumn = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.ZColumn = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.GeometryPanel = new System.Windows.Forms.GroupBox();
			this.GeometryEnabled = new System.Windows.Forms.CheckBox();
			this.TableList = new System.Windows.Forms.ListView();
			this.panel1.SuspendLayout();
			this.PropertyPanel.SuspendLayout();
			this.panel3.SuspendLayout();
			this.TablePanel.SuspendLayout();
			this.GeometryPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.TableList);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(168, 389);
			this.panel1.TabIndex = 0;
			// 
			// PropertyPanel
			// 
			this.PropertyPanel.Controls.Add(this.GeometryEnabled);
			this.PropertyPanel.Controls.Add(this.TablePanel);
			this.PropertyPanel.Controls.Add(this.GeometryPanel);
			this.PropertyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PropertyPanel.Location = new System.Drawing.Point(176, 0);
			this.PropertyPanel.Name = "PropertyPanel";
			this.PropertyPanel.Size = new System.Drawing.Size(360, 389);
			this.PropertyPanel.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.CancelBtn);
			this.panel3.Controls.Add(this.OKBtn);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 389);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(536, 48);
			this.panel3.TabIndex = 2;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(168, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 389);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// TablePanel
			// 
			this.TablePanel.Controls.Add(this.KeyColumn);
			this.TablePanel.Controls.Add(this.label1);
			this.TablePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.TablePanel.Location = new System.Drawing.Point(0, 0);
			this.TablePanel.Name = "TablePanel";
			this.TablePanel.Size = new System.Drawing.Size(360, 40);
			this.TablePanel.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Key column";
			// 
			// KeyColumn
			// 
			this.KeyColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.KeyColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.KeyColumn.Location = new System.Drawing.Point(104, 8);
			this.KeyColumn.Name = "KeyColumn";
			this.KeyColumn.Size = new System.Drawing.Size(248, 21);
			this.KeyColumn.TabIndex = 1;
			this.KeyColumn.SelectedIndexChanged += new System.EventHandler(this.KeyColumn_SelectedIndexChanged);
			// 
			// ListImages
			// 
			this.ListImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ListImages.ImageSize = new System.Drawing.Size(16, 16);
			this.ListImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ListImages.ImageStream")));
			this.ListImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// XColumn
			// 
			this.XColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.XColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.XColumn.Location = new System.Drawing.Point(120, 24);
			this.XColumn.Name = "XColumn";
			this.XColumn.Size = new System.Drawing.Size(216, 21);
			this.XColumn.TabIndex = 3;
			this.XColumn.SelectedIndexChanged += new System.EventHandler(this.XColumn_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 2;
			this.label6.Text = "X Column";
			// 
			// YColumn
			// 
			this.YColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.YColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.YColumn.Location = new System.Drawing.Point(120, 48);
			this.YColumn.Name = "YColumn";
			this.YColumn.Size = new System.Drawing.Size(216, 21);
			this.YColumn.TabIndex = 5;
			this.YColumn.SelectedIndexChanged += new System.EventHandler(this.YColumn_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 4;
			this.label4.Text = "Y Column";
			// 
			// ZColumn
			// 
			this.ZColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ZColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ZColumn.Location = new System.Drawing.Point(120, 72);
			this.ZColumn.Name = "ZColumn";
			this.ZColumn.Size = new System.Drawing.Size(216, 21);
			this.ZColumn.TabIndex = 7;
			this.ZColumn.SelectedIndexChanged += new System.EventHandler(this.ZColumn_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 16);
			this.label5.TabIndex = 6;
			this.label5.Text = "Z Column";
			// 
			// OKBtn
			// 
			this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.OKBtn.Location = new System.Drawing.Point(160, 16);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(104, 24);
			this.OKBtn.TabIndex = 0;
			this.OKBtn.Text = "OK";
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(280, 16);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(104, 24);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "Cancel";
			// 
			// GeometryPanel
			// 
			this.GeometryPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.GeometryPanel.Controls.Add(this.label6);
			this.GeometryPanel.Controls.Add(this.label5);
			this.GeometryPanel.Controls.Add(this.XColumn);
			this.GeometryPanel.Controls.Add(this.label4);
			this.GeometryPanel.Controls.Add(this.ZColumn);
			this.GeometryPanel.Controls.Add(this.YColumn);
			this.GeometryPanel.Location = new System.Drawing.Point(8, 40);
			this.GeometryPanel.Name = "GeometryPanel";
			this.GeometryPanel.Size = new System.Drawing.Size(344, 104);
			this.GeometryPanel.TabIndex = 8;
			this.GeometryPanel.TabStop = false;
			// 
			// GeometryEnabled
			// 
			this.GeometryEnabled.Location = new System.Drawing.Point(24, 40);
			this.GeometryEnabled.Name = "GeometryEnabled";
			this.GeometryEnabled.Size = new System.Drawing.Size(152, 16);
			this.GeometryEnabled.TabIndex = 9;
			this.GeometryEnabled.Text = "Geometry";
			this.GeometryEnabled.CheckedChanged += new System.EventHandler(this.GeometryEnabled_CheckedChanged);
			// 
			// TableList
			// 
			this.TableList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TableList.Location = new System.Drawing.Point(0, 0);
			this.TableList.MultiSelect = false;
			this.TableList.Name = "TableList";
			this.TableList.Size = new System.Drawing.Size(168, 389);
			this.TableList.SmallImageList = this.ListImages;
			this.TableList.TabIndex = 1;
			this.TableList.View = System.Windows.Forms.View.List;
			this.TableList.SelectedIndexChanged += new System.EventHandler(this.TableList_SelectedIndexChanged);
			// 
			// GeometryColumnsEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(536, 437);
			this.Controls.Add(this.PropertyPanel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "GeometryColumnsEditor";
			this.Text = "GeometryColumnsEditor";
			this.Load += new System.EventHandler(this.GeometryColumnsEditor_Load);
			this.panel1.ResumeLayout(false);
			this.PropertyPanel.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.TablePanel.ResumeLayout(false);
			this.GeometryPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void GeometryColumnsEditor_Load(object sender, System.EventArgs e)
		{
			PropertyPanel.Visible = false;
			if (TableList.Items.Count > 0)
				TableList.Items[0].Selected = true;
		}

		/// <summary>
		/// Sets up the dialog.
		/// </summary>
		/// <param name="tables">A schema definition</param>
		/// <param name="geometryColumns">A table of geometry coumns, where key is tablename, and data is an arraylist of GeometryColumn's</param>
		public void Setup(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription tables, Hashtable geometryColumns, Hashtable keys)
		{
			TableList.Items.Clear();
			foreach(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm in tables.Schemas)
			{
				ListViewItem lvi = new ListViewItem(scm.Name);
				lvi.Tag = scm;
				TableList.Items.Add(lvi);
			}
			m_geom = geometryColumns;
			m_keys = keys;
			UpdateListDisplay();
		}

		public Hashtable GeometryColumns { get { return m_geom; } }
		public Hashtable KeyColumns { get { return m_keys; } }

		private void SelectTable(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm)
		{
			if (scm == null)
				return;

			ArrayList coordinates = new ArrayList();
			ArrayList keys = new ArrayList();

			foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn fsc in scm.Columns)
			{
				if (fsc.Type != typeof(bool) && fsc.Type != OSGeo.MapGuide.MaestroAPI.Utility.RasterType && fsc.Type != OSGeo.MapGuide.MaestroAPI.Utility.GeometryType)
					keys.Add(fsc.Name);
				if (fsc.Type == typeof(double) || fsc.Type == typeof(int) || fsc.Type == typeof(short) || fsc.Type == typeof(long) || fsc.Type == typeof(float))
					coordinates.Add(fsc.Name);
			}

			XColumn.Items.Clear();
			YColumn.Items.Clear();
			ZColumn.Items.Clear();
			KeyColumn.Items.Clear();

			XColumn.Items.AddRange(coordinates.ToArray());
			YColumn.Items.AddRange(coordinates.ToArray());
			ZColumn.Items.Add("");
			ZColumn.Items.AddRange(coordinates.ToArray());

			KeyColumn.Items.Add("");
			KeyColumn.Items.AddRange(keys.ToArray());
		}

		private void UpdateColumnPanel(FeatureSourceEditorODBC.GeometryColumn column)
		{
			bool backSet = m_isUpdating;
			try
			{
				m_isUpdating = true;
				XColumn.SelectedIndex = XColumn.FindString(column.XColumn);
				YColumn.SelectedIndex = YColumn.FindString(column.YColumn);
				ZColumn.SelectedIndex = ZColumn.FindString(column.ZColumn);
			}
			finally
			{
				m_isUpdating = backSet;
			}
		}

		private void UpdateTablePanel(OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema table)
		{
			bool backSet = m_isUpdating;
			try
			{
				m_isUpdating = true;
				//KeyColumn.SelectedIndex = KeyColumn.FindString(key[0]);
			}
			finally
			{
				m_isUpdating = backSet;
			}
		}

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private FeatureSourceEditorODBC.GeometryColumn GetCurrentGeom()
		{
			if (TableList.SelectedItems.Count != 1)
				return null;
			
			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm = TableList.SelectedItems[0].Tag as OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema;
			if (scm == null)
				return null;

			if (m_geom.ContainsKey(scm.Name))
				return m_geom[scm.Name] as FeatureSourceEditorODBC.GeometryColumn;
			else
				return null;
		}

		private void XColumn_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			FeatureSourceEditorODBC.GeometryColumn gc = GetCurrentGeom();
			if (gc == null)
				return;

			gc.XColumn = XColumn.Text;
		}

		private void YColumn_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			FeatureSourceEditorODBC.GeometryColumn gc = GetCurrentGeom();
			if (gc == null)
				return;

			gc.YColumn = YColumn.Text;
		}

		private void ZColumn_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			FeatureSourceEditorODBC.GeometryColumn gc = GetCurrentGeom();
			if (gc == null)
				return;

			gc.ZColumn = ZColumn.Text == null || ZColumn.Text.Length == 0 ? null : ZColumn.Text;
		}

		private void KeyColumn_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (TableList.SelectedItems.Count != 1)
				return;
			
			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm = TableList.SelectedItems[0].Tag as OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema;
			if (scm == null)
				return;

			if (KeyColumn.SelectedIndex < 0 || KeyColumn.Text.Trim().Length == 0)
			{
				if (m_keys.Contains(scm.Name))
					m_keys.Remove(scm.Name);
			}
			else
				m_keys[scm.Name] = KeyColumn.Text;
		}

		private void TableList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (TableList.SelectedItems.Count != 1)
			{
				PropertyPanel.Visible = false;
				return;
			}
			
			GeometryEnabled.Tag = null;
			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm = TableList.SelectedItems[0].Tag as OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema;
			if (scm == null)
			{
				PropertyPanel.Visible = false;
				return;
			}

			SelectTable(scm);

			PropertyPanel.Visible = true;
			if (m_geom.ContainsKey(scm.Name))
			{
				GeometryEnabled.Checked = true;
				UpdateColumnPanel((FeatureSourceEditorODBC.GeometryColumn)m_geom[scm.Name]);
			}
			else
			{
				GeometryEnabled.Checked = false;
				bool backSet = m_isUpdating;
				try	{ XColumn.SelectedIndex = YColumn.SelectedIndex = ZColumn.SelectedIndex = -1; }
				finally { m_isUpdating = backSet; }
			}

			if (m_keys.ContainsKey(scm.Name))
			{
				bool backSet = m_isUpdating;
				try	{ KeyColumn.SelectedIndex = KeyColumn.FindString((string)m_keys[scm.Name]); }
				finally { m_isUpdating = backSet; }
			}

		}

		private void UpdateListDisplay()
		{
			foreach(ListViewItem lvi in TableList.Items)
				lvi.ImageIndex = m_geom.ContainsKey(lvi.Text) ? 1 : 0;
		}

		private void GeometryEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			GeometryPanel.Enabled = GeometryEnabled.Checked;
			if (m_isUpdating)
				return;

			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm = TableList.SelectedItems[0].Tag as OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema;
			if (scm == null)
				return;

			if (GeometryEnabled.Checked)
			{
				FeatureSourceEditorODBC.GeometryColumn gc;
				if (GeometryEnabled.Tag != null)
					gc = (FeatureSourceEditorODBC.GeometryColumn)GeometryEnabled.Tag;
				else
				{
					gc = new FeatureSourceEditorODBC.GeometryColumn();
					if (XColumn.Items.Count > 0)
					{
                        if (m_geom.ContainsKey(scm.Name))
                        {
                            FeatureSourceEditorODBC.GeometryColumn gCached = (FeatureSourceEditorODBC.GeometryColumn)m_geom[scm.Name];
                            gc.Name = gCached.Name;
                            gc.XColumn = gCached.XColumn;
                            gc.YColumn = gCached.YColumn;
                            gc.ZColumn = gCached.ZColumn;
                        }
                        else
                        {
                            gc.Name = "Geometry";
                            gc.XColumn = (string)XColumn.Items[0];
                            gc.YColumn = (string)XColumn.Items[Math.Min(1, XColumn.Items.Count - 1)];
                        }
					}
				}
				m_geom[scm.Name] = gc;

				UpdateColumnPanel(gc);
			}
			else
			{
				if (m_geom.ContainsKey(scm.Name))
				{
					GeometryEnabled.Tag = m_geom[scm.Name];
					m_geom.Remove(scm.Name);
				}
			}
			UpdateListDisplay();
		}
	}
}
