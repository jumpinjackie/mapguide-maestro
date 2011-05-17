Installing MapGuide Maestro
===========================

MapGuide Maestro is available in two forms:

 * A windows executable installer
 * A zip file

Installing on Windows
---------------------

You can choose either the installer or zip file 

.. note::

 MapGuide Maestro requires .net Framework 2.0 or higher installed. For Windows Vista or newer versions of Windows, this is already installed.

Installing on Linux/Mac OSX
---------------------------

Before you run MapGuide Maestro, you need to install the Mono framework. Mono is available for download at `http://www.mono-project.com <http://www.mono-project.com>`_

The installation is simply unzipping the file::

 unzip "MapGuideMaestro-3.0.0-Release.zip"

Then run the application::

 cd Maestro
 mono Maestro.exe
 
.. note::

    For distributions like Ubuntu, the Mono package is split into several components. You need to ensure that the winforms component is installed. The following command installs it for Ubuntu::
    
     sudo apt-get install mono-runtime libmono-winforms2.0-cil
    
    For other distros, please consult your system documentation. 
    
.. note::

    **Please note that Mono support for WinForms is not 100% stable.** Even though WinForms is feature complete, some features do not work flawlessly. This means that there are occasional drawing problems. Flipping a tab page back and forth usually solves the problem. Some buttons are missing, because Mono renders them outside the visible area. There are also some issues with certain icons.
    For best results, use the most current release of Mono (currently 2.10.2) where possible. The minimum usable release is Mono 2.0 (this was the release where WinForms was 100% API complete) 

