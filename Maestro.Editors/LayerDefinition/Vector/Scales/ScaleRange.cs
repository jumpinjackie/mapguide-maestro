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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System.Threading;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class ScaleRange : UserControl
    {
        private IVectorScaleRange m_vsc;
        private bool m_isUpdating = false;

        public event EventHandler ItemChanged;
        
        private VectorLayerEditorCtrl m_owner;

        public bool HasUnsupportedItems { get { return scaleRangeConditions.HasUnsupportedItems; } }

        public VectorLayerEditorCtrl Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                scaleRangeConditions.Owner = m_owner;
            }
        }

        private ILayerElementFactory _factory;

        internal ILayerElementFactory Factory
        {
            get { return _factory; }
            set
            {
                _factory = value;
                scaleRangeConditions.Factory = value;
            }
        }

        public ScaleRange()
        {
            InitializeComponent();
        }

        public IVectorScaleRange Item { get { return m_vsc; } }

        public void SetItem(IVectorScaleRange vsc)
        {
            try
            {
                m_isUpdating = true;
                m_vsc = vsc;
                MaxScale.Text = m_vsc.MaxScale.HasValue ? m_vsc.MaxScale.Value.ToString(Thread.CurrentThread.CurrentUICulture) : Properties.Resources.InfiniteValue;
                MinScale.Text = m_vsc.MinScale.HasValue ? m_vsc.MinScale.Value.ToString(Thread.CurrentThread.CurrentUICulture) : Properties.Resources.InfiniteValue;

                scaleRangeConditions.SetItem(m_vsc);
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private void MinScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            if (MinScale.SelectedIndex == 0)
                m_vsc.MinScale = null;

            if (ItemChanged != null)
                ItemChanged(m_vsc, null);
        }

        private void MaxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            if (MaxScale.SelectedIndex == 0)
                m_vsc.MaxScale = null;

            if (ItemChanged != null)
                ItemChanged(m_vsc, null);
        }

        private void MinScale_TextChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            double o;

            if (double.TryParse(MinScale.Text, System.Globalization.NumberStyles.Number, System.Threading.Thread.CurrentThread.CurrentUICulture, out o))
            {
                m_vsc.MinScale = o;
                errorProvider.SetError(MinScale, null);
            }
            else
                errorProvider.SetError(MinScale, Properties.Resources.InvalidValueError);

            if (ItemChanged != null)
                ItemChanged(m_vsc, null);
        }

        private void MaxScale_TextChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            double o;

            if (double.TryParse(MaxScale.Text, System.Globalization.NumberStyles.Number, System.Threading.Thread.CurrentThread.CurrentUICulture, out o))
            {
                m_vsc.MaxScale = o;
                errorProvider.SetError(MaxScale, null);
            }
            else
                errorProvider.SetError(MaxScale, Properties.Resources.InvalidValueError);

            if (ItemChanged != null)
                ItemChanged(m_vsc, null);
        }

        private void scaleRangeConditions_ItemChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            if (ItemChanged != null)
                ItemChanged(m_vsc, null);
        }

        public int GetPreferedHeight()
        {
            return panel2.Height + scaleRangeConditions.GetPreferedHeight();
        }

        public void ResizeAuto()
        {
            scaleRangeConditions.ResizeAuto();
            this.Height = this.GetPreferedHeight();
        }
    }
}
