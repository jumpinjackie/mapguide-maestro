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
	/// The interface for the mapping mechanism between resource types and editors.
	/// </summary>
	public interface ResourceEditorMap
	{
		/// <summary>
		/// Gets the display name of a resourcetype
		/// </summary>
		/// <param name="itemType">The resource type to find the displayname for</param>
		/// <returns>The displayname of the item or "Unknown" if no such resource is known</returns>
		string GetResourceDisplayNameFromResourceType(string itemType);

		/// <summary>
		/// Gets the instance type of a resourcetype
		/// </summary>
		/// <param name="itemType">The resource type to find the instance type for</param>
		/// <returns>The instance type of the item or null if no such resource is known</returns>
		Type GetResourceEditorTypeFromResourceType(string itemType);

		/// <summary>
		/// Gets a list of known resource types.
		/// </summary>
		string[] AvalibleResourceTypes { get; }

		/// <summary>
		/// Gets the image index of a resource, given the resourceID
		/// </summary>
		/// <param name="resourceID">The resourceID to find the imageindex for</param>
		/// <returns>The imageindex for the resource or BlankImage, if none was found</returns>
		int GetImageIndexFromResourceID(string resourceID);


		/// <summary>
		/// Gets the image index of a resource, given the resource type
		/// </summary>
		/// <param name="resourceID">The resource type to find the imageindex for</param>
		/// <returns>The imageindex for the resource or BlankImage, if none was found</returns>
		int GetImageIndexFromResourceType(string resourceID);
		
		/// <summary>
		/// Gets the name of a resource, by removing parts from the resourceID
		/// </summary>
		/// <param name="resourceID">The resourceID to find the name for</param>
		/// <returns>The name of the resource, throws an exception for invalid resourceID's</returns>
		string GetResourceNameFromResourceID(string resourceID);

		/// <summary>
		/// Splits a resourceID into the folder/resource parts.
		/// </summary>
		/// <param name="resourceID">Thr resourceID to return the parts for.</param>
		/// <returns>The parts of the resourceID, throws an exception on invalid resourceID.</returns>
		string[] SplitResourceID(string resourceID);


		/// <summary>
		/// Returns the editor for the given resourceID.
		/// </summary>
		/// <param name="resourceID">The resourceID to find the editor for.</param>
		/// <returns>The type of the resource editor. Throws an exception on invalid resourceID, returns null if no editor was found.</returns>
		System.Type GetResourceEditorTypeFromResourceID(string resourceID);

		/// <summary>
		/// Returns the instance type for a given resourceType
		/// </summary>
		/// <param name="resourceType">The resourcetype to find the instance type for</param>
		/// <returns>The instance type, or null if no such type was found</returns>
		Type GetResourceInstanceTypeFromResourceType(string resourceType);

		/// <summary>
		/// Returns the instance type for a given resourceID
		/// </summary>
		/// <param name="resourceID">The resourceID to find the instance type for</param>
		/// <returns>The instance type, or null if no such type was found</returns>
		Type GetResourceInstanceTypeFromResourceID(string resourceID);

		/// <summary>
		/// Gets the imageindex of the folder icon.
		/// </summary>
		int FolderIcon { get; }

		/// <summary>
		/// Gets the imageindex of the blank icon.
		/// </summary>
		int BlankIcon { get; }

		/// <summary>
		/// Gets the imageindex of the server icon.
		/// </summary>
		int ServerIcon { get; }

		/// <summary>
		/// Gets the imageindex of the unknown resource icon.
		/// </summary>
		int UnknownIcon { get; }
	}
}
