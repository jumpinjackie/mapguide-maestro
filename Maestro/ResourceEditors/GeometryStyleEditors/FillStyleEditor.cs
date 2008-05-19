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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// Summary description for FillStyleEditor.
	/// </summary>
	public class FillStyleEditor : System.Windows.Forms.UserControl
	{
		public ResourceEditors.GeometryStyleEditors.ColorComboBox foregroundColor;
		public ResourceEditors.GeometryStyleEditors.ColorComboBox backgroundColor;
		public ResourceEditors.GeometryStyleEditors.ImageStylePicker fillCombo;

		public System.Windows.Forms.CheckBox displayFill;
		private System.Windows.Forms.Label lblBackground;
		private System.Windows.Forms.Label lblForeground;
		private System.Windows.Forms.Label lblFill;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FillStyleEditor()
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FillStyleEditor));
			this.lblBackground = new System.Windows.Forms.Label();
			this.lblForeground = new System.Windows.Forms.Label();
			this.lblFill = new System.Windows.Forms.Label();
			this.foregroundColor = new ResourceEditors.GeometryStyleEditors.ColorComboBox();
			this.backgroundColor = new ResourceEditors.GeometryStyleEditors.ColorComboBox();
			this.fillCombo = new ResourceEditors.GeometryStyleEditors.ImageStylePicker();
			this.displayFill = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// lblBackground
			// 
			this.lblBackground.Location = new System.Drawing.Point(0, 80);
			this.lblBackground.Name = "lblBackground";
			this.lblBackground.Size = new System.Drawing.Size(96, 16);
			this.lblBackground.TabIndex = 8;
			this.lblBackground.Text = "Background color";
			// 
			// lblForeground
			// 
			this.lblForeground.Location = new System.Drawing.Point(0, 48);
			this.lblForeground.Name = "lblForeground";
			this.lblForeground.Size = new System.Drawing.Size(96, 16);
			this.lblForeground.TabIndex = 7;
			this.lblForeground.Text = "Foreground color";
			// 
			// lblFill
			// 
			this.lblFill.Location = new System.Drawing.Point(0, 24);
			this.lblFill.Name = "lblFill";
			this.lblFill.Size = new System.Drawing.Size(96, 16);
			this.lblFill.TabIndex = 6;
			this.lblFill.Text = "Fill pattern";
			// 
			// foregroundColor
			// 
			this.foregroundColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.foregroundColor.Location = new System.Drawing.Point(120, 48);
			this.foregroundColor.Name = "foregroundColor";
			this.foregroundColor.Size = new System.Drawing.Size(184, 21);
			this.foregroundColor.TabIndex = 10;
			// 
			// backgroundColor
			// 
			this.backgroundColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.backgroundColor.Location = new System.Drawing.Point(120, 80);
			this.backgroundColor.Name = "backgroundColor";
			this.backgroundColor.Size = new System.Drawing.Size(184, 21);
			this.backgroundColor.TabIndex = 11;
			// 
			// fillCombo
			// 
			this.fillCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.fillCombo.DisplayMember = "Name";
			this.fillCombo.Location = new System.Drawing.Point(120, 16);
			this.fillCombo.Name = "fillCombo";
			this.fillCombo.Size = new System.Drawing.Size(184, 21);
			this.fillCombo.TabIndex = 12;
			this.fillCombo.TextWidth = 50;
			this.fillCombo.ValueMember = "Name";
			// 
			// displayFill
			// 
			this.displayFill.Checked = true;
			this.displayFill.CheckState = System.Windows.Forms.CheckState.Checked;
			this.displayFill.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.displayFill.Location = new System.Drawing.Point(0, 0);
			this.displayFill.Name = "displayFill";
			this.displayFill.Size = new System.Drawing.Size(168, 16);
			this.displayFill.TabIndex = 13;
			this.displayFill.Text = "Display fill";
			this.displayFill.CheckedChanged += new System.EventHandler(this.displayFill_CheckedChanged);
			// 
			// FillStyleEditor
			// 
			this.Controls.Add(this.displayFill);
			this.Controls.Add(this.fillCombo);
			this.Controls.Add(this.backgroundColor);
			this.Controls.Add(this.foregroundColor);
			this.Controls.Add(this.lblBackground);
			this.Controls.Add(this.lblForeground);
			this.Controls.Add(this.lblFill);
			this.Name = "FillStyleEditor";
			this.Size = new System.Drawing.Size(304, 104);
			this.Load += new System.EventHandler(this.FillStyleEditor_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FillStyleEditor_Load(object sender, System.EventArgs e)
		{
			foregroundColor.AllowTransparent = false;
			backgroundColor.AllowTransparent = true;

			fillCombo.Items.Clear();
			fillCombo.Items.AddRange(FeaturePreviewRender.FillImages);
		}

		private void displayFill_CheckedChanged(object sender, System.EventArgs e)
		{
			lblFill.Enabled = 
			lblForeground.Enabled = 
			lblBackground.Enabled = 
			fillCombo.Enabled =
			foregroundColor.Enabled =
			backgroundColor.Enabled = 
				displayFill.Checked;
		}

	}
}
