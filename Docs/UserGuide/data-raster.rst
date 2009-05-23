Using raster data
-----------------

Updating FDO providers
======================

When you install MapGuide, it only has a subset of the avalible FDO providers. This will hopefully be fixed in the future, but for now you can manually upgrade you FDO provider collection.

Creating a feature source
=========================

To create a feature source, simply click on the "Add" button, and select "Dataconnection". You should new see the following dialog:

.. image:: images/ProviderSelection.png

Choosing a provider
===================

To use raster data, you must use the "OSGeo FDO provider for GDAL". The non-obvious name is due to the provider atually being a wrapper for the Geospatial Data Abstraction Library (GDAL) project. This is important, because the GDAL page has many tools and much information on using rasters, and most of this info applies to MapGuide as well.

If your data includes ECW files or MrSID files, you must download and install some extra files from the GDAL notes page.

If your raster data consist of a few files, you can make an easy setup, using the "Single file or folder" option. If you have a mosaic with many files, the best solution is to use the "Composite" mode.

Single file or folder
=====================

This mode is by far the easiest way to set up, but does not work well if you have more than 20 raster images. To use "Single file or folder" mode, simply enter the path to the raster data. Note that the path is as it looks from the MapGuide server, not as it looks from Maestro.

.. image:: images/rasterSingleFile.png

Composite
=========

If you have many files, or want to exclude some files from the display, "Composite" mode is the way to go. When you select "Composite", you can add any number of raster files to the FeatureSource. The files do not neccesarily have to come from the same folder, but they often do.

To add files, click the green add button, and select one of the options. The "Browse files" and "Browse folder" only works if Maestro is installed on the same machine as the MapGuide server.

When you select files, Maestro will then display a dialog showing you what files it found. When you click OK, it will proccess each file and determine what properties it has. Once done it will build a ConfigurationDocument with this info. The main benefit from this lengthy operation is that it can be done once, and not every time MapGuide tries to render a raster image.

.. image:: images/rasterComposite.png

When you are done, you should have a screen that looks like this: 

You can add and remove raster images as you like. If some of the files change, you can select the files and press the "Refresh" button.

Troubleshooting rasters
=======================

Rasters are known to be a bit tricky to get to work. The main reasons are:

 * Missing ECW or MrSID plugins
 * Invalid coordinate systems
 * The map and the raster FeatureSource must have the same coordinate system.
 
Your first step in troubleshooting rasters is the GDAL notes page. Next step would be the mapguide-users mailing list archive which is searchable from the exellent MapGuide Central.

Coordinate System Overrides
===========================

If your data does not contain coordinate system information, see Using Coordinate system overrides?. This is especially important since MapGuide does not perform raster reprojection, and thus the raster must have an override that makes it appear to have the same coordinate system as the map.

