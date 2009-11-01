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
		private Globalizator.Globalizator m_globalizor = null;
		private ArrayList m_languages = new ArrayList();

		public FormLogin()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_globalizor = new Globalizator.Globalizator(this);

			this.toolTip.SetToolTip(this.cmbServerUrl, m_globalizor.Translate("Enter the full URL of the MapAgent service here"));
			this.toolTip.SetToolTip(this.txtStartingpoint, m_globalizor.Translate("Enter the initial directory here"));
			this.toolTip.SetToolTip(this.txtUsername, m_globalizor.Translate("Enter the username used for login here"));
			this.toolTip.SetToolTip(this.txtPassword, m_globalizor.Translate("Enter the site password here"));
			this.toolTip.SetToolTip(this.chkAutoConnect, m_globalizor.Translate("Check this box to have Maestro log in to this site on startup automatically."));
			this.toolTip.SetToolTip(this.chkSavePassword, m_globalizor.Translate("If checked, a scrambled version of the password is stored on the computer. Please note that the password is NOT encrypted and may be read by a third party. Storing the password on the computer is considered unsafe, but increases convinience."));
			this.toolTip.SetToolTip(this.cmbLanguage, m_globalizor.Translate("Select your prefered language here"));

			cmbLanguage.Items.Clear();
			m_languages.Add(new DictionaryEntry("English (default)", System.Globalization.CultureInfo.InvariantCulture));
			foreach(System.Globalization.CultureInfo ci in Globalizator.Globalizator.AvalibleCultures)
				if (ci != System.Globalization.CultureInfo.InvariantCulture)
					m_languages.Add(new DictionaryEntry(ci.DisplayName, ci));

			foreach(DictionaryEntry de in m_languages)
			{
				cmbLanguage.Items.Add(de.Key);
				if (((System.Globalization.CultureInfo)de.Value).Name == Globalizator.Globalizator.CurrentCulture.Name)
					cmbLanguage.SelectedIndex = cmbLanguage.Items.Count - 1;
			}

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
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // cmbServerUrl
            // 
            this.cmbServerUrl.Location = new System.Drawing.Point(96, 16);
            this.cmbServerUrl.Name = "cmbServerUrl";
            this.cmbServerUrl.Size = new System.Drawing.Size(288, 21);
            this.cmbServerUrl.TabIndex = 1;
            this.cmbServerUrl.Text = "http://localhost/mapguide/mapagent/mapagent.fcgi";
            this.toolTip.SetToolTip(this.cmbServerUrl, "Enter the full URL of the MapAgent service here");
            this.cmbServerUrl.SelectedIndexChanged += new System.EventHandler(this.cmbServerUrl_SelectedIndexChanged);
            this.cmbServerUrl.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // label2
            // 
            this.label2.Enabled = false;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Startingpoint";
            // 
            // txtStartingpoint
            // 
            this.txtStartingpoint.Enabled = false;
            this.txtStartingpoint.Location = new System.Drawing.Point(96, 48);
            this.txtStartingpoint.Name = "txtStartingpoint";
            this.txtStartingpoint.Size = new System.Drawing.Size(288, 20);
            this.txtStartingpoint.TabIndex = 3;
            this.txtStartingpoint.Text = "Library://";
            this.toolTip.SetToolTip(this.txtStartingpoint, "Enter the initial directory here");
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(16, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(16, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(96, 80);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(288, 20);
            this.txtUsername.TabIndex = 6;
            this.txtUsername.Text = "Administrator";
            this.toolTip.SetToolTip(this.txtUsername, "Enter the username used for login here");
            this.txtUsername.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(96, 112);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(288, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.Text = "admin";
            this.toolTip.SetToolTip(this.txtPassword, "Enter the site password here");
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(96, 232);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 32);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(208, 232);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 32);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkAutoConnect
            // 
            this.chkAutoConnect.Enabled = false;
            this.chkAutoConnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAutoConnect.Location = new System.Drawing.Point(96, 200);
            this.chkAutoConnect.Name = "chkAutoConnect";
            this.chkAutoConnect.Size = new System.Drawing.Size(288, 16);
            this.chkAutoConnect.TabIndex = 10;
            this.chkAutoConnect.Text = "Log in automatically";
            this.toolTip.SetToolTip(this.chkAutoConnect, "Check this box to have Maestro log in to this site on startup automatically.");
            this.chkAutoConnect.CheckedChanged += new System.EventHandler(this.chkAutoConnect_CheckedChanged);
            // 
            // chkSavePassword
            // 
            this.chkSavePassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkSavePassword.Location = new System.Drawing.Point(96, 176);
            this.chkSavePassword.Name = "chkSavePassword";
            this.chkSavePassword.Size = new System.Drawing.Size(288, 16);
            this.chkSavePassword.TabIndex = 11;
            this.chkSavePassword.Text = "Save password on computer";
            this.toolTip.SetToolTip(this.chkSavePassword, resources.GetString("chkSavePassword.ToolTip"));
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.Location = new System.Drawing.Point(96, 144);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(288, 21);
            this.cmbLanguage.TabIndex = 13;
            this.toolTip.SetToolTip(this.cmbLanguage, "Select your prefered language here");
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Location = new System.Drawing.Point(16, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Language";
            // 
            // FormLogin
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(408, 287);
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
            this.Text = "Log on to a MapGuide Server";
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
					OSGeo.MapGuide.MaestroAPI.ServerConnectionI con = new OSGeo.MapGuide.MaestroAPI.HttpServerConnection(new Uri(cmbServerUrl.Text), txtUsername.Text, txtPassword.Text, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);
                    ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)con).UserAgent = "MapGuide Maestro v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#endif
					if (con.SiteVersion > con.MaxTestedVersion && con.SiteVersion > ps.ApprovedVersion)
					{
						if (MessageBox.Show(this, m_globalizor.Translate("The current site version is newer than the version Maestro was tested against.\nDo you want to connect anyway?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
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
                        m_sitelist.GUILanguage = ((System.Globalization.CultureInfo)(((DictionaryEntry)m_languages[cmbLanguage.SelectedIndex]).Value)).Name;

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
					MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to log on because: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                try
                {
                    if (!string.IsNullOrEmpty(m_sitelist.GUILanguage))
                        for(int i = 0; i < m_languages.Count; i++)
                            if (string.Compare(((System.Globalization.CultureInfo)(((DictionaryEntry)m_languages[i]).Value)).Name, m_sitelist.GUILanguage, true) == 0)
                            {
                                cmbLanguage.SelectedIndex = i;
                                break;
                            }
                }
                catch {}
			}

            //Fix, enable OK button in case the password is "admin" :)
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
			if (cmbLanguage.SelectedIndex >= 0 && cmbLanguage.SelectedIndex < m_languages.Count)
				Globalizator.Globalizator.CurrentCulture = ((System.Globalization.CultureInfo)(((DictionaryEntry)m_languages[cmbLanguage.SelectedIndex]).Value));
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
            btnOK.Enabled = txtPassword.Text.Trim().Length > 0 && cmbServerUrl.Text.Length > 0 && txtUsername.Text.Length > 0;
        }

	}
}
