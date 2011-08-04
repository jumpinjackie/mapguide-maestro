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
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ExtendedObjectModels;

namespace QueryFeatureSource
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

        protected override void OnLoad(EventArgs e)
        {
            //This call is a one-time only call that will instantly register all known resource 
            //version types and validators. This way you never have to manually reference a 
            //ObjectModels assembly of the desired resource type you want to work with
            ModelSetup.Initialize();

            //Anytime we work with the Maestro API, we require an IServerConnection
            //reference. The Maestro.Login.LoginDialog provides a UI to obtain such a 
            //reference.

            //If you need to obtain an IServerConnection reference programmatically and
            //without user intervention, use the ConnectionProviderRegistry class
            var login = new Maestro.Login.LoginDialog();
            if (login.ShowDialog() == DialogResult.OK)
            {
                _conn = login.Connection;
            }
            else //This sample does not work without an IServerConnection
            {
                Application.Exit();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            grdResults.Rows.Clear();
            grdResults.Columns.Clear();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            //This method does the actual query
            IFeatureReader reader = null;

            try
            {
                //QueryFeatureSource gives us an IFeatureReader which contains the reader results
                reader = _conn.FeatureService.QueryFeatureSource(
                    txtResourceId.Text,
                    cmbFeatureClasses.Text,
                    txtFilter.Text);

                //Set up the data grid with all the known fields in this reader
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    grdResults.Columns.Add(reader.GetName(i), reader.GetName(i));
                }

                //Now loop the reader to process each result
                while (reader.ReadNext())
                {
                    object[] values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        // Property values can be fetched by index or by name.
                        //
                        // Regardless of which way you fetch these values, do not blindly 
                        // fetch the reader value 
                        //
                        // Need to check for null for each value we want to fetch. 
                        if (!reader.IsNull(i))
                            values[i] = reader[i];
                        else
                            values[i] = DBNull.Value;
                    }
                    //Add the result to the data grid
                    grdResults.Rows.Add(values);
                }
            }
            catch (Exception ex) //Could happen due to bad query parameters, or the connection went out while query is being executed and processed.
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Always close the reader when done
                if (reader != null)
                    reader.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //The ResourcePicker class, functions like a file dialog allowing the user
            //to easily select a given resource. In our case, we want the user to select
            //a Map Definition
            using (var picker = new ResourcePicker(_conn.ResourceService,
                                                   ResourceTypes.FeatureSource,
                                                   ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LoadFeatureSource(picker.ResourceID);
                }
            }
        }

        private void LoadFeatureSource(string featureSourceId)
        {
            FeatureSourceDescription desc = _conn.FeatureService.DescribeFeatureSource(featureSourceId);

            List<ClassDefinition> classes = new List<ClassDefinition>(desc.AllClasses);
            cmbFeatureClasses.DisplayMember = "QualifiedName";
            cmbFeatureClasses.DataSource = classes;

            txtResourceId.Text = featureSourceId;
        }
    }
}
