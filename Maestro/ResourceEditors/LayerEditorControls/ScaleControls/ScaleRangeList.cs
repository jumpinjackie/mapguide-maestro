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

        public ScaleRangeList()
        {
            InitializeComponent();
        }

        public void SetItem(VectorLayerDefinitionType vldef)
        {
            ControlList.Controls.Clear();
            m_vldef = vldef;

            foreach (VectorScaleRangeType sc in m_vldef.VectorScaleRange)
                AddScaleRange(sc);
        }

        public void AddScaleRange(VectorScaleRangeType sc)
        {
            ScaleRange sr = new ScaleRange();
            sr.SetItem(sc);
            sr.Dock = DockStyle.Top;
            ControlList.Controls.Add(sr);
            sr.BringToFront();
            sr.ItemChanged += new EventHandler(Scale_ItemChanged);
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
