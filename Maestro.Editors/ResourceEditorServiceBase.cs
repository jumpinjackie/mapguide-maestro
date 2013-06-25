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
    /// <summary>
    /// A base class for providing editor services for a given resource being edited
    /// </summary>
    public abstract class ResourceEditorServiceBase : IEditorService
    {
        /// <summary>
        /// The server connection
        /// </summary>
        protected IServerConnection _conn;
        /// <summary>
        /// The resource being edited
        /// </summary>
        protected IResource _editCopy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceEditorServiceBase"/> class.
        /// </summary>
        /// <param name="resourceID">The resource ID.</param>
        /// <param name="conn">The conn.</param>
        /// <remarks>
        /// The editor service does not do live edits of the resource you pass in to this constructor
        /// 
        /// When an editor is modifying a resource, it is not modifying the resource you specify here. It is instead modifying a
        /// session-based copy of the resource that is created internally by the editor service. On a save action (a call to 
        /// <see cref="M:Maestro.Editors.ResourceEditorServiceBase.Save"/>), the session-based copy is copied back into the resource 
        /// id you specified, overwriting its contents and data files.
        /// 
        /// This provides an extra level of safety against unintentional edits, as such edits will only apply to the session-copy, only
        /// being committed back to the resource id you specified on an explicit save action.
        /// </remarks>
        protected ResourceEditorServiceBase(string resourceID, IServerConnection conn)
        {
            this.IsNew = ResourceIdentifier.IsSessionBased(resourceID);
            this.ResourceID = resourceID;
            _conn = conn;
            this.PreviewLocale = "en"; //NOXLATE
        }

        /// <summary>
        /// Gets the locale to use for previewing
        /// </summary>
        public virtual string PreviewLocale
        {
            get;
            set;
        }

        /// <summary>
        /// Raised when the edited resource has changed
        /// </summary>
        public event EventHandler DirtyStateChanged;

        /// <summary>
        /// Edits the expression.
        /// </summary>
        /// <param name="currentExpr">The current expr.</param>
        /// <param name="classDef">The class def.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="featureSourceId">The feature source id.</param>
        /// <param name="attachStylizationFunctions">If true, FDO stylization functions are also included in the function list</param>
        /// <returns></returns>
        public string EditExpression(string currentExpr, ClassDefinition classDef, string providerName, string featureSourceId, bool attachStylizationFunctions)
        {
            var ed = FdoExpressionEditorFactory.Create(); new ExpressionEditor();
            var caps = this.FeatureService.GetProviderCapabilities(providerName);
            ed.Initialize(this.FeatureService, caps, classDef, featureSourceId, attachStylizationFunctions);
            ed.Expression = currentExpr;
            if (ed.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return ed.Expression;
            }
            return null;
        }

        /// <summary>
        /// Initiates the editing process. The resource to be edited is copied to the session repository and
        /// a deserialized version is returned from this copy. Subsequent calls will return the same reference
        /// to this resource object.
        /// </summary>
        /// <returns>
        /// A deserialized version of a session-copy of the resource to be edited
        /// </returns>
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

        /// <summary>
        /// Called when [resource property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnResourcePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MarkDirty();
        }

        /// <summary>
        /// Called when [dirty state changed].
        /// </summary>
        protected void OnDirtyStateChanged()
        {
            var handler = this.DirtyStateChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Indicates whether the edited resource has unsaved changes
        /// </summary>
        public bool IsDirty
        {
            get;
            private set;
        }

        /// <summary>
        /// Forces the edited resource to be marked as dirty
        /// </summary>
        public void MarkDirty()
        {
            this.IsDirty = true;
            OnDirtyStateChanged();
        }

        /// <summary>
        /// Indicates whether the edited resource is a new resource
        /// </summary>
        public bool IsNew
        {
            get;
            private set;
        }

        /// <summary>
        /// Opens the specified URL
        /// </summary>
        /// <param name="url"></param>
        public abstract void OpenUrl(string url);

        /// <summary>
        /// Gets the resource ID of the resource, whose session-copy is being edited
        /// </summary>
        public string ResourceID { get; private set; }

        /// <summary>
        /// Invokes a prompt to select a resource of any type
        /// </summary>
        /// <returns></returns>
        public string SelectAnyResource()
        {
            var picker = new ResourcePicker(_conn.ResourceService, ResourcePickerMode.OpenResource);
            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return picker.ResourceID;
            }
            return string.Empty;
        }

        /// <summary>
        /// Invokes a prompt to select a resource of the specified type
        /// </summary>
        /// <param name="resType"></param>
        /// <returns></returns>
        public string SelectResource(OSGeo.MapGuide.MaestroAPI.ResourceTypes resType)
        {
            var picker = new ResourcePicker(_conn.ResourceService, resType, ResourcePickerMode.OpenResource);
            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return picker.ResourceID;
            }
            return string.Empty;
        }

        /// <summary>
        /// Invokes a prompt to select a folder
        /// </summary>
        /// <returns></returns>
        public string SelectFolder()
        {
            var picker = new ResourcePicker(_conn.ResourceService, ResourcePickerMode.OpenFolder);
            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return picker.ResourceID;
            }
            return string.Empty;
        }

        /// <summary>
        /// Invokes a prompt to select a file from an unmanaged alias
        /// </summary>
        /// <param name="startPath"></param>
        /// <param name="fileTypes"></param>
        /// <returns></returns>
        public abstract string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes);

        /// <summary>
        /// Saves the edited resource. The session copy, which holds the current changes is copied back
        /// to the original resource ID.
        /// </summary>
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

        /// <summary>
        /// Saves the edited resource under a different resource ID. The session copy, which holds the current changes is copied back
        /// to the specified resource ID
        /// </summary>
        /// <param name="resourceID"></param>
        public void SaveAs(string resourceID)
        {
            if (ResourceIdentifier.IsSessionBased(resourceID))
                throw new ArgumentException(Strings.NotSessionBasedId); //LOCALIZE

            if (!OnBeforeSave())
            {
                //_conn.ResourceService.SaveResourceAs(_editCopy, resourceID);
                _conn.ResourceService.CopyResource(_editCopy.ResourceID, resourceID, true);
                this.ResourceID = resourceID;
                this.IsNew = false;
                this.IsDirty = false;
                OnDirtyStateChanged();
                OnSaved();
            }
        }

        /// <summary>
        /// Indicates whether an upgrade for this resource is available
        /// </summary>
        public bool IsUpgradeAvailable
        {
            get 
            {
                if (_editCopy == null)
                    return false;
                return _conn.Capabilities.GetMaxSupportedResourceVersion(_editCopy.ResourceType) > _editCopy.ResourceVersion;
            }
        }

        /// <summary>
        /// Gets the resource ID of the actively edited resource
        /// </summary>
        public string EditedResourceID
        {
            get { return _editCopy.ResourceID; }
        }

        /// <summary>
        /// Registers a custom notifier
        /// </summary>
        /// <param name="irc"></param>
        public void RegisterCustomNotifier(INotifyResourceChanged irc)
        {
            irc.ResourceChanged += (sender, e) =>
            {
                this.IsDirty = true;
                OnDirtyStateChanged();
            };
        }

        /// <summary>
        /// Updates the session copy's resource content
        /// </summary>
        /// <param name="xml"></param>
        public void UpdateResourceContent(string xml)
        {
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    _conn.ResourceService.SetResourceXmlData(this.EditedResourceID, ms);
                }
            }
            catch (Exception ex)
            {
                XmlContentErrorDialog.CheckAndHandle(ex, xml, false);
            }
        }

        /// <summary>
        /// Called when [before save].
        /// </summary>
        /// <returns></returns>
        private bool OnBeforeSave()
        {
            var e = new CancelEventArgs();
            var handler = this.BeforeSave;
            if (handler != null)
                handler(this, e);

            return e.Cancel;
        }

        /// <summary>
        /// Raised before a save operation commences
        /// </summary>
        public event System.ComponentModel.CancelEventHandler BeforeSave;

        /// <summary>
        /// Gets the associated feature service
        /// </summary>
        public IFeatureService FeatureService
        {
            get { return _conn.FeatureService; }
        }

        /// <summary>
        /// Gets the associated resource service
        /// </summary>
        public IResourceService ResourceService
        {
            get { return _conn.ResourceService; }
        }

        /// <summary>
        /// Invokes a prompt to select the coordinate system
        /// </summary>
        /// <returns></returns>
        public string GetCoordinateSystem()
        {
            var dlg = new CoordinateSystemPicker(_conn.CoordinateSystemCatalog);
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.SelectedCoordSys.WKT;
            }
            return string.Empty;
        }


        /// <summary>
        /// Gets the associated drawing service
        /// </summary>
        public IDrawingService DrawingService
        {
            get { return (IDrawingService)_conn.GetService((int)ServiceType.Drawing); }
        }

        /// <summary>
        /// Forces the the <see cref="DirtyStateChanged"/> event. Normally the databinding
        /// system should auto-flag dirty state, only call this if you don't utilise this
        /// databinding system.
        /// </summary>
        public void HasChanged()
        {
            this.IsDirty = true;
            OnDirtyStateChanged();
        }

        /// <summary>
        /// Gets the session id
        /// </summary>
        public string SessionID
        {
            get { return _conn.SessionID; }
        }

        /// <summary>
        /// Indicates if a specified custom command is supported and can be created
        /// </summary>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public bool SupportsCommand(OSGeo.MapGuide.MaestroAPI.Commands.CommandType cmdType)
        {
            return Array.IndexOf(_conn.Capabilities.SupportedCommands, (int)cmdType) >= 0;
        }

        /// <summary>
        /// Create a custom command
        /// </summary>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public OSGeo.MapGuide.MaestroAPI.Commands.ICommand CreateCommand(OSGeo.MapGuide.MaestroAPI.Commands.CommandType cmdType)
        {
            return _conn.CreateCommand((int)cmdType);
        }

        /// <summary>
        /// Raises a request to refresh the Site Explorer
        /// </summary>
        public abstract void RequestRefresh();

        /// <summary>
        /// Raises a request to refresh the Site Explorer at the specified folder id
        /// </summary>
        /// <param name="folderId"></param>
        public abstract void RequestRefresh(string folderId);

        /// <summary>
        /// Called when [saved].
        /// </summary>
        protected void OnSaved()
        {
            var handler = this.Saved;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised when the edited resource is saved
        /// </summary>
        public event EventHandler Saved;

        /// <summary>
        /// Synchronises changes in the in-memory resource back to the session repository. This is usually called
        /// before validation of the edited resource begins.
        /// </summary>
        public void SyncSessionCopy()
        {
            string xml = ResourceTypeRegistry.SerializeAsString(_editCopy);
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    this.ResourceService.SetResourceXmlData(_editCopy.ResourceID, ms);
                }
            }
            catch (Exception ex)
            {
                XmlContentErrorDialog.CheckAndHandle(ex, xml, false);
            }
        }

        /// <summary>
        /// Gets the MapGuide Server version
        /// </summary>
        public Version SiteVersion
        {
            get { return _conn.SiteVersion; }
        }

        /// <summary>
        /// Opens the specified resource
        /// </summary>
        /// <param name="resourceId"></param>
        public abstract void OpenResource(string resourceId);

        /// <summary>
        /// Gets the suggested save folder for a "save as" operation
        /// </summary>
        public string SuggestedSaveFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value of a custom connection property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetCustomProperty(string name)
        {
            return _conn.GetCustomProperty(name);
        }

        /// <summary>
        /// Gets the service of the specified type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IService GetService(int serviceType)
        {
            return _conn.GetService(serviceType);
        }

        /// <summary>
        /// Gets the supported services
        /// </summary>
        public int[] SupportedServiceTypes
        {
            get { return _conn.Capabilities.SupportedServices; }
        }

        /// <summary>
        /// Runs the specified process with the given arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="args"></param>
        public abstract void RunProcess(string processName, params string[] args);

        /// <summary>
        /// Performs processing before a resource preview is generated
        /// </summary>
        public void PrePreviewProcess()
        {
            SyncSessionCopy();
            var handler = this.BeforePreview;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised when processing is required before a preview is generated
        /// </summary>
        public event EventHandler BeforePreview;
    }
}
