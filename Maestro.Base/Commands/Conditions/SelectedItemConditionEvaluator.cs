﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

namespace Maestro.Base.Commands.Conditions
{
    internal class SelectedItemConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition)
        {
            var wb = Workbench.Instance;
            if (wb != null)
            {
                if (wb.ActiveSiteExplorer != null && wb.ActiveSiteExplorer.SelectedItems.Length == 1)
                {
                    var node = wb.ActiveSiteExplorer.SelectedItems[0];
                    string prop = condition.Properties["type"]; //NOXLATE
                    if (!string.IsNullOrEmpty(prop) && node is RepositoryItem ri)
                    {
                        return prop.ToUpper() == ri.ResourceType.ToUpper();
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}