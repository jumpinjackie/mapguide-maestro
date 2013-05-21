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
using System.Linq;
using System.Text;
using Maestro.Shared.UI;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base;
using Maestro.Base.Services;
using System.IO;
using Maestro.Editors.Common;
using Microsoft.Scripting.Hosting.Shell;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Hosting;
using Maestro.AddIn.Scripting.UI;
using Maestro.Editors.Generic;
using Maestro.Base.Editor;
using Microsoft.Scripting.Hosting.Providers;
using Maestro.Editors.Preview;

namespace Maestro.AddIn.Scripting.Services
{
    /// <summary>
    /// Python built-ins injected into the Maestro IronPython REPL
    /// </summary>
    public static class ScriptGlobals
    {
        /// <summary>
        /// The Host Application
        /// </summary>
        public const string HostApp = "app"; //NOXLATE
    }

    /// <summary>
    /// A simplified helper class that is exposed to python scripts to provide
    /// convenience functionality or to workaround concepts that don't cleanly
    /// translate to IronPython (eg. Generics)
    /// </summary>
    public class HostApplication
    {
        /// <summary>
        /// The main application window
        /// </summary>
        public Workbench MainWindow
        {
            get { return Workbench.Instance; }
        }

        /// <summary>
        /// Returns a list of the names of all currently open connections
        /// </summary>
        /// <returns></returns>
        public string[] GetConnectionNames()
        {
            return ServiceRegistry.GetService<ServerConnectionManager>().GetConnectionNames().ToArray();
        }

        /// <summary>
        /// Gets the connection by its specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IServerConnection GetConnection(string name)
        {
            return ServiceRegistry.GetService<ServerConnectionManager>().GetConnection(name);
        }

        /// <summary>
        /// Gets the XML content of the given resource id
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public string GetResourceXml(IServerConnection conn, string resourceId)
        {
            var res = conn.ResourceService.GetResource(resourceId);
            return ResourceTypeRegistry.SerializeAsString(res);
        }

        /// <summary>
        /// Sets the XML content of the given resource id
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceId"></param>
        /// <param name="xml"></param>
        public void SetResourceXml(IServerConnection conn, string resourceId, string xml)
        {
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    conn.ResourceService.SetResourceXmlData(resourceId, ms);
                }
            }
            catch (Exception ex)
            {
                XmlContentErrorDialog.CheckAndHandle(ex, xml, false);
            }
        }

        /// <summary>
        /// Launches the specified url
        /// </summary>
        /// <param name="url"></param>
        public void OpenUrl(string url)
        {
            var svc = ServiceRegistry.GetService<UrlLauncherService>();
            svc.OpenUrl(url);
        }

        /// <summary>
        /// Prompts for a question that requires a boolean response
        /// </summary>
        /// <param name="title"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public bool AskQuestion(string title, string question)
        {
            return MessageService.AskQuestion(question, title);
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowMessage(string title, string message)
        {
            MessageService.ShowMessage(message, title);
        }

        /// <summary>
        /// Displays an exception in a dialog
        /// </summary>
        /// <param name="ex"></param>
        public void ShowError(Exception ex)
        {
            ErrorDialog.Show(ex);
        }

        /// <summary>
        /// Displays a resource picker for opening
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public string PickResourceOpen(IServerConnection conn, string resourceType)
        {
            Func<string> picker = () =>
            {
                using (var diag = new ResourcePicker(conn.ResourceService, (ResourceTypes)Enum.Parse(typeof(ResourceTypes), resourceType), ResourcePickerMode.OpenResource))
                {
                    if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return diag.ResourceID;
                    }
                }
                return null;
            };
            if (this.MainWindow.InvokeRequired)
            {
                var result = this.MainWindow.Invoke(picker);
                if (result != null)
                    return result.ToString();
                else
                    return null;
            }
            else
                return picker();
        }

        /// <summary>
        /// Displays a resource picker for saving
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public string PickResourceSave(IServerConnection conn, string resourceType)
        {
            Func<string> picker = () =>
            {
                using (var diag = new ResourcePicker(conn.ResourceService, (ResourceTypes)Enum.Parse(typeof(ResourceTypes), resourceType), ResourcePickerMode.SaveResource))
                {
                    if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return diag.ResourceID;
                    }
                }
                return null;
            };
            if (this.MainWindow.InvokeRequired)
            {
                var result = this.MainWindow.Invoke(picker);
                if (result != null)
                    return result.ToString();
                else
                    return null;
            }
            else
                return picker();
        }

        /// <summary>
        /// Prompts a dialog to select a folder
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public string PickFolder(IServerConnection conn)
        {
            Func<string> picker = () =>
            {
                using (var diag = new ResourcePicker(conn.ResourceService, ResourcePickerMode.OpenFolder))
                {
                    if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return diag.ResourceID;
                    }
                }
                return null;
            };
            if (this.MainWindow.InvokeRequired)
            {
                var result = this.MainWindow.Invoke(picker);
                if (result != null)
                    return result.ToString();
                else
                    return null;
            }
            else
                return picker();
        }

        /// <summary>
        /// Opens the default editor for the specified resource
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceId"></param>
        public void OpenEditor(IServerConnection conn, string resourceId)
        {
            Action action = () =>
            {
                var siteExp = this.MainWindow.ActiveSiteExplorer;
                var omgr = ServiceRegistry.GetService<OpenResourceManager>();
                omgr.Open(resourceId, conn, false, siteExp);
            };
            if (this.MainWindow.InvokeRequired)
                this.MainWindow.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Launches a preview of the given open resource
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resourceId"></param>
        /// <param name="locale"></param>
        public void PreviewResource(IServerConnection conn, string resourceId, string locale)
        {
            Action action = () =>
            {
                var siteExp = this.MainWindow.ActiveSiteExplorer;
                var omgr = ServiceRegistry.GetService<OpenResourceManager>();
                IEditorViewContent openEd = null;
                foreach (var ed in omgr.OpenEditors)
                {
                    if (ed.Resource.CurrentConnection == conn && ed.EditorService.ResourceID == resourceId)
                    {
                        openEd = ed;
                        break;
                    }
                }
                if (openEd != null)
                {
                    var previewer = ResourcePreviewerFactory.GetPreviewer(conn.ProviderName);
                    if (previewer != null)
                        previewer.Preview(openEd.Resource, openEd.EditorService, locale);
                    else
                        throw new Exception(string.Format(Strings.Error_NoPreviewer, conn.ProviderName));
                }
                else
                {
                    throw new Exception(string.Format(Strings.Error_NoOpenEditor, resourceId));
                }
            };
            if (this.MainWindow.InvokeRequired)
                this.MainWindow.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Invokes the specified method on the UI thread. Methods that interact with the UI or create UI components
        /// must be done on this thread
        /// </summary>
        /// <param name="method"></param>
        public void UIInvoke(Delegate method)
        {
            this.MainWindow.Invoke(method);
        }
    }
}
