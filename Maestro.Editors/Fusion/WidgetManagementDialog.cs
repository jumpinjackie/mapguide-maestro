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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

namespace Maestro.Editors.Fusion
{
    public partial class WidgetManagementDialog : Form, INotifyResourceChanged
    {
        private IApplicationDefinition _appDef;
        private FlexibleLayoutEditorContext _context;
        private IEditorService _edsvc;

        class WidgetItem
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string Label { get; set; }

            public IWidget Widget { get; set; }
        }

        private BindingList<WidgetItem> _items;

        public WidgetManagementDialog(IApplicationDefinition appDef, IEditorService edsvc, FlexibleLayoutEditorContext context)
        {
            InitializeComponent();
            edsvc.RegisterCustomNotifier(this);
            grdWidgets.AutoGenerateColumns = false;
            _items = new BindingList<WidgetItem>();
            _appDef = appDef;
            _context = context;
            _edsvc = edsvc;
            grdWidgets.DataSource = _items;

            var wset = appDef.GetFirstWidgetSet();
            foreach (var wgt in wset.Widgets)
            {
                AddWidgetItem(wgt);
            }

            this.Disposed += new EventHandler(OnDisposed);
        }

        void OnDisposed(object sender, EventArgs e)
        {
            var handler = this.ResourceChanged;
            if (handler != null)
            {
                foreach (var h in handler.GetInvocationList())
                {
                    this.ResourceChanged -= (EventHandler)h;
                }
                this.ResourceChanged = null;
            }
        }

        private void AddWidgetItem(IWidget widget)
        {
            var item = new WidgetItem()
            {
                Name = widget.Name,
                Type = widget.Type,
                Widget = widget
            };

            widget.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Name")
                    item.Name = widget.Name;
            };

            _items.Add(item);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var diag = new NewWidgetDialog(_appDef, _context);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var winfo = diag.SelectedWidget;
                var name = diag.WidgetName;

                var widget = _appDef.CreateWidget(name, winfo);
                _appDef.GetFirstWidgetSet().AddWidget(widget);
                AddWidgetItem(widget);
                OnResourceChanged();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdWidgets.SelectedRows.Count == 1)
            {
                var item = (WidgetItem)grdWidgets.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show(Properties.Resources.PromptDeleteWidgetAndReferences, Properties.Resources.Confirm, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _appDef.RemoveWidget(item.Widget.Name, true);
                    _items.Remove(item);
                    groupBox2.Controls.Clear();
                    OnResourceChanged();
                }
            }
        }

        private void grdWidgets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                grdWidgets.Rows[e.RowIndex].Selected = true;
                btnDelete.Enabled = true;
                LoadWidgetEditor((WidgetItem)grdWidgets.Rows[e.RowIndex].DataBoundItem);
            }
        }

        private void LoadWidgetEditor(WidgetItem widgetItem)
        {
            groupBox2.Controls.Clear();
            var ed = FusionWidgetEditorMap.GetEditorForWidget(widgetItem.Widget, _context, _edsvc);
            ed.Content.Dock = DockStyle.Fill;
            groupBox2.Controls.Add(ed.Content);
        }

        private void OnResourceChanged()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
