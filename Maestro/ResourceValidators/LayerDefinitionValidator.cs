using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.Maestro.ResourceValidators
{
    public class LayerDefinitionValidator : IValidator
    {
        public ValidationIssue[] Validate(object resource, bool recurse)
        {
            if (resource as MaestroAPI.LayerDefinition == null)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();



            return issues.ToArray();
        }
    }
}
