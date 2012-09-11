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
using Maestro.Shared.UI;
using Maestro.Editors.Generic;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Base.Commands
{
    internal class SaveResourceAsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var ed = wb.ActiveDocumentView as IEditorViewContent;
            var conn = ed.EditorService.GetEditedResource().CurrentConnection;
            
            if (ed != null)
            {
                using (var picker = new ResourcePicker(conn.ResourceService, ed.Resource.ResourceType, ResourcePickerMode.SaveResource))
                {
                    if (!string.IsNullOrEmpty(ed.EditorService.SuggestedSaveFolder))
                        picker.SetStartingPoint(ed.EditorService.SuggestedSaveFolder);

                    if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (omgr.IsOpen(picker.ResourceID, conn))
                        {
                            MessageService.ShowMessage(string.Format(Strings.CannotSaveToResourceAlreadyOpened, picker.ResourceID));
                            return;
                        }

                        using (new WaitCursor(wb))
                        {
                            var oldId = ed.EditorService.ResourceID;
                            ed.EditorService.SaveAs(picker.ResourceID);
                            if (oldId != ed.EditorService.ResourceID)
                                omgr.RenameResourceId(oldId, ed.EditorService.ResourceID, conn, exp);

                            try
                            {
                                var rid = new ResourceIdentifier(picker.ResourceID);
                                exp.RefreshModel(conn.DisplayName, rid.ParentFolder);
                            }
                            catch (NullReferenceException)
                            {
                                //HACK/FIXME: This can NRE if we created a new resource and
                                //we haven't expanded the Site Explorer for the first
                                //time. Muffling this NRE will just mean that the Site
                                //Explorer won't auto-expand to the folder where this
                                //resource was created. So nothing major.
                            }
                        }
                    }
                }
            }
        }
    }
}
