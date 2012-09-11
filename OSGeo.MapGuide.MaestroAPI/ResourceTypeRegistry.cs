#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.PrintLayout;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolLibrary;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.ObjectModels;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// 
    /// </summary>
    public delegate IResource ResourceDeserializationCallback(string xml);
    /// <summary>
    /// 
    /// </summary>
    public delegate Stream ResourceSerializationCallback(IResource res);

    /// <summary>
    /// A resource serializer
    /// </summary>
    public class ResourceSerializer
    {
        /// <summary>
        /// Gets or sets the serialize method.
        /// </summary>
        /// <value>The serialize method.</value>
        public ResourceSerializationCallback Serialize { get; set; }

        /// <summary>
        /// Gets or sets the deserialize method.
        /// </summary>
        /// <value>The deserialize method.</value>
        public ResourceDeserializationCallback Deserialize { get; set; }
    }

    //TODO: Expand on documentation as this is an important class

    /// <summary>
    /// A registry of serializers allowing automatic serialization/deserialization of any XML
    /// content based on its version and resource type.
    /// </summary>
    public static class ResourceTypeRegistry
    {
        private static Dictionary<ResourceTypeDescriptor, ResourceSerializer> _serializers;

        static ResourceTypeRegistry()
        {
            _serializers = new Dictionary<ResourceTypeDescriptor, ResourceSerializer>();

            //ApplicationDefinition 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.ApplicationDefinition,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.ApplicationDefinition_1_0_0.ApplicationDefinitionType.Deserialize(xml); }
                });

            //DrawingSource 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.DrawingSource,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.DrawingSource_1_0_0.DrawingSource.Deserialize(xml); }
                });

            //FeatureSource 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.FeatureSource,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.FeatureSourceType.Deserialize(xml); }
                });

            //LayerDefinition 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.LayerDefinition,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.LayerDefinition_1_0_0.LayerDefinition.Deserialize(xml); }
                });

            //LoadProcedure 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.LoadProcedure,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.LoadProcedure.Deserialize(xml); }
                });

            //MapDefinition 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.MapDefinition,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return OSGeo.MapGuide.ObjectModels.MapDefinition_1_0_0.MdfEntryPoint.Serialize(res); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.MapDefinition_1_0_0.MdfEntryPoint.Deserialize(xml); }
                });

            //PrintLayout 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.PrintLayout,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0.PrintLayout.Deserialize(xml); }
                });

            //SymbolDefinition 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.SymbolDefinition,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => 
                    { 
                        //HACK: We have to peek at the XML to determine if this is simple or compound.
                        var doc = new XmlDocument();
                        doc.LoadXml(xml);
                        if (doc.DocumentElement.Name == "SimpleSymbolDefinition") //NOXLATE
                        {
                            return OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.SimpleSymbolDefinition.Deserialize(xml);
                        }
                        else
                        {
                            if (doc.DocumentElement.Name == "CompoundSymbolDefinition") //NOXLATE
                                return OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.CompoundSymbolDefinition.Deserialize(xml);
                            else //WTF?
                                throw new SerializationException(Strings.ErrorCouldNotDetermineSymbolType);
                        }
                    }
                });

            //SymbolLibrary 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.SymbolLibrary,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.SymbolLibrary_1_0_0.SymbolLibraryType.Deserialize(xml); }
                });

            //WebLayout 1.0.0
            _serializers.Add(
                ResourceTypeDescriptor.WebLayout,
                new ResourceSerializer()
                {
                    Serialize = (res) => { return res.SerializeToStream(); },
                    Deserialize = (xml) => { return OSGeo.MapGuide.ObjectModels.WebLayout_1_0_0.WebLayoutType.Deserialize(xml); }
                });
        }

        /// <summary>
        /// Registers a resource serializer
        /// </summary>
        /// <param name="desc">The desc.</param>
        /// <param name="serializer">The serializer.</param>
        public static void RegisterResource(ResourceTypeDescriptor desc, ResourceSerializer serializer)
        {
            if (_serializers.ContainsKey(desc))
                throw new ArgumentException(string.Format(Strings.ERR_SERIALIZER_ALREADY_REGISTERED, desc.ResourceType, desc.Version), "desc"); //NOXLATE

            _serializers.Add(desc, serializer);
        }

        /// <summary>
        /// Registers a resource serializer
        /// </summary>
        /// <param name="desc">The desc.</param>
        /// <param name="serializeMethod">The serialize method.</param>
        /// <param name="deserializeMethod">The deserialize method.</param>
        public static void RegisterResource(ResourceTypeDescriptor desc, ResourceSerializationCallback serializeMethod, ResourceDeserializationCallback deserializeMethod)
        {
            RegisterResource(desc, new ResourceSerializer() { Deserialize = deserializeMethod, Serialize = serializeMethod });
        }

        internal static void Init() 
        { 
            //does nothing, it's just for kicking the static constructor into gear 
        }

        /// <summary>
        /// Deserializes the specified stream for the specified resource type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IResource Deserialize(ResourceTypes resourceType, Stream stream)
        {
            //UGLY: We have to peek inside the stream to determine the version number
            
            //House the stream inside a rewindable memory stream
            using (var ms = new MemoryStream())
            {
                Utility.CopyStream(stream, ms);
                ms.Position = 0L; //Rewind

                var rd = ResourceContentVersionChecker.GetVersionFromXmlStream(ms);
                Debug.Assert(rd.ResourceType.Equals(resourceType.ToString()));

                ms.Position = 0L; //Rewind

                using (var reader = new StreamReader(ms))
                {
                    var xml = reader.ReadToEnd();
                    if (_serializers.ContainsKey(rd))
                        return _serializers[rd].Deserialize(xml);
                    else
                        return new UntypedResource(xml, resourceType, rd.Version);
                }
            }
        }

        /// <summary>
        /// Serializes the specified resource.
        /// </summary>
        /// <param name="res">The resource.</param>
        /// <returns></returns>
        public static Stream Serialize(IResource res)
        {
            var rd = res.GetResourceTypeDescriptor();
            if (!_serializers.ContainsKey(rd))
            {
                var utr = res as UntypedResource;
                if (utr == null)
                    throw new SerializationException(Strings.ERR_NO_SERIALIZER + rd.ToString());
                return utr.SerializeToStream();
            }

            return _serializers[rd].Serialize(res);
        }

        /// <summary>
        /// Serializes the specified resource.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static string SerializeAsString(IResource res)
        {
            using (var stream = Serialize(res))
            {
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static IResource Deserialize(string xml)
        {
            var checker = new ResourceContentVersionChecker(xml);
            var rd = checker.GetVersion();
            if (rd == null)
                throw new SerializationException(Strings.ERR_NOT_RESOURCE_CONTENT_XML);

            if (!_serializers.ContainsKey(rd))
            {
                var rtype = (ResourceTypes)Enum.Parse(typeof(ResourceTypes), rd.ResourceType);
                return new UntypedResource(xml, rtype, rd.Version);
                //throw new SerializationException(Strings.ERR_NO_SERIALIZER + rd.ToString());
            }

            return _serializers[rd].Deserialize(xml);
        }
    }
}
