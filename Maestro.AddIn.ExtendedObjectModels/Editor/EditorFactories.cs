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
using Maestro.Base.Editor;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.AddIn.ExtendedObjectModels.Editor
{
    internal class WebLayout110EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public WebLayout110EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, "1.1.0");
        }

        public IEditorViewContent Create()
        {
            return new WebLayoutEditor();
        }
    }

    internal class LayerDefinition110EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public LayerDefinition110EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefinition, "1.1.0");
        }

        public IEditorViewContent Create()
        {
            //Use the same 1.0.0 editor for now
            return new LayerDefinitionEditor();
        }
    }

    internal class LayerDefinition120EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public LayerDefinition120EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefinition, "1.2.0");
        }

        public IEditorViewContent Create()
        {
            //Use the same 1.0.0 editor for now
            return new LayerDefinitionEditor(); 
        }
    }

    internal class LayerDefinition130EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public LayerDefinition130EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefinition, "1.3.0");
        }

        public IEditorViewContent Create()
        {
            //Use the same 1.0.0 editor for now
            return new LayerDefinitionEditor();
        }
    }

    internal class LoadProcedure110EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public LoadProcedure110EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.LoadProcedure, "1.1.0");
        }

        public IEditorViewContent Create()
        {
            //We can use the same editor, nothing structurally changed
            return new LoadProcedureEditor();
        }
    }

    internal class LoadProcedure220EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public LoadProcedure220EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.LoadProcedure, "2.2.0");
        }

        public IEditorViewContent Create()
        {
            //We can use the same editor, nothing structurally changed
            return new LoadProcedureEditor();
        }
    }
}
