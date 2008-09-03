#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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

namespace OSGeo.MapGuide.Maestro
{
    public partial class FormExpression : Form
    {
        private FeatureSourceDescription.FeatureSourceSchema m_schema;
        private string m_providername;
        private ServerConnectionI m_connection;

        public FormExpression()
        {
            InitializeComponent();
        }

        public string Expression
        {
            get { return ExpressionText.Text; }
            set { ExpressionText.Text = value; }
        }

        public void SetupForm(ServerConnectionI connection, FeatureSourceDescription.FeatureSourceSchema schema, string provider)
        {
            try
            {
                m_schema = schema;
                m_providername = provider;
                m_connection = connection;

                ColumnCombo.Items.Clear();
                ColumnCombo.Items.Add("< select column to insert here >");
                ColumnCombo.SelectedIndex = 0;

                /*FunctionCombo.Items.Clear();
                FunctionCombo.Items.Add("< select function to insert here >");*/

                //TODO: Perhaps add column type and indication of primary key
                foreach (FeatureSetColumn col in m_schema.Columns)
                    ColumnCombo.Items.Add(col.Name);

                //TODO: Figure out how to translate the enums into something usefull

                /*try
                {
                    /*FdoProviderCapabilities cap = m_connection.GetProviderCapabilities(m_providername);
                    foreach (FdoProviderCapabilitiesFilterType cmd in cap.Filter.Condition)
                        FunctionCombo.Items.Add(cmd.ToString());

                    FunctionLabel.Enabled = FunctionCombo.Enabled = true;
                }
                catch
                {
                    FunctionLabel.Enabled = FunctionCombo.Enabled = false;
                }*/
            }
            catch
            {
            }

        }

        private void SetupFunctions()
        {
            try
            {

                foreach (FeatureSetColumn col in m_schema.Columns)
                    ColumnCombo.Items.Add(col.Name);

            }
            catch
            {
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ColumnCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColumnCombo.SelectedIndex > 0)
            {
                ExpressionText.SelectedText = ColumnCombo.Text;
                ColumnCombo.SelectedIndex = 0;
            }
        }

    }
}