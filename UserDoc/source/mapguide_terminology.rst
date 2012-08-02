Understanding MapGuide Resources
================================

.. todo::
    Currently copy/paste of existing wiki content

Before you can start using MapGuide Maestro, you must understand how MapGuide divides the different parts that make up a map. To understand the MapGuide Maestro interface, you must also have a knowledgde of the names used in MapGuide. 

.. todo::
    Detail the features offered with each schema revision. Most of this already is covered in resource-readme.txt in the Maestro source code.

There are many types of resources that can be stored in a MapGuide Server

Feature Sources
^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Feature Sources describe a FDO connection to a particular data store, which could be inside the MapGuide Server or stored externally. (see below: Spatial Data and Regular Data)

Drawing Sources
^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Drawing Sources are DWF data sources which provide drawing data with full visual fidelity. 

Layer Definitions
^^^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007
 * 1.1.0 - MapGuide Open Source 1.2 / MapGuide Enterprise 2008
 * 1.2.0 - MapGuide Open Source 2.0 / MapGuide Enterprise 2009
 * 1.3.0 - MapGuide Open Source 2.1 / MapGuide Enterprise 2010
 * 2.3.0 - Autodesk Infrastructure Map Server 2012
 * 2.4.0 - MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

Layer Definitions describe how data from a Feature Source is to be styled. (see below: Displaying Data)

Layer Definitions are can be shared among different maps.

Map Definitions
^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007
 * 2.3.0 - MapGuide Open Source 2.3 / MapGuide Enterprise 2012
 * 2.4.0 - MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

Map Definitions contains one or more Layer Definitions grouped together and sorted by drawing order.

Web Layouts
^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007
 * 1.1.0 - MapGuide Open Source 2.2 / MapGuide Enterprise 2011
 * 2.4.0 - MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

Describes the user interface for the AJAX viewer (see: Understanding MapGuide Applications - Web Layouts)

Application Definitions
^^^^^^^^^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 2.0 / MapGuide Enterprise 2009

Version(s): 

 * 1.0.0 - MapGuide Open Source 2.0 / MapGuide Enterprise 2009

Describes the user interface for the Fusion viewer (see: Understanding MapGuide Applications - Application Definitions)

Load Procedures
^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 
 
 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007
 * 1.1.0 - MapGuide Open Source 2.0 / MapGuide Enterprise 2009
 * 2.2.0 - MapGuide Open Source 2.2 / MapGuide Enterprise 2011

Describes to a client application like MapGuide Studio, how to load a set of data files into the MapGuide Server

Print Layouts
^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Describes the printing layout for a DWF plot operation

Symbol Libraries
^^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.0 / MapGuide Enterprise 2007

Describes a collection of DWF-based symbols that can be used for point styles in a Layer Definitions

There is no specialized editor support for Symbol Libraries. Opening Symbol Libraries will default to the generic XML editor.

Symbol Definitions
^^^^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 1.2 / MapGuide Enterprise 2008

Version(s): 

 * 1.0.0 - MapGuide Open Source 1.2 / MapGuide Enterprise 2008
 * 1.1.0 - MapGuide Open Source 2.0 / MapGuide Enterprise 2009
 * 2.4.0 - MapGuide Open Source 2.4 / Autodesk Infrastructure Map Server 2013

Describes a cartographic symbol, which has lots of display and customisation parameters. Used by Layer Definitions.

Watermark Definitions
^^^^^^^^^^^^^^^^^^^^^

Introduced: MapGuide Open Source 2.3 / MapGuide Enterprise 2012

Version(s): 

 * 2.3.0 - MapGuide Open Source 2.3 / MapGuide Enterprise 2012

Describes a watermark that is rendered as part of the map when viewing it. Used by Map Definitions.