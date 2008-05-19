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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceValidators
{
	/// <summary>
	/// Summary description for FeatureSourceValidator.
	/// </summary>
	public class FeatureSourceValidator : OSGeo.MapGuide.Maestro.ResourceEditors.ValidatorInterface
	{
		public FeatureSourceValidator()
		{
		}

		public string[] ValidateResource(object resource)
		{
			if (resource as OSGeo.MapGuide.MaestroAPI.FeatureSource == null)
				return null;

			OSGeo.MapGuide.MaestroAPI.FeatureSource feature = resource as OSGeo.MapGuide.MaestroAPI.FeatureSource;
			string s = feature.CurrentConnection.TestConnection(feature);
			if (s == null || s.Length == 0)
				return new string[0];
			else
				return new string[] {s};
		}
	}
}
