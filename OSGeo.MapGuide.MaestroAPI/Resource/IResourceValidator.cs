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

namespace OSGeo.MapGuide.MaestroAPI.Resource
{
    using Validation;

    /// <summary>
    /// Interface for validating specific resource types
    /// </summary>
    public interface IResourceValidator
    {
        /// <summary>
        /// Gets the resource type and version this validator supports
        /// </summary>
        ResourceTypeDescriptor SupportedResourceAndVersion { get; }

        /// <summary>
        /// Validats the specified resources for common issues associated with this
        /// resource type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource">The resource to be validated</param>
        /// <param name="recurse">Indicates whether to also validate resources this resource depends on</param>
        /// <returns></returns>
        ValidationIssue[] Validate(ResourceValidationContext context, IResource resource, bool recurse);
    }

    /// <summary>
    /// Represents a validation issue collected during validation
    /// </summary>
    public class ValidationIssue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationIssue"/> class.
        /// </summary>
        /// <param name="res">The resource.</param>
        /// <param name="stat">The validation status.</param>
        /// <param name="code">The validation status code.</param>
        /// <param name="msg">The message.</param>
        public ValidationIssue(IResource res, ValidationStatus stat, ValidationStatusCode code, string msg)
        {
            Check.NotNull(res, "res"); //NOXLATE
            Check.NotEmpty(msg, "msg"); //NOXLATE

            this.Resource = res;
            this.Status = stat;
            this.Message = msg;
            this.StatusCode = code;
        }

        /// <summary>
        /// Gets the validation status code
        /// </summary>
        public ValidationStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the message for the validation issue
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the status of the validation issue
        /// </summary>
        public ValidationStatus Status { get; private set; }

        /// <summary>
        /// Gets the resource this issue pertains to
        /// </summary>
        public IResource Resource { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!typeof(ValidationIssue).IsAssignableFrom(obj.GetType()))
                return false;

            ValidationIssue vi = (ValidationIssue)obj;
            return this.Resource.ResourceID.Equals(vi.Resource.ResourceID) &&
                   this.Message.Equals(vi.Message) &&
                   this.Status == vi.Status &&
                   this.StatusCode == vi.StatusCode;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-systemobjectgethashcode
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Resource.ResourceID.GetHashCode();
                hash = hash * 23 + this.Message.GetHashCode();
                hash = hash * 23 + this.Status.GetHashCode();
                hash = hash * 23 + this.StatusCode.GetHashCode();

                return hash;
            }
        }
    }

    /// <summary>
    /// All possible states a validation issue may have
    /// </summary>
    public enum ValidationStatus
    {
        /// <summary>
        /// Indicates that the issue is non-vital, eg. a performance problem
        /// </summary>
        Information,
        /// <summary>
        /// Indicates that the issue is likely to cause problems
        /// </summary>
        Warning,
        /// <summary>
        /// Indicates that the issue will prevent correct operation of the map
        /// </summary>
        Error
    }
}
