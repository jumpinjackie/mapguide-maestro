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
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Base.Editor
{
    /// <summary>
    /// Defines an interface for creating editor views for a given resource
    /// </summary>
    public interface IEditorFactory
    {
        /// <summary>
        /// Gets the type of resource (and version) this editor can edit
        /// </summary>
        ResourceTypeDescriptor ResourceTypeAndVersion { get; }

        /// <summary>
        /// Creates an instance of this resource editor
        /// </summary>
        /// <returns></returns>
        IEditorViewContent Create();
    }

    internal class FusionEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public FusionEditorFactory()
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new FusionEditor();
        }
    }

    internal class DrawingSourceEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public DrawingSourceEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.DrawingSource, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new DrawingSourceEditor();
        }
    }

    internal class FeatureSourceEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public FeatureSourceEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.FeatureSource, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new FeatureSourceEditor();
        }
    }

    internal class LayerDefinitionEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public LayerDefinitionEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefinition, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new LayerDefinitionEditor();
        }
    }

    internal class LoadProcedureEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public LoadProcedureEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.LoadProcedure, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new LoadProcedureEditor();
        }
    }

    internal class MapDefinitionEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public MapDefinitionEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new MapDefinitionEditor();
        }
    }

    internal class PrintLayoutEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public PrintLayoutEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.PrintLayout, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new PrintLayoutEditor();
        }
    }

    internal class SymbolDefinitionEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }
        
        public SymbolDefinitionEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.SymbolDefinition, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new SymbolDefinitionEditor();
        }
    }

    internal class WebLayoutEditorFactory : IEditorFactory
    {
        public ResourceTypeDescriptor ResourceTypeAndVersion { get; private set; }

        public WebLayoutEditorFactory() 
        {
            this.ResourceTypeAndVersion = new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, "1.0.0"); //NOXLATE
        }

        public IEditorViewContent Create()
        {
            return new WebLayoutEditor();
        }
    }
}
