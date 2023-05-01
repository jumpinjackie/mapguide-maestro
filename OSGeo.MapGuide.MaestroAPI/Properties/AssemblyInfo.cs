﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OSGeo.MapGuide.MaestroAPI")]
[assembly: AssemblyDescription("The MapGuide Maestro API provides a unified interface for working with services provided by MapGuide Open Source /  Autodesk Infrastructure Map Server. Unlike the official MapGuide .net API, Maestro can work with many different versions of MapGuide with support for working with resources in a MapGuide Server in a fully object-oriented manner via the companion OSGeo.MapGuide.ObjectModels library.")]
[assembly: AssemblyProduct("OSGeo.MapGuide.MaestroAPI")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ab443f91-168a-4c13-9477-1b264241b3cd")]

// UGLY: Because this assembly is signed, the test library we're exposing internals to also has to be signed. In addition,
// we need to specify the full ugly public key of this target assembly as well!
[assembly: InternalsVisibleTo("OSGeo.MapGuide.MaestroAPI.Tests,PublicKey=00240000048000009400000006020000002400005253413100040000010001000f196e7ed5bff1e511efa3251b228582b26cbf78ea6d4282742d5c882db02b08ebc99922c0ddccf9ab79ee180250ac6716f986cf6fabdc1404b3dafee8873d4d6327be301f5ca52862065678cd5bc0c18ddc7ef6516723162c985a0c20eec07382a2090486f24393bf80976aa5ab57274620bafd62dfec34d5eed74bf41e72d2")]