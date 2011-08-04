This is a monolithic version of NetTopologySuite 1.8.

The v1.8 source and binaries was modified in the following way:

 - The NetTopologySuite project was changed to target .net framework 2.0 and references LinqBridge 1.2 to provide a FX 3.5 compatibility layer. Existing unit tests still pass with this change.
 - NetTopologySuite and its dependent assemblies (GeoAPI, LinqBridge, NetTopologySuite, PowerCollections, ProjNet) were combined with ILMerge-GUI (http://ilmerge-gui.devv.com)