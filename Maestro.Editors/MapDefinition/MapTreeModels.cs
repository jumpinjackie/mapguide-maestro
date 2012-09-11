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
using System.Drawing;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System.ComponentModel;

namespace Maestro.Editors.MapDefinition
{
    internal abstract class TreeItem<T>
    {
        public TreeItem(string text, Image icon, T item)
        {
            this.Text = text;
            this.Icon = icon;
            this.Tag = item;
        }

        public Image Icon { get; set; }

        public string Text { get; set; }

        public T Tag { get; set; }
    }

    internal class ScaleItem : TreeItem<IList<double>>
    {
        public ScaleItem(string name, IList<double> range)
            : base(name, Properties.Resources.magnifier, range)
        {
        }
    }

    internal class LayerItem : TreeItem<IMapLayer>
    {
        public LayerItem(IMapLayer layer)
            : base(layer.Name, Properties.Resources.layer, layer)
        {
            layer.PropertyChanged += OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.Text = this.Tag.Name;
            }
        }
    }

    internal class GroupItem : TreeItem<IMapLayerGroup>
    {
        public GroupItem(IMapLayerGroup grp)
            : base(grp.Name, Properties.Resources.folder_horizontal, grp)
        {
            grp.PropertyChanged += OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.Text = this.Tag.Name;
            }
        }
    }

    internal class BaseLayerItem : TreeItem<IBaseMapLayer>
    {
        public BaseLayerItem(IBaseMapLayer layer, IBaseMapGroup parent)
            : base(layer.Name, Properties.Resources.layer, layer)
        {
            layer.PropertyChanged += OnPropertyChanged;
            this.Parent = parent;
        }

        public IBaseMapGroup Parent
        {
            get;
            set;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.Text = this.Tag.Name;
            }
        }
    }

    internal class BaseLayerGroupItem : TreeItem<IBaseMapGroup>
    {
        public BaseLayerGroupItem(IBaseMapGroup group)
            : base(group.Name, Properties.Resources.folder_horizontal, group)
        {
            group.PropertyChanged += OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.Text = this.Tag.Name;
            }
        }
    }

    internal abstract class TreeModelBase : ITreeModel
    {
        public abstract System.Collections.IEnumerable GetChildren(TreePath treePath);

        public abstract bool IsLeaf(TreePath treePath);

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        protected void OnNodesInserted(TreeModelEventArgs e)
        {
            var handler = this.NodesInserted;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        protected void OnNodesRemoved(TreeModelEventArgs e)
        {
            var handler = this.NodesRemoved;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TreeModelEventArgs> NodesRemoved;
        
        protected void OnStructureChanged(TreePathEventArgs e)
        {
            var handler = this.StructureChanged;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TreePathEventArgs> StructureChanged;

        internal void Invalidate()
        {
            OnStructureChanged(new TreePathEventArgs(TreePath.Empty));
        }

        internal void Invalidate(TreePath path)
        {
            OnStructureChanged(new TreePathEventArgs(path));
        }
    }

    internal class DrawOrderLayerModel : TreeModelBase
    {
        private IMapDefinition _map;

        public DrawOrderLayerModel(IMapDefinition map)
        {
            _map = map;
        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                foreach (var layer in _map.MapLayer)
                {
                    yield return new LayerItem(layer);
                }
            }
            else
            {
                yield break;
            }
        }

        public override bool IsLeaf(TreePath treePath)
        {
            return !treePath.IsEmpty();
        }
    }

    internal class GroupedLayerModel : TreeModelBase
    {
        private IMapDefinition _map;

        public GroupedLayerModel(IMapDefinition map)
        {
            _map = map;
        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                foreach (var layer in _map.GetLayersWithoutGroups())
                {
                    yield return new LayerItem(layer);
                }
                foreach (var group in _map.MapLayerGroup)
                {
                    if (string.IsNullOrEmpty(group.Group))
                        yield return new GroupItem(group);
                }
            }
            else
            {
                var gitem = treePath.LastNode as GroupItem;
                if (gitem != null)
                {
                    var group = gitem.Tag;
                    foreach (var l in _map.GetLayersForGroup(group.Name))
                    {
                        yield return new LayerItem(l);
                    }
                    foreach (var g in _map.MapLayerGroup)
                    {
                        if (g.Group == group.Name)
                            yield return new GroupItem(g);
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

        public override bool IsLeaf(TreePath treePath)
        {
            var layer = treePath.LastNode as LayerItem;
            var group = treePath.LastNode as GroupItem;

            if (layer != null)
                return true;
            else if (group != null)
                return false;

            throw new ApplicationException();
        }
    }

    internal class TiledLayerModel : TreeModelBase
    {
        private IMapDefinition _map;

        public TiledLayerModel(IMapDefinition map)
        {
            _map = map;
        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                if (_map.BaseMap != null)
                {
                    yield return new ScaleItem(Strings.FiniteDisplayScales, new List<double>(_map.BaseMap.FiniteDisplayScale));
                    foreach (var grp in _map.BaseMap.BaseMapLayerGroup)
                    {
                        yield return new BaseLayerGroupItem(grp);
                    }
                }
                else
                {
                    yield return new ScaleItem(Strings.FiniteDisplayScales, new List<double>());
                }
            }
            else
            {
                var grp = treePath.LastNode as BaseLayerGroupItem;
                if (grp != null)
                {
                    if (_map.BaseMap != null)
                    {
                        foreach (var layer in _map.BaseMap.GetLayersForGroup(grp.Tag.Name))
                        {
                            yield return new BaseLayerItem(layer, grp.Tag);
                        }
                    }
                }
            }
        }

        public override bool IsLeaf(TreePath treePath)
        {
            var grp = treePath.LastNode as BaseLayerGroupItem;
            return grp == null;
        }
    }
}
