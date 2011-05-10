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
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace SchemaViewer
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            //The ResourcePicker class, functions like a file dialog allowing the user
            //to easily select a given resource. In our case, we want the user to select
            //a Feature Source
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
            var desc = _conn.FeatureService.DescribeFeatureSource(featureSourceId);
            treeView1.Nodes.Clear();
            foreach (FeatureSchema schema in desc.Schemas)
            {
                var node = CreateSchemaNode(schema);
                treeView1.Nodes.Add(node);
            }
        }

        private TreeNode CreateSchemaNode(FeatureSchema schema)
        {
            var node = new TreeNode();
            node.Name = schema.Name;
            node.Text = schema.Name;

            foreach (ClassDefinition cls in schema.Classes)
            {
                var clsNode = CreateClassNode(cls);
                node.Nodes.Add(clsNode);
            }

            node.Tag = schema;

            return node;
        }

        private TreeNode CreateClassNode(ClassDefinition cls)
        {
            var node = new TreeNode();
            node.Name = cls.Name;
            node.Text = cls.Name;

            foreach (PropertyDefinition prop in cls.Properties)
            {
                var pNode = new TreeNode();
                pNode.Name = prop.Name;
                pNode.Text = prop.Name;

                pNode.Tag = prop;

                node.Nodes.Add(pNode);
            }

            node.Tag = cls;
            return node;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }
    }
}
