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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace Maestro.Base.UI
{
    public partial class BoundsPicker : Form
    {
        private string m_bounds;
        public string SRSBounds
        {
            get { return m_bounds; }
        }

        public BoundsPicker(string bounds, string[] coordsys)
            : this()
        {
            m_bounds = bounds;

            if (coordsys == null)
            {
                SRSLabel.Visible =
                SRSCombo.Visible =
                    false;
                this.Height -= 30;
            }
            else
                SRSCombo.Items.AddRange(coordsys);

            if (!string.IsNullOrEmpty(bounds))
            {
                try
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    if (bounds.Trim().StartsWith("&lt;"))
                        bounds = System.Web.HttpUtility.HtmlDecode(bounds);
                    bounds = "<root>" + bounds + "</root>";
                    doc.LoadXml(bounds);
                    System.Xml.XmlNode root = doc["root"];
                    if (root["Bounds"] != null)
                    {
                        if (root["Bounds"].Attributes["SRS"] != null)
                            SRSCombo.Text = root["Bounds"].Attributes["SRS"].Value;

                        if (root["Bounds"].Attributes["west"] != null)
                            MinX.Text = root["Bounds"].Attributes["west"].Value;
                        if (root["Bounds"].Attributes["east"] != null)
                            MaxX.Text = root["Bounds"].Attributes["east"].Value;

                        if (root["Bounds"].Attributes["south"] != null)
                            MinY.Text = root["Bounds"].Attributes["south"].Value;
                        if (root["Bounds"].Attributes["north"] != null)
                            MaxY.Text = root["Bounds"].Attributes["north"].Value;
                    }
                    else
                        throw new Exception(Properties.Resources.BoundsPicker_MissingBoundsError);
                }
                catch(Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    MessageBox.Show(this, string.Format(Properties.Resources.BoundsPicker_BoundsDecodeError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
                }
            }
        }

        private BoundsPicker()
        {
            InitializeComponent();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            string srs;
            if (SRSCombo.Text.Trim().Length == 0 || SRSCombo.Visible == false)
                srs = null;
            else
                srs = SRSCombo.Text;

            if (MinX.Text.Trim().Length == 0 || MaxX.Text.Trim().Length == 0 || MinY.Text.Trim().Length == 0 || MaxY.Text.Trim().Length == 0 || (srs == null && SRSCombo.Visible))
            {
                MessageBox.Show(this, Properties.Resources.BoundsPicker_IncompleBoundsError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double temp;
            System.Globalization.CultureInfo localCI = System.Globalization.CultureInfo.CurrentUICulture;
            System.Globalization.CultureInfo usCI = System.Globalization.CultureInfo.InvariantCulture;
            
            bool isUs = double.TryParse(MinX.Text, System.Globalization.NumberStyles.Float, usCI, out temp) &&
                double.TryParse(MaxX.Text, System.Globalization.NumberStyles.Float, usCI, out temp) &&
                double.TryParse(MinY.Text, System.Globalization.NumberStyles.Float, usCI, out temp) &&
                double.TryParse(MaxY.Text, System.Globalization.NumberStyles.Float, usCI, out temp);

            bool isLocal = double.TryParse(MinX.Text, System.Globalization.NumberStyles.Float, localCI, out temp) &&
                double.TryParse(MaxX.Text, System.Globalization.NumberStyles.Float, localCI, out temp) &&
                double.TryParse(MinY.Text, System.Globalization.NumberStyles.Float, localCI, out temp) &&
                double.TryParse(MaxY.Text, System.Globalization.NumberStyles.Float, localCI, out temp);

            if (!isUs && isLocal)
            {
                switch (MessageBox.Show(this, Properties.Resources.BoundsPicker_NumbersInRegionalError, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        if (double.TryParse(MinX.Text, System.Globalization.NumberStyles.Float, localCI, out temp))
                            MinX.Text = temp.ToString(usCI);
                        else
                        {
                            MessageBox.Show(this, Properties.Resources.BoundsPicker_InvalidMinXError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (double.TryParse(MaxX.Text, System.Globalization.NumberStyles.Float, localCI, out temp))
                            MaxX.Text = temp.ToString(usCI);
                        else
                        {
                            MessageBox.Show(this, Properties.Resources.BoundsPicker_InvalidMaxXError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (double.TryParse(MinY.Text, System.Globalization.NumberStyles.Float, localCI, out temp))
                            MinY.Text = temp.ToString(usCI);
                        else
                        {
                            MessageBox.Show(this, Properties.Resources.BoundsPicker_InvalidMinYError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (double.TryParse(MaxY.Text, System.Globalization.NumberStyles.Float, localCI, out temp))
                            MaxY.Text = temp.ToString(usCI);
                        else
                        {
                            MessageBox.Show(this, Properties.Resources.BoundsPicker_InvalidMaxYError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }

            if (!isUs && !isLocal)
            {
                if (MessageBox.Show(this, Properties.Resources.BoundsPicker_UseInvalidCoordinatesWarning, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error) != DialogResult.Yes)
                    return;
            }

            m_bounds = "<Bounds west=\"" + MinX.Text + "\" east=\"" + MaxX.Text + "\" south=\"" + MinY.Text + "\" north=\"" + MaxY.Text + "\" ";
            if (srs != null)
            {
                m_bounds += " SRS=\"" + srs + "\"";
            }
            m_bounds += " />";
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}