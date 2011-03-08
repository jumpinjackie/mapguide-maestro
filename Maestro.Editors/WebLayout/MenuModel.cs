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
using System.Text;
using Aga.Controls.Tree;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System.Diagnostics;
using System.Drawing;

namespace Maestro.Editors.WebLayout
{
    internal abstract class ItemBase
    {
        public abstract string Label { get; }

        public abstract Image Icon { get; }

        public abstract IUIItem Item { get; }
    }

    internal abstract class ItemBase<T> : ItemBase where T : IUIItem
    {
        protected ItemBase(T item) { this.Tag = item; }

        public T Tag
        {
            get;
            private set;
        }

        public override IUIItem Item
        {
            get { return this.Tag; }
        }
    }

    internal class CommandItem : ItemBase<ICommandItem>
    {
        public CommandItem(ICommandItem item, Image icon)
            : base(item)
        {
            if (icon == null)
                _icon = Properties.Resources.question;
            else
                _icon = icon;
        }

        public override string Label
        {
            get
            {
                return this.Tag.Command;
            }
        }

        private Image _icon;

        public override Image Icon
        {
            get { return _icon; }
        }
    }

    internal class SeparatorItem : ItemBase<ISeparatorItem>
    {
        public SeparatorItem(ISeparatorItem sep)
            : base(sep)
        { }

        public override string Label
        {
            get { return this.Tag.Function.ToString(); }
        }

        public override Image Icon
        {
            get { return Properties.Resources.ui_splitter_horizontal; }
        }
    }

    internal class FlyoutItem : ItemBase<IFlyoutItem>
    {
        public FlyoutItem(IFlyoutItem fly)
            : base(fly)
        { }

        public override string Label
        {
            get { return this.Tag.Label; }
        }

        public override Image Icon
        {
            get { return Properties.Resources.ui_menu; }
        }

        public IEnumerable<IUIItem> SubItem
        {
            get { return this.Tag.Items; }
        }
    }

    internal class MenuTreeModel : ITreeModel
    {
        private IMenu _menu;
        private IWebLayout _wl;

        public MenuTreeModel(IMenu menu, IWebLayout wl)
        {
            _menu = menu;
            _wl = wl;
        }

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                foreach (var item in _menu.Items)
                {
                    if (item.Function == UIItemFunctionType.Command)
                    {
                        var ci = (ICommandItem)item;
                        var cmd = _wl.GetCommandByName(ci.Command);
                        Debug.Assert(cmd != null);

                        yield return new CommandItem(ci, CommandIconCache.GetStandardCommandIcon(cmd.ImageURL));
                    }
                    else if (item.Function == UIItemFunctionType.Flyout)
                        yield return new FlyoutItem((IFlyoutItem)item);
                    else
                        yield return new SeparatorItem((ISeparatorItem)item);
                }
            }
            else
            {
                var flyout = treePath.LastNode as FlyoutItem;
                if (flyout != null)
                {
                    foreach (var item in flyout.SubItem)
                    {
                        if (item.Function == UIItemFunctionType.Command)
                        {
                            var ci = (ICommandItem)item;
                            var cmd = _wl.GetCommandByName(ci.Command);
                            Debug.Assert(cmd != null);

                            yield return new CommandItem(ci, CommandIconCache.GetStandardCommandIcon(cmd.ImageURL));
                        }
                        else if (item.Function == UIItemFunctionType.Flyout)
                            yield return new FlyoutItem((IFlyoutItem)item);
                        else
                            yield return new SeparatorItem((ISeparatorItem)item);
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

        public bool IsLeaf(TreePath treePath)
        {
            return (treePath.LastNode as FlyoutItem) == null;
        }

        internal void Refresh()
        {
            var handler = this.StructureChanged;
            if (handler != null)
                handler(this, new TreePathEventArgs(TreePath.Empty));
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;
    }
}
