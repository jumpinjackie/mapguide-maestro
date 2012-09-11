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
using Aga.Controls.Tree;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System.Diagnostics;

namespace Maestro.Editors.WebLayout
{
    [ToolboxItem(false)]
    internal partial class MenuEditorCtrl : EditorBase
    {
        public MenuEditorCtrl()
        {
            InitializeComponent();
        }

        public ITreeModel Model
        {
            get { return trvMenuItems.Model; }
            set { trvMenuItems.Model = value; }
        }

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
        }

        protected override void UnsubscribeEventHandlers()
        {
            _wl.CommandSet.CustomCommandAdded -= OnCustomCommandAdded;
            _wl.CommandSet.CustomCommandRemoved -= OnCustomCommandRemoved;

            base.UnsubscribeEventHandlers();
        }

        private IEditorService _edsvc;
        private IWebLayout _wl;
        private IMenu _rootMenu;
        private MenuTreeModel _model;

        public void Bind(IEditorService service, IWebLayout wl, IMenu menu)
        {
            _edsvc = service;
            Bind(_edsvc);
            _wl = wl;
            _wl.CommandSet.CustomCommandAdded += OnCustomCommandAdded;
            _wl.CommandSet.CustomCommandRemoved += OnCustomCommandRemoved;
            _rootMenu = menu;
            this.Model = _model = new MenuTreeModel(menu, wl);
            
            InitBuiltinCommandMenu();
            InitCustomCommandMenu();
        }

        public void AddCommand(ICommand cmd)
        {
            var item = _wl.CreateCommandItem(cmd.Name);
            _rootMenu.AddItem(item);
            RefreshModel();
        }

        void OnCustomCommandRemoved(ICommand cmd)
        {
            RemoveCustomCommandEntry(mnuCustom, cmd);

            //Might have invalidated (and removed) some menu items, so refresh
            RefreshModel();
        }

        void OnCustomCommandAdded(ICommand cmd)
        {
            AddCustomCommandEntry(mnuCustom, cmd);
        }

        private void InitCustomCommandMenu()
        {
            foreach (var cmd in _wl.GetCustomCommands())
            {
                AddCustomCommandEntry(mnuCustom, cmd);
            }
        }

        private void InitBuiltinCommandMenu()
        {
            foreach (BuiltInCommandType type in Enum.GetValues(typeof(BuiltInCommandType)))
            {
                ToolStripMenuItem mi = new ToolStripMenuItem(type.ToString(), CommandIconCache.GetStandardCommandIcon(type), new EventHandler(OnAddBuiltInCommand));
                mi.Tag = type;
                mnuBuiltin.DropDown.Items.Add(mi);
            }
        }

        private void RemoveCustomCommandEntry(ToolStripMenuItem tsi, ICommand cmd)
        {
            ToolStripItem find = null;
            foreach (ToolStripItem ti in tsi.DropDown.Items)
            {
                if (ti.Tag == cmd)
                {
                    find = ti;
                    break;
                }
            }
            if (find != null)
            {
                tsi.DropDown.Items.Remove(find);
                //Unreg property listener
                if (_customCommandListeners.ContainsKey(find))
                {
                    var handler = _customCommandListeners[find];
                    cmd.PropertyChanged -= handler;
                    _customCommandListeners.Remove(find);
                }
            }
        }

        private Dictionary<ToolStripItem, PropertyChangedEventHandler> _customCommandListeners = new Dictionary<ToolStripItem, PropertyChangedEventHandler>();

        private void AddCustomCommandEntry(ToolStripMenuItem tsi, ICommand cmd)
        {
            var icon = CommandIconCache.GetStandardCommandIcon(cmd.ImageURL);
            if (icon == null)
                icon = Properties.Resources.question;

            ToolStripMenuItem mi = new ToolStripMenuItem(cmd.Name, icon, new EventHandler(OnAddCustomCommand));
            mi.Text = cmd.Name;
            //Reg property listener
            PropertyChangedEventHandler handler = (sender, e) =>
            {
                if (e.PropertyName == "Name")
                    mi.Text = cmd.Name;
            };
            _customCommandListeners[mi] = handler;
            cmd.PropertyChanged += handler;
            mi.Tag = cmd;
            tsi.DropDown.Items.Add(mi);
        }

        private void OnAddBuiltInCommand(object sender, EventArgs e)
        {
            var tsi = sender as ToolStripItem;
            if (tsi != null && tsi.Tag != null)
            {
                BuiltInCommandType cmdType = (BuiltInCommandType)tsi.Tag;

                //Append to end of model of active treeview
                var cmd = _wl.GetCommandByName(cmdType.ToString());
                if (cmd != null)
                {
                    var ci = _wl.CreateCommandItem(cmd.Name);
                    if (trvMenuItems.SelectedNode != null)
                    {
                        var fly = trvMenuItems.SelectedNode.Tag as FlyoutItem;
                        if (fly != null)
                        {
                            fly.Tag.AddItem(ci);
                        }
                        else
                        {
                            _rootMenu.AddItem(ci);
                        }
                    }
                    else
                    {
                        _rootMenu.AddItem(ci);
                    }
                    RefreshModel();
                }
            }
        }

        private void OnAddCustomCommand(object sender, EventArgs e)
        {
            var tsi = sender as ToolStripItem;
            if (tsi != null && tsi.Tag != null)
            {
                var cmd = (ICommand)tsi.Tag;

                var ci = _wl.CreateCommandItem(cmd.Name);
                //Reg property listener
                PropertyChangedEventHandler handler = (s, evt) =>
                {
                    if (evt.PropertyName == "Name")
                    {
                        ci.Command = cmd.Name;
                        trvMenuItems.Refresh();
                    }
                };
                cmd.PropertyChanged += handler;

                if (trvMenuItems.SelectedNode != null)
                {
                    var fly = trvMenuItems.SelectedNode.Tag as FlyoutItem;
                    if (fly != null)
                    {
                        fly.Tag.AddItem(ci);
                    }
                    else
                    {
                        _rootMenu.AddItem(ci);
                    }
                }
                else
                {
                    _rootMenu.AddItem(ci);
                }
                RefreshModel();
            }
        }

        private void addSeparator_Click(object sender, EventArgs e)
        {
            var sep = _wl.CreateSeparator();
            if (trvMenuItems.SelectedNode != null)
            {
                var obj = trvMenuItems.SelectedNode.Tag;

                var flyout = obj as FlyoutItem;
                if (flyout != null)
                {
                    flyout.Tag.AddItem(sep);
                }
                else
                {
                    _rootMenu.AddItem(sep);
                }
            }
            else
            {
                _rootMenu.AddItem(sep);
            }
            RefreshModel();
        }

        private void RefreshModel()
        {
            _model.Refresh();
            OnResourceChanged();
        }

        private void addFlyout_Click(object sender, EventArgs e)
        {
            var fly = _wl.CreateFlyout(
                Strings.NewFlyout,
                Strings.NewFlyout,
                Strings.NewFlyout,
                null, null);
            if (trvMenuItems.SelectedNode != null)
            {
                var obj = trvMenuItems.SelectedNode.Tag;

                var flyout = obj as FlyoutItem;
                if (flyout != null)
                {
                    flyout.Tag.AddItem(fly);
                }
                else
                {
                    _rootMenu.AddItem(fly);
                }
                RefreshModel();
            }
            else
            {
                _rootMenu.AddItem(fly);
                RefreshModel();
            }
        }

        private void trvMenuItems_SelectionChanged(object sender, EventArgs e)
        {
            EvaluateCommandState();
        }

        private void EvaluateCommandState()
        {
            btnDelete.Enabled = btnMoveDown.Enabled = btnMoveUp.Enabled =
                (trvMenuItems.SelectedNode != null || (trvMenuItems.SelectedNodes != null && trvMenuItems.SelectedNodes.Count > 0));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int removed = 0;
            if (trvMenuItems.SelectedNode != null) 
            {
                var it = trvMenuItems.SelectedNode.Tag as ItemBase;
                if (it != null)
                {
                    var menu = it.Item.Parent ?? _rootMenu;
                    menu.RemoveItem(it.Item);
                    removed++;
                }
            }
            else if (trvMenuItems.SelectedNodes != null)
            {
                foreach (var node in trvMenuItems.SelectedNodes)
                {
                    var it = node.Tag as ItemBase;
                    if (it != null)
                    {
                        var menu = it.Item.Parent ?? _rootMenu;
                        menu.RemoveItem(it.Item);
                        removed++;
                    }
                }
            }

            if (removed > 0)
            {
                RefreshModel();
                EvaluateCommandState();
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (trvMenuItems.SelectedNode != null)
            {
                var it = trvMenuItems.SelectedNode.Tag as ItemBase;
                if (it != null)
                {
                    var parent = it.Item.Parent ?? _rootMenu;
                    if (parent.MoveUp(it.Item))
                    {
                        RefreshModel();
                        RestoreItemSelection(it);
                        EvaluateCommandState();
                    }
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (trvMenuItems.SelectedNode != null)
            {
                var it = trvMenuItems.SelectedNode.Tag as ItemBase;
                if (it != null)
                {
                    var parent = it.Item.Parent ?? _rootMenu;
                    if (parent.MoveDown(it.Item))
                    {
                        RefreshModel();
                        RestoreItemSelection(it);
                        EvaluateCommandState();
                    }
                }
            }
        }

        private void RestoreItemSelection(ItemBase item)
        {
            TreeNodeAdv selectedNode = null;
            foreach (var node in trvMenuItems.AllNodes)
            {
                var it = node.Tag as ItemBase;
                if (it != null)
                {
                    if (it.Item == item.Item)
                    {
                        selectedNode = node;
                        break;
                    }
                }
            }

            if (selectedNode != null)
                trvMenuItems.SelectedNode = selectedNode;
        }

        private void trvMenuItems_ItemDrag(object sender, ItemDragEventArgs e)
        {
            trvMenuItems.DoDragDrop(((TreeNodeAdv[])e.Item)[0], DragDropEffects.All);
        }

        private void trvMenuItems_DragDrop(object sender, DragEventArgs e)
        {
            var dragNode = e.Data.GetData(typeof(TreeNodeAdv)) as TreeNodeAdv;
            if (dragNode == null)
                return;

            var item = ((ItemBase)dragNode.Tag).Item;

            //Detach from parent first
            if (item.Parent != null)
            {
                var m = item.Parent;
                m.RemoveItem(item);
            }
            else
            {
                _rootMenu.RemoveItem(item);
            }

            var dropNode = trvMenuItems.GetNodeAt(trvMenuItems.PointToClient(new Point(e.X, e.Y)));
            if (dropNode != null)
            {
                var dropItem = ((ItemBase)dropNode.Tag).Item;

                //Attach to new location
                var menu = dropItem as IMenu;
                if (menu != null)
                {
                    if (MessageBox.Show(Strings.QuestionAddItemToFlyout, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        menu.AddItem(item);
                    }
                    else
                    {
                        //Add to same level as dropped item
                        var pm = dropItem.Parent ?? _rootMenu;
                        var idx = pm.GetIndex(dropItem);
                        pm.Insert(item, idx);
                    }
                }
                else
                {
                    //Add to same level as dropped item
                    var pm = dropItem.Parent ?? _rootMenu;
                    var idx = pm.GetIndex(dropItem);
                    pm.Insert(item, idx);
                }
            }
            else
            {
                _rootMenu.AddItem(item);
            }
            RefreshModel();
        }

        private void trvMenuItems_DragOver(object sender, DragEventArgs e)
        {
            var node = e.Data.GetData(typeof(TreeNodeAdv)) as TreeNodeAdv;
            if (node == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
        }
    }
}
