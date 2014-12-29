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

#endregion Disclaimer / License

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Utility class that initializes the default set of resource validators
    /// </summary>
    /// <example>
    /// This example shows how a ResourceValidatorLoader is used
    /// <code>
    /// <![CDATA[
    /// IResource resource;
    /// IServerConnection conn;
    /// ...
    /// ResourceValidatorLoader.LoadStockValidators();
    /// var context = new ResourceValidationContext(conn);
    /// var issues = ResourceValidatorSet.Validate(context, item, false);
    /// ]]>
    /// </code>
    /// </example>
    public static class ResourceValidatorLoader
    {
        private static bool m_initialized = false;

        /// <summary>
        /// Loads the default set of validators in this assembly
        /// </summary>
        /// <remarks>
        /// This method only needs to be called once. Subsequent calls return immediately. As such it is ideal
        /// to put this call in your application's startup/initialization code.
        /// </remarks>
        public static void LoadStockValidators()
        {
            if (m_initialized)
                return;

            ResourceValidatorSet.RegisterValidator(new DrawingSourceValidator());
            ResourceValidatorSet.RegisterValidator(new SymbolLibraryValidator());
            ResourceValidatorSet.RegisterValidator(new PrintLayoutValidator());
            ResourceValidatorSet.RegisterValidator(new FeatureSourceValidator());
            ResourceValidatorSet.RegisterValidator(new ApplicationDefinitionValidator());

            //NOTE: As of right now, all resources (regardless of version) are validated
            //with the same logic with each respective validator. In the event that a specific
            //resource requires its own validation class, it should be registered here.

            ResourceValidatorSet.RegisterValidator(new LayerDefinitionValidator("1.0.0"));
            ResourceValidatorSet.RegisterValidator(new LayerDefinitionValidator("1.1.0"));
            ResourceValidatorSet.RegisterValidator(new LayerDefinitionValidator("1.2.0"));
            ResourceValidatorSet.RegisterValidator(new LayerDefinitionValidator("1.3.0"));
            ResourceValidatorSet.RegisterValidator(new LayerDefinitionValidator("2.3.0"));
            ResourceValidatorSet.RegisterValidator(new LayerDefinitionValidator("2.4.0"));
            ResourceValidatorSet.RegisterValidator(new MapDefinitionValidator("1.0.0"));
            ResourceValidatorSet.RegisterValidator(new MapDefinitionValidator("2.3.0"));
            ResourceValidatorSet.RegisterValidator(new MapDefinitionValidator("2.4.0"));
            ResourceValidatorSet.RegisterValidator(new WebLayoutValidator("1.0.0"));
            ResourceValidatorSet.RegisterValidator(new WebLayoutValidator("1.1.0"));
            ResourceValidatorSet.RegisterValidator(new WebLayoutValidator("2.4.0"));
            ResourceValidatorSet.RegisterValidator(new WebLayoutValidator("2.6.0"));
            ResourceValidatorSet.RegisterValidator(new LoadProcedureValidator("1.0.0"));
            ResourceValidatorSet.RegisterValidator(new LoadProcedureValidator("1.1.0"));
            ResourceValidatorSet.RegisterValidator(new LoadProcedureValidator("2.2.0"));
            ResourceValidatorSet.RegisterValidator(new SymbolDefinitionValidator("1.0.0"));
            ResourceValidatorSet.RegisterValidator(new SymbolDefinitionValidator("1.1.0"));
            ResourceValidatorSet.RegisterValidator(new SymbolDefinitionValidator("2.4.0"));

            m_initialized = true;
        }
    }
}