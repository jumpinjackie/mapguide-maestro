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

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    /// <summary>
    /// Represents a coordinate system category
    /// </summary>
    public abstract class CoordinateSystemCategory
    {
        private ICoordinateSystemCatalog _parent;
		private string m_name;
        private CoordinateSystemDefinitionBase[] m_items;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemCategory"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        protected CoordinateSystemCategory(ICoordinateSystemCatalog parent, string name)
		{
			m_name = name;
			_parent = parent;
		}

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name { get { return m_name; } }

        internal ICoordinateSystemCatalog Parent { get { return _parent; } }

        /// <summary>
        /// Gets an array of all coordinate systems in this category
        /// </summary>
		public CoordinateSystemDefinitionBase[] Items
		{
			get
			{
				if (m_items == null)
				{
                    if (_parent != null)
                    {
                        m_items = _parent.EnumerateCoordinateSystems(m_name);
                    }
				}
				return m_items;
			}
		}

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
		public override string ToString()
		{
			return m_name;
		}
    }
}
