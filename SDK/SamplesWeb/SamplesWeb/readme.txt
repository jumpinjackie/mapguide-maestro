Setup Instructions
------------------

In IIS, create a virtual directory named "SamplesWeb" as a child under the main mapguide virtual directory.

Set the default page to Default.aspx

This sample requires the Sheboygan sample dataset. The sample will check if this dataset exists on startup.

To debug this project from Visual Studio, set the project to start using IIS using the url of the virtual directory you
created. If you are on Windows Vista or newer, Visual Studio needs to be run as Administrator in order to be able
to debug ASP.net applications under IIS.