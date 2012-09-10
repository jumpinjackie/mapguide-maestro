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
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    /// <summary>
    /// Represents a coordinate system definition
    /// </summary>
    public abstract class CoordinateSystemDefinitionBase
    {
        /// <summary>
        /// The parent category
        /// </summary>
        protected CoordinateSystemCategory m_parent;
        /// <summary>
        /// The cs code
        /// </summary>
        protected string m_code;
        /// <summary>
        /// The description
        /// </summary>
        protected string m_description;
        /// <summary>
        /// The projection
        /// </summary>
        protected string m_projection;
        /// <summary>
        /// The projection description
        /// </summary>
        protected string m_projectionDescription;
        /// <summary>
        /// The datum
        /// </summary>
        protected string m_datum;
        /// <summary>
        /// The datum description
        /// </summary>
        protected string m_datumDescription;
        /// <summary>
        /// The ellipsoid
        /// </summary>
        protected string m_ellipsoid;
        /// <summary>
        /// The ellipsoid description
        /// </summary>
        protected string m_ellipsoidDescription;

        /// <summary>
        /// The cs wkt
        /// </summary>
        protected string m_wkt = null;
        /// <summary>
        /// The epsg code
        /// </summary>
        protected string m_epsg = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemBase"/> class.
        /// </summary>
        protected CoordinateSystemDefinitionBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemBase"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected CoordinateSystemDefinitionBase(CoordinateSystemCategory parent)
        {
            m_parent = parent;
        }

        internal CoordinateSystemCategory Parent
        {
            get { return m_parent; }
            set
            {
                m_parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code 
        { 
            get { return m_code; } 
            set { m_code = value; }
        }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description 
        { 
            get { return m_description; } 
            set { m_description = value; }
        }
        /// <summary>
        /// Gets the projection.
        /// </summary>
        /// <value>The projection.</value>
        public string Projection { get { return m_projection; } }
        /// <summary>
        /// Gets the projection description.
        /// </summary>
        /// <value>The projection description.</value>
        public string ProjectionDescription { get { return m_projectionDescription; } }
        /// <summary>
        /// Gets the datum.
        /// </summary>
        /// <value>The datum.</value>
        public string Datum { get { return m_datum; } }
        /// <summary>
        /// Gets the datum description.
        /// </summary>
        /// <value>The datum description.</value>
        public string DatumDescription { get { return m_datumDescription; } }
        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid.</value>
        public string Ellipsoid { get { return m_ellipsoid; } }
        /// <summary>
        /// Gets the ellipsoid description.
        /// </summary>
        /// <value>The ellipsoid description.</value>
        public string EllipsoidDescription { get { return m_ellipsoidDescription; } }

        /// <summary>
        /// Gets or sets the WKT.
        /// </summary>
        /// <value>The WKT.</value>
        public string WKT 
        {
            get 
            {
                if (m_wkt == null)
                    m_wkt = m_parent.Parent.ConvertCoordinateSystemCodeToWkt(m_code);
                return m_wkt;
            }
            set
            {
                m_wkt = value;
            }
        }

        /// <summary>
        /// Gets the EPSG code
        /// </summary>
        /// <value>The EPSG code.</value>
        public string EPSG 
        {
            get 
            {
                if (m_epsg == null)
                    if (m_code.StartsWith("EPSG:")) //NOXLATE
                        m_epsg = m_code.Substring(5);
                    else
                        m_epsg = m_parent.Parent.ConvertWktToEpsgCode(m_parent.Parent.ConvertCoordinateSystemCodeToWkt(m_code));

                return m_epsg;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (m_description == null && m_code == null)
                return m_wkt == null ? "<null>" : m_wkt; //NOXLATE
            else if (m_description == null)
                return m_code;
            else if (m_code == null)
                return m_description;
            else
                return m_description + " (" + m_code + ")"; //NOXLATE
        }
    }
}
