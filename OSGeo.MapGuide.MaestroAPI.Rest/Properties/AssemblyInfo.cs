using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Rest;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OSGeo.MapGuide.MaestroAPI.Rest")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("OSGeo.MapGuide.MaestroAPI.Rest")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b4e157a5-d3ff-482a-88ad-a353906ac919")]

[assembly: MaestroApiProvider("Maestro.Rest", "Maestro mapguide-rest API Provider", typeof(RestConnection), true, false)]