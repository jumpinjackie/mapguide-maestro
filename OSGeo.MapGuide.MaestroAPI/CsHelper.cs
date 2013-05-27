#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// An interface for calculating meters-per-unit values
    /// </summary>
    public interface IMpuCalculator
    {
        /// <summary>
        /// Calculates the meters-per-unit value
        /// </summary>
        /// <param name="csWkt">The coordinate system wkt</param>
        /// <param name="units">The number of units</param>
        /// <returns></returns>
        double Calculate(string csWkt, double units);
    }

    /// <summary>
    /// Provides application-level overrides for coordinate system related functionality.
    /// </summary>
    public static class CsHelper
    {
        private static IMpuCalculator sm_defaultCalculator;

        /// <summary>
        /// Gets or sets the default calculator
        /// </summary>
        public static IMpuCalculator DefaultCalculator
        {
            get { return sm_defaultCalculator; }
            set
            {
                sm_defaultCalculator = value;
                Debug.WriteLineIf((value != null), "Registered default MPU calculator: " + value.GetType().ToString());
            }
        }

        private static ICoordinateSystemCatalog sm_defaultCatalog;

        /// <summary>
        /// Gets or sets the default catalog
        /// </summary>
        public static ICoordinateSystemCatalog DefaultCatalog
        {
            get { return sm_defaultCatalog; }
            set
            { 
                sm_defaultCatalog = value;
                Debug.WriteLineIf((value != null), "Registered default CS catalog: " + value.GetType().ToString());
            }
        }
    }
}
