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

using System.Linq;
using System.Xml.Serialization;
#pragma warning disable 1591, 0114, 0108, 0114, 0108
namespace OSGeo.MapGuide.ObjectModels.Capabilities.v1_1_0
{
    partial class FdoProviderCapabilities : IFdoProviderCapabilities
    {
        IFdoCommandCapabilities IFdoProviderCapabilities.Command
        {
            get { return this.Command; }
        }

        IFdoConnectionCapabilities IFdoProviderCapabilities.Connection
        {
            get { return this.Connection; }
        }

        IFdoExpressionCapabilities IFdoProviderCapabilities.Expression
        {
            get { return this.Expression; }
        }

        IFdoFilterCapabilities IFdoProviderCapabilities.Filter
        {
            get { return this.Filter; }
        }

        IFdoGeometryCapabilities IFdoProviderCapabilities.Geometry
        {
            get { return this.Geometry; }
        }

        IFdoRasterCapabilities IFdoProviderCapabilities.Raster
        {
            get { return this.Raster; }
        }

        IFdoSchemaCapabilities IFdoProviderCapabilities.Schema
        {
            get { return this.Schema; }
        }
    }

    partial class FdoProviderCapabilitiesCommand : IFdoCommandCapabilities
    {
        string[] IFdoCommandCapabilities.SupportedCommands
        {
            get { return this.SupportedCommands.Select(x => x.ToString()).ToArray(); }
        }
    }

    partial class FdoProviderCapabilitiesConnection : IFdoConnectionCapabilities
    {
        [XmlIgnore]
        public string[] SupportedSpatialContextExtentTypes
        {
            get { return this.SpatialContextExtent.Select(x => x.ToString()).ToArray(); }
        }

        string IFdoConnectionCapabilities.ThreadCapability
        {
            get { return this.ThreadCapability.ToString(); }
        }
    }

    partial class FdoProviderCapabilitiesExpressionFunctionDefinition : IFdoFunctionDefintion
    {
        [XmlIgnore]
        public IFdoFunctionDefintionSignature[] Signatures
        {
            get { return this.SignatureDefinitionCollection.ToArray(); }
        }
    }

    partial class FdoProviderCapabilitiesExpressionFunctionDefinitionSignatureDefinition : IFdoFunctionDefintionSignature
    {
        [XmlIgnore]
        public IFdoFunctionArgumentDefinition[] Arguments
        {
            get { return this.ArgumentDefinitionList.ArgumentDefinition.ToArray(); }
        }

        [XmlIgnore]
        public string ReturnType
        {
            get { return this.DataType.ToString(); }
        }
    }

    partial class FdoProviderCapabilitiesExpressionFunctionDefinitionSignatureDefinitionArgumentDefinitionListArgumentDefinition : IFdoFunctionArgumentDefinition
    {
    }

    partial class FdoProviderCapabilitiesExpression : IFdoExpressionCapabilities
    {
        [XmlIgnore]
        public IFdoFunctionDefintion[] SupportedFunctions
        {
            get { return this.FunctionDefinitionList.ToArray(); }
        }

        [XmlIgnore]
        public string[] ExpressionTypes
        {
            get { return this.Type.Select(x => x.ToString()).ToArray(); }
        }
    }

    partial class FdoProviderCapabilitiesFilter : IFdoFilterCapabilities
    {
        [XmlIgnore]
        public string[] ConditionTypes
        {
            get { return this.Condition.Select(x => x.ToString()).ToArray(); }
        }

        [XmlIgnore]
        public string[] DistanceOperations
        {
            get { return this.Distance.Select(x => x.ToString()).ToArray(); }
        }

        [XmlIgnore]
        public string[] SpatialOperations
        {
            get { return this.Spatial.Select(x => x.ToString()).ToArray(); }
        }
    }

    partial class FdoProviderCapabilitiesGeometry : IFdoGeometryCapabilities
    {
        [XmlIgnore]
        public string[] SupportedComponentTypes
        {
            get { return this.Components.Select(x => x.ToString()).ToArray(); }
        }

        [XmlIgnore]
        public string[] SupportedGeometryTypes
        {
            get { return this.Types.Select(x => x.ToString()).ToArray(); }
        }
    }

    partial class FdoProviderCapabilitiesRaster : IFdoRasterCapabilities
    {
    }

    partial class FdoProviderCapabilitiesSchema : IFdoSchemaCapabilities
    {
        [XmlIgnore]
        public string[] SupportedClassTypes
        {
            get { return this.Class.Select(x => x.ToString()).ToArray(); }
        }

        [XmlIgnore]
        public string[] SupportedDataTypes
        {
            get { return this.Data.Select(x => x.ToString()).ToArray(); }
        }
    }
}