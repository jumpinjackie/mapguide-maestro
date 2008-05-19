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
namespace OSGeo.MapGuide.MaestroAPI 
{
	/// <summary>
	/// Represents a list of data in a featuresource
	/// </summary>
	[System.Xml.Serialization.XmlRootAttribute("DataStoreList", Namespace="", IsNullable=false)]
	public class DataStoreList 
	{
		private ServerConnectionI m_serverConnection;

		/// <summary>
		/// Gets or sets the connection used in various operations performed on this object
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public ServerConnectionI CurrentConnection
		{
			get { return m_serverConnection; }
			set { m_serverConnection = value; }
		}
    
		/// <summary>
		/// The name of the xsd document that will be used to validate this class before and after serialization
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public static readonly string SchemaName = "DataStoreList-1.0.0.xsd";
        
		/// <summary>
		/// Gets the name of the xsd document that will be used to validate this class before and after serialization
		/// </summary>
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

		private DataStoreItemCollection m_dataStore;
        
		/// <summary>
		/// Gets or sets the datastore elements
		/// </summary>
		[System.Xml.Serialization.XmlElementAttribute("DataStore")]
		public DataStoreItemCollection DataStore 
		{
			get { return this.m_dataStore; }
			set { this.m_dataStore = value;	}
		}
	}

	/// <summary>
	/// Represents a single datastore item
	/// </summary>
	public class DataStoreItem 
	{
        
		private string m_name;
		private bool m_fdoEnabled;
        
		/// <summary>
		/// Gets or sets name of the data item
		/// </summary>
		public string Name 
		{
			get { return this.m_name; }
			set { this.m_name = value; }
		}
        
		/// <summary>
		/// Gets or sets a value indicating if this item is FDO enabled
		/// </summary>
		public bool FdoEnabled 
		{
			get { return this.m_fdoEnabled; }
			set { this.m_fdoEnabled = value; }
		}
	}


	/// <summary>
	/// Represents a list of datastore items
	/// </summary>
	public class DataStoreItemCollection : System.Collections.CollectionBase 
	{
		/// <summary>
		/// Gets or sets a datastore item, using the item index
		/// </summary>
		public DataStoreItem this[int idx] 
		{
			get { return ((DataStoreItem)(base.InnerList[idx])); }
			set { base.InnerList[idx] = value; }
		}
        
		/// <summary>
		/// Adds a datastore item
		/// </summary>
		/// <param name="value">The item to add</param>
		/// <returns>The newly assigned index</returns>
		public int Add(DataStoreItem value) 
		{
			return base.InnerList.Add(value);
		}
	}

    
}
