using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    partial class FdoLongTransactionListLongTransaction : ILongTransaction
    {

    }
}
