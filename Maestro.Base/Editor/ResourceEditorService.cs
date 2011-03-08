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
using Maestro.Editors;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using System.ComponentModel;
using System.IO;
using Maestro.Editors.Common;
using Maestro.Base.UI;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Base.Editor
{
    public class ResourceEditorService : IEditorService
    {
        private IUrlLauncherService _launcher;
        private IServerConnection _conn;
        private ISiteExplorer _siteExp;

        private IResource _editCopy;
        private OpenResourceManager _orm;

        public ResourceEditorService(string resourceID, IServerConnection conn, IUrlLauncherService launcher, ISiteExplorer siteExp, OpenResourceManager orm)
        {
            this.IsNew = ResourceIdentifier.IsSessionBased(resourceID);
            this.ResourceID = resourceID;
            _siteExp = siteExp;
            _conn = conn;
            _launcher = launcher;
            _orm = orm;
        }

        public event EventHandler DirtyStateChanged;

        public string EditExpression(string currentExpr, ClassDefinition schema, string providerName, string featureSourceId)
        {
            var ed = new ExpressionEditor();
            var caps = this.FeatureService.GetProviderCapabilities(providerName);
            ed.Initialize(this.FeatureService, caps, schema, featureSourceId);
            ed.Expression = currentExpr;
            if (ed.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return ed.Expression;
            }
            return null;
        }

        internal void ReReadSessionResource()
        {
            _editCopy = _conn.ResourceService.GetResource(this.EditedResourceID);
            _editCopy.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnResourcePropertyChanged);
        }

        public IResource GetEditedResource()
        {
            if (_editCopy == null)
            {
                string copy = _conn.GenerateSessionResourceId(ResourceIdentifier.GetResourceType(this.ResourceID));

                _conn.ResourceService.CopyResource(this.ResourceID, copy, true);

                _editCopy = _conn.ResourceService.GetResource(copy);
                _editCopy.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnResourcePropertyChanged);
            }
            return _editCopy;
        }

        void OnResourcePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.IsDirty = true;
            OnDirtyStateChanged();
        }

        void OnDirtyStateChanged()
        {
            var handler = this.DirtyStateChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public bool IsDirty
        {
            get;
            private set;
        }

        public bool IsNew
        {
            get;
            private set;
        }

        public void OpenUrl(string url)
        {
            _launcher.OpenUrl(url);
        }

        public string ResourceID { get; private set; }

        public string SelectAnyResource()
        {
            var picker = new ResourcePicker(_conn.ResourceService, ResourcePickerMode.OpenResource);
            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return picker.ResourceID;
            }
            return string.Empty;
        }

        public string SelectResource(OSGeo.MapGuide.MaestroAPI.ResourceTypes resType)
        {
            var picker = new ResourcePicker(_conn.ResourceService, resType, ResourcePickerMode.OpenResource);
            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return picker.ResourceID;
            }
            return string.Empty;
        }

        public string SelectFolder()
        {
            var picker = new ResourcePicker(_conn.ResourceService, ResourcePickerMode.OpenFolder);
            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return picker.ResourceID;
            }
            return string.Empty;
        }

        public string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            if (!OnBeforeSave())
            {
                _conn.ResourceService.CopyResource(this.EditedResourceID, this.ResourceID, true);

                this.IsDirty = false;
                OnDirtyStateChanged();
                OnSaved();
            }
        }

        public void SaveAs(string resourceID)
        {
            if (ResourceIdentifier.IsSessionBased(resourceID))
                throw new ArgumentException(Properties.Resources.NotSessionBasedId); //LOCALIZE

            if (!OnBeforeSave())
            {
                _conn.ResourceService.SaveResourceAs(_editCopy, resourceID);
                //Don't forget to copy attached resource data!
                _editCopy.CopyResourceDataTo(resourceID);
                this.ResourceID = resourceID;
                this.IsNew = false;
                this.IsDirty = false;
                OnDirtyStateChanged();
                OnSaved();
            }
        }

        public bool IsUpgradeAvailable
        {
            get 
            {
                if (_editCopy == null)
                    return false;
                return _conn.Capabilities.GetMaxSupportedResourceVersion(_editCopy.ResourceType) > _editCopy.ResourceVersion;
            }
        }

        public string EditedResourceID
        {
            get { return _editCopy.ResourceID; }
        }

        public void RegisterCustomNotifier(INotifyResourceChanged irc)
        {
            irc.ResourceChanged += (sender, e) =>
            {
                this.IsDirty = true;
                OnDirtyStateChanged();
            };
        }


        public void UpdateResourceContent(string xml)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                _conn.ResourceService.SetResourceXmlData(this.EditedResourceID, ms);
            }
        }

        private bool OnBeforeSave()
        {
            var e = new CancelEventArgs();
            var handler = this.BeforeSave;
            if (handler != null)
                handler(this, e);

            return e.Cancel;
        }

        public event System.ComponentModel.CancelEventHandler BeforeSave;

        public IFeatureService FeatureService
        {
            get { return _conn.FeatureService; }
        }

        public IResourceService ResourceService
        {
            get { return _conn.ResourceService; }
        }


        public string GetCoordinateSystem()
        {
            var dlg = new CoordinateSystemPicker(_conn.CoordinateSystemCatalog);
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.SelectedCoordSys.WKT;
            }
            return string.Empty;
        }


        public IDrawingService DrawingService
        {
            get { return (IDrawingService)_conn.GetService((int)ServiceType.Drawing); }
        }

        public void HasChanged()
        {
            this.IsDirty = true;
            OnDirtyStateChanged();
        }

        public string SessionID
        {
            get { return _conn.SessionID; }
        }

        public bool SupportsCommand(OSGeo.MapGuide.MaestroAPI.Commands.CommandType cmdType)
        {
            return Array.IndexOf(_conn.Capabilities.SupportedCommands, (int)cmdType) >= 0;
        }

        public OSGeo.MapGuide.MaestroAPI.Commands.ICommand CreateCommand(OSGeo.MapGuide.MaestroAPI.Commands.CommandType cmdType)
        {
            return _conn.CreateCommand((int)cmdType);
        }

        public void RequestRefresh()
        {
            _siteExp.RefreshModel();
        }

        public void RequestRefresh(string folderId)
        {
            _siteExp.RefreshModel(folderId);
        }

        private void OnSaved()
        {
            var handler = this.Saved;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler Saved;

        public void SyncSessionCopy()
        {
            this.ResourceService.SetResourceXmlData(_editCopy.ResourceID, _editCopy.SerializeToStream());
        }

        public Version SiteVersion
        {
            get { return _conn.SiteVersion; }
        }

        public void OpenResource(string resourceId)
        {
            _orm.Open(resourceId, _conn, false, _siteExp);
        }

        public string SuggestedSaveFolder
        {
            get;
            set;
        }

        public object GetCustomProperty(string name)
        {
            return _conn.GetCustomProperty(name);
        }

        public IService GetService(int serviceType)
        {
            return _conn.GetService(serviceType);
        }

        public int[] SupportedServiceTypes
        {
            get { return _conn.Capabilities.SupportedServices; }
        }
    }
}
