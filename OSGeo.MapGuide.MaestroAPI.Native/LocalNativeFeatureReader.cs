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
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Internal;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    public class LocalNativeFeatureReader : FeatureReaderBase
    {
        private MgFeatureReader _reader;
        private FixedWKTReader _mgReader;
        private MgAgfReaderWriter _agfRw;
        private MgWktReaderWriter _wktRw;

        public LocalNativeFeatureReader(MgFeatureReader reader) 
        {
            _reader = reader;
            _mgReader = new FixedWKTReader();
            _agfRw = new MgAgfReaderWriter();
            _wktRw = new MgWktReaderWriter();
            base.ClassDefinition = Utility.ConvertClassDefinition(reader.GetClassDefinition());
            base.FieldCount = reader.GetPropertyCount();
        }

        protected override IFeature ReadNextFeature()
        {
            if (_reader.ReadNext())
                return new LocalNativeFeature(_reader, _mgReader, _agfRw, _wktRw);

            return null;
        }

        public override PropertyValueType GetPropertyType(int index)
        {
            return (PropertyValueType)_reader.GetPropertyType(index); //We can do this because the enum values map directly to MgPropertyType
        }

        public override PropertyValueType GetPropertyType(string name)
        {
            return (PropertyValueType)_reader.GetPropertyType(name); //We can do this because the enum values map directly to MgPropertyType
        }

        public override void Dispose()
        {
            Close();
            if (_agfRw != null)
            {
                try
                {
                    _agfRw.Dispose();
                }
                catch (MgException ex)
                {
                    ex.Dispose();
                }
                _agfRw = null;
            }
            if (_wktRw != null)
            {
                try
                {
                    _wktRw.Dispose();
                }
                catch (MgException ex)
                {
                    ex.Dispose();
                }
                _wktRw = null;
            }
            base.Dispose();
        }

        public override void Close()
        {
            if (_reader != null)
            {
                try
                {
                    _reader.Close();
                    _reader.Dispose();
                }
                catch (MgException ex)
                {
                    ex.Dispose();
                }
                _reader = null;
            }
        }
    }
}
