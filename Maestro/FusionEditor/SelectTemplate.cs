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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.FusionEditor
{
	/// <summary>
	/// Summary description for SelectTemplate.
	/// </summary>
	public class SelectTemplate : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox TemplateCombo;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private ApplicationDefinitionTemplateInfoTypeCollection m_tp;
		private string m_baseUrl;
		public System.Windows.Forms.TextBox txtUrl;
		private System.Windows.Forms.PictureBox PreviewPicture;
		private Globalizator.Globalizator m_globalizor = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SelectTemplate()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new  Globalizator.Globalizator(this);
		}

		public void SetupCombos(ApplicationDefinitionTemplateInfoSet tp)
		{
			m_tp = tp.TemplateInfo;	
			TemplateCombo.Items.Clear();
			foreach(OSGeo.MapGuide.MaestroAPI.ApplicationDefinitionTemplateInfoType template in m_tp)
				TemplateCombo.Items.Add(template.Name);
			if (TemplateCombo.Items.Count > 0)
				if (TemplateCombo.SelectedIndex == 0)
					TemplateCombo_SelectedIndexChanged(null, null);
				else
					TemplateCombo.SelectedIndex = 0;
		}

		public string BaseURL
		{
			get { return m_baseUrl; }
			set { m_baseUrl = value; }
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
			this.label2 = new System.Windows.Forms.Label();
			this.TemplateCombo = new System.Windows.Forms.ComboBox();
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.PreviewPicture = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Template";
			// 
			// label2
			// 
			this.label2.Enabled = false;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(24, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "URL";
			// 
			// TemplateCombo
			// 
			this.TemplateCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TemplateCombo.Location = new System.Drawing.Point(80, 16);
			this.TemplateCombo.Name = "TemplateCombo";
			this.TemplateCombo.Size = new System.Drawing.Size(352, 21);
			this.TemplateCombo.TabIndex = 2;
			this.TemplateCombo.SelectedIndexChanged += new System.EventHandler(this.TemplateCombo_SelectedIndexChanged);
			// 
			// txtUrl
			// 
			this.txtUrl.Location = new System.Drawing.Point(80, 40);
			this.txtUrl.Name = "txtUrl";
			this.txtUrl.ReadOnly = true;
			this.txtUrl.Size = new System.Drawing.Size(352, 20);
			this.txtUrl.TabIndex = 3;
			this.txtUrl.Text = "";
			// 
			// OKBtn
			// 
			this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKBtn.Enabled = false;
			this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKBtn.Location = new System.Drawing.Point(168, 72);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(80, 24);
			this.OKBtn.TabIndex = 4;
			this.OKBtn.Text = "OK";
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelBtn.Location = new System.Drawing.Point(288, 72);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(80, 24);
			this.CancelBtn.TabIndex = 5;
			this.CancelBtn.Text = "Cancel";
			// 
			// PreviewPicture
			// 
			this.PreviewPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PreviewPicture.Location = new System.Drawing.Point(456, 8);
			this.PreviewPicture.Name = "PreviewPicture";
			this.PreviewPicture.Size = new System.Drawing.Size(128, 88);
			this.PreviewPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PreviewPicture.TabIndex = 6;
			this.PreviewPicture.TabStop = false;
			// 
			// SelectTemplate
			// 
			this.AcceptButton = this.OKBtn;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(594, 109);
			this.Controls.Add(this.PreviewPicture);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.txtUrl);
			this.Controls.Add(this.TemplateCombo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectTemplate";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Template";
			this.ResumeLayout(false);

		}
		#endregion

		private void TemplateCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool useDefaultImage = true;
			OKBtn.Enabled = (TemplateCombo.SelectedIndex >= 0);
			if (TemplateCombo.SelectedIndex >= 0)
			{
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinitionTemplateInfoType adt = m_tp[TemplateCombo.SelectedIndex];
				txtUrl.Text = adt.LocationUrl;
				if (adt.PreviewImageUrl != null && adt.PreviewImageUrl.Trim().Length > 0 && m_baseUrl != null)
				{
					try
					{
						System.Net.WebRequest req = System.Net.WebRequest.Create(new Uri(new Uri(m_baseUrl), adt.PreviewImageUrl));
						PreviewPicture.Image = System.Drawing.Image.FromStream(req.GetResponse().GetResponseStream());
						useDefaultImage = false;
					}
					catch(Exception ex)
					{
						string s = ex.Message;
					}
				}
			}

			if (useDefaultImage)
			{
				try
				{
					PreviewPicture.Image = System.Drawing.Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "NoPreview.png"));
				}
				catch
				{
				}
			}

				
		}
	}
}
