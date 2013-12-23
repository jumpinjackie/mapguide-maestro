#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using Maestro.Editors.Preview;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
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
    /// A window container for any resource editor
    /// </summary>
    public partial class EditorWindow : Form
    {
        class DefaultResourceEditorService : ResourceEditorServiceBase
        {
            public DefaultResourceEditorService(string resourceID, IServerConnection conn)
                : base(resourceID, conn)
            { }

            /// <summary>
            /// Called when an editor requires to open a particular resource. In Maestro, this would
            /// spawn a new resource editor (or activate an existing one) for the selected resource id.
            /// </summary>
            /// <param name="resourceId"></param>
            public override void OpenResource(string resourceId)
            {
            
            }

            /// <summary>
            /// Called when an editor requires opening a url. 
            /// </summary>
            /// <param name="url"></param>
            public override void OpenUrl(string url)
            {
                System.Diagnostics.Process.Start(url);
            }

            /// <summary>
            /// Called when the editor requires a refresh of the site explorer for a specific folder
            /// </summary>
            /// <param name="folderId"></param>
            public override void RequestRefresh(string folderId)
            {
            
            }

            /// <summary>
            /// Called when the editor requires a refresh of the entire site explorer
            /// </summary>
            public override void RequestRefresh()
            {
            
            }

            /// <summary>
            /// Called when the editor needs to prompt the user to select an unmanaged resource. Currently not used
            /// </summary>
            /// <param name="startPath"></param>
            /// <param name="fileTypes"></param>
            /// <returns></returns>
            public override string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes)
            {
                throw new NotImplementedException();
            }

            public override void RunProcess(string processName, params string[] args)
            {
                throw new NotImplementedException();
            }
        }

        private EditorWindow()
        {
            InitializeComponent();
            _ed = new Generic.XmlEditorCtrl();
        }

        private IServerConnection _conn;
        private DefaultResourceEditorService _svc;

        //TODO: The reason we are using a generic XML editor is because the mechanism
        //for resolving specialized editor types is currently part of the Maestro AddIn
        //infrastructure and not available in this standalone library
        private Generic.XmlEditorCtrl _ed;

        /// <summary>
        /// Initializes a new instance of the EditorWindow class
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceId"></param>
        public EditorWindow(IServerConnection conn, string resourceId)
            : this()
        {
            _conn = conn;
            this.Text = this.Text + " - " + resourceId;
            _svc = new DefaultResourceEditorService(resourceId, conn);
            _svc.DirtyStateChanged += OnDirtyStateChanged;
            _ed.Dock = DockStyle.Fill;
            btnPreview.Enabled = ResourcePreviewEngine.IsPreviewableType(ResourceIdentifier.GetResourceType(resourceId));
            this.Controls.Add(_ed);
            _ed.Bind(_svc);
            _ed.ReadyForEditing();
            _ed.TextChanged += OnXmlContentChanged;
        }

        private void OnXmlContentChanged(object sender, EventArgs e)
        {
            _svc.MarkDirty();
        }

        const string DIRTY_PREFIX = "* ";

        void OnDirtyStateChanged(object sender, EventArgs e)
        {
            if (_svc.IsDirty)
            {
                btnSave.Enabled = true;
                if (!this.Text.StartsWith(DIRTY_PREFIX))
                    this.Text = DIRTY_PREFIX + this.Text;
            }
            else
            {
                btnSave.Enabled = false;
                if (this.Text.StartsWith(DIRTY_PREFIX))
                    this.Text = this.Text.Substring(DIRTY_PREFIX.Length);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (SyncXml())
            {
                var previewer = ResourcePreviewerFactory.GetPreviewer(_conn.ProviderName);
                if (previewer != null)
                    previewer.Preview(_svc.GetEditedResource(), _svc);
                else
                    MessageBox.Show(string.Format(Strings.NoRegisteredPreviewerForProvider, _conn.ProviderName));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SyncXml())
            {
                _svc.Save();
                OnDirtyStateChanged(this, EventArgs.Empty);
            }
        }

        private bool SyncXml()
        {
            //Force XML sync back to session repo before beginning save
            string xml = _ed.XmlContent;
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    _svc.ResourceService.SetResourceXmlData(_svc.EditedResourceID, ms);
                }
                return true;
            }
            catch (Exception ex)
            {
                XmlContentErrorDialog.CheckAndHandle(ex, xml, false);
                return false;
            }
        }
    }
}
