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
using System.IO;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Internal;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    public delegate IRecord RecordFactoryMethod();

    public abstract class XmlReaderBase : ReaderBase
    {
        protected FixedWKTReader _wktReader;
        protected XmlTextReader _reader;

        protected XmlProperty[] _properties;
        protected Dictionary<string, XmlProperty> _propertyMap;

        //The xml response format is mostly the same (structurally).

        protected abstract string ResponseRootElement { get; }

        protected abstract string DefinitionRootElement { get; }

        protected abstract string DefinitionChildElement { get; }

        protected abstract string DefinitionChildNameElement { get; }

        protected abstract string DefinitionChildTypeElement { get; }

        protected abstract string ValuesRootElement { get; }

        protected abstract string ValuesRowElement { get; }

        protected abstract string ValuesRowPropertyElement { get; }

        protected abstract string ValuesRowPropertyNameElement { get; }

        protected abstract string ValuesRowPropertyValueElement { get; }

        public XmlReaderBase(Stream source) 
        {
            _reader = new XmlTextReader(source);
            _wktReader = new FixedWKTReader();
            _reader.WhitespaceHandling = WhitespaceHandling.Significant;
            _propertyMap = new Dictionary<string, XmlProperty>();

            InitProperties();
        }

        protected virtual void InitProperties()
        {
            //SelectAggregate responses start with PropertySet
            _reader.Read();
            if (_reader.Name != "xml")
                throw new Exception("Bad document. Expected xml prolog"); //LOCALIZEME
            _reader.Read();
            if (_reader.Name != this.ResponseRootElement)
                throw new Exception("Bad document. Expected element: " + this.ResponseRootElement); //LOCALIZEME
            _reader.Read();
            if (_reader.Name != this.DefinitionRootElement)
                throw new Exception("Bad document. Expected element: " + this.DefinitionRootElement); //LOCALIZEME

            List<XmlProperty> properties = new List<XmlProperty>();

            XmlDocument headerDoc = new XmlDocument();
            string xml = _reader.ReadOuterXml();
            headerDoc.LoadXml(xml);

            foreach (XmlNode def in headerDoc.FirstChild.ChildNodes)
            {
                if (def.Name != this.DefinitionChildElement)
                    throw new Exception("Bad document. Expected element: " + this.DefinitionChildElement); //LOCALIZEME

                var n = def[this.DefinitionChildNameElement];
                var t = def[this.DefinitionChildTypeElement];

                if (n == null)
                    throw new Exception("Bad document. Expected element: " + this.DefinitionChildNameElement); //LOCALIZEME
                if (t == null)
                    throw new Exception("Bad document. Expected element: " + this.DefinitionChildTypeElement); //LOCALIZEME

                properties.Add(new XmlProperty(n.InnerText, GetPropertyValueType(t.InnerText)));
            }

            /*
            while (_reader.Read() && _reader.Name == this.DefinitionChildElement)
            {
                _reader.Read();
                if (_reader.Name != this.DefinitionChildNameElement)
                    throw new Exception("Bad document. Expected element: " + this.DefinitionChildNameElement); //LOCALIZEME
                string name = _reader.ReadInnerXml();
                _reader.Read();
                if (_reader.Name != this.DefinitionChildTypeElement)
                    throw new Exception("Bad document. Expected element: " + this.DefinitionChildTypeElement);
                string type = _reader.ReadInnerXml();

               
            }*/

            _properties = properties.ToArray();
            foreach (var p in _properties)
            {
                _propertyMap[p.Name] = p;
            }

            this.FieldCount = _properties.Length;

            if (_reader.Name != this.ValuesRootElement)
                throw new Exception("Bad document");

            _reader.Read();

            if (_reader.NodeType != XmlNodeType.EndElement)
            {
                if (_reader.Name == this.ValuesRootElement)
                    _reader = null; //No features :(
            }
        }

        public override PropertyValueType GetPropertyType(string name)
        {
            return _propertyMap[name].Type;
        }

        public override PropertyValueType GetPropertyType(int index)
        {
            return _properties[index].Type;
        }

        public override Type GetFieldType(int i)
        {
            return ClrFdoTypeMap.GetClrType(_properties[i].Type);
        }

        public override string GetName(int index)
        {
            return _properties[index].Name;
        }

        private OSGeo.MapGuide.MaestroAPI.Schema.PropertyValueType GetPropertyValueType(string type)
        {
            switch (type)
            {
                case "bool":
                case "boolean":
                    return PropertyValueType.Boolean;
                case "blob":
                    return PropertyValueType.Blob;
                case "byte":
                    return PropertyValueType.Byte;
                case "clob":
                    return PropertyValueType.Clob;
                case "double":
                case "decimal":
                    return PropertyValueType.Double;
                case "date":
                case "datetime":
                    return PropertyValueType.DateTime;
                case "geometry":
                    return PropertyValueType.Geometry;
                case "int":
                case "int32":
                case "integer":
                    return PropertyValueType.Int32;
                case "int16":
                    return PropertyValueType.Int16;
                case "int64":
                case "long":
                    return PropertyValueType.Int64;
                case "raster":
                    return PropertyValueType.Raster;
                case "float":
                case "single":
                    return PropertyValueType.Single;
                case "string":
                    return PropertyValueType.String;
            }
            throw new ArgumentException();
        }

        protected XmlDocument _recDoc;

        protected override IRecord ReadNextRecord()
        {
            if (_reader == null || (_reader.Name != this.ValuesRowElement))
            {
                return null;
            }
            else
            {
                if (_recDoc == null)
                    _recDoc = new XmlDocument();

                _recDoc.LoadXml(_reader.ReadOuterXml());
                var rec = new XmlRecord(_properties, _wktReader, _recDoc[this.ValuesRowElement].SelectNodes(this.ValuesRowPropertyElement), this.ValuesRowPropertyNameElement, this.ValuesRowPropertyValueElement);
                
                return rec;
            }
        }
    }
}
