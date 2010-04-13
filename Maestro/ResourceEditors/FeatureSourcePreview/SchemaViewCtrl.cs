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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview
{
    public partial class SchemaViewCtrl : UserControl
    {
        public bool SqlSupported
        {
            get { return btnSqlQuery.Enabled; }
            set { btnSqlQuery.Enabled = value; }
        }

        public delegate void PreviewClassEventHandler(string className);

        public event PreviewClassEventHandler OnRequestPreviewClass;

        public event EventHandler RequestRefresh;

        public event EventHandler RequestSqlQuery;

        public SchemaViewCtrl()
        {
            InitializeComponent();
        }

        private FeatureSourceDescription _schemas;

        public FeatureSourceDescription Schemas
        {
            get { return _schemas; }
            set
            {
                _schemas = value;
                trvSchema.Nodes.Clear();

                Dictionary<string, List<FeatureSourceDescription.FeatureSourceSchema>> schemas = new Dictionary<string, List<FeatureSourceDescription.FeatureSourceSchema>>();

                foreach (FeatureSourceDescription.FeatureSourceSchema sc in _schemas.Schemas)
                {
                    if (!schemas.ContainsKey(sc.Schema))
                        schemas[sc.Schema] = new List<FeatureSourceDescription.FeatureSourceSchema>();

                    schemas[sc.Schema].Add(sc);
                }

                foreach (string schemaName in schemas.Keys)
                {
                    TreeNode schemaNode = new TreeNode(schemaName);

                    foreach (FeatureSourceDescription.FeatureSourceSchema classDef in schemas[schemaName])
                    {
                        TreeNode classNode = new TreeNode(classDef.Name);
                        classNode.Tag = classDef.Fullname;
                        schemaNode.Nodes.Add(classNode);

                        foreach (FeatureSetColumn propDef in classDef.Columns)
                        {
                            TreeNode propNode = new TreeNode(propDef.Name);
                            classNode.Nodes.Add(propNode);
                        }
                    }

                    trvSchema.Nodes.Add(schemaNode);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var handler = this.RequestRefresh;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void btnSqlQuery_Click(object sender, EventArgs e)
        {
            var handler = this.RequestSqlQuery;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void btnStdQuery_Click(object sender, EventArgs e)
        {
            var node = trvSchema.SelectedNode;
            if (node.Level == 1)
            {
                var handler = this.OnRequestPreviewClass;
                if (handler != null)
                    handler(node.Tag.ToString()); //Tag has full name
            }
        }

        private void trvSchema_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnStdQuery.Enabled = (e.Node.Level == 1);
        }
    }
}
