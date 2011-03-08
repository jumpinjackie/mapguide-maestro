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
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.ObjectModels.Common
{

    /// <summary>
    /// Represents an item in the repository
    /// </summary>
    public interface IRepositoryItem
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>The type of the resource.</value>
        ResourceTypes ResourceType { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is folder.
        /// </summary>
        /// <value><c>true</c> if this instance is folder; otherwise, <c>false</c>.</value>
        bool IsFolder { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        bool HasChildren { get; }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        string Owner { get; }

        /// <summary>
        /// Gets the created date.
        /// </summary>
        /// <value>The created date.</value>
        DateTime CreatedDate { get; }

        /// <summary>
        /// Gets the modified date.
        /// </summary>
        /// <value>The modified date.</value>
        DateTime ModifiedDate { get; }
    }

}
