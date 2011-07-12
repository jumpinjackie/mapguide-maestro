#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Commands;
using System.IO;

namespace Maestro.Editors.Diagnostics
{
    /// <summary>
    /// Provides a simple user interface to display the results of a <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IGetFdoCacheInfo"/> command
    /// </summary>
    public partial class FdoCacheViewer : Form
    {
        private IGetFdoCacheInfo _cmd;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        public FdoCacheViewer(IGetFdoCacheInfo cache)
        {
            InitializeComponent();
            _cmd = cache;
            txtXml.Text = ToXml(_cmd.Execute());
        }

        private string ToXml(FdoCacheInfo fdoCacheInfo)
        {
            using (var ms = new MemoryStream())
            {
                FdoCacheInfo.Serializer.Serialize(ms, fdoCacheInfo);
                return Encoding.UTF8.GetString(ms.GetBuffer());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            tmRefresh.Enabled = chkAutoRefresh.Checked;
        }

        private void tmRefresh_Tick(object sender, EventArgs e)
        {
            txtXml.Text = ToXml(_cmd.Execute());
        }
    }
}
