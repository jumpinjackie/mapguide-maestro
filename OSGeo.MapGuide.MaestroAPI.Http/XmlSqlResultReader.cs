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
using System.Xml;
using System.IO;
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    public class XmlSqlResultReader : XmlReaderBase
    {
        public XmlSqlResultReader(Stream source) : base(source) { }

        public override ReaderType ReaderType
        {
            get { return ReaderType.Sql; }
        }

        protected override string ResponseRootElement
        {
            get { return "RowSet"; }
        }

        protected override string DefinitionRootElement
        {
            get { return "ColumnDefinitions"; }
        }

        protected override string DefinitionChildElement
        {
            get { return "Column"; }
        }

        protected override string DefinitionChildNameElement
        {
            get { return "Name"; }
        }

        protected override string DefinitionChildTypeElement
        {
            get { return "Type"; }
        }

        protected override string ValuesRootElement
        {
            get { return "Rows"; }
        }

        protected override string ValuesRowElement
        {
            get { return "Row"; }
        }

        protected override string ValuesRowPropertyElement
        {
            get { return "Column"; }
        }

        protected override string ValuesRowPropertyNameElement
        {
            get { return "Name"; }
        }

        protected override string ValuesRowPropertyValueElement
        {
            get { return "Value"; }
        }
    }
}
