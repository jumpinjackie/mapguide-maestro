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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Props = ICSharpCode.Core.PropertyService;
using Maestro.Base.UI.Preferences;

namespace Maestro.AddIn.Scripting.UI
{
    public partial class IronPythonPreferences : UserControl, IPreferenceSheet
    {
        public IronPythonPreferences()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var bShow = Props.Get(ShowIronPythonConsole, true);
            chkShowOnStartup.Checked = bShow;
        }

        public string Title
        {
            get { return Strings.Title_IronPython_Console; }
        }

        public Control ContentControl
        {
            get { return this; }
        }

        public const string ShowIronPythonConsole = "Scripting.ShowIronPythonConsole";//to support newline for textbox use

        public bool ApplyChanges()
        {
            bool restart = false;

            if (Apply(ShowIronPythonConsole, chkShowOnStartup.Checked))
                restart = true;

            return restart;
        }

        private bool Apply<T>(string key, T newValue)
        {
            if (Props.Get(key).Equals((object)newValue))
                return false;

            Props.Set(key, newValue);
            return true;
        }

        public void ApplyDefaults()
        {
            Props.Set(ShowIronPythonConsole, true);
        }
    }
}
