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
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Utility class that initializes the default set of resource validators
    /// </summary>
    public static class ResourceValidatorLoader
    {
        private static bool m_initialized = false;

        /// <summary>
        /// Loads the default set of validators in this assembly
        /// </summary>
        public static void LoadStockValidators()
        {
            if (m_initialized)
                return;

            ResourceValidatorSet.RegisterValidator(new DrawingSourceValidator());
            ResourceValidatorSet.RegisterValidator(new FeatureSourceValidator());
            ResourceValidatorSet.RegisterValidator(new LayerDefinitionValidator());
            ResourceValidatorSet.RegisterValidator(new MapDefinitionValidator());
            ResourceValidatorSet.RegisterValidator(new WebLayoutValidator());
            ResourceValidatorSet.RegisterValidator(new ApplicationDefinitionValidator());
            ResourceValidatorSet.RegisterValidator(new LoadProcedureValidator());
            ResourceValidatorSet.RegisterValidator(new SymbolDefinitionValidator());
            ResourceValidatorSet.RegisterValidator(new SymbolLibraryValidator());
            ResourceValidatorSet.RegisterValidator(new PrintLayoutValidator());

            m_initialized = true;
        }
    }
}
