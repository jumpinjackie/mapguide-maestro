﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// The Workbench base class
    /// </summary>
    public partial class WorkbenchBase : Form
    {
        private MenuStrip menu;
        private ToolStripContainer toolStripContainer;
        private ToolStrip toolbar;

        private StatusStrip status;
        private ToolStripStatusLabel statusLabel;

        private readonly DockPanel contentPanel;

        private ContextMenuStrip ctxToolbar;

        private IWorkbenchInitializer _workbenchInitializer;

        private WorkbenchBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the WorkbenchBase class
        /// </summary>
        /// <param name="init"></param>
        protected WorkbenchBase(IWorkbenchInitializer init)
            : this()
        {
            _workbenchInitializer = init;
            _toolstrips = new Dictionary<string, ToolStrip>();
            _toolstripRegions = new Dictionary<string, ToolbarRegion>();

            this.Icon = _workbenchInitializer.GetIcon();
            this.WindowState = init.StartMaximized ? FormWindowState.Maximized : FormWindowState.Normal;

            contentPanel = new DockPanel();
            contentPanel.ActiveDocumentChanged += OnActiveDocumentChanged;
            contentPanel.DocumentStyle = DocumentStyle.DockingWindow;
            contentPanel.ShowDocumentIcon = true;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.DockLeftPortion = 250;
            contentPanel.DockBottomPortion = 200;
            contentPanel.DockRightPortion = 250;

            menu = _workbenchInitializer.GetMainMenu(this);

            toolStripContainer = new ToolStripContainer();
            toolStripContainer.ContentPanel.Controls.Add(contentPanel);
            toolStripContainer.Dock = DockStyle.Fill;

            this.Controls.Add(toolStripContainer);

            ctxToolbar = new ContextMenuStrip();
            menu.ContextMenuStrip = ctxToolbar;
            toolStripContainer.TopToolStripPanel.ContextMenuStrip = ctxToolbar;
            toolStripContainer.BottomToolStripPanel.ContextMenuStrip = ctxToolbar;
            toolStripContainer.LeftToolStripPanel.ContextMenuStrip = ctxToolbar;
            toolStripContainer.RightToolStripPanel.ContextMenuStrip = ctxToolbar;

            toolbar = _workbenchInitializer.GetMainToolStrip(this);
            toolbar.Stretch = true;
            toolbar.Tag = BASE_TOOLSTRIP;

            AddToolbar(toolbar.Tag.ToString(), toolbar, ToolbarRegion.Top, true); //NOXLATE

            status = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            status.Items.Add(statusLabel);

            this.Controls.Add(menu);
            this.Controls.Add(status);

            // Use the Idle event to update the status of menu and toolbar items.
            Application.Idle += OnApplicationIdle;
        }

        /// <summary>
        /// The currently applied theme
        /// </summary>
        public ThemeBase Theme => contentPanel.Theme;

        /// <summary>
        /// Applies the specific theme
        /// </summary>
        /// <param name="theme"></param>
        public virtual void ApplyTheme(ThemeBase theme)
        {
            contentPanel.Theme = theme;
            if (theme != null)
            {
                var toolstrips = toolStripContainer
                                    .TopToolStripPanel
                                    .Controls
                                    .OfType<ToolStrip>()
                                .Concat(toolStripContainer
                                    .BottomToolStripPanel
                                    .Controls
                                    .OfType<ToolStrip>())
                                .Concat(toolStripContainer
                                    .LeftToolStripPanel
                                    .Controls
                                    .OfType<ToolStrip>())
                                .Concat(toolStripContainer
                                    .RightToolStripPanel
                                    .Controls
                                    .OfType<ToolStrip>());

                foreach (var ts in toolstrips)
                {
                    theme.ApplyTo(ts);
                }
                theme.ApplyTo(menu);
                theme.ApplyTo(status);
                theme.ApplyTo(ctxToolbar);
            }
        }

        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            var doc = contentPanel.ActiveDocument as DockContent;
            if (doc != null)
            {
                var vc = doc.Tag as IViewContent;
                if (vc != null)
                    OnViewActivated(this, vc);
            }
        }

        private const string BASE_TOOLSTRIP = "Base"; //NOXLATE

        /// <summary>
        /// Called when a view content has been activated
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="content">The view content</param>
        protected virtual void OnViewActivated(object sender, IViewContent content)
        {
        }

        private Dictionary<string, ToolStrip> _toolstrips;
        private Dictionary<string, ToolbarRegion> _toolstripRegions;

        /// <summary>
        /// Adds the toolbar.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="toolbar">The toolbar.</param>
        /// <param name="region">The region.</param>
        /// <param name="canToggleVisibility">if set to <c>true</c> [can toggle visibility].</param>
        public void AddToolbar(string name, ToolStrip toolbar, ToolbarRegion region, bool canToggleVisibility)
        {
            _toolstrips.Add(name, toolbar);
            _toolstripRegions.Add(name, region);

            if (canToggleVisibility)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = name;
                item.Tag = name;
                item.Checked = true;
                item.CheckOnClick = true;
                item.Click += delegate
                {
                    SetToolbarVisibility(name, item.Checked);
                };
                ctxToolbar.Items.Add(item);
            }

            switch (region)
            {
                case ToolbarRegion.Top:
                    AddTopToolStrip(toolbar);
                    break;

                case ToolbarRegion.Bottom:
                    toolStripContainer.BottomToolStripPanel.Controls.Add(toolbar);
                    break;

                case ToolbarRegion.Left:
                    toolStripContainer.LeftToolStripPanel.Controls.Add(toolbar);
                    break;

                case ToolbarRegion.Right:
                    toolStripContainer.RightToolStripPanel.Controls.Add(toolbar);
                    break;
            }
        }

        /// <summary>
        /// Sets the toolbar visibility.
        /// </summary>
        /// <param name="toolbarName">Name of the toolbar.</param>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public void SetToolbarVisibility(string toolbarName, bool visible)
        {
            ToolStrip strip = GetToolbar(toolbarName);
            if (strip != null)
            {
                ToolbarRegion region = _toolstripRegions[toolbarName];
                if (visible)
                {
                    switch (region)
                    {
                        case ToolbarRegion.Bottom:
                            toolStripContainer.BottomToolStripPanel.Controls.Add(strip);
                            break;

                        case ToolbarRegion.Left:
                            toolStripContainer.LeftToolStripPanel.Controls.Add(strip);
                            break;

                        case ToolbarRegion.Right:
                            toolStripContainer.RightToolStripPanel.Controls.Add(strip);
                            break;

                        case ToolbarRegion.Top:
                            AddTopToolStrip(strip);
                            break;
                    }
                }
                else
                {
                    switch (region)
                    {
                        case ToolbarRegion.Bottom:
                            toolStripContainer.BottomToolStripPanel.Controls.Remove(strip);
                            break;

                        case ToolbarRegion.Left:
                            toolStripContainer.LeftToolStripPanel.Controls.Remove(strip);
                            break;

                        case ToolbarRegion.Right:
                            toolStripContainer.RightToolStripPanel.Controls.Remove(strip);
                            break;

                        case ToolbarRegion.Top:
                            toolStripContainer.TopToolStripPanel.Controls.Remove(strip);
                            break;
                    }
                }
            }
        }

        //SUPER-DUPER HACK: ToolStrip objects are added in the most unintuitive manner when ordering is a concern
        //
        //You'd think first one added will be the top-most tool strip. Nope! Not at all!
        //So we need this hacky method to ensure the base toolstrip will always be the top-most one

        private void AddTopToolStrip(ToolStrip strip)
        {
            var panel = toolStripContainer.TopToolStripPanel;
            if ((string)strip.Tag == BASE_TOOLSTRIP)
                panel.Controls.Add(strip);
            else
            {
                var controls = new List<Control>();
                panel.SuspendLayout();
                Control baseTs = null;
                for (int i = 0; i < panel.Controls.Count; i++)
                {
                    if ((string)panel.Controls[i].Tag == BASE_TOOLSTRIP)
                        baseTs = panel.Controls[i];
                    else
                        controls.Add(panel.Controls[i]);
                }
                panel.Controls.Clear();
                foreach (var cnt in controls)
                {
                    panel.Controls.Add(cnt);
                }
                panel.Controls.Add(baseTs);
                panel.Controls.Add(strip);
                panel.ResumeLayout();
            }
        }

        /// <summary>
        /// Gets the toolbar.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ToolStrip GetToolbar(string name)
        {
            if (_toolstrips.ContainsKey(name))
                return _toolstrips[name];
            return null;
        }

        /// <summary>
        /// Gets the toolbar names.
        /// </summary>
        /// <value>The toolbar names.</value>
        public ICollection<string> ToolbarNames => _toolstrips.Keys;

        /// <summary>
        /// Sets the status label.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetStatusLabel(string text) => statusLabel.Text = text;

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="title">The title.</param>
        public void SetTitle(string title) => this.Text = title;

        /// <summary>
        /// Gets the active view in the document region
        /// </summary>
        public IViewContent ActiveDocumentView
        {
            get
            {
                var doc = contentPanel.ActiveDocument as DockContent;
                if (doc != null)
                    return doc.Tag as IViewContent;
                return null;
            }
        }

        /// <summary>
        /// Shows the content.
        /// </summary>
        /// <param name="vc">The view content.</param>
        internal void ShowContent(IViewContent vc)
        {
            DockContent content = new DockContent();
            content.TabText = vc.Title;
            content.Text = vc.Title;
            content.ToolTipText = vc.Description;
            content.CloseButton = vc.AllowUserClose;
            content.CloseButtonVisible = vc.AllowUserClose;
            content.Tag = vc;
            var icon = vc.ViewIcon;
            if (icon != null)
            {
                content.Icon = icon;
                content.ShowIcon = true;
            }

            if (vc.IsExclusiveToDocumentRegion)
            {
                content.DockAreas = DockAreas.Document;
            }
            else
            {
                content.DockAreas = (DockAreas)(vc.DefaultRegion);
            }
            vc.SetParentForm(content);
            vc.ViewContentActivating += (sender, e) =>
            {
                content.Activate();
            };
            vc.TitleChanged += (sender, e) =>
            {
                content.TabText = vc.Title;
                content.Text = vc.Title;
            };
            vc.DescriptionChanged += (sender, e) =>
            {
                content.ToolTipText = vc.Description;
            };
            if (vc.AllowUserClose && vc.IsExclusiveToDocumentRegion)
                content.TabPageContextMenuStrip = documentTabContextMenu;

            content.ClientSize = vc.ContentControl.Size;
            vc.ContentControl.Dock = DockStyle.Fill;
            content.Controls.Add(vc.ContentControl);

            if (vc.IsModalWindow && vc.DefaultRegion == ViewRegion.Floating)
            {
                content.StartPosition = FormStartPosition.CenterParent;
                content.ShowDialog();
            }
            else
            {
                content.Show(contentPanel);
            }
        }

        // Use the Idle event to update the status of menu and toolbar.
        // Depending on your application and the number of menu items with complex conditions,
        // you might want to update the status less frequently.
        private void OnApplicationIdle(object sender, EventArgs e) => _workbenchInitializer.UpdateMenuItemStatus(menu, _toolstrips.Values);

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = contentPanel.ActiveDocument as DockContent;
            if (doc != null)
            {
                var view = doc.Tag as IViewContent;
                view.Close();
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var closeMe = new List<IViewContent>();
            foreach (var doc in contentPanel.Documents)
            {
                var cnt = doc as DockContent;
                if (cnt != null)
                {
                    var view = cnt.Tag as IViewContent;
                    if (view != null)
                        closeMe.Add(view);
                }
            }

            foreach (var view in closeMe)
                view.Close();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var closeMe = new List<IViewContent>();
            foreach (var doc in contentPanel.Documents)
            {
                if (doc == contentPanel.ActiveDocument)
                    continue;

                var cnt = doc as DockContent;
                if (cnt != null)
                {
                    var view = cnt.Tag as IViewContent;
                    if (view != null)
                        closeMe.Add(view);
                }
            }

            foreach (var view in closeMe)
                view.Close();
        }
    }

    /// <summary>
    /// Defines the valid regions a toolbar can reside on a workbench
    /// </summary>
    public enum ToolbarRegion
    {
        /// <summary>
        /// On the top
        /// </summary>
        Top,

        /// <summary>
        /// On the left
        /// </summary>
        Left,

        /// <summary>
        /// On the right
        /// </summary>
        Right,

        /// <summary>
        /// On the bottom
        /// </summary>
        Bottom
    }
}