#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
	/// An interface to provide resource validation
	/// </summary>
	public interface ValidatorInterface
	{
		/// <summary>
		/// Validates a single resource. If the validator instance does not support the resource, it should return null.
		/// </summary>
		/// <param name="resource">The resource to validate</param>
		/// <returns>Null if the resource was not recognized by the validator. An empty list if no errors were found, otherwise a list of error messages is returned</returns>
		string[] ValidateResource(object resource);
	}
}
