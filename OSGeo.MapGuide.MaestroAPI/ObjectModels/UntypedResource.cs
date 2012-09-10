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
using OSGeo.MapGuide.MaestroAPI;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Represents resource content that could not be deserialized into a corresponding
    /// strongly-typed resource class. This is just a container of arbitrary xml content.
    /// </summary>
    public class UntypedResource : IResource
    {
        internal UntypedResource(string xml, ResourceTypes resourceType, string version)
        {
            this.XmlContent = xml;
            this.ResourceType = resourceType;
            this.ResourceVersion = new Version(version);
        }

        internal UntypedResource(string xml, ResourceTypes resourceType, Version version)
        {
            this.XmlContent = xml;
            this.ResourceType = resourceType;
            this.ResourceVersion = version;
        }

        /// <summary>
        /// Gets or sets the current connection.
        /// </summary>
        /// <value>The current connection.</value>
        public IServerConnection CurrentConnection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the validating XML schema
        /// </summary>
        public string ValidatingSchema
        {
            get { return this.ResourceType + "-" + this.ResourceVersion.ToString() + ".xsd"; } //NOXLATE
        }

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
        public ResourceTypes ResourceType
        {
            get;
            private set;
        }

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
        public string Serialize()
        {
            return this.XmlContent;
        }

        /// <summary>
        /// Gets the resource version
        /// </summary>
        public Version ResourceVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a clone of this instance
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Raised when a property changes
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets whether this resource is strongly typed
        /// </summary>
        public bool IsStronglyTyped
        {
            get { return false; }
        }
    }
}
