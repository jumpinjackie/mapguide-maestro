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

#endregion Disclaimer / License

using Maestro.Editors.Common;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.Windows.Forms;

namespace Maestro.Editors.SymbolDefinition.GraphicsEditors
{
    internal partial class PathDialog : Form
    {
        private ISimpleSymbolDefinition _sym;
        private readonly EditorBindableCollapsiblePanel _ed;
        private IPathGraphic _path;

        public PathDialog(EditorBindableCollapsiblePanel parent, ISimpleSymbolDefinition sym, IPathGraphic path)
        {
            InitializeComponent();
            _ed = parent;
            _sym = sym;
            _path = path;
        }

        private bool _init = false;

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _init = true;

                TextBoxBinder.BindText(txtGeometry, _path, nameof(_path.Geometry));
                symFillColor.Bind(_path, nameof(_path.FillColor));
                symLineCap.Bind(_path, nameof(_path.LineCap));
                symLineColor.Bind(_path, nameof(_path.LineColor));
                symLineJoin.Bind(_path, nameof(_path.LineJoin));
                symLineMiterLimit.Bind(_path, nameof(_path.LineMiterLimit));
                symLineWeight.Bind(_path, nameof(_path.LineWeight));
                symLineWeightScalable.Bind(_path, nameof(_path.LineWeightScalable));

                IPathGraphic2 path2 = _path as IPathGraphic2;
                if (path2 != null)
                {
                    symScaleX.Bind(path2, nameof(path2.ScaleX));
                    symScaleY.Bind(path2, nameof(path2.ScaleY));
                }
                else
                {
                    tabControl1.TabPages.Remove(TAB_ADVANCED);
                }
            }
            finally
            {
                _init = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void OnContentChanged(object sender, EventArgs e) => _ed.RaiseResourceChanged();

        private void OnRequestBrowse(SymbolField sender) => ParameterSelector.ShowParameterSelector(_sym.ParameterDefinition.Parameter, sender);

        private void txtGeometry_TextChanged(object sender, EventArgs e) => _ed.RaiseResourceChanged();
    }
}