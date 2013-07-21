.. _scripting-and-automating-maestro:

Scripting and Automating Maestro
================================

.. note::

    The IronPython Console feature is only available on Windows and is only bundled with the Windows installer package of Maestro

As mentioned in the section (:ref:`user-interface`), Maestro includes a **IronPython Console** which provides the user an interactive scripting environment to drive and automate certain parts of the Maestro application with IronPython code

About IronPython
----------------

IronPython is an open source implementation of the Python programming language which is tightly integrated with the .net Framework. IronPython can use the .net Framework and Python libraries and also allows for .net applications to easily use Python code.

Maestro embeds an IronPython scripting engine allowing for easy scripting and automation of the application by users.

For more information about the .net specific details of IronPython, `click here <http://www.ironpython.net/documentation/dotnet/>`_

The Host Application
--------------------

The IronPython Console exposes a top-level global object named `app`, which represents the Maestro Host Application. The Host Application provides the following services:

 * Displaying messages/questions in a dialog box
 * Displaying file pickers for file selection
 * Obtaining `IServerConnection` references
 * Convenience methods to fetch/set resource XML content by their resource id
 * A UI-thread invoker for running UI interaction code from a background thread

The `IServerConnection` interface represents the top-level interface of the Maestro API and is the gateway into most of the functionality that is offered by the Maestro API.

Each root node (connection) in the **Site Explorer** represents an `IServerConnection` instance. The Host Application gives you the ability to retrieve such instances in order to be able to work with resources under that connection instance.

For more information about the `IServerConnection` interface, consult the Maestro API documentation which is included with the `Maestro SDK <http://trac.osgeo.org/mapguide/wiki/maestro/Downloads>`_

You can bring up documentation of any object or function with the `help` command. For example, to describe the structure of our global Host Application:

.. highlight:: Python
.. code-block:: Python

    >>> help(app)
    ?Help on HostApplication in module __builtin__
     |      A simplified helper class that is exposed to python scripts to provide
     |                  convenience functionality or to workaround concepts that don't cleanly
     |                  translate to IronPython (eg. Generics)
     |      
     |      HostApplication()
     |      
     |      
     |  Data and other attributes defined here:
     |  
     |      AskQuestion(...)
     |              AskQuestion(self: HostApplication, title: str, question: str) -> bool
     |              
     |                  Prompts for a question that requires a boolean response
     |              
     |      GetConnection(...)
     |              GetConnection(self: HostApplication, name: str) -> IServerConnection
     |              
     |                  Gets the connection by its specified name
     |              
     |      GetConnectionNames(...)
     |              GetConnectionNames(self: HostApplication) -> Array[str]
     |              
     |                  Returns a list of the names of all currently open connections
     |              
     |      GetResourceXml(...)
     |              GetResourceXml(self: HostApplication, conn: IServerConnection, resourceId: str) -> str
     |              
     |                  Gets the XML content of the given resource id
     |              
     |      OpenEditor(...)
     |              OpenEditor(self: HostApplication, conn: IServerConnection, resourceId: str)
     |                  Opens the default editor for the specified resource
     |              
     |      OpenUrl(...)
     |              OpenUrl(self: HostApplication, url: str)
     |                  Launches the specified url
     |              
     |      PickFolder(...)
     |              PickFolder(self: HostApplication, conn: IServerConnection) -> str
     |              
     |                  Prompts a dialog to select a folder
     |              
     |      PickResourceOpen(...)
     |              PickResourceOpen(self: HostApplication, conn: IServerConnection, resourceType: str) -> str
     |              
     |                  Displays a resource picker for opening
     |              
     |      PickResourceSave(...)
     |              PickResourceSave(self: HostApplication, conn: IServerConnection, resourceType: str) -> str
     |              
     |                  Displays a resource picker for saving
     |              
     |      PreviewResource(...)
     |              PreviewResource(self: HostApplication, conn: IServerConnection, resourceId: str, locale: str)
     |                  Launches a preview of the given open resource
     |              
     |      SetResourceXml(...)
     |              SetResourceXml(self: HostApplication, conn: IServerConnection, resourceId: str, xml: str)
     |                  Sets the XML content of the given resource id
     |              
     |      ShowError(...)
     |              ShowError(self: HostApplication, ex: Exception)
     |                  Displays an exception in a dialog
     |              
     |      ShowMessage(...)
     |              ShowMessage(self: HostApplication, title: str, message: str)
     |                  Displays a message
     |              
     |      UIInvoke(...)
     |              UIInvoke(self: HostApplication, method: Delegate)
     |                  Invokes the specified method on the UI thread. Methods that interact with the 
     |                   UI or create UI components
     |                          must be done on this thread
     |              
     |              
     |      __repr__(...)
     |              __repr__(self: object) -> str
     |              
    >>> 

Loading scripts
---------------

The IronPython console also supports loading pre-defined scripts. Click **Run File** and select the python script you want to load.

.. figure:: images/ironpython_runfile.png

All classes and functions defined in that python script will be loaded into the console's global namespace.

Example scripts
---------------

You can find some example scripts `here <http://trac.osgeo.org/mapguide/wiki/CodeSamples/Other/MaestroScripts>`_ which may give you some ideas as to what you can do through scripting and automation with IronPython