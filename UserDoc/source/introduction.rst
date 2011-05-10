Introduction
============

MapGuide Maestro is an Open Source (LGPL) map authoring tool for MapGuide Open Source and Autodesk MapGuide Enterprise / Autodesk Infrastructure Map Server.

MapGuide Maestro is a free application that can ease the management of spatial data in a MapGuide Server.

MapGuide Maestro is a windows application written in 100% managed C#, targeting the .Net 2.0 Framework and can run in Linux and Mac OSX with the Mono framework (http://www.mono-project.com). It consists of a user interface, and an API (Maestro API). The API wraps server communication and Xml formats, in easily accesible, fully managed classes. It can use the official MapGuide API for fast access on the local network, or a fully managed http-only connection. Other connection types are planned in future releases.

MapGuide Maestro is designed with flexibility and extensibility in mind. The tool tries very hard not to take any options away, and always allows you to edit the internal Xml of the feature you are working with. This ensures that features that are missing from the interface can still be accessed.