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

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A generic dialog that allows selection of an object from a list of objects
    /// </summary>
    public partial class GenericItemSelectionDialog : Form
    {
        private GenericItemSelectionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Displays a dialog to select an item from an array of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="items"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <returns></returns>
        public static T SelectItem<T>(string title, string prompt, T[] items, string displayMember, string valueMember) where T : class
        {
            var dlg = new GenericItemSelectionDialog();
            if (!string.IsNullOrEmpty(title))
                dlg.Text = title;
            if (!string.IsNullOrEmpty(prompt))
                dlg.lblPrompt.Text = prompt;

            if (!string.IsNullOrEmpty(displayMember))
                dlg.lstItems.DisplayMember = displayMember;
            if (!string.IsNullOrEmpty(valueMember))
                dlg.lstItems.ValueMember = valueMember;

            dlg.lstItems.DataSource = items;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return (T)dlg.lstItems.SelectedItem;
            }
            return null;
        }

        /// <summary>
        /// Displays a dialog to select items from an array of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="items"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <returns></returns>
        public static T[] SelectItems<T>(string title, string prompt, T[] items, string displayMember, string valueMember) where T : class
        {
            var dlg = new GenericItemSelectionDialog();
            if (!string.IsNullOrEmpty(title))
                dlg.Text = title;
            if (!string.IsNullOrEmpty(prompt))
                dlg.lblPrompt.Text = prompt;

            dlg.lstItems.SelectionMode = SelectionMode.MultiSimple;
            dlg.lstItems.DataSource = items;
            if (!string.IsNullOrEmpty(displayMember))
                dlg.lstItems.DisplayMember = displayMember;
            if (!string.IsNullOrEmpty(valueMember))
                dlg.lstItems.ValueMember = valueMember;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                List<T> values = new List<T>();
                foreach (T item in dlg.lstItems.SelectedItems)
                {
                    values.Add(item);
                }
                return values.ToArray();
            }
            return new T[0];
        }

        /// <summary>
        /// Displays a dialog to select a string from an array of strings
        /// </summary>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string SelectItem(string title, string prompt, string[] items)
        {
            var dlg = new GenericItemSelectionDialog();
            if (!string.IsNullOrEmpty(title))
                dlg.Text = title;
            if (!string.IsNullOrEmpty(prompt))
                dlg.lblPrompt.Text = prompt;

            dlg.lstItems.DataSource = items;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.lstItems.SelectedItem.ToString();
            }
            return null;
        }

        /// <summary>
        /// Displays a dialog to select strings from an array of strings
        /// </summary>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string[] SelectItems(string title, string prompt, string[] items)
        {
            var dlg = new GenericItemSelectionDialog();
            if (!string.IsNullOrEmpty(title))
                dlg.Text = title;
            if (!string.IsNullOrEmpty(prompt))
                dlg.lblPrompt.Text = prompt;

            dlg.lstItems.SelectionMode = SelectionMode.MultiSimple;
            dlg.lstItems.DataSource = items;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                List<string> values = new List<string>();
                foreach (var item in dlg.lstItems.SelectedItems)
                {
                    values.Add(item.ToString());
                }
                return values.ToArray();
            }
            return new string[0];
        }

        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (lstItems.SelectedItem != null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void lstItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstItems.SelectedItem != null)
                this.DialogResult = DialogResult.OK;
        }
    }
}
