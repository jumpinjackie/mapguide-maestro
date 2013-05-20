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
using System.Xml;

namespace Maestro.Editors.Fusion
{
    internal partial class WidgetManagementDialog : Form, INotifyResourceChanged
    {
        private IApplicationDefinition _appDef;
        private FlexibleLayoutEditorContext _context;
        private IEditorService _edsvc;

        class WidgetItem
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string Label { get; set; }

            public bool IsDockable { get; set; }

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
            txtMapWidgetXml.SetHighlighting("XML"); //NOXLATE
            var wset = appDef.GetFirstWidgetSet();
            SetupMapWidget(wset.MapWidget);
            foreach (var wgt in wset.Widgets)
            {
                AddWidgetItem(wgt);
            }

            this.Disposed += new EventHandler(OnDisposed);
        }

        private IMapWidget _mapWidget;
        private string _initMapXml;

        private void SetupMapWidget(IMapWidget mapWidget)
        {
            _mapWidget = mapWidget;
            _initMapXml = _mapWidget.ToXml();
            txtMapWidgetXml.Text = _initMapXml;
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
                Widget = widget,
                IsDockable = true
            };

            item.IsDockable = _context.IsWidgetDockable(widget.Type);

            widget.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Name") //NOXLATE
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
                if (MessageBox.Show(Strings.PromptDeleteWidgetAndReferences, Strings.Confirm, MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        private void tabWidgets_TabIndexChanged(object sender, EventArgs e)
        {
            lblNonDockableNote.Visible = (tabWidgets.SelectedIndex == 1);
        }

        private void txtMapWidgetXml_TextChanged(object sender, EventArgs e)
        {
            btnSaveMapWidgetXml.Enabled = !(txtMapWidgetXml.Text.Equals(_initMapXml));
        }

        private void btnSaveMapWidgetXml_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtMapWidgetXml.Text);

                //Base widget properties
                XmlNode node = doc.SelectSingleNode("//WidgetType/Name"); //NOXLATE
                if (node != null)
                    _mapWidget.Name = node.InnerText;

                node = doc.SelectSingleNode("//WidgetType/Type"); //NOXLATE
                if (node != null)
                    _mapWidget.Type = node.InnerText;

                node = doc.SelectSingleNode("//WidgetType/Location"); //NOXLATE
                if (node != null)
                    _mapWidget.Location = node.InnerText;

                node = doc.SelectSingleNode("//WidgetType/MapId"); //NOXLATE
                if (node != null)
                    _mapWidget.MapId = node.InnerText;

                //Extension elements
                node = doc.SelectSingleNode("//WidgetType/Extension"); //NOXLATE
                if (node != null)
                {
                    List<XmlElement> elements = new List<XmlElement>();
                    //foreach (XmlNode child in node.ChildNodes)
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        var el = doc.CreateElement(node.ChildNodes[i].Name);
                        el.InnerXml = node.ChildNodes[i].InnerXml;
                        elements.Add(el);
                    }
                    _mapWidget.Extension.Content = elements.ToArray();
                }

                //If a UI widget, set its properties
                var uiw = _mapWidget as IUIWidget;
                if (uiw != null)
                {
                    node = doc.SelectSingleNode("//WidgetType/StatusItem"); //NOXLATE
                    if (node != null)
                        uiw.StatusText = node.InnerText;

                    node = doc.SelectSingleNode("//WidgetType/ImageUrl"); //NOXLATE
                    if (node != null)
                        uiw.ImageUrl = node.InnerText;

                    node = doc.SelectSingleNode("//WidgetType/ImageClass"); //NOXLATE
                    if (node != null)
                        uiw.ImageClass = node.InnerText;

                    node = doc.SelectSingleNode("//WidgetType/Tooltip"); //NOXLATE
                    if (node != null)
                        uiw.Tooltip = node.InnerText;

                    node = doc.SelectSingleNode("//WidgetType/Label"); //NOXLATE
                    if (node != null)
                        uiw.Label = node.InnerText;

                    node = doc.SelectSingleNode("//WidgetType/Disabled"); //NOXLATE
                    if (node != null)
                        uiw.Disabled = node.InnerText;
                }

                MessageBox.Show(Strings.WidgetUpdated);

                _initMapXml = _mapWidget.ToXml();
                txtMapWidgetXml.Text = _initMapXml;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabWidgets_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNonDockableNote.Visible = (tabWidgets.SelectedIndex == 1);
        }
    }
}
