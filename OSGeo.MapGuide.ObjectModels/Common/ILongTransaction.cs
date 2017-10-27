#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using System.Collections.Generic;

namespace OSGeo.MapGuide.ObjectModels.Common
{
    /// <summary>
    /// Represents a list of long transactions
    /// </summary>
    public interface ILongTransactionList
    {
        /// <summary>
        /// Gets the long transactions in this list
        /// </summary>
        IEnumerable<ILongTransaction> Transactions { get; }
    }

    /// <summary>
    /// Represents a long transaction
    /// </summary>
    public interface ILongTransaction
    {
        /// <summary>
        /// Gets the name of the long transaction
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the long transaction
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the owner of the long transaction
        /// </summary>
        string Owner { get; }

        /// <summary>
        /// Gets the creation date of the long transaction
        /// </summary>
        string CreationDate { get; }

        /// <summary>
        /// Gets whether the long transaction is active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets whether the long transaction is frozen
        /// </summary>
        bool IsFrozen { get; }
    }

    /// <summary>
    /// A list of FDO long transactions
    /// </summary>
    partial class FdoLongTransactionList : ILongTransactionList
    {
        /// <summary>
        /// Gets the long transactions in this list
        /// </summary>
        public IEnumerable<ILongTransaction> Transactions
        {
            get
            {
                return this.LongTransaction;
            }
        }
    }

    /// <summary>
    /// A FDO long transaction
    /// </summary>
    partial class FdoLongTransactionListLongTransaction : ILongTransaction
    {
    }
}