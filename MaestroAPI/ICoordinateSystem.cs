#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
namespace OSGeo.MapGuide.MaestroAPI
{
    public interface ICoordinateSystem
    {
        HttpCoordinateSystem.Category[] Categories { get; }
        string ConvertCoordinateSystemCodeToWkt(string coordcode);
        string ConvertEpsgCodeToWkt(string epsg);
        string ConvertWktToCoordinateSystemCode(string wkt);
        string ConvertWktToEpsgCode(string wkt);
        HttpCoordinateSystem.CoordSys[] Coordsys { get; }
        HttpCoordinateSystem.CoordSys FindCoordSys(string coordcode);
        bool IsValid(string wkt);
        string LibraryName { get; }
    }
}
