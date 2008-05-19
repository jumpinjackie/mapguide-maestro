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

namespace OSGeo.MapGuide.Maestro
{

	/// <summary>
	/// Simple list style container for sites
	/// </summary>
	public class PreferedSiteList
	{
		private PreferedSite[] m_sites;
		private int m_initialSite;
		private bool m_autoconnect;

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


		public void AddSite(PreferedSite site)
		{
			//A generic collection would be nice :/
			PreferedSite[] n = new PreferedSite[m_sites.Length + 1];
			Array.Copy(m_sites, 0, n, 0, m_sites.Length);
			n[n.Length-1] = site;
			m_sites = n;
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
