Building a Fusion Application
-----------------------------

This page describes briefly how to get started with the Fusion editor in MapGuide Maestro.

It is very easy to create a new Fusion application, just click the Add icon and choose "Fusion Application".

To select a template, click the browse button on the right of the url text box.

You may reference any number of maps inside the Fusion application. However, there is no support to edit the MapWidget part of the new Maps. You can edit these in the Xml. If you have just one map, do not edit the ID of the map, and you should be safe.

I need a sample Fusion application that uses multiple maps, before I can finish that part. If you have a multi-map sample that I can use, please email it to me.

Fusion groups its widgets in containers. You can create containers of type "ContextMenu" or "Toolbar". Each container may have an arbitrary number of widgets, seperators and submenus. You can only use widgets that are defined. To create a widget, click to Widget (gear icon) button.

The newest version is now capable of editing the extensions in the widgets. For all known widgets, there are customized editors, and for extra/unknown extensions you may use the datagrid. The grid and the custom editor is syncronized. You may add or delete extensions as you please. Trying to add two rows with the same name, will result in one being overwritten with data from the other, ie. you end up with just one entry.

For more information about building and using Fusion applications, please consult the MapGuide documentation, and look at the item titled "Fusion Learning Materials for MapGuide".