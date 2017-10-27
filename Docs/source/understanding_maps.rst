Understanding Map Definitions
=============================

The Map Definition represents the collection of layers to be displayed. Layers are organised logically into 
groups and can be sorted by drawing order. Individual layer visibility and legend visibily control can be 
configured here.

The Map Definition editor is composed to two sections, both are covered below.

Map Settings
------------

The **Map Settings** section of the editor allows you to define the core properties of this Map Definition

.. figure:: images/mdf_settings.png

You can specify the coordinate system, background color and the initial extents that this map encompasses. The
coordinate system and initial extents are automatically set when the first layer is added to this map. You can
re-calculate the extent at any time.

.. note::
    The coordinate system you specify here is determines the units and coordinates that are displayed when you 
    view this map from the AJAX or Fusion viewer. In addition, it is also used to automatically re-project 
    any layers whose coordinate systems do not match. 
    
.. note::
    Automatic layer re-projection incurs a minor performance penalty when rendering the map. So if possible
    ensure that all the layers are in the same coordinate system (as the one you specify here)
    
.. note::
    Extent calculation can take some time, especially if there are lots (hundreds) of layers in the Map Definition.
    In such cases, it is faster to enter the numbers manually.
    
.. note::
    Extent calculation can even be inaccurate, especially if one or more layers references a badly set up feature
    source (eg. A GDAL raster feature source is a common source of bad extents). Again, in such cases manual
    entry of extents is faster and safer.

Layer Configuration
-------------------

The **Layer Configuration** section allows you to organise the layers in the Map Definition by different facets.

Selecting a layer or group in any of these tabs will allow you to edit the properties of that layer or group

Layers by Group
^^^^^^^^^^^^^^^

The **Layers by Group** tab allows you to organise the layers in your Map Definition logically into groups

.. figure:: images/mdf_layers_grouping.png

A Layer Group in this view may be converted to a Base Layer Group.

Layers by Drawing Order
^^^^^^^^^^^^^^^^^^^^^^^

The **Layers by Group** tab allows you to organise the layers in your Map Definition by drawing order

.. figure:: images/mdf_layers_draworder.png

Base Layer Groups
^^^^^^^^^^^^^^^^^

The **Base Layer Groups** tab allows you to define and configure tiled layer and group settings

.. figure:: images/mdf_layers_base.png