#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Text;
using System.Runtime.CompilerServices;

using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;

using Ldf110 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_1_0;
using Ldf120 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_2_0;
using Ldf130 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_3_0;
using Ldf230 = OSGeo.MapGuide.ObjectModels.LayerDefinition_2_3_0;
using Ldf240 = OSGeo.MapGuide.ObjectModels.LayerDefinition_2_4_0;

using Lp110 = OSGeo.MapGuide.ObjectModels.LoadProcedure_1_1_0;
using Lp220 = OSGeo.MapGuide.ObjectModels.LoadProcedure_2_2_0;
using WL110 = OSGeo.MapGuide.ObjectModels.WebLayout_1_1_0;
using WL240 = OSGeo.MapGuide.ObjectModels.WebLayout_2_4_0;

using Sym110 = OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0;
using Sym240 = OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0;

using Mdf230 = OSGeo.MapGuide.ObjectModels.MapDefinition_2_3_0;
using Mdf240 = OSGeo.MapGuide.ObjectModels.MapDefinition_2_4_0;

using Wdf230 = OSGeo.MapGuide.ObjectModels.WatermarkDefinition_2_3_0;
using Wdf240 = OSGeo.MapGuide.ObjectModels.WatermarkDefinition_2_4_0;

using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using OSGeo.MapGuide.ObjectModels;

namespace OSGeo.MapGuide.ExtendedObjectModels
{
    /// <summary>
    /// <c>OSGeo.MapGuide.ExtendedObjectModels</c> provides the <see cref="ModelSetup"/> utility class, which
    /// provides a convenient method of registering all known resources with schema versions greater than v1.0.0 
    /// 
    /// </summary>
    [CompilerGenerated]
    class NsDoc { }

    /// <summary>
    /// A helper class that registers validatiors, serializers and factories for resource types beyond the initial version
    /// of a resource's XML schema. This allows for the consuming application to be able to properly consume all supported
    /// versions of any resource type in their object-oriented forms. Versions that are not supported or recognised are treated
    /// as <see cref="T:OSGeo.MapGuide.ObjectModels.UntypedResource"/> instances which are effectively containers of arbitrary
    /// XML content
    /// </summary>
    public static class ModelSetup
    {
        /// <summary>
        /// Registers validators, serializers and instance factories of all known extended resource data types.
        /// 
        /// Invoke this method as part of your application's startup process before using any other part of the Maestro API
        /// </summary>
        /// <remarks>
        /// This only needs to be called once, and should generally be done as part of your application's startup routine
        /// </remarks>
        public static void Initialize()
        {
            //By default the ObjectFactory, ResourceTypeRegistry and ResourceValidatorSet only
            //support v1.0.0 of all resource types. To support additional types we need to inject
            //this information as part of the consuming application's init/startup process
            //
            //This is our application's entry point, so we do this here.

            //Layer Definition 1.1.0
            ResourceValidatorSet.RegisterValidator(new Ldf110.LayerDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.1.0"),
                new ResourceSerializationCallback(Ldf110.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf110.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 1, 0), new LayerCreatorFunc(Ldf110.LdfEntryPoint.CreateDefault));

            //Layer Definition 1.2.0
            ResourceValidatorSet.RegisterValidator(new Ldf120.LayerDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.2.0"),
                new ResourceSerializationCallback(Ldf120.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf120.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 2, 0), new LayerCreatorFunc(Ldf120.LdfEntryPoint.CreateDefault));

            //Layer Definition 1.3.0
            ResourceValidatorSet.RegisterValidator(new Ldf130.LayerDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.3.0"),
                new ResourceSerializationCallback(Ldf130.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf130.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 3, 0), new LayerCreatorFunc(Ldf130.LdfEntryPoint.CreateDefault));

            //Layer Definition 2.3.0
            ResourceValidatorSet.RegisterValidator(new Ldf230.LayerDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "2.3.0"),
                new ResourceSerializationCallback(Ldf230.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf230.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(2, 3, 0), new LayerCreatorFunc(Ldf230.LdfEntryPoint.CreateDefault));

            //Layer Definition 2.4.0
            ResourceValidatorSet.RegisterValidator(new Ldf240.LayerDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "2.4.0"),
                new ResourceSerializationCallback(Ldf240.LdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Ldf240.LdfEntryPoint.Deserialize));
            ObjectFactory.RegisterLayerFactoryMethod(new Version(2, 4, 0), new LayerCreatorFunc(Ldf240.LdfEntryPoint.CreateDefault));

            //Load Procedure 1.1.0
            ResourceValidatorSet.RegisterValidator(new Lp110.LoadProcedureValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LoadProcedure, "1.1.0"),
                new ResourceSerializationCallback(Lp110.LoadProcEntryPoint.Serialize),
                new ResourceDeserializationCallback(Lp110.LoadProcEntryPoint.Deserialize));

            //Load Procedure 1.1.0 schema offers nothing new for the ones we want to support, so nothing to register
            //with the ObjectFactory

            //Load Procedure 2.2.0
            ResourceValidatorSet.RegisterValidator(new Lp220.LoadProcedureValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.LoadProcedure, "2.2.0"),
                new ResourceSerializationCallback(Lp220.LoadProcEntryPoint.Serialize),
                new ResourceDeserializationCallback(Lp220.LoadProcEntryPoint.Deserialize));
            ObjectFactory.RegisterLoadProcedureFactoryMethod(LoadType.Sqlite, new LoadProcCreatorFunc(Lp220.LoadProcEntryPoint.CreateDefaultSqlite));

            //Web Layout 1.1.0
            ResourceValidatorSet.RegisterValidator(new WL110.WebLayoutValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout, "1.1.0"),
                new ResourceSerializationCallback(WL110.WebLayoutEntryPoint.Serialize),
                new ResourceDeserializationCallback(WL110.WebLayoutEntryPoint.Deserialize));
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(1, 1, 0), new WebLayoutCreatorFunc(WL110.WebLayoutEntryPoint.CreateDefault));

            //Web Layout 2.4.0
            ResourceValidatorSet.RegisterValidator(new WL240.WebLayoutValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WebLayout, "2.4.0"),
                new ResourceSerializationCallback(WL240.WebLayoutEntryPoint.Serialize),
                new ResourceDeserializationCallback(WL240.WebLayoutEntryPoint.Deserialize));
            ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(2, 4, 0), new WebLayoutCreatorFunc(WL240.WebLayoutEntryPoint.CreateDefault));

            //Symbol Definition 1.1.0
            ResourceValidatorSet.RegisterValidator(new Sym110.SymbolDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition, "1.1.0"),
                new ResourceSerializationCallback(Sym110.SymbolDefEntryPoint.Serialize),
                new ResourceDeserializationCallback(Sym110.SymbolDefEntryPoint.Deserialize));
            ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(1, 1, 0), new CompoundSymbolDefCreatorFunc(Sym110.SymbolDefEntryPoint.CreateDefaultCompound));
            ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(1, 1, 0), new SimpleSymbolDefCreatorFunc(Sym110.SymbolDefEntryPoint.CreateDefaultSimple));

            //Symbol Definition 2.4.0
            ResourceValidatorSet.RegisterValidator(new Sym240.SymbolDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition, "2.4.0"),
                new ResourceSerializationCallback(Sym240.SymbolDefEntryPoint.Serialize),
                new ResourceDeserializationCallback(Sym240.SymbolDefEntryPoint.Deserialize));
            ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(2, 4, 0), new CompoundSymbolDefCreatorFunc(Sym240.SymbolDefEntryPoint.CreateDefaultCompound));
            ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(2, 4, 0), new SimpleSymbolDefCreatorFunc(Sym240.SymbolDefEntryPoint.CreateDefaultSimple));

            //Map Definition 2.3.0
            ResourceValidatorSet.RegisterValidator(new Mdf230.MapDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.MapDefinition, "2.3.0"),
                new ResourceSerializationCallback(Mdf230.MdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Mdf230.MdfEntryPoint.Deserialize));
            ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(2, 3, 0), new MapDefinitionCreatorFunc(Mdf230.MdfEntryPoint.CreateDefault));

            //Map Definition 2.4.0
            ResourceValidatorSet.RegisterValidator(new Mdf240.MapDefinitionValidator());
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.MapDefinition, "2.4.0"),
                new ResourceSerializationCallback(Mdf240.MdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Mdf240.MdfEntryPoint.Deserialize));
            ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(2, 4, 0), new MapDefinitionCreatorFunc(Mdf240.MdfEntryPoint.CreateDefault));

            //Watermark Definition 2.3.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition, "2.3.0"),
                new ResourceSerializationCallback(Wdf230.WdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Wdf230.WdfEntryPoint.Deserialize));
            ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(2, 3, 0), new WatermarkCreatorFunc(Wdf230.WdfEntryPoint.CreateDefault));

            //Watermark Definition 2.4.0
            ResourceTypeRegistry.RegisterResource(
                new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition, "2.4.0"),
                new ResourceSerializationCallback(Wdf240.WdfEntryPoint.Serialize),
                new ResourceDeserializationCallback(Wdf240.WdfEntryPoint.Deserialize));
            ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(2, 4, 0), new WatermarkCreatorFunc(Wdf240.WdfEntryPoint.CreateDefault));
        }
    }
}
