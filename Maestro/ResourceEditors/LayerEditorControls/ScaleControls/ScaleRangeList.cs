using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
{
    public partial class ScaleRangeList : UserControl
    {
        private VectorLayerDefinitionType m_vldef;
        public event EventHandler ItemChanged;
        public event EventHandler SelectionChanged;
        private VectorScaleRangeType m_selectedItem;
        private VectorLayer m_owner;

        public VectorLayer Owner
        {
            get { return m_owner; }
            set 
            {
                m_owner = value;
                foreach (ScaleRange sr in this.ControlList.Controls)
                    sr.Owner = m_owner; ;
            }
        }

        public ScaleRangeList()
        {
            m_selectedItem = null;
            InitializeComponent();
        }

        public VectorLayerDefinitionType Item
        {
            get { return m_vldef; }
        }

        public void SetItem(VectorLayerDefinitionType vldef)
        {
            ControlList.Controls.Clear();
            m_vldef = vldef;

            foreach (VectorScaleRangeType sc in m_vldef.VectorScaleRange)
                AddScaleRange(sc);
        }

        public VectorScaleRangeType SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
        }

        public void AddScaleRange(VectorScaleRangeType sc)
        {
            ScaleRange sr = new ScaleRange();
            sr.Owner = m_owner;
            sr.SetItem(sc);
            sr.Dock = DockStyle.Top;
            ControlList.Controls.Add(sr);
            sr.BringToFront();
            sr.ItemChanged += new EventHandler(Scale_ItemChanged);
            sr.Enter += new EventHandler(Scale_Enter);
            sr.Leave += new EventHandler(Scale_Leave);
        }

        void Scale_Leave(object sender, EventArgs e)
        {
            m_selectedItem = null;
            if (SelectionChanged != null)
                SelectionChanged(this, null);

            if (sender as ScaleRange != null)
                (sender as ScaleRange).BackColor = SystemColors.Control;
        }

        void Scale_Enter(object sender, EventArgs e)
        {
            m_selectedItem = sender as ScaleRange == null ? null : (sender as ScaleRange).Item;
            if (SelectionChanged != null)
                SelectionChanged(this, null);

            if (sender as ScaleRange != null)
                (sender as ScaleRange).BackColor = Color.FromArgb(Math.Min(SystemColors.Control.R + 20, 255), Math.Min(SystemColors.Control.G + 20, 255), Math.Min(SystemColors.Control.B + 20, 255));
        }

        void Scale_ItemChanged(object sender, EventArgs e)
        {
            if (ItemChanged != null)
                ItemChanged(m_vldef, null);
        }

        public void RemoveScaleRange(VectorScaleRangeType sc)
        {
            foreach(ScaleRange sr in ControlList.Controls)
                if (sr.Item == sc)
                {
                    ControlList.Controls.Remove(sr);
                    break;
                }
        }
    }
}
