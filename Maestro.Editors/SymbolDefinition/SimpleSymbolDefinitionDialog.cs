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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

namespace Maestro.Editors.SymbolDefinition
{
    /// <summary>
    /// A dialog-based version of the <see cref="SimpleSymbolDefinitionEditorCtrl"/>
    /// </summary>
    public partial class SimpleSymbolDefinitionDialog : Form
    {
        private IEditorService _edSvc;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="edSvc"></param>
        /// <param name="symDef"></param>
        public SimpleSymbolDefinitionDialog(IEditorService edSvc, ISimpleSymbolDefinition symDef)
        {
            InitializeComponent();
            _edSvc = new SymbolEditorService(edSvc, symDef);
        }
    }
}
