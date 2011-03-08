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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Generic
{
    /// <summary>
    /// Dialog to show the results of a validation
    /// </summary>
    internal partial class XmlValidationResult : Form
    {
        class MessageItem
        {
            public Image Icon { get; set; }
            public string Message { get; set; }
        }

        private XmlValidationResult()
        {
            InitializeComponent();
        }

        public XmlValidationResult(IEnumerable<string> errors, IEnumerable<string> warnings)
            : this()
        {
            List<MessageItem> items = new List<MessageItem>();
            foreach (var err in errors)
            {
                items.Add(new MessageItem { Icon = Properties.Resources.cross, Message = err });
            }
            foreach (var warn in warnings)
            {
                items.Add(new MessageItem { Icon = Properties.Resources.exclamation, Message = warn });
            }

            grdMessages.DataSource = items;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
