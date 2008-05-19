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

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// This interface describes all the required methods and properties a resource editor must have
	/// The .Net interface model does not allow an interface to describe the required constructors.
	/// The required constructors are:
	/// 
	/// Constructor(EditorInterface editor);
	/// Constructor(EditorInterface editor, string resourceID);
	/// 
	/// The first constructor is used for creating a new resource, the second for editing an existing.
	/// </summary>
	public interface ResourceEditor
	{
		/// <summary>
		/// Gets or sets the resource in its current state
		/// </summary>
		object Resource { get; set; }

		/// <summary>
		/// Refreshes the display to reflect the current object
		/// </summary>
		void UpdateDisplay();

		/// <summary>
		/// Gets or sets the ID of the current resource
		/// </summary>
		string ResourceId { get; set; }

		/// <summary>
		/// Initiates a preview, returns true if the call succeded
		/// </summary>
		bool Preview();

		/// <summary>
		/// Called before a save, to let the provider do the save, or some preliminary work.
		/// Return false to let the generic code handle the save.
		/// </summary>
		bool Save(string savename);
	}
}
