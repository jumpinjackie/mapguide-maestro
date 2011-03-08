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
using Maestro.Base.UI;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base.Editor;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Base.Services
{
    public class OpenResourceManager : ServiceBase
    {
        private Dictionary<string, IEditorViewContent> _openItems;

        private Dictionary<ResourceTypeDescriptor, IEditorFactory> _factories;

        public IEditorViewContent[] OpenEditors
        {
            get { return new List<IEditorViewContent>(_openItems.Values).ToArray(); }
        }

        public void CloseEditors(string resourceId, bool discardChanges)
        {
            if (_openItems.ContainsKey(resourceId))
            {
                var ed = _openItems[resourceId];
                ed.Close(discardChanges);
            }
        }

        public void CloseEditorsExceptFor(string resourceId, bool discardChanges)
        {
            var eds = new List<IEditorViewContent>(_openItems.Values);
            if (_openItems.ContainsKey(resourceId))
                eds.Remove(_openItems[resourceId]);

            foreach (var ed in eds)
            {
                ed.Close(discardChanges);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            _openItems = new Dictionary<string, IEditorViewContent>();
            _factories = new Dictionary<ResourceTypeDescriptor, IEditorFactory>();

            var facts = AddInTree.BuildItems<IEditorFactory>("/Maestro/Editors", this);
            foreach (var fact in facts)
            {
                if (_factories.ContainsKey(fact.ResourceTypeAndVersion))
                {
                    LoggingService.Info(string.Format(Properties.Resources.OpenResourceManager_SkipEditorRegistration, fact.ResourceTypeAndVersion.ResourceType, fact.ResourceTypeAndVersion.Version));
                }
                else
                {
                    _factories.Add(fact.ResourceTypeAndVersion, fact);
                    LoggingService.Info(string.Format(Properties.Resources.EditorRegistered, fact.GetType()));
                }
            }

            LoggingService.Info(Properties.Resources.Service_Init_Open_Resource_Manager);
        }

        private IEditorFactory GetRegisteredEditor(ResourceTypeDescriptor rtd)
        {
            if (_factories.ContainsKey(rtd))
                return _factories[rtd];
            else
                return null;
        }

        private IEditorFactory GetRegisteredEditor(ResourceTypes type, string version)
        {
            var rtd = new ResourceTypeDescriptor(type, version);
            return GetRegisteredEditor(rtd);
        }

        /// <summary>
        /// Opens the specified resource using its assigned editor. If the resource is already
        /// open, the the existing editor view is activated instead. If the resource has no assigned
        /// editor or <see cref="useXmlEditor"/> is true, the resource will be opened in the default
        /// XML editor.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="conn"></param>
        /// <param name="useXmlEditor"></param>
        public IEditorViewContent Open(IResource res, IServerConnection conn, bool useXmlEditor, ISiteExplorer siteExp)
        {
            string resourceId = res.ResourceID;
            if (!_openItems.ContainsKey(resourceId))
            {
                var svc = ServiceRegistry.GetService<ViewContentManager>();
                IEditorViewContent ed = null;
                if (useXmlEditor || !res.IsStronglyTyped)
                {
                    ed = svc.OpenContent<XmlEditor>(ViewRegion.Document);
                }
                else
                {
                    ed = FindEditor(svc, res.GetResourceTypeDescriptor());
                }
                var launcher = ServiceRegistry.GetService<UrlLauncherService>();
                var editorSvc = new ResourceEditorService(resourceId, conn, launcher, siteExp, this);
                ed.EditorService = editorSvc;
                _openItems[resourceId] = ed;
                ed.ViewContentClosing += (sender, e) =>
                {
                    if (ed.IsDirty && !ed.DiscardChangesOnClose)
                    {
                        string name = ed.IsNew ? string.Empty : "(" + ResourceIdentifier.GetName(ed.EditorService.ResourceID) + ")";
                        if (!MessageService.AskQuestion(string.Format(Properties.Resources.CloseUnsavedResource, name)))
                        {
                            e.Cancel = true;
                        }
                    }
                };
                ed.ViewContentClosed += (sender, e) =>
                {
                    _openItems.Remove(resourceId);
                    siteExp.FlagNode(resourceId, NodeFlagAction.None);
                };
                ed.DirtyStateChanged += (sender, e) =>
                {
                    siteExp.FlagNode(resourceId, ed.IsDirty ? NodeFlagAction.HighlightDirty : NodeFlagAction.HighlightOpen);
                };
            }
            _openItems[resourceId].Activate();
            siteExp.FlagNode(resourceId, NodeFlagAction.HighlightOpen);
            return _openItems[resourceId];
        }

        /// <summary>
        /// Opens the specified resource using its assigned editor. If the resource is already
        /// open, the the existing editor view is activated instead. If the resource has no assigned
        /// editor or <see cref="useXmlEditor"/> is true, the resource will be opened in the default
        /// XML editor.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="conn"></param>
        /// <param name="useXmlEditor"></param>
        public IEditorViewContent Open(string resourceId, IServerConnection conn, bool useXmlEditor, ISiteExplorer siteExp)
        {
            IResource res = null;
            try
            {
                res = (IResource)conn.ResourceService.GetResource(resourceId);
                return Open(res, conn, useXmlEditor, siteExp);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return null;
            }
        }

        private IEditorViewContent FindEditor(ViewContentManager svc, ResourceTypeDescriptor rtd)
        {
            IEditorViewContent ed = null;
            IEditorFactory fact = GetRegisteredEditor(rtd);

            //No registered editor, use the xml editor fallback
            if (fact == null)
            {
                ed = svc.OpenContent<XmlEditor>(ViewRegion.Document);
            }
            else
            {
                //I LOVE ANONYMOUS DELEGATES!
                ed = svc.OpenContent(ViewRegion.Document, () => { return fact.Create(); });
            }

            if (ed == null)
                ed = svc.OpenContent<XmlEditor>(ViewRegion.Document);

            return ed;
        }

        internal bool IsOpen(string resourceId)
        {
            return _openItems.ContainsKey(resourceId);
        }

        internal IEditorViewContent GetOpenEditor(string resourceId)
        {
            return _openItems[resourceId];
        }
    }
}
