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
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    public class LocalNativeSqlReader : ReaderBase
    {
        private MgSqlDataReader _reader;
        private Topology.IO.WKTReader _mgReader;
        private MgAgfReaderWriter _agfRw;
        private MgWktReaderWriter _wktRw;

        public LocalNativeSqlReader(MgSqlDataReader reader) 
        {
            _reader = reader;
            _mgReader = new Topology.IO.WKTReader();
            _agfRw = new MgAgfReaderWriter();
            _wktRw = new MgWktReaderWriter();
        }

        public override ReaderType ReaderType
        {
            get { return ReaderType.Sql; }
        }

        public override void Dispose()
        {
            _reader.Dispose();
            _agfRw.Dispose();
            _wktRw.Dispose();
            base.Dispose();
        }

        protected override IRecord ReadNextRecord()
        {
            if (_reader.ReadNext())
                return new LocalNativeRecord(_reader, _mgReader, _agfRw, _wktRw);

            return null;
        }

        public override void Close()
        {
            _reader.Close();
        }

        public override string GetName(int index)
        {
            return _reader.GetPropertyName(index);
        }

        public override Type GetFieldType(int i)
        {
            string name = GetName(i);
            //The enum uses the same values as MgPropertyType
            var type = (PropertyValueType)_reader.GetPropertyType(name);
            return ClrFdoTypeMap.GetClrType(type);
        }
    }
}
