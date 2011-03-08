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
using System.Windows.Forms.VisualStyles;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base
{
    public delegate void ViewContentActivateEventHandler(object sender, IViewContent content);

    internal partial class ZonedContainer : UserControl
    {
        public ZonedContainer()
        {
            InitializeComponent();
        }

        public IViewContent ActiveDocumentView
        {
            get
            {
                if (documentTabs.TabCount == 0)
                    return null;

                var indx = documentTabs.SelectedIndex;
                if (indx >= 0)
                {
                    var page = documentTabs.TabPages[indx];
                    var cnt = (IViewContent)page.Tag;
                    return cnt;
                }
                return null;
            }
        }

        public event ViewContentActivateEventHandler ViewActivated;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CheckContainerStatus();

            var mgr = ServiceRegistry.GetService<ViewContentManager>();
            mgr.ViewHidden += new EventHandler(OnViewHidden);
        }

        void OnViewHidden(object sender, EventArgs e)
        {
            CheckContainerStatus();
        }

        private IEnumerable<TabControl> AllTabs()
        {
            yield return bottomZone;
            yield return documentTabs;
            yield return leftZone;
            yield return rightZone;
        }

        internal void CheckContainerStatus()
        {
            docBottomContainer.Panel2Collapsed = (bottomZone.TabPages.Count == 0);
            topContainer.Panel1Collapsed = (leftZone.TabPages.Count == 0);
            docRightContainer.Panel2Collapsed = (rightZone.TabPages.Count == 0) ;
        }

        public void AddContent(IViewContent content)
        {
            TabControl zone = null;
            ImageList zoneImgList = null;
            switch (content.DefaultRegion)
            {
                case ViewRegion.Bottom:
                    zone = bottomZone;
                    zoneImgList = bottomImgList;
                    break;
                case ViewRegion.Document:
                    zone = documentTabs;
                    zoneImgList = docImgList;
                    break;
                case ViewRegion.Left:
                    zone = leftZone;
                    zoneImgList = leftImgList;
                    break;
                case ViewRegion.Right:
                    zone = rightZone;
                    zoneImgList = rightImgList;
                    break;
                default: //Something further up should've handled this
                    throw new InvalidOperationException("Not zoned content");
            }

            var page = TabFactory.CreateTab(content, null);
            zone.TabPages.Add(page);
            var idx = zone.TabPages.IndexOf(page);
            if (zone.SelectedIndex != idx)
                zone.SelectedIndex = idx;
            else
                OnViewActivated(content);

            CheckContainerStatus();
        }

        // Close button on tabs implementation
        //
        // http://www.dotnetspider.com/resources/29206-Custom-drawn-Close-button-TabControl.aspx

        private Point _imageLocation = new Point(17, 4);
        private Point _imgHitArea = new Point(15, 2);

        private void ZoneDrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tab = (TabControl)sender;
            //The tag specifies whether the user can manually close this tab
            var page = tab.TabPages[e.Index];

            bool draw = (page.Tag != null && ((IViewContent)page.Tag).AllowUserClose);

            try
            {
                Rectangle r = e.Bounds;
                r = tab.GetTabRect(e.Index);
                r.Offset(2, 2);

                Brush TitleBrush = new SolidBrush(Color.Black);
                Font f = this.Font;

                string title = page.Text;

                var tabRect = tab.GetTabRect(e.Index);

                e.Graphics.FillRectangle(new SolidBrush(SystemColors.ControlLightLight), tabRect);
                e.Graphics.DrawString(title, f, TitleBrush, new Point(r.X, r.Y));

                if (draw)
                {
                    //Close Image to draw
                    Image img = Properties.Resources.cross_small;
                    e.Graphics.DrawImage(img, new Point(r.X + (tab.GetTabRect(e.Index).Width - _imageLocation.X), _imageLocation.Y));
                }
            }
            catch { }
        }

        private void ZoneMouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                TabControl tc = (TabControl)sender;
                Point p = e.Location;
                int _tabWidth = 0;
                _tabWidth = tc.GetTabRect(tc.SelectedIndex).Width - (_imgHitArea.X);
                Rectangle r = tc.GetTabRect(tc.SelectedIndex);
                r.Offset(_tabWidth, _imgHitArea.Y);
                r.Width = 16;
                r.Height = 16;
                if (r.Contains(p))
                {
                    TabPage page = (TabPage)tc.TabPages[tc.SelectedIndex];
                    //The tag specifies whether the user can manually close this tab
                    if (page.Tag != null && ((IViewContent)page.Tag).AllowUserClose)
                    {
                        ((IViewContent)page.Tag).Close();
                    }
                }
            }
            catch { }
        }

        private void OnViewActivated(IViewContent content)
        {
            var handler = this.ViewActivated;
            if (handler != null)
                handler(this, content);
        }

        private void ZoneTabSelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabs = (TabControl)sender;
            var indx = tabs.SelectedIndex;
            if (indx >= 0)
            {
                TabPage page = tabs.TabPages[indx];
                OnViewActivated((IViewContent)page.Tag);
            }
        }
    }
}
