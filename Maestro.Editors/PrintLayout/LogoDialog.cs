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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Common;

namespace Maestro.Editors.PrintLayout
{
    internal partial class LogoDialog : Form
    {
        private LogoDialog()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

        public LogoDialog(IServerConnection conn)
            : this()
        {
            //TOOD: Maybe be more graceful? Like allow text entry, but disable browsing buttons?
            if (Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Drawing) < 0)
            {
                throw new InvalidOperationException(Strings.RequiredServiceNotSupported + ServiceType.Drawing.ToString());
            }
            _conn = conn;
        }

        public string SymbolLibraryID
        {
            get { return txtSymbolLibraryId.Text; }
            set { txtSymbolLibraryId.Text = value; }
        }

        public string SymbolName
        {
            get { return txtSymbolName.Text; }
            set { txtSymbolName.Text = value; }
        }

        public float PositionLeft
        {
            get { return Convert.ToSingle(numPosLeft.Value); }
            set { numPosLeft.Value = Convert.ToDecimal(value); }
        }

        public float PositionBottom
        {
            get { return Convert.ToSingle(numPosBottom.Value); }
            set { numPosBottom.Value = Convert.ToDecimal(value); }
        }

        public string PositionUnits
        {
            get { return cmbPositionUnits.Text; }
            set { cmbPositionUnits.Text = value; }
        }

        public float SizeWidth
        {
            get { return Convert.ToSingle(numSizeWidth.Value); }
            set { numSizeWidth.Value = Convert.ToDecimal(value); }
        }

        public float SizeHeight
        {
            get { return Convert.ToSingle(numSizeHeight.Value); }
            set { numSizeHeight.Value = Convert.ToDecimal(value); }
        }

        public float Rotation
        {
            get { return Convert.ToSingle(numRotation.Value); }
            set { numRotation.Value = Convert.ToDecimal(value); }
        }

        public string SizeUnits
        {
            get { return cmbSizeUnits.Text; }
            set { cmbSizeUnits.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnBrowseSymbolLibrary_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourceTypes.SymbolLibrary, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    this.SymbolLibraryID = picker.ResourceID;
                    this.SymbolName = string.Empty;
                }
            }
        }

        private void btnBrowseSymbolName_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.SymbolLibraryID))
            {
                MessageBox.Show(Strings.SelectSymbolLibraryFirst);
                return;
            }

            using (var picker = new SymbolPicker(this.SymbolLibraryID, _conn))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    this.SymbolName = picker.SymbolName;
                }
            }
        }
    }
}
