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
namespace OSGeo.MapGuide.MaestroAPI {
   
	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute("ResourceSecurity", Namespace="", IsNullable=true)]
	public class ResourceSecurityType 
	{
		public static readonly string SchemaName = "ResourceSecurity-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
        
		private bool m_inherited;
        
		private ResourceSecurityTypeUsers m_users;
        
		private ResourceSecurityTypeGroups m_groups;
        
		/// <remarks/>
		public bool Inherited 
		{
			get 
			{
				return this.m_inherited;
			}
			set 
			{
				this.m_inherited = value;
			}
		}
        
		/// <remarks/>
		public ResourceSecurityTypeUsers Users 
		{
			get 
			{
				return this.m_users;
			}
			set 
			{
				this.m_users = value;
			}
		}
        
		/// <remarks/>
		public ResourceSecurityTypeGroups Groups 
		{
			get 
			{
				return this.m_groups;
			}
			set 
			{
				this.m_groups = value;
			}
		}
	}

}
