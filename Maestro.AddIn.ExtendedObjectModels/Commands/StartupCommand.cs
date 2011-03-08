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
using System.Text;
using ICSharpCode.Core;
using Maestro.Base;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;

using Ldf110 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_1_0;
using Ldf120 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_2_0;
using Ldf130 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_3_0;

using Lp110 = OSGeo.MapGuide.ObjectModels.LoadProcedure_1_1_0;
using Lp220 = OSGeo.MapGuide.ObjectModels.LoadProcedure_2_2_0;
using WL110 = OSGeo.MapGuide.ObjectModels.WebLayout_1_1_0;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.AddIn.ExtendedObjectModels.Commands
{
    public class StartupCommand : AbstractCommand
    {
        public override void Run()
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

        }
    }
}
