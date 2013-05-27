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
    public class LocalNativeMpuCalculator : IMpuCalculator
    {
        private MgCoordinateSystemFactory _csFact;

        public LocalNativeMpuCalculator() { _csFact = new MgCoordinateSystemFactory(); }

        public double Calculate(string csWkt, double units)
        {
            MgCoordinateSystem cs = _csFact.Create(csWkt);
            return cs.ConvertCoordinateSystemUnitsToMeters(units);
        }
    }

    public class LocalNativeCoordinateSystemDefinition : CoordinateSystemDefinitionBase
    {
        internal LocalNativeCoordinateSystemDefinition() : base() { }

        internal LocalNativeCoordinateSystemDefinition(CoordinateSystemCategory parent, MgPropertyCollection props)
            : base(parent)
        {
            int pcount = props.GetCount();
            for (int i = 0; i < pcount; i++)
            {
                var prop = props.GetItem(i);
                switch (prop.Name.ToLower())
                {
                    case "code":
                        m_code = (prop as MgStringProperty).Value;
                        break;
                    case "description":
                        m_description = (prop as MgStringProperty).Value;
                        break;
                    case "projection":
                        m_projection = (prop as MgStringProperty).Value;
                        break;
                    case "projection description":
                        m_projectionDescription = (prop as MgStringProperty).Value;
                        break;
                    case "Datum":
                        m_datum = (prop as MgStringProperty).Value;
                        break;
                    case "datum description":
                        m_datumDescription = (prop as MgStringProperty).Value;
                        break;
                    case "ellipsoid":
                        m_ellipsoid = (prop as MgStringProperty).Value;
                        break;
                    case "ellipsoid description":
                        m_ellipsoidDescription = (prop as MgStringProperty).Value;
                        break;
                }
            }
        }
    }
}
