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
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro
{

	/// <summary>
	/// Simple list style container for sites
	/// </summary>
	public class PreferedSiteList
	{
		private PreferedSite[] m_sites;
        private string m_systemBrowser;
		private int m_initialSite;
		private bool m_autoconnect;
        private bool m_useFusionPreview;
        private bool m_maximizedWindow;
        private int m_windowLeft;
        private int m_windowTop;
        private int m_windowWidth;
        private int m_windowHeight;

		public PreferedSite[] Sites
		{
			get 
			{ 
				if (m_sites == null)
					m_sites = new PreferedSite[0];
				return m_sites; 
			}
			set { m_sites = value; }
		}

        [System.Xml.Serialization.XmlAttribute()]
        public bool UseFusionPreview
        {
            get { return m_useFusionPreview; }
            set { m_useFusionPreview = value; }
        }

        [System.Xml.Serialization.XmlAttribute()]
        public string SystemBrowser
        {
            get 
            {
                if (string.IsNullOrEmpty(m_systemBrowser))
                {
                    //Windows, use system default by url handler
                    if (System.IO.Path.DirectorySeparatorChar == '\\')
                        m_systemBrowser = "";
                    //Linux, assume firefox
                    else
                        m_systemBrowser = "firefox";
                }

                return m_systemBrowser; 
            }
            set { m_systemBrowser = value; }
        }


		[System.Xml.Serialization.XmlAttribute()]
		public int PreferedSite
		{
			get { return m_initialSite; }
			set { m_initialSite = value; }
		}

		[System.Xml.Serialization.XmlAttribute()]
		public bool AutoConnect
		{
			get { return m_autoconnect; }
			set { m_autoconnect = value; }
		}

        [System.Xml.Serialization.XmlAttribute()]
        public bool MaximizedWindow
        {
            get { return m_maximizedWindow; }
            set { m_maximizedWindow = value; }
        }

        [System.Xml.Serialization.XmlAttribute()]
        public int WindowLeft
        {
            get { return m_windowLeft; }
            set { m_windowLeft = value; }
        }

        [System.Xml.Serialization.XmlAttribute()]
        public int WindowTop
        {
            get { return m_windowTop; }
            set { m_windowTop = value; }
        }

        [System.Xml.Serialization.XmlAttribute()]
        public int WindowWidth
        {
            get { return m_windowWidth; }
            set { m_windowWidth = value; }
        }

        [System.Xml.Serialization.XmlAttribute()]
        public int WindowHeight
        {
            get { return m_windowHeight; }
            set { m_windowHeight = value; }
        }

		public void AddSite(PreferedSite site)
		{
			//A generic collection would be nice :/
			PreferedSite[] n = new PreferedSite[m_sites.Length + 1];
			Array.Copy(m_sites, 0, n, 0, m_sites.Length);
			n[n.Length-1] = site;
			m_sites = n;
		}

        private static string AppSettingFile
        {
            get
            {
                string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Application.ProductName);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                path = System.IO.Path.Combine(path, "sitelist.xml");

                string oldPath = System.IO.Path.Combine(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Maestro"), "sitelist.xml");
                if (System.IO.File.Exists(oldPath) && !System.IO.File.Exists(path))
                    try { System.IO.File.Move(oldPath, path); }
                    catch { }

                return path;
            }
        }

        public static PreferedSiteList Load()
        {
            try
            {
                if (System.IO.File.Exists(AppSettingFile))
                {
                    System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(PreferedSiteList));
                    using (System.IO.FileStream fs = System.IO.File.Open(AppSettingFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                        return (PreferedSiteList)sz.Deserialize(fs);
                }
            }
            catch
            {
            }

            return new PreferedSiteList();
        }

        public void Save()
        {
            System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(PreferedSiteList));
            using (System.IO.FileStream fs = System.IO.File.Open(AppSettingFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                fs.SetLength(0);
                sz.Serialize(fs, this);
            }
        }
	}

	/// <summary>
	/// Simple container class for sites
	/// </summary>
	public class PreferedSite
	{
		private string m_siteURL;
		private string m_startingPoint;
		private string m_username;
		private string m_scrambledPassword;
		private bool m_savePassword;
		private Version m_approvedVersion;

		public PreferedSite()
		{
		}

		public string SiteURL
		{
			get { return m_siteURL; }
			set { m_siteURL = value; }
		}

		public string StartingPoint
		{
			get { return m_startingPoint; }
			set { m_startingPoint = value; }
		}

		public string Username
		{
			get { return m_username; }
			set { m_username = value; }
		}

		public string ScrambledPassword
		{
			get { return m_scrambledPassword; }
			set { m_scrambledPassword = value; }
		}

		public bool SavePassword
		{
			get { return m_savePassword; }
			set { m_savePassword = value; }
		}

		public string ApprovedVersionString
		{
			get { return ApprovedVersion.ToString(); }
			set { ApprovedVersion = new Version(value); }
		}

		[System.Xml.Serialization.XmlIgnore()]
		public Version ApprovedVersion
		{
			get 
			{
				if (m_approvedVersion == null)
					m_approvedVersion = new Version(0, 0, 0, 0);
				return m_approvedVersion; 
			}
			set { m_approvedVersion = value; }
		}

		[System.Xml.Serialization.XmlIgnore()]
		public string UnscrambledPassword
		{
			get { return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(m_scrambledPassword == null ? "" : m_scrambledPassword)); }
			set { m_scrambledPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value)); }
		}


		public override string ToString()
		{
			return m_siteURL;
		}

	}
}
