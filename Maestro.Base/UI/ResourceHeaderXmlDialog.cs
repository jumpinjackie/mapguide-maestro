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
using System.Xml.Serialization;
using System.IO;
using OSGeo.MapGuide.ObjectModels.Common;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Base.UI
{
    /// <summary>
    /// A dialog for editing a resource's header content
    /// </summary>
    internal partial class ResourceHeaderXmlDialog : Form
    {
        private ResourceHeaderXmlDialog()
        {
            InitializeComponent();
        }

        private IResourceService _resSvc;
        private string _resourceId;

        public ResourceHeaderXmlDialog(string resourceId, IResourceService resSvc)
            : this()
        {
            _resSvc = resSvc;
            _resourceId = resourceId;
            lblResourceId.Text = _resourceId;
        }

        private XmlSerializer _serializer;

        public Stream XmlStream { get; set; }

        bool isFolder;

        public void LoadResourceHeader(ResourceDocumentHeaderType docHeader)
        {
            txtXml.Text = docHeader.Serialize();
            _serializer = new XmlSerializer(typeof(ResourceDocumentHeaderType));
            isFolder = false;
        }

        public void LoadFolderHeader(ResourceFolderHeaderType folderHeader)
        {
            txtXml.Text = folderHeader.Serialize();
            _serializer = new XmlSerializer(typeof(ResourceFolderHeaderType));
            isFolder = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var obj = _serializer.Deserialize(new StringReader(txtXml.Text));

                if (isFolder)
                    _resSvc.SetFolderHeader(_resourceId, (ResourceFolderHeaderType)obj);
                else
                    _resSvc.SetResourceHeader(_resourceId, (ResourceDocumentHeaderType)obj);

                LoggingService.Info("Resource Header Updated: " + _resourceId); //NOXLATE
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.ToString());
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
