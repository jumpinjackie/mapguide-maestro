#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI
{
    public class MgFeatureSetReader : FeatureSetReader
    {
        private OSGeo.MapGuide.MgReader m_rd;

        public MgFeatureSetReader(OSGeo.MapGuide.MgReader mr) : base()
		{
			m_rd = mr;

            FeatureSetColumn[] cols = new FeatureSetColumn[mr.GetPropertyCount()];
            for (int i = 0; i < cols.Length; i++)
                cols[i] = new MgFeatureSetColumn(mr.GetPropertyName(i), mr.GetPropertyType(mr.GetPropertyName(i)));

            InitColumns(cols);
		}

        protected override bool ReadInternal()
        {
            return m_rd.ReadNext();
        }

        protected override FeatureSetRow ProcessFeatureRow()
        {
            return new MgFeatureSetRow(this, m_rd);
        }

        protected override void CloseInternal()
        {
            m_rd.Close();
        }

        public override int Depth
        {
            get
            {
                return -1;
            }
        }

        public override System.Data.DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public override int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class MgFeatureSetRow : FeatureSetRow
    {
        private Topology.IO.MapGuide.MgReader m_mgReader = null;

        private Topology.IO.MapGuide.MgReader MgReader
        {
            get
            {
                if (m_mgReader == null)
                {
                    m_mgReader = new Topology.IO.MapGuide.MgReader();
                }

                return m_mgReader;
            }
        }

        internal MgFeatureSetRow(FeatureSetReader parent, OSGeo.MapGuide.MgReader rd)
			: base(parent)
		{
			for(int i = 0; i < m_parent.Columns.Length; i++)
			{
				string p = m_parent.Columns[i].Name;
				int ordinal = GetOrdinal(p);
				m_nulls[ordinal] = rd.IsNull(p);

                if (!m_nulls[ordinal])
                {
                    if (parent.Columns[ordinal].Type == typeof(string))
                        m_items[ordinal] = rd.GetString(p);
                    else if (parent.Columns[ordinal].Type == typeof(int))
                        m_items[ordinal] = rd.GetInt32(p);
                    else if (parent.Columns[ordinal].Type == typeof(long))
                        m_items[ordinal] = rd.GetInt64(p);
                    else if (parent.Columns[ordinal].Type == typeof(short))
                        m_items[ordinal] = rd.GetInt16(p);
                    else if (parent.Columns[ordinal].Type == typeof(double))
                        m_items[ordinal] = rd.GetDouble(p);
                    else if (parent.Columns[ordinal].Type == typeof(float))
                        m_items[ordinal] = rd.GetSingle(p);
                    else if (parent.Columns[ordinal].Type == typeof(bool))
                        m_items[ordinal] = rd.GetBoolean(p);
                    else if (parent.Columns[ordinal].Type == typeof(DateTime))
                    {
                        MgDateTime t = rd.GetDateTime(p);
                        try
                        {
                            m_items[ordinal] = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
                        }
                        catch(Exception ex)
                        {
                            //Unfortunately FDO supports invalid dates, such as the 30th feb
                            m_nulls[ordinal] = true;
                            m_items[ordinal] = ex;
                        }
                    }
                    else if (parent.Columns[ordinal].Type == Utility.GeometryType)
                    {
                        //TODO: Uncomment this once the Topology.Net API gets updated to 2.0.0
                        //It is optional to include the Topology.IO.MapGuide dll
                        /*if (this.MgReader != null)
                            m_items[ordinal] = this.MgReader.ReadGeometry(ref rd, p);
                        else*/
                        {
                            try
                            {
                                //No MapGuide dll, convert to WKT and then to internal representation
                                System.IO.MemoryStream ms = Utility.MgStreamToNetStream(rd, rd.GetType().GetMethod("GetGeometry"), new string[] { p });
                                OSGeo.MapGuide.MgAgfReaderWriter rdw = new OSGeo.MapGuide.MgAgfReaderWriter();
                                OSGeo.MapGuide.MgGeometry g = rdw.Read(rd.GetGeometry(p));
                                OSGeo.MapGuide.MgWktReaderWriter rdww = new OSGeo.MapGuide.MgWktReaderWriter();
                                m_items[ordinal] = this.Reader.Read(rdww.Write(g));
                            }
                            catch (MgException ex)
                            {
                                //Just like the XmlFeatureSetReader, invalid geometry can bite us again
                                m_nulls[ordinal] = true;
                                m_items[ordinal] = NestedExceptionMessageProcessor.GetFullMessage(ex);
                                ex.Dispose();
                            }
                        }
                    }
                    else if (parent.Columns[ordinal].Type == Utility.UnmappedType)
                    {
                        //Attempt to read it as a string
                        try { m_items[ordinal] = rd.GetString(p); }
                        catch { m_items[ordinal] = null; }
                    }
                    else
                        throw new Exception("Unknown type: " + parent.Columns[ordinal].Type.FullName);
                }
			}
		}

        public object this[int index]
        {
            get
            {
                if (index >= m_items.Length)
                    throw new InvalidOperationException("Index " + index.ToString() + ", was out of bounds");
                else
                {
                    if (m_lazyloadGeometry[index] && !m_nulls[index])
                    {
                        m_items[index] = this.Reader.Read((string)m_items[index]);
                        m_lazyloadGeometry[index] = false;
                    }

                    return m_items[index];
                }
            }
        }
    }

    public class MgFeatureSetColumn : FeatureSetColumn
    {
        internal MgFeatureSetColumn(string name, int type) : base()
		{
			m_name = name;
			m_type = Utility.ConvertMgTypeToNetType(type);
		}
    }
}
