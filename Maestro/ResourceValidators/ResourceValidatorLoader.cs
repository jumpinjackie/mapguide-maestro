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
