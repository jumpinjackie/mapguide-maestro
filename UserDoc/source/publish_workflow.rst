The Maestro Publishing Workflow
===============================

The basic workflow for creating maps and publishing them in Maestro is shown below

.. image:: images/maestro_publish_workflow.png

The first step is to connect to a MapGuide Server. Maestro will always prompt for a login when starting up.

Find Data
---------

For this step, you have 2 choices.

 1. **Create a Load Procedure**. A **Load Procedure** specifies a set of source files to load from. When the **Load Procedure** is executed, the following occurs:
 
  * A **Feature Source** is created for each source file
  * A default **Layer Definition** is created for each created Feature Source
  
  **Load Procedures** will always load the source files into the MapGuide Server as embedded resource data of the Feature Source. If you want to maintain
  a connection to externally stored files, then you will have to create these **Feature Sources** manually. This approach is a quick way to load data into MapGuide.
  **Load Procedures** can be saved and executed at a later point in time to refresh this data. 
  
 2. **Create Feature Sources**. For data sources like relational databases or web services, you do not have the **Load Procedure** option available. So therefore 
    you have to create these **Feature Sources** manually. If you want to connect to externally stored or aliased data files, you have to use this approach.
  
Build Layers
------------

For this step, create **Layer Definitions** that point to a **Feature Source**. **Layer Definitions** give defines style and presentation rules for data coming from a
**Feature Sources**. You can do things like:

 * Theme data by attributes
 * Style line thickness by attributes (or a computed expression based on attributes)
 * Size point features by attributes (or a computed expression based on attributes)
 * Use attributes (or a computed expression based on attributes) for feature labels
 * Filter data by an attribute filter

**Layer Definitions** also contains settings for viewer interactions.

 * You can specify a tooltip that is shown when you put a mouse over a feature from this layer. This can be an attribute or a computed expression based on attributes
 * You can specify a url that is launched when the user selects a feature from this layer. This can be an attribute or a computed expression based on attributes.

If you created and executed a **Load Procedure**, then these **Layer Definitions** have been created for you. However these are created with default settings 
(monochromatic styles with basic symbology). So you will have to open and edit these resources to suit your needs.

Make a Map
----------

For this step, create a **Map Definition** and bring in the **Layer Definitions** you have created. Organise these layers into groups (optional) and sort them by drawing 
order with rasters and polygon layers at the bottom, followed by line layers, then followed by point layers. You don't need to specifically follow this draw order, but
it's a generally useful rule to follow.

For layers that change very rarely, you can set them as tiled layers. Tiled layers are rendered once as tiles and stored into a Tile Cache. Subsequent requests for the
same view will request the cached tiles. This is a useful way to improve map performance.

.. note::

    If you edit and save a map with tiled layers, the existing Tile Cache is deleted, as the generated tiles most likely no longer apply to the new map. Maestro will
    warn you about such situations.

Make the map available
----------------------

For this step. You can create either a **Web Layout** or a **Application Definition** which describe the viewer interface which to view and interact with the map.

A **Web Layout** describes the user interface and functionality for the basic AJAX viewer. The AJAX viewer provies basic functionality for map viewing and interaction.

A **Flexible Layout** describes the user interface and functionality for the Fusion viewer. The Fusion viewer has greater flexibility in terms of looks and functionality.

Once you saved either resource, a preview URL will be visible in the respective editor. This URL allows your users to view the specific map using the specified 
viewer application.