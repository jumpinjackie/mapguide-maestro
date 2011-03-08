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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.LayerDefinition.Vector.Scales;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.Editors.LayerDefinition;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
{
    internal partial class ScaleRangeList : UserControl
    {
        private IVectorLayerDefinition m_vldef;
        public event EventHandler ItemChanged;
        public event EventHandler SelectionChanged;
        private IVectorScaleRange m_selectedItem;
        private VectorLayerEditorCtrl m_owner;

        public bool HasUnsupportedItems
        {
            get
            {
                foreach (ScaleRange sr in this.ControlList.Controls)
                    if (sr.HasUnsupportedItems)
                        return true;

                return false;
            }
        }

        public VectorLayerEditorCtrl Owner
        {
            get { return m_owner; }
            set 
            {
                m_owner = value;
                foreach (ScaleRange sr in this.ControlList.Controls)
                    sr.Owner = m_owner;
            }
        }

        public ScaleRangeList()
        {
            m_selectedItem = null;
            InitializeComponent();
        }

        public IVectorLayerDefinition Item
        {
            get { return m_vldef; }
        }

        public void SetItem(IVectorLayerDefinition vldef)
        {
            ControlList.Controls.Clear();
            m_vldef = vldef;

            foreach (IVectorScaleRange sc in m_vldef.VectorScaleRange)
                AddScaleRange(sc);
        }

        public IVectorScaleRange SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
        }

        public ScaleRange AddScaleRange(IVectorScaleRange sc)
        {
            ScaleRange sr = new ScaleRange();
            sr.Factory = (ILayerElementFactory)m_owner.Editor.GetEditedResource();
            sr.Owner = m_owner;
            sr.SetItem(sc);
            sr.Dock = DockStyle.Top;
            ControlList.Controls.Add(sr);
            sr.BringToFront();
            sr.ItemChanged += new EventHandler(Scale_ItemChanged);
            sr.Enter += new EventHandler(Scale_Enter);
            sr.Leave += new EventHandler(Scale_Leave);
            return sr;
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

        public void RemoveScaleRange(IVectorScaleRange sc)
        {
            foreach(ScaleRange sr in ControlList.Controls)
                if (sr.Item == sc)
                {
                    ControlList.Controls.Remove(sr);
                    break;
                }
        }

        public void ResizeAuto()
        {
            foreach (ScaleRange sr in ControlList.Controls)
                sr.ResizeAuto();
        }
    }
}
