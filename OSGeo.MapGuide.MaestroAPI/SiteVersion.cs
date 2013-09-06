#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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
        /// <summary>
        /// MapGuide Enterprise 2007
        /// </summary>
		MapGuideEP1_1,              
        /// <summary>
        /// MapGuide Open Source 1.1.0
        /// </summary>
		MapGuideOS1_1,              
        /// <summary>
        /// MapGuide Open Source 1.2.0
        /// </summary>
		MapGuideOS1_2,              
        /// <summary>
        /// MapGuide Enterprise 2008
        /// </summary>
		MapGuideEP1_2,              
        /// <summary>
        /// MapGuide Open Source 2.0.0 beta 1
        /// </summary>
		MapGuideOS2_0B1,            
        /// <summary>
        /// MapGuide Open Source 2.0.0
        /// </summary>
		MapGuideOS2_0,              
        /// <summary>
        /// MapGuide Enterprise 2009
        /// </summary>
		MapGuideEP2009,             
        /// <summary>
        /// MapGuide Open Source 2.0.2
        /// </summary>
        MapGuideOS2_0_2,            
        /// <summary>
        /// MapGuide Enterprise 2009 SP1
        /// </summary>
        MapGuideEP2009_SP1,         
        /// <summary>
        /// MapGuide Enterprise 2010
        /// </summary>
        MapGuideEP2010,             
        /// <summary>
        /// MapGuide Enterprise 2010 Update 1
        /// </summary>
        MapGuideEP2010_SP1,         
        /// <summary>
        /// MapGuide Enterprise 2010 Update 1b
        /// </summary>
        MapGuideEP2010_SP1b,        
        /// <summary>
        /// MapGuide Open Source 2.1.0
        /// </summary>
        MapGuideOS2_1, 
        /// <summary>
        /// MapGuide Enterprise 2011
        /// </summary>
        MapGuideEP2011,
        /// <summary>
        /// MapGuide Enterprise 2011 Update 1
        /// </summary>
        MapGuideEP2011_SP1,
        /// <summary>
        /// MapGuide Open Source 2.2.0
        /// </summary>
        MapGuideOS2_2,
        /// <summary>
        /// Autodesk Infrastructure Map Server 2012
        /// </summary>
        MapGuideEP2012,
        /// <summary>
        /// MapGuide Open Source 2.4.0
        /// </summary>
        MapGuideOS2_4,
        /// <summary>
        /// Autodesk Infrastructure Map Server 2013 SP1
        /// </summary>
        Aims2013_SP1,
        /// <summary>
        /// MapGuide Open Source 2.5.0
        /// </summary>
        MapGuideOS2_5,
        /// <summary>
        /// Autodesk Infrastructure Map Server 2014
        /// </summary>
        Aims2014,
        /// <summary>
        /// MapGuide Open Source 2.4.1
        /// </summary>
        MapGuideOS2_4_1,
        /// <summary>
        /// MapGuide Open Source 2.5.1
        /// </summary>
        MapGuideOS2_5_1,
	}

    /// <summary>
    /// Helper class containing known MapGuide Site Versions
    /// </summary>
	public class SiteVersions
	{
        /// <summary>
        /// The array of supported site versions
        /// </summary>
		public static readonly Version[] SiteVersionNumbers = new Version[] 
		{ 
			new Version(1,0,0,17864),   //MGE 2007
			new Version(1,1,0,301),     //MGOS 1.1.0
			new Version(1,2,0,1307),    //MGOS 1.2.0
			new Version(1,2,0,4103),    //MGE 2008
			new Version(2,0,0,1402),    //MGOS 2.0.0 b1
			new Version(2,0,0,2308),    //MGOS 2.0.0
			new Version(2,0,0,3202),    //MGE 2009
            new Version(2,0,2,3011),    //MGOS 2.0.2
            new Version(2,0,2,3402),    //MGE 2009 SP1
            new Version(2,1,0,3001),    //MGE 2010
            new Version(2,1,0,3505),    //MGE 2010 Update 1
            new Version(2,1,0,3701),    //MGE 2010 Update 1b
            new Version(2,1,0,4283),    //MGOS 2.1.0
            new Version(2,2,0,5305),    //MGE 2011
            new Version(2,2,0,6001),    //MGE 2011 Update 1
            new Version(2,2,0,5703),    //MGOS 2.2.0
            new Version(2,3,0,4202),    //AIMS 2012
            new Version(2,4,0,7096),    //MGOS 2.4.0
            new Version(2,4,0,5901),    //AIMS 2013 SP1
            new Version(2,5,0,7449),    //MGOS 2.5.0
            new Version(2,5,1,7601),    //AIMS 2014
            new Version(2,4,1,7767),    //MGOS 2.4.1
            new Version(2,5,1,7768)     //MGOS 2.5.1
		};

        /// <summary>
        /// Gets the specified version by the known site version identifier
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public static Version GetVersion(KnownSiteVersions index)
		{
			return SiteVersionNumbers[(int)index];
		}

	}
}
