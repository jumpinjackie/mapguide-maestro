MaestroAPI LocalNative Provider template README
===============================================

What is this tool?
------------------

This tool (ProviderTemplate.exe) generates a custom built Maestro.LocalNative provider for the MaestroAPI against your specific version of MapGuide.

This tool was created for the following reasons:
 1. The actual source for the provider is effectively the same for most versions of MapGuide due to the additive nature of the MapGuide API
 2. It is redundant to keep and maintain the local provider source for each specific version of MapGuide.
 3. Be able to have a tailor-made LocalNative provider for your specific version of MapGuide without having to wait for an official Maestro release.

How to use this tool?
---------------------

Assuming you extracted the Maestro SDK to C:\MaestroSDK, the ProviderTemplate.exe should reside in C:\MaestroSDK\LocalNativeProvider with the
source code under C:\MaestroSDK\LocalNativeProvider\Src and the SDK binaries under C:\MaestroSDK\bin

DO NOT ALTER THIS DIRECTORY STRUCTURE! The tool currently assumes this directory structure as it is currently laid out.

Run the tool and fill in the following options

 - The .net Framework 4.0 directory (the path to csc.exe)
 - The MapGuide .net assemblies directory (the path to mapviewernet/bin of your MGOS/AIMS installation)
 - The MapGuide version (x.y.z). This is used to name the final provider assembly
 - Assembly type. Basically this tells us what we're referencing. Either the 5 OSGeo assemblies (2.2 and newer) or MapGuideDotNetApi.dll (2.1 and older)

Once you have filled in the required options, click build and wait a few moments. Assuming you extracted the Maestro SDK to C:\MaestroSDK:

 - The provider assembly will be under C:\MaestroSDK\LocalNativeProvider\Bin
 - The signed MapGuide assemblies the provider was built against will be under C:\MaestroSDK\LocalNativeProvider\Lib\MapGuide

Copy the built provider assembly and the MapGuide assemblies under the aforementioned directories out to your application's directory. Edit the ConnectionProviders.xml file
and register this provider assembly. Your application can now create connections for this particular provider.

Please note that it is not currently possible to a MaestroAPI application to be able to create LocalNative connections of different 
versions of MapGuide from within the same application session. For example, if you create a LocalNative connection with C:\foo\webconfig.ini
and then create a LocalNative connection with C:\bar\webconfig.ini, that connection and subsequent connections after will always be initialized
with settings from C:\foo\webconfig.ini

This is an uncommon use-case (actually more common with Maestro proper, due to its multi-connection support) since most of the time 
your application will most likely be working against a specific version of MapGuide, but it is something to keep in mind.

Testing your LocalNative provider
---------------------------------

It is recommended to run the unit tests for your LocalNative provider to ensure it is functional. This SDK includes a test runner with test data
that you can exercise your LocalNative provider with.

To test your LocalNative provider, do the following:

 1. Build the LocalNative provider using this tool
 2. Copy the LocalNative provider and supporting binaries to the test runner directory
 3. Edit ConnectionProviders.xml and include your LocalNative provider
 4. Run MaestroAPITestRunner.exe (if you built a LocalNative provider from 64-bit assemblies, run MaestroAPITestRunner64.exe). If the test 
    runner reports 0 test failures, your LocalNative provider has passed basic validation of our test suite. If there are any test failures, 
    please report such issues on Trac and indicate the version of MapGuide you built your LocalNative provider from