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
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

namespace Maestro.Editors.SymbolDefinition
{
    internal class SymbolEditorService : IEditorService
    {
        private IEditorService _inner;
        private ISymbolDefinitionBase _symDef;

        public SymbolEditorService(IEditorService edSvc, ISymbolDefinitionBase symDef)
        {
            _inner = edSvc;
            _symDef = symDef;
            this.PreviewLocale = "en"; //NOXLATE
        }

        public bool SupportsCommand(CommandType cmdType)
        {
            return _inner.SupportsCommand(cmdType);
        }

        public ICommand CreateCommand(CommandType cmdType)
        {
            return _inner.CreateCommand(cmdType);
        }

        public IFeatureService FeatureService
        {
            get { return _inner.FeatureService; }
        }

        public IResourceService ResourceService
        {
            get { return _inner.ResourceService; }
        }

        public IDrawingService DrawingService
        {
            get { return _inner.DrawingService; }
        }

        public string SessionID
        {
            get { return _inner.SessionID; }
        }

        public string SuggestedSaveFolder
        {
            get
            {
                return _inner.SuggestedSaveFolder;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public void RegisterCustomNotifier(INotifyResourceChanged irc)
        {
            _inner.RegisterCustomNotifier(irc);
        }

        public bool IsUpgradeAvailable
        {
            get { return false; }
        }

        public string SelectAnyResource()
        {
            return _inner.SelectAnyResource();
        }

        public string SelectResource(ResourceTypes resType)
        {
            return _inner.SelectResource(resType);
        }

        public string SelectFolder()
        {
            return _inner.SelectFolder();
        }

        public void UpdateResourceContent(string xml)
        {
            throw new NotSupportedException();
        }

        public string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes)
        {
            return _inner.SelectUnmanagedData(startPath, fileTypes);
        }

        public string EditExpression(string currentExpr, OSGeo.MapGuide.MaestroAPI.Schema.ClassDefinition schema, string providerName, string featureSourceId, bool attachStylizationFunctions)
        {
            return _inner.EditExpression(currentExpr, schema, providerName, featureSourceId, attachStylizationFunctions);
        }

        public string ResourceID
        {
            get { return _inner.ResourceID; }
        }

        public string EditedResourceID
        {
            get { return _inner.EditedResourceID; }
        }

        public OSGeo.MapGuide.MaestroAPI.Resource.IResource GetEditedResource()
        {
            return _symDef;
        }

        public event System.ComponentModel.CancelEventHandler BeforeSave;

        public void Save()
        {
            throw new NotSupportedException();
        }

        public void SaveAs(string resourceID)
        {
            throw new NotSupportedException();
        }

        public void OpenUrl(string url)
        {
            throw new NotSupportedException();
        }

        public bool IsNew
        {
            get { return _inner.IsNew; }
        }

        public bool IsDirty
        {
            get { throw new NotImplementedException(); }
        }

        public void MarkDirty()
        {
            _inner.MarkDirty();
        }

        public event EventHandler DirtyStateChanged;

        public string GetCoordinateSystem()
        {
            return _inner.GetCoordinateSystem();
        }

        public void HasChanged()
        {
            _inner.HasChanged();
        }

        public void RequestRefresh()
        {
            throw new NotSupportedException();
        }

        public void RequestRefresh(string folderId)
        {
            throw new NotSupportedException();
        }

        public event EventHandler Saved;

        public void SyncSessionCopy()
        {
            _inner.SyncSessionCopy();
        }

        public Version SiteVersion
        {
            get { return _inner.SiteVersion; }
        }

        public void OpenResource(string resourceId)
        {
            _inner.OpenResource(resourceId);
        }

        public int[] SupportedServiceTypes
        {
            get { return _inner.SupportedServiceTypes; }
        }

        public IService GetService(int serviceType)
        {
            return _inner.GetService(serviceType);
        }

        public object GetCustomProperty(string name)
        {
            return _inner.GetCustomProperty(name);
        }

        public void RunProcess(string processName, params string[] args)
        {
            throw new NotImplementedException();
        }

        public void PrePreviewProcess()
        {
            var handler = this.BeforePreview;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler BeforePreview;

        public string PreviewLocale
        {
            get;
            set;
        }
    }
}
