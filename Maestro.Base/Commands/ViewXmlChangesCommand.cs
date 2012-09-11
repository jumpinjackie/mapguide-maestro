#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.Core;
using Maestro.Editors.Diff;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using System.Xml;
using Maestro.Base.Util;

namespace Maestro.Base.Commands
{
    internal class ViewXmlChangesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            if (wb == null) return;
            var ed = wb.ActiveEditor;
            if (ed == null) return;
            var edSvc = ed.EditorService;

            TextFileDiffList sLF = null;
            TextFileDiffList dLF = null;
            string sourceFile = null;
            string targetFile = null;
            try
            {
                edSvc.SyncSessionCopy();
                XmlCompareUtil.PrepareForComparison(edSvc.ResourceService,
                                                    edSvc.ResourceID,
                                                    edSvc.EditedResourceID,
                                                    out sourceFile,
                                                    out targetFile);
                sLF = new TextFileDiffList(sourceFile);
                dLF = new TextFileDiffList(targetFile);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
                return;
            }
            finally
            {
                try { File.Delete(sourceFile); }
                catch { }
                try { File.Delete(targetFile); }
                catch { }
            }

            try
            {
                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(sLF, dLF, DiffEngineLevel.SlowPerfect);

                var rep = de.DiffReport();
                TextDiffView dlg = new TextDiffView(sLF, dLF, rep, time);
                dlg.Text += " - " + edSvc.ResourceID; //NOXLATE
                dlg.ShowDialog();
                dlg.Dispose();
            }
            catch (Exception ex)
            {
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace);
                MessageBox.Show(tmp, Strings.CompareError);
                return;
            }
        }
    }
}
