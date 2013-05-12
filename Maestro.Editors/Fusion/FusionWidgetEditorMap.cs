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
using System.Text;
using Maestro.Editors.Fusion.WidgetEditors;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.Fusion
{
    /// <summary>
    /// Maintains the collection of specialized widget editors
    /// </summary>
    public static class FusionWidgetEditorMap
    {
        /// <summary>
        /// Gets the editor for widget.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="context">The context.</param>
        /// <param name="edsvc">The edsvc.</param>
        /// <returns></returns>
        public static IWidgetEditor GetEditorForWidget(IWidget widget, FlexibleLayoutEditorContext context, IEditorService edsvc)
        {
            Check.NotNull(widget, "widget"); //NOXLATE
            Check.NotNull(context, "context"); //NOXLATE
            Check.NotNull(edsvc, "edsvc"); //NOXLATE

            IWidgetEditor ed = new GenericWidgetCtrl();
            ed.Setup(widget, context, edsvc);
            return ed;
        }
    }
}
