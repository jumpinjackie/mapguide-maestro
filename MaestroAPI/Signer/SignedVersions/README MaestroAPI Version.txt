MaestroAPI targets MGOS 2.0 (and MGEP 2010).

The HttpServerConnection can work with any version
of MapGuide, but the LocalNativeConnection needs
the correct version of MapGuideDotNetApi.dll

Unfortunately the official MapGuideDotNetApi.dll
has varying version numbers, and is usually not signed.

To use the LocalNativeConnction with 1.2 or 2.1, you must
use a signed version of MapGuideDotNetApi.dll.

The Web.Config file shows how to add a bindingRedirect
for a ASP.Net application, the same can be done for
a regulat executable. Just rename Web.Config to match
your executable, eg MyApp.exe -> MyApp.exe.config.

If you need a special version of MapGuideDotNetApi.dll,
eg. for a development version of MapGuide, you can
sign the dll by issuing this command:

..\signer.exe -k ..\..\maestroapi.key -outdir .\out -a MapGuideDotNetApi.dll