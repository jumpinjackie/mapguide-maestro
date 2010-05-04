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
using System.Collections.Generic;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for FormLogin.
	/// </summary>
	public class FormLogin : System.Windows.Forms.Form
	{
		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;
		private bool m_useAutoConnect = false;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbServerUrl;
		private System.Windows.Forms.TextBox txtStartingpoint;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkAutoConnect;
		private System.Windows.Forms.CheckBox chkSavePassword;
		private System.Windows.Forms.ToolTip toolTip;
		private System.ComponentModel.IContainer components;
		private PreferedSiteList m_sitelist = new PreferedSiteList();
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cmbLanguage;
        private List<System.Globalization.CultureInfo> m_supportedLanguages = new List<System.Globalization.CultureInfo>();

		public FormLogin()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            m_supportedLanguages.Add(System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            System.Text.RegularExpressions.Regex cix = new System.Text.RegularExpressions.Regex("[A-z][A-z](\\-[A-z][A-z])?");

            foreach (string f in System.IO.Directory.GetDirectories(Application.StartupPath))
                if (cix.Match(System.IO.Path.GetFileName(f)).Length == System.IO.Path.GetFileName(f).Length)
                    try
                    {
                        m_supportedLanguages.Add(System.Globalization.CultureInfo.GetCultureInfo(System.IO.Path.GetFileName(f)));
                    }
                    catch { }

            cmbLanguage.Items.Clear();
            foreach (System.Globalization.CultureInfo ci in m_supportedLanguages)
                cmbLanguage.Items.Add(ci.DisplayName);

            try
            {
                for (int i = 0; i < m_supportedLanguages.Count; i++)
                    if (string.Compare(m_supportedLanguages[i].Name, System.Threading.Thread.CurrentThread.CurrentUICulture.Name, true) == 0)
                    {
                        cmbLanguage.SelectedIndex = i;
                        break;
                    }
            }
            catch { }


			if (cmbLanguage.SelectedIndex < 0)
				cmbLanguage.SelectedIndex = 0;


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbServerUrl = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStartingpoint = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkAutoConnect = new System.Windows.Forms.CheckBox();
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbServerUrl
            // 
            resources.ApplyResources(this.cmbServerUrl, "cmbServerUrl");
            this.cmbServerUrl.Name = "cmbServerUrl";
            this.toolTip.SetToolTip(this.cmbServerUrl, resources.GetString("cmbServerUrl.ToolTip"));
            this.cmbServerUrl.SelectedIndexChanged += new System.EventHandler(this.cmbServerUrl_SelectedIndexChanged);
            this.cmbServerUrl.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Name = "label2";
            // 
            // txtStartingpoint
            // 
            resources.ApplyResources(this.txtStartingpoint, "txtStartingpoint");
            this.txtStartingpoint.Name = "txtStartingpoint";
            this.toolTip.SetToolTip(this.txtStartingpoint, resources.GetString("txtStartingpoint.ToolTip"));
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtUsername
            // 
            resources.ApplyResources(this.txtUsername, "txtUsername");
            this.txtUsername.Name = "txtUsername";
            this.toolTip.SetToolTip(this.txtUsername, resources.GetString("txtUsername.ToolTip"));
            this.txtUsername.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.toolTip.SetToolTip(this.txtPassword, resources.GetString("txtPassword.ToolTip"));
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkAutoConnect
            // 
            resources.ApplyResources(this.chkAutoConnect, "chkAutoConnect");
            this.chkAutoConnect.Name = "chkAutoConnect";
            this.toolTip.SetToolTip(this.chkAutoConnect, resources.GetString("chkAutoConnect.ToolTip"));
            this.chkAutoConnect.CheckedChanged += new System.EventHandler(this.chkAutoConnect_CheckedChanged);
            // 
            // chkSavePassword
            // 
            resources.ApplyResources(this.chkSavePassword, "chkSavePassword");
            this.chkSavePassword.Name = "chkSavePassword";
            this.toolTip.SetToolTip(this.chkSavePassword, resources.GetString("chkSavePassword.ToolTip"));
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbLanguage, "cmbLanguage");
            this.cmbLanguage.Name = "cmbLanguage";
            this.toolTip.SetToolTip(this.cmbLanguage, resources.GetString("cmbLanguage.ToolTip"));
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // FormLogin
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkSavePassword);
            this.Controls.Add(this.chkAutoConnect);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtStartingpoint);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbServerUrl);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			using(new WaitCursor(this))
			{
				try
				{
					PreferedSite ps = null;
					int index = 0;
					foreach(PreferedSite s in cmbServerUrl.Items)
						if (s.SiteURL == cmbServerUrl.Text)
						{
							ps = s;
							break;
						}
						else
							index++;

					if (ps == null)
						ps = new PreferedSite();

					if (ps.ApprovedVersion == null)
						ps.ApprovedVersion = new Version(0,0,0,0);

#if TEST_NATIVE
					OSGeo.MapGuide.MaestroAPI.ServerConnectionI con;
					if (new Uri(cmbServerUrl.Text).Host.ToLower() == "localhost")
						con = new OSGeo.MapGuide.MaestroAPI.LocalNativeConnection(@"C:\Programmer\MapGuideOpenSource2.0\WebServerExtensions\www\webconfig.ini", txtUsername.Text, txtPassword.Text, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
					else
						con = new OSGeo.MapGuide.MaestroAPI.HttpServerConnection(new Uri(cmbServerUrl.Text), txtUsername.Text, txtPassword.Text, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);
#else
					OSGeo.MapGuide.MaestroAPI.ServerConnectionI con = OSGeo.MapGuide.MaestroAPI.ConnectionFactory.CreateHttpConnection(new Uri(cmbServerUrl.Text), txtUsername.Text, txtPassword.Text, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);
                    ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)con).UserAgent = "MapGuide Maestro v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#endif
					if (con.SiteVersion > con.MaxTestedVersion && con.SiteVersion > ps.ApprovedVersion)
					{
						if (MessageBox.Show(this, Strings.FormLogin.UntestedServerVersion, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
							return;
					}

					m_connection = con;
					m_connection.AutoRestartSession = true;
					
					try
					{
						ps.SiteURL = cmbServerUrl.Text;
						ps.StartingPoint = txtStartingpoint.Text;
						ps.Username = txtUsername.Text;
						ps.SavePassword = chkSavePassword.Checked;
						ps.ApprovedVersion = ps.ApprovedVersion > con.SiteVersion ? ps.ApprovedVersion : con.SiteVersion;
						if (ps.SavePassword)
							ps.UnscrambledPassword = txtPassword.Text;
						else
							ps.ScrambledPassword = "";

						if (index >= m_sitelist.Sites.Length)
							m_sitelist.AddSite(ps);

						m_sitelist.AutoConnect = chkAutoConnect.Checked;
						m_sitelist.PreferedSite = index;
                        m_sitelist.GUILanguage = m_supportedLanguages[cmbLanguage.SelectedIndex].Name;

                        m_sitelist.Save();
					}
					catch (Exception ex)
					{
						string s = ex.Message;
					}
					
					this.DialogResult = DialogResult.OK;
					this.Close();

				}
				catch (Exception ex)
				{
					MessageBox.Show(this, string.Format(Strings.FormLogin.ConnectionFailedError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void FormLogin_Load(object sender, System.EventArgs e)
		{
            m_sitelist = PreferedSiteList.Load();

            if (m_sitelist.Sites.Length == 0)
			{
				cmbServerUrl.Text = "http://localhost/mapguide/mapagent/mapagent.fcgi";
				txtStartingpoint.Text = "Library://";
				txtUsername.Text = "Administrator";
				txtPassword.Text = "admin";
				chkSavePassword.Checked = true;
				chkAutoConnect.Checked = false;
			}
			else
			{
				cmbServerUrl.Items.AddRange(m_sitelist.Sites);
                //In case the site was removed...
                try { cmbServerUrl.SelectedIndex = m_sitelist.PreferedSite; }
                catch { } 
				chkAutoConnect.Checked = m_sitelist.AutoConnect;
			}

            txtPassword_TextChanged(null, null);

			//TODO: Enable the Starting Point, once the functionality is created
			if (m_useAutoConnect && chkAutoConnect.Checked)
			{
				this.Refresh();
				btnOK.PerformClick();
			}
		}

		private void chkAutoConnect_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void cmbServerUrl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PreferedSite site = cmbServerUrl.SelectedItem as PreferedSite;
			if (site == null)
				return;

			txtStartingpoint.Text = site.StartingPoint;
			txtUsername.Text = site.Username;
			if (site.SavePassword)
				txtPassword.Text = site.UnscrambledPassword;
			else
				txtPassword.Text = "";
			chkSavePassword.Checked = site.SavePassword;
		}

		private void cmbLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (cmbLanguage.SelectedIndex >= 0 && cmbLanguage.SelectedIndex < m_supportedLanguages.Count)
            {
                //System.Threading.Thread.CurrentThread.CurrentCulture = m_supportedLanguages[cmbLanguage.SelectedIndex];
                //System.Threading.Thread.CurrentThread.CurrentUICulture = m_supportedLanguages[cmbLanguage.SelectedIndex];

                if (this.Visible)
                {
                    try
                    {
                        m_sitelist.GUILanguage = m_supportedLanguages[cmbLanguage.SelectedIndex].Name;
                        m_sitelist.Save();
                    }
                    catch { }

                    MessageBox.Show(this, Strings.FormLogin.RestartForLanguageChange, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
		}

		public OSGeo.MapGuide.MaestroAPI.ServerConnectionI Connection
		{
			get { return m_connection; }
		}

		public bool UseAutoConnect
		{
			get { return m_useAutoConnect; }
			set { m_useAutoConnect = value; }
		}

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbServerUrl.Text.Length > 0 && txtUsername.Text.Length > 0;
        }

        public string Username { get { return txtUsername.Text; } }

        public string Password { get { return txtPassword.Text; } }
	}
}
