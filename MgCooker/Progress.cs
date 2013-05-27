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
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Tile;

namespace MgCooker
{
    public partial class Progress : Form
    {
        private TilingRunCollection m_bx;
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

        private Progress()
        {
            InitializeComponent();
        }

        public Progress(TilingRunCollection bx)
            : this()
        {
            m_bx = bx;
            bx.BeginRenderingMap += new ProgressCallback(bx_BeginRenderingMap);
            bx.BeginRenderingGroup += new ProgressCallback(bx_BeginRenderingGroup);
            bx.BeginRenderingScale += new ProgressCallback(bx_BeginRenderingScale);
            bx.BeginRenderingTile += new ProgressCallback(bx_BeginRenderingTile);

            bx.FinishRenderingTile += new ProgressCallback(bx_FinishRenderingTile);
            bx.FinishRenderingMaps += new ProgressCallback(bx_FinishRenderingMaps);
            bx.FailedRenderingTile += new ErrorCallback(bx_FailedRenderingTile);
            m_tileRuns = new List<TimeSpan>();

            m_grandTotalTiles = 0;
            foreach (MapTilingConfiguration bm in m_bx.Maps)
                m_grandTotalTiles += bm.TotalTiles;

            m_grandBegin = DateTime.Now;
        }

        void bx_FailedRenderingTile(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref Exception exception)
        {
            m_failCount++;
            exception = null; //Eat it
        }

        public TimeSpan TotalTime { get; private set; }

        private void DoClose()
        {
            m_allowClose = true;

            this.TotalTime = DateTime.Now - m_grandBegin;

            if (m_cancel)
                this.DialogResult = DialogResult.Cancel;
            else
                this.DialogResult = DialogResult.OK;

            this.Close();
        }

        void bx_FinishRenderingMaps(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            if (this.InvokeRequired)
                this.Invoke(new System.Threading.ThreadStart(DoClose));
            else
                DoClose();
        }

        void bx_FinishRenderingTile(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
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

                DisplayProgress(map, group, scaleindex, row, column, ref cancel);
            }
        }

        private delegate void DisplayProgressDelegate(MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel);

        private void DisplayProgress(MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            if (m_cancel)
                cancel = true;

            if (this.InvokeRequired)
                this.Invoke(new DisplayProgressDelegate(DisplayProgress), new object[] { map, group, scaleindex, row, column, cancel });
            else
            {
                label1.Text = string.Format(Strings.CurrentGroupStatus, group, map.ResourceId);

                tilePG.Value = (int)Math.Max(Math.Min((m_tileCount / (double)m_totalTiles) * (tilePG.Maximum - tilePG.Minimum), tilePG.Maximum), tilePG.Minimum);
                totalPG.Value = (int)Math.Max(Math.Min((m_grandTotalTileCount / (double)m_grandTotalTiles) * (totalPG.Maximum - totalPG.Minimum), totalPG.Maximum), totalPG.Minimum);

                if (m_failCount == 0)
                    tileCounter.Text = string.Format(Strings.CurrentTileCounter, m_grandTotalTileCount, m_grandTotalTiles, "");
                else
                    tileCounter.Text = string.Format(Strings.CurrentTileCounter, m_grandTotalTileCount, m_grandTotalTiles, string.Format(Strings.TileErrorCount, m_failCount));

                TimeSpan elapsed = DateTime.Now - m_grandBegin;
                DateTime finish = DateTime.Now + (new TimeSpan(m_prevDuration.Ticks * m_grandTotalTiles) - elapsed);
                TimeSpan remain = finish - DateTime.Now;

                if (finish < DateTime.Now)
                   finishEstimate.Text = Strings.InsufficientTimePassed;
                else
                    finishEstimate.Text = string.Format(Strings.RemainingTime, finish.ToShortTimeString(), string.Format("{0}:{1}:{2}", (int)Math.Floor(remain.TotalHours), remain.Minutes.ToString("00"), remain.Seconds.ToString("00")));
            }
        }


        void bx_BeginRenderingTile(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            m_beginTile = DateTime.Now;
        }

        void bx_BeginRenderingScale(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
        }

        void bx_BeginRenderingGroup(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            m_totalTiles = map.TotalTiles;
            m_tileCount = 0;
        }

        void bx_BeginRenderingMap(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            m_tileCount = 0;
        }

        private void BeginRendering()
        {
            m_bx.RenderAll();
        }

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