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
        /// <summary>
        /// Reads the next feature.
        /// </summary>
        /// <returns></returns>
        protected abstract IFeature ReadNextFeature();

        private ClassDefinition _cls;

        /// <summary>
        /// Gets the class definition of the object currently being read. If the user has requested
        /// only a subset of the class properties (as specified in the filter text), the class
        /// definition reflects what the user has requested, rather than the full class definition.
        /// </summary>
        public OSGeo.MapGuide.MaestroAPI.Schema.ClassDefinition ClassDefinition
        {
            get { return _cls; }
            protected set
            {
                _cls = value;
                this.FieldCount = _cls.Properties.Count;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFeatureReader GetFeatureObject(string name)
        {
            return ((IFeature)this.Current).GetFeatureObject(name);
        }

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IFeatureReader GetFeatureObject(int index)
        {
            return ((IFeature)this.Current).GetFeatureObject(index);
        }

        /// <summary>
        /// Gets the name of the field at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override string GetName(int index)
        {
            return this.ClassDefinition.Properties[index].Name;
        }

        /// <summary>
        /// Gets the CLR type of the field at the specified index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override Type GetFieldType(int i)
        {
            return ClrFdoTypeMap.GetClrType(this.ClassDefinition[i]);
        }

        /// <summary>
        /// Gets the type of the reader.
        /// </summary>
        /// <value>
        /// The type of the reader.
        /// </value>
        public override ReaderType ReaderType
        {
            get { return ReaderType.Feature; }
        }

        /// <summary>
        /// Reads the next record.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IFeature> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
