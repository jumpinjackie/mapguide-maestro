#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using Maestro.Base.Editor;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.AddIn.ExtendedObjectModels.Editor
{
    internal class WebLayout110EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public WebLayout110EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "1.1.0"); //NOXLATE
        }

        public IEditorViewContent Create() => new WebLayoutEditor();
    }

    internal class WebLayout240EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public WebLayout240EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "2.4.0"); //NOXLATE
        }

        public IEditorViewContent Create() => new WebLayoutEditor();
    }

    internal class WebLayout260EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public WebLayout260EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "2.6.0"); //NOXLATE
        }

        public IEditorViewContent Create() => new WebLayoutEditor();
    }

    internal class LayerDefinition110EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LayerDefinition110EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.1.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new LayerDefinitionEditor();
    }

    internal class LayerDefinition120EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LayerDefinition120EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.2.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new LayerDefinitionEditor();
    }

    internal class LayerDefinition130EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LayerDefinition130EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.3.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new LayerDefinitionEditor();
    }

    internal class LayerDefinition230EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LayerDefinition230EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "2.3.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new LayerDefinitionEditor();
    }

    internal class LayerDefinition240EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LayerDefinition240EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "2.4.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new LayerDefinitionEditor();
    }

    internal class LayerDefinition400EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LayerDefinition400EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "4.0.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new LayerDefinitionEditor();
    }

    internal class MapDefinition230EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public MapDefinition230EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "2.3.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new MapDefinitionEditor();
    }

    internal class MapDefinition240EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public MapDefinition240EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "2.4.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new MapDefinitionEditor();
    }

    internal class MapDefinition300EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public MapDefinition300EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "3.0.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new MapDefinitionEditor();
    }

    internal class WatermarkDefinition230EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public WatermarkDefinition230EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition.ToString(), "2.3.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new WatermarkDefinitionEditor();
    }

    internal class WatermarkDefinition240EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public WatermarkDefinition240EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.WatermarkDefinition.ToString(), "2.4.0"); //NOXLATE
        }

        //Use the same 1.0.0 editor for now
        public IEditorViewContent Create() => new WatermarkDefinitionEditor();
    }

    internal class LoadProcedure110EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LoadProcedure110EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "1.1.0"); //NOXLATE
        }

        //We can use the same editor, nothing structurally changed
        public IEditorViewContent Create() => new LoadProcedureEditor();
    }

    internal class LoadProcedure220EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public LoadProcedure220EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "2.2.0"); //NOXLATE
        }

        //We can use the same editor, nothing structurally changed
        public IEditorViewContent Create() => new LoadProcedureEditor();
    }

    internal class SymbolDefinition110EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public SymbolDefinition110EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "1.1.0"); //NOXLATE
        }

        public IEditorViewContent Create() => new SymbolDefinitionEditor();
    }

    internal class SymbolDefinition240EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public SymbolDefinition240EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "2.4.0"); //NOXLATE
        }

        public IEditorViewContent Create() => new SymbolDefinitionEditor();
    }

    internal class TileSetDefinition300EditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        public TileSetDefinition300EditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(ResourceTypes.TileSetDefinition.ToString(), "3.0.0"); //NOXLATE
        }

        public IEditorViewContent Create() => new TileSetEditor();
    }
}