#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// This interface represents the properties and methods avalible for a resource editor, 
	/// for interacting with the main editor window.
	/// </summary>
	public interface EditorInterface
	{
		/// <summary>
		/// Gets the current imagelist with known item icons
		/// </summary>
		System.Windows.Forms.ImageList ImageList { get; }

		/// <summary>
		/// Transforms the enumeration value int an image index for use with the list of known images
		/// </summary>
		/// <param name="iconType">The type of icon to return</param>
		/// <returns>The imageindex for the desired icon</returns>
		int ImageIndexForItem(string itemType);

		//TODO: Figure out how to loose this dependency
		/// <summary>
		/// Gets the current server connection. Can be used to retrieve the resource or other server items.
		/// </summary>
		OSGeo.MapGuide.MaestroAPI.ServerConnectionI CurrentConnection { get; }

		/// <summary>
		/// Instructs the editor to begin editing the selected resource. Focus will go to new editor with the selected resource.
		/// </summary>
		/// <param name="resourceID">The resource to edit</param>
		void EditItem(string resourceID);

		/// <summary>
		/// Instructs the editor to begin creating a new resource.
		/// </summary>
		/// <param name="itemType">The type of resource to create</param>
		void CreateItem(string itemType);

		/// <summary>
		/// Notifies the main editor that the resource has changed. This will put an asterisk (*) in the resource editor title.
		/// </summary>
		void HasChanged();

		/// <summary>
		/// Request a browse dialog for the specified resource type
		/// </summary>
		/// <param name="type">The resource to browse for</param>
		/// <returns>The name of the selected resource, or null if the user cancelled</returns>
		string BrowseResource(string itemType);

		/// <summary>
		/// Request a browse dialog for the specified resource type
		/// </summary>
		/// <param name="type">The resource to browse for, null for all valid resource types</param>
		/// <returns>The name of the selected resource, or null if the user cancelled</returns>
		string BrowseResource(string[] itemTypes);

		/// <summary>
		/// Deletes the current item from the server and removes the current page from the display
		/// </summary>
		void Delete();

		/// <summary>
		/// Closes the current interface
		/// </summary>
		/// <param name="askUser">Ask the user to save changes (if any)</param>
		/// <returns>True if the page was closed, false if the user declined or an error occured</returns>
		bool Close(bool askUser);

		/// <summary>
		/// Gets or sets a value indicating if the resource is created in the repository
		/// </summary>
		bool Existing { get; set; }

		/// <summary>
		/// Informs the control that the editor is closing
		/// </summary>
		event EventHandler Closing;

		ResourceEditorMap ResourceEditorMap { get; }

		/// <summary>
		/// Request a browse dialog for an unmanaged file
		/// </summary>
		/// <param name="startPath">The initial path of the file</param>
		/// <param name="filetypes">A list of valid file types. Key is extension, including leading period (ea: &quot;.txt&quot;). Value is text to display, (ea: &quot;Text files (*.txt)&quot;).</param>
		/// <returns>The name of the selected file, or null if the user cancelled</returns>
		string BrowseUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection filetypes);

		System.Windows.Forms.DialogResult LengthyOperation(object caller, System.Reflection.MethodInfo mi);

        /// <summary>
        /// Edits an SQL expression, ea. a filter or label expression
        /// </summary>
        /// <param name="current">The current text</param>
        /// <param name="featureSource">The featureSource this expression is executed against</param>
        /// <returns>Null if the user cancelled, otherwise the new expression</returns>
        string EditExpression(string current, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema, string providername);

	}
}
