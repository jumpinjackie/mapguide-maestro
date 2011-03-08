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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.IO;

#pragma warning disable 1591, 0114, 0108

#if LDF_110
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_1_1_0
#elif LDF_120
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_1_2_0
#elif LDF_130
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_1_3_0
#else
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_1_0_0
#endif
{
    /// <summary>
    /// A publically accessible entry point primarily used for registration with the <see cref="ObjectFactory"/> and
    /// <see cref="ResourceTypeRegistry"/> classes
    /// </summary>
    public static class LdfEntryPoint
    {
        public static ILayerDefinition CreateDefault(LayerType type)
        {
            return LayerDefinition.CreateDefault(type);
        }

        public static IResource Deserialize(string xml)
        {
            return LayerDefinition.Deserialize(xml);
        }

        public static Stream Serialize(IResource res)
        {
            return res.SerializeToStream();
        }
    }

    partial class LayerDefinition : ILayerElementFactory
    {
        public static ILayerDefinition CreateDefault(LayerType type)
        {
            var lyr = new LayerDefinition();
            switch (type)
            { 
                case LayerType.Drawing:
                    lyr.CreateDrawingLayer();
                    break;
                case LayerType.Raster:
                    lyr.CreateRasterLayer();
                    break;
                case LayerType.Vector:
                    lyr.CreateVectorLayer();
                    break;
            }
            return lyr;
        }

        public void CreateVectorLayer()
        {
            var vl = new VectorLayerDefinitionType();

            vl.VectorScaleRange = new System.ComponentModel.BindingList<VectorScaleRangeType>();
            var defaultRange = new VectorScaleRangeType()
            {
                Items = new System.ComponentModel.BindingList<object>(),
                AreaStyle = CreateDefaultAreaStyle(),
                LineStyle = CreateDefaultLineStyle(),
                PointStyle = CreateDefaultPointStyle()
            };
            defaultRange.AreaStyle.AddRule(CreateDefaultAreaRule());
            defaultRange.LineStyle.AddRule(CreateDefaultLineRule());
            defaultRange.PointStyle.AddRule(CreateDefaultPointRule());
            vl.VectorScaleRange.Add(defaultRange);

            this.Item = vl;
        }

        public void CreateRasterLayer()
        {
            this.Item = new GridLayerDefinitionType();
        }

        public void CreateDrawingLayer()
        {
            this.Item = new DrawingLayerDefinitionType();
        }

        /// <summary>
        /// Creates a fill
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="background"></param>
        /// <param name="foreground"></param>
        /// <returns></returns>
        public IFill CreateFill(string pattern, System.Drawing.Color background, System.Drawing.Color foreground)
        {
            return new FillType()
            {
                BackgroundColor = Utility.SerializeHTMLColor(background, true),
                FillPattern = pattern,
                ForegroundColor = Utility.SerializeHTMLColor(foreground, true)
            };
        }

        /// <summary>
        /// Creates a line stroke with default settings
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public IStroke CreateStroke(System.Drawing.Color color)
        {
            return new StrokeType()
            {
                Color = Utility.SerializeHTMLColor(color, true),
                LineStyle = "Solid",
                Thickness = "1",
                Unit = LengthUnitType.Points
            };
        }

        public IStroke CreateDefaultStroke()
        {
            return CreateStroke(Color.Black);
        }

        public IPointVectorStyle CreateDefaultPointStyle()
        {
            return new PointTypeStyleType()
            {
                PointRule = new System.ComponentModel.BindingList<PointRuleType>()
            };
        }

        public ILineVectorStyle CreateDefaultLineStyle()
        {
            return new LineTypeStyleType()
            {
                LineRule = new System.ComponentModel.BindingList<LineRuleType>()
            };
        }

        public IAreaVectorStyle CreateDefaultAreaStyle()
        {
            return new AreaTypeStyleType()
            {
                AreaRule = new System.ComponentModel.BindingList<AreaRuleType>()
            };
        }

        public IVectorScaleRange CreateVectorScaleRange()
        {
            return new VectorScaleRangeType()
            {
                Items = new System.ComponentModel.BindingList<object>(),
                AreaStyle = CreateDefaultAreaStyle(),
                LineStyle = CreateDefaultLineStyle(),
                PointStyle = CreateDefaultPointStyle()
            };
        }

        public IFill CreateDefaultFill()
        {
            return CreateFill("Solid", Color.White, Color.White);
        }

        public IMarkSymbol CreateDefaultMarkSymbol()
        {
            IMarkSymbol sym = new MarkSymbolType()
            {
                
                SizeContext = SizeContextType.DeviceUnits,
                MaintainAspect = true,
                Shape = ShapeType.Square,
                Rotation = "0",
                SizeX = "10",
                SizeY = "10",
                Unit = LengthUnitType.Points
            };
            sym.Edge = CreateDefaultStroke();
            sym.Fill = CreateDefaultFill();
            return sym;
        }

        public IFontSymbol CreateDefaultFontSymbol()
        {
            IFontSymbol sym = new FontSymbolType()
            {
                SizeContext = SizeContextType.DeviceUnits,
                MaintainAspect = true,
                FontName = "Arial",
                Rotation = "0",
                SizeX = "10",
                SizeY = "10",
                Unit = LengthUnitType.Points
            };
            sym.SetForegroundColor(Color.Black);
            return sym;
        }

        public IPointSymbolization2D CreateDefaultPointSymbolization2D()
        {
            IPointSymbolization2D sym = new PointSymbolization2DType();
            sym.Symbol = CreateDefaultMarkSymbol();
            return sym;
        }

        public IPointRule CreateDefaultPointRule()
        {
            IPointRule pr = new PointRuleType()
            {
                LegendLabel = ""
            };
            pr.PointSymbolization2D = CreateDefaultPointSymbolization2D();
            return pr;
        }

        public IAreaRule CreateDefaultAreaRule()
        {
            IAreaRule ar = new AreaRuleType()
            {
                LegendLabel = ""
            };
            ar.AreaSymbolization2D = CreateDefaultAreaSymbolization2D();
            return ar;
        }

        private IAreaSymbolizationFill CreateDefaultAreaSymbolization2D()
        {
            IAreaSymbolizationFill fill = new AreaSymbolizationFillType();
            fill.Fill = CreateDefaultFill();
            fill.Stroke = CreateDefaultStroke();

            return fill;
        }

        public ILineRule CreateDefaultLineRule()
        {
            ILineRule lr = new LineRuleType()
            {
                LegendLabel = "",
                Items = new System.ComponentModel.BindingList<StrokeType>()
            };
            lr.AddStroke(CreateDefaultStroke());
            return lr;
        }

        public ITextSymbol CreateDefaultTextSymbol()
        {
            return new TextSymbolType()
            {
                AdvancedPlacement = null,
                BackgroundColor = Utility.SerializeHTMLColor(Color.White, true),
                BackgroundStyle = BackgroundStyleType.Transparent,
                Bold = "false",
                FontName = "Arial",
                ForegroundColor = Utility.SerializeHTMLColor(Color.Black, true),
                HorizontalAlignment = "'Center'",
                Italic = "false",
                Rotation = "0",
                SizeContext = SizeContextType.DeviceUnits,
                SizeX = "10",
                SizeY = "10",
                Text = "",
                Underlined = "false",
                Unit = LengthUnitType.Points,
                VerticalAlignment = "'Baseline'"
            };
        }
    }
}
