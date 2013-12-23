#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using Maestro.Editors.SymbolDefinition;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class SymbolInstancePropertiesDialog : Form
    {
        private SymbolInstancePropertiesDialog()
        {
            InitializeComponent();
        }

        private IEditorService _edSvc;
        private ClassDefinition _cls;
        private string _featureSourceId;
        private string _providerName;

        public SymbolInstancePropertiesDialog(ISymbolInstance inst, IEditorService edSvc, ClassDefinition cls, string featureSourceId, string providerName)
            : this()
        {
            _edSvc = edSvc;
            _cls = cls;
            _featureSourceId = featureSourceId;
            _providerName = providerName;

            symAddToExclusionRegion.SetBooleanMode();
            symAddToExclusionRegion.Bind(inst, "AddToExclusionRegion");

            symCheckExclusionRegion.SetBooleanMode();
            symCheckExclusionRegion.Bind(inst, "CheckExclusionRegion");

            symDrawLast.SetBooleanMode();
            symDrawLast.Bind(inst, "DrawLast");

            symSizeContext.SetEnumMode<SizeContextType>();
            symSizeContext.Bind(inst, "SizeContext");

            symInsertOffsetX.Bind(inst, "InsertionOffsetX");
            symInsertOffsetY.Bind(inst, "InsertionOffsetY");

            symPositioningAlgorithm.Items = new string[]
            {
                "'Default'",
                "'EightSurrounding'",
                "'PathLabels'"
            };
            symPositioningAlgorithm.Bind(inst, "PositioningAlgorithm");

            symScaleX.Bind(inst, "ScaleX");
            symScaleY.Bind(inst, "ScaleY");

            var inst2 = inst as ISymbolInstance2;
            if (inst2 != null)
            {
                symGeometryContext.SetEnumMode<GeometryContextType>();
                symUsageContext.SetEnumMode<UsageContextType>();

                symRenderingPass.Bind(inst2, "RenderingPass");
                symGeometryContext.Bind(inst2, "GeometryContext");
                symUsageContext.Bind(inst2, "UsageContext");
            }
            else
            {
                symRenderingPass.Visible =
                symGeometryContext.Visible =
                symUsageContext.Visible =
                lblRenderingPass.Visible =
                lblGeometryContext.Visible =
                lblUsageContext.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void OnRequestBrowse(SymbolDefinition.SymbolField sender)
        {
            string expr = _edSvc.EditExpression(sender.Content, _cls, _providerName, _featureSourceId, true);
            if (expr != null)
                sender.Content = expr;
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            this.HasChanged = true;
        }

        public bool HasChanged { get; private set; }
    }
}
