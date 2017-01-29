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

using System.Drawing;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// The base class of all serializable resource types
    /// </summary>
    public abstract class BaseSerializable
    {
        /// <summary>
        /// Performs a normalized serialization
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual string NormalizedSerialize(XmlSerializer serializer, object obj)
        {
            return Utils.NormalizedSerialize(serializer, obj);
        }
    }

    /// <summary>
    /// The base class of all serializable map definition types
    /// </summary>
    public class BaseSerializableMapDefinition : BaseSerializable
    {
        /// <summary>
        /// Serializes the given color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="bIncludeAlpha"></param>
        /// <returns></returns>
        protected string SerializeHTMLColor(Color color, bool bIncludeAlpha)
        {
            return Utils.SerializeHTMLColor(color, bIncludeAlpha);
        }

        /// <summary>
        /// Parses the given HTML color
        /// </summary>
        /// <param name="strColor"></param>
        /// <returns></returns>
        protected Color ParseHTMLColor(string strColor)
        {
            return Utils.ParseHTMLColor(strColor);
        }
    }
}