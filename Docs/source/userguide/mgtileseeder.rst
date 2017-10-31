.. _using-mgtileseeder:

Using MgTileSeeder
==================

Introduction
------------

**MgTileSeeder** is the successor to **MgCooker**. It has the following improvements over **MgCooker**:

 * Supports seeding of XYZ tile sets introduced in MapGuide Open Source 3.0
   * Can also function as a generic seeder for any tile set based on a XYZ tiling scheme (eg. OpenStreetMap)
 * A simpler, cleaner architecture with pluggable tile walking strategies
 * **MgTileSeeder** is a truly cross-platform console application, targeting .net Core 2.0 (and all the platforms supported by .net Core 2.0)

Unlike **MgCooker**, the **MgTileSeeder** tool is currently a purely commandline tool. There is no GUI counterpart.

.. note::

    **MgTileSeeder** is also only available currently as a standalone package. It is not currently bundled with MapGuide Maestro.

Usage
-----

**MgTileSeeder** has 2 modes of operation:

 * Standard tiling of MapGuide tiled map definitions or tile sets (using the default provider)
 * XYZ tiling of any XYZ tile set url with ``{x}``, ``{y}`` and ``{z}`` placeholders and a specific bounding box in lat/long coordinates

The basic command syntax for standard tiling is as follows:

.. highlight:: bash
.. code-block:: bash

    MgTileSeeder.exe mapguide --mapagent <mapagent url> --map <mapdefinition/tileset resource id> --meters-per-unit <meters per unit> [--username <MapGuide username>] [--password <MapGuide user password>] [--groups <Base Layer Group Names>] [--minx <BBOX minx>] [--miny <BBOX miny>] [--maxx <BBOX maxx>] [--maxy <BBOX maxy>]

The basic command syntax for xyz tiling is as follows:

.. highlight:: bash
.. code-block:: bash

    MgTileSeeder.exe xyz --url <url> --minx <BBOX minx> --miny <BBOX miny> --maxx <BBOX maxx> --maxy <BBOX maxy>

Examples
--------

The following example starts a tiling run for a tiled map ``Library://Samples/Sheboygan/MapsTiled/Sheboygan.MapDefinition``

.. highlight:: bash
.. code-block:: bash

    MgTileSeeder mapguide --mapagent "http://localhost/mapguide/mapagent/mapagent.fcgi" --map "Library://Samples/Sheboygan/MapsTiled/Sheboygan.MapDefinition" --meters-per-unit 111319.4908

The following example starts a tiling run for a XYZ tileset ``Library://Samples/Sheboygan/TileSets/Sheboygan.TileSetDefinition``

.. highlight:: bash
.. code-block:: bash

    MgTileSeeder xyz --url "http://localhost/mapguide/mapagent/mapagent.fcgi?OPERATION=GETTILEIMAGE&VERSION=1.2.0&CLIENTAGENT=OpenLayers&USERNAME=Anonymous&MAPDEFINITION=Library://Samples/Sheboygan/MapsTiled/Sheboygan.MapDefinition&BASEMAPLAYERGROUPNAME=Base+Layer+Group&TILECOL=${y}&TILEROW=${x}&SCALEINDEX=${z}" --minx -87.797866013832 --miny 43.6868578621819 --maxx -87.6645277718692 --maxy 43.8037962206133

Note the ``{x}``, ``{y}`` and ``{z}`` placeholders in the above example. MgTileSeeder's ``xyz`` mode can work with any XYZ tile set (not just ones served by MapGuide).