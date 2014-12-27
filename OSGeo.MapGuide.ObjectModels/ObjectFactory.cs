#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.PrintLayout;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Ldf110 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_1_0;
using Ldf120 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_2_0;
using Ldf130 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_3_0;
using Ldf230 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v2_3_0;
using Ldf240 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v2_4_0;
using Lp110 = OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_1_0;
using Lp220 = OSGeo.MapGuide.ObjectModels.LoadProcedure.v2_2_0;
using Mdf230 = OSGeo.MapGuide.ObjectModels.MapDefinition.v2_3_0;
using Mdf240 = OSGeo.MapGuide.ObjectModels.MapDefinition.v2_4_0;
using Sym110 = OSGeo.MapGuide.ObjectModels.SymbolDefinition.v1_1_0;
using Sym240 = OSGeo.MapGuide.ObjectModels.SymbolDefinition.v2_4_0;
using Wdf230 = OSGeo.MapGuide.ObjectModels.WatermarkDefinition.v2_3_0;
using Wdf240 = OSGeo.MapGuide.ObjectModels.WatermarkDefinition.v2_4_0;
using WL110 = OSGeo.MapGuide.ObjectModels.WebLayout.v1_1_0;
using WL240 = OSGeo.MapGuide.ObjectModels.WebLayout.v2_4_0;
using WL260 = OSGeo.MapGuide.ObjectModels.WebLayout.v2_6_0;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Factory method signature for creating layer definitions
    /// </summary>
    public delegate ILayerDefinition LayerCreatorFunc(LayerType type);

    /// <summary>
    /// Factory method signature for creating load procedures
    /// </summary>
    public delegate ILoadProcedure LoadProcCreatorFunc();

    /// <summary>
    /// Factory method signature for creating web layouts
    /// </summary>
    public delegate IWebLayout WebLayoutCreatorFunc(string mapDefinitionId);

    /// <summary>
    /// Factory method signature for creating compound symbol definitions
    /// </summary>
    public delegate ICompoundSymbolDefinition CompoundSymbolDefCreatorFunc();

    /// <summary>
    /// Factory method signature for creating simple symbol definitions
    /// </summary>
    public delegate ISimpleSymbolDefinition SimpleSymbolDefCreatorFunc();

    /// <summary>
    /// Factory method signature for creating watermarks
    /// </summary>
    /// <returns></returns>
    public delegate IWatermarkDefinition WatermarkCreatorFunc(SymbolDefinitionType type);

    /// <summary>
    /// Factory method signature for creating map definitions
    /// </summary>
    /// <returns></returns>
    public delegate IMapDefinition MapDefinitionCreatorFunc();

    /// <summary>
    /// Factory class to create MapGuide resource objects with either pre-defined or
    /// sensible default values. This is recommended over creating the objects directly
    /// as this ensures that there are no null child properties where the XML schema forbids
    /// it.
    ///
    /// By default this class will only create v1.0.0 of any specified top-level resource unless specified. In order to be able to create
    /// newer versions, you need to register the appropriate methods that can create resources of this version:
    ///
    ///  - <see cref="RegisterLayerFactoryMethod"/> for Layer Definitions
    ///
    /// This registration needs to be done as part of your application's startup/initialization phase.
    ///
    /// In the context of Maestro, this registration is automatically done as part of the addin's startup process
    /// </summary>
    public class ObjectFactory
    {
        private static Dictionary<Version, LayerCreatorFunc> _layerFactories;
        private static Dictionary<LoadType, LoadProcCreatorFunc> _loadProcFactories;
        private static Dictionary<Version, WebLayoutCreatorFunc> _wlFactories;
        private static Dictionary<Version, SimpleSymbolDefCreatorFunc> _simpleSymbolFactories;
        private static Dictionary<Version, CompoundSymbolDefCreatorFunc> _compoundSymbolFactories;
        private static Dictionary<Version, MapDefinitionCreatorFunc> _mapDefinitionFactories;
        private static Dictionary<Version, WatermarkCreatorFunc> _watermarkFactories;

        static ObjectFactory()
        {
            _layerFactories = new Dictionary<Version, LayerCreatorFunc>();
            _wlFactories = new Dictionary<Version, WebLayoutCreatorFunc>();
            _loadProcFactories = new Dictionary<LoadType, LoadProcCreatorFunc>();
            _simpleSymbolFactories = new Dictionary<Version, SimpleSymbolDefCreatorFunc>();
            _compoundSymbolFactories = new Dictionary<Version, CompoundSymbolDefCreatorFunc>();
            _mapDefinitionFactories = new Dictionary<Version, MapDefinitionCreatorFunc>();
            _watermarkFactories = new Dictionary<Version, WatermarkCreatorFunc>();

            Init();
        }

        public static void Reset()
        {
            _layerFactories.Clear();
            _wlFactories.Clear();
            _loadProcFactories.Clear();
            _simpleSymbolFactories.Clear();
            _compoundSymbolFactories.Clear();
            _mapDefinitionFactories.Clear();
            _watermarkFactories.Clear();
            ResourceTypeRegistry.Reset();
            Init();
        }

        private static void Init()
        {
            _layerFactories.Add(
                new Version(1, 0, 0),
                new LayerCreatorFunc(OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_0_0.LdfEntryPoint.CreateDefault));

            _loadProcFactories.Add(
                LoadType.Sdf,
                new LoadProcCreatorFunc(OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcEntryPoint.CreateDefaultSdf));
            _loadProcFactories.Add(
                LoadType.Shp,
                new LoadProcCreatorFunc(OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcEntryPoint.CreateDefaultShp));
            _loadProcFactories.Add(
                LoadType.Dwf,
                new LoadProcCreatorFunc(OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcEntryPoint.CreateDefaultDwf));

            _wlFactories.Add(
                new Version(1, 0, 0),
                new WebLayoutCreatorFunc(OSGeo.MapGuide.ObjectModels.WebLayout.v1_0_0.WebLayoutEntryPoint.CreateDefault));

            _compoundSymbolFactories.Add(
                new Version(1, 0, 0),
                new CompoundSymbolDefCreatorFunc(OSGeo.MapGuide.ObjectModels.SymbolDefinition.v1_0_0.CompoundSymbolDefinition.CreateDefault));

            _simpleSymbolFactories.Add(
                new Version(1, 0, 0),
                new SimpleSymbolDefCreatorFunc(OSGeo.MapGuide.ObjectModels.SymbolDefinition.v1_0_0.SimpleSymbolDefinition.CreateDefault));

            _mapDefinitionFactories.Add(
                new Version(1, 0, 0),
                new MapDefinitionCreatorFunc(OSGeo.MapGuide.ObjectModels.MapDefinition.v1_0_0.MdfEntryPoint.CreateDefault));

            //Layer Definition 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.1.0"),
                new ResourceSerializationCallback(Ldf110.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf110.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 1, 0), new LayerCreatorFunc(Ldf110.LdfEntryPoint.CreateDefault));

            //Layer Definition 1.2.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.2.0"),
                new ResourceSerializationCallback(Ldf120.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf120.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 2, 0), new LayerCreatorFunc(Ldf120.LdfEntryPoint.CreateDefault));

            //Layer Definition 1.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.3.0"),
                new ResourceSerializationCallback(Ldf130.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf130.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 3, 0), new LayerCreatorFunc(Ldf130.LdfEntryPoint.CreateDefault));

            //Layer Definition 2.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "2.3.0"),
                new ResourceSerializationCallback(Ldf230.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf230.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(2, 3, 0), new LayerCreatorFunc(Ldf230.LdfEntryPoint.CreateDefault));

            //Layer Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "2.4.0"),
                new ResourceSerializationCallback(Ldf240.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf240.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(2, 4, 0), new LayerCreatorFunc(Ldf240.LdfEntryPoint.CreateDefault));

            //Load Procedure 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "1.1.0"),
                new ResourceSerializationCallback(Lp110.LoadProcEntryPoint.Serialize),
                new ResourceDeserializationCallback(Lp110.LoadProcEntryPoint.Deserialize));

            //Load Procedure 1.1.0 schema offers nothing new for the ones we want to support, so nothing to register
            //with the ObjectFactory

            //Load Procedure 2.2.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "2.2.0"),
                new ResourceSerializationCallback(Lp220.LoadProcEntryPoint.Serialize),
                new ResourceDeserializationCallback(Lp220.LoadProcEntryPoint.Deserialize));
            ObjectFactory.RegisterLoadProcedureFactoryMethod(LoadType.Sqlite, new LoadProcCreatorFunc(Lp220.LoadProcEntryPoint.CreateDefaultSqlite));

            //Web Layout 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "1.1.0"),
                new ResourceSerializationCallback(WL110.WebLayoutEntryPoint.Serialize),
                new ResourceDeserializationCallback(WL110.WebLayoutEntryPoint.Deserialize));
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(1, 1, 0), new WebLayoutCreatorFunc(WL110.WebLayoutEntryPoint.CreateDefault));

            //Web Layout 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "2.4.0"),
                new ResourceSerializationCallback(WL240.WebLayoutEntryPoint.Serialize),
                new ResourceDeserializationCallback(WL240.WebLayoutEntryPoint.Deserialize));
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(2, 4, 0), new WebLayoutCreatorFunc(WL240.WebLayoutEntryPoint.CreateDefault));

            //Web Layout 2.6.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "2.6.0"),
                new ResourceSerializationCallback(WL260.WebLayoutEntryPoint.Serialize),
                new ResourceDeserializationCallback(WL260.WebLayoutEntryPoint.Deserialize));
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(2, 6, 0), new WebLayoutCreatorFunc(WL260.WebLayoutEntryPoint.CreateDefault));

            //Symbol Definition 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "1.1.0"),
                new ResourceSerializationCallback(Sym110.SymbolDefEntryPoint.Serialize),
                new ResourceDeserializationCallback(Sym110.SymbolDefEntryPoint.Deserialize));
            ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(1, 1, 0), new CompoundSymbolDefCreatorFunc(Sym110.SymbolDefEntryPoint.CreateDefaultCompound));
            ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(1, 1, 0), new SimpleSymbolDefCreatorFunc(Sym110.SymbolDefEntryPoint.CreateDefaultSimple));

            //Symbol Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "2.4.0"),
                new ResourceSerializationCallback(Sym240.SymbolDefEntryPoint.Serialize),
                new ResourceDeserializationCallback(Sym240.SymbolDefEntryPoint.Deserialize));
            ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(2, 4, 0), new CompoundSymbolDefCreatorFunc(Sym240.SymbolDefEntryPoint.CreateDefaultCompound));
            ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(2, 4, 0), new SimpleSymbolDefCreatorFunc(Sym240.SymbolDefEntryPoint.CreateDefaultSimple));

            //Map Definition 2.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "2.3.0"),
                new ResourceSerializationCallback(Mdf230.MdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Mdf230.MdfEntryPoint.Deserialize));
            ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(2, 3, 0), new MapDefinitionCreatorFunc(Mdf230.MdfEntryPoint.CreateDefault));

            //Map Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "2.4.0"),
                new ResourceSerializationCallback(Mdf240.MdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Mdf240.MdfEntryPoint.Deserialize));
            ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(2, 4, 0), new MapDefinitionCreatorFunc(Mdf240.MdfEntryPoint.CreateDefault));

            //Watermark Definition 2.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition.ToString(), "2.3.0"),
                new ResourceSerializationCallback(Wdf230.WdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Wdf230.WdfEntryPoint.Deserialize));
            ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(2, 3, 0), new WatermarkCreatorFunc(Wdf230.WdfEntryPoint.CreateDefault));

            //Watermark Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition.ToString(), "2.4.0"),
                new ResourceSerializationCallback(Wdf240.WdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Wdf240.WdfEntryPoint.Deserialize));
            ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(2, 4, 0), new WatermarkCreatorFunc(Wdf240.WdfEntryPoint.CreateDefault));
        }

        #region Factory registration

        /// <summary>
        /// Registers a resource serializer
        /// </summary>
        /// <param name="desc">The desc.</param>
        /// <param name="serializer">The serializer.</param>
        public static void RegisterResource(ResourceTypeDescriptor desc, ResourceSerializer serializer)
        {
            Check.ArgumentNotNull(desc, "desc");
            Check.ArgumentNotNull(serializer, "serializer");
            ResourceTypeRegistry.RegisterResource(desc, serializer);
        }

        /// <summary>
        /// Registers a resource serializer
        /// </summary>
        /// <param name="resourceType">The resource type descriptor.</param>
        /// <param name="serializeMethod">The serialize method.</param>
        /// <param name="deserializeMethod">The deserialize method.</param>
        public static void RegisterResourceSerializer(ResourceTypeDescriptor resourceType, ResourceSerializationCallback serializer, ResourceDeserializationCallback deserializer)
        {
            Check.ArgumentNotNull(resourceType, "resourceType");
            Check.ArgumentNotNull(serializer, "serializer");
            Check.ArgumentNotNull(deserializer, "deserializer");
            ResourceTypeRegistry.RegisterResource(resourceType, serializer, deserializer);
        }

        /// <summary>
        /// Registers the compound symbol factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="func"></param>
        public static void RegisterCompoundSymbolFactoryMethod(Version version, CompoundSymbolDefCreatorFunc func)
        {
            Check.ArgumentNotNull(version, "version");
            Check.ArgumentNotNull(func, "func");
            if (_compoundSymbolFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _compoundSymbolFactories[version] = func;
        }

        /// <summary>
        /// Regsiters the simple symbol factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="func"></param>
        public static void RegisterSimpleSymbolFactoryMethod(Version version, SimpleSymbolDefCreatorFunc func)
        {
            Check.ArgumentNotNull(version, "version");
            Check.ArgumentNotNull(func, "func");
            if (_simpleSymbolFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _simpleSymbolFactories[version] = func;
        }

        /// <summary>
        /// Registers the layer factory method.
        /// </summary>
        /// <param name="version">The ver.</param>
        /// <param name="method">The method.</param>
        public static void RegisterLayerFactoryMethod(Version version, LayerCreatorFunc method)
        {
            Check.ArgumentNotNull(version, "version");
            Check.ArgumentNotNull(method, "method");
            if (_layerFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _layerFactories[version] = method;
        }

        /// <summary>
        /// Registers the load procedure factory method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        public static void RegisterLoadProcedureFactoryMethod(LoadType type, LoadProcCreatorFunc method)
        {
            Check.ArgumentNotNull(method, "method");
            if (_loadProcFactories.ContainsKey(type))
                throw new ArgumentException(Strings.LoadProcFactoryMethodAlreadyRegistered + type);

            _loadProcFactories[type] = method;
        }

        /// <summary>
        /// Registers the web layout factory method.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="method">The method.</param>
        public static void RegisterWebLayoutFactoryMethod(Version version, WebLayoutCreatorFunc method)
        {
            Check.ArgumentNotNull(version, "version");
            Check.ArgumentNotNull(method, "method");
            if (_wlFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _wlFactories[version] = method;
        }

        /// <summary>
        /// Register the map definition factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="method"></param>
        public static void RegisterMapDefinitionFactoryMethod(Version version, MapDefinitionCreatorFunc method)
        {
            Check.ArgumentNotNull(version, "version");
            Check.ArgumentNotNull(method, "method");
            if (_mapDefinitionFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _mapDefinitionFactories[version] = method;
        }

        /// <summary>
        /// Registers the Watermark Definition factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="method"></param>
        public static void RegisterWatermarkDefinitionFactoryMethod(Version version, WatermarkCreatorFunc method)
        {
            Check.ArgumentNotNull(version, "version");
            Check.ArgumentNotNull(method, "method");
            if (_watermarkFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _watermarkFactories[version] = method;
        }

        #endregion Factory registration

        /// <summary>
        /// Creates an empty resource document header metadata object
        /// </summary>
        /// <returns></returns>
        public static ResourceDocumentHeaderTypeMetadata CreateMetadata()
        {
            return new ResourceDocumentHeaderTypeMetadata()
            {
                Simple = new ResourceDocumentHeaderTypeMetadataSimple()
                {
                    Property = new System.ComponentModel.BindingList<ResourceDocumentHeaderTypeMetadataSimpleProperty>()
                }
            };
        }

        /// <summary>
        /// Creates an empty group security object
        /// </summary>
        /// <returns></returns>
        public static ResourceSecurityTypeGroups CreateSecurityGroup()
        {
            return new ResourceSecurityTypeGroups()
            {
                Group = new System.ComponentModel.BindingList<ResourceSecurityTypeGroupsGroup>()
            };
        }

        /// <summary>
        /// Creates an empty user security object
        /// </summary>
        /// <returns></returns>
        public static ResourceSecurityTypeUsers CreateSecurityUser()
        {
            return new ResourceSecurityTypeUsers()
            {
                User = new System.ComponentModel.BindingList<ResourceSecurityTypeUsersUser>()
            };
        }

        /// <summary>
        /// Creates the feature source extension.
        /// </summary>
        /// <returns></returns>
        public static IFeatureSourceExtension CreateFeatureSourceExtension(string name, string featureClass)
        {
            return new OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.FeatureSourceTypeExtension()
            {
                Name = name,
                FeatureClass = featureClass,
                CalculatedProperty = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.CalculatedPropertyType>(),
                AttributeRelate = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.AttributeRelateType>()
            };
        }

        /// <summary>
        /// Creates the feature source extension.
        /// </summary>
        /// <returns></returns>
        public static IFeatureSourceExtension CreateFeatureSourceExtension()
        {
            return new OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.FeatureSourceTypeExtension()
            {
                CalculatedProperty = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.CalculatedPropertyType>(),
                AttributeRelate = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.AttributeRelateType>()
            };
        }

        /// <summary>
        /// Creates the calculated property.
        /// </summary>
        /// <returns></returns>
        public static ICalculatedProperty CreateCalculatedProperty()
        {
            return new OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.CalculatedPropertyType()
            {
            };
        }

        /// <summary>
        /// Creates the calculated property.
        /// </summary>
        /// <param name="name">The name of the calculated property</param>
        /// <param name="expression">The FDO Expression</param>
        /// <returns></returns>
        public static ICalculatedProperty CreateCalculatedProperty(string name, string expression)
        {
            return new OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.CalculatedPropertyType()
            {
                Name = name,
                Expression = expression
            };
        }

        /// <summary>
        /// Creates the attribute relation.
        /// </summary>
        /// <returns></returns>Properties.Resources.
        public static IAttributeRelation CreateAttributeRelation()
        {
            IAttributeRelation rel = new OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.AttributeRelateType()
            {
                RelateProperty = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.RelatePropertyType>(),
            };
            rel.RelateType = RelateTypeEnum.LeftOuter;
            rel.ForceOneToOne = false;
            return rel;
        }

        /// <summary>
        /// Creates an envelope (bounding box)
        /// </summary>
        /// <param name="minx"></param>
        /// <param name="miny"></param>
        /// <param name="maxx"></param>
        /// <param name="maxy"></param>
        /// <returns></returns>
        public static IEnvelope CreateEnvelope(double minx, double miny, double maxx, double maxy)
        {
            if (minx > maxx)
                throw new ArgumentException("minx > maxx", "minx"); //NOXLATE

            if (miny > maxy)
                throw new ArgumentException("miny > maxy", "miny"); //NOXLATE

            return new Envelope()
            {
                LowerLeftCoordinate = new EnvelopeLowerLeftCoordinate()
                {
                    X = minx,
                    Y = miny
                },
                UpperRightCoordinate = new EnvelopeUpperRightCoordinate()
                {
                    X = maxx,
                    Y = maxy
                }
            };
        }

        /// <summary>
        /// Creates the default layer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static ILayerDefinition CreateDefaultLayer(LayerType type, Version version)
        {
            if (!_layerFactories.ContainsKey(version))
                throw new ArgumentException(Strings.UnknownLayerVersion + version.ToString());

            var layer = _layerFactories[version](type);

            return layer;
        }

        /// <summary>
        /// Creates the drawing source.
        /// </summary>
        /// <returns></returns>
        public static IDrawingSource CreateDrawingSource()
        {
            return new OSGeo.MapGuide.ObjectModels.DrawingSource.v1_0_0.DrawingSource()
            {
                SourceName = string.Empty,
                CoordinateSpace = string.Empty,
                Sheet = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.DrawingSource.v1_0_0.DrawingSourceSheet>()
            };
        }

        /// <summary>
        /// Creates the feature source.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static IFeatureSource CreateFeatureSource(string provider)
        {
            return new OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.FeatureSourceType()
            {
                Provider = provider,
                Parameter = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0.NameValuePairType>()
            };
        }

        /// <summary>
        /// Creates the feature source.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="values">The connection properties.</param>
        /// <returns></returns>
        public static IFeatureSource CreateFeatureSource(string provider, NameValueCollection values)
        {
            var fs = CreateFeatureSource(provider);
            fs.ApplyConnectionProperties(values);

            return fs;
        }

        /// <summary>
        /// Creates a Watermark Definition
        /// </summary>
        /// <param name="type"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static IWatermarkDefinition CreateWatermark(SymbolDefinitionType type, Version version)
        {
            if (!_watermarkFactories.ContainsKey(version))
                throw new ArgumentException(Strings.UnknownWatermarkDefinitionVersion + version.ToString());

            var wdf = _watermarkFactories[version](type);
            return wdf;
        }

        /// <summary>
        /// Creates the load procedure.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fileNames">The file names.</param>
        /// <returns></returns>
        public static ILoadProcedure CreateLoadProcedure(LoadType type, IEnumerable<string> fileNames)
        {
            var proc = CreateLoadProcedure(type);
            var fproc = proc.SubType as IBaseLoadProcedure;
            if (fproc != null)
            {
                if (fileNames != null)
                {
                    foreach (var f in fileNames)
                    {
                        fproc.SourceFile.Add(f);
                    }
                }
            }
            return proc;
        }

        /// <summary>
        /// Creates the load procedure.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ILoadProcedure CreateLoadProcedure(LoadType type)
        {
            if (type == LoadType.Dwg || type == LoadType.Raster)
                throw new NotSupportedException(Strings.UnsupportedLoadProcedureType);

            if (_loadProcFactories.ContainsKey(type))
            {
                var proc = _loadProcFactories[type]();
                return proc;
            }

            throw new InvalidOperationException(Strings.CannotCreateLoadProcedureSubType + type);
        }

        /// <summary>
        /// Creates the map definition.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(Version version, string name)
        {
            if (!_mapDefinitionFactories.ContainsKey(version))
                throw new ArgumentException(Strings.UnknownMapDefinitionVersion + version.ToString());

            var mdf = _mapDefinitionFactories[version]();
            mdf.Name = name;
            return mdf;
        }

        /// <summary>
        /// Creates the map definition.
        /// </summary>
        /// <param name="ver"></param>
        /// <param name="name"></param>
        /// <param name="coordinateSystemWkt"></param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(Version ver, string name, string coordinateSystemWkt)
        {
            var map = CreateMapDefinition(ver, name);
            map.CoordinateSystem = coordinateSystemWkt;

            return map;
        }

        /// <summary>
        /// Creates the map definition.
        /// </summary>
        /// <param name="ver"></param>
        /// <param name="name"></param>
        /// <param name="coordinateSystemWkt"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(Version ver, string name, string coordinateSystemWkt, IEnvelope env)
        {
            var map = CreateMapDefinition(ver, name, coordinateSystemWkt);
            map.Extents = env;

            return map;
        }

        /// <summary>
        /// Creates the web layout.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="mapDefinitionId">The map definition id.</param>
        /// <returns></returns>
        public static IWebLayout CreateWebLayout(Version version, string mapDefinitionId)
        {
            if (!_wlFactories.ContainsKey(version))
                throw new ArgumentException(Strings.UnknownWebLayoutVersion + version.ToString());

            var wl = _wlFactories[version](mapDefinitionId);

            return wl;
        }

        /// <summary>
        /// Creates a simple label symbol
        /// </summary>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ISimpleSymbolDefinition CreateSimpleLabel(Version version, GeometryContextType type)
        {
            var sym = CreateSimpleSymbol(version, "MTEXT", "Default MTEXT Symbol");
            var text = sym.CreateTextGraphics();

            bool bSupportsAdvancedTypes = !(version.Major == 1 && version.Minor == 0 && version.Build == 0);

            text.Content = "%CONTENT%";
            text.FontName = "%FONTNAME%";
            text.Bold = "%BOLD%";
            text.Italic = "%ITALIC%";
            text.Underlined = "%UNDERLINED%";
            var text2 = text as ITextGraphic2;
            if (text2 != null)
            {
                text2.Overlined = "%OVERLINED%";
            }
            text.Height = "%FONTHEIGHT%";
            text.PositionX = "%StyleEditorGenerated_TextPositionX_0%";
            text.PositionY = "%StyleEditorGenerated_TextPositionY_0%";
            text.HorizontalAlignment = "%HORIZONTALALIGNMENT%";
            text.VerticalAlignment = "%VERTICALALIGNMENT%";
            text.Justification = "%JUSTIFICATION%";
            text.LineSpacing = "%LINESPACING%";
            text.TextColor = "%TEXTCOLOR%";
            text.GhostColor = "%GHOSTCOLOR%";
            var frame = sym.CreateFrame();
            frame.LineColor = "%FRAMELINECOLOR%";
            frame.FillColor = "%FRAMEFILLCOLOR%";
            frame.OffsetX = 0.0;
            frame.OffsetY = 0.0;
            text.Frame = frame;
            if (text2 != null)
            {
                text2.Markup = "'MTEXT'";
            }

            sym.AddGraphics(text);
            switch (type)
            {
                case GeometryContextType.LineString:
                    {
                        var usage = sym.CreateLineUsage();
                        usage.AngleControl = "'FromAngle'";
                        usage.Angle = "%ROTATION%";
                        usage.Repeat = "1.0";
                        sym.LineUsage = usage;
                    }
                    break;

                case GeometryContextType.Polygon:
                    {
                        var usage = sym.CreateAreaUsage();
                        usage.Angle = "%ROTATION%";
                        usage.RepeatX = "100.0";
                        usage.RepeatY = "100.0";
                        sym.AreaUsage = usage;
                    }
                    break;

                case GeometryContextType.Point:
                    {
                        var usage = sym.CreatePointUsage();
                        usage.Angle = "%ANGLE%";
                        sym.PointUsage = usage;
                    }
                    break;
            }

            sym.DefineParameter("CONTENT", "'text'", "T&amp;ext", "Text", bSupportsAdvancedTypes ? "Content" : "String");
            sym.DefineParameter("FONTNAME", "'Arial'", "&amp;Font Name", "Font Name", bSupportsAdvancedTypes ? "FontName" : "String");
            sym.DefineParameter("FONTHEIGHT", "4.0", "Font &amp;Size", "Font Size", bSupportsAdvancedTypes ? "FontHeight" : "Real");
            sym.DefineParameter("BOLD", "false", "Bold", "Bold", bSupportsAdvancedTypes ? "Bold" : "Boolean");
            sym.DefineParameter("ITALIC", "false", "Italic", "Italic", bSupportsAdvancedTypes ? "Italic" : "Boolean");
            sym.DefineParameter("UNDERLINED", "false", "Underlined", "Underlined", bSupportsAdvancedTypes ? "Underlined" : "Boolean");
            if (text2 != null)
            {
                sym.DefineParameter("OVERLINED", "false", "Overlined", "Overlined", bSupportsAdvancedTypes ? "Overlined" : "Boolean");
            }
            sym.DefineParameter("JUSTIFICATION", "'FromAlignment'", "Justification", "Justification", bSupportsAdvancedTypes ? "Justification" : "String");
            sym.DefineParameter("LINESPACING", "1.05", "Line Spacing", "Line Spacing", bSupportsAdvancedTypes ? "LineSpacing" : "Real");
            sym.DefineParameter("GHOSTCOLOR", "", "Ghost Color", "Ghost Color", bSupportsAdvancedTypes ? "GhostColor" : "Color");
            sym.DefineParameter("FRAMELINECOLOR", "", "Frame Line Color", "Frame Line Color", bSupportsAdvancedTypes ? "FrameLineColor" : "Color");
            sym.DefineParameter("FRAMEFILLCOLOR", "", "Frame Fill Color", "Frame Fill Color", bSupportsAdvancedTypes ? "FrameFillColor" : "Color");
            sym.DefineParameter("TEXTCOLOR", "0xff000000", "Text Color", "Text Color", bSupportsAdvancedTypes ? "TextColor" : "Color");
            sym.DefineParameter("VERTICALALIGNMENT", "'Halfline'", "&amp;Vertical Alignment", "Vertical Alignment", bSupportsAdvancedTypes ? "VerticalAlignment" : "String");
            sym.DefineParameter("ROTATION", "0.0", "&amp;Rotation", "Rotation", bSupportsAdvancedTypes ? "Angle" : "Real");
            sym.DefineParameter("HORIZONTALALIGNMENT", "'Center'", "Hori&amp;zontal Alignment", "Horizontal Alignment", bSupportsAdvancedTypes ? "HorizontalAlignment" : "String");
            sym.DefineParameter("StyleEditorGenerated_TextPositionX_0", "0.0", "PositionX", "PositionX", "Real");
            sym.DefineParameter("StyleEditorGenerated_TextPositionY_0", "0.0", "PositionY", "PositionY", "Real");

            return sym;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static ISimpleSymbolDefinition CreateSimplePoint(Version version)
        {
            var sym = CreateSimpleSymbol(version, "Square", "Default Point Symbol");
            var path = sym.CreatePathGraphics();

            bool bSupportsAdvancedTypes = !(version.Major == 1 && version.Minor == 0 && version.Build == 0);

            path.Geometry = "M -1.0,-1.0 L 1.0,-1.0 L 1.0,1.0 L -1.0,1.0 L -1.0,-1.0";
            IPathGraphic2 path2 = path as IPathGraphic2;
            if (path2 != null)
            {
                path2.ScaleX = "%StyleEditorGenerated_ScaleX_0%";
                path2.ScaleY = "%StyleEditorGenerated_ScaleY_0%";
            }
            path.FillColor = "%FILLCOLOR%";
            path.LineColor = "%LINECOLOR%";
            path.LineWeight = "%LINEWEIGHT%";
            path.LineCap = "%StyleEditorGenerated_LineCap_0%";
            path.LineJoin = "%StyleEditorGenerated_LineJoin_0%";

            var usage = sym.CreatePointUsage();
            usage.Angle = "%ROTATION%";
            sym.PointUsage = usage;

            sym.DefineParameter("FILLCOLOR", "0xffffffff", "&amp;Fill Color", "Fill Color", bSupportsAdvancedTypes ? "FillColor" : "Color");
            sym.DefineParameter("LINECOLOR", "0xff000000", "Line &amp;Color", "Line Color", bSupportsAdvancedTypes ? "LineColor" : "Color");
            sym.DefineParameter("LINEWEIGHT", "0.0", "Line &amp;Thickness", "Line Thickness", bSupportsAdvancedTypes ? "LineWeight" : "Real");
            sym.DefineParameter("ROTATION", "0.0", "&amp;Rotation", "Rotation", bSupportsAdvancedTypes ? "Angle" : "Real");
            if (path2 != null)
            {
                sym.DefineParameter("StyleEditorGenerated_ScaleX_0", "1.0", "Path ScaleX", "Path ScaleX", "Real");
                sym.DefineParameter("StyleEditorGenerated_ScaleY_0", "1.0", "Path ScaleY", "Path ScaleY", "Real");
            }
            sym.DefineParameter("StyleEditorGenerated_LineCap_0", "'Round'", "Line Cap", "The cap type to use at the ends of each segment in the path outline.  This must evaluate to one of: None, Round (default), Triangle, or Square.", "String");
            sym.DefineParameter("StyleEditorGenerated_LineJoin_0", "'Round'", "Line Join", "The join type to use at each vertex in the path outline.  This must evaluate to one of: None, Bevel, Round (default), or Miter.", "String");
            return sym;
        }

        /// <summary>
        /// Creates a simple solid line symbol
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static ISimpleSymbolDefinition CreateSimpleSolidLine(Version version)
        {
            var sym = CreateSimpleSymbol(version, "Solid Line", "Default Line Symbol");
            var path = sym.CreatePathGraphics();

            bool bSupportsAdvancedTypes = !(version.Major == 1 && version.Minor == 0 && version.Build == 0);

            path.Geometry = "M 0.0,0.0 L 1.0,0.0";
            path.LineColor = "%LINECOLOR%";
            path.LineWeight = "%LINEWEIGHT%";
            path.LineCap = "%StyleEditorGenerated_LineCap_0%";
            path.LineJoin = "%StyleEditorGenerated_LineJoin_0%";
            IPathGraphic2 path2 = path as IPathGraphic2;
            if (path2 != null)
            {
                path2.ScaleX = "%StyleEditorGenerated_ScaleX_0%";
                path2.ScaleY = "%StyleEditorGenerated_ScaleY_0%";
            }
            sym.AddGraphics(path);
            var lineUsage = sym.CreateLineUsage();
            lineUsage.Repeat = "1.0";
            sym.LineUsage = lineUsage;

            sym.DefineParameter("LINECOLOR", "0xff000000", "Line &amp;Color", "Line Color", bSupportsAdvancedTypes ? "LineColor" : "Color");
            sym.DefineParameter("LINEWEIGHT", "0.0", "Line &amp;Thickness", "Line Thickness", bSupportsAdvancedTypes ? "LineWeight" : "Real");
            if (path2 != null)
            {
                sym.DefineParameter("StyleEditorGenerated_ScaleX_0", "1.0", "Path ScaleX", "Path ScaleX", "Real");
                sym.DefineParameter("StyleEditorGenerated_ScaleY_0", "1.0", "Path ScaleY", "Path ScaleY", "Real");
            }
            sym.DefineParameter("StyleEditorGenerated_LineCap_0", "'Round'", "Line Cap", "The cap type to use at the ends of each segment in the path outline.  This must evaluate to one of: None, Round (default), Triangle, or Square.", "String");
            sym.DefineParameter("StyleEditorGenerated_LineJoin_0", "'Round'", "Line Join", "The join type to use at each vertex in the path outline.  This must evaluate to one of: None, Bevel, Round (default), or Miter.", "String");
            return sym;
        }

        /// <summary>
        /// Creates a simple solid fill symbol
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static ISimpleSymbolDefinition CreateSimpleSolidFill(Version version)
        {
            var sym = CreateSimpleSymbol(version, "Solid Fill", "Default Area Symbol");
            var path = sym.CreatePathGraphics();

            bool bSupportsAdvancedTypes = !(version.Major == 1 && version.Minor == 0 && version.Build == 0);

            path.Geometry = "M 0.0,0.0 h 100.0 v 100.0 h -100.0 z";
            path.FillColor = "%FILLCOLOR%";
            path.LineCap = "%StyleEditorGenerated_LineCap_0%";
            path.LineJoin = "%StyleEditorGenerated_LineJoin_0%";
            IPathGraphic2 path2 = path as IPathGraphic2;
            if (path2 != null)
            {
                path2.ScaleX = "%StyleEditorGenerated_ScaleX_0%";
                path2.ScaleY = "%StyleEditorGenerated_ScaleY_0%";
            }
            sym.AddGraphics(path);
            var areaUsage = sym.CreateAreaUsage();
            areaUsage.RepeatX = "100.0";
            areaUsage.RepeatY = "100.0";
            sym.AreaUsage = areaUsage;

            sym.DefineParameter("FILLCOLOR", "0xffbfbfbf", "&amp;Fill Color", "Fill Color", bSupportsAdvancedTypes ? "FillColor" : "Color");
            if (path2 != null)
            {
                sym.DefineParameter("StyleEditorGenerated_ScaleX_0", "1.0", "Path ScaleX", "Path ScaleX", "Real");
                sym.DefineParameter("StyleEditorGenerated_ScaleY_0", "1.0", "Path ScaleY", "Path ScaleY", "Real");
            }
            sym.DefineParameter("StyleEditorGenerated_LineCap_0", "'Round'", "Line Cap", "The cap type to use at the ends of each segment in the path outline.  This must evaluate to one of: None, Round (default), Triangle, or Square.", "String");
            sym.DefineParameter("StyleEditorGenerated_LineJoin_0", "'Round'", "Line Join", "The join type to use at each vertex in the path outline.  This must evaluate to one of: None, Bevel, Round (default), or Miter.", "String");
            return sym;
        }

        /// <summary>
        /// Creates a simple symbol definition.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static ISimpleSymbolDefinition CreateSimpleSymbol(Version version, string name, string description)
        {
            if (!_simpleSymbolFactories.ContainsKey(version))
                throw new ArgumentException(Strings.UnknownSymbolDefVersion + version.ToString());

            var simp = _simpleSymbolFactories[version]();
            simp.Name = name;
            simp.Description = description;
            return simp;
        }

        /// <summary>
        /// Creates the compound symbol.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static ICompoundSymbolDefinition CreateCompoundSymbol(Version version, string name, string description)
        {
            if (!_compoundSymbolFactories.ContainsKey(version))
                throw new ArgumentException(Strings.UnknownSymbolDefVersion + version.ToString());

            var comp = _compoundSymbolFactories[version]();
            comp.Name = name;
            comp.Description = description;
            return comp;
        }

        /// <summary>
        /// Creates the print layout.
        /// </summary>
        /// <returns></returns>
        public static IPrintLayout CreatePrintLayout()
        {
            return new OSGeo.MapGuide.ObjectModels.PrintLayout.v1_0_0.PrintLayout()
            {
                CustomLogos = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.PrintLayout.v1_0_0.PrintLayoutLogo>(),
                CustomText = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.PrintLayout.v1_0_0.PrintLayoutText>(),
                LayoutProperties = new OSGeo.MapGuide.ObjectModels.PrintLayout.v1_0_0.PrintLayoutLayoutProperties()
                {
                    ShowCustomLogos = false,
                    ShowCustomLogosSpecified = true,
                    ShowCustomText = false,
                    ShowCustomTextSpecified = true,
                    ShowDateTime = false,
                    ShowDateTimeSpecified = true,
                    ShowLegend = false,
                    ShowLegendSpecified = true,
                    ShowNorthArrow = false,
                    ShowNorthArrowSpecified = true,
                    ShowScaleBar = false,
                    ShowScaleBarSpecified = true,
                    ShowTitle = false,
                    ShowTitleSpecified = true,
                    ShowURL = false,
                    ShowURLSpecified = true
                },
                PageProperties = new OSGeo.MapGuide.ObjectModels.PrintLayout.v1_0_0.PrintLayoutPageProperties()
                {
                    BackgroundColor = new OSGeo.MapGuide.ObjectModels.PrintLayout.v1_0_0.PrintLayoutPagePropertiesBackgroundColor()
                    {
                    }
                },
            };
        }

        /// <summary>
        /// Creates a 2d point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IPoint2D CreatePoint2D(double x, double y)
        {
            return new Point2DImpl() { X = x, Y = y };
        }

        /// <summary>
        /// Creates a 3d point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static IPoint3D CreatePoint3D(double x, double y, double z)
        {
            return new Point3DImpl() { X = x, Y = y, Z = z };
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static IResource DeserializeResourceXml(string xml)
        {
            return ResourceTypeRegistry.Deserialize(xml);
        }

        /// <summary>
        /// Serializes the specified resource.
        /// </summary>
        /// <param name="res">The resource.</param>
        /// <returns></returns>
        public static Stream SerializeResource(IResource resource)
        {
            return ResourceTypeRegistry.Serialize(resource);
        }

        /// <summary>
        /// Serializes the specified resource.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static string SerializeResourceAsString(IResource resource)
        {
            return ResourceTypeRegistry.SerializeAsString(resource);
        }

        /// <summary>
        /// Deserializes the specified stream for the specified resource type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IResource DeserializeResourceStream(string resourceType, Stream stream)
        {
            return ResourceTypeRegistry.Deserialize(resourceType, stream);
        }
    }
}