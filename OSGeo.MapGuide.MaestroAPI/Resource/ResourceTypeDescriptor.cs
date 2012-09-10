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
    /// <summary>
    /// Represents a unique resource type / version pair
    /// </summary>
    public class ResourceTypeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceTypeDescriptor"/> class.
        /// </summary>
        /// <param name="resType">Type of the res.</param>
        /// <param name="ver">The ver.</param>
        public ResourceTypeDescriptor(ResourceTypes resType, string ver)
            : this(resType.ToString(), ver) 
        { }

        private ResourceTypeDescriptor(string resType, string ver)
        {
            Check.NotEmpty(resType, "resType"); //NOXLATE
            Check.NotEmpty(ver, "ver"); //NOXLATE

            this.ResourceType = resType;
            this.Version = ver;
        }

        /// <summary>
        /// Gets the name of the validating XML schema
        /// </summary>
        public string XsdName
        {
            get
            {
                return ResourceType + "-" + Version + ".xsd"; //NOXLATE
            }
        }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>The type of the resource.</value>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

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
            var desc = obj as ResourceTypeDescriptor;
            if (desc == null)
                return false;

            return this.ToString() == desc.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.ResourceType + this.Version;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Application Definition v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor ApplicationDefinition
        {
            get { return new ResourceTypeDescriptor("ApplicationDefinition", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Feature Source v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor FeatureSource
        {
            get { return new ResourceTypeDescriptor("FeatureSource", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Drawing Source v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor DrawingSource
        {
            get { return new ResourceTypeDescriptor("DrawingSource", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Layer Definition v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor LayerDefinition
        {
            get { return new ResourceTypeDescriptor("LayerDefinition", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Load Procedure v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor LoadProcedure
        {
            get { return new ResourceTypeDescriptor("LoadProcedure", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Map Definition v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor MapDefinition
        {
            get { return new ResourceTypeDescriptor("MapDefinition", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Print Layout v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor PrintLayout
        {
            get { return new ResourceTypeDescriptor("PrintLayout", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Symbol Library v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor SymbolLibrary
        {
            get { return new ResourceTypeDescriptor("SymbolLibrary", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Symbol Definition v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor SymbolDefinition
        {
            get { return new ResourceTypeDescriptor("SymbolDefinition", "1.0.0"); } //NOXLATE
        }

        /// <summary>
        /// Web Layout v1.0.0
        /// </summary>
        public static ResourceTypeDescriptor WebLayout
        {
            get { return new ResourceTypeDescriptor("WebLayout", "1.0.0"); } //NOXLATE
        }
    }
}
