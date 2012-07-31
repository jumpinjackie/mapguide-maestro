Resource Schema Changelog:
--------------------------

Layer Definition
----------------

All changes relate to vector layers only. As such, creating drawing or raster layers in Maestro
will always use the v1.0.0 schema

v1.1.0
------

Introduced: MapGuide Open Source 1.2 / MapGuide Enterprise 2008

 - Support for Advanced Symbolization (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc14)
 - Support for KML extrusion (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc16)
 - Support for Map-space line widths (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc17)
 
v1.2.0
------

Introduced: MapGuide Open Source 2.0 / MapGuide Enterprise 2009
 
 - Support for extra Advanced Stylization options (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc29)
 
v1.3.0
------

Introduced: MapGuide Open Source 2.1 / MapGuide Enterprise 2010
 
 - Support for disabling certain geometry styles from appearing in the legend (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc50)
 

v2.3.0
------

Introduced: Autodesk Infrastructure Map Server 2012

 - Support for Watermarks (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc108)

v2.4.0
------

Introduced: MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

 - No new features. Schema version bump a result of Symbol Definition version bump to 2.4.0

Map Definition
--------------

v2.3.0
------

Introduced: Autodesk Infrastructure Map Server 2012

 - Support for Watermarks (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc108)
 
v2.4.0
------

Introduced: MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

 - No new features. Schema version bump a result of Symbol Definition version bump to 2.4.0

Symbol Definition
-----------------

v1.1.0
------

Introduced: MapGuide Open Source 2.0 / MapGuide Enterprise 2009

 - Support for edit controls and rich text (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc31)

v2.4.0
------

Introduced: MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

 - Support for path scaling (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc113)

Web Layout
----------

v1.1.0
------

Introduced: MapGuide Open Source 2.2 / MapGuide Enterprise 2011

 - New ping server option (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc66)

v2.4.0
------

Introduced: MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

 - Support for a new tooltip-toggle command (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc120)

Load Procedure
--------------

v1.1.0
------

Introduced: MapGuide Open Source 2.0 / MapGuide Enterprise 2009
 
 - DWG-specific changes not relevant to Maestro (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc27)
 - As this version introduces changes not relevant to Maestro, there is no way to create Load Procedures of this version as it is not necessary
 
v2.2.0
------

Introduced: MapGuide Open Source 2.2 / MapGuide Enterprise 2011

 - New SQLite load procedure type (http://trac.osgeo.org/mapguide/wiki/MapGuideRfc85)