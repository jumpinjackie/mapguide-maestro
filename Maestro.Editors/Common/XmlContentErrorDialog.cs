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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A specialized error dialog for XML content errors
    /// </summary>
    public partial class XmlContentErrorDialog : Form
    {
        private XmlContentErrorDialog()
        {
            InitializeComponent();
            txtXmlContent.SetHighlighting("XML"); //NOXLATE
        }

        /// <summary>
        /// Checks if the given exception is XML related and if so will either
        /// display the error dialog, or attach the 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="origXml"></param>
        /// <param name="bDisplay"></param>
        public static void CheckAndHandle(Exception ex, string origXml, bool bDisplay)
        {
            if (IsDbXmlError(ex))
            {
                ex.Data[Utility.XML_EXCEPTION_KEY] = origXml;
                if (bDisplay)
                    Show(ex);
                else
                    throw ex;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets whether the thrown exception is related to DBXML
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool IsDbXmlError(Exception ex)
        {
            return Utility.IsDbXmlError(ex);
        }

        /// <summary>
        /// Gets whether the given exception has original xml content attached
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool HasOriginalXml(Exception ex)
        {
            return Utility.HasOriginalXml(ex);
        }

        private Exception _ex;

        /// <summary>
        /// Displays this dialog
        /// </summary>
        /// <param name="ex"></param>
        public static void Show(Exception ex)
        {
            Check.NotNull(ex, "ex"); //NOXLATE
            Check.NotNull(ex.Data[Utility.XML_EXCEPTION_KEY], "ex.Data[Utility.XML_EXCEPTION_KEY]"); //NOXLATE
            string origXmlContent = ex.Data[Utility.XML_EXCEPTION_KEY].ToString();
            var diag = new XmlContentErrorDialog();
            diag._ex = ex;
            diag.txtErrorDetails.Text = ex.ToString();
            diag.txtXmlContent.Text = origXmlContent;
            diag.txtXmlContent.IsReadOnly = true;
            diag.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var diag = DialogFactory.SaveFile())
            {
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var sw = new StreamWriter(diag.FileName, false))
                    {
                        sw.WriteLine("========== ERROR DETAILS =========\n"); //NOXLATE
                        sw.WriteLine(_ex.ToString());
                        sw.WriteLine("\n========== XML CONTENT ==========\n\n"); //NOXLATE
                        sw.WriteLine(txtXmlContent.Text);
                        sw.Close();
                    }
                }
            }
        }
    }
}
