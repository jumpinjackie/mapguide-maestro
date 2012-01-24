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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Maestro.Shared.UI;
using System.IO;

namespace Maestro.AddIn.GeoRest.UI
{
    public partial class TextEditor : Form
    {
        private string _origTitle;

        public TextEditor()
        {
            InitializeComponent();
            _origTitle = this.Text;
        }

        private string _file;

        public void LoadFile(string file)
        {
            _file = file;
            txtContent.Text = File.ReadAllText(file);
            this.Text = _origTitle + " - " + file;
        }

        private void txtXml_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText(_file, txtContent.Text);
            MessageBox.Show(string.Format(Properties.Resources.FileSaved, _file));
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(txtContent.Text);
                MessageBox.Show(Properties.Resources.XmlWellFormed);
            }
            catch (XmlException ex)
            {
                ErrorDialog.Show(ex);
            }
        }
    }
}
