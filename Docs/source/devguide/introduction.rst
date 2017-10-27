Introduction
============

The code that drives the MapGuide Maestro application can also be used in your own applications. This is collectively known as the **Maestro API**

The Maestro API consists of the following components:

 * (Optional) The FDO expression parsing library (OSGeo.FDO.Expressions)
 * The MapGuide Resource Object Model library (OSGeo.MapGuide.ObjectModels)
 * The MapGuide Maestro API (OSGeo.MapGuide.MaestroAPI)

OSGeo.FDO.Expressions
---------------------

This is an optional library for parsing FDO expression strings into Abstract Syntax Trees (AST) for structured analysis of the expression.

The MapGuide Maestro application uses this library for parsing and validating FDO expressions against the feature classes and properties they are referencing.

OSGeo.MapGuide.ObjectModels
---------------------------

This is a library that defines all the resource types in MapGuide as strongly-typed classes and relieves the burden from the MapGuide Developer from working with resources as XML documents.

OSGeo.MapGuide.MaestroAPI
-------------------------

This library is the heart of the Maestro API. It provides a set of unified interfaces for working with the various services provided by MapGuide.

The Maestro API offers a provider model that allows for consuming these interfaces across various implementations:

 * The HTTP mapagent
 * The official MapGuide .net API
 * The mg-desktop implementation of the MapGuide API