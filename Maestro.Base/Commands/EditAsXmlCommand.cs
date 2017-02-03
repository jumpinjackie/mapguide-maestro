#region Disclaimer / License

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
using Maestro.Base.Editor;
using Maestro.Editors.Generic;
using System.Windows.Forms;

namespace Maestro.Base.Commands
{
    internal class EditAsXmlCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var ed = wb.ActiveEditor;

            if (ed != null && ed.CanEditAsXml)
            {
                var win = new XmlEditorDialog(ed.EditorService);
                win.SetXmlContent(ed.GetXmlContent(), ed.Resource.ResourceType);
                if (win.ShowDialog() == DialogResult.OK)
                {
                    //
                    //
                    //
                    ed.EditorService.UpdateResourceContent(win.XmlContent);
                    ((ResourceEditorService)ed.EditorService).ReReadSessionResource();
                    ed.EditorService = ed.EditorService;
                }
            }
        }
    }
}