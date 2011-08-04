#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Editors.Common;
using System.ComponentModel;
using System.IO;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors
{
    public abstract class ResourceEditorServiceBase : IEditorService
    {
        protected IServerConnection _conn;
        protected IResource _editCopy;

        protected ResourceEditorServiceBase(string resourceID, IServerConnection conn)
        {
            this.IsNew = ResourceIdentifier.IsSessionBased(resourceID);
            this.ResourceID = resourceID;
            _conn = conn;
        }

        public event EventHandler DirtyStateChanged;

        public string EditExpression(string currentExpr, ClassDefinition classDef, string providerName, string featureSourceId)
        {
            var ed = new ExpressionEditor();
            var caps = this.FeatureService.GetProviderCapabilities(providerName);
            ed.Initialize(this.FeatureService, caps, classDef, featureSourceId);
            ed.Expression = currentExpr;
            if (ed.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return ed.Expression;
            }
            return null;
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

        protected void OnResourcePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MarkDirty();
        }

        protected void OnDirtyStateChanged()
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

        public void MarkDirty()
        {
            this.IsDirty = true;
            OnDirtyStateChanged();
        }

        public bool IsNew
        {
            get;
            private set;
        }

        public abstract void OpenUrl(string url);

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

        public abstract string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes);

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

        public abstract void RequestRefresh();

        public abstract void RequestRefresh(string folderId);

        protected void OnSaved()
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

        public abstract void OpenResource(string resourceId);

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
