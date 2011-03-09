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
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using System.Xml;
using System.IO;

#pragma warning disable 1591, 0114, 0108

#if SYM_DEF_110
namespace OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0
#else
namespace OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0
#endif
{
    partial class SimpleSymbolDefinition : ISimpleSymbolDefinition
    {
        public static SimpleSymbolDefinition CreateDefault()
        {
            var simpleSym = new SimpleSymbolDefinition()
            {
                Graphics = new System.ComponentModel.BindingList<GraphicBase>(),
                ParameterDefinition = new ParameterDefinition()
                {
                    Parameter = new System.ComponentModel.BindingList<Parameter>()
                }
            };
            return simpleSym;
        }

        private static readonly Version RES_VERSION = new Version(1, 0, 0);

        [XmlIgnore]
        public OSGeo.MapGuide.MaestroAPI.IServerConnection CurrentConnection
        {
            get;
            set;
        }

        private string _resId;

        [XmlIgnore]
        public string ResourceID
        {
            get
            {
                return _resId;
            }
            set
            {
                if (!ResourceIdentifier.Validate(value))
                    throw new InvalidOperationException("Not a valid resource identifier"); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.SymbolDefinition.ToString())
                    throw new InvalidOperationException("Invalid resource identifier for this type of object: " + res.Extension); //LOCALIZE

                _resId = value;
                this.OnPropertyChanged("ResourceID");
            }
        }

        [XmlIgnore]
        public ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.SymbolDefinition;
            }
        }

        [XmlIgnore]
        public Version ResourceVersion
        {
            get
            {
                return RES_VERSION;
            }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string ValidatingSchema 
        { 
            get { return "SymbolDefinition-1.0.0.xsd"; }
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        IEnumerable<IGraphicBase> ISimpleSymbolDefinition.Graphics
        {
            get 
            {
                foreach (var g in this.Graphics)
                {
                    yield return g;
                }
            }
        }

        void ISimpleSymbolDefinition.AddGraphics(IGraphicBase graphics)
        {
            var g = graphics as GraphicBase;
            if (g != null)
            {
                this.Graphics.Add(g);
            }
        }

        void ISimpleSymbolDefinition.RemoveGraphics(IGraphicBase graphics)
        {
            var g = graphics as GraphicBase;
            if (g != null)
            {
                this.Graphics.Remove(g);
            }
        }

        IResizeBox ISimpleSymbolDefinition.ResizeBox
        {
            get
            {
                return resizeBoxField;
            }
            set
            {
                resizeBoxField = (ResizeBox)value;
            }
        }

        IPointUsage ISimpleSymbolDefinition.PointUsage
        {
            get
            {
                return pointUsageField;
            }
            set
            {
                pointUsageField = (PointUsage)value;
            }
        }

        ILineUsage ISimpleSymbolDefinition.LineUsage
        {
            get
            {
                return lineUsageField;
            }
            set
            {
                lineUsageField = (LineUsage)value;
            }
        }

        IAreaUsage ISimpleSymbolDefinition.AreaUsage
        {
            get
            {
                return areaUsageField;
            }
            set
            {
                areaUsageField = (AreaUsage)value;
            }
        }

        IParameterDefinition ISimpleSymbolDefinition.ParameterDefinition
        {
            get { return parameterDefinitionField; }
        }
    }

#if SYM_DEF_110
    partial class Text : IText2
#else
    partial class Text : IText
#endif
    {

        bool? IText.Bold
        {
            get
            {
                bool b;
                if (!bool.TryParse(this.Bold, out b))
                    return b;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Bold = value.Value ? "true" : "false";
                else
                    this.Bold = null;
            }
        }

        bool? IText.Italic
        {
            get
            {
                bool b;
                if (!bool.TryParse(this.Italic, out b))
                    return b;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Italic = value.Value ? "true" : "false";
                else
                    this.Italic = null;
            }
        }

        bool? IText.Underlined
        {
            get
            {
                bool b;
                if (!bool.TryParse(this.Underlined, out b))
                    return b;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Underlined = value.Value ? "true" : "false";
                else
                    this.Underlined = null;
            }
        }

        double? IText.Height
        {
            get
            {
                double d;
                if (!double.TryParse(this.Height, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Height = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.Height = null;
            }
        }

        bool? IText.HeightScalable
        {
            get
            {
                bool b;
                if (!bool.TryParse(this.HeightScalable, out b))
                    return b;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.HeightScalable = value.Value ? "true" : "false";
                else
                    this.HeightScalable = null;
            }
        }

        double? IText.Angle
        {
            get
            {
                double d;
                if (!double.TryParse(this.Angle, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Angle = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.Angle = null;
            }
        }

        double? IText.PositionX
        {
            get
            {
                double d;
                if (!double.TryParse(this.PositionX, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.PositionX = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.PositionX = null;
            }
        }

        double? IText.PositionY
        {
            get
            {
                double d;
                if (!double.TryParse(this.PositionY, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.PositionY = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.PositionY = null;
            }
        }

        double? IText.LineSpacing
        {
            get
            {
                double d;
                if (!double.TryParse(this.LineSpacing, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.LineSpacing = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.LineSpacing = null;
            }
        }

        ITextFrame IText.Frame
        {
            get
            {
                return this.Frame;
            }
            set
            {
                this.Frame = (TextFrame)value;
            }
        }
    }

    partial class TextFrame : ITextFrame
    {
        double? ITextFrame.OffsetX
        {
            get
            {
                double d;
                if (!double.TryParse(this.OffsetX, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.OffsetX = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.OffsetX = null;
            }
        }

        double? ITextFrame.OffsetY
        {
            get
            {
                double d;
                if (!double.TryParse(this.OffsetY, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.OffsetY = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.OffsetY = null;
            }
        }
    }

    partial class ResizeBox : IResizeBox
    {
        double? IResizeBox.SizeX
        {
            get
            {
                double d;
                if (!double.TryParse(this.SizeX, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.SizeX = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.SizeX = null;
            }
        }

        double? IResizeBox.SizeY
        {
            get
            {
                double d;
                if (!double.TryParse(this.SizeY, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.SizeY = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.SizeY = null;
            }
        }

        double? IResizeBox.PositionX
        {
            get
            {
                double d;
                if (!double.TryParse(this.PositionX, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.PositionX = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.PositionX = null;
            }
        }

        double? IResizeBox.PositionY
        {
            get
            {
                double d;
                if (!double.TryParse(this.PositionY, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.PositionY = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.PositionY = null;
            }
        }
    }

    partial class Graphics : IGraphics
    {
        public IEnumerable<IGraphicBase> Elements
        {
            get 
            {
                foreach (var g in this.Items)
                {
                    yield return g;
                }
            }
        }

        public void AddGraphicElement(IGraphicBase graphics)
        {
            var g = graphics as GraphicBase;
            if (g != null)
            {
                this.Items.Add(g);
            }
        }

        public void RemoveGraphicElement(IGraphicBase graphics)
        {
            var g = graphics as GraphicBase;
            if (g != null)
            {
                this.Items.Remove(g);
            }
        }
    }

    partial class GraphicBase : IGraphicBase
    {
        
    }

    partial class Parameter : IParameter
    {
        string IParameter.DataType
        {
            get
            {
                return dataTypeField.ToString();
            }
            set
            {
#if SYM_DEF_110
                dataTypeField = (DataType2)Enum.Parse(typeof(DataType2), value);
#else
                dataTypeField = (DataType)Enum.Parse(typeof(DataType), value);
#endif
            }
        }
    }
 

    partial class ParameterDefinition : IParameterDefinition
    {
        IEnumerable<IParameter> IParameterDefinition.Parameter
        {
            get 
            {
                foreach (var p in this.Parameter)
                {
                    yield return p;
                }
            }
        }

        public void AddParameter(IParameter param)
        {
            var p = param as Parameter;
            if (p != null)
            {
                this.Parameter.Add(p);
            }
        }

        public void RemoveParameter(IParameter param)
        {
            var p = param as Parameter;
            if (p != null)
            {
                this.Parameter.Remove(p);
            }
        }
    }

    partial class LineUsage : ILineUsage
    {
        double? ILineUsage.StartOffset
        {
            get
            {
                double d;
                if (!double.TryParse(this.StartOffset, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.StartOffset = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.StartOffset = null;
            }
        }

        double? ILineUsage.EndOffset
        {
            get
            {
                double d;
                if (!double.TryParse(this.EndOffset, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.EndOffset = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.EndOffset = null;
            }
        }

        double? ILineUsage.Repeat
        {
            get
            {
                double d;
                if (!double.TryParse(this.Repeat, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Repeat = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.Repeat = null;
            }
        }

        double? ILineUsage.VertexAngleLimit
        {
            get
            {
                double d;
                if (!double.TryParse(this.VertexAngleLimit, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.VertexAngleLimit = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.VertexAngleLimit = null;
            }
        }

        IPath ILineUsage.DefaultPath
        {
            get
            {
                return this.DefaultPath;
            }
            set
            {
                this.DefaultPath = (Path)value;
            }
        }


        double? IUsageBase.Angle
        {
            get
            {
                double d;
                if (!double.TryParse(this.Angle, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Angle = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.Angle = null;
            }
        }
    }

    partial class PointUsage : IPointUsage
    {
        double? IPointUsage.OriginOffsetX
        {
            get
            {
                double d;
                if (!double.TryParse(this.OriginOffsetX, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.OriginOffsetX = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.OriginOffsetX = null;
            }
        }

        double? IPointUsage.OriginOffsetY
        {
            get
            {
                double d;
                if (!double.TryParse(this.OriginOffsetY, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.OriginOffsetY = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.OriginOffsetY = null;
            }
        }

        string IUsageBase.AngleControl
        {
            get
            {
                return this.AngleControl;
            }
            set
            {
                this.AngleControl = value;
            }
        }

        double? IUsageBase.Angle
        {
            get
            {
                double d;
                if (!double.TryParse(this.Angle, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Angle = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.Angle = null;
            }
        }
    }

    partial class AreaUsage : IAreaUsage
    {
        double? IAreaUsage.OriginX
        {
            get
            {
                double d;
                if (!double.TryParse(this.OriginX, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.OriginX = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.OriginX = null;
            }
        }

        double? IAreaUsage.OriginY
        {
            get
            {
                double d;
                if (!double.TryParse(this.OriginY, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.OriginY = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.OriginY = null;
            }
        }

        double? IAreaUsage.RepeatX
        {
            get
            {
                double d;
                if (!double.TryParse(this.RepeatX, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.RepeatX = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.RepeatX = null;
            }
        }

        double? IAreaUsage.RepeatY
        {
            get
            {
                double d;
                if (!double.TryParse(this.RepeatY, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.RepeatY = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.RepeatY = null;
            }
        }

        double? IAreaUsage.BufferWidth
        {
            get
            {
                double d;
                if (!double.TryParse(this.BufferWidth, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.BufferWidth = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.BufferWidth = null;
            }
        }

        double? IUsageBase.Angle
        {
            get
            {
                double d;
                if (!double.TryParse(this.Angle, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.Angle = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.Angle = null;
            }
        }
    }

    partial class Path : IPath
    {
        double? IPath.LineWeight
        {
            get
            {
                double d;
                if (!double.TryParse(this.LineWeight, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.LineWeight = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.LineWeight = null;
            }
        }

        bool? IPath.LineWeightScalable
        {
            get
            {
                bool b;
                if (!bool.TryParse(this.LineWeightScalable, out b))
                    return b;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.LineWeightScalable = value.Value ? "true" : "false";
                else
                    this.LineWeightScalable = null;
            }
        }

        double? IPath.LineMiterLimit
        {
            get
            {
                double d;
                if (!double.TryParse(this.LineMiterLimit, out d))
                    return d;
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.LineMiterLimit = value.Value.ToString(CultureInfo.InvariantCulture);
                else
                    this.LineMiterLimit = null;
            }
        }
    }

    partial class CompoundSymbolDefinition : ICompoundSymbolDefinition
    {
        public static CompoundSymbolDefinition CreateDefault()
        {
            var sym = new CompoundSymbolDefinition()
            {
                SimpleSymbol = new System.ComponentModel.BindingList<SimpleSymbol>(),
            };
            return sym;
        }

        private static readonly Version RES_VERSION = new Version(1, 0, 0);

        [XmlIgnore]
        public OSGeo.MapGuide.MaestroAPI.IServerConnection CurrentConnection
        {
            get;
            set;
        }

        private string _resId;

        [XmlIgnore]
        public string ResourceID
        {
            get
            {
                return _resId;
            }
            set
            {
                if (!ResourceIdentifier.Validate(value))
                    throw new InvalidOperationException("Not a valid resource identifier"); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.SymbolDefinition.ToString())
                    throw new InvalidOperationException("Invalid resource identifier for this type of object: " + res.Extension); //LOCALIZE

                _resId = value;
                this.OnPropertyChanged("ResourceID");
            }
        }

        [XmlIgnore]
        public ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.SymbolDefinition;
            }
        }

        [XmlIgnore]
        public Version ResourceVersion
        {
            get
            {
                return RES_VERSION;
            }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string ValidatingSchema
        {
            get { return "SymbolDefinition-1.0.0.xsd"; }
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        IEnumerable<ISimpleSymbolReferenceBase> ICompoundSymbolDefinition.SimpleSymbol
        {
            get 
            {
                foreach (var sym in this.SimpleSymbol)
                {
                    yield return sym;
                }
            }
        }

        void ICompoundSymbolDefinition.AddSimpleSymbol(ISimpleSymbolReferenceBase sym)
        {
            var symb = sym as SimpleSymbol;
            if (symb != null)
            {
                this.SimpleSymbol.Add(symb);
            }
        }

        void ICompoundSymbolDefinition.RemoveSimpleSymbol(ISimpleSymbolReferenceBase sym)
        {
            var symb = sym as SimpleSymbol;
            if (symb != null)
            {
                this.SimpleSymbol.Remove(symb);
            }
        }
    }

    partial class SimpleSymbol : ISimpleSymbolInlineReference, ISimpleSymbolLibraryReference
    {
        ISimpleSymbolDefinition ISimpleSymbolInlineReference.SimpleSymbolDefinition
        {
            get
            {
                return (ISimpleSymbolDefinition)this.Item;
            }
            set
            {
                this.Item = value;
            }
        }

        public SimpleSymbolReferenceType Type
        {
            get 
            {
                if (this.Item != null)
                {
                    if (typeof(ISimpleSymbolDefinition).IsAssignableFrom(this.Item.GetType()))
                        return SimpleSymbolReferenceType.Inline;
                    else if (typeof(string) == this.Item.GetType())
                        return SimpleSymbolReferenceType.Library;
                }
                return SimpleSymbolReferenceType.Undefined;
            }
        }

        string ISimpleSymbolLibraryReference.ResourceId
        {
            get
            {
                return (string)this.Item;
            }
            set
            {
                if (!ResourceIdentifier.Validate(value))
                    throw new InvalidOperationException("Not a valid resource identifier"); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.SymbolDefinition.ToString())
                    throw new InvalidOperationException("Invalid resource identifier for this type of object: " + res.Extension); //LOCALIZE

                this.Item = value;
                OnPropertyChanged("ResourceId");
            }
        }
    }
 
    partial class ImageReference : ISymbolLibraryReference
    {
        SymbolInstanceType ISymbolInstanceReference.Type
        {
            get { return SymbolInstanceType.Reference; }
        }
    }
}
