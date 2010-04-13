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
    public partial class FeatureSourcePreviewCtrl : UserControl
    {
        private EditorInterface _ed;
        private string _provider;
        private string _featureSourceId;

        private Dictionary<string, TabPage> _openPreviews;
        private FeatureSourceDescription _schemas;
        private SchemaViewCtrl _schemaView;

        private string _sqlPreviewKey;

        public FeatureSourcePreviewCtrl(EditorInterface ed, string provider, string featureSourceId)
        {
            InitializeComponent();
            _ed = ed;
            _provider = provider;
            _featureSourceId = featureSourceId;
            _openPreviews = new Dictionary<string, TabPage>();

            var caps = ed.CurrentConnection.GetProviderCapabilities(provider);
            bool sqlSupported = false;
            foreach(FdoProviderCapabilitiesCommandName cmd in caps.Command.SupportedCommands)
            {
                if (cmd == FdoProviderCapabilitiesCommandName.SQLCommand)
                    sqlSupported = true;
            }

            _schemaView = new SchemaViewCtrl();
            _schemaView.Dock = DockStyle.Fill;
            _schemaView.SqlSupported = sqlSupported;
            _schemaView.OnRequestPreviewClass += new SchemaViewCtrl.PreviewClassEventHandler(OnRequestClassPreview);
            _schemaView.RequestRefresh += new EventHandler(OnRequestRefresh);
            _schemaView.RequestSqlQuery += new EventHandler(OnRequestSqlQuery);
            splitContainer1.Panel1.Controls.Add(_schemaView);

            _sqlPreviewKey = Guid.NewGuid().ToString();

            try
            {
                RefreshSchemas();
            }
            catch { } //Can happen when initally loaded
        }

        void OnRequestSqlQuery(object sender, EventArgs e)
        {
            if (_openPreviews.ContainsKey(_sqlPreviewKey))
            {
                tabPreviews.SelectedTab = _openPreviews[_sqlPreviewKey];
            }
            else
            {
                TabPage page = new TabPage();
                page.Name = _sqlPreviewKey;
                page.Text = "SQL Query"; //TODO: Localize

                SqlPreviewCtrl ctl = new SqlPreviewCtrl(_ed);
                ctl.Dock = DockStyle.Fill;
                page.Controls.Add(ctl);

                _openPreviews[_sqlPreviewKey] = page;
                tabPreviews.TabPages.Add(page);
                tabPreviews.SelectedTab = page;
            }
            CheckCloseTabStatus();
        }

        void OnRequestRefresh(object sender, EventArgs e)
        {
            RefreshSchemas();
        }

        void RefreshSchemas()
        {
            _schemas = _ed.CurrentConnection.DescribeFeatureSource(_featureSourceId);
            _schemaView.Schemas = _schemas;
        }

        void OnRequestClassPreview(string className)
        {
            if (_openPreviews.ContainsKey(className))
            {
                tabPreviews.SelectedTab = _openPreviews[className];
            }
            else
            {
                foreach (FeatureSourceDescription.FeatureSourceSchema cls in _schemas.Schemas)
                {
                    FeatureSourceDescription.FeatureSourceSchema c = cls;
                    if (cls.Fullname == className)
                    {
                        TabPage page = new TabPage();
                        page.Name = className;
                        page.Text = className;

                        ClassPreviewCtrl ctl = new ClassPreviewCtrl(_ed, _provider, c, _featureSourceId);
                        ctl.Dock = DockStyle.Fill;
                        page.Controls.Add(ctl);

                        _openPreviews[className] = page;
                        tabPreviews.TabPages.Add(page);
                        tabPreviews.SelectedTab = page;
                    }
                }
            }
            CheckCloseTabStatus();
        }

        private void btnCloseTab_Click(object sender, EventArgs e)
        {
            TabPage page = tabPreviews.SelectedTab;
            tabPreviews.TabPages.Remove(page);
            _openPreviews.Remove(page.Name);
            page.Dispose();

            CheckCloseTabStatus();
        }

        private void CheckCloseTabStatus()
        {
            btnCloseTab.Enabled = tabPreviews.TabPages.Count > 0;
        }
    }
}
