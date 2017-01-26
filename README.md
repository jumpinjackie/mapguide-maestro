# MapGuide Maestro

[![Build status](https://ci.appveyor.com/api/projects/status/7250l5cn21yfr1mb?svg=true)](https://ci.appveyor.com/project/jumpinjackie/mapguide-maestro)

MapGuide Maestro is an Open Source ([LGPL 2.1](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.en.html)) map authoring application for [MapGuide Open Source](https://mapguide.osgeo.org) and [Autodesk Infrastructure Map Server](http://www.autodesk.com/products/infrastructure-map-server/overview).

MapGuide Maestro is written in 100% managed C#, targeting the .net Framework. It consists of:

 * The Maestro windows application.
    * Provides specialized user interfaces for editing most resource types supported by MapGuide/AIMS
    * Designed for maximum productivity and authoring efficiency
    * Provides validation services with a rich set of validation rules for ensuring integrity of your resources, data sources and configurations.
    * Maestro is built on a flexible extension architecture allowing for custom addins. Some addins bundled with Maestro include:
       * An addin for managing resources in a locally bundled [mg-desktop](https://trac.osgeo.org/mapguide/wiki/mg-desktop)
       * An addin for creating and editing [mapguide-rest](https://github.com/jumpinjackie/mapguide-rest) configuration files.
       * An addin for scripting/automating Maestro via [IronPython](http://ironpython.net/)
 * An API (MaestroAPI) that provides unified access to services provided by MapGuide/aims that consists of:
    * A strongly-typed model library that models all the resources provided by MapGuide/AIMS, allowing you to work with classes, interfaces and objects instead of manipulating XML documents.
    * A unified provider-based API for accessing services provided by MapGuide/AIMS through a common set of interfaces:
        * A HTTP provider for accessing the mapagent
        * A provider that wraps the official MapGuide API
        * A provider that wraps the [mg-desktop](https://trac.osgeo.org/mapguide/wiki/mg-desktop) API

MapGuide Maestro is designed with flexiblity in mind. The application tries very hard not to take any options away, and always allows you to edit the internal XML representation of any resources you are working with. This ensures that features that are missing from the specialized editor user interfaces can still be accessed, and allows Maestro to be future-proofed against any new resource types that MapGuide/AIMS may introduce in the future.

## Unsupported Features

MapGuide Maestro aims to most of the features avalible in MapGuide Open Source and AIMS, but a few things are not (and will not) be supported:

 * Creating and editing Symbol Library resources
 * Full support for Load Procedures. Current support only covers loading of SDF/SHP/SQLite files without no support for file conversion or transforming coordinates.

## Issues/Questions/etc

When using the tool, please bear in mind that the project is a work in progress, and may not always perform as expected. Try to keep backups of all important data. If you encounter an error, or have a feature request, please check our [issue list](https://github.com/jumpinjackie/mapguide-maestro/issues), and if it has not already been reported ​please report it (You must have a GitHub account). If you have a question about the usage, you can ask at the [mapguide-users mailing list](http://lists.osgeo.org/listinfo/mapguide-users).