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
	/// Summary description for CoordinateSystemOverrideDialog.
	/// </summary>
	public class CoordinateSystemOverrideDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private EditorInterface m_editor;
		private System.Windows.Forms.Button BrowseSource;
		private System.Windows.Forms.Button BrowseTarget;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TextBox SourceCoordsys;
		private System.Windows.Forms.TextBox TargetCoordsys;
		private OSGeo.MapGuide.MaestroAPI.SpatialContextType m_item;

		public CoordinateSystemOverrideDialog(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.SpatialContextType item)
			: this()
		{
			m_editor = editor;
			m_item = item;
		}

		public OSGeo.MapGuide.MaestroAPI.SpatialContextType Item
		{
			get { return m_item; }
		}


		public CoordinateSystemOverrideDialog()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinateSystemOverrideDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SourceCoordsys = new System.Windows.Forms.TextBox();
            this.TargetCoordsys = new System.Windows.Forms.TextBox();
            this.BrowseSource = new System.Windows.Forms.Button();
            this.BrowseTarget = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // SourceCoordsys
            // 
            resources.ApplyResources(this.SourceCoordsys, "SourceCoordsys");
            this.SourceCoordsys.Name = "SourceCoordsys";
            // 
            // TargetCoordsys
            // 
            resources.ApplyResources(this.TargetCoordsys, "TargetCoordsys");
            this.TargetCoordsys.Name = "TargetCoordsys";
            // 
            // BrowseSource
            // 
            resources.ApplyResources(this.BrowseSource, "BrowseSource");
            this.BrowseSource.Name = "BrowseSource";
            this.BrowseSource.Click += new System.EventHandler(this.BrowseSource_Click);
            // 
            // BrowseTarget
            // 
            resources.ApplyResources(this.BrowseTarget, "BrowseTarget");
            this.BrowseTarget.Name = "BrowseTarget";
            this.BrowseTarget.Click += new System.EventHandler(this.BrowseTarget_Click);
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
            // CoordinateSystemOverrideDialog
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.BrowseTarget);
            this.Controls.Add(this.BrowseSource);
            this.Controls.Add(this.TargetCoordsys);
            this.Controls.Add(this.SourceCoordsys);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordinateSystemOverrideDialog";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.CoordinateSystemOverrideDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void BrowseSource_Click(object sender, System.EventArgs e)
		{
			SelectCoordinateSystem dlg = new SelectCoordinateSystem(m_editor.CurrentConnection);
			dlg.SetWKT(SourceCoordsys.Text);
			if (dlg.ShowDialog(this) == DialogResult.OK)
				SourceCoordsys.Text = dlg.SelectedCoordSys.Projection;
		}

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			if (SourceCoordsys.Text.Trim().Length == 0)
			{
				try { SourceCoordsys.Focus(); }
				catch {}
				MessageBox.Show(this, Strings.CoordinateSystemOverrideDialog.CoordinateSystemMissingError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (TargetCoordsys.Text.Trim().Length == 0)
			{
				try { TargetCoordsys.Focus(); }
				catch {}
				MessageBox.Show(this, Strings.CoordinateSystemOverrideDialog.CoordinateSystemMissingError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			m_item.Name = SourceCoordsys.Text;
			m_item.CoordinateSystem = TargetCoordsys.Text;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void BrowseTarget_Click(object sender, System.EventArgs e)
		{
			SelectCoordinateSystem dlg = new SelectCoordinateSystem(m_editor.CurrentConnection);
			dlg.SetWKT(TargetCoordsys.Text);
			if (dlg.ShowDialog(this) == DialogResult.OK)
				TargetCoordsys.Text = dlg.SelectedCoordSys.WKT;
		}

		private void CoordinateSystemOverrideDialog_Load(object sender, System.EventArgs e)
		{
			SourceCoordsys.Text = m_item.Name;
			TargetCoordsys.Text = m_item.CoordinateSystem;
		}

	}
}
