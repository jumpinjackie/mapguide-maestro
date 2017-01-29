#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using System;

#pragma warning disable 1591, 0114, 0108, 0067

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Represents resource content that could not be deserialized into a corresponding
    /// strongly-typed resource class. This is just a container of arbitrary xml content.
    /// </summary>
    public class UntypedResource : IResource
    {
        internal UntypedResource(string xml, string resourceType, string version)
        {
            this.XmlContent = xml;
            this.ResourceType = resourceType;
            this.ResourceVersion = new Version(version);
        }

        internal UntypedResource(string xml, string resourceType, Version version)
        {
            this.XmlContent = xml;
            this.ResourceType = resourceType;
            this.ResourceVersion = version;
        }

        /// <summary>
        /// Gets the validating XML schema
        /// </summary>
        public string ValidatingSchema => $"{this.ResourceType}-{this.ResourceVersion.ToString()}.xsd"; //NOXLATE

        /// <summary>
        /// Gets or sets the resource id
        /// </summary>
        public string ResourceID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the resource type
        /// </summary>
        public string ResourceType { get; }

        /// <summary>
        /// Gets or sets the XML content
        /// </summary>
        public string XmlContent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the XML form of this instance
        /// </summary>
        /// <returns></returns>
        public string Serialize() => this.XmlContent;

        /// <summary>
        /// Gets the resource version
        /// </summary>
        public Version ResourceVersion { get; }

        /// <summary>
        /// Returns a clone of this instance
        /// </summary>
        /// <returns></returns>
        public object Clone() => this.MemberwiseClone();

        /// <summary>
        /// Raised when a property changes
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets whether this resource is strongly typed
        /// </summary>
        public bool IsStronglyTyped => false;
    }
}