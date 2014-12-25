#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.ObjectModels
{
    public class BaseSerializable
    {
        protected virtual string NormalizedSerialize(XmlSerializer serializer, object obj)
        {
            return Utility.NormalizedSerialize(serializer, obj);
        }
    }

    public class BaseSerializableMapDefinition : BaseSerializable
    {
        protected string SerializeHTMLColor(Color color, bool bIncludeAlpha)
        {
            return Utility.SerializeHTMLColor(color, bIncludeAlpha);
        }

        protected Color ParseHTMLColor(string strColor)
        {
            return Utility.ParseHTMLColor(strColor);
        }
    }
}