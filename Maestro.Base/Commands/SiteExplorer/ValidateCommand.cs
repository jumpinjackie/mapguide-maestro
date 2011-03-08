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
using Maestro.Base.Services;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using Maestro.Base.UI;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class ValidateCommand : AbstractMenuCommand
    {
        private IServerConnection _conn;

        public override void Run()
        {
            var wb = Workbench.Instance;
            var items = wb.ActiveSiteExplorer.SelectedItems;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            _conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);

            if (items.Length > 0)
            {
                var pdlg = new ProgressDialog();
                pdlg.CancelAbortsThread = true;

                string[] args = new string[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    args[i] = items[i].ResourceId;
                }

                var issues = (ValidationIssue[])pdlg.RunOperationAsync(wb, new ProgressDialog.DoBackgroundWork(BackgroundValidate), items[0].ResourceId);

                if (issues != null)
                {
                    if (issues.Length > 0)
                    {
                        //Sigh! LINQ would've made this code so simple...

                        var sort = new Dictionary<string, List<ValidationIssue>>();
                        foreach (var issue in issues)
                        {
                            string resId = issue.Resource.ResourceID;
                            if (!sort.ContainsKey(resId))
                                sort[resId] = new List<ValidationIssue>();

                            sort[resId].Add(issue);
                        }

                        var groupedIssues = new List<KeyValuePair<string, ValidationIssue[]>>();
                        foreach (var kvp in sort)
                        {
                            groupedIssues.Add(
                                new KeyValuePair<string, ValidationIssue[]>(
                                    kvp.Key,
                                    kvp.Value.ToArray()));
                        }

                        var resDlg = new ValidationResultsDialog(groupedIssues);
                        resDlg.Show();
                    }
                    else
                    {
                        MessageService.ShowMessage(Properties.Resources.ValidationNoIssues);
                    }
                }
            }
        }

        private object BackgroundValidate(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
            //Collect all documents to be validated. Some of these selected items
            //may be folders.
            var documents = new List<string>();
            foreach (object a in args)
            {
                string rid = a.ToString();
                if (ResourceIdentifier.Validate(rid))
                {
                    var resId = new ResourceIdentifier(rid);
                    if (resId.IsFolder)
                    {
                        foreach (IRepositoryItem o in _conn.ResourceService.GetRepositoryResources((string)args[0]).Children)
                        {
                            if (!o.IsFolder)
                            {
                                documents.Add(o.ResourceId);
                            }
                        }
                    }
                    else
                    {
                        documents.Add(rid);
                    }
                }
            }

            worker.ReportProgress(0);
            var context = new ResourceValidationContext(_conn.ResourceService, _conn.FeatureService);

            var set = new ValidationResultSet();
            int i = 0;
            foreach (string s in documents)
            {
                worker.ReportProgress((int)((i / (double)documents.Count) * 100), s);
                IResource item = null;
                try
                {
                    //TODO: This will validate resources multiple times, if they are referenced by
                    //resources inside the folder
                    item = _conn.ResourceService.GetResource(s);
                    set.AddIssues(ResourceValidatorSet.Validate(context, item, true));
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    set.AddIssue(new ValidationIssue(item, ValidationStatus.Error, string.Format(Properties.Resources.ValidationResourceLoadFailed, msg)));
                }
                i++;
                worker.ReportProgress((int)((i / (double)documents.Count) * 100), s);
            }

            return set.GetAllIssues();
        }
    }
}
