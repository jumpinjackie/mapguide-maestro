#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using ICSharpCode.Core;
using Maestro.Base.Editor;

namespace Maestro.Base.Commands.Conditions
{
    internal class ActiveEditorConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition)
        {
            var wb = Workbench.Instance;
            if (wb != null)
            {
                var cnt = wb.ActiveDocumentView;
                var ed = cnt as IEditorViewContent;
                string prop = condition.Properties["property"];
                if (!string.IsNullOrEmpty(prop))
                {
                    prop = prop.ToUpper();
                    switch (prop)
                    {
                        case "CANPREVIEW":
                            return ed != null && ed.CanBePreviewed;
                        case "CANVALIDATE":
                            return ed != null && ed.CanBeValidated;
                        case "CANSAVE":
                            return ed != null && !ed.IsNew && ed.IsDirty;
                        case "CANPROFILE":
                            return ed != null && ed.CanProfile;
                        case "CANEDITASXML":
                            return ed != null && ed.CanEditAsXml;
                        default:
                            return false;
                    }
                }
                else //No property, then just see if active doc is an editor
                {
                    return ed != null;
                }
            }
            return false;
        }
    }
}
