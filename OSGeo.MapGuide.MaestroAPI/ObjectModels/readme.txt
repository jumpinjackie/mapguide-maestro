Running generate.bat will cause redundant generation of the following classes:

MapDefinition will have an invalid initialization statemtent on the BackgroundColor property. Simply remove it.

Also any schemas with ExtendedData elements will have to be changed. Anything attributed 
with XmlAnyElementAttribute is usually a BindingList<T> (the default list I've chosen for 
generating classes). BindingList<T> is not compatible for XML serialization, so these have
to be changed from:

BindingList<XmlElement>

to:

XmlElement[]

A simple search for XmlAnyElementAttribute will reveal the bits of code that need to change.


A special note about ObjectFactory
----------------------------------

Due to the fragile nature of serializing auto-generated classes and the need to avoid NullReferenceExceptions
at all costs, all constructors of all auto-generated classes are marked internal. External consumers of this
API will need to use ObjectFactory to create these classes. That way, ObjectFactory ensures that any
child object properties are properly initialized.