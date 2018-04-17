#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition.v1_0_0;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.PrintLayout;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolLibrary;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Ldf110 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_1_0;
using Ldf120 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_2_0;
using Ldf130 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_3_0;
using Ldf230 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v2_3_0;
using Ldf240 = OSGeo.MapGuide.ObjectModels.LayerDefinition.v2_4_0;
using Lp110 = OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_1_0;
using Lp220 = OSGeo.MapGuide.ObjectModels.LoadProcedure.v2_2_0;
using Mdf230 = OSGeo.MapGuide.ObjectModels.MapDefinition.v2_3_0;
using Mdf240 = OSGeo.MapGuide.ObjectModels.MapDefinition.v2_4_0;
using Mdf300 = OSGeo.MapGuide.ObjectModels.MapDefinition.v3_0_0;
using Sym110 = OSGeo.MapGuide.ObjectModels.SymbolDefinition.v1_1_0;
using Sym240 = OSGeo.MapGuide.ObjectModels.SymbolDefinition.v2_4_0;
using Tsd300 = OSGeo.MapGuide.ObjectModels.TileSetDefinition.v3_0_0;
using Wdf230 = OSGeo.MapGuide.ObjectModels.WatermarkDefinition.v2_3_0;
using Wdf240 = OSGeo.MapGuide.ObjectModels.WatermarkDefinition.v2_4_0;
using WL110 = OSGeo.MapGuide.ObjectModels.WebLayout.v1_1_0;
using WL240 = OSGeo.MapGuide.ObjectModels.WebLayout.v2_4_0;
using WL260 = OSGeo.MapGuide.ObjectModels.WebLayout.v2_6_0;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Factory class to create MapGuide resource objects with either pre-defined or
    /// sensible default values. This is recommended over creating the objects directly
    /// as this ensures that there are no null child properties where the XML schema forbids
    /// it.
    ///
    /// By default this class will only create known versions of any top-level resource (as of writing, up to MapGuide
    /// Open Source 2.6 / AIMS 2015). In order to be able to create newer versions where support has not yet been added to
    /// this library, you will need to register the appropriate methods that can create resources of this version:
    ///
    ///  - <see cref="RegisterLayerFactoryMethod"/> for Layer Definitions
    ///
    /// This registration needs to be done as part of your application's startup/initialization phase.
    /// </summary>
    public static class ObjectFactory
    {
        private static Dictionary<Version, Func<LayerType, ILayerDefinition>> _layerFactories;
        private static Dictionary<LoadType, Func<ILoadProcedure>> _loadProcFactories;
        private static Dictionary<Version, Func<string, IWebLayout>> _wlFactories;
        private static Dictionary<Version, Func<ISimpleSymbolDefinition>> _simpleSymbolFactories;
        private static Dictionary<Version, Func<ICompoundSymbolDefinition>> _compoundSymbolFactories;
        private static Dictionary<Version, Func<IMapDefinition>> _mapDefinitionFactories;
        private static Dictionary<Version, Func<SymbolDefinitionType, IWatermarkDefinition>> _watermarkFactories;
        private static Dictionary<Version, Func<ITileSetDefinition>> _tileSetDefinitionFactories;

        static ObjectFactory()
        {
            _layerFactories = new Dictionary<Version,Func<LayerType,ILayerDefinition>>();
            _wlFactories = new Dictionary<Version,Func<string,IWebLayout>>();
            _loadProcFactories = new Dictionary<LoadType,Func<ILoadProcedure>>();
            _simpleSymbolFactories = new Dictionary<Version,Func<ISimpleSymbolDefinition>>();
            _compoundSymbolFactories = new Dictionary<Version,Func<ICompoundSymbolDefinition>>();
            _mapDefinitionFactories = new Dictionary<Version,Func<IMapDefinition>>();
            _watermarkFactories = new Dictionary<Version,Func<SymbolDefinitionType,IWatermarkDefinition>>();
            _tileSetDefinitionFactories = new Dictionary<Version, Func<ITileSetDefinition>>();

            Init();
        }

        /// <summary>
        /// Resets the object factory by clearing all registered factories
        /// </summary>
        public static void Reset()
        {
            _layerFactories.Clear();
            _wlFactories.Clear();
            _loadProcFactories.Clear();
            _simpleSymbolFactories.Clear();
            _compoundSymbolFactories.Clear();
            _mapDefinitionFactories.Clear();
            _watermarkFactories.Clear();
            _tileSetDefinitionFactories.Clear();
            ResourceTypeRegistry.Reset();
            Init();
        }

        private static void Init()
        {
            _layerFactories.Add(
                new Version(1, 0, 0),
                OSGeo.MapGuide.ObjectModels.LayerDefinition.v1_0_0.LdfEntryPoint.CreateDefault);

            _loadProcFactories.Add(
                LoadType.Sdf,
                OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcEntryPoint.CreateDefaultSdf);
            _loadProcFactories.Add(
                LoadType.Shp,
                OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcEntryPoint.CreateDefaultShp);
            _loadProcFactories.Add(
                LoadType.Dwf,
                OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcEntryPoint.CreateDefaultDwf);

            _wlFactories.Add(
                new Version(1, 0, 0),
                OSGeo.MapGuide.ObjectModels.WebLayout.v1_0_0.WebLayoutEntryPoint.CreateDefault);

            _compoundSymbolFactories.Add(
                new Version(1, 0, 0),
                OSGeo.MapGuide.ObjectModels.SymbolDefinition.v1_0_0.CompoundSymbolDefinition.CreateDefault);

            _simpleSymbolFactories.Add(
                new Version(1, 0, 0),
                OSGeo.MapGuide.ObjectModels.SymbolDefinition.v1_0_0.SimpleSymbolDefinition.CreateDefault);

            _mapDefinitionFactories.Add(
                new Version(1, 0, 0),
                OSGeo.MapGuide.ObjectModels.MapDefinition.v1_0_0.MdfEntryPoint.CreateDefault);

            //Layer Definition 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.1.0"),
                Ldf110.LdfEntryPoint.Serialize,
                Ldf110.LdfEntryPoint.Deserialize);
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 1, 0), Ldf110.LdfEntryPoint.CreateDefault);

            //Layer Definition 1.2.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.2.0"),
                Ldf120.LdfEntryPoint.Serialize,
                Ldf120.LdfEntryPoint.Deserialize);
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 2, 0), Ldf120.LdfEntryPoint.CreateDefault);

            //Layer Definition 1.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.3.0"),
                Ldf130.LdfEntryPoint.Serialize,
                Ldf130.LdfEntryPoint.Deserialize);
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 3, 0), Ldf130.LdfEntryPoint.CreateDefault);

            //Layer Definition 2.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "2.3.0"),
                Ldf230.LdfEntryPoint.Serialize,
                Ldf230.LdfEntryPoint.Deserialize);
            ObjectFactory.RegisterLayerFactoryMethod(new Version(2, 3, 0), Ldf230.LdfEntryPoint.CreateDefault);

            //Layer Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "2.4.0"),
                Ldf240.LdfEntryPoint.Serialize,
                Ldf240.LdfEntryPoint.Deserialize);
            ObjectFactory.RegisterLayerFactoryMethod(new Version(2, 4, 0), Ldf240.LdfEntryPoint.CreateDefault);

            //Load Procedure 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "1.1.0"),
                Lp110.LoadProcEntryPoint.Serialize,
                Lp110.LoadProcEntryPoint.Deserialize);

            //Load Procedure 1.1.0 schema offers nothing new for the ones we want to support, so nothing to register
            //with the ObjectFactory

            //Load Procedure 2.2.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "2.2.0"),
                Lp220.LoadProcEntryPoint.Serialize,
                Lp220.LoadProcEntryPoint.Deserialize);
            ObjectFactory.RegisterLoadProcedureFactoryMethod(LoadType.Sqlite, Lp220.LoadProcEntryPoint.CreateDefaultSqlite);

            //Web Layout 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "1.1.0"),
                WL110.WebLayoutEntryPoint.Serialize,
                WL110.WebLayoutEntryPoint.Deserialize);
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(1, 1, 0), WL110.WebLayoutEntryPoint.CreateDefault);

            //Web Layout 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "2.4.0"),
                WL240.WebLayoutEntryPoint.Serialize,
                WL240.WebLayoutEntryPoint.Deserialize);
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(2, 4, 0), WL240.WebLayoutEntryPoint.CreateDefault);

            //Web Layout 2.6.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "2.6.0"),
                WL260.WebLayoutEntryPoint.Serialize,
                WL260.WebLayoutEntryPoint.Deserialize);
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(2, 6, 0), WL260.WebLayoutEntryPoint.CreateDefault);

            //Symbol Definition 1.1.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "1.1.0"),
                Sym110.SymbolDefEntryPoint.Serialize,
                Sym110.SymbolDefEntryPoint.Deserialize);
            ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(1, 1, 0), Sym110.SymbolDefEntryPoint.CreateDefaultCompound);
            ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(1, 1, 0), Sym110.SymbolDefEntryPoint.CreateDefaultSimple);

            //Symbol Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "2.4.0"),
                Sym240.SymbolDefEntryPoint.Serialize,
                Sym240.SymbolDefEntryPoint.Deserialize);
            ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(2, 4, 0), Sym240.SymbolDefEntryPoint.CreateDefaultCompound);
            ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(2, 4, 0), Sym240.SymbolDefEntryPoint.CreateDefaultSimple);

            //Map Definition 2.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "2.3.0"),
                Mdf230.MdfEntryPoint.Serialize,
                Mdf230.MdfEntryPoint.Deserialize);
            ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(2, 3, 0), Mdf230.MdfEntryPoint.CreateDefault);

            //Map Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "2.4.0"),
                Mdf240.MdfEntryPoint.Serialize,
                Mdf240.MdfEntryPoint.Deserialize);
            ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(2, 4, 0), Mdf240.MdfEntryPoint.CreateDefault);

            //Map Definition 3.0.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "3.0.0"),
                Mdf300.MdfEntryPoint.Serialize,
                Mdf300.MdfEntryPoint.Deserialize);
            ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(3, 0, 0), Mdf300.MdfEntryPoint.CreateDefault);

            //Watermark Definition 2.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition.ToString(), "2.3.0"),
                Wdf230.WdfEntryPoint.Serialize,
                Wdf230.WdfEntryPoint.Deserialize);
            ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(2, 3, 0), Wdf230.WdfEntryPoint.CreateDefault);

            //Watermark Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition.ToString(), "2.4.0"),
                Wdf240.WdfEntryPoint.Serialize,
                Wdf240.WdfEntryPoint.Deserialize);
            ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(2, 4, 0), Wdf240.WdfEntryPoint.CreateDefault);
            
            //Tile Set Definition 3.0.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.TileSetDefinition.ToString(), "3.0.0"),
                Tsd300.TileSetDefinition.Serialize,
                Tsd300.TileSetDefinition.Deserialize);
            ObjectFactory.RegisterTileSetDefinitionFactoryMethod(new Version(3, 0, 0), Tsd300.TileSetDefinition.CreateDefault);
        }

        #region Factory registration

        /// <summary>
        /// Registers a resource serializer
        /// </summary>
        /// <param name="desc">The desc.</param>
        /// <param name="serializer">The serializer.</param>
        public static void RegisterResource(ResourceTypeDescriptor desc, ResourceSerializer serializer)
        {
            Check.ArgumentNotNull(desc, nameof(desc));
            Check.ArgumentNotNull(serializer, nameof(serializer));
            ResourceTypeRegistry.RegisterResource(desc, serializer);
        }

        /// <summary>
        /// Registers a resource serializer
        /// </summary>
        /// <param name="resourceType">The resource type descriptor.</param>
        /// <param name="serializer">The serialize method.</param>
        /// <param name="deserializer">The deserialize method.</param>
        public static void RegisterResourceSerializer(ResourceTypeDescriptor resourceType, Func<IResource, Stream> serializer, Func<string, IResource> deserializer)
        {
            Check.ArgumentNotNull(resourceType, nameof(resourceType));
            Check.ArgumentNotNull(serializer, nameof(serializer));
            Check.ArgumentNotNull(deserializer, nameof(deserializer));
            ResourceTypeRegistry.RegisterResource(resourceType, serializer, deserializer);
        }

        /// <summary>
        /// Registers the compound symbol factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="func"></param>
        public static void RegisterCompoundSymbolFactoryMethod(Version version, Func<ICompoundSymbolDefinition> func)
        {
            Check.ArgumentNotNull(version, nameof(version));
            Check.ArgumentNotNull(func, nameof(func));
            if (_compoundSymbolFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _compoundSymbolFactories[version] = func;
        }

        /// <summary>
        /// Regsiters the simple symbol factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="func"></param>
        public static void RegisterSimpleSymbolFactoryMethod(Version version, Func<ISimpleSymbolDefinition> func)
        {
            Check.ArgumentNotNull(version, nameof(version));
            Check.ArgumentNotNull(func, nameof(func));
            if (_simpleSymbolFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _simpleSymbolFactories[version] = func;
        }

        /// <summary>
        /// Registers the layer factory method.
        /// </summary>
        /// <param name="version">The ver.</param>
        /// <param name="method">The method.</param>
        public static void RegisterLayerFactoryMethod(Version version, Func<LayerType, ILayerDefinition> method)
        {
            Check.ArgumentNotNull(version, nameof(version));
            Check.ArgumentNotNull(method, nameof(method));
            if (_layerFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _layerFactories[version] = method;
        }

        /// <summary>
        /// Registers the load procedure factory method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        public static void RegisterLoadProcedureFactoryMethod(LoadType type, Func<ILoadProcedure> method)
        {
            Check.ArgumentNotNull(method, nameof(method));
            if (_loadProcFactories.ContainsKey(type))
                throw new ArgumentException(Strings.LoadProcFactoryMethodAlreadyRegistered + type);

            _loadProcFactories[type] = method;
        }

        /// <summary>
        /// Registers the web layout factory method.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="method">The method.</param>
        public static void RegisterWebLayoutFactoryMethod(Version version, Func<string, IWebLayout> method)
        {
            Check.ArgumentNotNull(version, nameof(version));
            Check.ArgumentNotNull(method, nameof(method));
            if (_wlFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _wlFactories[version] = method;
        }

        /// <summary>
        /// Register the map definition factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="method"></param>
        public static void RegisterMapDefinitionFactoryMethod(Version version, Func<IMapDefinition> method)
        {
            Check.ArgumentNotNull(version, nameof(version));
            Check.ArgumentNotNull(method, nameof(method));
            if (_mapDefinitionFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _mapDefinitionFactories[version] = method;
        }

        /// <summary>
        /// Registers the Watermark Definition factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="method"></param>
        public static void RegisterWatermarkDefinitionFactoryMethod(Version version, Func<SymbolDefinitionType, IWatermarkDefinition> method)
        {
            Check.ArgumentNotNull(version, nameof(version));
            Check.ArgumentNotNull(method, nameof(method));
            if (_watermarkFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _watermarkFactories[version] = method;
        }
        
        /// <summary>
        /// Registers the Tile Set Definition factory method
        /// </summary>
        /// <param name="version"></param>
        /// <param name="func"></param>
        public static void RegisterTileSetDefinitionFactoryMethod(Version version, Func<ITileSetDefinition> func)
        {
            Check.ArgumentNotNull(version, nameof(version));
            Check.ArgumentNotNull(func, nameof(func));
            if (_tileSetDefinitionFactories.ContainsKey(version))
                throw new ArgumentException(Strings.FactoryMethodAlreadyRegistered + version);

            _tileSetDefinitionFactories[version] = func;
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
                throw new ArgumentException($"{nameof(minx)} > {nameof(maxx)}", nameof(minx)); //NOXLATE

            if (miny > maxy)
                throw new ArgumentException($"{nameof(miny)} > {nameof(maxy)}", nameof(miny)); //NOXLATE

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
        /// Creates the tile set definition
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static ITileSetDefinition CreateTileSetDefinition(Version version)
        {
            Check.ArgumentNotNull(version, nameof(version));
            if (!_mapDefinitionFactories.ContainsKey(version))
                throw new ArgumentException(Strings.UnknownTileDefinitionVersion + version.ToString());

            var tsd = _tileSetDefinitionFactories[version]();
            return tsd;
        }

        /// <summary>
        /// Creates the tile set definition using the default provider
        /// </summary>
        /// <param name="version"></param>
        /// <param name="extents"></param>
        /// <returns></returns>
        public static ITileSetDefinition CreateTileSetDefinition(Version version, IEnvelope extents)
        {
            var tsd = CreateTileSetDefinition(version);
            tsd.Extents = extents;
            return tsd;
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
        /// Returns a deserialized copy of an embedded Flexible Layout
        /// </summary>
        /// <param name="siteVersion"></param>
        /// <returns></returns>
        public static IApplicationDefinition DeserializeEmbeddedFlexLayout(Version siteVersion)
        {
            Check.ArgumentNotNull(siteVersion, nameof(siteVersion));
            if (siteVersion >= VER_240)
                return (IApplicationDefinition)ResourceTypeRegistry.Deserialize(Strings.BaseTemplate240_ApplicationDefinition);
            else
                return (IApplicationDefinition)ResourceTypeRegistry.Deserialize(Strings.BaseTemplate_ApplicationDefinition);
        }

        private static readonly string[] parameterizedWidgets =
        {
            KnownWidgetNames.CTRLClick,
            KnownWidgetNames.ZoomOnClick,
            KnownWidgetNames.ExtentHistory,
            KnownWidgetNames.Buffer,
            KnownWidgetNames.Measure,
            KnownWidgetNames.InvokeScript,
            KnownWidgetNames.InvokeURL,
            KnownWidgetNames.Search,
            KnownWidgetNames.CursorPosition,
            KnownWidgetNames.SelectionInfo,
            KnownWidgetNames.ViewSize
        };

        private static readonly string[] reactLayoutIncompatibleWidgets =
        {
            KnownWidgetNames.ColorPicker,
            KnownWidgetNames.ActivityIndicator,
            KnownWidgetNames.Print,
            KnownWidgetNames.SaveMap,
            KnownWidgetNames.LinkToView,
            KnownWidgetNames.SelectRadiusValue,
            KnownWidgetNames.PanQuery,
            KnownWidgetNames.PanOnClick
        };

        private static IUIWidget CreateVerticalWidget(IUIWidget widget)
        {
            var vert = widget.Clone();
            vert.Name = "vert" + widget.Name; //NOXLATE
            vert.Label = string.Empty;
            return vert;
        }

        private static Version VER_240 = new Version(2, 4);

        /// <summary>
        /// Creates a fusion flexible layout
        /// </summary>
        /// <param name="siteVersion">The site version</param>
        /// <param name="templates">The set of available templates</param>
        /// <param name="widgets">The set of available widgets</param>
        /// <param name="containers">The set of available containers</param>
        /// <param name="templateName">The name of the template. See <see cref="OSGeo.MapGuide.ObjectModels.ApplicationDefinition.FusionTemplateNames"/> for the common pre-defined names</param>
        /// <returns></returns>
        public static IApplicationDefinition CreateFlexibleLayout(Version siteVersion,
                                                                  IApplicationDefinitionTemplateInfoSet templates,
                                                                  IApplicationDefinitionWidgetInfoSet widgets,
                                                                  IApplicationDefinitionContainerInfoSet containers,
                                                                  string templateName)
        {
            Check.ArgumentNotNull(templates, nameof(templates));
            Check.ArgumentNotNull(widgets, nameof(widgets));
            Check.ArgumentNotNull(containers, nameof(containers));
            Check.ArgumentNotNull(templateName, nameof(templateName));

            IApplicationDefinition appDef = new ApplicationDefinitionType()
            {
                MapSet = new System.ComponentModel.BindingList<MapGroupType>(),
                WidgetSet = new System.ComponentModel.BindingList<WidgetSetType>()
            };

            //Find matching template. If it's a known template we should be able to
            //build it programatically, otherwise return a deserialized copy from our
            //embedded resource
            var tpl = templates.FindTemplate(templateName);
            if (tpl != null)
            {
                appDef.TemplateUrl = tpl.LocationUrl;
                appDef.Title = tpl.Name;
            }
            else
            {
                //NOTE: Depending on MapGuide Server version, this document may be
                //invalid (eg. References to widgets not available in that version)
                return DeserializeEmbeddedFlexLayout(siteVersion);
            }

            //Toolbars, every template has them
            var toolbar = appDef.CreateContainer("Toolbar", containers.FindContainer("Toolbar")); //NOXLATE
            var secToolbar = appDef.CreateContainer("ToolbarSecondary", containers.FindContainer("Toolbar")); //NOXLATE
            var vertToolbar = appDef.CreateContainer("ToolbarVertical", containers.FindContainer("Toolbar")); //NOXLATE

            //Context menus, every template has them
            var mapContextMenu = appDef.CreateContainer("MapContextMenu", containers.FindContainer("ContextMenu")); //NOXLATE
            var taskPaneMenu = appDef.CreateContainer("TaskMenu", containers.FindContainer("ContextMenu")); //NOXLATE

            //Menu
            var menu = appDef.CreateContainer("FileMenu", containers.FindContainer("Toolbar")); //NOXLATE

            //Status bar
            var statusbar = appDef.CreateContainer("Statusbar", containers.FindContainer("Splitterbar")); //NOXLATE

            string mapId = "MainMap"; //NOXLATE
            //Set default map group
            appDef.AddMapGroup(mapId, true, string.Empty);

            //Create default widget set
            var widgetSet = appDef.CreateWidgetSet(appDef.CreateMapWidget(mapId, mapContextMenu.Name));
            appDef.AddWidgetSet(widgetSet);

            //Add all known non-parameterized widgets to this widget set
            foreach (var wgt in widgets.WidgetInfo)
            {
                //Skip any widgets not compatible with mapguide-react-layout, these can always be added later on
                if (Array.IndexOf(reactLayoutIncompatibleWidgets, wgt.Type) >= 0)
                    continue;

                if (Array.IndexOf(parameterizedWidgets, wgt.Type) < 0)
                {
                    var widget = appDef.CreateWidget(wgt.Type, wgt);
                    widgetSet.AddWidget(widget);
                }
            }

            //Add some parameterized ones

            //Zoom In
            var zoomIn = (IUIWidget)appDef.CreateWidget("ZoomIn", widgets.FindWidget(KnownWidgetNames.ZoomOnClick)); //NOXLATE
            zoomIn.SetValue("Factor", "2"); //NOXLATE
            zoomIn.StatusText = zoomIn.Tooltip = Strings.ADF_Widget_ZoomIn_Desc;
            zoomIn.Label = Strings.ADF_Widget_ZoomIn_Label;
            zoomIn.ImageUrl = "images/icons.png"; //NOXLATE
            zoomIn.ImageClass = "zoom-in-fixed"; //NOXLATE
            var vZoomIn = CreateVerticalWidget(zoomIn);

            //Zoom Out
            var zoomOut = (IUIWidget)appDef.CreateWidget("ZoomOut", widgets.FindWidget(KnownWidgetNames.ZoomOnClick)); //NOXLATE
            zoomOut.SetValue("Factor", "0.5"); //NOXLATE
            zoomOut.StatusText = zoomOut.Tooltip = Strings.ADF_Widget_ZoomOut_Desc;
            zoomOut.Label = Strings.ADF_Widget_ZoomOut_Label;
            zoomOut.ImageUrl = "images/icons.png"; //NOXLATE
            zoomOut.ImageClass = "zoom-out-fixed"; //NOXLATE
            var vZoomOut = CreateVerticalWidget(zoomOut);

            //Previous View
            var prevView = (IUIWidget)appDef.CreateWidget("PreviousView", widgets.FindWidget(KnownWidgetNames.ExtentHistory)); //NOXLATE
            prevView.SetValue("Direction", "previous"); //NOXLATE
            prevView.StatusText = prevView.Tooltip = Strings.ADF_Widget_PreviousView_Desc;
            prevView.Label = Strings.ADF_Widget_PreviousView_Label;
            prevView.ImageUrl = "images/icons.png"; //NOXLATE
            prevView.ImageClass = "view-back"; //NOXLATE
            var vPrevView = CreateVerticalWidget(prevView);

            //Next View
            var nextView = (IUIWidget)appDef.CreateWidget("NextView", widgets.FindWidget(KnownWidgetNames.ExtentHistory)); //NOXLATE
            nextView.SetValue("Direction", "next"); //NOXLATE
            nextView.StatusText = nextView.Tooltip = Strings.ADF_Widget_NextView_Desc;
            nextView.Label = Strings.ADF_Widget_NextView_Label;
            nextView.ImageUrl = "images/icons.png"; //NOXLATE
            nextView.ImageClass = "view-forward"; //NOXLATE
            var vNextView = CreateVerticalWidget(nextView);

            //Buffer
            var buffer = (IUIWidget)appDef.CreateWidget("tbBuffer", widgets.FindWidget(KnownWidgetNames.BufferPanel)); //NOXLATE
            //buffer.SetValue("Target", "TaskPane"); //NOXLATE
            buffer.StatusText = buffer.Tooltip = Strings.ADF_Widget_Buffer_Desc;
            buffer.Tooltip = Strings.ADF_Widget_Buffer_Label;

            //Measure
            var measure = (IUIWidget)appDef.CreateWidget("Measure", widgets.FindWidget(KnownWidgetNames.Measure)); //NOXLATE
            var measureParams = new NameValueCollection();
            measureParams["Type"] = "both"; //NOXLATE
            measureParams["MeasureTooltipContainer"] = "MeasureResult"; //NOXLATE
            measureParams["MeasureTooltipType"] = "dynamic"; //NOXLATE
            measureParams["DistancePrecision"] = "0"; //NOXLATE
            measureParams["AreaPrecision"] = "0"; //NOXLATE
            measureParams["Units"] = "meters"; //NOXLATE
            measureParams["Target"] = "TaskPane"; //NOXLATE
            measure.SetAllValues(measureParams);
            measure.StatusText = buffer.Tooltip = Strings.ADF_Widget_Measure_Desc;
            measure.Tooltip = Strings.ADF_Widget_Measure_Label;

            //Show Overview
            var showOverview = (IUIWidget)appDef.CreateWidget("showOverview", widgets.FindWidget(KnownWidgetNames.InvokeScript)); //NOXLATE
            showOverview.Label = "Show Overview"; //NOXLATE
            showOverview.SetValue("Script", "showOverviewMap()"); //NOXLATE

            //Show Task Pane
            var showTaskPane = (IUIWidget)appDef.CreateWidget("showTaskPane", widgets.FindWidget(KnownWidgetNames.InvokeScript)); //NOXLATE
            showTaskPane.Label = "Show Task Pane"; //NOXLATE
            showTaskPane.SetValue("Script", "showTaskPane()"); //NOXLATE

            //Show Legend
            var showLegend = (IUIWidget)appDef.CreateWidget("showLegend", widgets.FindWidget(KnownWidgetNames.InvokeScript)); //NOXLATE
            showLegend.Label = "Show Legend"; //NOXLATE
            showLegend.SetValue("Script", "showLegend()"); //NOXLATE

            //Show Selection Panel
            var showSelectionPanel = (IUIWidget)appDef.CreateWidget("showSelectionPanel", widgets.FindWidget(KnownWidgetNames.InvokeScript)); //NOXLATE
            showSelectionPanel.Label = "Show Selection Panel"; //NOXLATE
            showSelectionPanel.SetValue("Script", "showSelectionPanel()"); //NOXLATE

            //Coordinate Tracker
            var coordTracker = appDef.CreateWidget("statusCoordinates", widgets.FindWidget(KnownWidgetNames.CursorPosition)); //NOXLATE
            coordTracker.SetValue("Template", "X: {x} {units}, Y: {y} {units}"); //NOXLATE
            coordTracker.SetValue("Precision", "4"); //NOXLATE
            coordTracker.SetValue("EmptyText", "&amp;nbsp;"); //NOXLATE

            //Selection Info
            var selInfo = appDef.CreateWidget("statusSelection", widgets.FindWidget(KnownWidgetNames.SelectionInfo)); //NOXLATE
            selInfo.SetValue("EmptyText", "No selection"); //NOXLATE

            //View Size
            var viewSize = appDef.CreateWidget("statusViewSize", widgets.FindWidget(KnownWidgetNames.ViewSize)); //NOXLATE
            viewSize.SetValue("Template", "{w} x {h} ({units})"); //NOXLATE
            viewSize.SetValue("Precision", "2"); //NOXLATE

            widgetSet.AddWidget(zoomIn);
            widgetSet.AddWidget(zoomOut);
            widgetSet.AddWidget(prevView);
            widgetSet.AddWidget(nextView);
            widgetSet.AddWidget(buffer);
            widgetSet.AddWidget(measure);
            widgetSet.AddWidget(showOverview);
            widgetSet.AddWidget(showTaskPane);
            widgetSet.AddWidget(showLegend);
            widgetSet.AddWidget(showSelectionPanel);
            widgetSet.AddWidget(coordTracker);
            widgetSet.AddWidget(selInfo);
            widgetSet.AddWidget(viewSize);

            widgetSet.AddWidget(vZoomIn);
            widgetSet.AddWidget(vZoomOut);
            widgetSet.AddWidget(vPrevView);
            widgetSet.AddWidget(vNextView);

            //Now here's where things may diverge completely between templates
            //So let's try for something that is somewhat consistent

            //2.2 specific stuff
            if (siteVersion >= new Version(2, 2))
            {
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.QuickPlot));
            }

            toolbar.AddItem(appDef.CreateSeparator());
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.RefreshMap));
            //2.4 requires maptips to be a toggle widget
            if (siteVersion >= VER_240)
            {
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Maptip));
            }
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.SelectRadius));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.SelectPolygon));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ClearSelection));

            toolbar.AddItem(appDef.CreateWidgetReference(buffer.Name));
            toolbar.AddItem(appDef.CreateWidgetReference(measure.Name));

            //2.2 specific stuff
            if (siteVersion >= new Version(2, 2))
            {
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.FeatureInfo));
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Query));
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Theme));
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Redline));
            }

            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ViewOptions));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.About));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Help));

            //Init secondary toolbar
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Select));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Pan));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Zoom));
            secToolbar.AddItem(appDef.CreateWidgetReference(zoomIn.Name));
            secToolbar.AddItem(appDef.CreateWidgetReference(zoomOut.Name));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.InitialMapView));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ZoomToSelection));
            secToolbar.AddItem(appDef.CreateWidgetReference(prevView.Name));
            secToolbar.AddItem(appDef.CreateWidgetReference(nextView.Name));

            //Init vertical toolbar
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.Select, widgets.FindWidget(KnownWidgetNames.Select))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.Pan, widgets.FindWidget(KnownWidgetNames.Pan))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.Zoom, widgets.FindWidget(KnownWidgetNames.Zoom))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.InitialMapView, widgets.FindWidget(KnownWidgetNames.InitialMapView))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.ZoomToSelection, widgets.FindWidget(KnownWidgetNames.ZoomToSelection))));

            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.Select)); //NOXLATE
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.Pan)); //NOXLATE
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.Zoom)); //NOXLATE
            vertToolbar.AddItem(appDef.CreateWidgetReference(vZoomIn.Name));
            vertToolbar.AddItem(appDef.CreateWidgetReference(vZoomOut.Name));
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.InitialMapView)); //NOXLATE
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.ZoomToSelection)); //NOXLATE
            vertToolbar.AddItem(appDef.CreateWidgetReference(vPrevView.Name));
            vertToolbar.AddItem(appDef.CreateWidgetReference(vNextView.Name));

            //Main menu
            menu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.MapMenu));

            //2.2 specific stuff
            if (siteVersion >= new Version(2, 2))
            {
                menu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.BasemapSwitcher));
            }
            var viewMenu = appDef.CreateFlyout(Strings.ADF_Flyout_View);
            viewMenu.AddItem(appDef.CreateWidgetReference(showOverview.Name));
            viewMenu.AddItem(appDef.CreateWidgetReference(showTaskPane.Name));
            viewMenu.AddItem(appDef.CreateWidgetReference(showLegend.Name));
            viewMenu.AddItem(appDef.CreateWidgetReference(showSelectionPanel.Name));
            menu.AddItem(viewMenu);

            //status bar
            statusbar.AddItem(appDef.CreateWidgetReference(coordTracker.Name));
            statusbar.AddItem(appDef.CreateWidgetReference(selInfo.Name));
            statusbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.EditableScale));
            statusbar.AddItem(appDef.CreateWidgetReference(viewSize.Name));

            //Map Context Menu
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.RefreshMap));
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Pan));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Zoom));
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(zoomIn.Name));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(zoomOut.Name));
            mapContextMenu.AddItem(appDef.CreateSeparator());
            var zoomMenu = appDef.CreateFlyout(Strings.ADF_Flyout_Zoom);

            mapContextMenu.AddItem(zoomMenu);
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Select));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ClearSelection));
            var selectMoreMenu = appDef.CreateFlyout(Strings.ADF_Flyout_SelectMore);

            mapContextMenu.AddItem(selectMoreMenu);
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(buffer.Name));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(measure.Name));

            if (siteVersion >= new Version(2, 2))
            {
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.FeatureInfo));
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Query));
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Theme));
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Redline));
            }

            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ViewOptions));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Help));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.About));

            //Tasks Context Menu
            taskPaneMenu.AddItem(appDef.CreateWidgetReference(measure.Name));
            taskPaneMenu.AddItem(appDef.CreateWidgetReference(buffer.Name));

            if (siteVersion >= new Version(2, 2))
            {
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.FeatureInfo));
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Query));
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Theme));
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Redline));
            }

            //Now add them all to the main widget set
            widgetSet.AddContainer(toolbar);
            widgetSet.AddContainer(secToolbar);
            widgetSet.AddContainer(vertToolbar);
            widgetSet.AddContainer(menu);
            widgetSet.AddContainer(statusbar);
            widgetSet.AddContainer(mapContextMenu);
            widgetSet.AddContainer(taskPaneMenu);

            //Set positioning
            toolbar.Position = "top"; //NOXLATE
            secToolbar.Position = "top"; //NOXLATE
            menu.Position = "top"; //NOXLATE
            statusbar.Position = "bottom"; //NOXLATE
            mapContextMenu.Position = "top"; //NOXLATE
            taskPaneMenu.Position = "top"; //NOXLATE
            vertToolbar.Position = "left"; //NOXLATE

            return appDef;
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
        /// Creates a new symbol library
        /// </summary>
        /// <returns></returns>
        public static ISymbolLibrary CreateSymbolLibrary()
        {
            return new OSGeo.MapGuide.ObjectModels.SymbolLibrary.v1_0_0.SymbolLibraryType()
            {
                Symbol = new System.ComponentModel.BindingList<SymbolLibrary.v1_0_0.SymbolType>()
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
        public static IResource DeserializeXml(string xml)
        {
            return ResourceTypeRegistry.Deserialize(xml);
        }

        /// <summary>
        /// Serializes the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public static Stream Serialize(IResource resource)
        {
            return ResourceTypeRegistry.Serialize(resource);
        }

        /// <summary>
        /// Serializes the specified resource.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static string SerializeAsString(IResource resource)
        {
            return ResourceTypeRegistry.SerializeAsString(resource);
        }

        /// <summary>
        /// Deserializes the specified stream for the specified resource type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IResource Deserialize(string resourceType, Stream stream)
        {
            return ResourceTypeRegistry.Deserialize(resourceType, stream);
        }
    }
}