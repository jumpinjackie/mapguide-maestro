#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    /// <summary>
    /// A user control to manage composite styles
    /// </summary>
    [ToolboxItem(true)]
    public partial class CompositeStyleListCtrl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeStyleListCtrl"/> class.
        /// </summary>
        public CompositeStyleListCtrl()
        {
            InitializeComponent();
        }

        internal VectorLayerEditorCtrl Owner { get; set; }
        internal ILayerElementFactory Factory { get; set; }

        private IVectorScaleRange2 _parent;
        private BindingList<ICompositeTypeStyle> _styles;

        /// <summary>
        /// Loads the specified parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="styles">The styles.</param>
        public void LoadStyles(IVectorScaleRange2 parent, BindingList<ICompositeTypeStyle> styles)
        {
            _parent = parent;
            foreach (var ctrl in _controls.Values)
            {
                ctrl.Dispose();
            }
            _controls.Clear();
            _styles = styles;
            lstStyles.DataSource = _styles;
        }

        private Dictionary<ICompositeTypeStyle, ConditionListButtons> _controls = new Dictionary<ICompositeTypeStyle, ConditionListButtons>();

        internal void ResizeAuto()
        {
            if (splitContainer1.Panel2.Controls.Count == 1)
            {
                var ctrl = splitContainer1.Panel2.Controls[0] as ConditionListButtons;
                if (ctrl != null)
                    ctrl.ResizeAuto();
            }
        }

        private void lstStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            var style = lstStyles.SelectedItem as ICompositeTypeStyle;
            if (style != null)
            {
                btnDelete.Enabled = true;
                if (!_controls.ContainsKey(style))
                {
                    _controls[style] = new ConditionListButtons();
                    _controls[style].Owner = this.Owner;
                    _controls[style].Factory = this.Factory;
                    _controls[style].SetItem(_parent, style);

                    if (style.RuleCount == 0)
                        _controls[style].AddRule();
                }

                SetActiveControl(_controls[style]);
            }
        }

        private void SetActiveControl(ConditionListButtons conditionListButtons)
        {
            splitContainer1.Panel2.Controls.Clear();
            conditionListButtons.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(conditionListButtons);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _styles.Add(this.Factory.CreateDefaultCompositeStyle());
            _parent.CompositeStyle = _styles;
            this.Owner.RaiseResourceChanged();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var style = lstStyles.SelectedItem as ICompositeTypeStyle;
            if (style != null)
            {
                _styles.Remove(style);
                _parent.CompositeStyle = _styles;
                splitContainer1.Panel2.Controls.Clear();
                this.Owner.RaiseResourceChanged();
            }
        }
    }
}
