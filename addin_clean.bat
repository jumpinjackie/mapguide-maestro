echo Cleaning unnecessary dlls in %1
REM The following assemblies will already be in the Maestro root dir and a redunant in their respective addin directories
pushd %1
if exist Aga.Controls.* del Aga.Controls.*
if exist GeoAPI.* del GeoAPI.*
if exist ICSharpCode.SharpZipLib.* del ICSharpCode.SharpZipLib.*
if exist Irony.* del Irony.*
if exist Microsoft.IO.RecyclableMemoryStream.* del Microsoft.IO.RecyclableMemoryStream.*
if exist NetTopologySuite.* del NetTopologySuite.*
if exist ProjNET.* del ProjNET.*
if exist WeifenLuo.WinFormsUI.Docking.* del WeifenLuo.WinFormsUI.Docking.*
popd