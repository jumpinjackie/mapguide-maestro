This directory contains a series of samples demonstrating usage of the Maestro API

DrawMap
-------

This sample demonstrates using the Maestro API to create a Runtime Map from a Map Definition and to render the map to an image.

QueryFeatureSource
------------------

This sample demonstrates using the Maestro API to query data from a feature source

NOTE: Due to a long-standing bug on the mapagent operation which this underlying sample relies on (http://trac.osgeo.org/mapguide/ticket/708)
this sample may not fully work for MapGuide Open Source 2.1 or older releases. This problem is only applicable if you're connecting to this
sample via http.

SchemaViewer
------------

This sample demonstrates using the Maestro API to describe and present the schema(s) of a feature source in a WinForms TreeView. Each
selected node can be shown in the WinForms PropertyGrid beside it.

DwfInspector
------------

This sample demonstrates using the Maestro API to describe and inspect a Drawing Source or a DWF file, which can be uploaded
as a temporary Drawing Source

StandaloneWebLayoutEditor
-------------------------

This sample demonstrates using the Maestro.Editors library allowing you to re-use the resource editor UI components for use outside of MapGuide Maestro

MapViewer
---------

This sample demonstrates using the Maestro.MapViewer library for an easily embeddable map viewer with most functionality found in the AJAX viewer, but as a pure WinForms library (this is not an embedded WebBrowser that connects to the AJAX viewer)