#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

#endregion Disclaimer / License
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maestro.Shared.UI;
using RestSharp;
using Maestro.AddIn.Rest.Model;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base.Services;
using System.IO;

namespace Maestro.AddIn.Rest.UI
{
    internal partial class RestExplorer : SingletonViewContent
    {
        private IServerConnection _conn;
        private RestClient _client;

        public RestExplorer()
        {
            InitializeComponent();
            this.Title = this.Description = Maestro.AddIn.Rest.Strings.RestExplorer;
        }

        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Right;
            }
        }

        public override bool AllowUserClose
        {
            get
            {
                return false;
            }
        }

        private bool IsConnected
        {
            get { return _client != null; }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var diag = new RestLoginDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                _client = diag.GetClient();
                DoRefresh();
            }
        }

        private void DoRefresh()
        {
            _client.ExecuteGetRequestAsync<DataConfigurationResponse>("data/configs.json", (resp, ex) =>
            {
                this.UIThreadInvoke(() =>
                {
                    if (ex != null)
                    {
                        ErrorDialog.Show(ex);
                        return;
                    }
                    var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                    foreach (var name in connMgr.GetConnectionNames())
                    {
                        var conn = connMgr.GetConnection(name);
                        var param = conn.CloneParameters;
                        if (resp.DataConfigurationList.MapAgentUrl.IndexOf(param["Url"] ?? Guid.NewGuid().ToString()) >= 0)
                        {
                            _conn = conn;
                            break;
                        }
                    }

                    if (_conn == null)
                    {
                        //Ask if the user wants to add this connection to the Site Explorer
                    }

                    PopulateTree(resp);
                    btnRefresh.Enabled = true;
                    btnNew.Enabled = true;
                });
            });
        }

        const int IDX_FOLDER = 0;
        const int IDX_CONFIG = 1;
        const int IDX_FILE = 2;
        const int IDX_SERVER = 3;

        private void PopulateTree(DataConfigurationResponse resp)
        {
            var list = resp.DataConfigurationList;
            try
            {
                trvRestExplorer.BeginUpdate();
                trvRestExplorer.Nodes.Clear();
                var root = trvRestExplorer.Nodes.Add(list.RootUri);
                root.ImageIndex = root.SelectedImageIndex = IDX_SERVER;
                foreach (var conf in list.Configuration)
                {
                    string name = conf.ConfigUriPart;
                    if (name.EndsWith("/config"))
                        name = name.Substring(0, name.Length - "/config".Length);
                    var node = root.Nodes.Add(name);
                    node.Tag = conf;
                    node.ImageIndex = node.SelectedImageIndex = IDX_CONFIG;
                    node.Nodes.Add("dummy"); //HACK: To show the "+" beside the node
                }
                root.Expand();
            }
            finally
            {
                trvRestExplorer.EndUpdate();
            }
        }

        private void trvRestExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var conf = e.Node.Tag as DataConfiguration;
            var list = e.Node.Tag as DataFileList;
            btnDelete.Enabled = btnEdit.Enabled = btnAddFile.Enabled = false;
            if (conf != null)
                btnDelete.Enabled = btnEdit.Enabled = btnAddFile.Enabled = true;
            else if (list != null)
                btnDelete.Enabled = true;
        }

        private void LoadConfig(string json, string uriPart, bool isNew)
        {
            new RestConfigurationEditor(_conn, _client, json, uriPart, isNew).ShowDialog();
        }

        private void trvRestExplorer_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var conf = e.Node.Tag as DataConfiguration;
            if (conf != null && e.Node.Nodes[0].Text == "dummy")
            {
                e.Node.Nodes.Clear();
                var req = new RestRequest(conf.ConfigUriPart.Replace("/config", "/files.json"));
                _client.ExecuteAsync<DataFileListResponse>(req, (resp) =>
                {
                    var list = resp.Data.DataConfigurationFileList;
                    this.UIThreadInvoke(() =>
                    {
                        foreach (var file in list.File)
                        {
                            var n = e.Node.Nodes.Add(file);
                            n.Tag = list;
                            n.ImageIndex = n.SelectedImageIndex = IDX_FILE;
                        }
                    });
                });
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var conf = trvRestExplorer.SelectedNode.Tag as DataConfiguration;
            if (conf != null)
            {
                var req = new RestRequest(conf.ConfigUriPart);
                _client.ExecuteAsync(req, (resp) =>
                {
                    var json = resp.Content;
                    this.UIThreadInvoke(() =>
                    {
                        LoadConfig(json, conf.ConfigUriPart, false);
                    });
                });
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var conf = trvRestExplorer.SelectedNode.Tag as DataConfiguration;
            var list = trvRestExplorer.SelectedNode.Tag as DataFileList;
            if (conf != null)
            {
                if (MessageBox.Show(Maestro.AddIn.Rest.Strings.PromptDeleteConfiguration, Maestro.AddIn.Rest.Strings.DeleteConfiguration, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var req = new RestRequest(conf.ConfigUriPart, Method.POST);
                    req.AddHeader("X-HTTP-METHOD-OVERRIDE", "DELETE");
                    _client.ExecuteAsync(req, (resp) =>
                    {
                        if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            this.UIThreadInvoke(() =>
                            {
                                this.DoRefresh();
                            });
                        }
                        else
                            MessageBox.Show(resp.Content);
                    });
                }
            }
            else if (list != null)
            {
                string file = trvRestExplorer.SelectedNode.Text;
                //Parent node is the configuration
                conf = trvRestExplorer.SelectedNode.Parent.Tag as DataConfiguration;
                if (conf != null)
                {
                    if (MessageBox.Show(Maestro.AddIn.Rest.Strings.DeleteFilePrompt, Maestro.AddIn.Rest.Strings.DeleteFile, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var req = new RestRequest(conf.ConfigUriPart.Replace("/config", "/file"), Method.POST);
                        req.AddHeader("X-HTTP-METHOD-OVERRIDE", "DELETE");
                        req.AddParameter("filename", file);
                        _client.ExecuteAsync(req, (resp) =>
                        {
                            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                this.UIThreadInvoke(() =>
                                {
                                    this.DoRefresh();
                                });
                            }
                            else
                                MessageBox.Show(resp.Content);
                        });
                    }
                }
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            var conf = trvRestExplorer.SelectedNode.Tag as DataConfiguration;
            var list = trvRestExplorer.SelectedNode.Tag as DataFileList;
            if (list != null) 
            {
                conf = trvRestExplorer.SelectedNode.Parent.Tag as DataConfiguration;
            }
            if (conf != null)
            {
                using (var picker = new OpenFileDialog())
                {
                    if (picker.ShowDialog() == DialogResult.OK)
                    {
                        var req = new RestRequest(conf.ConfigUriPart.Replace("/config", "/file"), Method.POST);
                        req.AddParameter("filename", Path.GetFileName(picker.FileName));
                        req.AddFile("data", picker.FileName);
                        _client.ExecuteAsync(req, (resp) =>
                        {
                            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                this.UIThreadInvoke(() =>
                                {
                                    this.DoRefresh();
                                });
                            }
                            else
                                MessageBox.Show(resp.Content);
                        });
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            LoadConfig("{}", "", true);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.DoRefresh();
        }
    }
}
