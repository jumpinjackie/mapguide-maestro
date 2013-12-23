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

namespace Maestro.Shared.UI
{
    /// <summary>
    /// A method that creates an object of the given type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public delegate T CreateFunc<T>();

    /// <summary>
    /// Manages <see cref="T:Maestro.Shared.UI.IViewContent"/> instances
    /// </summary>
    public interface IViewContentManager
    {
        /// <summary>
        /// Hides the given single-instance view content
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        void HideContent<T>() where T : IViewContent;

        /// <summary>
        /// Initializes this instance
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets whether the given single-instance view content type has already been created
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <returns></returns>
        bool IsCreated<T>() where T : IViewContent;

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="region">The desired region to show the view content in</param>
        /// <param name="method">A method that will create the required view content the view content has not been created yet</param>
        /// <returns></returns>
        T OpenContent<T>(ViewRegion region, CreateFunc<T> method) where T : IViewContent;

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="region">The desired region to show the view content in</param>
        /// <returns></returns>
        T OpenContent<T>(ViewRegion region) where T : IViewContent;

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="region">The desired region to show the view content in</param>
        /// <param name="method">A method that will create the required view content the view content has not been created yet</param>
        /// <returns></returns>
        T OpenContent<T>(string title, string description, ViewRegion region, CreateFunc<T> method) where T : IViewContent;

        /// <summary>
        /// Creates and/or opens the given view content in the specified region
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="region">The desired region to show the view content in</param>
        /// <returns></returns>
        T OpenContent<T>(string title, string description, ViewRegion region) where T : IViewContent;

        /// <summary>
        /// Displays the given single-instance view content
        /// </summary>
        /// <typeparam name="T">The type implementing the <see cref="T:Maestro.Shared.UI.IViewContent"/> interface</typeparam>
        void ShowContent<T>() where T : IViewContent;

        /// <summary>
        /// Raised when a view is hidden
        /// </summary>
        event EventHandler ViewHidden;
    }
}
