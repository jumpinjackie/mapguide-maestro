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

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Marks this assembly as being an implementation of the Maestro API
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class MaestroApiProviderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaestroApiProviderAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="implType">Type of the impl.</param>
        /// <param name="multiPlatform">if set to <c>true</c> [multi platform].</param>
        /// <param name="bHasGlobalState">Whether this provider has global connection state</param>
        public MaestroApiProviderAttribute(string name, string description, Type implType, bool multiPlatform, bool bHasGlobalState)
        {
            this.Name = name;
            this.Description = description;
            this.ImplType = implType;
            this.IsMultiPlatform = multiPlatform;
            this.HasGlobalState = bHasGlobalState;
        }

        /// <summary>
        /// The name of this provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description for this provider
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether this implementation can be used on non-windows environments (eg. Mono)
        /// </summary>
        public bool IsMultiPlatform { get; set; }

        /// <summary>
        /// Indicates whether this provider contains global connection-specific state. For such providers, connections are effectively
        /// single-instance. This is corner-case property required by a particular provider (Maestro.Local), but may extend to other providers
        /// in the future
        /// </summary>
        public bool HasGlobalState { get; set; }

        /// <summary>
        /// The type that implements our main server connection interface
        /// </summary>
        public Type ImplType { get; set; }
    }
}
