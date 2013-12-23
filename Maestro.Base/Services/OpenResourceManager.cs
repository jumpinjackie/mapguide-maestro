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
using Maestro.Shared.UI;

namespace Maestro.Base.Services
{
    /// <summary>
    /// Manages resoure editor views. This is recommeded over using the <see cref="T:Maestro.Base.Services.ViewContentManager"/> as this has built-in
    /// logic for handling cases such as:
    /// <list type="bullet">
    /// <item><description>Creating or activating an editor instance based on whether one has already been open or not</description></item>
    /// <item><description>Handling unsupported resource types and/or versions</description></item>
    /// </list>
    /// </summary>
    public class OpenResourceManager : ServiceBase
    {
        private Dictionary<string, IEditorViewContent> _openItems;

        private Dictionary<ResourceTypeDescriptor, IEditorFactory> _factories;

        /// <summary>
        /// Gets the open editor instances
        /// </summary>
        public IEditorViewContent[] OpenEditors
        {
            get { return new List<IEditorViewContent>(_openItems.Values).ToArray(); }
        }

        /// <summary>
        /// Closes the editor instances for the given resource id
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceId"></param>
        /// <param name="discardChanges"></param>
        public void CloseEditors(IServerConnection conn, string resourceId, bool discardChanges)
        {
            string key = ComputeResourceKey(resourceId, conn);
            if (_openItems.ContainsKey(key))
            {
                var ed = _openItems[key];
                ed.Close(discardChanges);
            }
        }

        /// <summary>
        /// Closes the editor instances for resources not of the specified id
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceId"></param>
        /// <param name="discardChanges"></param>
        public void CloseEditorsExceptFor(IServerConnection conn, string resourceId, bool discardChanges)
        {
            string key = ComputeResourceKey(resourceId, conn);
            var eds = new List<IEditorViewContent>(_openItems.Values);
            if (_openItems.ContainsKey(key))
                eds.Remove(_openItems[key]);

            foreach (var ed in eds)
            {
                ed.Close(discardChanges);
            }
        }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _openItems = new Dictionary<string, IEditorViewContent>();
            _factories = new Dictionary<ResourceTypeDescriptor, IEditorFactory>();

            var facts = AddInTree.BuildItems<IEditorFactory>("/Maestro/Editors", this); //NOXLATE
            foreach (var fact in facts)
            {
                if (_factories.ContainsKey(fact.ResourceTypeAndVersion))
                {
                    LoggingService.Info(string.Format(Strings.OpenResourceManager_SkipEditorRegistration, fact.ResourceTypeAndVersion.ResourceType, fact.ResourceTypeAndVersion.Version));
                }
                else
                {
                    _factories.Add(fact.ResourceTypeAndVersion, fact);
                    LoggingService.Info(string.Format(Strings.EditorRegistered, fact.GetType()));
                }
            }

            LoggingService.Info(Strings.Service_Init_Open_Resource_Manager);
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

        internal void RenameResourceId(string oldId, string newId, IServerConnection conn, ISiteExplorer siteExp)
        {
            Check.NotEmpty(oldId, "oldId"); //NOXLATE
            Check.NotEmpty(newId, "newId"); //NOXLATE
            Check.NotNull(siteExp, "siteExp"); //NOXLATE
            Check.NotNull(conn, "conn"); //NOXLATE

            string oldKey = ComputeResourceKey(oldId, conn);
            string newKey = ComputeResourceKey(newId, conn);

            if (oldKey.Equals(newKey))
                return;

            //Original must exist and new id must not
            if (_openItems.ContainsKey(oldKey) && !_openItems.ContainsKey(newKey))
            {
                var ed = _openItems[oldKey];
                _openItems.Remove(oldKey);
                _openItems[newKey] = ed;

                siteExp.FlagNode(conn.DisplayName, oldId, NodeFlagAction.None);
                siteExp.FlagNode(conn.DisplayName, newId, NodeFlagAction.HighlightOpen);
            }
        }

        internal static string ComputeResourceKey(string resId, IServerConnection conn)
        {
            return conn.DisplayName + "|" + resId; //NOXLATE
        }

        internal static string ComputeResourceKey(IResource res, IServerConnection conn)
        {
            return conn.DisplayName + "|" + res.ResourceID; //NOXLATE
        }

        /// <summary>
        /// Opens the specified resource using its assigned editor. If the resource is already
        /// open, the the existing editor view is activated instead. If the resource has no assigned
        /// editor or useXmlEditor is true, the resource will be opened in the default
        /// XML editor.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="conn"></param>
        /// <param name="useXmlEditor"></param>
        /// <param name="siteExp"></param>
        public IEditorViewContent Open(IResource res, IServerConnection conn, bool useXmlEditor, ISiteExplorer siteExp)
        {
            string key = ComputeResourceKey(res, conn);
            if (!_openItems.ContainsKey(key))
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
                var editorSvc = new ResourceEditorService(res.ResourceID, conn, launcher, siteExp, this);
                ed.EditorService = editorSvc;
                _openItems[key] = ed;
                ed.ViewContentClosing += (sender, e) =>
                {
                    if (ed.IsDirty && !ed.DiscardChangesOnClose)
                    {
                        if (ed.IsNew)
                        {
                            if (!MessageService.AskQuestion(string.Format(Strings.CloseUnsavedResource, string.Empty)))
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            using (var diag = new DirtyStateConfirmationDialog(ed.EditorService))
                            {
                                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.No)
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                    }
                };
                ed.ViewContentClosed += (sender, e) =>
                {
                    //Recompute the resource key as that may have changed by a save as operation
                    _openItems.Remove(ComputeResourceKey(((EditorContentBase)sender).EditorService.ResourceID, conn));
                    siteExp.FlagNode(conn.DisplayName, ed.EditorService.ResourceID, NodeFlagAction.None);
                };
                ed.EditorService.Saved += (sender, e) =>
                {
                    //If saved from new resource, the resource id would be session based
                    //So we need to update this to the new resource id as defined by the
                    //editor service
                    if (_openItems.ContainsKey(key))
                    {
                        var ed2 = _openItems[key];
                        _openItems.Remove(key);
                        _openItems[ComputeResourceKey(ed.EditorService.ResourceID, conn)] = ed2;
                    }
                };
                ed.DirtyStateChanged += (sender, e) =>
                {
                    siteExp.FlagNode(conn.DisplayName, res.ResourceID, ed.IsDirty ? NodeFlagAction.HighlightDirty : NodeFlagAction.HighlightOpen);
                };
            }
            _openItems[key].Activate();
            siteExp.FlagNode(conn.DisplayName, res.ResourceID, _openItems[key].IsDirty ? NodeFlagAction.HighlightDirty : NodeFlagAction.HighlightOpen);
            return _openItems[key];
        }

        /// <summary>
        /// Opens the specified resource using its assigned editor. If the resource is already
        /// open, the the existing editor view is activated instead. If the resource has no assigned
        /// editor or useXmlEditor is true, the resource will be opened in the default
        /// XML editor.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="conn"></param>
        /// <param name="useXmlEditor"></param>
        /// <param name="siteExp"></param>
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
                ErrorDialog.Show(ex);
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

        internal bool IsOpen(string resourceId, IServerConnection conn)
        {
            return _openItems.ContainsKey(ComputeResourceKey(resourceId, conn));
        }

        internal IEditorViewContent GetOpenEditor(string resourceId, IServerConnection conn)
        {
            return _openItems[ComputeResourceKey(resourceId, conn)];
        }
    }
}