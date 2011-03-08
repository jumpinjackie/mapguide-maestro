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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OSGeo.MapGuide.MaestroAPI.Native;
using OSGeo.MapGuide.MaestroAPI;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OSGeo.MapGuide.MaestroAPI.Native")]
[assembly: AssemblyDescription("MaestroAPI implementation wrapper for Official MapGuide API")]
[assembly: AssemblyProduct("OSGeo.MapGuide.MaestroAPI.Native")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("D80991DF-E4D3-43ee-AF2B-C96AD8462C70")]

// This long string is the public key of the maestroapi.key token
[assembly: InternalsVisibleTo("MaestroAPITests, PublicKey=00240000048000009400000006020000002400005253413100040000010001000f196e7ed5bff1e511efa3251b228582b26cbf78ea6d4282742d5c882db02b08ebc99922c0ddccf9ab79ee180250ac6716f986cf6fabdc1404b3dafee8873d4d6327be301f5ca52862065678cd5bc0c18ddc7ef6516723162c985a0c20eec07382a2090486f24393bf80976aa5ab57274620bafd62dfec34d5eed74bf41e72d2")]
[assembly: InternalsVisibleTo("MaestroBaseTests, PublicKey=00240000048000009400000006020000002400005253413100040000010001000f196e7ed5bff1e511efa3251b228582b26cbf78ea6d4282742d5c882db02b08ebc99922c0ddccf9ab79ee180250ac6716f986cf6fabdc1404b3dafee8873d4d6327be301f5ca52862065678cd5bc0c18ddc7ef6516723162c985a0c20eec07382a2090486f24393bf80976aa5ab57274620bafd62dfec34d5eed74bf41e72d2")]

[assembly: MaestroApiProvider("Maestro.LocalNative", "Maestro wrapper for official MapGuide API", typeof(LocalNativeConnection), true)]