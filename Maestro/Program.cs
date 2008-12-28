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
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for Program.
	/// </summary>
	public class Program
	{
		[STAThread()]
		public static void Main(string[] args)
		{
            //System.Text.RegularExpressions.Regex m_filenameTransformer = new System.Text.RegularExpressions.Regex(@"[^A-Za-z0-9\.-\/]");
            //System.Text.RegularExpressions.Match m = m_filenameTransformer.Match("xxæøå");

			try
			{
				Application.EnableVisualStyles();
				Application.DoEvents();
				Globalizator.Globalizator.InitializeResourceManager();
				Application.Run(new FormMain());
			}
			catch(Exception ex)
			{
				MessageBox.Show("A serious error occured: " + ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private static string AppSettingFile
        {
            get
            {
                string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Application.ProductName);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                path = System.IO.Path.Combine(path, "sitelist.xml");

                return path;
            }
        }

        private static PreferedSiteList _ApplicationSettings = null;

        public static PreferedSiteList ApplicationSettings
        {
            get 
            {
                if (_ApplicationSettings == null)
                {
                    try
                    {
                        if (System.IO.File.Exists(AppSettingFile))
                        {
                            System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(PreferedSiteList));
                            using (System.IO.FileStream fs = System.IO.File.Open(AppSettingFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                                return (PreferedSiteList)sz.Deserialize(fs);
                        }
                        else
                            _ApplicationSettings = new PreferedSiteList();
                    }
                    catch
                    {
                        _ApplicationSettings = new PreferedSiteList();
                    }
                }

                return _ApplicationSettings;
            }
            set
            {
                System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(PreferedSiteList));
                using (System.IO.FileStream fs = System.IO.File.Open(AppSettingFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None))
                {
                    fs.SetLength(0);
                    sz.Serialize(fs, value);
                }

                _ApplicationSettings = value;
            }
        }

        /// <summary>
        /// Opens the given URL in a browser
        /// </summary>
        public static void OpenUrl(string url)
        {
            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                    throw new Exception("Malformed URL");

                if (string.IsNullOrEmpty(ApplicationSettings.SystemBrowser))
                {
                    try
                    {
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = url;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
                    catch
                    {
                        //The straightforward method gives an error: "The requested lookup key was not found in any active activation context"
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "rundll32.exe";
                        process.StartInfo.Arguments = "url.dll,FileProtocolHandler " + url;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
                }
                else
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = ApplicationSettings.SystemBrowser;
                    process.StartInfo.Arguments = url;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                MessageBox.Show(string.Format(Globalizator.Globalizator.Translate("OSGeo.MapGuide.Maestro.Localization.FormAbout", System.Reflection.Assembly.GetExecutingAssembly(), "Unable to open a browser window, please manually visit: \r\n{0}"), url), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
	}
}
