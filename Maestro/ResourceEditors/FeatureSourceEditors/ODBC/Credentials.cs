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
using System.Collections.Specialized;
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC
{
	/// <summary>
	/// Summary description for Credentials.
	/// </summary>
	public class Credentials : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.RadioButton NoCredentials;
		private System.Windows.Forms.RadioButton MapGuideCredentials;
		private System.Windows.Forms.RadioButton SpecifiedCredentials;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Username;
		private System.Windows.Forms.TextBox Password;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ResourceEditors.EditorInterface m_editor = null;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item = null;
		private System.Windows.Forms.Panel CredentialPanel;
		private bool m_isUpdating = false;
		public event FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate ConnectionStringUpdated;
		
		public delegate void CredentialsChangedDelegate(string username, string password);
		public event CredentialsChangedDelegate CredentialsChanged;

		public Credentials()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item)
		{
			m_editor = editor;
			m_item = item;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if (m_item == null)
				return;

			try
			{
				//TODO: Since the credentials control is used elsewhere, it 
				//should not have ODBC specific code
				m_isUpdating = true;
				NameValueCollection nv = ConnectionStringManager.SplitConnectionString(m_item.Parameter["ConnectionString"]);
				if (ConnectionStringManager.GetUserId(nv) == "%MG_LOGIN_USERNAME%")
					MapGuideCredentials.Checked = true;
				else if (ConnectionStringManager.GetUserId(nv) == "%MG_USERNAME%")
					SpecifiedCredentials.Checked = true;
				else
					NoCredentials.Checked = true;
				
				if (m_item.Parameter == null || m_item.Parameter["UserId"] == null)
					Username.Text = "";
				else
					Username.Text = m_item.Parameter["UserId"];

				if (m_item.Parameter == null || m_item.Parameter["Password"] == null)
					Password.Text = "";
				else
					Password.Text = m_item.Parameter["Password"];
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Credentials));
            this.NoCredentials = new System.Windows.Forms.RadioButton();
            this.MapGuideCredentials = new System.Windows.Forms.RadioButton();
            this.SpecifiedCredentials = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CredentialPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.Username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.CredentialPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NoCredentials
            // 
            resources.ApplyResources(this.NoCredentials, "NoCredentials");
            this.NoCredentials.Name = "NoCredentials";
            this.NoCredentials.CheckedChanged += new System.EventHandler(this.Credentials_CheckedChanged);
            // 
            // MapGuideCredentials
            // 
            resources.ApplyResources(this.MapGuideCredentials, "MapGuideCredentials");
            this.MapGuideCredentials.Name = "MapGuideCredentials";
            this.MapGuideCredentials.CheckedChanged += new System.EventHandler(this.Credentials_CheckedChanged);
            // 
            // SpecifiedCredentials
            // 
            resources.ApplyResources(this.SpecifiedCredentials, "SpecifiedCredentials");
            this.SpecifiedCredentials.Name = "SpecifiedCredentials";
            this.SpecifiedCredentials.CheckedChanged += new System.EventHandler(this.Credentials_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SpecifiedCredentials);
            this.groupBox1.Controls.Add(this.MapGuideCredentials);
            this.groupBox1.Controls.Add(this.NoCredentials);
            this.groupBox1.Controls.Add(this.CredentialPanel);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // CredentialPanel
            // 
            resources.ApplyResources(this.CredentialPanel, "CredentialPanel");
            this.CredentialPanel.Controls.Add(this.label2);
            this.CredentialPanel.Controls.Add(this.Username);
            this.CredentialPanel.Controls.Add(this.label1);
            this.CredentialPanel.Controls.Add(this.Password);
            this.CredentialPanel.Name = "CredentialPanel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Username
            // 
            resources.ApplyResources(this.Username, "Username");
            this.Username.Name = "Username";
            this.Username.TextChanged += new System.EventHandler(this.Username_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Password
            // 
            resources.ApplyResources(this.Password, "Password");
            this.Password.Name = "Password";
            this.Password.TextChanged += new System.EventHandler(this.Password_TextChanged);
            // 
            // Credentials
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox1);
            this.Name = "Credentials";
            this.groupBox1.ResumeLayout(false);
            this.CredentialPanel.ResumeLayout(false);
            this.CredentialPanel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public void SetCredentials(string username, string password)
		{
			if (username == "%MG_LOGIN_USERNAME%" && password == "%MG_LOGIN_PASSWORD%")
			{
				MapGuideCredentials.Checked = true;
			}
			else if (username == null || password == null
				|| (username.Trim().Length == 0 && password.Trim().Length == 0))
			{
				NoCredentials.Checked = true;
			}
			else
			{
				SpecifiedCredentials.Checked = true;
				Username.Text = username;
				Password.Text = password;
			}

		}

		private void Credentials_CheckedChanged(object sender, System.EventArgs e)
		{
			CredentialPanel.Enabled = SpecifiedCredentials.Checked;

			if (CredentialsChanged != null)
			{
				if (SpecifiedCredentials.Checked)
					CredentialsChanged(Username.Text, Password.Text);
				else if (NoCredentials.Checked)
					CredentialsChanged("", "");
				else if (MapGuideCredentials.Checked)
					CredentialsChanged("%MG_LOGIN_USERNAME%", "%MG_LOGIN_PASSWORD%");

			}

			if (m_item == null)
				return;

			if (m_isUpdating)
				return;

			NameValueCollection nv = ConnectionStringManager.SplitConnectionString(m_item.Parameter["ConnectionString"]);
			if (SpecifiedCredentials.Checked)
			{
				nv[ConnectionStringManager.GetUidName(nv)] = "%MG_USERNAME%";
				nv[ConnectionStringManager.GetPwdName(nv)] = "%MG_PASSWORD%";
			}
			else if (MapGuideCredentials.Checked)
			{
				nv[ConnectionStringManager.GetUidName(nv)] = "%MG_LOGIN_USERNAME%";
				nv[ConnectionStringManager.GetPwdName(nv)] = "%MG_LOGIN_PASSWORD%";
			}
			else
			{
				nv[ConnectionStringManager.GetUidName(nv)] = "";
				nv[ConnectionStringManager.GetPwdName(nv)] = "";
			}

			if (SpecifiedCredentials.Checked)
			{
				m_item.Parameter["UserId"] = Username.Text;
				m_item.Parameter["Password"] = Password.Text;
			}

			m_item.Parameter["ConnectionString"] = ConnectionStringManager.JoinConnectionString(nv);

			m_editor.HasChanged();

			if (ConnectionStringUpdated != null)
				ConnectionStringUpdated(m_item.Parameter["ConnectionString"]);
		}

		private void Username_TextChanged(object sender, System.EventArgs e)
		{
			if (CredentialsChanged != null)
				CredentialsChanged(Username.Text, Password.Text);

			if (m_item == null || m_isUpdating)
				return;

			m_item.Parameter["UserId"] = Username.Text;
			m_editor.HasChanged();
		}

		private void Password_TextChanged(object sender, System.EventArgs e)
		{
			if (CredentialsChanged != null)
				CredentialsChanged(Username.Text, Password.Text);

			if (m_item == null || m_isUpdating)
				return;

			m_item.Parameter["Password"] = Password.Text;
			m_editor.HasChanged();
		}
	}
}
