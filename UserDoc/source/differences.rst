.. _maestro-differences:

Differences between Maestro and MapGuide Studio
===============================================

.. note::

 As of the 2012 release, MapGuide Enterprise is now known as Autodesk Infrastructure Map Server and the MapGuide Studio product is now called Autodesk Infrastructure Studio. This section will refer to these products by their old names.

Both Maestro and MapGuide Studio fulfill the same purpose: To create, author and publish spatial data on a MapGuide Server. However there are some main differences:

 * MapGuide Studio is a commerical product that comes with a MapGuide Enterprise licence only. It cannot be purchased separately.
 * MapGuide Maestro is free and open-source. It is the only viable authoring solution for a MapGuide Open Source user. A licensed MapGuide Enterprise user has the choice of either tools when authoring.

Fortunately, MapGuide Maestro is a capable alternative to MapGuide Studio. Besides the base shared features of loading/creating/publishing data, both tools have their own unique set of features, which this section will cover.

Features unique to MapGuide Studio / Autodesk Infrastructure Studio
-------------------------------------------------------------------

 * MapGuide Studio supports loading of DWG files and raster images. Due to licensing and/or the need to be multi-platform, Maestro does not (and probably will never) support these features.
 * MapGuide Studio supports creating and editing Symbol Libraries. Maestro cannot edit Symbol Libraries, however it has read support for these resources and Maestro supports symbol selection from Symbol Libraries.
 * MapGuide Studio supports the "Convert to SDF" and generalization options of Load Procedures.
 * Autodesk Infrastructure Studio (2013 onwards) supports publishing DWG files directly with no conversion and full visual fidelity.
 * Autodesk Infrastructure Studio supports creating connections to Industry Model (Topobase) data sources.

Features unique to MapGuide Maestro
-----------------------------------

 * Maestro is multi-platform. It runs on Windows, Linux and Mac OSX.
 * Maestro can connect to many different versions of MapGuide Open Source and MapGuide Enterprise (as far back as MGOS 1.2 / MGE 2008)
 * Maestro supports validating resources ensuring data/resource integrity and trapping common errors and pitfalls that would normally be silent under MapGuide Studio.
 * Maestro has a small installation footprint compared to MapGuide Studio.
 * Maestro allows you to edit any resource in its XML form. The XML editor is used as a fallback measure if Maestro encounters unsupported or unrecognised resources.
 * Maestro supports all known resource versions. Maestro will *never* silently upgrade your resources to the latest version.
 * Maestro's functionality can be extended by users, either through dedicated extension APIs or through the built-in IronPython scripting engine.
 * Maestro supports custom resource templates
 * Maestro supports the creation and loading of MapGuide Packages (MGP) without needing to use the Site Administrator web application.
 * Maestro has a more active development cycle.
 * Maestro offers plenty of tools and shortcuts that greatly improves authoring productivity.
 * Maestro supports authoring data from an mg-desktop application
 * Maestro includes other various tools and utilities that fully exploit the services provided by MapGuide
   * A Feature Source previewing utility
   * A Runtime Map state inspection tool
   * A tile pre-generation tool (MgCooker)
   * A "Live" Map Definition editor that supports editing workflows similar to MapGuide Author 6.5
 * and much more!