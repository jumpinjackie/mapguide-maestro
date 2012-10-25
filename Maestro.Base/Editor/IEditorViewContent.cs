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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors;
using Maestro.Shared.UI;

namespace Maestro.Base.Editor
{
    /// <summary>
    /// Defines an interface for editor view content
    /// </summary>
    public interface IEditorViewContent : IViewContent
    {
        /// <summary>
        /// Gets or sets the current resource being edited
        /// </summary>
        IEditorService EditorService { get; set; }

        /// <summary>
        /// Gets the current resource being edited
        /// </summary>
        IResource Resource { get; }

        /// <summary>
        /// Gets whether to discard unsaved changes when <see cref="Close"/> is called. If true,
        /// the normal user prompt to save unsaved changes is suppressed.
        /// </summary>
        bool DiscardChangesOnClose { get; }

        /// <summary>
        /// Closes this view
        /// </summary>
        /// <param name="discardChanges">indicates whether to discard any unsaved changes.</param>
        void Close(bool discardChanges);

        /// <summary>
        /// Gets the XML content of the edited resource
        /// </summary>
        /// <returns></returns>
        string GetXmlContent();

        /// <summary>
        /// Indicates whether this current resource is a newly created resource
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// Indicates whether this current resource can be edited with the xml editor
        /// </summary>
        bool CanEditAsXml { get; }

        /// <summary>
        /// Indicates whether this current resource can be profiled
        /// </summary>
        bool CanProfile { get; }

        /// <summary>
        /// Indicates whether this current resource can be validated
        /// </summary>
        bool CanBeValidated { get; }

        /// <summary>
        /// Indicates whether this current resource can be upgraded.
        /// </summary>
        bool CanUpgrade { get; }

        /// <summary>
        /// Indicates whether the resource being edited can be previewed
        /// </summary>
        bool CanBePreviewed { get; }

        /// <summary>
        /// Indicates whether the resource being edited has unsaved changes
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Performs a preview of the edited resource
        /// </summary>
        void Preview();

        /// <summary>
        /// Raised when the value of <see cref="IsDirty"/> changes
        /// </summary>
        event EventHandler DirtyStateChanged;

        /// <summary>
        /// Instructs the editor to write the in-memory edited resource back to the session
        /// repository. This is called before the edited resource is to be validated
        /// </summary>
        void SyncSessionCopy();
    }
}
