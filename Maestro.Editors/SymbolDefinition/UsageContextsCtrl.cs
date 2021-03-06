﻿#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

#endregion Disclaimer / License

using Maestro.Editors.Common;
using Maestro.Editors.SymbolDefinition.GraphicsEditors;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.ComponentModel;

namespace Maestro.Editors.SymbolDefinition
{
    /// <summary>
    /// Displays usage contexts options for a Symbol Definition
    /// </summary>
    [ToolboxItem(false)]
    internal partial class UsageContextsCtrl : EditorBindableCollapsiblePanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsageContextsCtrl"/> class.
        /// </summary>
        public UsageContextsCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edSvc;
        private ISimpleSymbolDefinition _sym;

        private IPointUsage _pu;
        private ILineUsage _lu;
        private IAreaUsage _au;

        private bool _init = false;

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _sym = (ISimpleSymbolDefinition)service.GetEditedResource();
            _edSvc = service;

            _pu = _sym.PointUsage;
            _lu = _sym.LineUsage;
            _au = _sym.AreaUsage;

            try
            {
                _init = true;

                chkPoint.Checked = (_pu != null);
                chkLine.Checked = (_lu != null);
                chkArea.Checked = (_au != null);

                //Dunno why the event handler is not triggering at this point
                grpPoint.Enabled = chkPoint.Checked;
                grpLine.Enabled = chkLine.Checked;
                grpArea.Enabled = chkArea.Checked;

                if (_pu == null)
                    _pu = _sym.CreatePointUsage();

                if (_lu == null)
                    _lu = _sym.CreateLineUsage();

                if (_au == null)
                    _au = _sym.CreateAreaUsage();

                //Fill the lists fields
                symAreaAngleControl.Items = SymbolField.GetItems<AngleControl>();
                symAreaClippingControl.Items = SymbolField.GetItems<ClippingControl>();
                symAreaOriginControl.Items = SymbolField.GetItems<OriginControl>();
                symLineAngleControl.Items = SymbolField.GetItems<AngleControl>();
                symLineUnitsControl.Items = SymbolField.GetItems<UnitsControl>();
                symLineVertexControl.Items = SymbolField.GetItems<VertexControl>();
                symLineVertexJoin.Items = SymbolField.GetItems<VertexJoin>();
                symPointAngleControl.Items = SymbolField.GetItems<AngleControl>();

                symAreaAngle.Bind(_au, nameof(_au.Angle));
                symAreaAngleControl.Bind(_au, nameof(_au.AngleControl));
                symAreaBufferWidth.Bind(_au, nameof(_au.BufferWidth));
                symAreaClippingControl.Bind(_au, nameof(_au.ClippingControl));
                symAreaOriginControl.Bind(_au, nameof(_au.OriginControl));
                symAreaOriginX.Bind(_au, nameof(_au.OriginX));
                symAreaOriginY.Bind(_au, nameof(_au.OriginY));
                symAreaRepeatX.Bind(_au, nameof(_au.RepeatX));
                symAreaRepeatY.Bind(_au, nameof(_au.RepeatY));

                symLineAngle.Bind(_lu, nameof(_lu.Angle));
                symLineAngleControl.Bind(_lu, nameof(_lu.AngleControl));
                symLineEndOffset.Bind(_lu, nameof(_lu.EndOffset));
                symLineRepeat.Bind(_lu, nameof(_lu.Repeat));
                symLineStartOffset.Bind(_lu, nameof(_lu.StartOffset));
                symLineUnitsControl.Bind(_lu, nameof(_lu.UnitsControl));
                symLineVertexAngleLimit.Bind(_lu, nameof(_lu.VertexAngleLimit));
                symLineVertexControl.Bind(_lu, nameof(_lu.VertexControl));
                symLineVertexJoin.Bind(_lu, nameof(_lu.VertexJoin));
                symLineVertexMiterLimit.Bind(_lu, nameof(_lu.VertexMiterLimit));

                txtDefaultPath.Text = _lu.DefaultPath?.Geometry;

                symPointAngle.Bind(_pu, nameof(_pu.Angle));
                symPointAngleControl.Bind(_pu, nameof(_pu.AngleControl));
                symPointOriginOffsetX.Bind(_pu, nameof(_pu.OriginOffsetX));
                symPointOriginOffsetY.Bind(_pu, nameof(_pu.OriginOffsetY));
            }
            finally
            {
                _init = false;
            }
        }

        private void chkPoint_CheckedChanged(object sender, EventArgs e)
        {
            grpPoint.Enabled = chkPoint.Checked;
            if (_init)
                return;

            _sym.PointUsage = (chkPoint.Checked) ? _pu : null;
            _edSvc.MarkDirty();
        }

        private void chkLine_CheckedChanged(object sender, EventArgs e)
        {
            grpLine.Enabled = chkLine.Checked;
            if (_init)
                return;

            _sym.LineUsage = (chkLine.Checked) ? _lu : null;
            _edSvc.MarkDirty();
        }

        private void chkArea_CheckedChanged(object sender, EventArgs e)
        {
            grpArea.Enabled = chkArea.Checked;
            if (_init)
                return;

            _sym.AreaUsage = (chkArea.Checked) ? _au : null;
            _edSvc.MarkDirty();
        }

        private void OnRequestBrowse(SymbolField sender) => ParameterSelector.ShowParameterSelector(_sym.ParameterDefinition.Parameter, sender);

        private void OnContentChanged(object sender, EventArgs e) => OnResourceChanged();

        private void btnEditDefaultPath_Click(object sender, EventArgs e)
        {
            var path = _lu.DefaultPath ?? _sym.CreatePathGraphics();
            using (var diag = new PathDialog(this, _sym, path))
            {
                diag.ShowDialog();
                _lu.DefaultPath = path;
                txtDefaultPath.Text = _lu.DefaultPath.Geometry;
            }
        }

        private void btnDeleteDefaultPath_Click(object sender, EventArgs e)
        {
            _lu.DefaultPath = null;
            txtDefaultPath.Text = string.Empty;
        }
    }
}