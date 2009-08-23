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
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for TreeViewUtil.
	/// </summary>
	public class TreeViewUtil
	{
		public static TreeNode FindItemExact(TreeView tree, string fullpath)
		{
			TreeNode n = FindItem(tree, fullpath);
			if (n == null || n.FullPath != fullpath)
				return null;
			else
				return n;
		}

		public static TreeNode FindItem(TreeView tree, string fullpath)
		{
			TreeNodeCollection nodes = tree.Nodes;
			TreeNode node = null;

			foreach(string s in fullpath.Split(tree.PathSeparator[0]))
			{
				bool found = false;
				foreach(TreeNode n in nodes)
					if (n.Text == s)
					{
						found = true;
						nodes = n.Nodes;
						node = n;
						break;
					}
				if (!found)
					break;
			}

			return node;
		}

	}
}
