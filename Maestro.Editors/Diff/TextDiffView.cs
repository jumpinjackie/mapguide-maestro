#region Disclaimer / License

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

// Original code by Michael Potter, made available under Public Domain
//
// http://www.codeproject.com/Articles/6943/A-Generic-Reusable-Diff-Algorithm-in-C-II/

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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

        private const string NumFormat = "00000";

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public TextDiffView(TextFileDiffList source, TextFileDiffList destination, List<DiffResultSpan> DiffLines, double seconds)
            : this()
        {
            int cnt = 1;
            int i;

            foreach (DiffResultSpan drs in DiffLines)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (i = 0; i < drs.Length; i++)
                        {
                            var lvi = new ListViewItem(cnt.ToString(NumFormat));
                            lvi.UseItemStyleForSubItems = false;
                            var sline = lvi.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            sline.BackColor = Color.Red;
                            var dlineNo = lvi.SubItems.Add(cnt.ToString(NumFormat));
                            var dline = lvi.SubItems.Add(string.Empty);
                            dline.BackColor = Color.LightGray;
                            lvSource.Items.Add(lvi);
                            cnt++;
                        }

                        break;

                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < drs.Length; i++)
                        {
                            var lvi = new ListViewItem(cnt.ToString(NumFormat));
                            lvi.UseItemStyleForSubItems = false;
                            var sline = lvi.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            sline.BackColor = Color.White;
                            var dlineNo = lvi.SubItems.Add(cnt.ToString(NumFormat));
                            var dline = lvi.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);
                            dline.BackColor = Color.White;
                            lvSource.Items.Add(lvi);
                            cnt++;
                        }

                        break;

                    case DiffResultSpanStatus.AddDestination:
                        for (i = 0; i < drs.Length; i++)
                        {
                            var lvi = new ListViewItem(cnt.ToString(NumFormat));
                            lvi.UseItemStyleForSubItems = false;
                            var sline = lvi.SubItems.Add(string.Empty);
                            sline.BackColor = Color.LightGray;
                            var dlineNo = lvi.SubItems.Add(cnt.ToString(NumFormat));
                            var dline = lvi.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);
                            dline.BackColor = Color.LightGreen;
                            lvSource.Items.Add(lvi);
                            cnt++;
                        }

                        break;

                    case DiffResultSpanStatus.Replace:
                        for (i = 0; i < drs.Length; i++)
                        {
                            var lvi = new ListViewItem(cnt.ToString(NumFormat));
                            lvi.UseItemStyleForSubItems = false;
                            var sline = lvi.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            sline.BackColor = Color.Red;
                            var dlineNo = lvi.SubItems.Add(cnt.ToString(NumFormat));
                            var dline = lvi.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);
                            dline.BackColor = Color.LightGreen;
                            lvSource.Items.Add(lvi);
                            cnt++;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Sets the labels of the left and right sides of the comparison
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void SetLabels(string left, string right)
        {
            lblLeft.Text = left;
            lblRight.Text = right;
        }

        private void lvSource_Resize(object sender, System.EventArgs e)
        {
            try
            {
                lvSource.SuspendLayout();
                int width = (int)((lvSource.Width - lvSource.Columns[0].Width - lvSource.Columns[2].Width) / 2);

                lvSource.Columns[1].Width = width;
                lvSource.Columns[3].Width = width;
            }
            finally
            {
                lvSource.ResumeLayout();
            }
        }

        private void Results_Resize(object sender, System.EventArgs e)
        {
            int w = this.ClientRectangle.Width / 2;
            lblRight.Location = new Point(w + 1, lblRight.Location.Y);
        }

        private void Results_Load(object sender, System.EventArgs e)
        {
            lvSource_Resize(sender, e);
        }
    }
}