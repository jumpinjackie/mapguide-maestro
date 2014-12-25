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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Feature;
using System.IO;
using System.Net;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    public class XmlDataReader : XmlReaderBase
    {
        internal XmlDataReader(Stream stream)
            : base(stream)
        {
        }

        public XmlDataReader(HttpWebResponse resp)
            : base(resp)
        {
        }

        public override ReaderType ReaderType
        {
            get { return ReaderType.Data; }
        }

        protected override string DefinitionRootElement
        {
            get { return "PropertyDefinitions"; }
        }

        protected override string DefinitionChildElement
        {
            get { return "PropertyDefinition"; }
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
            get { return "Properties"; }
        }

        protected override string ValuesRowElement
        {
            get { return "PropertyCollection"; }
        }

        protected override string ValuesRowPropertyElement
        {
            get { return "Property"; }
        }

        protected override string ValuesRowPropertyNameElement
        {
            get { return "Name"; }
        }

        protected override string ValuesRowPropertyValueElement
        {
            get { return "Value"; }
        }

        protected override string ResponseRootElement
        {
            get { return "PropertySet"; }
        }
    }
}