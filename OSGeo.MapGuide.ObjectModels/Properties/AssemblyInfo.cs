using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OSGeo.MapGuide.ObjectModels")]
[assembly: AssemblyDescription("MapGuide Object Model Library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Open Source Geospatial Foundation")]
[assembly: AssemblyCopyright("Copyright (c) 2014, Jackie Ng")]
[assembly: AssemblyProduct("OSGeo.MapGuide.ObjectModels")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b7638193-e61e-4f87-a775-31dd931fb0bd")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.0")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Code", "RECS0145:Removes 'private' modifiers that are not required", Justification = "The author likes to be explicit with accessibility modifiers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Code", "RECS0129:Removes 'internal' modifiers that are not required", Justification = "The author likes to be explicit with accessibility modifiers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Language", "CSE0003:Use expression-bodied members", Justification = "The author prefers debuggability over succintness")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Symbol Declarations", "RECS0007:The default underlying type of enums is int, so defining it explicitly is redundant.", Justification = "The author prefers to be explicit about underlying enum types")]