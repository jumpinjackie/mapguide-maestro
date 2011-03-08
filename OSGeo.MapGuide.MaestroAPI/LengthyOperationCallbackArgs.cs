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

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// A delegate used to represent relay progress for lengthy operations
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="items"></param>
	public delegate void LengthyOperationCallBack(object sender, LengthyOperationCallbackArgs items);

    /// <summary>
    /// A delegate used to represent relay progress for lengthy operations
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	public delegate void LengthyOperationProgressCallBack(object sender, LengthyOperationProgressArgs e);

    /// <summary>
    /// Represents progress of a lengthy operation
    /// </summary>
	public class LengthyOperationProgressArgs
	{
        /// <summary>
        /// The message
        /// </summary>
		public string StatusMessage;

        /// <summary>
        /// The progress percentage
        /// </summary>
		public int Progress;

        /// <summary>
        /// Indicates whether a cancel request has been made
        /// </summary>
		public bool Cancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthyOperationProgressArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="progress">The progress.</param>
		public LengthyOperationProgressArgs(string message, int progress)
		{
			StatusMessage = message;
			Progress = progress;
			Cancel = false;
		}
	}


	/// <summary>
    /// Represents progress of a lengthy operation
	/// </summary>
	public class LengthyOperationCallbackArgs
	{
		private LengthyOperationItem[] m_items;
		private int m_index;
		private bool m_cancel;


        /// <summary>
        /// Initializes a new instance of the <see cref="LengthyOperationCallbackArgs"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
		public LengthyOperationCallbackArgs(LengthyOperationItem[] items)
		{
			m_items = items;
			m_index = 0;
			m_cancel = false;
		}

        /// <summary>
        /// Gets or sets whether this lengthy operation should be cancelled
        /// </summary>
		public bool Cancel
		{
			get { return m_cancel; }
			set { m_cancel = value; }
		}
        
        /// <summary>
        /// Gets or sets the index
        /// </summary>
		public int Index
		{
			get { return m_index; }
			set { m_index = value; }
		}

        /// <summary>
        /// Gets the operation items
        /// </summary>
		public LengthyOperationItem[] Items
		{
			get { return m_items; }
		}


        /// <summary>
        /// 
        /// </summary>
		public class LengthyOperationItem
		{
            /// <summary>
            /// Defines the possible operation status values
            /// </summary>
			public enum OperationStatus
			{
                /// <summary>
                /// None
                /// </summary>
				None,
                /// <summary>
                /// Pending
                /// </summary>
				Pending,
                /// <summary>
                /// Success
                /// </summary>
				Success,
                /// <summary>
                /// Failure
                /// </summary>
				Failure
			}

			private string m_itempath;
			private OperationStatus m_status;

            /// <summary>
            /// Gets the item path
            /// </summary>
			public string Itempath { get { return m_itempath; } }

            /// <summary>
            /// Gets or sets the operation status
            /// </summary>
			public OperationStatus Status 
			{ 
				get { return m_status; } 
				set { m_status = value; }
			}

            /// <summary>
            /// Initializes a new instance of the <see cref="LengthyOperationItem"/> class.
            /// </summary>
            /// <param name="path">The path.</param>
			public LengthyOperationItem(string path)
			{
				m_itempath = path;
				m_status = OperationStatus.None;
			}
		}

	}
}
