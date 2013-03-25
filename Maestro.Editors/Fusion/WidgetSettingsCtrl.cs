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
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.Fusion
{
    [ToolboxItem(false)]
    //[ToolboxItem(true)]
    internal partial class WidgetSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public WidgetSettingsCtrl()
        {
            InitializeComponent();
        }

        const int IDX_CONTAINER = 0;
        const int IDX_WIDGET = 1;
        const int IDX_SEPARATOR = 2;
        const int IDX_MENU = 3;

        private IFusionService _fsvc;
        private IApplicationDefinition _flexLayout;
        private string _baseUrl;
        private IEditorService _edsvc;
        private FlexibleLayoutEditorContext _context;

        public override void Bind(IEditorService service)
        {
            try
            {
                _edsvc = service;
                _fsvc = (IFusionService)_edsvc.GetService((int)ServiceType.Fusion);
                _context = new FlexibleLayoutEditorContext(_fsvc);
                _baseUrl = service.GetCustomProperty("BaseUrl").ToString(); //NOXLATE

                if (!_baseUrl.EndsWith("/")) //NOXLATE
                    _baseUrl += "/"; //NOXLATE
            }
            catch
            {
                throw new NotSupportedException(Strings.IncompatibleConnection);
            }
            service.RegisterCustomNotifier(this);
            _flexLayout = (IApplicationDefinition)service.GetEditedResource();
            InitWidgetSet();
        }

        private void InitWidgetSet()
        {
            var set = _flexLayout.GetFirstWidgetSet();
            if (set != null)
            {
                foreach (var cnt in set.Containers)
                {
                    IUIItemContainer uicnt = cnt as IUIItemContainer;
                    if (uicnt != null)
                    {
                        var node = CreateContainerNode(cnt);
                        trvWidgets.Nodes.Add(node);

                        foreach (var wgtref in uicnt.Items)
                        {
                            var child = CreateNode(wgtref);
                            node.Nodes.Add(child);
                            if (wgtref.Function == UiItemFunctionType.Flyout)
                            {
                                ProcessFlyoutChildren(child, (IFlyoutItem)wgtref);
                            }
                        }
                    }
                }
            }
        }

        private void ProcessFlyoutChildren(TreeNode parent, IFlyoutItem menu)
        {
            foreach (var item in menu.Items)
            {
                var node = CreateNode(item);
                parent.Nodes.Add(node);
                if (item.Function == UiItemFunctionType.Flyout)
                {
                    ProcessFlyoutChildren(node, (IFlyoutItem)item);
                }
            }
        }

        private TreeNode CreateContainerNode(IWidgetContainer cnt)
        {
            var node = new TreeNode();
            node.Name = cnt.Name;
            node.Text = cnt.Name;
            node.ImageIndex = IDX_CONTAINER;
            node.Tag = cnt;

            return node;
        }

        private void widgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cnt = this.SelectedContainer;
            if (cnt != null)
            {
                string[] widgets = _context.GetDockableWidgetNames(_flexLayout);
                string widget = GenericItemSelectionDialog.SelectItem(
                    Strings.AddWidgetReference,
                    Strings.SelectWidget,
                    widgets);
                if (widget != null)
                {
                    //at end
                    var item = _flexLayout.CreateWidgetReference(widget);
                    cnt.Insert(item, cnt.ItemCount);
                    this.SelectedNode.Nodes.Add(CreateNode(item));
                }
            }
            else
            {
                var menu = this.SelectedNode.Tag as IMenu;
                if (menu != null)
                {
                    string[] widgets = _context.GetDockableWidgetNames(_flexLayout);
                    string widget = GenericItemSelectionDialog.SelectItem(
                        Strings.AddWidgetReference,
                        Strings.SelectWidget,
                        widgets);
                    if (widget != null)
                    {
                        //at end
                        var item = _flexLayout.CreateWidgetReference(widget);
                        menu.Insert(item, menu.ItemCount);
                        this.SelectedNode.Nodes.Add(CreateNode(item));
                    }
                }
            }
        }

        private void separatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cnt = this.SelectedContainer;
            if (cnt != null)
            {
                //at end
                var item = _flexLayout.CreateSeparator();
                cnt.Insert(item, cnt.ItemCount);
                this.SelectedNode.Nodes.Add(CreateNode(item));
            }
            else
            {
                var menu = this.SelectedNode.Tag as IMenu;
                if (menu != null)
                {
                    //at end
                    var item = _flexLayout.CreateSeparator();
                    menu.Insert(item, menu.ItemCount);
                    this.SelectedNode.Nodes.Add(CreateNode(item));
                }
            }
        }

        private void flyoutMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cnt = this.SelectedContainer;
            if (cnt != null)
            {
                //at end
                var item = _flexLayout.CreateFlyout(Strings.NewFlyout);
                cnt.Insert(item, cnt.ItemCount);
                this.SelectedNode.Nodes.Add(CreateNode(item));
            }
            else
            {
                var menu = this.SelectedNode.Tag as IMenu;
                if (menu != null)
                {
                    //at end
                    var item = _flexLayout.CreateFlyout(Strings.NewFlyout);
                    menu.Insert(item, menu.ItemCount);
                    this.SelectedNode.Nodes.Add(CreateNode(item));
                }
            }
        }

        private TreeNode CreateNode(IUIItem item)
        {
            Check.NotNull(item, "item"); //NOXLATE
            var node = new TreeNode();
            switch (item.Function)
            {
                case UiItemFunctionType.Flyout:
                    {
                        node.ImageIndex = node.SelectedImageIndex = IDX_MENU;
                        node.Text = ((IFlyoutItem)item).Label;
                        node.Tag = item;

                        item.PropertyChanged += (s, evt) =>
                        {
                            if (evt.PropertyName == "Label") //NOXLATE
                            {
                                node.Text = ((IFlyoutItem)item).Label;
                                OnResourceChanged();
                            }
                        };
                    }
                    break;
                case UiItemFunctionType.Separator:
                    {
                        node.ImageIndex = node.SelectedImageIndex = IDX_SEPARATOR;
                        node.Text = Strings.Separator;
                        node.Tag = item;
                    }
                    break;
                case UiItemFunctionType.Widget:
                    {
                        node.ImageIndex = node.SelectedImageIndex = IDX_WIDGET;
                        node.Text = ((IWidgetItem)item).Widget;
                        node.Tag = item;

                        item.PropertyChanged += (s, evt) =>
                        {
                            if (evt.PropertyName == "Widget") //NOXLATE
                            {
                                node.Text = ((IWidgetItem)item).Widget;
                                OnResourceChanged();
                            }
                        };
                    }
                    break;
            }
            return node;
        }

        internal TreeNode SelectedNode
        {
            get { return trvWidgets.SelectedNode; }
        }

        internal IUIItemContainer SelectedContainer
        {
            get
            {
                return this.SelectedNode.Tag as IUIItemContainer;
            }
        }

        internal IUIItem SelectedWidgetReference
        {
            get
            {
                return this.SelectedNode.Tag as IUIItem;
            }
        }

        private void btnRemoveWidget_Click(object sender, EventArgs e)
        {
            var node = trvWidgets.SelectedNode;
            var parent = node.Parent;
            if (parent != null)
            {
                var item = node.Tag as IUIItem;
                var cnt = parent.Tag as IUIItemContainer;
                if (cnt != null && item != null)
                {
                    cnt.RemoveItem(item);
                    parent.Nodes.Remove(node);
                    OnResourceChanged();
                }
                else
                {
                    var menu = parent.Tag as IMenu;
                    if (menu != null && item != null)
                    {
                        menu.RemoveItem(item);
                        parent.Nodes.Remove(node);
                        OnResourceChanged();
                    }
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            var node = trvWidgets.SelectedNode;
            var parent = node.Parent;
            if (parent != null)
            {
                var item = node.Tag as IUIItem;
                var cnt = parent.Tag as IUIItemContainer;
                if (cnt != null && item != null)
                {
                    int index = node.Index;
                    if (index >= 0)
                    {
                        index--;
                        cnt.MoveUp(item);
                        parent.Nodes.Remove(node);
                        parent.Nodes.Insert(index, node);
                        trvWidgets.SelectedNode = node;
                        OnResourceChanged();
                    }
                }
                else
                {
                    var menu = parent.Tag as IMenu;
                    if (menu != null && item != null)
                    {
                        int index = node.Index;
                        if (index >= 0)
                        {
                            index--;
                            menu.MoveUp(item);
                            parent.Nodes.Remove(node);
                            parent.Nodes.Insert(index, node);
                            trvWidgets.SelectedNode = node;
                            OnResourceChanged();
                        }
                    }
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            var node = trvWidgets.SelectedNode;
            var parent = node.Parent;
            if (parent != null)
            {
                var item = node.Tag as IUIItem;
                var cnt = parent.Tag as IUIItemContainer;
                if (cnt != null && item != null)
                {
                    int index = node.Index;
                    if (index < parent.Nodes.Count - 1)
                    {
                        index++;
                        cnt.MoveDown(item);
                        parent.Nodes.Remove(node);
                        parent.Nodes.Insert(index, node);
                        trvWidgets.SelectedNode = node;
                        OnResourceChanged();
                    }
                }
                else
                { 
                    var menu = parent.Tag as IMenu;
                    if (menu != null && item != null)
                    {
                        int index = node.Index;
                        if (index < parent.Nodes.Count - 1)
                        {
                            index++;
                            menu.MoveDown(item);
                            parent.Nodes.Remove(node);
                            parent.Nodes.Insert(index, node);
                            trvWidgets.SelectedNode = node;
                            OnResourceChanged();
                        }
                    }
                }
            }
        }

        private void btnManageWidgets_Click(object sender, EventArgs e)
        {
            new WidgetManagementDialog(_flexLayout, _edsvc, _context).ShowDialog();
            //Widget references may have been removed, so rebuild
            trvWidgets.Nodes.Clear();
            InitWidgetSet();
        }

        private void trvWidgets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var menu = trvWidgets.SelectedNode.Tag as IMenu;
            var item = trvWidgets.SelectedNode.Tag as IWidgetItem;
            propertiesPanel.Controls.Clear();
            btnAddWidget.Enabled = false;
            btnMoveUp.Enabled = btnMoveDown.Enabled = false;
            btnRemoveWidget.Enabled = false;
            if (menu != null)
            {
                btnAddWidget.Enabled = true;
                var fly = menu as IFlyoutItem;
                if (fly != null) //Can only edit menus that are flyouts
                {
                    btnRemoveWidget.Enabled = true;
                    btnMoveDown.Enabled = btnMoveUp.Enabled = true;
                    var ctrl = new MenuCtrl(fly, _edsvc);
                    ctrl.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(ctrl);
                }
            }
            else
            {
                btnMoveDown.Enabled = btnMoveUp.Enabled = true;
                btnRemoveWidget.Enabled = true;
                if (item != null)
                {
                    var ctrl = new WidgetReferenceCtrl(item, _edsvc);
                    ctrl.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(ctrl);
                }
            }
        }

        private void btnAddContainer_Click(object sender, EventArgs e)
        {

        }
    }
}
