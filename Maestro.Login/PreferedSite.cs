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

namespace Maestro.Login
{

    /// <summary>
    /// Simple list style container for sites
    /// </summary>
    public class PreferredSiteList
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
        private string m_guiLanguage;

        /// <summary>
        /// Gets or sets the sites.
        /// </summary>
        /// <value>The sites.</value>
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

        /// <summary>
        /// Gets or sets the GUI language.
        /// </summary>
        /// <value>The GUI language.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public string GUILanguage
        {
            get { return m_guiLanguage; }
            set { m_guiLanguage = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use fusion preview].
        /// </summary>
        /// <value><c>true</c> if [use fusion preview]; otherwise, <c>false</c>.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public bool UseFusionPreview
        {
            get { return m_useFusionPreview; }
            set { m_useFusionPreview = value; }
        }

        /// <summary>
        /// Gets or sets the system browser.
        /// </summary>
        /// <value>The system browser.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public string SystemBrowser
        {
            get 
            {
                if (string.IsNullOrEmpty(m_systemBrowser))
                {
                    //Windows, use system default by url handler
                    if (System.IO.Path.DirectorySeparatorChar == '\\') //NOXLATE
                        m_systemBrowser = string.Empty;
                    //Linux, assume firefox
                    else
                        m_systemBrowser = "firefox";
                }

                return m_systemBrowser; 
            }
            set { m_systemBrowser = value; }
        }


        /// <summary>
        /// Gets or sets the prefered site.
        /// </summary>
        /// <value>The prefered site.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public int PreferedSite
        {
            get { return m_initialSite; }
            set { m_initialSite = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [auto connect].
        /// </summary>
        /// <value><c>true</c> if [auto connect]; otherwise, <c>false</c>.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public bool AutoConnect
        {
            get { return m_autoconnect; }
            set { m_autoconnect = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [maximized window].
        /// </summary>
        /// <value><c>true</c> if [maximized window]; otherwise, <c>false</c>.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public bool MaximizedWindow
        {
            get { return m_maximizedWindow; }
            set { m_maximizedWindow = value; }
        }

        /// <summary>
        /// Gets or sets the window left.
        /// </summary>
        /// <value>The window left.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public int WindowLeft
        {
            get { return m_windowLeft; }
            set { m_windowLeft = value; }
        }

        /// <summary>
        /// Gets or sets the window top.
        /// </summary>
        /// <value>The window top.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public int WindowTop
        {
            get { return m_windowTop; }
            set { m_windowTop = value; }
        }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>The width of the window.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public int WindowWidth
        {
            get { return m_windowWidth; }
            set { m_windowWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        /// <value>The height of the window.</value>
        [System.Xml.Serialization.XmlAttribute()]
        public int WindowHeight
        {
            get { return m_windowHeight; }
            set { m_windowHeight = value; }
        }

        /// <summary>
        /// Adds the site.
        /// </summary>
        /// <param name="site">The site.</param>
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
                path = System.IO.Path.Combine(path, "sitelist.xml"); //NOXLATE

                string oldPath = System.IO.Path.Combine(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Maestro"), "sitelist.xml"); //NOXLATE
                if (System.IO.File.Exists(oldPath) && !System.IO.File.Exists(path))
                    try { System.IO.File.Move(oldPath, path); }
                    catch { }

                return path;
            }
        }

        /// <summary>
        /// Loads the preferred site list
        /// </summary>
        /// <returns></returns>
        public static PreferredSiteList Load()
        {
            try
            {
                if (System.IO.File.Exists(AppSettingFile))
                {
                    System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(PreferredSiteList));
                    using (System.IO.FileStream fs = System.IO.File.Open(AppSettingFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                        return (PreferredSiteList)sz.Deserialize(fs);
                }
            }
            catch
            {
            }

            return new PreferredSiteList();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(PreferredSiteList));
            using (System.IO.FileStream fs = System.IO.File.Open(AppSettingFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                fs.SetLength(0);
                sz.Serialize(fs, this);
            }
        }

        /// <summary>
        /// Sets the active culture based on the culture specified in the preferred sites
        /// </summary>
        public static string InitCulture()
        {
            try
            {
                PreferredSiteList sites = PreferredSiteList.Load();
                if (!string.IsNullOrEmpty(sites.GUILanguage))
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture =
                    System.Threading.Thread.CurrentThread.CurrentCulture =
                        System.Globalization.CultureInfo.GetCultureInfo(sites.GUILanguage);
                }
            }
            catch
            {
            }

            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferedSite"/> class.
        /// </summary>
        public PreferedSite()
        {
        }

        /// <summary>
        /// Gets or sets the site URL.
        /// </summary>
        /// <value>The site URL.</value>
        public string SiteURL
        {
            get { return m_siteURL; }
            set { m_siteURL = value; }
        }

        /// <summary>
        /// Gets or sets the starting point.
        /// </summary>
        /// <value>The starting point.</value>
        public string StartingPoint
        {
            get { return m_startingPoint; }
            set { m_startingPoint = value; }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        /// <summary>
        /// Gets or sets the scrambled password.
        /// </summary>
        /// <value>The scrambled password.</value>
        public string ScrambledPassword
        {
            get { return m_scrambledPassword; }
            set { m_scrambledPassword = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [save password].
        /// </summary>
        /// <value><c>true</c> if [save password]; otherwise, <c>false</c>.</value>
        public bool SavePassword
        {
            get { return m_savePassword; }
            set { m_savePassword = value; }
        }

        /// <summary>
        /// Gets or sets the approved version string.
        /// </summary>
        /// <value>The approved version string.</value>
        public string ApprovedVersionString
        {
            get { return ApprovedVersion.ToString(); }
            set { ApprovedVersion = new Version(value); }
        }

        /// <summary>
        /// Gets or sets the approved version.
        /// </summary>
        /// <value>The approved version.</value>
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

        /// <summary>
        /// Gets or sets the unscrambled password.
        /// </summary>
        /// <value>The unscrambled password.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public string UnscrambledPassword
        {
            get { return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(m_scrambledPassword == null ? string.Empty : m_scrambledPassword)); }
            set { m_scrambledPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value)); }
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return m_siteURL;
        }

    }
}
