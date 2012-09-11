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
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// A collection of resource validators
    /// </summary>
    public static class ResourceValidatorSet
    {
        private static List<IResourceValidator> m_validators = new List<IResourceValidator>();

        /// <summary>
        /// Registers the validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public static void RegisterValidator(IResourceValidator validator)
        {
            Check.NotNull(validator, "validator"); //NOXLATE

            if (!m_validators.Contains(validator))
                m_validators.Add(validator);
        }

        /// <summary>
        /// Validates the specified items.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="items">The items.</param>
        /// <param name="recurse">if set to <c>true</c> [recurse].</param>
        /// <returns></returns>
        public static ValidationIssue[] Validate(ResourceValidationContext context, IEnumerable<IResource> items, bool recurse)
        {
            Check.NotNull(items, "items"); //NOXLATE
            var issues = new List<ValidationIssue>();
            foreach (var item in items)
            {
                issues.AddRange(Validate(context, item, true));
            }
            return issues.ToArray();
        }

        /// <summary>
        /// Validates the specified item using an existing validation context to skip over
        /// items already validated
        /// </summary>
        /// <param name="context"></param>
        /// <param name="item"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public static ValidationIssue[] Validate(ResourceValidationContext context, IResource item, bool recurse)
        {
            Check.NotNull(item, "item"); //NOXLATE
            var issueSet = new ValidationResultSet();
            if (!HasValidator(item.ResourceType, item.ResourceVersion))
            {
                issueSet.AddIssue(new ValidationIssue(item, ValidationStatus.Warning, ValidationStatusCode.Warning_General_NoRegisteredValidatorForResource, string.Format(Strings.ERR_NO_REGISTERED_VALIDATOR, item.ResourceType, item.ResourceVersion)));
            }
            else
            {
                foreach (IResourceValidator v in m_validators)
                {
                    if (!v.SupportedResourceAndVersion.Equals(item.GetResourceTypeDescriptor()))
                        continue;

                    try
                    {
                        ValidationIssue[] tmp = v.Validate(context, item, recurse);
                        if (tmp != null)
                            issueSet.AddIssues(tmp);
                    }
                    catch (Exception ex)
                    {
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        if (ex is NullReferenceException)
                            msg = ex.ToString();
                        issueSet.AddIssue(new ValidationIssue(item, ValidationStatus.Error, ValidationStatusCode.Error_General_ValidationError, string.Format(Strings.ErrorValidationGeneric, msg)));
                    }
                }
            }
            return issueSet.GetAllIssues();
        }

        /// <summary>
        /// Determines whether the specified resource types has validator.
        /// </summary>
        /// <param name="resourceTypes">The resource types.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// 	<c>true</c> if the specified resource types has validator; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValidator(ResourceTypes resourceTypes, Version version)
        {
            bool found = false;
            var find = new ResourceTypeDescriptor(resourceTypes, version.ToString());
            foreach (var v in m_validators)
            {
                if (v.SupportedResourceAndVersion.Equals(find))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}
