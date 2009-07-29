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
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.Maestro.ResourceValidators
{
    public static class ResourceValidatorLoader
    {
        private static bool m_initialized = false;

        public static void LoadStockValidators()
        {
            if (m_initialized)
                return;

            Validation.RegisterValidator(new FeatureSourceValidator());
            Validation.RegisterValidator(new LayerDefinitionValidator());
            Validation.RegisterValidator(new MapDefinitionValidator());
            Validation.RegisterValidator(new WebLayoutValidator());
            Validation.RegisterValidator(new ApplicationDefinitionValidator());

            m_initialized = true;
        }
    }
}
