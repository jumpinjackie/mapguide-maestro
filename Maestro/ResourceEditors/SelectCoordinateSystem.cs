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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for SelectCoordinateSystem.
	/// </summary>
	public class SelectCoordinateSystem : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton SelectByList;
		private System.Windows.Forms.RadioButton SelectByWKT;
		private System.Windows.Forms.RadioButton SelectByCoordSysCode;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton SelectByEPSGCode;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button ValidateEPSG;
		private System.Windows.Forms.Button ValidateCoordSysCode;
		private System.Windows.Forms.Button ValidateWKT;
		private System.Windows.Forms.GroupBox SelectByListGroup;
		private System.Windows.Forms.GroupBox SelectByWKTGroup;
		private System.Windows.Forms.GroupBox SelectByCoordSysCodeGroup;
		private System.Windows.Forms.GroupBox SelectByEPSGCodeGroup;
		private System.Windows.Forms.Label CoordinateSystemLabel;
		private System.Windows.Forms.ComboBox CoordinateCategory;
		private System.Windows.Forms.ComboBox CoordinateSystem;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TextBox WKTText;

		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;

		private OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys m_wktCoordSys = null;
		private OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys m_epsgCoordSys = null;
		private OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys m_coordsysCodeCoordSys = null;
		private OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys m_selectedCoordsys = null;

		private Globalizator.Globalizator m_globalizor = null;
		private System.Windows.Forms.Label CoordinateWait;
		private System.Windows.Forms.ComboBox EPSGCodeText;
		private System.Windows.Forms.ComboBox CoordSysCodeText;
		private bool m_isUpdating = false;

		public void SetWKT(string wkt)
		{
			//Unfortunately WKT to Coordsyscode is a bit flacky, so it is disabled
			//WKTText.Text = wkt;
		}

		public SelectCoordinateSystem(OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection)
			: this()
		{
			m_connection = connection;
			if (m_connection.CoordinateSystem == null)
			{
				SelectByList.Enabled = 
					SelectByCoordSysCode.Enabled =
					SelectByEPSGCode.Enabled = 
					ValidateWKT.Enabled =
					false;

				SelectByWKT.Enabled = SelectByWKT.Checked = true;
				
			}
			else
			{
				CoordinateCategory.Items.Clear();
				CoordinateCategory.Items.AddRange(m_connection.CoordinateSystem.Categories);
			}
		}

		private SelectCoordinateSystem()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new  Globalizator.Globalizator(this);
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
			this.label1 = new System.Windows.Forms.Label();
			this.CoordinateSystemLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.WKTText = new System.Windows.Forms.TextBox();
			this.SelectByListGroup = new System.Windows.Forms.GroupBox();
			this.CoordinateSystem = new System.Windows.Forms.ComboBox();
			this.CoordinateCategory = new System.Windows.Forms.ComboBox();
			this.SelectByList = new System.Windows.Forms.RadioButton();
			this.SelectByWKTGroup = new System.Windows.Forms.GroupBox();
			this.ValidateWKT = new System.Windows.Forms.Button();
			this.SelectByWKT = new System.Windows.Forms.RadioButton();
			this.SelectByCoordSysCode = new System.Windows.Forms.RadioButton();
			this.SelectByCoordSysCodeGroup = new System.Windows.Forms.GroupBox();
			this.ValidateCoordSysCode = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.SelectByEPSGCode = new System.Windows.Forms.RadioButton();
			this.SelectByEPSGCodeGroup = new System.Windows.Forms.GroupBox();
			this.ValidateEPSG = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.CoordinateWait = new System.Windows.Forms.Label();
			this.EPSGCodeText = new System.Windows.Forms.ComboBox();
			this.CoordSysCodeText = new System.Windows.Forms.ComboBox();
			this.SelectByListGroup.SuspendLayout();
			this.SelectByWKTGroup.SuspendLayout();
			this.SelectByCoordSysCodeGroup.SuspendLayout();
			this.SelectByEPSGCodeGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Coordinate Category";
			// 
			// CoordinateSystemLabel
			// 
			this.CoordinateSystemLabel.Enabled = false;
			this.CoordinateSystemLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CoordinateSystemLabel.Location = new System.Drawing.Point(8, 48);
			this.CoordinateSystemLabel.Name = "CoordinateSystemLabel";
			this.CoordinateSystemLabel.Size = new System.Drawing.Size(136, 16);
			this.CoordinateSystemLabel.TabIndex = 1;
			this.CoordinateSystemLabel.Text = "Coordinate System";
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(8, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Well-Known-Text (WKT)";
			// 
			// WKTText
			// 
			this.WKTText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.WKTText.Location = new System.Drawing.Point(160, 24);
			this.WKTText.Name = "WKTText";
			this.WKTText.Size = new System.Drawing.Size(280, 20);
			this.WKTText.TabIndex = 5;
			this.WKTText.Text = "";
			this.WKTText.TextChanged += new System.EventHandler(this.WKTText_TextChanged);
			// 
			// SelectByListGroup
			// 
			this.SelectByListGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SelectByListGroup.Controls.Add(this.CoordinateSystem);
			this.SelectByListGroup.Controls.Add(this.CoordinateCategory);
			this.SelectByListGroup.Controls.Add(this.CoordinateSystemLabel);
			this.SelectByListGroup.Controls.Add(this.label1);
			this.SelectByListGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByListGroup.Location = new System.Drawing.Point(8, 8);
			this.SelectByListGroup.Name = "SelectByListGroup";
			this.SelectByListGroup.Size = new System.Drawing.Size(512, 80);
			this.SelectByListGroup.TabIndex = 6;
			this.SelectByListGroup.TabStop = false;
			// 
			// CoordinateSystem
			// 
			this.CoordinateSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.CoordinateSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CoordinateSystem.Enabled = false;
			this.CoordinateSystem.Location = new System.Drawing.Point(160, 48);
			this.CoordinateSystem.Name = "CoordinateSystem";
			this.CoordinateSystem.Size = new System.Drawing.Size(344, 21);
			this.CoordinateSystem.TabIndex = 3;
			this.CoordinateSystem.SelectedIndexChanged += new System.EventHandler(this.CoordinateSystem_SelectedIndexChanged);
			// 
			// CoordinateCategory
			// 
			this.CoordinateCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.CoordinateCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CoordinateCategory.Location = new System.Drawing.Point(160, 24);
			this.CoordinateCategory.Name = "CoordinateCategory";
			this.CoordinateCategory.Size = new System.Drawing.Size(344, 21);
			this.CoordinateCategory.TabIndex = 2;
			this.CoordinateCategory.SelectedIndexChanged += new System.EventHandler(this.CoordinateCategory_SelectedIndexChanged);
			// 
			// SelectByList
			// 
			this.SelectByList.Checked = true;
			this.SelectByList.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByList.Location = new System.Drawing.Point(16, 8);
			this.SelectByList.Name = "SelectByList";
			this.SelectByList.Size = new System.Drawing.Size(96, 16);
			this.SelectByList.TabIndex = 4;
			this.SelectByList.TabStop = true;
			this.SelectByList.Text = "Select by list";
			this.SelectByList.CheckedChanged += new System.EventHandler(this.SelectByList_CheckedChanged);
			// 
			// SelectByWKTGroup
			// 
			this.SelectByWKTGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SelectByWKTGroup.Controls.Add(this.ValidateWKT);
			this.SelectByWKTGroup.Controls.Add(this.WKTText);
			this.SelectByWKTGroup.Controls.Add(this.label3);
			this.SelectByWKTGroup.Enabled = false;
			this.SelectByWKTGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByWKTGroup.Location = new System.Drawing.Point(8, 96);
			this.SelectByWKTGroup.Name = "SelectByWKTGroup";
			this.SelectByWKTGroup.Size = new System.Drawing.Size(512, 56);
			this.SelectByWKTGroup.TabIndex = 7;
			this.SelectByWKTGroup.TabStop = false;
			// 
			// ValidateWKT
			// 
			this.ValidateWKT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ValidateWKT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ValidateWKT.Location = new System.Drawing.Point(448, 24);
			this.ValidateWKT.Name = "ValidateWKT";
			this.ValidateWKT.Size = new System.Drawing.Size(56, 24);
			this.ValidateWKT.TabIndex = 6;
			this.ValidateWKT.Text = "Validate";
			this.ValidateWKT.Click += new System.EventHandler(this.ValidateWKT_Click);
			// 
			// SelectByWKT
			// 
			this.SelectByWKT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByWKT.Location = new System.Drawing.Point(16, 96);
			this.SelectByWKT.Name = "SelectByWKT";
			this.SelectByWKT.Size = new System.Drawing.Size(112, 16);
			this.SelectByWKT.TabIndex = 8;
			this.SelectByWKT.Text = "Type WKT code";
			this.SelectByWKT.CheckedChanged += new System.EventHandler(this.SelectByWKT_CheckedChanged);
			// 
			// SelectByCoordSysCode
			// 
			this.SelectByCoordSysCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByCoordSysCode.Location = new System.Drawing.Point(16, 160);
			this.SelectByCoordSysCode.Name = "SelectByCoordSysCode";
			this.SelectByCoordSysCode.Size = new System.Drawing.Size(184, 16);
			this.SelectByCoordSysCode.TabIndex = 10;
			this.SelectByCoordSysCode.Text = "Type coordinate system code";
			this.SelectByCoordSysCode.CheckedChanged += new System.EventHandler(this.SelectByCoordSysCode_CheckedChanged);
			// 
			// SelectByCoordSysCodeGroup
			// 
			this.SelectByCoordSysCodeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SelectByCoordSysCodeGroup.Controls.Add(this.CoordSysCodeText);
			this.SelectByCoordSysCodeGroup.Controls.Add(this.ValidateCoordSysCode);
			this.SelectByCoordSysCodeGroup.Controls.Add(this.label5);
			this.SelectByCoordSysCodeGroup.Enabled = false;
			this.SelectByCoordSysCodeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByCoordSysCodeGroup.Location = new System.Drawing.Point(8, 160);
			this.SelectByCoordSysCodeGroup.Name = "SelectByCoordSysCodeGroup";
			this.SelectByCoordSysCodeGroup.Size = new System.Drawing.Size(512, 56);
			this.SelectByCoordSysCodeGroup.TabIndex = 9;
			this.SelectByCoordSysCodeGroup.TabStop = false;
			// 
			// ValidateCoordSysCode
			// 
			this.ValidateCoordSysCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ValidateCoordSysCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ValidateCoordSysCode.Location = new System.Drawing.Point(448, 24);
			this.ValidateCoordSysCode.Name = "ValidateCoordSysCode";
			this.ValidateCoordSysCode.Size = new System.Drawing.Size(56, 24);
			this.ValidateCoordSysCode.TabIndex = 6;
			this.ValidateCoordSysCode.Text = "Validate";
			this.ValidateCoordSysCode.Click += new System.EventHandler(this.ValidateCoordSysCode_Click);
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(8, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(144, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Coordinate system code";
			// 
			// SelectByEPSGCode
			// 
			this.SelectByEPSGCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByEPSGCode.Location = new System.Drawing.Point(16, 224);
			this.SelectByEPSGCode.Name = "SelectByEPSGCode";
			this.SelectByEPSGCode.Size = new System.Drawing.Size(184, 16);
			this.SelectByEPSGCode.TabIndex = 12;
			this.SelectByEPSGCode.Text = "Type EPSG code";
			this.SelectByEPSGCode.CheckedChanged += new System.EventHandler(this.SelectByEPSGCode_CheckedChanged);
			// 
			// SelectByEPSGCodeGroup
			// 
			this.SelectByEPSGCodeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SelectByEPSGCodeGroup.Controls.Add(this.EPSGCodeText);
			this.SelectByEPSGCodeGroup.Controls.Add(this.ValidateEPSG);
			this.SelectByEPSGCodeGroup.Controls.Add(this.label4);
			this.SelectByEPSGCodeGroup.Enabled = false;
			this.SelectByEPSGCodeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectByEPSGCodeGroup.Location = new System.Drawing.Point(8, 224);
			this.SelectByEPSGCodeGroup.Name = "SelectByEPSGCodeGroup";
			this.SelectByEPSGCodeGroup.Size = new System.Drawing.Size(512, 56);
			this.SelectByEPSGCodeGroup.TabIndex = 11;
			this.SelectByEPSGCodeGroup.TabStop = false;
			// 
			// ValidateEPSG
			// 
			this.ValidateEPSG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ValidateEPSG.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ValidateEPSG.Location = new System.Drawing.Point(448, 24);
			this.ValidateEPSG.Name = "ValidateEPSG";
			this.ValidateEPSG.Size = new System.Drawing.Size(56, 24);
			this.ValidateEPSG.TabIndex = 5;
			this.ValidateEPSG.Text = "Validate";
			this.ValidateEPSG.Click += new System.EventHandler(this.ValidateEPSG_Click);
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(8, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(144, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Coordinate system code";
			// 
			// OKBtn
			// 
			this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKBtn.Enabled = false;
			this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKBtn.Location = new System.Drawing.Point(184, 288);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(80, 32);
			this.OKBtn.TabIndex = 13;
			this.OKBtn.Text = "OK";
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelBtn.Location = new System.Drawing.Point(280, 288);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(80, 32);
			this.CancelBtn.TabIndex = 14;
			this.CancelBtn.Text = "Cancel";
			// 
			// CoordinateWait
			// 
			this.CoordinateWait.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.CoordinateWait.Location = new System.Drawing.Point(8, 8);
			this.CoordinateWait.Name = "CoordinateWait";
			this.CoordinateWait.Size = new System.Drawing.Size(512, 272);
			this.CoordinateWait.TabIndex = 15;
			this.CoordinateWait.Text = "Loading coordinate system, please wait";
			this.CoordinateWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// EPSGCodeText
			// 
			this.EPSGCodeText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.EPSGCodeText.Location = new System.Drawing.Point(168, 24);
			this.EPSGCodeText.Name = "EPSGCodeText";
			this.EPSGCodeText.Size = new System.Drawing.Size(272, 21);
			this.EPSGCodeText.TabIndex = 6;
			// 
			// CoordSysCodeText
			// 
			this.CoordSysCodeText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.CoordSysCodeText.Location = new System.Drawing.Point(168, 24);
			this.CoordSysCodeText.Name = "CoordSysCodeText";
			this.CoordSysCodeText.Size = new System.Drawing.Size(272, 21);
			this.CoordSysCodeText.TabIndex = 7;
			// 
			// SelectCoordinateSystem
			// 
			this.AcceptButton = this.OKBtn;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(528, 333);
			this.Controls.Add(this.SelectByList);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.SelectByEPSGCode);
			this.Controls.Add(this.SelectByEPSGCodeGroup);
			this.Controls.Add(this.SelectByCoordSysCode);
			this.Controls.Add(this.SelectByCoordSysCodeGroup);
			this.Controls.Add(this.SelectByWKT);
			this.Controls.Add(this.SelectByWKTGroup);
			this.Controls.Add(this.SelectByListGroup);
			this.Controls.Add(this.CoordinateWait);
			this.Name = "SelectCoordinateSystem";
			this.Text = "SelectCoordinateSystem";
			this.Load += new System.EventHandler(this.SelectCoordinateSystem_Load);
			this.SelectByListGroup.ResumeLayout(false);
			this.SelectByWKTGroup.ResumeLayout(false);
			this.SelectByCoordSysCodeGroup.ResumeLayout(false);
			this.SelectByEPSGCodeGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void SelectByList_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateAfterRadioButtons();
		}

		private void SelectCoordinateSystem_Load(object sender, System.EventArgs e)
		{
			this.Visible = true;
			CoordinateWait.Visible = true;
			CoordinateWait.BringToFront();
			this.Refresh();
			m_connection.CoordinateSystem.FindCoordSys("");

			OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys[] items = null;
			try
			{
				items = m_connection.CoordinateSystem.Coordsys;
			}
			catch
			{
			}

			EPSGCodeText.BeginUpdate();
			try
			{
				EPSGCodeText.Items.Clear();
				if (items != null)
					foreach(OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys c in items)
						if (c.Code.StartsWith("EPSG:"))
							EPSGCodeText.Items.Add(c.EPSG);
			}
			finally
			{
				EPSGCodeText.EndUpdate();
			}

			CoordSysCodeText.BeginUpdate();
			try
			{
				CoordSysCodeText.Items.Clear();
				if (items != null)
					foreach(OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys c in items)
						CoordSysCodeText.Items.Add(c.Code);
			}
			finally
			{
				CoordSysCodeText.EndUpdate();
			}

			if (WKTText.Text != "")
			{
				SelectByWKT.Checked = true;
				ValidateWKT_Click(null, null);
			}

			CoordinateWait.Visible = false;
		}

		private void UpdateAfterRadioButtons()
		{
			SelectByListGroup.Enabled = SelectByList.Checked;
			SelectByWKTGroup.Enabled = SelectByWKT.Checked;
			SelectByCoordSysCodeGroup.Enabled = SelectByCoordSysCode.Checked;
			SelectByEPSGCodeGroup.Enabled = SelectByEPSGCode.Checked;
			
			UpdateOKButton();

		}

		private void UpdateOKButton()
		{
			UpdateOthers();
			if (SelectByList.Checked)
			{
				if (CoordinateCategory.SelectedIndex >= 0 && CoordinateSystem.SelectedIndex >= 0)
					OKBtn.Enabled = true;
			}
			else if (m_connection.CoordinateSystem == null)
				OKBtn.Enabled = true;
			else if (SelectByWKT.Checked)
				OKBtn.Enabled = m_wktCoordSys != null;
			else if (SelectByCoordSysCode.Checked)
				OKBtn.Enabled = m_coordsysCodeCoordSys != null;
			else if (SelectByEPSGCode.Checked)
				OKBtn.Enabled = m_epsgCoordSys != null;
			else
				OKBtn.Enabled = false;
		}

		private void SelectByWKT_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateAfterRadioButtons();
		}

		private void SelectByCoordSysCode_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateAfterRadioButtons();
		}

		private void SelectByEPSGCode_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateAfterRadioButtons();
		}

		private void CoordinateCategory_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			CoordinateSystem.Enabled = CoordinateSystemLabel.Enabled = CoordinateCategory.SelectedIndex >= 0;
			if (CoordinateCategory.SelectedIndex >= 0)
			{
				OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.Category cat = CoordinateCategory.SelectedItem as OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.Category;
				if (cat == null)
				{
					OKBtn.Enabled = false;
					return;
				}

				CoordinateSystem.Items.Clear();
				CoordinateSystem.Items.AddRange(cat.Items);
			}
		}

		private void UpdateOthers()
		{
			OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys selectedCoordsys;
			if (SelectByList.Checked)
				selectedCoordsys = CoordinateSystem.SelectedItem as OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys;
			else if (SelectByCoordSysCode.Checked)
				selectedCoordsys = m_coordsysCodeCoordSys;
			else if (SelectByWKT.Checked)
				selectedCoordsys = m_wktCoordSys;
			else if (SelectByEPSGCode.Checked)
				selectedCoordsys = m_epsgCoordSys;
			else
				selectedCoordsys = null;

			try
			{
				m_isUpdating = true;
				if (!SelectByList.Checked)
					try { CoordinateSystem.SelectedItem = selectedCoordsys; }
					catch {}
				if (!SelectByCoordSysCode.Checked)
					try { CoordSysCodeText.Text = selectedCoordsys == null ? "" : selectedCoordsys.Code; }
					catch {}
				if (!SelectByWKT.Checked)
					try { WKTText.Text = selectedCoordsys == null ? "" : selectedCoordsys.WKT; }
					catch {}
				if (!SelectByEPSGCode.Checked)
					try { EPSGCodeText.Text = selectedCoordsys.EPSG; }
					catch {}
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void CoordinateSystem_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			UpdateOKButton();
			
		}

		private void WKTText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

//			if (!WKTText.Focused)
//				return;
			m_wktCoordSys = null;
			if (WKTText.Focused)
				UpdateOKButton();
		}

		private void CoordSysCodeText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

//			if (!CoordSysCodeText.Focused)
//				return;
			m_coordsysCodeCoordSys = null;
			if (CoordSysCodeText.Focused)
				UpdateOKButton();
		}

		private void EPSGCodeText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

//			if (!EPSGCodeText.Focused)
//				return;
			m_epsgCoordSys = null;
			if (EPSGCodeText.Focused)
				UpdateOKButton();
		}

		private void ValidateWKT_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_wktCoordSys = null;
				if (m_connection.CoordinateSystem.IsValid(WKTText.Text))
				{
					try
					{
						string coordcode = m_connection.CoordinateSystem.ConvertWktToCoordinateSystemCode(WKTText.Text);
						m_wktCoordSys = m_connection.CoordinateSystem.FindCoordSys(coordcode);
					}
					catch
					{
					}

					if (m_wktCoordSys == null)
					{
						m_wktCoordSys = new OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys();
						m_wktCoordSys.Code = null;
						m_wktCoordSys.Description = null;
						m_wktCoordSys.WKT = WKTText.Text;
					}
				}
			}
			catch
			{
			}
			UpdateOKButton();
		}

		private void ValidateCoordSysCode_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_coordsysCodeCoordSys = null;
				string s = m_connection.CoordinateSystem.ConvertCoordinateSystemCodeToWkt(CoordSysCodeText.Text);
				m_coordsysCodeCoordSys = m_connection.CoordinateSystem.FindCoordSys(CoordSysCodeText.Text);
			}
			catch
			{
			}
			UpdateOKButton();
		}

		private void ValidateEPSG_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_epsgCoordSys = null;
				m_epsgCoordSys = m_connection.CoordinateSystem.FindCoordSys("EPSG:" + EPSGCodeText.Text);
				if (m_epsgCoordSys == null)
				{
					string s = m_connection.CoordinateSystem.ConvertEpsgCodeToWkt(EPSGCodeText.Text);
					s = m_connection.CoordinateSystem.ConvertWktToCoordinateSystemCode(s);
					m_epsgCoordSys = m_connection.CoordinateSystem.FindCoordSys(s);
				}
			}
			catch
			{
			}
			UpdateOKButton();
		}

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			if (SelectByList.Checked)
				m_selectedCoordsys = CoordinateSystem.SelectedItem as OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys;
			else if (SelectByCoordSysCode.Checked)
				m_selectedCoordsys = m_coordsysCodeCoordSys;
			else if (SelectByWKT.Checked && m_connection.CoordinateSystem == null)
			{
				m_selectedCoordsys = new OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys();
				m_selectedCoordsys.Code = null;
				m_selectedCoordsys.Description = null;
				m_selectedCoordsys.WKT = WKTText.Text;
			}
			else if (SelectByWKT.Checked)
				m_selectedCoordsys = m_wktCoordSys;
			else if (SelectByEPSGCode.Checked)
				m_selectedCoordsys = m_epsgCoordSys;
			else
				m_selectedCoordsys = null;
		}

		public OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys SelectedCoordSys
		{
			get { return m_selectedCoordsys; }
		}
	}
}
