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
using System.Collections.Specialized;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC
{
	/// <summary>
	/// Contains static methods for manipulating a connectionstring
	/// </summary>
	public class ConnectionStringManager
	{

		/// <summary>
		/// A list of known setting names for the user id
		/// </summary>
		private static string[] m_Uid = new string[] { "Uid", "User Id", "User" };

		/// <summary>
		/// A list of known setting names for the password
		/// </summary>
		private static string[] m_Pwd = new string[] { "Pwd", "Password" };

		/// <summary>
		/// A list of alternative Pwd names for known providers
		/// </summary>
		private static string[] m_PwdOverrides = new string[]
		{
			"{Oracle ODBC Driver}", "Password",
			"{MySQL ODBC 3.51 Driver}", "Password"
		};

		/// <summary>
		/// A list of alternative Uid names for known providers
		/// </summary>
		private static string[] m_UidOverrides = new string[]
		{
			"{Oracle ODBC Driver}", "User Id",
			"{MySQL ODBC 3.51 Driver}", "User"
		};



		/// <summary>
		/// Builds a file based connection string
		/// </summary>
		/// <param name="item">Feature source</param>
		/// <param name="file">Name of file</param>
		/// <returns>A full connectionstring</returns>
		public static string BuildConnectionString(OSGeo.MapGuide.MaestroAPI.FeatureSource item, string file)
		{
			ArrayList parts = new ArrayList();
			int x = file.LastIndexOf(".");
			string ext = "";
			if (x >= 0)
				ext = file.Substring(x + 1).Trim().ToLower();

			switch(ext)
			{
				case "mdb":
					parts.Add("Driver={Microsoft Access Driver (*.mdb)}");
					parts.Add("Dbq=" + file);
					break;
				case "asc":
				case "csv":
				case "tab":
				case "txt":
					parts.Add("Driver={Microsoft Text Driver (*.txt; *.csv)}");
					parts.Add("Dbq=" + file);
					parts.Add("Extensions=asc,csv,tab,txt");
					break;
				case "xls":
					parts.Add("Driver={Microsoft Excel Driver (*.xls)}");
					parts.Add("DriverId=790");
					parts.Add("Dbq=" + file);
					break;
				case "sqlite":
				case "db":
					parts.Add("DRIVER=SQLite3 ODBC Driver");
					parts.Add("LongNames=0");
					parts.Add("Timeout=1000");
					parts.Add("NoTXN=0");
					parts.Add("SyncPragma=NORMAL");
					parts.Add("StepAPI=0");
					parts.Add("Database=" + file);
					break;
				case "fdb":
					parts.Add("Driver=Firebird/InterBase(r) driver");
					parts.Add("DbName=" + file);
					break;
				case "dbf":
					parts.Add("Driver={Microsoft dBASE Driver (*.dbf)}");
					parts.Add("DriverID=277");
					parts.Add("Dbq=" + file);
					break;
				default:
					parts.Add("Driver=<Unknown>");
					parts.Add("<Data>=" + file);
					break;
			}

			NameValueCollection nvc = SplitConnectionString(string.Join(";", (string[])parts.ToArray(typeof(string))));
            NameValueCollection nv = SplitConnectionString(item.Parameter["ConnectionString"]);
			MergeCredentialsIntoConnectionString(nv, nvc);
			return JoinConnectionString(nvc);
		}

		/// <summary>
		/// Returns the user id from the connection string
		/// </summary>
		/// <param name="nv">The parsed connectionstring setting dictionary</param>
		/// <returns>The user id found in the connectionstring</returns>
		public static string GetUserId(NameValueCollection nv)
		{
			foreach(string s in m_Uid)
				if (nv[s] != null)
					return nv[s];
			return "";
		}

		/// <summary>
		/// Returns the password from the connection string
		/// </summary>
		/// <param name="nv">The parsed connectionstring setting dictionary</param>
		/// <returns>The password found in the connectionstring</returns>
		public static string GetPassword(NameValueCollection nv)
		{
			foreach(string s in m_Pwd)
				if (nv[s] != null)
					return nv[s];
			return "";
		}

		/// <summary>
		/// Inserts user id and password from the previous dictionary into the new.
		/// </summary>
		/// <param name="prevDict">The dictionary containing the credentials</param>
		/// <param name="newDict">The dictionary recieving the credentials</param>
		public static void MergeCredentialsIntoConnectionString(NameValueCollection prevDict, NameValueCollection newDict)
		{
			newDict[GetUidName(newDict)] = GetUserId(prevDict);
			newDict[GetPwdName(newDict)] = GetPassword(prevDict);
		}


		/// <summary>
		/// Gets the name of the Pwd setting for the given provider
		/// </summary>
		/// <param name="nv">The parsed connectionstring setting dictionary</param>
		/// <returns>The name of the Pwd field for the given provider</returns>
		public static string GetPwdName(NameValueCollection nv)
		{
			string pwdname = m_Pwd[0];

			for(int i = 0; i < m_PwdOverrides.Length; i+=2)
				if (m_PwdOverrides[i].ToLower().Trim() == nv["Driver"] || m_PwdOverrides[i].ToLower().Trim() == nv["Provider"])
					pwdname = m_PwdOverrides[i + 1];

			return pwdname;
		}

		/// <summary>
		/// Gets the name of the Uid setting for the given provider
		/// </summary>
		/// <param name="nv">The parsed connectionstring setting dictionary</param>
		/// <returns>The name of the Uid field for the given provider</returns>
		public static string GetUidName(NameValueCollection nv)
		{
			string uidname = m_Uid[0];

			for(int i = 0; i < m_UidOverrides.Length; i+=2)
				if (m_UidOverrides[i].ToLower().Trim() == nv["Driver"] || m_UidOverrides[i].ToLower().Trim() == nv["Provider"])
					uidname = m_UidOverrides[i + 1];

			return uidname;
		}

		/// <summary>
		/// Parses a connection string, and returns a dictionary of settings
		/// </summary>
		/// <param name="connectionstring">The connectionstring to parse</param>
		/// <returns>The setting dictionary</returns>
		public static NameValueCollection SplitConnectionString(string connectionstring)
		{
			return SplitConnectionString(connectionstring, ';');
		}

		/// <summary>
		/// Parses a connection string, and returns a dictionary of settings
		/// </summary>
		/// <param name="connectionstring">The connectionstring to parse</param>
		/// <param name="separator">The charater that separates the items</param>
		/// <returns>The setting dictionary</returns>
		public static NameValueCollection SplitConnectionString(string connectionstring, char separator)
		{
			if (connectionstring == null)
				return new NameValueCollection();
			NameValueCollection ht = new NameValueCollection();
			foreach(string s in TokenizeConnectionString(connectionstring, separator, -1))
			{
				if (s.Trim().Length > 0)
				{
					string[] pars = TokenizeConnectionString(s, '=', 2);
					if (pars.Length == 2)
						ht[pars[0]] = pars[1];
					else
						ht[s] = "";
				}
			}

			return ht;
		}

		/// <summary>
		/// Tokenizes a connectionstring by a delimiter. 
		/// Curly braces and backslash escapes.
		/// Double delim also escapes.
		/// </summary>
		/// <param name="connectionstring">The connectionstring to tokenize</param>
		/// <param name="delim">The delimiting character</param>
		/// <param name="maxcount">The max number of pairs to return</param>
		/// <returns>An array of tokens</returns>
		private static string[] TokenizeConnectionString(string connectionstring, char delim, int maxcount)
		{
			bool inCurly = false;

			ArrayList vals = new ArrayList();
			int first = 0;
			for(int i = 0; i < connectionstring.Length; i++)
			{
				//Everything in a curly is escaped
				if (inCurly)
				{
					if (connectionstring[i] == '}')
						inCurly = false;
					continue;
				}

				if (connectionstring[i] == '{')
				{
					inCurly = true;
					continue;
				}
				
				if (connectionstring[i] == delim)
				{
					if (vals.Count + 1 == maxcount)
					{
						vals.Add(connectionstring.Substring(first));
						first = connectionstring.Length;
						break;
					}

					vals.Add(connectionstring.Substring(first, i - first));
					first = i + 1;
				}

				//Backslash escapes the next char
				if (connectionstring[i] == '\\')
					i++;
			}

			if (first <= connectionstring.Length - 1)
				vals.Add(connectionstring.Substring(first));

			return (string[])vals.ToArray(typeof(string));
		}

		/// <summary>
		/// Builds a connection string from a setting dictionary
		/// </summary>
		/// <param name="ht">The dictionary to build the connection string from</param>
		/// <returns>The combined connection string</returns>
		public static string JoinConnectionString(NameValueCollection ht)
		{
			string[] items = new string[ht.Keys.Count];
			for(int i = 0; i < ht.Keys.Count; i++)
				items[i] = ht.Keys[i] + "=" + ht[ht.Keys[i]];

			return string.Join(";", items);
		}

		/// <summary>
		/// Inserts known default values into the setting dictionary
		/// </summary>
		/// <param name="nv">The dictionary to update</param>
		/// <param name="drivername">The ODBC driver name</param>
		public static void InsertDefaultValues(NameValueCollection nv, string drivername)
		{
			switch(drivername)
			{
				case "{MySQL ODBC 3.51 Driver}":
					SetDefault(nv, "Server", "localhost");
					SetDefault(nv, "Port", "3306");
					SetDefault(nv, "Option", "3");
					SetDefault(nv, "Database", "database");
					break;
				case "{SQL Server}":
					SetDefault(nv, "Server", "localhost");
					SetDefault(nv, "Database", "database");
					break;
				case "{PostgreSQL}":
					SetDefault(nv, "Server", "127.0.0.1");
					SetDefault(nv, "Port", "5432");
					SetDefault(nv, "Database", "database");
					break;
				case "{INFORMIX 3.30 32 BIT}":
					SetDefault(nv, "Host", "hostname");
					SetDefault(nv, "Server", "localhost");
					SetDefault(nv, "Service", "service-name");
					SetDefault(nv, "Protocol", "olsoctcp");
					SetDefault(nv, "Database", "database");
					break;
				case "{Microsoft ODBC for Oracle}":
					SetDefault(nv, "Server", "localhost");
					SetDefault(nv, "Database", "database");
					break;
				case "{Oracle ODBC Driver}":
					SetDefault(nv, "Dbq", "Database");
					break;
			}
		}

		private static void SetDefault(NameValueCollection nv, string propertyname, string value)
		{
			if (nv[propertyname] == null || nv[propertyname].Trim().Length == 0)
				nv[propertyname] = value;
		}
	}
}
