#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
using System.Collections.Specialized;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Gdal
{
	/// <summary>
	/// Summary description for Simple.
	/// </summary>
	public class Simple : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private System.Windows.Forms.Button BrowseFileButton;
        public TextBox Filepath;
		private System.Windows.Forms.Label label1;
		private bool m_isUpdating = false;
		private Globalizator.Globalizator m_globalizor;
		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;

		public Simple()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature,  Globalizator.Globalizator globalizor)
		{
			m_editor = editor;
			m_feature = feature;
			m_globalizor = globalizor;

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_feature == null || m_feature.Parameter == null)
					return;

				Filepath.Text = m_feature.Parameter["DefaultRasterFileLocation"];			}
			finally
			{
				m_isUpdating = false;
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
            this.BrowseFileButton = new System.Windows.Forms.Button();
            this.Filepath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BrowseFileButton
            // 
            this.BrowseFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseFileButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseFileButton.Location = new System.Drawing.Point(184, 8);
            this.BrowseFileButton.Name = "BrowseFileButton";
            this.BrowseFileButton.Size = new System.Drawing.Size(24, 20);
            this.BrowseFileButton.TabIndex = 42;
            this.BrowseFileButton.Text = "...";
            this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // Filepath
            // 
            this.Filepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Filepath.Location = new System.Drawing.Point(112, 8);
            this.Filepath.Name = "Filepath";
            this.Filepath.Size = new System.Drawing.Size(72, 20);
            this.Filepath.TabIndex = 41;
            this.Filepath.TextChanged += new System.EventHandler(this.Filepath_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 40;
            this.label1.Text = "Path to file(s)";
            // 
            // Simple
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(216, 40);
            this.Controls.Add(this.BrowseFileButton);
            this.Controls.Add(this.Filepath);
            this.Controls.Add(this.label1);
            this.Name = "Simple";
            this.Size = new System.Drawing.Size(216, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Filepath_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			if (m_feature.ConfigurationDocument != null && m_feature.ConfigurationDocument.Trim().Length != 0)
				if (MessageBox.Show(this, m_globalizor.Translate("This will remove the configuration document and any specialized setup.\nAre you sure you want to continue?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
					return;
				else
					m_feature.ConfigurationDocument = null;
 
			m_feature.Parameter["DefaultRasterFileLocation"] = Filepath.Text;
			m_editor.HasChanged();
		}

		private void BrowseFileButton_Click(object sender, System.EventArgs e)
		{
			NameValueCollection nv = new NameValueCollection();
			nv.Add("", "All files");
			string s = m_editor.BrowseUnmanagedData("", nv);
			if (s != null)
				Filepath.Text = s;
		}
	}
}
