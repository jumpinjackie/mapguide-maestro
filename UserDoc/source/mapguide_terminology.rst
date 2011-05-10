Understanding MapGuide Terminology
==================================

.. todo::
    Currently copy/paste of existing wiki content

Before you can start using MapGuide Maestro, you must understand how MapGuide divides the different parts that make up a map. To understand the MapGuide Maestro interface, you must also have a knowledgde of the names used in MapGuide. 

Types of Resources
------------------

...

Spatial Data and Regular Data
-----------------------------

Data can come from a number of sources, such as a spread sheet or a database. Some data is spatially enabled, and this means that it has one or more columns of data that describes a geometric entry. Such data is usually called vector data. In spatially enabled data, the non-spatial data is usually called "attributes".

Some data has an X and Y column (or LAT/LON), and is thus not spatially enabled. MapGuide can treat such data as if it was spatially enabled though.

Another type of data is raster data, which is typically some form of aerial foto.

MapGuide uses  FDO to connect to data, and calls each connection a "FeatureSource", regardless of what type of data it points to.

A "FeatureSource" does not have any information about how it looks, it's just lines, polygons and points. 

Displaying Data
---------------

In MapGuide a "LayerDefinition" is used to describe how to represent a layer visually. This distinction between data and representation, means that you can define your "FeatureSource" once, and display it in red, green and blue.

In a "LayerDefinition" you define how the layer should look in different "ScaleRanges". A "ScaleRange" is defined by two numbers, and while the map is displayed at a given scale, MapGuide will show it as the first matching "ScaleRange". If you define a "ScaleRange" 0 - 1000 and one 1001 - 2000, you can display the data differently depending on how close the user is viewing the map.

For each "ScaleRange" you can define a number of rules. This can be used to visually show difference in the non-spatial values, eg. land value, or a broken pipe, etc. The default rule in MapGuide is a blank rule, which will match all data.

If you use rules to display eg. land values with a gradient color, ranging from low land value to high land value, it is called "Theming".

An object in the map can also have a "Label", which is displayed on top of the item. This can be used to display road names.

You can also set a "Tooltip" on an object, which will be displayed when the cursor hovers over the object.

Finally you can set a "Link" on an object, which will open when the user double clicks on the feature (CTRL+click in some places). 

Combining Data
--------------

Once you have set up the visual apperance of your layers, you can combine them together in a map. In the map, the layers have a "Drawing Order", which is used to order how the layers are drawn. If you have a county and a subway layer, you might not be able to see the subways if they are below the large county layer. Usually polygons are at the bottom, then lines, then points, and finally labels on top.

In MapGuide, such a collection of layers is called a "MapDefinition". In a "MapDefinition", you can also group layers, so the users can easily toggle the visibilty of a number of layers with a single click.

In a "MapDefinition" you can also define what layers should be visible in the legend, and what names should be displayed in the legend. 

Presenting Data to the user
---------------------------

Now that you have a "MapDefinition", you can set up a frame the user can view the data in. This frame defines what tools the user has (Zoom tool, select tool, measure tool, etc.), as well as what items are visible (legend, overview map, etc.).

There are currently two types of frames you can use, one called "WebLayout" and one called "Fusion Application". Autodesk has named the "Fusion Application" "Flexible Layout", so you may see that term as well.

MapGuide Packages
-----------------

When you have put a lot of effort into setting up a map, you can take a backup of the entire setup. Such a backup is known as a "Package". A "Package" is a compressed file with the extension ".mgp". You can create, edit and restore such packages using either MapGuide Maestro, or the MapGuide Site Administrator. 