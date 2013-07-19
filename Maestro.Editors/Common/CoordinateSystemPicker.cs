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
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A coordinate system picker dialog
    /// </summary>
    public partial class CoordinateSystemPicker : Form
    {
        private CoordinateSystemPicker()
        {
            InitializeComponent();
        }

        private CoordinateSystemDefinitionBase m_wktCoordSys = null;
        private CoordinateSystemDefinitionBase m_epsgCoordSys = null;
        private CoordinateSystemDefinitionBase m_coordsysCodeCoordSys = null;
        private CoordinateSystemDefinitionBase m_selectedCoordsys = null;

        private bool m_isUpdating = false;

        private ICoordinateSystemCatalog _cat;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemPicker"/> class.
        /// </summary>
        /// <param name="cat">The cat.</param>
        public CoordinateSystemPicker(ICoordinateSystemCatalog cat) : this()
        {
            if (cat == null)
            {
                SelectByList.Enabled = SelectByCoordSysCode.Enabled = SelectByEPSGCode.Enabled = ValidateWKT.Enabled = false;

                SelectByWKT.Enabled = SelectByWKT.Checked = true;
            }
            else
            {
                CoordinateCategory.Items.Clear();
                CoordinateCategory.Items.AddRange(cat.Categories);
            }

            _cat = cat;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            this.Visible = true;
            CoordinateWait.Visible = true;
            CoordinateWait.BringToFront();
            this.Refresh();
            _cat.FindCoordSys(""); //NOXLATE

            CoordinateSystemDefinitionBase[] items = null;
            try
            {
                items = _cat.Coordsys;
            }
            catch
            {
                items = new CoordinateSystemDefinitionBase[0];
            }

            EPSGCodeText.BeginUpdate();
            try
            {
                EPSGCodeText.Items.Clear();
                foreach (CoordinateSystemDefinitionBase c in items)
                {
                    if (c.Code.StartsWith("EPSG:")) //NOXLATE
                        EPSGCodeText.Items.Add(c.EPSG);
                }
            }
            finally
            {
                EPSGCodeText.EndUpdate();
            }

            CoordSysCodeText.BeginUpdate();
            try
            {
                CoordSysCodeText.Items.Clear();
                foreach (CoordinateSystemDefinitionBase c in items)
                {
                    CoordSysCodeText.Items.Add(c.Code);
                }
            }
            finally
            {
                CoordSysCodeText.EndUpdate();
            }

            if (WKTText.Text != "") //NOXLATE
            {
                SelectByWKT.Checked = true;
                ValidateWKT_Click(null, null);
            }

            CoordinateWait.Visible = false;
        }

        private void SelectByList_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAfterRadioButtons();
        }

        private void SelectByWKT_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAfterRadioButtons();
        }

        private void SelectByCoordSysCode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAfterRadioButtons();
        }

        private void SelectByEPSGCode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAfterRadioButtons();
        }

        private void ValidateWKT_Click(object sender, EventArgs e)
        {
            try
            {
                m_wktCoordSys = null;
                if (_cat.IsValid(WKTText.Text))
                {
                    try
                    {
                        string coordcode = _cat.ConvertWktToCoordinateSystemCode(WKTText.Text);
                        m_wktCoordSys = _cat.FindCoordSys(coordcode);
                    }
                    catch
                    {
                    }

                    if (m_wktCoordSys == null)
                    {
                        m_wktCoordSys = _cat.CreateEmptyCoordinateSystem();
                        m_wktCoordSys.Code = null;
                        m_wktCoordSys.Description = null;
                        m_wktCoordSys.WKT = WKTText.Text;
                    }
                }
                else
                {
                    if (MessageBox.Show(Strings.ConfirmNonMapGuideSupportedCsWkt, Strings.NonMapGuideSupportedCsWkt, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        m_wktCoordSys = _cat.CreateEmptyCoordinateSystem();
                        m_wktCoordSys.Code = null;
                        m_wktCoordSys.Description = null;
                        m_wktCoordSys.WKT = WKTText.Text;
                    }
                }
            }
            catch
            {
                if (MessageBox.Show(Strings.ConfirmNonMapGuideSupportedCsWkt, Strings.NonMapGuideSupportedCsWkt, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    m_wktCoordSys = _cat.CreateEmptyCoordinateSystem();
                    m_wktCoordSys.Code = null;
                    m_wktCoordSys.Description = null;
                    m_wktCoordSys.WKT = WKTText.Text;
                }
            }
            UpdateOKButton();
        }

        private void ValidateCoordSysCode_Click(object sender, EventArgs e)
        {
            try
            {
                m_coordsysCodeCoordSys = null;
                string s = _cat.ConvertCoordinateSystemCodeToWkt(CoordSysCodeText.Text);
                m_coordsysCodeCoordSys = _cat.FindCoordSys(CoordSysCodeText.Text);
            }
            catch
            {
                
            }
            UpdateOKButton();
        }

        private void ValidateEPSG_Click(object sender, EventArgs e)
        {
            try
            {
                m_epsgCoordSys = null;
                m_epsgCoordSys = _cat.FindCoordSys("EPSG:" + EPSGCodeText.Text); //NOXLATE
                if (m_epsgCoordSys == null)
                {
                    string s = _cat.ConvertEpsgCodeToWkt(EPSGCodeText.Text);
                    s = _cat.ConvertWktToCoordinateSystemCode(s);
                    m_epsgCoordSys = _cat.FindCoordSys(s);
                }
            }
            catch
            {
            }
            UpdateOKButton();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (SelectByList.Checked)
                m_selectedCoordsys = CoordinateSystem.SelectedItem as CoordinateSystemDefinitionBase;
            else if (SelectByCoordSysCode.Checked)
                m_selectedCoordsys = m_coordsysCodeCoordSys;
            else if (SelectByWKT.Checked && _cat == null)
            {
                m_selectedCoordsys = _cat.CreateEmptyCoordinateSystem();
                m_selectedCoordsys.Code = null;
                m_selectedCoordsys.Description = null;
                m_selectedCoordsys.WKT = WKTText.Text;
            }
            else if (SelectByWKT.Checked)
                m_selectedCoordsys = m_wktCoordSys;
            else if (SelectByEPSGCode.Checked)
                m_selectedCoordsys = m_epsgCoordSys;
            else
                m_selectedCoordsys = null;
        }

        /// <summary>
        /// Gets the selected coord sys.
        /// </summary>
        /// <value>The selected coord sys.</value>
        public CoordinateSystemDefinitionBase SelectedCoordSys
        {
            get { return m_selectedCoordsys; }
        }

        private void WKTText_TextChanged(object sender, System.EventArgs e)
        {
            if (m_isUpdating)
                return;

            //			if (!WKTText.Focused)
            //				return;
            m_wktCoordSys = null;
            if (WKTText.Focused)
                UpdateOKButton();
        }

        private void CoordSysCodeText_TextChanged(object sender, System.EventArgs e)
        {
            if (m_isUpdating)
                return;

            //			if (!CoordSysCodeText.Focused)
            //				return;
            m_coordsysCodeCoordSys = null;
            if (CoordSysCodeText.Focused)
                UpdateOKButton();
        }

        private void EPSGCodeText_TextChanged(object sender, System.EventArgs e)
        {
            if (m_isUpdating)
                return;

            //			if (!EPSGCodeText.Focused)
            //				return;
            m_epsgCoordSys = null;
            if (EPSGCodeText.Focused)
                UpdateOKButton();
        }

        private void CoordinateSystem_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_isUpdating)
                return;

            UpdateOKButton();

        }

        private void UpdateAfterRadioButtons()
        {
            SelectByListGroup.Enabled = SelectByList.Checked;
            SelectByWKTGroup.Enabled = SelectByWKT.Checked;
            SelectByCoordSysCodeGroup.Enabled = SelectByCoordSysCode.Checked;
            SelectByEPSGCodeGroup.Enabled = SelectByEPSGCode.Checked;

            UpdateOKButton();

        }

        private void UpdateOKButton()
        {
            UpdateOthers();
            if (SelectByList.Checked)
            {
                if (CoordinateCategory.SelectedIndex >= 0 && CoordinateSystem.SelectedIndex >= 0)
                    OKBtn.Enabled = true;
            }
            else if (_cat == null)
                OKBtn.Enabled = true;
            else if (SelectByWKT.Checked)
                OKBtn.Enabled = m_wktCoordSys != null;
            else if (SelectByCoordSysCode.Checked)
                OKBtn.Enabled = m_coordsysCodeCoordSys != null;
            else if (SelectByEPSGCode.Checked)
                OKBtn.Enabled = m_epsgCoordSys != null;
            else
                OKBtn.Enabled = false;
        }

        private void UpdateOthers()
        {
            CoordinateSystemDefinitionBase selectedCoordsys;
            if (SelectByList.Checked)
                selectedCoordsys = CoordinateSystem.SelectedItem as CoordinateSystemDefinitionBase;
            else if (SelectByCoordSysCode.Checked)
                selectedCoordsys = m_coordsysCodeCoordSys;
            else if (SelectByWKT.Checked)
                selectedCoordsys = m_wktCoordSys;
            else if (SelectByEPSGCode.Checked)
                selectedCoordsys = m_epsgCoordSys;
            else
                selectedCoordsys = null;

            try
            {
                m_isUpdating = true;
                if (!SelectByList.Checked)
                    try { CoordinateSystem.SelectedItem = selectedCoordsys; }
                    catch { }
                if (!SelectByCoordSysCode.Checked)
                    try { CoordSysCodeText.Text = selectedCoordsys == null ? "" : selectedCoordsys.Code; } //NOXLATE
                    catch { }
                if (!SelectByWKT.Checked)
                    try { WKTText.Text = selectedCoordsys == null ? "" : selectedCoordsys.WKT; } //NOXLATE
                    catch { }
                if (!SelectByEPSGCode.Checked)
                    try { EPSGCodeText.Text = selectedCoordsys.EPSG; }
                    catch { }
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private void CoordinateCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            CoordinateSystem.Enabled = CoordinateSystemLabel.Enabled = CoordinateCategory.SelectedIndex >= 0;
            if (CoordinateCategory.SelectedIndex >= 0)
            {
                CoordinateSystemCategory cat = CoordinateCategory.SelectedItem as CoordinateSystemCategory;
                if (cat == null)
                {
                    OKBtn.Enabled = false;
                    return;
                }

                CoordinateSystem.Items.Clear();
                CoordinateSystem.Items.AddRange(cat.Items);
            }
        }
    }
}
