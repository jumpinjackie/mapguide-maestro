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
using ICSharpCode.Core;
using Maestro.Base.Editor;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    internal class ResourceIdNavigator
    {
        // How this tool strip is visually laid out
        //
        // [Resource ID: ][ComboBox: Resource ID][@][ComboBox: Active Connections][ Go =>]
        //

        private ToolStrip _strip;

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
            _connMgr.ConnectionAdded += OnConnectionAdded;
            _connMgr.ConnectionRemoved += OnConnectionRemoved;

            _omgr = omgr;
            _viewMgr = viewMgr;
            _viewMgr.ViewActivated += OnViewActivated;

            _siteExp = siteExp;
            _siteExp.ItemsSelected += OnSiteExplorerItemsSelected;

            _strip = new ToolStrip();
            _strip.Layout += OnToolStripLayout;
            _strip.Stretch = true;

            _resIdLabel = new ToolStripLabel(Strings.Label_ResourceID);
            _cmbResourceId = new ToolStripComboBox();
            _cmbResourceId.AutoSize = false;
            _cmbResourceId.Width = 250;
            _cmbResourceId.TextChanged += OnResourceIdChanged;
            _cmbResourceId.KeyUp += OnResourceIdKeyUp;

            _atLabel = new ToolStripLabel("@"); //NOXLATE
            _cmbActiveConnections = new ToolStripComboBox();
            _cmbActiveConnections.AutoSize = false;
            _cmbActiveConnections.Width = 250;
            _cmbActiveConnections.ComboBox.SelectedIndexChanged += OnActiveConnectionChanged;
            _cmbActiveConnections.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            _btnGo = new ToolStripButton(Strings.Label_Open);
            _btnGo.Image = Properties.Resources.arrow;
            _btnGo.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _btnGo.TextImageRelation = TextImageRelation.TextBeforeImage;
            _btnGo.ToolTipText = Strings.Label_OpenResource;
            _btnGo.Click += btnGo_Click;

            _btnOpenAsXml = new ToolStripButton(Strings.Label_OpenAsXml);
            _btnOpenAsXml.Image = Properties.Resources.arrow;
            _btnOpenAsXml.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _btnOpenAsXml.TextImageRelation = TextImageRelation.TextBeforeImage;
            _btnOpenAsXml.ToolTipText = Strings.Label_OpenResourceAsXml;
            _btnOpenAsXml.Click += btnOpenAsXml_Click;

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

        void OnSiteExplorerItemsSelected(object sender, RepositoryItem[] items)
        {
            if (items == null)
                return;

            if (items.Length != 1)
                return;

            var idx = _cmbActiveConnections.Items.IndexOf(items[0].ConnectionName);
            if (idx >= 0)
            {
                _cmbResourceId.Text = items[0].ResourceId;
                _cmbActiveConnections.SelectedIndex = idx;
            }
        }

        void OnViewActivated(object sender, Shared.UI.IViewContent content)
        {
            var ed = content as IEditorViewContent;
            if (ed != null && !ed.IsNew)
            {
                var conn = ed.Resource.CurrentConnection;
                var idx = _cmbActiveConnections.Items.IndexOf(conn.DisplayName);
                if (idx >= 0)
                {
                    _cmbActiveConnections.SelectedIndex = idx;
                    _cmbResourceId.Text = ed.EditorService.ResourceID;
                }
            }
        }

        void OnResourceIdKeyUp(object sender, KeyEventArgs e)
        {
            if (_btnGo.Enabled && _btnOpenAsXml.Enabled && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return))
            {
                DoNavigate(false);
            }
        }

        void OnActiveConnectionChanged(object sender, EventArgs e)
        {
            _cmbActiveConnections.ToolTipText = (string)_cmbActiveConnections.SelectedItem ?? string.Empty;
        }

        void OnToolStripLayout(object sender, LayoutEventArgs e)
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

        void OnResourceIdChanged(object sender, EventArgs e)
        {
            UpdateNavigationState();
        }

        private void UpdateNavigationState()
        {
            _btnGo.Enabled = _btnOpenAsXml.Enabled = ResourceIdentifier.Validate(_cmbResourceId.Text) && !ResourceIdentifier.IsFolderResource(_cmbResourceId.Text);
        }

        void OnConnectionRemoved(object sender, string name)
        {
            UpdateConnectionList();
        }

        void OnConnectionAdded(object sender, string name)
        {
            UpdateConnectionList();
        }

        void btnGo_Click(object sender, EventArgs e)
        {
            DoNavigate(false);
        }

        void btnOpenAsXml_Click(object sender, EventArgs e)
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
