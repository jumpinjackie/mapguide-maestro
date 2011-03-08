#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

namespace OSGeo.MapGuide.MaestroAPI.Serialization
{
	/// <summary>
	/// An object that can be serialized, using the internal MapGuide format, must implement this interface
	/// </summary>
	public interface IBinarySerializable
	{
        /// <summary>
        /// Serializes using the specified serializer.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
		void Serialize(MgBinarySerializer serializer);
        /// <summary>
        /// Deserializes using the specified deserializer.
        /// </summary>
        /// <param name="deserializer">The deserializer.</param>
		void Deserialize(MgBinaryDeserializer deserializer);
	}
}
