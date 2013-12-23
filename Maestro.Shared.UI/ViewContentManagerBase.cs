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
using Maestro.Shared.UI;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// The base class for managing View Content instances
    /// </summary>
    public abstract class ViewContentManagerBase : ServiceBase, IViewContentManager
    {
        /// <summary>
        /// A dictionary of single-instance view content types
        /// </summary>
        protected Dictionary<string, Type> _singletonViewContentTypes = new Dictionary<string, Type>();

        /// <summary>
        /// A list of single-instance view content
        /// </summary>
        protected List<IViewContent> _singletonInstances = new List<IViewContent>();

        /// <summary>
        /// Raised when a view is hidden
        /// </summary>
        public event EventHandler ViewHidden;

        /// <summary>
        /// Raised when a view is activated
        /// </summary>
        public event ViewEventHandler ViewActivated;

        /// <summary>
        /// Gets the workbench
        /// </summary>
        /// <returns></returns>
        protected abstract WorkbenchBase GetWorkbench();

        /// <summary>
        /// Initializes this instance. Subclasses must override this and populate the <see cref="_singletonViewContentTypes"/>
        /// and <see cref="_singletonInstances"/> collections
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Gets whether the given single-instance view content type has already been created
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <returns></returns>
        public bool IsCreated<T>() where T : IViewContent
        {
            var type = typeof(T);
            if (_singletonViewContentTypes.ContainsKey(type.Name))
            {
                foreach (var cnt in _singletonInstances)
                {
                    if (type.IsAssignableFrom(cnt.GetType()))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                throw new InvalidOperationException(string.Format(Strings.Error_ViewContent_Type_Not_Singleton, type.Name));
            }
        }

        /// <summary>
        /// Hides the given single-instance view content
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        public void HideContent<T>() where T : IViewContent
        {
            var type = typeof(T);
            if (_singletonViewContentTypes.ContainsKey(type.Name))
            {
                foreach (var cnt in _singletonInstances)
                {
                    if (type.IsAssignableFrom(cnt.GetType()))
                    {
                        cnt.Hide();
                        var handler = this.ViewHidden;
                        if (handler != null)
                            handler(this, EventArgs.Empty);

                        //var wb = GetWorkbench();
                        //if (wb != null)
                        //    wb.CheckContainerStatus();
                        return;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format(Strings.Error_ViewContent_Type_Not_Singleton, type.Name));
            }
        }

        /// <summary>
        /// Displays the given single-instance view content
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        public void ShowContent<T>() where T : IViewContent
        {
            var type = typeof(T);
            if (_singletonViewContentTypes.ContainsKey(type.Name))
            {
                foreach (var cnt in _singletonInstances)
                {
                    if (type.IsAssignableFrom(cnt.GetType()))
                    {
                        var wb = GetWorkbench();
                        if (!cnt.IsAttached)
                            wb.ShowContent(cnt);
                        cnt.Activate();
                        var h = this.ViewActivated;
                        if (h != null)
                            h(this, cnt);
                        //wb.CheckContainerStatus();
                        return;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format(Strings.Error_ViewContent_Type_Not_Singleton, type.Name));
            }
        }

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="region">The desired region to show the view content in</param>
        /// <param name="method">A method that will create the required view content the view content has not been created yet</param>
        /// <returns></returns>
        public T OpenContent<T>(ViewRegion region, CreateFunc<T> method) where T : IViewContent
        {
            return OpenContent<T>(null, null, region, method);
        }

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="region">The desired region to show the view content in</param>
        /// <returns></returns>
        public T OpenContent<T>(ViewRegion region) where T : IViewContent
        {
            return OpenContent<T>(null, null, region);
        }

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="region">The desired region to show the view content in</param>
        /// <returns></returns>
        public T OpenContent<T>(string title, string description, ViewRegion region) where T : IViewContent
        {
            return OpenContent<T>(title, description, region, () => { return (T)Activator.CreateInstance(typeof(T), true); });
        }

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="region">The desired region to show the view content in</param>
        /// <param name="method">A method that will create the required view content the view content has not been created yet</param>
        /// <returns></returns>
        public T OpenContent<T>(string title, string description, ViewRegion region, CreateFunc<T> method) where T : IViewContent
        {
            var type = typeof(T);
            var wb = GetWorkbench();
            if (_singletonViewContentTypes.ContainsKey(type.Name))
            {
                foreach (var cnt in _singletonInstances)
                {
                    if (type.IsAssignableFrom(cnt.GetType()))
                    {
                        if (!cnt.IsAttached)
                            wb.ShowContent(cnt);
                        cnt.Activate();
                        var h = this.ViewActivated;
                        if (h != null)
                            h(this, cnt);
                        //wb.CheckContainerStatus();
                        return (T)cnt;
                    }
                }
            }

            T obj = method(); //(T)Activator.CreateInstance(type, true);
            SingletonViewContent svc = obj as SingletonViewContent;
            if (svc != null)
                throw new InvalidOperationException(string.Format(Strings.Error_ViewContent_Not_Registered, type.Name));

            obj.Title = title;
            obj.Description = description;
            wb.ShowContent(obj);
            return obj;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="content"></param>
    public delegate void ViewEventHandler(object sender, IViewContent content);
}
