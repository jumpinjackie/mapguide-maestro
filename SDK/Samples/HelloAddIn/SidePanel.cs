using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;
using Maestro.Base.UI;
using Maestro.Base;
using Maestro.Base.Services;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels.Common;

namespace HelloAddIn
{
    public partial class SidePanel : SingletonViewContent
    {
        public SidePanel()
        {
            InitializeComponent();
            this.Title = "Side Panel";
        }

        private ISiteExplorer _siteExplorer;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _siteExplorer = Workbench.Instance.ActiveSiteExplorer;
            _siteExplorer.ItemsSelected += new RepositoryItemEventHandler(OnItemsSelected);
            CheckButtonState(new RepositoryItem[0]);
        }

        void OnItemsSelected(object sender, RepositoryItem[] items)
        {
            CheckButtonState(items);
        }

        private void CheckButtonState(RepositoryItem[] items)
        {
            btnOpenEditor.Enabled = false;
            btnValidate.Enabled = false;

            if (items.Length == 1)
            {
                btnOpenEditor.Enabled = (!items[0].IsFolder);
                btnValidate.Enabled = true;
            }
        }

        /// <summary>
        /// Gets the default preferred region this view will "dock" to. For this example we prefer the view docks
        /// to the right
        /// </summary>
        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Right;
            }
        }

        private void btnOpenEditor_Click(object sender, EventArgs e)
        {
            //ServerConnectionManager manages the open site connections
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();

            //OpenResourceManager manages the display of resource editor views. This is strongly recommended over using the
            //ViewContentManager as it knows about what type of editor to create, if it is already open (which in that case
            //it will activate that instance instead), and knowing whether to fall-back to a generic XML editor (for unknown
            //resource types and/or unsupported schema versions)
            var openMgr = ServiceRegistry.GetService<OpenResourceManager>();

            //The connection name property of the Site Explorer gives us the name of the active connection. The active connection
            //is the connection that the selected item belongs to.
            string connName = _siteExplorer.ConnectionName;

            //We use the connection name to obtain the IServerConnection instance from the ServerConnectionManager
            var conn = connMgr.GetConnection(connName);

            //We normally shouldn't dive in like this (ie. We should check the number of items and if it's the valid type
            //but we already validated this condition in OnItemsSelected, meaning when this method executes, this next line
            //should never be illegal.
            var resId = _siteExplorer.SelectedItems[0].ResourceId;

            openMgr.Open(resId, conn, false, _siteExplorer);
        }

        protected object BackgroundValidate(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
            //Collect all documents to be validated. Some of these selected items
            //may be folders.
            var documents = new List<string>();
            var conn = (IServerConnection)args[0];
            foreach (object a in args.Skip(1))
            {
                string rid = a.ToString();
                if (ResourceIdentifier.Validate(rid))
                {
                    var resId = new ResourceIdentifier(rid);
                    if (resId.IsFolder)
                    {
                        foreach (IRepositoryItem o in conn.ResourceService.GetRepositoryResources(rid).Children)
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
            var context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);

            var set = new ValidationResultSet();
            int i = 0;
            foreach (string s in documents)
            {
                worker.ReportProgress((int)((i / (double)documents.Count) * 100), s);
                IResource item = null;
                try
                {
                    item = conn.ResourceService.GetResource(s);
                    set.AddIssues(ResourceValidatorSet.Validate(context, item, true));
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    set.AddIssue(new ValidationIssue(item, ValidationStatus.Error, ValidationStatusCode.Error_General_ValidationError, string.Format("Failed to validate resource: {0}", msg)));
                }
                i++;
                worker.ReportProgress((int)((i / (double)documents.Count) * 100), s);
            }

            return set.GetAllIssues();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            var wb = Workbench.Instance;
            var items = wb.ActiveSiteExplorer.SelectedItems;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);

            if (items.Length > 0)
            {
                var pdlg = new ProgressDialog();
                pdlg.CancelAbortsThread = true;

                object[] args = new object[items.Length + 1];
                args[0] = conn;
                for (int i = 0; i < items.Length; i++)
                {
                    args[i+1] = items[i].ResourceId;
                }

                var issues = (ValidationIssue[])pdlg.RunOperationAsync(wb, new ProgressDialog.DoBackgroundWork(BackgroundValidate), args);

                CollectAndDisplayIssues(issues);
            }
        }

        static void CollectAndDisplayIssues(ValidationIssue[] issues)
        {
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
                    MessageService.ShowMessage("No validation issues found");
                }
            }
        }
    }
}
