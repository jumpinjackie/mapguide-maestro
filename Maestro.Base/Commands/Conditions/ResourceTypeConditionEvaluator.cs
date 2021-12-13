#region Disclaimer / License

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

using ICSharpCode.Core;
using Maestro.Base.UI;
using System.Collections.Generic;
using System.Linq;

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
                            foreach (var it in wb.ActiveSiteExplorer.GetSelectedResources())
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