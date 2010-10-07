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
using System.IO;
using System.Xml;
using Topology.Geometries;
using System.Data;
using System.Collections.Generic;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Represents a set of results from a query
	/// </summary>
	public abstract class FeatureSetReader : IDisposable, IDataReader, IEnumerable<IDataRecord>
	{
		protected FeatureSetColumn[] m_columns;
		protected FeatureSetRow m_row;

        protected Dictionary<string, int> _nameOrdinalMap;

        protected FeatureSetReader() { }

        /// <summary>
        /// Initializes the column array for this reader. Must be called before
        /// any reading operations commence.
        /// </summary>
        /// <param name="cols"></param>
        protected void InitColumns(FeatureSetColumn[] cols)
        {
            m_columns = cols;
            _nameOrdinalMap = new Dictionary<string, int>();
            for (int i = 0; i < m_columns.Length; i++)
            {
                _nameOrdinalMap.Add(m_columns[i].Name, i);
            }
        }

		public FeatureSetColumn[] Columns
		{
			get { return m_columns; }
		}

        public bool Read()
        {
            m_row = null;
            bool next = ReadInternal();
            if (next)
            {
                m_row = ProcessFeatureRow();
            }
            return next;
        }

        protected abstract bool ReadInternal();

        protected abstract FeatureSetRow ProcessFeatureRow();
        
		public FeatureSetRow Row
		{
			get { return m_row; }
		}

        public virtual void Dispose() { }

        public void Close()
        {
            CloseInternal();
            this.IsClosed = true;
        }

        protected abstract void CloseInternal();

        public abstract int Depth { get; }

        public abstract DataTable GetSchemaTable();

        public bool IsClosed { get; private set; }

        public bool NextResult()
        {
            return Read();
        }

        public abstract int RecordsAffected { get; }

        public int FieldCount
        {
            get { return m_columns.Length; }
        }

        public virtual bool GetBoolean(int i)
        {
            return (bool)m_row[GetName(i)];
        }

        public virtual byte GetByte(int i)
        {
            return (byte)m_row[GetName(i)];
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public virtual char GetChar(int i)
        {
            return (char)m_row[GetName(i)];
        }

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public virtual IDataReader GetData(int i)
        {
            return (IDataReader)m_row[GetName(i)];
        }

        public virtual string GetDataTypeName(int i)
        {
            return m_columns[i].Type.Name;
        }

        public virtual DateTime GetDateTime(int i)
        {
            return (DateTime)m_row[GetName(i)];
        }

        public virtual decimal GetDecimal(int i)
        {
            return (decimal)m_row[GetName(i)];
        }

        public virtual double GetDouble(int i)
        {
            return (double)m_row[GetName(i)];
        }

        public virtual Type GetFieldType(int i)
        {
            return m_columns[i].Type;
        }

        public float GetFloat(int i)
        {
            return (float)m_row[GetName(i)];
        }

        public virtual Guid GetGuid(int i)
        {
            return (Guid)m_row[GetName(i)];
        }

        public short GetInt16(int i)
        {
            return (short)m_row[GetName(i)];
        }

        public int GetInt32(int i)
        {
            return (int)m_row[GetName(i)];
        }

        public long GetInt64(int i)
        {
            return (long)m_row[GetName(i)];
        }

        public string GetName(int i)
        {
            return m_columns[i].Name;
        }

        public int GetOrdinal(string name)
        {
            return _nameOrdinalMap[name];
        }

        public string GetString(int i)
        {
            return (string)m_row[GetName(i)];
        }

        public object GetValue(int i)
        {
            return m_row[GetName(i)];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return m_row.IsValueNull(i);
        }

        public object this[string name]
        {
            get { return m_row[name]; }
        }

        public object this[int i]
        {
            get { return m_row[GetName(i)]; }
        }

        internal class FeatureSetRowEnumerator : IEnumerator<IDataRecord>
        {
            private FeatureSetReader _parent;

            public FeatureSetRowEnumerator(FeatureSetReader parent)
            {
                _parent = parent;
            }

            public IDataRecord Current
            {
                get { return _parent.Row; }
            }

            public void Dispose() { }

            object System.Collections.IEnumerator.Current
            {
                get { return _parent.Row; }
            }

            public bool MoveNext()
            {
                return _parent.Read();
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        public IEnumerator<IDataRecord> GetEnumerator()
        {
            return new FeatureSetRowEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new FeatureSetRowEnumerator(this);
        }
    }

	public abstract class FeatureSetColumn
	{
        protected System.Collections.Hashtable m_metadata = new System.Collections.Hashtable();

		protected string m_name;
        protected Type m_type;
        protected bool m_allowNull;

		public string Name { get { return m_name; } }
		public Type Type { get { return m_type; } }
        public bool IsIdentity { get; internal set; }

        public System.Collections.ICollection MetadataKeys { get { return m_metadata.Keys; } }

        public object GetMetadata(string key)
        {
            return m_metadata[key];
        }

        public void SetMetadata(string key, object value)
        {
            m_metadata[key] = value;
        }
	}

	public abstract class FeatureSetRow : IDataRecord
	{
        private Topology.IO.WKTReader m_reader = null;

        protected Topology.IO.WKTReader Reader
        {
            get
            {
                if (m_reader == null)
                    m_reader = new Topology.IO.WKTReader();
                return m_reader;
            }
        }

        protected FeatureSetReader m_parent;
        protected object[] m_items;
        protected bool[] m_nulls;
        protected bool[] m_lazyloadGeometry;
        
		protected FeatureSetRow(FeatureSetReader parent)
		{
			m_parent = parent;
			m_items = new object[parent.Columns.Length];
			m_nulls = new bool[parent.Columns.Length];
			m_lazyloadGeometry = new bool[parent.Columns.Length];
			for(int i = 0;i < m_nulls.Length; i++)
			{
				m_nulls[i] = true;
				m_lazyloadGeometry[i] = false;
			}
		}

        [Obsolete("This will be gone in a future release. Use IsDBNull(int i) instead. To get the index use GetOrdinal(string name)")]
		public bool IsValueNull(string name)
		{
			return IsValueNull(GetOrdinal(name));
		}

        [Obsolete("This will be gone in a future release. Use IsDBNull(int i) instead")]
		public bool IsValueNull(int index)
		{
			if (index >= m_nulls.Length)
				throw new InvalidOperationException("Index " + index.ToString() + ", was out of bounds");
			else
				return m_nulls[index];
		}

		public int GetOrdinal(string name)
		{
            if (name == null)
                throw new ArgumentNullException("name");

            if (name == "")
                throw new Exception("The name parameter must not be empty");

			name = name.Trim();

			for(int i = 0; i < m_parent.Columns.Length; i++)
				if (m_parent.Columns[i].Name.Equals(name))
					return i;

			for(int i = 0; i < m_parent.Columns.Length; i++)
				if (m_parent.Columns[i].Name.ToLower().Equals(name.ToLower()))
					return i;

			string[] t = new string[m_parent.Columns.Length];
			for(int i = 0; i < m_parent.Columns.Length; i++)
				t[i] = m_parent.Columns[i].Name;

			throw new InvalidOperationException("Column name: " + name + ", was not found\nColumn names (" + m_parent.Columns.Length.ToString() + "): " + string.Join(", ", t));
		}

		public object this[string name]
		{
			get 
			{
				return this[GetOrdinal(name)];
			}
		}

        public int FieldCount
        {
            get { return m_parent.Columns.Length; }
        }

        public bool GetBoolean(int i)
        {
            return (bool)m_items[i];
        }

        public byte GetByte(int i)
        {
            return (byte)m_items[i];
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return (char)m_items[i];
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            return (IDataReader)m_items[i];
        }

        public string GetDataTypeName(int i)
        {
            return m_parent.Columns[i].Type.Name;
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)m_items[i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)m_items[i];
        }

        public double GetDouble(int i)
        {
            return (double)m_items[i];
        }

        public Type GetFieldType(int i)
        {
            return m_parent.Columns[i].Type;
        }

        public float GetFloat(int i)
        {
            return (float)m_items[i];
        }

        public Guid GetGuid(int i)
        {
            return (Guid)m_items[i];
        }

        public short GetInt16(int i)
        {
            return (short)m_items[i];
        }

        public int GetInt32(int i)
        {
            return (int)m_items[i];
        }

        public long GetInt64(int i)
        {
            return (long)m_items[i];
        }

        public string GetName(int i)
        {
            return m_parent.GetName(i);
        }

        public string GetString(int i)
        {
            return (string)m_items[i];
        }

        public object GetValue(int i)
        {
            return m_items[i];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int index)
        {
            if (index >= m_nulls.Length)
                throw new InvalidOperationException("Index " + index.ToString() + ", was out of bounds");
            else
                return m_nulls[index];
        }

        public object this[int i]
        {
            get 
            {
                if (i >= m_items.Length)
                {
                    throw new InvalidOperationException("Index " + i.ToString() + ", was out of bounds");
                }
                else
                {
                    if (m_lazyloadGeometry[i] && !m_nulls[i])
                    {
                        m_items[i] = this.Reader.Read((string)m_items[i]);
                        m_lazyloadGeometry[i] = false;
                    }
                    return m_items[i];
                }
            }
        }
    }
}
