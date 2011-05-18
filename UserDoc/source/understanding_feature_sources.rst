Understanding Feature Sources
=============================

As already covered, **Feature Sources** describe a FDO connection to a particular spatial or non-spatial data store. FDO technology provides a generic, 
command-based interface to a number of back-end data source technologies for storing, retrieving, updating, and analyzing GIS (Geographic Information System) data.

Support for various data sources exists in the form of FDO providers.

File-based Data
---------------

Spatial data exists in many different forms and data formats. MapGuide supports various file-based data sources out of the box:

 * SDF using the SDF FDO provider
 * SHP using the SHP FDO provider
 * SQLite using the SQLite FDO provider (for MapGuide Open Source 2.2 or newer)
 * MS Access Databases and Excel Spreadsheets using the ODBC provider.
 * Various other file formats using the OGR FDO provider.

Support for other file formats exist via third party FDO providers.

Relational Data
---------------

MapGuide also supports various relational data sources

 * SQL Server
 * MySQL
 * Oracle
 * PostgreSQL (for MapGuide Open Source 2.2 or newer)

Support for other relational databases exist via third party FDO providers.

Raster Data
-----------

MapGuide also supports raster data sources via the GDAL FDO provider.

The Generic Editor
------------------

Maestro provides a specialized feature source editor for the most frequently used FDO providers. For third party or other unrecognised FDO providers, a generic editor is
available to configure the connection parameters to your particular data store.

.. figure:: images/fs_generic.png
   
   *The Generic Feature Source Editor*

If a connection property involves an embedded data file, upload the file first as a **Resource Data File** and you can reference it 
by prefixing `%MG_DATA_FILE_PATH%` in front of the resource data file name.

Coordinate System Overrides
---------------------------

The Coordinate System Overrides section of the editor allows you to override the coordinate systems of this Feature Source. 

.. figure:: images/fs_cs_overrides.png
   
   *The Coordinate System Overrides Editor*

For example, a Feature Source may report an Arbitrary or incorrectly specified coordinate system, but the actual geometries themselves 
have real world geographical relevance. You can use this editor to replace this Arbitrary coordinate system with a different coordinate 
system. 

This is  important if you require MapGuide to transform data. The source and target coordinate systems need to be correct. This override 
mechanism allows you to fix the source side of the transformation.

Joins and Extensions
--------------------

The Joins and Extensions section of the editor allows you to define **Extended Feature Classes**.

.. figure:: images/fs_extensions.png

   *The Joins and Extensions Editor*

Extended Feature Classes extend a given feature class in the edited feature source with:

 * Extra calculated properties derived from FDO expressions
 * Extra properties from another Feature Class in another Feature Source by performing a **Feature Join**
 
If extending a Feature Class via a **Feature Join** you may choose the type of join to perform:

 * Left Outer
 * Right Outer
 * Inner 
 
Also you have to specify at least one property from both participating Feature Classes that will be joined on.

Finally you can force a 1-to-1 cardinality to avoid redundant secondary Feature Class attributes for each primary feature or vice versa.

.. note::

    Feature Join performance is generally bad, especially when the join is performed across Feature Classes from different Feature Sources for different FDO providers. Consider doing joins at the database level, outside of MapGuide