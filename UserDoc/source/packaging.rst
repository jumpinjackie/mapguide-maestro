Packaging Data
==============

.. index::
    single: Resources; Packaging and Loading

**MapGuide Packages** allow you to import and export portions of the MapGuide Server repository to another MapGuide Server repostiory.

It is a very useful way to backup, migrate and deploy data from one MapGuide Server to another.

The MapGuide Server Site Administrator web application allows you to load and create these packages. But has some
downsides:

 * It is a server-side web application. Loading and retrieving packages requires the package be stored physically on the server's package directory
 * Ability to automate such a process is limited.
 
MapGuide Maestro however, allows you to load and create such packages **completely on the client-side**.

Creating Packages
-----------------

There are 2 ways to load a package:

 1. The **Package - Package Folder** menu option.
 2. Right clicking the desired folder in the **Site Explorer** and choosing the **Package Folder** option.

The time it takes to create a package is a function of how many resources in total are in the folder to be packaged up.

Loading Packages
----------------

There are 2 ways to load a package:

 1. The **Package - Load Package** menu option
 2. Dragging and dropping the package file into the **Site Explorer**

The time it takes to load a package is a function of how large the package is. Once loaded, the **Site Explorer**
will refresh itself.

.. note::

    Because Maestro communicates to the MapGuide Server over http, loading large packages (over several hundred MBs) can be problematic. As such
    using the Site Administrator is recommended. Otherwise, an data moving strategy is required (eg. Repository backup/restore or use of aliases instead of embedded data files)