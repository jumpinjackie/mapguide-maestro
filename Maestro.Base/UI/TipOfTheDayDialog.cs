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
using ICSharpCode.Core;
using Maestro.Base.UI.Preferences;
using System.Xml;
using System.IO;
using System.Threading;

namespace Maestro.Base.UI
{
    internal partial class TipOfTheDayDialog : Form
    {
        private static bool _firstTime = true;

        private static string[] _tips;

        private static int _tipIndex = 0;

        internal static void FirstTimeOpen()
        {
            LoadTips();

            if (!_firstTime)
                return;

            _firstTime = false;
            if (PropertyService.Get(ConfigProperties.ShowTipOfTheDay, ConfigProperties.DefaultShowTipOfTheDay))
                Open();
        }

        private static void LoadTips()
        {
            //Init the tips
            List<string> tips = new List<string>();
            XmlDocument doc = new XmlDocument();
            string totdRoot = Path.Combine(FileUtility.ApplicationRootPath, "Data/TipOfTheDay"); //NOXLATE
            var ci = Thread.CurrentThread.CurrentUICulture;
            //Try to find a localized source based on current UI culture, fallback to english if none found
            string path = Path.Combine(totdRoot, ci.Name + ".xml"); //NOXLATE
            if (!File.Exists(path))
                path = Path.Combine(totdRoot, "en.xml"); //NOXLATE
            doc.Load(path);
            foreach (XmlNode node in doc.SelectNodes("//Tips/TipOfTheDay")) //NOXLATE
            {
                tips.Add(node.InnerText);
            }
            _tips = tips.ToArray();
            _tipIndex = new Random().Next(0, _tips.Length);
        }

        /// <summary>
        /// Displays the tip of the day dialog
        /// </summary>
        public static void Open()
        {
            var diag = new TipOfTheDayDialog();
            diag.Show(Workbench.Instance);
        }

        public TipOfTheDayDialog()
        {
            InitializeComponent();
            chkShowTip.Checked = PropertyService.Get(ConfigProperties.ShowTipOfTheDay, ConfigProperties.DefaultShowTipOfTheDay);
            chkShowTip.CheckedChanged += new EventHandler(chkShowTip_CheckedChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            txtTip.Text = GetTip(_tipIndex);
        }

        private static string GetTip(int index)
        {
            return string.Format(Strings.TipNumber, (index + 1), _tips.Length)
                + Environment.NewLine
                + Environment.NewLine
                + _tips[index];
        }

        void chkShowTip_CheckedChanged(object sender, EventArgs e)
        {
            PropertyService.Set(ConfigProperties.ShowTipOfTheDay, chkShowTip.Checked);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNextTip_Click(object sender, EventArgs e)
        {
            _tipIndex++;
            if (_tipIndex == _tips.Length)
                _tipIndex = 0;

            txtTip.Text = GetTip(_tipIndex);
        }

        private void btnRandomTip_Click(object sender, EventArgs e)
        {
            _tipIndex = new Random().Next(0, _tips.Length);
            txtTip.Text = GetTip(_tipIndex);
        }

        private void btnPrevTip_Click(object sender, EventArgs e)
        {
            _tipIndex--;
            if (_tipIndex < 0)
                _tipIndex = _tips.Length - 1;

            txtTip.Text = GetTip(_tipIndex);
        }
    }
}
