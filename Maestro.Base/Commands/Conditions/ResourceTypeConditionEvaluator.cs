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
using ICSharpCode.Core;

namespace Maestro.Base.Commands.Conditions
{
    internal class ResourceTypeConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition)
        {
            var types = condition.Properties["types"]; //NOXLATE
            if (types != null)
            {
                var wb = Workbench.Instance;
                if (wb != null)
                {
                    if (wb.ActiveSiteExplorer != null)
                    {
                        var resTypes = new List<string>(types.Split(',')); //NOXLATE
                        if (resTypes.Count > 0)
                        {
                            var items = wb.ActiveSiteExplorer.SelectedItems;
                            foreach (var it in items)
                            {
                                if (resTypes.Contains(it.ResourceType))
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
