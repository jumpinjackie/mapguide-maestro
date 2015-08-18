#region Disclaimer / License

// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

using OSGeo.MapGuide.MaestroAPI.Tile;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MgCooker
{
    public partial class Progress : Form
    {
        private readonly TilingRunCollection m_bx;
        private List<TimeSpan> m_tileRuns;
        private long m_tileCount;
        private DateTime m_lastUpdate;
        private DateTime m_beginTile;
        private TimeSpan m_prevDuration;
        private long m_totalTiles;
        private bool m_cancel = false;

        private long m_grandTotalTiles = 0;
        private long m_grandTotalTileCount = 0;
        private DateTime m_grandBegin;
        private bool m_allowClose = true;
        private long m_failCount = 0;

        private string m_origTitle;

        private Progress()
        {
            InitializeComponent();
            m_origTitle = this.Text;
        }

        public Progress(TilingRunCollection bx)
            : this()
        {
            m_bx = bx;
            bx.BeginRenderingMap += OnBeginRenderingMap;
            bx.BeginRenderingGroup += OnBeginRenderingGroup;
            bx.BeginRenderingScale += OnBeginRenderingScale;
            bx.BeginRenderingTile += OnBeginRenderingTile;
            bx.FinishRenderingTile += OnFinishRenderingTile;
            bx.FinishRenderingMaps += OnFinishRenderingMaps;
            bx.FailedRenderingTile += OnFailedRenderingTile;
            m_tileRuns = new List<TimeSpan>();

            m_grandTotalTiles = 0;
            foreach (MapTilingConfiguration bm in m_bx.Maps)
            {
                m_grandTotalTiles += bm.TotalTiles;
            }
            m_grandBegin = DateTime.Now;
        }

        private void OnFailedRenderingTile(object sender, TileRenderingErrorEventArgs args)
        {
            m_failCount++;
            args.Error = null; //Eat it
        }

        public TimeSpan TotalTime { get; private set; }

        private void DoClose()
        {
            m_allowClose = true;

            this.TotalTime = DateTime.Now - m_grandBegin;
            this.DialogResult = m_cancel ? DialogResult.Cancel : DialogResult.OK;
            this.Close();
        }

        private void OnFinishRenderingMaps(object sender, TileProgressEventArgs args)
        {
            if (this.InvokeRequired)
                this.Invoke(new System.Threading.ThreadStart(DoClose));
            else
                DoClose();
        }

        private void OnFinishRenderingTile(object sender, TileProgressEventArgs args)
        {
            m_tileRuns.Add(DateTime.Now - m_beginTile);
            m_tileCount++;
            m_grandTotalTileCount++;

            //Update display, after 1000 tiles
            if (m_tileRuns.Count > 500 || (DateTime.Now - m_lastUpdate).TotalSeconds > 5 || m_cancel)
            {
                long d = 0;
                foreach (TimeSpan ts in m_tileRuns)
                    d += ts.Ticks;

                d /= m_tileRuns.Count;

                //For all other than the first calculation, we use the previous counts too
                if (m_grandTotalTileCount != m_tileRuns.Count)
                    d = (d + m_prevDuration.Ticks) / 2;

                m_prevDuration = new TimeSpan(d);
                TimeSpan duration = new TimeSpan(d * m_totalTiles);

                m_tileRuns.Clear();
                m_lastUpdate = DateTime.Now;

                DisplayProgress(sender, args);
            }
        }
        
        private void DisplayProgress(object sender, TileProgressEventArgs args)
        {
            if (m_cancel)
            {
                args.Cancel = true;
            }

            if (this.InvokeRequired)
            {
                TileProgressEventHandler action = DisplayProgress;
                this.Invoke(action, new object[] { sender, args });
            }
            else
            {
                label1.Text = string.Format(Strings.CurrentGroupStatus, args.Group, args.Map.ResourceId);

                tilePG.Value = (int)Math.Max(Math.Min((m_tileCount / (double)m_totalTiles) * (tilePG.Maximum - tilePG.Minimum), tilePG.Maximum), tilePG.Minimum);
                totalPG.Value = (int)Math.Max(Math.Min((m_grandTotalTileCount / (double)m_grandTotalTiles) * (totalPG.Maximum - totalPG.Minimum), totalPG.Maximum), totalPG.Minimum);

                var percentage = (int)(((double)m_grandTotalTileCount / (double)m_grandTotalTiles) * 100.0);
                this.Text = $"{m_origTitle} - ({percentage}%)";

                if (m_failCount == 0)
                    tileCounter.Text = string.Format(Strings.CurrentTileCounter, m_grandTotalTileCount, m_grandTotalTiles, string.Empty);
                else
                    tileCounter.Text = string.Format(Strings.CurrentTileCounter, m_grandTotalTileCount, m_grandTotalTiles, string.Format(Strings.TileErrorCount, m_failCount));

                TimeSpan elapsed = DateTime.Now - m_grandBegin;
                DateTime finish = DateTime.Now + (new TimeSpan(m_prevDuration.Ticks * m_grandTotalTiles) - elapsed);
                TimeSpan remain = finish - DateTime.Now;

                if (finish < DateTime.Now)
                    finishEstimate.Text = Strings.InsufficientTimePassed;
                else
                    finishEstimate.Text = string.Format(Strings.RemainingTime, finish.ToShortTimeString(), $"{Math.Floor(remain.TotalHours)}:{remain.Minutes.ToString("00")}:{remain.Seconds.ToString("00")}");
            }
        }

        private void OnBeginRenderingTile(object sender, TileProgressEventArgs args) => m_beginTile = DateTime.Now;

        private void OnBeginRenderingScale(object sender, TileProgressEventArgs args) { }

        private void OnBeginRenderingGroup(object sender, TileProgressEventArgs args)
        {
            m_totalTiles = args.Map.TotalTiles;
            m_tileCount = 0;
        }

        private void OnBeginRenderingMap(object sender, TileProgressEventArgs args) => m_tileCount = 0;

        private void BeginRendering() => m_bx.RenderAll();

        private void Progress_Load(object sender, EventArgs e)
        {
            this.Show();
            m_allowClose = false;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(BeginRendering));
            t.IsBackground = true;
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.m_cancel)
                MessageBox.Show(this, Strings.AlreadyAborting, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                this.m_cancel = true;
        }

        private void Progress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_allowClose)
            {
                button1_Click(sender, e);
                e.Cancel = true;
            }
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            m_bx.PauseEvent.Reset();
            MessageBox.Show(this, Strings.PauseMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            m_bx.PauseEvent.Set();
        }
    }
}