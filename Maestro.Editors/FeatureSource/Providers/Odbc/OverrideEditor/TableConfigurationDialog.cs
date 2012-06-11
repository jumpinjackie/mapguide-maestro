using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.OverrideEditor
{
    internal partial class TableConfigurationDialog : Form
    {
        private BindingList<TableOverrideItem> _tables;

        private TableConfigurationDialog()
        {
            InitializeComponent();
            grdTables.AutoGenerateColumns = false;
            _tables = new BindingList<TableOverrideItem>();
            grdTables.DataSource = _tables;
        }

        private IEditorService _edSvc;

        public TableConfigurationDialog(IEditorService edSvc, OdbcConfigurationDocument doc, string schemaName)
            : this()
        {
            _edSvc = edSvc;
            var scNames = doc.GetSpatialContextNames();
            tableConfigCtrl.SetSpatialContexts(scNames);
            var sc = doc.GetSpatialContext(scNames[0]);
            txtCoordinateSystem.Text = sc.CoordinateSystemWkt;

            var schema = doc.GetSchema(schemaName);
            Dictionary<string, TableOverrideItem> tables = new Dictionary<string, TableOverrideItem>();
            List<TableOverrideItem> existing = new List<TableOverrideItem>();
            foreach (var tbl in doc.GetMappingsForSchema(schemaName))
            {
                var cls = doc.GetClass(schemaName, tbl.ClassName);
                tables[tbl.ClassName] = new TableOverrideItem()
                {
                    Override = true,
                    Class = cls,
                    TableName = tbl.ClassName,
                    Key = cls.IdentityProperties.Count > 0 ? cls.IdentityProperties[0].Name : string.Empty,
                    X = tbl.XColumn,
                    Y = tbl.YColumn,
                    Z = tbl.ZColumn
                };
                if (!string.IsNullOrEmpty(cls.DefaultGeometryPropertyName))
                {
                    var prop = cls.FindProperty(cls.DefaultGeometryPropertyName) as GeometricPropertyDefinition;
                    if (prop != null)
                        tables[tbl.ClassName].SpatialContext = prop.SpatialContextAssociation;
                }
                existing.Add(tables[tbl.ClassName]);
            }
            foreach (var cls in schema.Classes)
            {
                if (!tables.ContainsKey(cls.Name))
                {
                    tables[cls.Name] = new TableOverrideItem()
                    {
                        Override = false,
                        Class = cls,
                        TableName = cls.Name,
                        Geometry = false,
                        Key = cls.IdentityProperties.Count > 0 ? cls.IdentityProperties[0].Name : string.Empty
                    };
                }
            }

            foreach (var tbl in tables.Values)
            {
                _tables.Add(tbl);
            }
        }

        public string CoordinateSystemWkt { get { return txtCoordinateSystem.Text; } }

        public OdbcTableItem[] ConfiguredTables
        {
            get
            {
                var items = new List<OdbcTableItem>();
                foreach (DataGridViewRow row in grdTables.Rows)
                {
                    if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                    {
                        var table = (TableOverrideItem)row.DataBoundItem;
                        items.Add(new OdbcTableItem()
                        {
                            ClassName = table.Class.Name,
                            SchemaName = table.Class.Parent.Name,
                            SpatialContextName = table.SpatialContext,
                            XColumn = table.X,
                            YColumn = table.Y,
                            ZColumn = table.Z
                        });
                    }
                }
                return items.ToArray();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void grdTables_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var cell = grdTables.CurrentCell;
            if (cell is DataGridViewCheckBoxCell)
            {
                grdTables.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (cell.Value != null && Convert.ToBoolean(cell.Value))
                {
                    var row = grdTables.Rows[cell.RowIndex];
                    tableConfigCtrl.Reset();
                    tableConfigCtrl.Init((TableOverrideItem)row.DataBoundItem);
                }
                else
                {
                    tableConfigCtrl.Reset();
                }
            }
        }

        private void grdTables_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                tableConfigCtrl.Reset();
                var row = grdTables.Rows[e.RowIndex];
                if (row.Cells[0].Value != null && Convert.ToBoolean(row.Cells[0].Value))
                {
                    tableConfigCtrl.Init((TableOverrideItem)row.DataBoundItem);
                }
            }
        }

        private void btnPickCs_Click(object sender, EventArgs e)
        {
            var cs = _edSvc.GetCoordinateSystem();
            if (!string.IsNullOrEmpty(cs))
                txtCoordinateSystem.Text = cs;
        }
    }
}
