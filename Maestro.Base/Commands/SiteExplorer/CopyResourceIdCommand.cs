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
using System;
using System.Collections.Generic;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class CopyResourceIdCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;

            List<string> buffer = new List<string>();
            foreach (var item in exp.GetSelectedResources())
            {
                buffer.Add(item.ResourceId);
            }

            try
            {
                System.Windows.Forms.Clipboard.SetText(string.Join(Environment.NewLine, buffer.ToArray()));
            }
            catch
            {
            }
        }
    }
}