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

.. image:: images/fs_generic.png

Coordinate System Overrides
---------------------------

TODO

.. image:: images/fs_cs_overrides.png

Joins and Extensions
--------------------

TODO

.. image:: images/fs_extensions.png

.. note::

    Feature Join performance is generally bad, especially when the join is performed across Feature Classes from different Feature Sources for different FDO providers. Consider doing joins at the database level, outside of MapGuide