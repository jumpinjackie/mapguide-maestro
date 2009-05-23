Using file-based data
---------------------

Many applications produce files with data, such as a spreadsheet, ESRI Shape files or MapInfo TAB files. MapGuide has its own spatial file format, named SDF. SDF is based on SQLite, but not compatible with a SQLite tools. MapGuide also supports SQLite databases with spatial data, using an FDO provider.

Updating FDO providers
======================

When you install MapGuide, it only has a subset of the avalible FDO providers. This will hopefully be fixed in the future, but for now you can manually upgrade you FDO provider collection.

Creating a feature source
=========================

To create a feature source, simply click on the "Add" button, and select "Dataconnection". You should new see the following dialog:

.. image:: images/ProviderSelection.png

Choosing a provider
===================

If you are missing a provider for your special data type, make sure you have upgraded your FDO providers. If you have data in the SDF format, use the SDF provider. Note that SDF is actually SDF+ or SDF v3, and is not compatible with previous SDF formats, used in MapGuide 6.5 and older.

If you are using ESRI Shape files, you can use the SHP provider, or the OGR provider.

If you are using MapInfo TAB, s57 or a number of other file based data, you can use the OGR provider. I will use the OGR provider below, but the comments apply to the SHP and SDF provider as well.

When you have create a new Feature Source, your screen looks like this:

.. image:: images/NewOGRDataSource.png

Where to store the files
========================

Usually your file bases data is placed in a folder. In MapGuide you can choose to use the data from its location, or copy the data into the MapGuide server. If you use data in a folder outside of MapGuide, that is called "Unmanged data", and if you store it inside the MapGuide server, it is called "Managed data".

The reason there are two ways of placing data, is that they both have advantages and disadvantages.
Managed file advantages:

Cannot be modified by another program
Is seperated from the original data
Is contained in a backup (MGP package)
Unmanage file advantages:

Can be modified or read by another program
There is only one copy, so the data is the same
Is not contained in a backup (MGP package), but can be backed up by traditional backup programs.
As you may have noticed, these advantages are the exact oposites. Which one you prefer is entirely up to you.

Managed data
============

If you prefer managed data, you can upload the files required using the "add" button. If your data consists of multiple files, you can either select the primary file and click the "star" button, or use the entire folder as a a repository. If you use the entire folder, the OGR provider will scan it to figure out what data you have placed there. Note: Due to a bug in the FDO OGR provider, the "use entire folder" option does not work with FDO version 3.3.1 or older.

Unmanaged data
==============

If you prefer unmanaged data, you can simply enter the path to the data. Again, you can specify either a folder or a file. If you specify a folder, it must not contain a trailing slash or backslash. The "..." button will allow you to pick the file or folder. To use this feature you must setup one or more Alias folders in the MapGuide Site Administrator (avalible from the main menu, under "File", "Open Site Administrator..."). Please note that the path is to the file on the machine where MapGuide is installed, NOT the machine where MapGuide Maestro is installed.

Other OGR options
=================

OGR supports a very large set of datasources. Some of the datasources it supports are not file based. If you select the desired data type on the list MapGuide Maestro will show a customized editor for that datasource.

The generic editor
==================

If OGR is updated, or the editor shown by MapGuide Maestro does not do what you want, you can use the "Generic" editor. Simply click on the "Generic Editor" tab. The generic editor shows you the raw data that is being passed to the provider. In the generic editor, you can both view and modify the values as you please. For OGR the field "DataSource" is actually an OGR connection string:

.. image:: images/GenericFeatureEditor.png

For other providers, you must seek information on what the values should be, if they are not obvious.

In some rare cases, you may also want to modify files attached to a datasource. If you click the "Show All" button, you may see some previously hidden files. If you are really looking for adventure, you can click the "Edit as Xml..." button on the top to see and edit the underlying Xml. If you click OK, the display will reflect your changes to the Xml.

Extensions
==========

If you have data in multiple sources, you can Join datasources with extensions?.

Coordinate System Overrides
===========================

If your data does not contain coordinate system information, see Using Coordinate system overrides?.