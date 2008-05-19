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

namespace OSGeo.MapGuide.MaestroAPI
{
	///<summary>
	/// This file contains the current known versions of the MapGuide server.
	///</summary>
	public enum KnownSiteVersions
	{
		MapGuideEP1_1,
		MapGuideOS1_1,
		MapGuideOS1_2,
		MapGuideEP1_2,
		MapGuideOS2_0B1,
		MapGuideOS2_0,
		MapGuideEP2009,
	}

	public class SiteVersions
	{

		public static readonly Version[] SiteVersionNumbers = new Version[] 
		{ 
			new Version(1,0,0,17864), 
			new Version(1,1,0,301), 
			new Version(1,2,0,1307), 
			new Version(1,2,0,4103), 
			new Version(2,0,0,1402), 
			new Version(2,0,0,2308),
			new Version(2,0,0,3202),
		};

		public static Version GetVersion(KnownSiteVersions index)
		{
			return SiteVersionNumbers[(int)index];
		}

	}
}
