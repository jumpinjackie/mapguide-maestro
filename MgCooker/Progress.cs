using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.MgCooker
{
    public partial class Progress : Form
    {
        private BatchSettings m_bx;
        private long m_update;
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

        private Progress()
        {
            InitializeComponent();
        }

        public Progress(BatchSettings bx)
            : this()
        {
            m_bx = bx;
            bx.BeginRenderingMap += new ProgressCallback(bx_BeginRenderingMap);
            bx.BeginRenderingGroup += new ProgressCallback(bx_BeginRenderingGroup);
            bx.BeginRenderingScale += new ProgressCallback(bx_BeginRenderingScale);
            bx.BeginRenderingTile += new ProgressCallback(bx_BeginRenderingTile);

            bx.FinishRenderingTile += new ProgressCallback(bx_FinishRenderingTile);
            bx.FinishRenderingMaps += new ProgressCallback(bx_FinishRenderingMaps);
            m_tileRuns = new List<TimeSpan>();

            m_grandTotalTiles = 0;
            foreach (BatchMap bm in m_bx.Maps)
                m_grandTotalTiles += bm.TotalTiles;

            m_grandBegin = DateTime.Now;
        }

        private void DoClose()
        {
            m_allowClose = true;

            if (m_cancel)
                this.DialogResult = DialogResult.Cancel;
            else
                this.DialogResult = DialogResult.OK;

            this.Close();
        }

        void bx_FinishRenderingMaps(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            if (this.InvokeRequired)
                this.Invoke(new System.Threading.ThreadStart(DoClose));
            else
                DoClose();
        }

        void bx_FinishRenderingTile(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
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

        private delegate void DisplayProgressDelegate(BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel);

        private void DisplayProgress(BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            if (m_cancel)
                cancel = true;

            if (this.InvokeRequired)
                this.Invoke(new DisplayProgressDelegate(DisplayProgress), new object[] { map, group, scaleindex, row, column, cancel });
            else
            {
                label1.Text = group + " in " + map.ResourceId;

                tilePG.Value = (int)Math.Max(Math.Min((m_tileCount / (double)m_totalTiles) * (tilePG.Maximum - tilePG.Minimum), tilePG.Maximum), tilePG.Minimum);
                totalPG.Value = (int)Math.Max(Math.Min((m_grandTotalTileCount / (double)m_grandTotalTiles) * (totalPG.Maximum - totalPG.Minimum), totalPG.Maximum), totalPG.Minimum);

                tileCounter.Text = string.Format("Tile {0} of {1}", m_grandTotalTileCount, m_grandTotalTiles);

                TimeSpan elapsed = DateTime.Now - m_grandBegin;
                DateTime finish = DateTime.Now + (new TimeSpan(m_prevDuration.Ticks * m_grandTotalTiles) - elapsed);
                TimeSpan remain = finish - DateTime.Now;

                if (finish < DateTime.Now)
                   finishEstimate.Text = "< Inaccurate measure of remaining time >";
                else
                    finishEstimate.Text = string.Format("{0}, remaining time: {1}:{2}:{3}", finish.ToShortTimeString(), (int)Math.Floor(remain.TotalHours), remain.Minutes.ToString("00"), remain.Seconds.ToString("00"));
            }
        }


        void bx_BeginRenderingTile(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            m_beginTile = DateTime.Now;
        }

        void bx_BeginRenderingScale(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
        }

        void bx_BeginRenderingGroup(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            m_totalTiles = map.TotalTiles;
            m_tileCount = 0;
        }

        void bx_BeginRenderingMap(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
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
            {
                MessageBox.Show(this, "I heard you the first time!\nPlease be patient.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
    }
}