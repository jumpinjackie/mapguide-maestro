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

namespace Maestro.Base.Editor
{
    public partial class EditorContentBase : ViewContentBase, IEditorViewContent
    {
        public EditorContentBase()
        {
            InitializeComponent();
        }

        public bool CanUpgrade
        {
            get { return upgradePanel.Visible; }
            private set { upgradePanel.Visible = value; }
        }

        private IEditorService _svc;

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
                }

                _svc = value;
                _svc.DirtyStateChanged += OnDirtyStateChanged;
                _svc.Saved += OnSaved;
                _svc.BeforeSave += OnBeforeSave;

                this.Resource = _svc.GetEditedResource();
                UpdateTitle();
                
                this.CanUpgrade = _svc.IsUpgradeAvailable;

                Bind(_svc);
            }
        }

        /// <summary>
        /// Gets the XML content of the edited resource
        /// </summary>
        /// <returns></returns>
        public virtual string GetXmlContent()
        {
            return this.Resource.Serialize();
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
                var errors = new List<ValidationIssue>(ValidateEditedResource()).ToArray();
                if (errors.Length > 0)
                {
                    MessageService.ShowError(Properties.Resources.FixErrorsBeforeSaving);
                    ValidationResultsDialog diag = new ValidationResultsDialog(this.Resource.ResourceID, errors);
                    diag.ShowDialog(Workbench.Instance);
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                e.Cancel = true;
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

        private void UpdateTitle()
        {
            this.Title = this.IsNew ? Properties.Resources.NewResource : ResourceIdentifier.GetName(_svc.ResourceID);
            this.Description = this.IsNew ? Properties.Resources.NewResource : _svc.ResourceID;
        }

        const string DIRTY_PREFIX = "* ";

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

        public IResource Resource { get; private set; }

        /// <summary>
        /// Binds the specified resource to this control. This effectively initializes
        /// all the fields in this control and sets up databinding on all fields. All
        /// subclasses *must* override this method. 
        /// 
        /// Also note that this method may be called more than once (e.g. Returning from
        /// and XML edit of this resource). Thus subclasses must take this scenario into
        /// account when implementing
        /// </summary>
        /// <param name="value"></param>
        protected virtual void Bind(IEditorService service) 
        {
            throw new NotImplementedException();
        }

        public virtual bool CanBePreviewed
        {
            get
            {
                var res = this.Resource;
                if (res != null)
                {
                    var rt = res.ResourceType;
                    return ResourcePreviewEngine.IsPreviewableType(rt) && res.CurrentConnection.Capabilities.SupportsResourcePreviewUrls;                    
                }
                return false;
            }
        }

        public event EventHandler DirtyStateChanged;

        private bool _dirty;

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

        public bool IsNew
        {
            get { return _svc != null ? _svc.IsNew : true; } //Mono
        }

        public virtual bool CanProfile
        {
            get { return false; }
        }

        public virtual bool CanBeValidated
        {
            get { return true; }
        }

        public virtual bool CanEditAsXml
        {
            get { return true; }
        }

        public virtual string SetupPreviewUrl(string mapguideRootUrl)
        {
            //Save the current resource to another session copy
            string resId = "Session:" + this.EditorService.SessionID + "//" + Guid.NewGuid() + "." + this.Resource.ResourceType.ToString();
            this.EditorService.ResourceService.SetResourceXmlData(resId, this.Resource.SerializeToStream());

            //Copy any resource data
            var previewCopy = this.EditorService.ResourceService.GetResource(resId);
            this.Resource.CopyResourceDataTo(previewCopy);

            //Now feed it to the preview engine
            return new ResourcePreviewEngine(mapguideRootUrl, this.EditorService).GeneratePreviewUrl(previewCopy);
        }

        public virtual void SyncSessionCopy()
        {
            this.EditorService.SyncSessionCopy();
        }

        public bool DiscardChangesOnClose
        {
            get;
            private set;
        }

        public virtual void Close(bool discardChanges)
        {
            this.DiscardChangesOnClose = discardChanges;
            base.Close();
        }
    }
}
