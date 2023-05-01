﻿#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using Maestro.Base.Editor;
using Maestro.Base.UI;
using Maestro.Shared.UI;
using System;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Maestro.Base
{
    /// <summary>
    /// The top level application window
    /// </summary>
    public class Workbench : WorkbenchBase
    {
        private static Workbench instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Workbench Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Occurs when [workbench initialized].
        /// </summary>
        public static event EventHandler WorkbenchInitialized = delegate { };

        private static bool _init = false;

        /// <summary>
        /// Applies the current theme to the given toolstrip
        /// </summary>
        /// <param name="ts"></param>
        public void ApplyThemeTo(ToolStrip ts)
        {
            this.Theme?.ApplyTo(ts);
        }

        /// <summary>
        /// Initializes the workbench.
        /// </summary>
        /// <param name="init">The initializer</param>
        public static void InitializeWorkbench(IWorkbenchInitializer init)
        {
            if (!_init)
            {
                instance = new Workbench(init);
                _init = true;
                WorkbenchInitialized(instance, EventArgs.Empty);
            }
        }

        private ISiteExplorer _siteExp;

        /// <summary>
        /// Gets the active site explorer
        /// </summary>
        public ISiteExplorer ActiveSiteExplorer
        {
            get { return _siteExp; }
            internal set
            {
                var current = _siteExp;
                _siteExp = value;
                if (value == null && current != null)
                {
                    current.Close();
                    //CheckContainerStatus();
                }
                var theme = this.Theme;
                var exp = _siteExp as SiteExplorer;
                if (exp != null && theme != null)
                {
                    exp.AcceptTheme(theme);
                }
            }
        }

        /// <summary>
        /// Applies the given theme to this workbench
        /// </summary>
        /// <param name="theme">The theme to apply</param>
        public override void ApplyTheme(ThemeBase theme)
        {
            base.ApplyTheme(theme);
            var exp = _siteExp as SiteExplorer;
            if (exp != null && theme != null)
            {
                exp.AcceptTheme(theme);
            }
        }

        /// <summary>
        /// Gets the active editor view
        /// </summary>
        public IEditorViewContent ActiveEditor
        {
            get { return this.ActiveDocumentView as IEditorViewContent; }
        }

        /// <summary>
        /// Raised when the active document has changed
        /// </summary>
        public event EventHandler ActiveDocumentChanged;

        /// <summary>
        /// Raises the ActiveDocumentChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="content"></param>
        protected override void OnViewActivated(object sender, IViewContent content)
        {
            this.ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the Workbench class
        /// </summary>
        /// <param name="init">The workbench initializer</param>
        public Workbench(IWorkbenchInitializer init)
            : base(init)
        {
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Workbench
            // 
            this.ClientSize = new System.Drawing.Size(1264, 861);
            this.Name = "Workbench";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }
    }
}