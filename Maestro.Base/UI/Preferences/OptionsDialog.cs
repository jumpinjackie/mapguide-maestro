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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.Core;

namespace Maestro.Base.UI.Preferences
{
    internal partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();
        }

        private IList<IPreferenceSheet> _sheets;

        protected override void OnLoad(EventArgs e)
        {
            _sheets = AddInTree.BuildItems<IPreferenceSheet>("/Maestro/Preferences", this); //NOXLATE
            tabPreferences.TabPages.Clear();
            foreach (var sh in _sheets)
            {
                var page = new TabPage(sh.Title);
                sh.ContentControl.Dock = DockStyle.Fill;
                page.Controls.Add(sh.ContentControl);
                tabPreferences.TabPages.Add(page);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.RestartRequired = SaveChanges();
            this.DialogResult = DialogResult.OK;
        }

        public bool RestartRequired
        {
            get;
            private set;
        }

        private bool SaveChanges()
        {
            bool restart = false;
            foreach (IPreferenceSheet sh in _sheets)
            {
                if (sh.ApplyChanges())
                    restart = true;
            }
            PropertyService.Save();
            LoggingService.Info("Preferences saved"); //NOXLATE
            return restart;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion(Strings.ConfirmResetPrefs))
            {
                this.RestartRequired = ApplyDefaults();
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ApplyDefaults()
        {
            bool restart = false;
            foreach (IPreferenceSheet sh in _sheets)
            {
                sh.ApplyDefaults();
                restart = true;
            }
            PropertyService.Save();
            LoggingService.Info("Preferences reset with default values"); //NOXLATE
            return restart;
        }
    }
}
