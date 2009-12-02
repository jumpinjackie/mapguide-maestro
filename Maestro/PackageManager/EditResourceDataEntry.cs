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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.PackageManager
{
    public partial class EditResourceDataEntry : Form
    {
        private string m_resourceName;
        private string m_contentType;
        private string m_filename;
        private string m_datatype;

        public string ResourceName
        {
            get { return m_resourceName; }
            set { m_resourceName = value; }
        }

        public string ContentType
        {
            get { return m_contentType; }
            set { m_contentType = value; }
        }

        public string Filename
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        public string DataType
        {
            get { return m_datatype; }
            set { m_datatype = value; }
        }

        public EditResourceDataEntry()
        {
            InitializeComponent();
        }

        private void EditResourceDataEntry_Load(object sender, EventArgs e)
        {
            ResourceNameBox.Text = m_resourceName;
            ContentTypeBox.Text = m_contentType;
            DataTypeBox.SelectedIndex = DataTypeBox.FindString(m_datatype);
            FilenameBox.Text = m_filename;

        }

        private void ValidateForm(object sender, EventArgs e)
        {
            OKBtn.Enabled =
                !string.IsNullOrEmpty(ResourceNameBox.Text) &&
                !string.IsNullOrEmpty(ContentTypeBox.Text) &&
                !string.IsNullOrEmpty(DataTypeBox.Text);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            m_resourceName = ResourceNameBox.Text;
            m_contentType = ContentTypeBox.Text;
            m_datatype = DataTypeBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }



    }
}