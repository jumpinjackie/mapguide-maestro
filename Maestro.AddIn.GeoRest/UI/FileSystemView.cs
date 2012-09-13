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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.AddIn.GeoRest.Model;
using Maestro.AddIn.GeoRest.Services;
using Maestro.Base.Services;
using System.IO;
using Maestro.Shared.UI;
using Maestro.Editors.Common;

namespace Maestro.AddIn.GeoRest.UI
{
    public partial class FileSystemView : ViewContentBase
    {
        public FileSystemView()
        {
            InitializeComponent();
            this.Title = this.Description = Properties.Resources.GeoRestExplorer;
        }

        const int IDX_FOLDER = 0;
        const int IDX_CONFIG = 1;
        const int IDX_GENERIC = 2;
        const int IDX_ODATA = 3;
        const int IDX_JSON = 4;
        const int IDX_XML = 5;
        const int IDX_IMAGE = 6;
        const int IDX_FDOSCHEMA = 7;
        const int IDX_TEMPLATE = 8;

        private GeoRestService _service;

        protected override void OnLoad(EventArgs e)
        {
            _service = ServiceRegistry.GetService<GeoRestService>();
            base.OnLoad(e);
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

        private void fileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FileSystemEntry res = e.Node.Tag as FileSystemEntry;
            btnPreview.Enabled = (res != null && !res.IsFolder && res.Name.ToLower().Equals("restcfg.xml"));
            btnRefresh.Enabled = (res != null && res.IsFolder);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (var diag = new ConnectDialog())
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    fileTree.Nodes.Clear();
                    _service.Connect(diag.ConfigurationRoot, diag.GeoRestUrl);
                    btnOptions.Enabled = true;
                    var node = new TreeNode();
                    node.Text = "Root";
                    node.Tag = new FileSystemEntry() { Name = _service.ConfigurationRoot, IsFolder = true };
                    node.ImageIndex = node.SelectedImageIndex = IDX_FOLDER;
                    fileTree.Nodes.Add(node);
                    var entries = _service.GetEntries(diag.ConfigurationRoot);
                    PopulateEntries(node.Nodes, entries);
                }
            }
        }

        class Placeholder { }

        private void PopulateEntries(TreeNodeCollection nodes, FileSystemEntry[] entries)
        {
            nodes.Clear();
            foreach (var ent in entries)
            {
                var node = new TreeNode();
                node.Text = ent.Name;
                node.Tag = ent;
                node.ImageIndex = node.SelectedImageIndex = IDX_GENERIC;
                if (ent.IsFolder)
                {
                    node.ImageIndex = node.SelectedImageIndex = IDX_FOLDER;
                    node.Nodes.Add(new TreeNode() { Tag = new Placeholder() });
                }
                else if (ent.Name.ToLower().Equals("restcfg.xml"))
                {
                    node.ImageIndex = node.SelectedImageIndex = IDX_CONFIG;
                }
                nodes.Add(node);
            }
        }

        private string GetFilePath(TreeNode node)
        {
            var ent = (FileSystemEntry)node.Tag;
            LinkedList<string> components = new LinkedList<string>();

            var parent = node.Parent;
            components.AddFirst(ent.Name);
            while (parent != null)
            {
                ent = parent.Tag as FileSystemEntry;
                components.AddFirst(ent.Name);
                parent = parent.Parent;
            }

            return Path.Combine(_service.ConfigurationRoot, string.Join(Path.DirectorySeparatorChar.ToString(), new List<string>(components).ToArray()));
        }

        private void fileTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            node.Nodes.Clear();

            var entries = _service.GetEntries(GetFilePath(node));
            PopulateEntries(node.Nodes, entries);
        }

        private void fileTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var ent = e.Node.Tag as FileSystemEntry;
            if (ent != null && !ent.IsFolder)
            {
                string fullPath = GetFilePath(e.Node);
                if (ent.Name.ToLower().Equals("restcfg.xml"))
                {
                    var editor = new RestConfigEditor();
                    editor.LoadFile(fullPath);
                    editor.ShowDialog();
                }
                else 
                {
                    var editor = new TextEditor();
                    editor.LoadFile(fullPath);
                    editor.ShowDialog();
                }
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            RepresentationPreview[] previewables = null;
            try
            {
                previewables = _service.GetRepresentationPreviews(GetFilePath(fileTree.SelectedNode));
                if (previewables.Length == 0)
                {
                    MessageBox.Show(Properties.Resources.ErrNoPreviewableRepresentations);
                    return;
                }

                var result = GenericItemSelectionDialog.SelectItem<RepresentationPreview>(
                    Properties.Resources.TitleSelectPreview,
                    Properties.Resources.PromptSelectPreview,
                    previewables,
                    "Name",
                    "Url");

                if (result != null)
                {
                    var launcher = ServiceRegistry.GetService<UrlLauncherService>();
                    launcher.OpenUrl(result.Url);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            var node = fileTree.SelectedNode;
            node.Nodes.Clear();

            var entries = _service.GetEntries(GetFilePath(node));
            PopulateEntries(node.Nodes, entries);
        }

        private void saveMaestroConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var save = DialogFactory.SaveFile())
            {
                save.Filter = string.Format("{0} (*.{1})|*.{1}", Strings.PickXml, "xml"); //NOXLATE
                if (save.ShowDialog() == DialogResult.OK)
                {
                    var doc = _service.GetMaestroConfig();
                    doc.Save(save.FileName);
                    MessageBox.Show(string.Format(Properties.Resources.MaestroConfigSaved, save.FileName));
                }
            }
        }
    }

    public delegate void ObjectSelectionEventHandler<T>(object sender, T obj);
}
