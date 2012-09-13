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
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Provides a forward-only, read-only iterator 
    /// for reading features selected from a feature source
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><description>You must call <see cref="M:ReadNext"/> before you can access the data</description></item>
    /// <item><description>For each property, determine the property type and then call the appropriate Get&lt;type&gt;() method to get the value of the property.</description></item>
    /// <item><description>The exception for this is if you are access the value via the indexer. In this case you only need determine the property type when casting from the System.Object that is returned by the indexer</description></item>
    /// </list>
    /// </remarks>
    public interface IFeatureReader : IReader, IFeature, IEnumerable<IFeature>
    {
        
    }

    /// <summary>
    /// Provides access to the property values within each feature for a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/>
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><description>For each property, determine the property type and then call the appropriate Get&lt;type&gt;() method to get the value of the property.</description></item>
    /// <item><description>The exception for this is if you are access the value via the indexer. In this case you only need determine the property type when casting from the System.Object that is returned by the indexer</description></item>
    /// </list>
    /// </remarks>
    public interface IFeature : IRecord
    {
        /// <summary>
        /// Gets the class definition of the object currently being read. If the user has requested 
        /// only a subset of the class properties (as specified in the filter text), the class 
        /// definition reflects what the user has requested, rather than the full class definition. 
        /// </summary>
        ClassDefinition ClassDefinition { get; }

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IFeatureReader GetFeatureObject(string name);

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IFeatureReader GetFeatureObject(int index);
    }

    /// <summary>
    /// Defines a mutable feature record
    /// </summary>
    public interface IMutableFeature : IMutableRecord
    {
        /// <summary>
        /// Gets the class definition of the object currently being read. If the user has requested 
        /// only a subset of the class properties (as specified in the filter text), the class 
        /// definition reflects what the user has requested, rather than the full class definition. 
        /// </summary>
        ClassDefinition ClassDefinition { get; }

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IFeatureReader GetFeatureObject(string name);

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IFeatureReader GetFeatureObject(int index);
    }
}
