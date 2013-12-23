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
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Shared.UI;
using Maestro.Base.UI;
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Capability;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.Common;

#pragma warning disable 1591

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class TestResourceCompatibilityCommand : AbstractMenuCommand
    {
        private Version _checkVersion;
        private IServerConnection _conn;

        private object BackgroundCheckResources(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
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

            var items = new List<string>();
            var test = new MockServerConnection() { SiteVersion = _checkVersion };
            var caps = new TestCapabilities(test);

            if (documents.Count == 0)
                return items.ToArray();

            var unit = 100 / documents.Count;
            var progress = 0.0;

            worker.ReportProgress(0);

            foreach (string resId in documents)
            {
                IResource res = _conn.ResourceService.GetResource(resId);
                Version ver = null;
                try
                {
                    ver = caps.GetMaxSupportedResourceVersion(res.ResourceType);
                }
                catch
                {
                    items.Add(resId);
                    continue;
                }

                //The resource's version is greater than the maximum version supported by
                //the user's selected site version
                if (res.ResourceVersion > ver)
                {
                    items.Add(resId);
                }
                progress += unit;
                worker.ReportProgress((int)progress);
            }

            worker.ReportProgress(100);
            return items.ToArray();
        }

        public override void Run()
        {
            var wb = Workbench.Instance;
            var items = wb.ActiveSiteExplorer.SelectedItems;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            _conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);

            if (items.Length > 0)
            {
                var diag = new TestResourceCompatibilityDialog();
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _checkVersion = diag.SelectedVersion;

                    var pdlg = new ProgressDialog();
                    pdlg.CancelAbortsThread = true;

                    var args = new HashSet<string>();
                    for (int i = 0; i < items.Length; i++)
                    {
                        args.Add(items[i].ResourceId);
                    }

                    var incompatibleItems = (string[])pdlg.RunOperationAsync(wb, new ProgressDialog.DoBackgroundWork(BackgroundCheckResources), args.ToArray());
                    if (incompatibleItems.Length > 0)
                    {
                        new IncompatibleResourcesDialog(_checkVersion, incompatibleItems).ShowDialog();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format(Strings.ResourcesCompatibleWithSelectedVersion, _checkVersion));
                    }
                }
            }
        }
    }

    class TestCapabilities : ConnectionCapabilities
    {
        public TestCapabilities(MockServerConnection conn)
            : base(conn)
        {

        }

        public override int[] SupportedCommands
        {
            get { throw new NotImplementedException(); }
        }

        public override bool SupportsResourcePreviews
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsMultithreaded
        {
            get { throw new NotImplementedException(); }
        }
    }

    class MockServerConnection : IServerConnection
    {
        public string ProviderName
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Specialized.NameValueCollection CloneParameters
        {
            get { throw new NotImplementedException(); }
        }

        public IServerConnection Clone()
        {
            throw new NotImplementedException();
        }

        public string[] ExecuteLoadProcedure(OSGeo.MapGuide.ObjectModels.LoadProcedure.ILoadProcedure loadProc, LengthyOperationProgressCallBack callback, bool ignoreUnsupportedFeatures)
        {
            throw new NotImplementedException();
        }

        public string[] ExecuteLoadProcedure(string resourceID, LengthyOperationProgressCallBack callback, bool ignoreUnsupportedFeatures)
        {
            throw new NotImplementedException();
        }

        public OSGeo.MapGuide.MaestroAPI.Services.IFeatureService FeatureService
        {
            get { throw new NotImplementedException(); }
        }

        public OSGeo.MapGuide.MaestroAPI.Services.IResourceService ResourceService
        {
            get { throw new NotImplementedException(); }
        }

        public OSGeo.MapGuide.MaestroAPI.Commands.ICommand CreateCommand(int commandType)
        {
            throw new NotImplementedException();
        }

        public IConnectionCapabilities Capabilities
        {
            get { throw new NotImplementedException(); }
        }

        public string SessionID
        {
            get { throw new NotImplementedException(); }
        }

        public bool AutoRestartSession
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public OSGeo.MapGuide.MaestroAPI.Services.IService GetService(int serviceType)
        {
            throw new NotImplementedException();
        }

        public Version MaxTestedVersion
        {
            get { throw new NotImplementedException(); }
        }

        public Version SiteVersion
        {
            get;
            set;
        }

        public bool DisableValidation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public OSGeo.MapGuide.MaestroAPI.CoordinateSystem.ICoordinateSystemCatalog CoordinateSystemCatalog
        {
            get { throw new NotImplementedException(); }
        }

        public string DisplayName
        {
            get { throw new NotImplementedException(); }
        }

        public void RestartSession()
        {
            throw new NotImplementedException();
        }

        public bool RestartSession(bool throwException)
        {
            throw new NotImplementedException();
        }

        public string[] GetCustomPropertyNames()
        {
            throw new NotImplementedException();
        }

        public Type GetCustomPropertyType(string name)
        {
            throw new NotImplementedException();
        }

        public void SetCustomProperty(string name, object value)
        {
            throw new NotImplementedException();
        }

        public object GetCustomProperty(string name)
        {
            throw new NotImplementedException();
        }

        public event RequestEventHandler RequestDispatched;

        public event EventHandler SessionIDChanged;

        public IMpuCalculator GetCalculator()
        {
            throw new NotImplementedException();
        }
    }
}
