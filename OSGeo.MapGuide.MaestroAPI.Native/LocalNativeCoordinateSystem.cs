#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    public class LocalNativeCoordinateSystemDefinition : CoordinateSystemDefinitionBase
    {
        internal LocalNativeCoordinateSystemDefinition() : base() { }

        internal LocalNativeCoordinateSystemDefinition(CoordinateSystemCategory parent, MgPropertyCollection props)
            : base(parent)
        {
            for (int i = 0; i < props.Count; i++)
            {
                switch (props[i].Name.ToLower())
                {
                    case "code":
                        m_code = (props[i] as MgStringProperty).Value;
                        break;
                    case "description":
                        m_description = (props[i] as MgStringProperty).Value;
                        break;
                    case "projection":
                        m_projection = (props[i] as MgStringProperty).Value;
                        break;
                    case "projection description":
                        m_projectionDescription = (props[i] as MgStringProperty).Value;
                        break;
                    case "Datum":
                        m_datum = (props[i] as MgStringProperty).Value;
                        break;
                    case "datum description":
                        m_datumDescription = (props[i] as MgStringProperty).Value;
                        break;
                    case "ellipsoid":
                        m_ellipsoid = (props[i] as MgStringProperty).Value;
                        break;
                    case "ellipsoid description":
                        m_ellipsoidDescription = (props[i] as MgStringProperty).Value;
                        break;
                }
            }
        }
    }
}
