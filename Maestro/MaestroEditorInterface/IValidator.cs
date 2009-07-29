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
    /// <summary>
    /// The interface for validating an item
    /// </summary>
    public interface IValidator
    {
        ValidationIssue[] Validate(object resource, bool recurse);
    }

    /// <summary>
    /// A class holding results of a validation
    /// </summary>
    public class ValidationIssue
    {
        private string m_message;
        private ValidationStatus m_status;
        private string m_field;
        private object m_resource;

        /// <summary>
        /// Gets the message for the validation issue
        /// </summary>
        public string Message { get { return m_message; } }
        /// <summary>
        /// Gets the status of the validation issue
        /// </summary>
        public ValidationStatus Status { get { return m_status; } }
        /// <summary>
        /// Gets the field the issue relates to (if any)
        /// </summary>
        public string Field { get { return m_field; } }
        /// <summary>
        /// Returns the resource the issue is related to
        /// </summary>
        public object Resource { get { return m_resource; } }

        /// <summary>
        /// Returns a textual representation of the issue
        /// </summary>
        /// <returns>A textual representation of the issue</returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", this.Status, this.Message) + (this.Field == null ? "" : "(" + this.Field + ")");
        }

        /// <summary>
        /// Constructs a new validation issue
        /// </summary>
        /// <param name="status">The issue status</param>
        /// <param name="message">The issue message</param>
        public ValidationIssue(object resource, ValidationStatus status, string message)
        {
            m_message = message;
            m_status = status;
            m_resource = resource;
        }

        /// <summary>
        /// Constructs a new validation issue
        /// </summary>
        /// <param name="status">The issue status</param>
        /// <param name="message">The issue message</param>
        /// <param name="field">The field thtat the issue relates to</param>
        public ValidationIssue(object resource, ValidationStatus status, string message, string field)
            : this(resource, status, message)
        {
            m_field = field;
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

    public static class Validation
    {
        private static List<IValidator> m_validators = new List<IValidator>();

        public static ValidationIssue[] Validate(object item, bool recurse)
        {
            List<ValidationIssue> issues = new List<ValidationIssue>();
            foreach (IValidator v in m_validators)
            {
                try
                {
                    ValidationIssue[] tmp = v.Validate(item, recurse);
                    if (tmp != null)
                        issues.AddRange(tmp);
                }
                catch (Exception ex)
                {
                    issues.Add(new ValidationIssue(item, ValidationStatus.Error, "Failed in validator: " + ex.Message));
                }
            }
                
            return issues.ToArray();
        }

        public static void RegisterValidator(IValidator v)
        {
            if (!m_validators.Contains(v))
                m_validators.Add(v);
        }
    }
}
