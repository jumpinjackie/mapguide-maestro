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
using System.Windows.Forms;

namespace Maestro.Shared.UI
{
    internal class HiddenTab
    {
        public TabControl Parent { get; set; }
        public TabPage Tab { get; set; }
    }

    /// <summary>
    /// Platform check helper class. Use this class to determine if this assembly is executing
    /// under the Mono Runtime environment.
    /// </summary>
    static class Platform
    {
        static Platform()
        {
            _mrtType = Type.GetType("Mono.Runtime"); //NOXLATE
        }

        private static Type _mrtType;

        /// <summary>
        /// Gets whether this application is running under the Mono CLR
        /// </summary>
        public static bool IsRunningOnMono
        {
            get
            {
                return _mrtType != null;
            }
        }
    }

    // A BIG NOTE TO ANYONE THINKING ABOUT HACKING THIS CODE:
    //
    // There are 2 ways to handle tab selections (setting/getting):
    // - Using the SelectedTab property of TabControl
    // - Using the SelectedIndex property of TabControl and fetching the relevant tab from its TabPages collection
    //
    // Mono (2.4) has a buggy implementation of TabControl and will do crazy things if you use the SelectedTab property. 
    // So if you're ever hacking code that involves setting or getting a selected tab, NEVER USE THE SelectedTab PROPERTY!

    internal static class TabFactory
    {
        private static List<HiddenTab> _hiddenTabs = new List<HiddenTab>();

        internal static TabPage CreateTab(IViewContent content, string imgKey)
        {
            TabPage page = new TabPage();
            page.ImageKey = imgKey;
            page.Text = content.Title; 
            page.ToolTipText = content.Description;
            page.Tag = content;

            content.TitleChanged += (sender, e) => 
            {
                page.Text = content.Title; 
            };
            content.DescriptionChanged += (sender, e) =>
            {
                page.ToolTipText = content.Description;
            };
            content.ViewContentActivating += (sender, e) =>
            {
                //Find matching hidden tab entry, and restore
                HiddenTab hiddenTab = null;
                foreach (var htab in _hiddenTabs)
                {
                    if (htab.Tab == page)
                    {
                        hiddenTab = htab;
                    }
                }
                if (hiddenTab != null)
                {
                    hiddenTab.Parent.TabPages.Add(page);
                    var indx = hiddenTab.Parent.TabPages.IndexOf(page);
                    hiddenTab.Parent.SelectedIndex = indx;
                    _hiddenTabs.Remove(hiddenTab);
                }
                else //Wasn't hidden in the first place
                {
                    var tabs = page.Parent as TabControl;
                    if (tabs != null)
                    {
                        var indx = tabs.TabPages.IndexOf(page);
                        tabs.SelectedIndex = indx;
                    }
                }
            };


            content.ViewContentClosed += (sender, e) =>
            {
                //Remove itself from the tab control
                var tabs = page.Parent as TabControl;
                if (tabs != null && tabs.TabPages.Contains(page))
                {
                    if (!Platform.IsRunningOnMono)
                    {
                        var idx = tabs.TabPages.IndexOf(page);
                        tabs.TabPages.Remove(page);
                        if (idx > 0)
                            tabs.SelectedIndex = --idx;
                    }
                    else
                    {
                        int idx = -1;
                        //HACK: Mono (2.4) will chuck a hissy fit if we remove
                        //a tab from a TabControl that has a selected tab so we
                        //have to null the selected tab, but this cause weird
                        //visual effects once the tab is removed, so we record
                        //the selected index, so we can assign the one beside it
                        //to be the selected tab after removal.
                        if (tabs.SelectedTab == page)
                        {
                            idx = tabs.SelectedIndex;
                            tabs.SelectedTab = null;
                        }
                        tabs.TabPages.Remove(page);

                        if (idx > 0)
                        {
                            idx--;
                            tabs.SelectedIndex = idx;
                        }
                        else
                        {
                            //Set to first tab if available.
                            if (tabs.TabCount > 0)
                            {
                                tabs.SelectedIndex = 0;
                            }
                        }
                    }
                    page.Dispose();
                }
            };

            content.ViewContentHiding += (sender, e) =>
            {
                //Store in hidden tabs collection
                var tabs = page.Parent as TabControl;
                if (tabs != null && tabs.TabPages.Contains(page))
                {
                    var htab = new HiddenTab() { Parent = tabs, Tab = page };
                    _hiddenTabs.Add(htab);
                    htab.Parent.TabPages.Remove(page);
                }
            };

            content.ContentControl.Dock = DockStyle.Fill;
            page.Controls.Add(content.ContentControl);

            return page;
        }
    }
}
