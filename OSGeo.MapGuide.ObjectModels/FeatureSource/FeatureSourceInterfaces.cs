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

using OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.FeatureSource
{
    /// <summary>
    /// Represents an FDO feature source
    /// </summary>
    public interface IFeatureSource : IResource
    {
        /// <summary>
        /// Removes all specified connection properties
        /// </summary>
        void ClearConnectionProperties();

        /// <summary>
        /// Gets an array of names of the currently specified connection properties
        /// </summary>
        string[] ConnectionPropertyNames { get; }

        /// <summary>
        /// Gets or sets the FDO provider.
        /// </summary>
        /// <value>The FDO provider.</value>
        string Provider { get; set; }

        /// <summary>
        /// Gets the connection property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string GetConnectionProperty(string name);

        /// <summary>
        /// Sets the connection property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value. If null, the matching parameter is removed from the feature source</param>
        void SetConnectionProperty(string name, string value);

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the name of the embedded data resource. Can only be called if <see cref="UsesEmbeddedDataFiles"/> returns true.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If <see cref="UsesEmbeddedDataFiles"/> is false</exception>
        string GetEmbeddedDataName();

        /// <summary>
        /// Gets the name of the alias. Can only be called if <see cref="UsesAliasedDataFiles"/> returns true
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If <see cref="UsesAliasedDataFiles"/> is false </exception>
        string GetAliasName();

        /// <summary>
        /// Gets the name of the aliased file. Can only be called if <see cref="UsesAliasedDataFiles"/> returns true. An
        /// empty string is returned if it is a directory (ie. no file name was found)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If <see cref="UsesAliasedDataFiles"/> is false</exception>
        string GetAliasedFileName();

        /// <summary>
        /// Gets a value indicating whether [uses embedded data files].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [uses embedded data files]; otherwise, <c>false</c>.
        /// </value>
        bool UsesEmbeddedDataFiles { get; }

        /// <summary>
        /// Gets a value indicating whether [uses aliased data files].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [uses aliased data files]; otherwise, <c>false</c>.
        /// </value>
        bool UsesAliasedDataFiles { get; }

        /// <summary>
        /// Gets the supplemental spatial context info (coordinate system overrides).
        /// </summary>
        /// <value>The supplemental spatial context info.</value>
        IEnumerable<ISpatialContextInfo> SupplementalSpatialContextInfo { get; }

        /// <summary>
        /// Adds the spatial context override.
        /// </summary>
        /// <param name="sc">The sc.</param>
        void AddSpatialContextOverride(ISpatialContextInfo sc);

        /// <summary>
        /// Removes the spatial context override.
        /// </summary>
        /// <param name="sc">The sc.</param>
        void RemoveSpatialContextOverride(ISpatialContextInfo sc);

        /// <summary>
        /// Gets the extensions for this feature source.
        /// </summary>
        /// <value>The extensions.</value>
        IEnumerable<IFeatureSourceExtension> Extension { get; }

        /// <summary>
        /// Adds the extension.
        /// </summary>
        /// <param name="ext">The ext.</param>
        void AddExtension(IFeatureSourceExtension ext);

        /// <summary>
        /// Removes the extension.
        /// </summary>
        /// <param name="ext">The ext.</param>
        void RemoveExtension(IFeatureSourceExtension ext);

        /// <summary>
        /// Gets or sets the name of the configuration document.
        /// </summary>
        /// <value>The name of the configuration document.</value>
        string ConfigurationDocument { get; set; }
    }

    /// <summary>
    /// Represents a spatial context override
    /// </summary>
    public interface ISpatialContextInfo
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the coordinate system.
        /// </summary>
        /// <value>The coordinate system.</value>
        string CoordinateSystem { get; set; }
    }

    /// <summary>
    /// Represents an extended feature class
    /// </summary>
    public interface IFeatureSourceExtension : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the feature class to extend
        /// </summary>
        /// <value>The feature class.</value>
        string FeatureClass { get; set; }

        /// <summary>
        /// Gets the calculated properties.
        /// </summary>
        /// <value>The calculated properties.</value>
        IEnumerable<ICalculatedProperty> CalculatedProperty { get; }

        /// <summary>
        /// Adds the calculated property.
        /// </summary>
        /// <param name="prop">The prop.</param>
        void AddCalculatedProperty(ICalculatedProperty prop);

        /// <summary>
        /// Removes the calculated property.
        /// </summary>
        /// <param name="prop">The prop.</param>
        void RemoveCalculatedProperty(ICalculatedProperty prop);

        /// <summary>
        /// Gets the attribute joins
        /// </summary>
        /// <value>The attribute joins.</value>
        IEnumerable<IAttributeRelation> AttributeRelate { get; }

        /// <summary>
        /// Adds the relation.
        /// </summary>
        /// <param name="relate">The relate.</param>
        void AddRelation(IAttributeRelation relate);

        /// <summary>
        /// Removes the relation.
        /// </summary>
        /// <param name="relate">The relate.</param>
        void RemoveRelation(IAttributeRelation relate);
    }

    /// <summary>
    /// Represents a FDO calculated property
    /// </summary>
    public interface ICalculatedProperty : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the FDO expression.
        /// </summary>
        /// <value>The FDO expression.</value>
        string Expression { get; set; }
    }

    /// <summary>
    /// Defines the type of joins
    /// </summary>
    [System.SerializableAttribute()]
    public enum RelateTypeEnum
    {
        /// <remarks/>
        LeftOuter,

        /// <remarks/>
        RightOuter,

        /// <remarks/>
        Inner,

        /// <remarks/>
        Association,
    }

    /// <summary>
    /// Represents an attribute join
    /// </summary>
    public interface IAttributeRelation : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets whether to force 1:1 cardinality
        /// </summary>
        bool ForceOneToOne { get; set; }

        /// <summary>
        /// Gets the type of join
        /// </summary>
        RelateTypeEnum RelateType { get; set; }

        /// <summary>
        /// Gets or sets the feature source id containing the feature class to extend
        /// </summary>
        string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the feature class to extend
        /// </summary>
        string AttributeClass { get; set; }

        /// <summary>
        /// Gets or sets the name of the join
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the prefix that prevents a naming collision on both sides of the join
        /// </summary>
        string AttributeNameDelimiter { get; set; }

        /// <summary>
        /// Gets the property pairs involved in this join
        /// </summary>
        /// <value>The property pairs.</value>
        IEnumerable<IRelateProperty> RelateProperty { get; }

        /// <summary>
        /// Gets the number of properties being joined on
        /// </summary>
        int RelatePropertyCount { get; }

        /// <summary>
        /// Creates the property join.
        /// </summary>
        /// <param name="primaryProperty">The primary property.</param>
        /// <param name="secondaryProperty">The secondary property.</param>
        /// <returns></returns>
        IRelateProperty CreatePropertyJoin(string primaryProperty, string secondaryProperty);

        /// <summary>
        /// Adds the relate property.
        /// </summary>
        /// <param name="prop">The prop.</param>
        void AddRelateProperty(IRelateProperty prop);

        /// <summary>
        /// Removes the relate property.
        /// </summary>
        /// <param name="prop">The prop.</param>
        void RemoveRelateProperty(IRelateProperty prop);

        /// <summary>
        /// Removes all relate properties.
        /// </summary>
        void RemoveAllRelateProperties();
    }

    /// <summary>
    /// Represents a property pair in an attribute join
    /// </summary>
    public interface IRelateProperty
    {
        /// <summary>
        /// Gets or sets the feature class property.
        /// </summary>
        /// <value>The feature class property.</value>
        string FeatureClassProperty { get; set; }

        /// <summary>
        /// Gets or sets the attribute class property.
        /// </summary>
        /// <value>The attribute class property.</value>
        string AttributeClassProperty { get; set; }
    }

    /// <summary>
    /// Extension method class
    /// </summary>
    public static class FeatureSourceExtensions
    {
        /// <summary>
        /// Gets a collection of connection properties
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static NameValueCollection GetConnectionProperties(this IFeatureSource fs)
        {
            Check.ArgumentNotNull(fs, nameof(fs));
            var values = new NameValueCollection();
            foreach (string name in fs.ConnectionPropertyNames)
            {
                values[name] = fs.GetConnectionProperty(name);
            }
            return values;
        }

        /// <summary>
        /// Sets the connection properties of the feature source
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="values"></param>
        public static void ApplyConnectionProperties(this IFeatureSource fs, NameValueCollection values)
        {
            Check.ArgumentNotNull(fs, nameof(fs));
            Check.ArgumentNotNull(values, nameof(values));

            fs.ClearConnectionProperties();

            foreach (string name in values.Keys)
            {
                string value = values[name];

                fs.SetConnectionProperty(name, value);
            }
        }

        /// <summary>
        /// Adds a spatial context override
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="name"></param>
        /// <param name="coordSys"></param>
        public static void AddSpatialContextOverride(this IFeatureSource fs, string name, string coordSys)
        {
            Check.ArgumentNotNull(fs, nameof(fs));
            fs.AddSpatialContextOverride(new SpatialContextType() { Name = name, CoordinateSystem = coordSys });
        }

        /// <summary>
        /// Adds the specified property pair to this join
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="primary"></param>
        /// <param name="secondary"></param>
        public static void AddRelateProperty(this IAttributeRelation rel, string primary, string secondary)
        {
            Check.ArgumentNotNull(rel, nameof(rel));
            rel.AddRelateProperty(new RelatePropertyType() { FeatureClassProperty = primary, AttributeClassProperty = secondary });
        }
    }
}