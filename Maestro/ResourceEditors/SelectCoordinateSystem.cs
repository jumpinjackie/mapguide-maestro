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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectCoordinateSystem));
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
            this.CoordSysCodeText = new System.Windows.Forms.ComboBox();
            this.ValidateCoordSysCode = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SelectByEPSGCode = new System.Windows.Forms.RadioButton();
            this.SelectByEPSGCodeGroup = new System.Windows.Forms.GroupBox();
            this.EPSGCodeText = new System.Windows.Forms.ComboBox();
            this.ValidateEPSG = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.CoordinateWait = new System.Windows.Forms.Label();
            this.SelectByListGroup.SuspendLayout();
            this.SelectByWKTGroup.SuspendLayout();
            this.SelectByCoordSysCodeGroup.SuspendLayout();
            this.SelectByEPSGCodeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CoordinateSystemLabel
            // 
            resources.ApplyResources(this.CoordinateSystemLabel, "CoordinateSystemLabel");
            this.CoordinateSystemLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CoordinateSystemLabel.Name = "CoordinateSystemLabel";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // WKTText
            // 
            resources.ApplyResources(this.WKTText, "WKTText");
            this.WKTText.Name = "WKTText";
            this.WKTText.TextChanged += new System.EventHandler(this.WKTText_TextChanged);
            // 
            // SelectByListGroup
            // 
            resources.ApplyResources(this.SelectByListGroup, "SelectByListGroup");
            this.SelectByListGroup.Controls.Add(this.CoordinateSystem);
            this.SelectByListGroup.Controls.Add(this.CoordinateCategory);
            this.SelectByListGroup.Controls.Add(this.CoordinateSystemLabel);
            this.SelectByListGroup.Controls.Add(this.label1);
            this.SelectByListGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByListGroup.Name = "SelectByListGroup";
            this.SelectByListGroup.TabStop = false;
            // 
            // CoordinateSystem
            // 
            resources.ApplyResources(this.CoordinateSystem, "CoordinateSystem");
            this.CoordinateSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CoordinateSystem.Name = "CoordinateSystem";
            this.CoordinateSystem.SelectedIndexChanged += new System.EventHandler(this.CoordinateSystem_SelectedIndexChanged);
            // 
            // CoordinateCategory
            // 
            resources.ApplyResources(this.CoordinateCategory, "CoordinateCategory");
            this.CoordinateCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CoordinateCategory.Name = "CoordinateCategory";
            this.CoordinateCategory.SelectedIndexChanged += new System.EventHandler(this.CoordinateCategory_SelectedIndexChanged);
            // 
            // SelectByList
            // 
            this.SelectByList.Checked = true;
            resources.ApplyResources(this.SelectByList, "SelectByList");
            this.SelectByList.Name = "SelectByList";
            this.SelectByList.TabStop = true;
            this.SelectByList.CheckedChanged += new System.EventHandler(this.SelectByList_CheckedChanged);
            // 
            // SelectByWKTGroup
            // 
            resources.ApplyResources(this.SelectByWKTGroup, "SelectByWKTGroup");
            this.SelectByWKTGroup.Controls.Add(this.ValidateWKT);
            this.SelectByWKTGroup.Controls.Add(this.WKTText);
            this.SelectByWKTGroup.Controls.Add(this.label3);
            this.SelectByWKTGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByWKTGroup.Name = "SelectByWKTGroup";
            this.SelectByWKTGroup.TabStop = false;
            // 
            // ValidateWKT
            // 
            resources.ApplyResources(this.ValidateWKT, "ValidateWKT");
            this.ValidateWKT.Name = "ValidateWKT";
            this.ValidateWKT.Click += new System.EventHandler(this.ValidateWKT_Click);
            // 
            // SelectByWKT
            // 
            resources.ApplyResources(this.SelectByWKT, "SelectByWKT");
            this.SelectByWKT.Name = "SelectByWKT";
            this.SelectByWKT.CheckedChanged += new System.EventHandler(this.SelectByWKT_CheckedChanged);
            // 
            // SelectByCoordSysCode
            // 
            resources.ApplyResources(this.SelectByCoordSysCode, "SelectByCoordSysCode");
            this.SelectByCoordSysCode.Name = "SelectByCoordSysCode";
            this.SelectByCoordSysCode.CheckedChanged += new System.EventHandler(this.SelectByCoordSysCode_CheckedChanged);
            // 
            // SelectByCoordSysCodeGroup
            // 
            resources.ApplyResources(this.SelectByCoordSysCodeGroup, "SelectByCoordSysCodeGroup");
            this.SelectByCoordSysCodeGroup.Controls.Add(this.CoordSysCodeText);
            this.SelectByCoordSysCodeGroup.Controls.Add(this.ValidateCoordSysCode);
            this.SelectByCoordSysCodeGroup.Controls.Add(this.label5);
            this.SelectByCoordSysCodeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByCoordSysCodeGroup.Name = "SelectByCoordSysCodeGroup";
            this.SelectByCoordSysCodeGroup.TabStop = false;
            // 
            // CoordSysCodeText
            // 
            resources.ApplyResources(this.CoordSysCodeText, "CoordSysCodeText");
            this.CoordSysCodeText.Name = "CoordSysCodeText";
            // 
            // ValidateCoordSysCode
            // 
            resources.ApplyResources(this.ValidateCoordSysCode, "ValidateCoordSysCode");
            this.ValidateCoordSysCode.Name = "ValidateCoordSysCode";
            this.ValidateCoordSysCode.Click += new System.EventHandler(this.ValidateCoordSysCode_Click);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // SelectByEPSGCode
            // 
            resources.ApplyResources(this.SelectByEPSGCode, "SelectByEPSGCode");
            this.SelectByEPSGCode.Name = "SelectByEPSGCode";
            this.SelectByEPSGCode.CheckedChanged += new System.EventHandler(this.SelectByEPSGCode_CheckedChanged);
            // 
            // SelectByEPSGCodeGroup
            // 
            resources.ApplyResources(this.SelectByEPSGCodeGroup, "SelectByEPSGCodeGroup");
            this.SelectByEPSGCodeGroup.Controls.Add(this.EPSGCodeText);
            this.SelectByEPSGCodeGroup.Controls.Add(this.ValidateEPSG);
            this.SelectByEPSGCodeGroup.Controls.Add(this.label4);
            this.SelectByEPSGCodeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByEPSGCodeGroup.Name = "SelectByEPSGCodeGroup";
            this.SelectByEPSGCodeGroup.TabStop = false;
            // 
            // EPSGCodeText
            // 
            resources.ApplyResources(this.EPSGCodeText, "EPSGCodeText");
            this.EPSGCodeText.Name = "EPSGCodeText";
            // 
            // ValidateEPSG
            // 
            resources.ApplyResources(this.ValidateEPSG, "ValidateEPSG");
            this.ValidateEPSG.Name = "ValidateEPSG";
            this.ValidateEPSG.Click += new System.EventHandler(this.ValidateEPSG_Click);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            // 
            // CoordinateWait
            // 
            resources.ApplyResources(this.CoordinateWait, "CoordinateWait");
            this.CoordinateWait.Name = "CoordinateWait";
            // 
            // SelectCoordinateSystem
            // 
            this.AcceptButton = this.OKBtn;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.CancelBtn;
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
            this.Load += new System.EventHandler(this.SelectCoordinateSystem_Load);
            this.SelectByListGroup.ResumeLayout(false);
            this.SelectByWKTGroup.ResumeLayout(false);
            this.SelectByWKTGroup.PerformLayout();
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
