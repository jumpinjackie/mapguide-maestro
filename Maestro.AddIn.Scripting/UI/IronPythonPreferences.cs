﻿#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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

using Maestro.Base.UI.Preferences;
using System;
using System.Windows.Forms;
using Props = ICSharpCode.Core.PropertyService;

namespace Maestro.AddIn.Scripting.UI
{
    internal partial class IronPythonPreferences : UserControl, IPreferenceSheet
    {
        public IronPythonPreferences()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var bShow = Props.Get(ScriptingConfigProperties.ShowIronPythonConsole, ScriptingConfigProperties.DefaultShowIronPythonConsole);
            chkShowOnStartup.Checked = bShow;

            txtModulePaths.Text = Props.Get(ScriptingConfigProperties.IronPythonModulePath, ScriptingConfigProperties.DefaultIronPythonModulePath);
        }

        public string Title => Strings.Title_IronPython_Console;

        public Control ContentControl => this;

        public bool ApplyChanges()
        {
            bool restart = false;

            if (Apply(ScriptingConfigProperties.ShowIronPythonConsole, chkShowOnStartup.Checked))
                restart = true;

            if (Apply(ScriptingConfigProperties.IronPythonModulePath, txtModulePaths.Text))
                restart = true;

            return restart;
        }

        private bool Apply<T>(string key, T newValue)
        {
            //2nd condition is for booleans
            if (Props.Get(key).Equals((object)newValue) || Props.Get(key).ToString().Equals(newValue.ToString()))
                return false;

            Props.Set(key, newValue);
            return true;
        }

        public void ApplyDefaults()
        {
            Props.Set(ScriptingConfigProperties.ShowIronPythonConsole, ScriptingConfigProperties.DefaultShowIronPythonConsole);
            Props.Set(ScriptingConfigProperties.IronPythonModulePath, ScriptingConfigProperties.DefaultIronPythonModulePath);
        }
    }
}