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
using System.Collections;
using System.Data;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base implementation of the <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/>
    /// interface
    /// </summary>
    public abstract class FeatureReaderBase : ReaderBase, IFeatureReader
    {
        protected abstract IFeature ReadNextFeature();

        private ClassDefinition _cls;

        public OSGeo.MapGuide.MaestroAPI.Schema.ClassDefinition ClassDefinition
        {
            get { return _cls; }
            protected set
            {
                _cls = value;
                this.FieldCount = _cls.Properties.Count;
            }
        }

        public IFeatureReader GetFeatureObject(string name)
        {
            return ((IFeature)this.Current).GetFeatureObject(name);
        }

        public IFeatureReader GetFeatureObject(int index)
        {
            return ((IFeature)this.Current).GetFeatureObject(index);
        }

        public override string GetName(int index)
        {
            return this.ClassDefinition.Properties[index].Name;
        }

        public override Type GetFieldType(int i)
        {
            return ClrFdoTypeMap.GetClrType(this.ClassDefinition[i]);
        }

        public override ReaderType ReaderType
        {
            get { return ReaderType.Feature; }
        }

        protected override IRecord ReadNextRecord()
        {
            return ReadNextFeature();
        }

        class Enumerator : IEnumerator<IFeature>
        {
            private FeatureReaderBase _reader;

            public Enumerator(FeatureReaderBase reader)
            {
                _reader = reader;
            }

            public IFeature Current
            {
                get { return (IFeature)_reader.Current; }
            }

            public void Dispose()
            {
                
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                return _reader.ReadNext();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerator<IFeature> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
