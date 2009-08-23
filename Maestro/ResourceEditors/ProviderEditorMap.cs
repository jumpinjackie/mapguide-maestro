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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Represents the mapping from provider name to Editor type
	/// </summary>
	public class ProviderEditorMap
	{
		private ProviderItem[] m_mapppings;

		public ProviderEditorMap()
		{
		}

		/// <summary>
		/// Gets or sets a list of mappings
		/// </summary>
		[System.Xml.Serialization.XmlElementAttribute("ProviderItem")]
		public ProviderItem[] Mappings
		{
			get { return m_mapppings; }
			set { m_mapppings = value; }
		}

		/// <summary>
		/// Represents a single mapping from provider name to control
		/// </summary>
		public class ProviderItem
		{
			private string m_provider;
			private string m_control;
			private string m_assemblyPath;

			/// <summary>
			/// The name of the provider, ea. OSGeo.SDF
			/// </summary>
			public string Provider
			{
				get { return m_provider; }
				set { m_provider = value; }
			}

			/// <summary>
			/// The fully qualified name of the control to load.
			/// </summary>
			public string Control
			{
				get { return m_control; }
				set { m_control = value; }
			}

			/// <summary>
			/// The full path to the assemly containg the editor control
			/// </summary>
			public string AssemblyPath
			{
				get { return m_assemblyPath; }
				set { m_assemblyPath = value; }
			}
		}
	}
}
