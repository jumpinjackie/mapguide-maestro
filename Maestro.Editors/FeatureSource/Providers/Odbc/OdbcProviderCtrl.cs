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
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Schema;
using Maestro.Editors.FeatureSource.Providers.Odbc.OverrideEditor;
using Maestro.Editors.FeatureSource.Providers.Common;
using Maestro.Shared.UI;

namespace Maestro.Editors.FeatureSource.Providers.Odbc
{
    [ToolboxItem(false)]
    internal partial class OdbcProviderCtrl : EditorBindableCollapsiblePanel
    {
        enum OdbcConnectionMethod
        {
            ManagedFile,
            Unmanaged,
            DSN,
            KnownDriver,
            RawConnectionString,
        }

        public OdbcProviderCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;
        private IFeatureSource _fs;

        bool Use64BitDriver { get { return chkUse64Bit.Visible && chkUse64Bit.Checked; } }

        void InternalConnectionChanged(object sender, EventArgs e)
        {
            btnTest.Enabled = true;
            var props = (Use64BitDriver) ? this.ChildEditor.Get64BitConnectionProperties() : this.ChildEditor.ConnectionProperties;
            if (this.ChildEditor != null)
                txtConnStr.Text = Utility.ToConnectionString(props);
            else
                txtConnStr.Text = string.Empty;

            if (!_init)
                OnResourceChanged();
        }

        void RequestedDocumentReset(object sender, EventArgs e)
        {
            DoDocumentReset();
        }

        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            _init = true;
            _service = service;
            _fs = (IFeatureSource)_service.GetEditedResource();

            cmbMethod.DataSource = (OdbcConnectionMethod[])Enum.GetValues(typeof(OdbcConnectionMethod));
            var values = _fs.GetConnectionProperties();
            string odbcStr = values["ConnectionString"]; //NOXLATE
            if (!string.IsNullOrEmpty(odbcStr))
            {
                //See if it is file-basd
                if (odbcStr.Contains("Dbq=")) //NOXLATE
                {
                    //Check driver bitness
                    chkUse64Bit.Checked = OdbcDriverNames.Uses64BitDriver(odbcStr);

                    if (odbcStr.Contains("%MG_DATA_FILE_PATH%")) //NOXLATE
                    {
                        cmbMethod.SelectedItem = OdbcConnectionMethod.ManagedFile;
                    }
                    else if (odbcStr.Contains("%MG_DATA_PATH_ALIAS")) //NOXLATE
                    {
                        cmbMethod.SelectedItem = OdbcConnectionMethod.Unmanaged;
                    }
                    else
                    {
                        cmbMethod.SelectedItem = OdbcConnectionMethod.RawConnectionString;
                    }
                }
                else //Non-file connection string. Has to be a known driver
                {
                    cmbMethod.SelectedItem = OdbcConnectionMethod.KnownDriver;
                }
            }
            else if (values["DataSourceName"] != null) //NOXLATE
            {
                cmbMethod.SelectedItem = OdbcConnectionMethod.DSN;
            }
            else
            {
                cmbMethod.SelectedItem = OdbcConnectionMethod.RawConnectionString;
            }

            //Fall back to raw connection string if we still can't figure it out
            if (this.ChildEditor == null)
                cmbMethod.SelectedItem = OdbcConnectionMethod.RawConnectionString;

            System.Diagnostics.Debug.Assert(this.ChildEditor != null);
            if (values.Count > 0)
                this.ChildEditor.ConnectionProperties = values;

            _init = false;
        }

        public IOdbcSubEditor ChildEditor
        {
            get;
            private set;
        }

        private void cmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtConnStr.Text = txtConnectionStatus.Text = string.Empty;
            OdbcConnectionMethod method = (OdbcConnectionMethod)cmbMethod.SelectedItem;
            IOdbcSubEditor childEditor = null;
            switch (method)
            {
                case OdbcConnectionMethod.DSN:
                    childEditor = new DSNCtrl();
                    break;
                case OdbcConnectionMethod.KnownDriver:
                    childEditor = new KnownDriversCtrl();
                    break;
                case OdbcConnectionMethod.ManagedFile:
                    childEditor = new ManagedCtrl();
                    break;
                case OdbcConnectionMethod.RawConnectionString:
                    childEditor = new ConnectionStringCtrl();
                    break;
                case OdbcConnectionMethod.Unmanaged:
                    childEditor = new UnmanagedCtrl();
                    break;
            }

            if (childEditor != null)
            {
                childEditor.Bind(_service);
                //See if the current connection settings apply
                try
                {
                    childEditor.ConnectionProperties = _fs.GetConnectionProperties();
                }
                catch { }

                childEditor.ConnectionChanged += InternalConnectionChanged;
                childEditor.RequestDocumentReset += RequestedDocumentReset;
                pnlMethod.Controls.Clear();
                childEditor.Content.Dock = DockStyle.Fill;
                pnlMethod.Controls.Add(childEditor.Content);
            }

            if (this.ChildEditor != null)
            {
                this.ChildEditor.ConnectionChanged -= InternalConnectionChanged;
                this.ChildEditor.RequestDocumentReset -= RequestedDocumentReset;
            }

            this.ChildEditor = childEditor;

            if (this.ChildEditor != null)
            {
                if (!_init)
                    _fs.ApplyConnectionProperties(this.ChildEditor.ConnectionProperties);

                btnTest.Enabled = true;
            }
            else
            {
                btnTest.Enabled = false;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            txtConnectionStatus.Text = string.Empty;
            if (this.ChildEditor != null)
            {
                var props = Use64BitDriver ? this.ChildEditor.Get64BitConnectionProperties() : this.ChildEditor.ConnectionProperties;
                _fs.ApplyConnectionProperties(props);
                //Flush back to session before testing
                _service.SyncSessionCopy();
                string result = _fs.TestConnection();

                txtConnectionStatus.Text = string.Format(Strings.FdoConnectionStatus, result);
            }
        }

        private OdbcConfigurationDocument _doc;

        //This is ODBC, so there will only be one
        private string _defaultSchemaName;

        private bool CheckValidConnection()
        {
            if (string.IsNullOrEmpty(_fs.ConnectionString))
            {
                if (this.ChildEditor != null)
                {
                    var props = this.ChildEditor.ConnectionProperties;
                    _fs.ApplyConnectionProperties(props);
                }
            }

            //Flush back to session before testing
            _service.SyncSessionCopy();

            string result = _fs.TestConnection();
            if (!result.ToLower().Equals("true")) //NOXLATE
            {
                MessageBox.Show(string.Format(Strings.InvalidConnection, result), Strings.TitleError);
                return false;
            }

            return true;
        }

        private void btnEditSchema_Click(object sender, EventArgs e)
        {
            if (CheckValidConnection())
            {
                if (string.IsNullOrEmpty(_defaultSchemaName))
                {
                    var names = _fs.GetSchemaNames();
                    if (names.Length == 1)
                    {
                        _defaultSchemaName = names[0];
                    }
                    else
                    {
                        MessageBox.Show(Strings.NoSchemasInFeatureSource);
                        return;
                    }
                }

                string xml = _fs.GetConfigurationContent();
                if (!string.IsNullOrEmpty(xml))
                {
                    _doc = (OdbcConfigurationDocument)ConfigurationDocument.LoadXml(xml);
                }
                else
                {
                    if (_doc == null)
                    {
                        BuildDefaultDocument();
                    }
                }

                var diag = new TableConfigurationDialog(_service, _doc, _defaultSchemaName);
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    _doc.ClearMappings();
                    foreach (var table in diag.ConfiguredTables)
                    {
                        _doc.AddOverride(table);
                    }
                    foreach (var sc in _doc.SpatialContexts)
                    {
                        sc.CoordinateSystemWkt = diag.CoordinateSystemWkt;
                    }
                    string updatedContent = _doc.ToXml();
                    _fs.SetConfigurationContent(updatedContent);
                    OnResourceChanged();
                }
            }
        }

        private void BuildDefaultDocument()
        {
            _doc = new OdbcConfigurationDocument();

            var xmlDoc = new XmlDocument();
            XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDoc.NameTable);
            mgr.AddNamespace("xs", XmlNamespaces.XS); //NOXLATE
            mgr.AddNamespace("xsi", XmlNamespaces.XSI); //NOXLATE
            mgr.AddNamespace("fdo", XmlNamespaces.FDO); //NOXLATE
            mgr.AddNamespace("gml", XmlNamespaces.GML); //NOXLATE
            mgr.AddNamespace("xlink", XmlNamespaces.XLINK); //NOXLATE
            mgr.AddNamespace("fds", XmlNamespaces.FDS); //NOXLATE

            //This may have changed, so reapply
            var props = Use64BitDriver ? this.ChildEditor.Get64BitConnectionProperties() : this.ChildEditor.ConnectionProperties;
            _fs.ApplyConnectionProperties(props);
            _service.SyncSessionCopy();

            try
            {
                var schemaName = _fs.GetSchemaNames()[0];
                var classNames = _fs.GetClassNames(schemaName);
                var diag = new FilteredLogicalSchemaDialog(classNames);
                if (diag.ShowDialog() == DialogResult.Cancel)
                    throw new ApplicationException(Strings.TextNoItemSelected);

                var names = diag.ClassNames;

                BusyWaitDelegate worker = () =>
                {
                    classNames = names.Select(x => x.Contains(":") ? x.Split(':')[1] : x).ToArray(); //NOXLATE
                    var schema = _fs.CurrentConnection.FeatureService.DescribeFeatureSourcePartial(_fs.ResourceID, schemaName, classNames);
                    
                    _doc.AddSchema(schema); //Only one schema is supported by ODBC so this is ok
                    var scList = _fs.GetSpatialInfo(false);
                    foreach (var sc in scList.SpatialContext)
                    {
                        _doc.AddSpatialContext(sc);
                    }
                    return null;
                };
                BusyWaitDialog.Run(Strings.TextPreparingConfigurationDocument, worker, (obj, ex) => 
                {
                    if (ex != null)
                        throw ex;
                    //Done
                });
            }
            catch (Exception ex)
            {
                _doc = null;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DoDocumentReset();
        }

        private void DoDocumentReset()
        {
            _fs.SetConfigurationContent(null);
            _fs.ConfigurationDocument = null;
            _service.SyncSessionCopy();
            _doc = null;
            BuildDefaultDocument();

            if (null != _doc)
            {
                _fs.ConfigurationDocument = "config.xml";
                _fs.SetConfigurationContent(_doc.ToXml());
                MessageBox.Show(Strings.ConfigurationDocumentReset);
            }
        }

        private void chkUse64Bit_CheckedChanged(object sender, EventArgs e)
        {
            if (_init) return;

            OdbcConnectionMethod method = (OdbcConnectionMethod)cmbMethod.SelectedItem;
            if (method == OdbcConnectionMethod.ManagedFile || method == OdbcConnectionMethod.Unmanaged)
            {
                InternalConnectionChanged(this, EventArgs.Empty);
            }
        }
    }
}
