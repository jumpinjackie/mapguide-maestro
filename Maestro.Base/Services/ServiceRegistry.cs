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
using Maestro.Shared.UI;
using Maestro.Base.Events;

namespace Maestro.Base.Services
{
    /// <summary>
    /// A utility class to access application-level services
    /// </summary>
    public static class ServiceRegistry
    {
        private static List<ServiceBase> _services;

        private static bool _init = false;

        /// <summary>
        /// Initializes from the AddIn registry
        /// </summary>
        /// <param name="callback"></param>
        public static void Initialize(Action callback)
        {
            if (_init)
                return;

            _services = AddInTree.BuildItems<ServiceBase>("/Maestro/ApplicationServices", null); //NOXLATE
            foreach (var svc in _services)
            {
                svc.Initialize();
                svc.Load();
            }
            _init = true;
            EventWatcher.Initialize();
            if (callback != null)
                callback();
        }

        /// <summary>
        /// Gets the application-level service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : ServiceBase
        {
            foreach (var svc in _services)
            {
                if (typeof(T).IsAssignableFrom(svc.GetType()))
                    return (T)svc;
            }
            return null;
        }
    }
}
