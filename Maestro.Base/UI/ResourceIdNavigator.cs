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

#endregion Disclaimer / License

using ICSharpCode.Core;
using Maestro.Base.Editor;
using Maestro.Base.Services;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    internal class ResourceIdNavigator
    {
        // How this tool strip is visually laid out
        //
        // [Resource ID: ][ComboBox: Resource ID][@][ComboBox: Active Connections][ Go =>]
        //

        private readonly ToolStrip _strip;

        private ToolStripLabel _resIdLabel;
        private ToolStripComboBox _cmbResourceId;
        private ToolStripLabel _atLabel;
        private ToolStripComboBox _cmbActiveConnections;
        private ToolStripButton _btnGo;
        private ToolStripButton _btnOpenAsXml;

        private ServerConnectionManager _connMgr;
        private OpenResourceManager _omgr;
        private ISiteExplorer _siteExp;
        private ViewContentManager _viewMgr;

        public ResourceIdNavigator(ServerConnectionManager connMgr,
                                   OpenResourceManager omgr,
                                   ViewContentManager viewMgr,
                                   ISiteExplorer siteExp)
        {
            _connMgr = connMgr;
            _connMgr.ConnectionAdded += WeakEventHandler.Wrap<ServerConnectionEventHandler>(OnConnectionAdded, (eh) => _connMgr.ConnectionAdded -= eh);
            _connMgr.ConnectionRemoved += WeakEventHandler.Wrap<ServerConnectionEventHandler>(OnConnectionRemoved, (eh) => _connMgr.ConnectionRemoved -= eh);

            _omgr = omgr;
            _viewMgr = viewMgr;
            _viewMgr.ViewActivated += WeakEventHandler.Wrap<Maestro.Shared.UI.ViewEventHandler>(OnViewActivated, (eh) => _viewMgr.ViewActivated -= eh);

            _siteExp = siteExp;
            _siteExp.ItemsSelected += WeakEventHandler.Wrap<RepositoryItemEventHandler>(OnSiteExplorerItemsSelected, (eh) => _siteExp.ItemsSelected -= eh);

            _strip = new ToolStrip();
            _strip.Layout += WeakEventHandler.Wrap<LayoutEventHandler>(OnToolStripLayout, (eh) => _strip.Layout -= eh);
            _strip.Stretch = true;

            _resIdLabel = new ToolStripLabel(Strings.Label_ResourceID);
            _cmbResourceId = new ToolStripComboBox();
            _cmbResourceId.AutoSize = false;
            _cmbResourceId.Width = 250;
            _cmbResourceId.TextChanged += WeakEventHandler.Wrap(OnResourceIdChanged, (eh) => _cmbResourceId.TextChanged -= eh);
            _cmbResourceId.KeyUp += WeakEventHandler.Wrap<KeyEventHandler>(OnResourceIdKeyUp, (eh) => _cmbResourceId.KeyUp -= eh);

            _atLabel = new ToolStripLabel("@"); //NOXLATE
            _cmbActiveConnections = new ToolStripComboBox();
            _cmbActiveConnections.AutoSize = false;
            _cmbActiveConnections.Width = 250;
            _cmbActiveConnections.ComboBox.SelectedIndexChanged += WeakEventHandler.Wrap(OnActiveConnectionChanged, (eh) => _cmbActiveConnections.ComboBox.SelectedIndexChanged -= eh);
            _cmbActiveConnections.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            _btnGo = new ToolStripButton(Strings.Label_Open);
            _btnGo.Image = Properties.Resources.arrow;
            _btnGo.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _btnGo.TextImageRelation = TextImageRelation.TextBeforeImage;
            _btnGo.ToolTipText = Strings.Label_OpenResource;
            _btnGo.Click += WeakEventHandler.Wrap(btnGo_Click, (eh) => _btnGo.Click -= eh);

            _btnOpenAsXml = new ToolStripButton(Strings.Label_OpenAsXml);
            _btnOpenAsXml.Image = Properties.Resources.arrow;
            _btnOpenAsXml.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _btnOpenAsXml.TextImageRelation = TextImageRelation.TextBeforeImage;
            _btnOpenAsXml.ToolTipText = Strings.Label_OpenResourceAsXml;
            _btnOpenAsXml.Click += WeakEventHandler.Wrap(btnOpenAsXml_Click, (eh) => _btnOpenAsXml.Click -= eh);

            UpdateConnectionList();
            UpdateNavigationState();

            _strip.Items.AddRange(new ToolStripItem[]
            {
                _resIdLabel,
                _cmbResourceId,
                _atLabel,
                _cmbActiveConnections,
                _btnGo,
                _btnOpenAsXml
            });
        }

        private void OnSiteExplorerItemsSelected(object sender, RepositoryItemEventArgs e)
        {
            if (e.Items == null)
                return;

            if (e.Items.Length != 1)
                return;

            var idx = _cmbActiveConnections.Items.IndexOf(e.Items[0].ConnectionName);
            if (idx >= 0)
            {
                _cmbResourceId.Text = e.Items[0].ResourceId;
                _cmbActiveConnections.SelectedIndex = idx;
            }
        }

        private void OnViewActivated(object sender, ViewEventArgs e)
        {
            var ed = e.View as IEditorViewContent;
            if (ed != null && !ed.IsNew)
            {
                var conn = ed.EditorService.CurrentConnection;
                var idx = _cmbActiveConnections.Items.IndexOf(conn.DisplayName);
                if (idx >= 0)
                {
                    _cmbActiveConnections.SelectedIndex = idx;
                    _cmbResourceId.Text = ed.EditorService.ResourceID;
                }
            }
        }

        private void OnResourceIdKeyUp(object sender, KeyEventArgs e)
        {
            if (_btnGo.Enabled && _btnOpenAsXml.Enabled && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return))
            {
                DoNavigate(false);
            }
        }

        private void OnActiveConnectionChanged(object sender, EventArgs e)
        {
            _cmbActiveConnections.ToolTipText = (string)_cmbActiveConnections.SelectedItem ?? string.Empty;
        }

        private void OnToolStripLayout(object sender, LayoutEventArgs e)
        {
            int width = _strip.DisplayRectangle.Width;
            foreach (ToolStripItem tsi in _strip.Items)
            {
                if (!(tsi == _cmbResourceId))
                {
                    width -= tsi.Width;
                    width -= tsi.Margin.Horizontal;
                }
            }
            _cmbResourceId.Width = Math.Max(0, width - _cmbResourceId.Margin.Horizontal);
        }

        private void OnResourceIdChanged(object sender, EventArgs e)
        {
            UpdateNavigationState();
        }

        private void UpdateNavigationState()
        {
            _btnGo.Enabled = _btnOpenAsXml.Enabled = ResourceIdentifier.Validate(_cmbResourceId.Text) && !ResourceIdentifier.IsFolderResource(_cmbResourceId.Text);
        }

        private void OnConnectionRemoved(object sender, ServerConnectionEventArgs e)
        {
            UpdateConnectionList();
        }

        private void OnConnectionAdded(object sender, ServerConnectionEventArgs e)
        {
            UpdateConnectionList();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DoNavigate(false);
        }

        private void btnOpenAsXml_Click(object sender, EventArgs e)
        {
            DoNavigate(true);
        }

        private void DoNavigate(bool useXmlEditor)
        {
            var conn = GetActiveConnection();
            if (conn != null)
            {
                string resId = _cmbResourceId.Text;
                if (!ResourceIdentifier.Validate(resId))
                {
                    MessageService.ShowError(OSGeo.MapGuide.MaestroAPI.Strings.ErrorInvalidResourceIdentifier);
                }
                else
                {
                    if (_omgr.IsOpen(resId, conn))
                    {
                        var ed = _omgr.GetOpenEditor(resId, conn);
                        ed.Activate();
                    }
                    else
                    {
                        _omgr.Open(resId, conn, useXmlEditor, _siteExp);
                    }
                }
            }
            else
            {
                MessageService.ShowError(Strings.ErrorNoActiveConnection);
            }
        }

        private IServerConnection GetActiveConnection()
        {
            if (_cmbActiveConnections.SelectedItem != null)
            {
                return _connMgr.GetConnection(_cmbActiveConnections.SelectedItem.ToString());
            }
            return null;
        }

        private void UpdateConnectionList()
        {
            var connNames = _connMgr.GetConnectionNames();
            _btnGo.Enabled = _btnOpenAsXml.Enabled = connNames.Count > 0;
            _cmbActiveConnections.ComboBox.Items.Clear();
            if (connNames.Count > 0)
            {
                foreach (var name in connNames)
                {
                    _cmbActiveConnections.ComboBox.Items.Add(name);
                }
                _cmbActiveConnections.ComboBox.SelectedIndex = 0;
            }
            else
            {
                _cmbActiveConnections.ToolTipText = Strings.ErrorNoActiveConnection; //Not a real "error" just re-using the same message
            }
            UpdateNavigationState();
        }

        public ToolStrip NavigatorToolStrip { get { return _strip; } }
    }
}