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
}
