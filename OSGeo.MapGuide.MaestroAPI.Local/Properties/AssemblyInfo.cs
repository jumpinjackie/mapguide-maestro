using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Local;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OSGeo.MapGuide.MaestroAPI.Local")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("OSGeo.MapGuide.MaestroAPI.Local")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1a33e84e-0180-4b3f-bae5-51b1a62f6bd1")]

[assembly: MaestroApiProvider("Maestro.Local", "Maestro connection wrapper for mg-desktop library", typeof(LocalConnection), true)]