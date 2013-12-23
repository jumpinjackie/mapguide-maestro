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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using Maestro.Base.UI;
using Maestro.Base.UI.Preferences;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource.Conversion;
using System.IO;
using Maestro.Base.Services;
using Maestro.Editors.Preview;

#pragma warning disable 1591

namespace Maestro.Base.Editor
{
    /// <summary>
    /// The base class of all editor views
    /// </summary>
    /// <remarks>
    /// Although public, this class is undocumented and reserved for internal use by built-in Maestro AddIns
    /// </remarks>
    public partial class EditorContentBase : ViewContentBase, IEditorViewContent
    {
        public EditorContentBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets whether the edited resource can be upgraded
        /// </summary>
        public bool CanUpgrade
        {
            get { return upgradePanel.Visible; }
            private set { upgradePanel.Visible = value; }
        }

        /// <summary>
        /// Gets whether the editor requires to be reloaded. Usually triggered if the session of the underlying connection has expired
        /// </summary>
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RequiresReload
        {
            get { return sessionRestartPanel.Visible; }
            set { sessionRestartPanel.Visible = value; }
        }

        private IEditorService _svc;

        /// <summary>
        /// Gets or sets the editor service instance
        /// </summary>
        public IEditorService EditorService
        {
            get
            {
                return _svc;
            }
            set
            {
                if (_svc != null)
                {
                    //Just being responsible
                    _svc.DirtyStateChanged -= OnDirtyStateChanged;
                    _svc.Saved -= OnSaved;
                    _svc.BeforeSave -= OnBeforeSave;

                    var res = _svc.GetEditedResource();
                    res.CurrentConnection.SessionIDChanged -= OnSessionIdChanged;
                }

                _svc = value;
                _svc.PreviewLocale = PropertyService.Get(ConfigProperties.PreviewLocale, ConfigProperties.DefaultPreviewLocale);
                _svc.DirtyStateChanged += OnDirtyStateChanged;
                _svc.Saved += OnSaved;
                _svc.BeforeSave += OnBeforeSave;

                {
                    var res = _svc.GetEditedResource();
                    res.CurrentConnection.SessionIDChanged += OnSessionIdChanged;
                }

                UpdateTitle();

                this.CanUpgrade = _svc.IsUpgradeAvailable;

                Bind(_svc);
                //Do dirty state check
                OnDirtyStateChanged(this, EventArgs.Empty);

                //This is to ensure that save works when returning from
                //XML edit mode
                this.Focus();
            }
        }

        void OnSessionIdChanged(object sender, EventArgs e)
        {
            this.RequiresReload = true;
        }

        /// <summary>
        /// Gets the XML content of the edited resource
        /// </summary>
        /// <returns></returns>
        public virtual string GetXmlContent()
        {
            using (var sr = new System.IO.StreamReader(ResourceTypeRegistry.Serialize(this.Resource)))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets the edited resource
        /// </summary>
        public IResource Resource 
        { 
            get 
            {
                if (this.EditorService == null)
                    return null;
                return this.EditorService.GetEditedResource(); 
            } 
        }

        private void OpenAffectedResource(IResource res)
        {
            _svc.OpenResource(res.ResourceID);
        }

        /// <summary>
        /// Performs any pre-save validation logic. The base implementation performs
        /// a <see cref="ResourceValidatorSet"/> validation (non-casccading) on the 
        /// edited resource before attempting a save into the session repository 
        /// (triggering any errors relating to invalid XML content). Override this
        /// method if the base implementation just described does not cover your 
        /// validation needs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnBeforeSave(object sender, CancelEventArgs e) 
        {
            //We've been editing an in-memory model of the session copy
            //so we need to save this model back to the session copy before Save()
            //commits the changes back to the original resource
            _svc.UpdateResourceContent(GetXmlContent());
            try
            {
                var validate = PropertyService.Get(ConfigProperties.ValidateOnSave, true);
                if (this.IsDirty && validate)
                {
                    BusyWaitDelegate del = () => 
                    {
                        var errors = new List<ValidationIssue>(ValidateEditedResource()).ToArray();
                        return errors;
                    };
                    
                    BusyWaitDialog.Run(Strings.PrgPreSaveValidation, del, (result, ex) => {
                        if (ex != null)
                            throw ex;

                        ValidationIssue[] errors = result as ValidationIssue[];
                        if (errors.Length > 0)
                        {
                            MessageService.ShowError(Strings.FixErrorsBeforeSaving);
                            ValidationResultsDialog diag = new ValidationResultsDialog(this.Resource.ResourceID, errors, OpenAffectedResource);
                            diag.ShowDialog(Workbench.Instance);
                            e.Cancel = true;
                        }
                        else
                        {
                            e.Cancel = false;
                        }               
                    });
                }
                else
                {
                    LoggingService.Info("Skipping validation on save"); //NOXLATE
                    e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Gets whether this view can only be housed within the document region
        /// </summary>
        public override bool IsExclusiveToDocumentRegion
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Performs any pre-save validation on the currently edited resource, by default
        /// this returns the results of a non-cascading <see cref="ResourceValidatorSet"/>
        /// validation run. Override this if you have a custom method of validation.
        /// </summary>
        protected virtual ICollection<ValidationIssue> ValidateEditedResource() 
        {
            var context = new ResourceValidationContext(_svc.ResourceService, _svc.FeatureService);
            //Don't recurse as we only want to validate the current resource
            var issues = ResourceValidatorSet.Validate(context, this.Resource, false);
            var set = new ValidationResultSet(issues);

            var errors = set.GetIssuesForResource(this.Resource.ResourceID, ValidationStatus.Error);
            return errors;
        }

        void OnSaved(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        private string GetTooltip(string item)
        {
            return string.Format(Strings.EditorTitleTemplate, item, Environment.NewLine, this.Resource.CurrentConnection.DisplayName, this.Resource.ResourceVersion);
        }

        private void UpdateTitle()
        {
            this.Title = this.IsNew ? Strings.NewResource : ResourceIdentifier.GetName(_svc.ResourceID);
            this.Description = GetTooltip(this.IsNew ? Strings.NewResource : _svc.ResourceID);
        }

        const string DIRTY_PREFIX = "* "; //NOXLATE

        void OnDirtyStateChanged(object sender, EventArgs e)
        {
            this.IsDirty = _svc.IsDirty; //Sync states
            if (_svc.IsDirty)
            {
                if (!this.Title.StartsWith(DIRTY_PREFIX))
                    this.Title = DIRTY_PREFIX + this.Title;
            }
            else
            {
                if (this.Title.StartsWith(DIRTY_PREFIX))
                    this.Title = this.Title.Substring(1);
            }
        }

        /// <summary>
        /// Binds the specified resource to this control. This effectively initializes
        /// all the fields in this control and sets up databinding on all fields. All
        /// subclasses *must* override this method. 
        /// 
        /// Also note that this method may be called more than once (e.g. Returning from
        /// and XML edit of this resource). Thus subclasses must take this scenario into
        /// account when implementing
        /// </summary>
        /// <param name="service"></param>
        protected virtual void Bind(IEditorService service) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets whether this resource can be previewed
        /// </summary>
        public virtual bool CanBePreviewed
        {
            get
            {
                var res = this.Resource;
                if (res != null)
                {
                    var type = res.CurrentConnection.ProviderName;
                    return ResourcePreviewerFactory.IsPreviewable(type, res);
                }
                return false;
            }
        }

        /// <summary>
        /// Raised when the resource's dirty state has changed
        /// </summary>
        public event EventHandler DirtyStateChanged;

        private bool _dirty;

        /// <summary>
        /// Gets whether this resource is dirty (ie. has unsaved changes)
        /// </summary>
        public bool IsDirty
        {
            get { return _dirty; }
            set
            {
                _dirty = value;
                var handler = this.DirtyStateChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets whether this resource is a new un-saved resource
        /// </summary>
        public bool IsNew
        {
            get { return _svc != null ? _svc.IsNew : true; } //Mono
        }

        /// <summary>
        /// Gets whether this resource can be previewed
        /// </summary>
        public virtual bool CanProfile
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether this resource can be validated
        /// </summary>
        public virtual bool CanBeValidated
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether this resource can be edited in its raw XML form
        /// </summary>
        public virtual bool CanEditAsXml
        {
            get { return true; }
        }

        /// <summary>
        /// Previews this resource
        /// </summary>
        public virtual void Preview()
        {
            var conn = this.Resource.CurrentConnection;
            _svc.PrePreviewProcess();
            var previewer = ResourcePreviewerFactory.GetPreviewer(conn.ProviderName);
            if (previewer != null)
                previewer.Preview(this.Resource, this.EditorService, _svc.PreviewLocale);
        }

        /// <summary>
        /// Synchronizes the in-memory resource object back to its session-based resource id
        /// </summary>
        public virtual void SyncSessionCopy()
        {
            this.EditorService.SyncSessionCopy();
        }

        /// <summary>
        /// Gets whether to discard unsaved changes when closed
        /// </summary>
        public bool DiscardChangesOnClose
        {
            get;
            private set;
        }

        /// <summary>
        /// Closes this view
        /// </summary>
        /// <param name="discardChanges"></param>
        public virtual void Close(bool discardChanges)
        {
            this.DiscardChangesOnClose = discardChanges;
            base.Close();
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            var res = _svc.GetEditedResource();
            var conn = res.CurrentConnection;
            var ver = conn.Capabilities.GetMaxSupportedResourceVersion(res.ResourceType);

            using (new WaitCursor(this))
            {
                var conv = new ResourceObjectConverter();
                var res2 = conv.Convert(res, ver);
                
                using (var stream = ResourceTypeRegistry.Serialize(res2))
                {
                    using (var sr =new StreamReader(stream))
                    {
                        _svc.UpdateResourceContent(sr.ReadToEnd());
                        ((ResourceEditorService)_svc).ReReadSessionResource();
                    }
                }
                
                //This will re-init everything
                this.EditorService = this.EditorService;
                MessageBox.Show(string.Format(Strings.ResourceUpgraded, ver.Major, ver.Minor, ver.Build));
                this.EditorService.MarkDirty(); //It gets re-init with a clean slate, but an in-place upgrade is a dirty operation
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (this.EditorService.IsNew)
            {
                MessageService.ShowMessage(Strings.TextReloadNewResource);
                this.Close(true);
            }
            else
            {
                var omgr = ServiceRegistry.GetService<OpenResourceManager>();
                var res = this.EditorService.GetEditedResource();
                var origResId = this.EditorService.ResourceID;
                var conn = res.CurrentConnection;
                var wb = Workbench.Instance;
                this.Close();
                omgr.Open(origResId, conn, false, wb.ActiveSiteExplorer);
            }
        }
    }
}
