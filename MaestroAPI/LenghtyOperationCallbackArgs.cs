#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
	public delegate void LengthyOperationCallBack(object sender, LengthyOperationCallbackArgs items);

	public delegate void LengthyOperationProgressCallBack(object sender, LengthyOperationProgressArgs e);

	public class LengthyOperationProgressArgs
	{
		public string StatusMessage;
		public int Progress;
		public bool Cancel;

		public LengthyOperationProgressArgs(string message, int progress)
		{
			StatusMessage = message;
			Progress = progress;
			Cancel = false;
		}
	}


	/// <summary>
	/// Summary description for LengthyOperationCallbackArgs.
	/// </summary>
	public class LengthyOperationCallbackArgs
	{
		private LengthyOperationItem[] m_items;
		private int m_index;
		private bool m_cancel;

        
		public LengthyOperationCallbackArgs(LengthyOperationItem[] items)
		{
			m_items = items;
			m_index = 0;
			m_cancel = false;
		}

		public bool Cancel
		{
			get { return m_cancel; }
			set { m_cancel = value; }
		}

		public int Index
		{
			get { return m_index; }
			set { m_index = value; }
		}

		public LengthyOperationItem[] Items
		{
			get { return m_items; }
		}
		

		public class LengthyOperationItem
		{
			public enum OperationStatus
			{
				None,
				Pending,
				Success,
				Failure
			}

			private string m_itempath;
			private OperationStatus m_status;

			public string Itempath { get { return m_itempath; } }
			public OperationStatus Status 
			{ 
				get { return m_status; } 
				set { m_status = value; }
			}

			public LengthyOperationItem(string path)
			{
				m_itempath = path;
				m_status = OperationStatus.None;
			}
		}

	}
}
