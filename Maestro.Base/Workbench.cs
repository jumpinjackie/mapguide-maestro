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
using Maestro.Base.Editor;
using Maestro.Shared.UI;
using Maestro.Base.UI;

namespace Maestro.Base
{
    /// <summary>
    /// The top level application window
    /// </summary>
    public partial class Workbench : WorkbenchBase
    {
        static Workbench instance;

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
        /// Initializes the workbench.
        /// </summary>
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
            var h = this.ActiveDocumentChanged;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the Workbench class
        /// </summary>
        /// <param name="init">The workbench initializer</param>
        public Workbench(IWorkbenchInitializer init) : base(init) { }
    }
}
