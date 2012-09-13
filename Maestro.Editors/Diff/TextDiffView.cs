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

// Original code by Michael Potter, made available under Public Domain
//
// http://www.codeproject.com/Articles/6943/A-Generic-Reusable-Diff-Algorithm-in-C-II/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using System.Collections;

namespace Maestro.Editors.Diff
{
    /// <summary>
    /// Displays a visual comparison of two bodies of text
    /// </summary>
    public partial class TextDiffView : Form
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public TextDiffView()
        {
            InitializeComponent();
        }

        const string NumFormat = "00000";

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public TextDiffView(TextFileDiffList source, TextFileDiffList destination, List<DiffResultSpan> DiffLines, double seconds)
            : this()
        {
            ListViewItem lviS;
            ListViewItem lviD;
            int cnt = 1;
            int i;

            foreach (DiffResultSpan drs in DiffLines)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString(NumFormat));
                            lviD = new ListViewItem(cnt.ToString(NumFormat));
                            lviS.BackColor = Color.Red;
                            lviS.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD.BackColor = Color.LightGray;
                            lviD.SubItems.Add(string.Empty);

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString(NumFormat));
                            lviD = new ListViewItem(cnt.ToString(NumFormat));
                            lviS.BackColor = Color.White;
                            lviS.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD.BackColor = Color.White;
                            lviD.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString(NumFormat));
                            lviD = new ListViewItem(cnt.ToString(NumFormat));
                            lviS.BackColor = Color.LightGray;
                            lviS.SubItems.Add(string.Empty);
                            lviD.BackColor = Color.LightGreen;
                            lviD.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.Replace:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString(NumFormat));
                            lviD = new ListViewItem(cnt.ToString(NumFormat));
                            lviS.BackColor = Color.Red;
                            lviS.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD.BackColor = Color.LightGreen;
                            lviD.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;
                        }

                        break;
                }

            }
        }

        private void lvSource_Resize(object sender, System.EventArgs e)
        {
            if (lvSource.Width > 100)
            {
                lvSource.Columns[1].Width = -2;
            }
        }

        private void lvDestination_Resize(object sender, System.EventArgs e)
        {
            if (lvDestination.Width > 100)
            {
                lvDestination.Columns[1].Width = -2;
            }
        }

        private void Results_Resize(object sender, System.EventArgs e)
        {
            int w = this.ClientRectangle.Width / 2;
            lvSource.Location = new Point(0, 0);
            lvSource.Width = w;
            lvSource.Height = this.ClientRectangle.Height;

            lvDestination.Location = new Point(w + 1, 0);
            lvDestination.Width = this.ClientRectangle.Width - (w + 1);
            lvDestination.Height = this.ClientRectangle.Height;
        }

        private void Results_Load(object sender, System.EventArgs e)
        {
            Results_Resize(sender, e);
        }

        private void lvSource_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lvSource.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvDestination.Items[lvSource.SelectedItems[0].Index];
                lvi.Selected = true;
                lvi.EnsureVisible();
            }
        }

        private void lvDestination_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lvDestination.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvSource.Items[lvDestination.SelectedItems[0].Index];
                lvi.Selected = true;
                lvi.EnsureVisible();
            }
        }
    }
}
