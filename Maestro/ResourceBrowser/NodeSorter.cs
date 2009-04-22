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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceBrowser
{
    /// <summary>
    /// Sorting of nodes, based on resource type and name
    /// </summary>
    public class DocumentSorter : IComparer<ResourceListResourceDocument>
    {
        #region IComparer<ResourceListResourceDocument> Members

        public int Compare(ResourceListResourceDocument x, ResourceListResourceDocument y)
        {
            if (x != null && y != null)
            {
                string extX = x.ResourceId.Substring(x.ResourceId.LastIndexOf("."));
                string extY = y.ResourceId.Substring(y.ResourceId.LastIndexOf("."));

                if (extX != extY)
                    return string.Compare(extX, extY, false, System.Globalization.CultureInfo.CurrentUICulture);
                else
                    return string.Compare(x.ResourceId, y.ResourceId, false, System.Globalization.CultureInfo.CurrentUICulture);
            }
            else if (x != null)
                return -1;
            else if (y != null)
                return 1;
            else
                return 0; //Don't know...
        }

        #endregion
    }

    /// <summary>
    /// Sorting of treenodes, based on resource type and name
    /// </summary>
    public class NodeSorter : System.Collections.IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            //Determine both operands are treenodes
            TreeNode nx = x as TreeNode;
            TreeNode ny = y as TreeNode;

            if (nx == null && ny == null)
                return 0;
            else if (nx == null)
                return -1;
            else if (ny == null)
                return 1;

            //Determine if any or both are folders
            ResourceListResourceFolder fx = nx.Tag as ResourceListResourceFolder;
            ResourceListResourceFolder fy = ny.Tag as ResourceListResourceFolder;
            
            //Folders must be before documents
            if (fx != null && fy != null)
                return string.Compare(fx.ResourceId, fy.ResourceId, false, System.Globalization.CultureInfo.CurrentUICulture);
            else if (fx != null)
                return -1;
            else if (fy != null)
                return 1;


            ResourceListResourceDocument dx = nx.Tag as ResourceListResourceDocument;
            ResourceListResourceDocument dy = ny.Tag as ResourceListResourceDocument;

            if (dx != null && dy != null)
            {
                //Sort on extension
                string extX = dx.ResourceId.Substring(dx.ResourceId.LastIndexOf("."));
                string extY = dy.ResourceId.Substring(dy.ResourceId.LastIndexOf("."));

                if (extX != extY)
                    return string.Compare(extX, extY, false, System.Globalization.CultureInfo.CurrentUICulture);
                else //Same extension, sort on name
                    return string.Compare(dx.ResourceId, dy.ResourceId, false, System.Globalization.CultureInfo.CurrentUICulture);
            }
            else if (dx != null)
                return -1;
            else if (dy != null)
                return 1;
            else
                return 0; //Don't know...
        }

        #endregion
    }
}
