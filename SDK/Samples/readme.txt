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