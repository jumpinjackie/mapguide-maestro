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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.MapViewer
{
    /// <summary>
    /// A helper component to auto-wire common viewer events to related UI components
    /// </summary>
    public class MapStatusTracker : Component
    {
        private IMapViewer _viewer;

        /// <summary>
        /// Gets the viewer that is being wired
        /// </summary>
        public IMapViewer Viewer
        {
            get { return _viewer; }
            set 
            {
                if (_viewer != null)
                    UnwireViewer(_viewer);
                _viewer = value;
                if (value != null)
                    WireViewer(value);
            }
        }

        /// <summary>
        /// Gets the "items selected" label
        /// </summary>
        public ToolStripLabel SelectedLabel { get; set; }

        /// <summary>
        /// Gets the "scale" label
        /// </summary>
        public ToolStripLabel ScaleLabel { get; set; }

        /// <summary>
        /// Gets the "mouse coordinates" label
        /// </summary>
        public ToolStripLabel CoordinatesLabel { get; set; }

        private void WireViewer(IMapViewer viewer)
        {
            viewer.MapScaleChanged += OnMapScaleChanged;
            viewer.MouseMapPositionChanged += OnMapPositionChanged;
            viewer.SelectionChanged += OnMapSelectionChanged;
            viewer.MapRefreshed += OnMapRefreshed;
        }

        private void UnwireViewer(IMapViewer viewer)
        {
            viewer.MapScaleChanged -= OnMapScaleChanged;
            viewer.MouseMapPositionChanged -= OnMapPositionChanged;
            viewer.SelectionChanged -= OnMapSelectionChanged;
            viewer.MapRefreshed -= OnMapRefreshed;
        }

        void OnMapRefreshed(object sender, EventArgs e)
        {
            if (this.ScaleLabel == null)
                return;

            var map = this.Viewer.GetMap();
            if (this.ScaleLabel != null)
                this.ScaleLabel.Text = string.Format("1:{0:0.00000}", map.ViewScale);
        }

        void OnMapSelectionChanged(object sender, EventArgs e)
        {
            if (this.SelectedLabel == null)
                return;

            var map = this.Viewer.GetMap();
            var sel = map.Selection;
            if (sel.Count > 0)
            {
                int total = 0;
                for (int i = 0; i < sel.Count; i++)
                    total += sel[i].Count;

                this.SelectedLabel.Text = string.Format(Properties.Resources.TextSelectedFeatures, total);
            }
            else
            {
                this.SelectedLabel.Text = string.Format(Properties.Resources.TextSelectedFeatures, 0);
            }
        }

        void OnMapPositionChanged(object sender, MapPointEventArgs e)
        {
            if (this.CoordinatesLabel == null)
                return;

            this.CoordinatesLabel.Text = string.Format(Properties.Resources.TextCoordinatePosition, e.X, e.Y);
        }

        void OnMapScaleChanged(object sender, EventArgs e)
        {
            if (this.ScaleLabel == null)
                return;

            var map = this.Viewer.GetMap();
            if (this.ScaleLabel != null)
                this.ScaleLabel.Text = string.Format("1:{0:0.00000}", map.ViewScale);
        }
    }
}
