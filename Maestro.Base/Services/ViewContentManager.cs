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

namespace Maestro.Base.Services
{
    public class ViewContentManager : ServiceBase
    {
        private Dictionary<string, Type> _singletonViewContentTypes;
        private List<IViewContent> _singletonInstances;

        public event EventHandler ViewHidden;

        private Workbench _wb;

        public override void Initialize()
        {
            base.Initialize();

            Workbench.WorkbenchInitialized += (sender, e) =>
            {
                _wb = Workbench.Instance;
            };

            _singletonInstances = new List<IViewContent>();
            _singletonViewContentTypes = new Dictionary<string, Type>();

            List<IViewContent> views = AddInTree.BuildItems<IViewContent>("/Maestro/Shell/SingleViewContent", null);
            _singletonInstances.AddRange(views);

            foreach (var v in views)
            {
                var type = v.GetType();
                _singletonViewContentTypes.Add(type.Name, type);
            }

            LoggingService.Info(Properties.Resources.Service_Init_ViewContent_Manager);
        }

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
                throw new InvalidOperationException(string.Format(Properties.Resources.Error_ViewContent_Type_Not_Singleton, type.Name));
            }
        }

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

                        if (_wb != null)
                            _wb.CheckContainerStatus();
                        return;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.Error_ViewContent_Type_Not_Singleton, type.Name));
            }
        }

        public void ShowContent<T>() where T : IViewContent
        {
            var type = typeof(T);
            if (_singletonViewContentTypes.ContainsKey(type.Name))
            {
                foreach (var cnt in _singletonInstances)
                {
                    if (type.IsAssignableFrom(cnt.GetType()))
                    {
                        if (!cnt.IsAttached)
                            _wb.ShowContent(cnt);
                        cnt.Activate();
                        if (_wb != null)
                            _wb.CheckContainerStatus();
                        return;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format(Properties.Resources.Error_ViewContent_Type_Not_Singleton, type.Name));
            }
        }

        public T OpenContent<T>(ViewRegion region, CreateFunc<T> method) where T : IViewContent
        {
            return OpenContent<T>(null, null, region, method);
        }

        public T OpenContent<T>(ViewRegion region) where T : IViewContent
        {
            return OpenContent<T>(null, null, region);
        }

        public T OpenContent<T>(string title, string description, ViewRegion region) where T : IViewContent
        {
            return OpenContent<T>(title, description, region, () => { return (T)Activator.CreateInstance(typeof(T), true); });
        }

        public T OpenContent<T>(string title, string description, ViewRegion region, CreateFunc<T> method) where T : IViewContent
        {
            var type = typeof(T);
            var wb = Workbench.Instance;
            if (_singletonViewContentTypes.ContainsKey(type.Name))
            {
                foreach (var cnt in _singletonInstances)
                {
                    if (type.IsAssignableFrom(cnt.GetType()))
                    {
                        if (!cnt.IsAttached)
                            _wb.ShowContent(cnt);
                        cnt.Activate();
                        if (_wb != null)
                            _wb.CheckContainerStatus();
                        return (T)cnt;
                    }
                }
            }

            T obj = method(); //(T)Activator.CreateInstance(type, true);
            SingletonViewContent svc = obj as SingletonViewContent;
            if (svc != null)
                throw new InvalidOperationException(string.Format(Properties.Resources.Error_ViewContent_Not_Registered, type.Name));

            obj.Title = title;
            obj.Description = description;
            wb.ShowContent(obj);
            return obj;
        }
    }

    public delegate T CreateFunc<T>();
}
