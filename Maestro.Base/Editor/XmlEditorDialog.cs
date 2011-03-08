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
using Maestro.Editors.Generic;
using System.IO;
using OSGeo.MapGuide.MaestroAPI;
using System.Xml;
using Maestro.Editors;

namespace Maestro.Base.Editor
{
    public partial class XmlEditorDialog : Form
    {
        private XmlEditorCtrl _ed;
        private IEditorService _edSvc;

        internal XmlEditorDialog()
        {
            InitializeComponent();
            _ed = new XmlEditorCtrl();
            _ed.Validator = new XmlValidationCallback(ValidateXml);
            _ed.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(_ed);
        }

        public XmlEditorDialog(IEditorService edsvc)
            : this()
        {
            _edSvc = edsvc;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_edSvc != null)
                _ed.InitResourceData(_edSvc);
        }

        public ResourceTypes ResourceType
        {
            get;
            private set;
        }

        private void ValidateXml(out string[] errors, out string[] warnings)
        {
            errors = new string[0];
            warnings = new string[0];

            List<string> err = new List<string>();
            List<string> warn = new List<string>();

            var res = this.ResourceType;

            //Test for well-formedness
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.XmlContent);
            }
            catch (XmlException ex)
            {
                err.Add(ex.Message);
            }

            //Test that this is serializable
            try
            {
                if (_enableResourceTypeValidation)
                {
                    //Test by simply attempting to deserialize the current xml content
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(this.XmlContent)))
                    {
                        //Use original resource type to determine how to deserialize
                        var obj = ResourceTypeRegistry.Deserialize(this.ResourceType, ms);
                    }
                }
            }
            catch (Exception ex)
            {
                err.Add(ex.Message);
            }

            errors = err.ToArray();
            warnings = warn.ToArray();
        }

        private bool _enableResourceTypeValidation = false;

        public void SetXmlContent(string xml, ResourceTypes type)
        {
            _ed.XmlContent = xml;
            this.ResourceType = type;
            _enableResourceTypeValidation = true;
        }

        /// <summary>
        /// Gets or sets the XML content for this dialog.
        /// </summary>
        public string XmlContent
        {
            get { return _ed.XmlContent; }
            set { _ed.XmlContent = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}