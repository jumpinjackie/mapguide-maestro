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

using System.Reflection;
using System.Resources;
using System.Collections;
using System.Globalization;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Class that enables globalization of a form
	/// </summary>
	public class Globalizator : IDisposable
	{

		/// <summary>
		/// A global list of translated objects
		/// </summary>
		private static Hashtable _LocalizedObjects = new Hashtable();

		/// <summary>
		/// The current culture
		/// </summary>
		private static CultureInfo _CurrentCulture = CultureInfo.CurrentUICulture;

		/// <summary>
		/// Changes the global cultureinfo on all registered objects
		/// </summary>
		public static CultureInfo CurrentCulture 
		{
			get { return _CurrentCulture; }
			set 
			{
				_CurrentCulture = value;
				foreach(Globalizator g in _LocalizedObjects.Keys)
					g.Culture = value;
			}			
		}


		/// <summary>
		/// Object that can be localized
		/// </summary>
 		private object m_obj;

		/// <summary>
		/// The Culture used to translate the item
		/// </summary>
		private CultureInfo m_culture;

		/// <summary>
		/// The name (path) to the resource file, excluding the extension
		/// </summary>
		private string m_resourceName;

		/// <summary>
		/// The resource manager
		/// </summary>
		private ResourceManager m_rm;

		/// <summary>
		/// Constructs a globalizer, this method is made for the Maestro project, and the resource path construction may differ in other projects.
		/// </summary>
		/// <param name="obj">The object to localize</param>
		public Globalizator(object obj)
			: this(obj, obj.GetType().Namespace + ".Localization." + obj.GetType().Name)
		{
		}

		/// <summary>
		/// Constructs a new globalization object
		/// </summary>
		/// <param name="obj">The object to localize</param>
		/// <param name="resourceName">The name (path) to the resource file, excluding the extension</param>
		/// <param name="culture">The culture to use as default</param>
		public Globalizator(object obj, string resourceName)
			: this(obj, resourceName, Globalizator.CurrentCulture)
		{
			//We add the item here, so an overriden culture is not updated
			_LocalizedObjects.Add(this, null);
		}

		/// <summary>
		/// Constructs a new globalization object
		/// </summary>
		/// <param name="obj">The object to localize</param>
		/// <param name="resourceName">The name (path) to the resource file, excluding the extension</param>
		/// <param name="culture">The culture to use as default</param>
		public Globalizator(object obj, string resourceName, CultureInfo culture)
		{
			m_obj = obj;
			m_culture = culture;
			m_resourceName = resourceName;
			m_rm = new ResourceManager(resourceName, obj.GetType().Assembly);

			Translate();
		}

		/// <summary>
		/// Gets or sets the current culture
		/// </summary>
		public CultureInfo Culture
		{
			get { return m_culture; }
			set 
			{ 
				m_culture = value; 
				Translate();
			}
		}

		/// <summary>
		/// (Re-)translate the current object
		/// </summary>
		public void Translate()
		{
			Translate(m_obj, "", m_culture, m_resourceName, null, null, m_rm);
		}

		public string Translate(string text)
		{
			string x = m_rm.GetString(text.Replace("\n", "\\n").Replace("\"", "\\\"").Trim(), m_culture);
			if (x == null)
				return text;
			else
                return x.Replace("\\n", "\n");
		}

		public static string Translate(string resourceName, Assembly asm, string text)
		{
			ResourceManager rm = new ResourceManager(resourceName, asm);
			return rm.GetString(resourceName + '.' + text, Globalizator.CurrentCulture);
		}


		public static void Translate(object obj, string resourceName)
		{
			Translate(obj, "", Globalizator.CurrentCulture, resourceName, null, null, null);
		}

		public static void Translate(object obj, string resourceName, CultureInfo culture)
		{
			Translate(obj, "", culture, resourceName, null, null, null);
		}

		private static void Translate(object obj, string baseName, CultureInfo culture, string resourceName, object parent, System.Collections.ArrayList alreadyDone, ResourceManager rm)
		{
			if (null == obj)
				return ;

			Type t = obj.GetType();
			string str;
			
			if (alreadyDone == null || rm == null) 
			{
				alreadyDone = new System.Collections.ArrayList();
				rm = new ResourceManager(resourceName, t.Assembly);
			}
			
			str = parent == null ? t.FullName : baseName;

			//Currently, these are the only localized values
			TranslateProperty(str, rm, obj, culture, "Text", typeof(System.String));
			TranslateProperty(str, rm, obj, culture, "Width", typeof(System.Int32));
			TranslateProperty(str, rm, obj, culture, "Height", typeof(System.Int32));
			TranslateProperty(str, rm, obj, culture, "Top", typeof(System.Int32));
			TranslateProperty(str, rm, obj, culture, "Left", typeof(System.Int32));
			if (obj.GetType().GetProperty("ToolTipText") != null)
				TranslateProperty(str, rm, obj, culture, "ToolTipText", typeof(System.String));

			FieldInfo [] fi = t.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (FieldInfo f in fi)	
			{
				System.ComponentModel.Component c = f.GetValue(obj) as System.ComponentModel.Component;
				if (c != null && !alreadyDone.Contains(c))
				{
					alreadyDone.Add(obj);
					Translate(f.GetValue(obj), str + "." + f.Name, culture, resourceName , parent == null ? obj: parent, alreadyDone, rm);
				}
			}

		}

		private static void TranslateProperty(string resName, ResourceManager rm, object obj, CultureInfo culture, string propname, System.Type returnType)
		{
			try	
			{
				PropertyInfo p = obj.GetType().GetProperty(propname, returnType);
				if (p != null && p.CanWrite)
				{
					object resObj = rm.GetObject(resName + '.' + p.Name, culture);	

					if (resObj != null)
						p.SetValue(obj, resObj, null);
//When debuggin, this drops the current values into the console for grabbing
#if DEBUG
					else 
					{
						if (p.PropertyType == typeof(string))
						{
							string s = Convert.ToString(p.GetValue(obj, null));
							Console.WriteLine("<data name=\"" + resName + '.' + p.Name +"\" type=\"" + p.PropertyType + "\">");
							Console.WriteLine("\t<value>" + s + "</value>");
							Console.WriteLine("</data>");
						}
					}
#endif
				}

			}
#if DEBUG
			catch (Exception ex)	
			{
				Console.WriteLine("Failed on " + ex.Message);
			}
#else
			catch
			{
			}
#endif
		}
		#region IDisposable Members

		public void Dispose()
		{
			if (_LocalizedObjects.ContainsKey(this))
				_LocalizedObjects.Remove(this);
		}

		#endregion
	}
}
