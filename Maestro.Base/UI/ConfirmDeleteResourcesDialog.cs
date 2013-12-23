#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    /// <summary>
    /// A dialog that is used to provide a detailed confirmation prompt when deleting a resource
    /// with dependent resources.
    /// </summary>
    public partial class ConfirmDeleteResourcesDialog : Form
    {
        private ConfirmDeleteResourcesDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the ConfirmDeleteResourcesDialog class
        /// </summary>
        /// <param name="dependents"></param>
        public ConfirmDeleteResourcesDialog(IEnumerable<Editors.RepositoryHandle> dependents)
            : this()
        {
            var connNames = dependents.Select(x => x.Connection.DisplayName).Distinct();
            foreach (var name in connNames)
            {
                var node = trvDependents.Nodes.Add(name);
                foreach (var res in dependents.Where(x => x.Connection.DisplayName == name))
                {
                    node.Nodes.Add(res.ResourceId);
                }
                node.Text += " (" + node.Nodes.Count + ")"; //NOXLATE
            }
        }

        internal static bool AskQuestion(IEnumerable<Editors.RepositoryHandle> dependentResourceIds)
        {
            var diag = new ConfirmDeleteResourcesDialog(dependentResourceIds);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            return false;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
