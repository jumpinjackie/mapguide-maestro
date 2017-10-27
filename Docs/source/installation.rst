Installing MapGuide Maestro
===========================

MapGuide Maestro is available in two forms:

 * A windows executable installer
 * A zip file

Installing on Windows
---------------------

You can choose either the installer or zip file 

.. note::

 MapGuide Maestro requires .net Framework 4.0 or higher installed first.
 
.. note::

 As of the 4.0 release of Maestro, the zip package differs from the installer package in that the zip package does not include the :doc:`Local Connection Mode <local_connection_mode>` feature.

Installing on Linux/Mac OSX
---------------------------

Before you run MapGuide Maestro, you need to install the Mono framework. Mono is available for download at `http://www.mono-project.com <http://www.mono-project.com>`_

The installation is simply unzipping the file::

 unzip "MapGuideMaestro-5.0.0-Release.zip"

Then run the application::

 cd Maestro
 mono Maestro.exe
 
.. note::

    For distributions like Ubuntu, the Mono package is split into several components. You need to ensure that the winforms component is installed. The following command installs it for Ubuntu::
    
     sudo apt-get install mono-runtime libmono-system-windows-forms4.0-cil libmono-system-core4.0-cil libmono-system-numerics4.0-cil libmono-system-web4.0-cil
    
    For other distros, please consult your system documentation. 
    
.. note::

    **Please note that Mono support for WinForms is not 100% stable.** Even though WinForms is feature complete, some features do not work flawlessly. This means that there are occasional drawing problems. Flipping a tab page back and forth usually solves the problem. Some buttons are missing, because Mono renders them outside the visible area. There are also some issues with certain icons.
    For best results, use the most current release of Mono (currently 2.10.2) where possible. The minimum usable release is Mono 2.8
    
.. note::

    The :doc:`Local Connection Mode <local_connection_mode>` feature is only available on windows. The zip package does not include this feature.

