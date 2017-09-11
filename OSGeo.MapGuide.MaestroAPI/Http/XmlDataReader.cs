#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

        public override ReaderType ReaderType => ReaderType.Data;

        protected override string DefinitionRootElement => "PropertyDefinitions"; //NOXLATE

        protected override string DefinitionChildElement => "PropertyDefinition"; //NOXLATE

        protected override string DefinitionChildNameElement => "Name"; //NOXLATE

        protected override string DefinitionChildTypeElement => "Type"; //NOXLATE

        protected override string ValuesRootElement => "Properties"; //NOXLATE

        protected override string ValuesRowElement => "PropertyCollection"; //NOXLATE

        protected override string ValuesRowPropertyElement => "Property"; //NOXLATE

        protected override string ValuesRowPropertyNameElement => "Name"; //NOXLATE

        protected override string ValuesRowPropertyValueElement => "Value"; //NOXLATE

        protected override string ResponseRootElement => "PropertySet"; //NOXLATE
    }
}