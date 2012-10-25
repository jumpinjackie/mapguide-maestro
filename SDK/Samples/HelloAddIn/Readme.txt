HelloAddIn README
=================

HelloAddIn is an example of an add-in for MapGuide Maestro.

AddIns extend the functionality of MapGuide Maestro

Anatomy of a Maestro AddIn
==========================

A Maestro AddIn consists of the following files:

 - The AddIn assembly and its supporting libraries
 - The AddIn manifest file (with a .addin extension)

The AddIn manifest describes the addin and the extension points that it will integrate into. See the Manifest.addin for more information

Supporting Libraries
====================

A Maestro AddIn requires referencing the following assemblies:

 - Maestro.Base (Your Maestro directory or under addin-bin in the SDK)
 - ICSharpCode.Core (Your Maestro directory or under addin-bin in the SDK)
 - ICSharpCode.Core.WinForms (Your Maestro directory or under addin-bin in the SDK)
 - Maestro.Shared.UI (Your Maestro directory or under bin in the SDK)

Additionally, you will most likely want to reference these libraries as well in order to tap into some of the shared components used by Maestro:

 - OSGeo.MapGuide.MaestroAPI (Your Maestro directory or under bin in the SDK)
 - Maestro.Editors (Your Maestro directory or under bin in the SDK)
 - Maestro.Login (Your Maestro directory or under bin in the SDK)

All of these aforementioned libraries do not have to be included with your final addin project output. To reduce deployment footprint, set these references to <Copy Local = false>
so that these references won't be copied to your project's output directory

Debugging your AddIn project
============================

To debug your add-in project, it is recommended for your project to have the following settings:

 - Build: Output Directory: <your maestro directory>\AddIns\<Your AddIn name>
 - Debug:
	- Start External Program: <your maestro directory>\Maestro.exe
	- Working Directory: <your maestro directory>

Debugging this project will launch Maestro and attach the VS debugger to it.

AddIn Packaging/Deployment
==========================

There are many ways to package and deploy your add-in:

 1. Copying your addin directory to <your maestro directory>\AddIns
 2. Zipping your addin directory and installing this zip file via Maestro's AddIn Manager

Method #1 is okay as long as <your maestro directory> is not under Program Files as this directory is extremely locked down in Windows Vista and newer (due to UAC). If you installed Maestro via the Windows installer, then this is not a viable method of deployment. Uninstallation is a case of deleting this addin directory

Method #2 is the preferred method as such addins will be installed to a safe location that is UAC compatible (addins are generally installed to: %APPDATA%\Maestro-5.0)

AddIn installations and uninstallations via Maestro's AddIn Manager require a restart to take effect

Other AddIn notes
=================

Your addin project cannot be 64-bit or consume 64-bit libraries. It must either be x86 or AnyCPU. This is because Maestro.exe is a 32-bit application, and thus can only load 32-bit or AnyCPU assemblies.