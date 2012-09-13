#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
    /// Represents a Feature instance whose properties can be modified
    /// </summary>
    public class MutableFeatureBase : MutableRecordBase, IMutableFeature
    {
        /// <summary>
        /// The class definition
        /// </summary>
        protected ClassDefinition _clsDef;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="source"></param>
        protected MutableFeatureBase(IRecordInitialize feature, ClassDefinition source)
            : base(feature)
        {
            _clsDef = ClassDefinition.Clone(source);
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="feat"></param>
        protected MutableFeatureBase(MutableFeatureBase feat)
            : this(feat, feat.ClassDefinition)
        {

        }

        /// <summary>
        /// Gets the associated class definition
        /// </summary>
        public ClassDefinition ClassDefinition
        {
            get { return _clsDef; }
        }
    }
}
