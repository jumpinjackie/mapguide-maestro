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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

#pragma warning disable 1591, 0114, 0108

#if LDF_110
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_1_1_0
#elif LDF_120
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_1_2_0
#elif LDF_130
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_1_3_0
#elif LDF_230
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_2_3_0
#elif LDF_240
namespace OSGeo.MapGuide.ObjectModels.LayerDefinition_2_4_0
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
            var ldf = (ILayerDefinition)res;
            var vl = ldf.SubLayer as IVectorLayerDefinition;
            if (vl != null)
            {
                foreach (var vsr in vl.VectorScaleRange)
                {
                    var vsr2 = vsr as IVectorScaleRange2;
                    if (vsr2 != null)
                    {
                        var ctss = vsr2.CompositeStyle;
                        if (ctss != null)
                        {
                            foreach (var cts in ctss)
                            {
                                foreach (var crs in cts.CompositeRule)
                                {
                                    var csym = crs.CompositeSymbolization;
                                    if (csym != null)
                                    {
                                        foreach (var si in csym.SymbolInstance)
                                        {
                                            if (si.Reference.Type == OSGeo.MapGuide.ObjectModels.SymbolDefinition.SymbolInstanceType.Inline)
                                            {
                                                var symBase = ((ISymbolInstanceReferenceInline)si.Reference).SymbolDefinition;
                                                symBase.RemoveSchemaAttributes();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

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
            var vl = new VectorLayerDefinitionType()
            {
#if LDF_240
                Watermarks = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.WatermarkDefinition_2_4_0.WatermarkType>()
#elif LDF_230
                Watermarks = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.WatermarkDefinition_2_3_0.WatermarkType>()
#endif
            };

            //TODO: Create composite type style if 1.2 or 1.3 schema

            vl.VectorScaleRange = new System.ComponentModel.BindingList<VectorScaleRangeType>();
            var defaultRange = new VectorScaleRangeType()
            {
                Items = new System.ComponentModel.BindingList<object>(),
                AreaStyle = CreateDefaultAreaStyle(),
                LineStyle = CreateDefaultLineStyle(),
                PointStyle = CreateDefaultPointStyle(),
#if LDF_100 || LDF_110
#else
                CompositeStyle = new ICompositeTypeStyle[] { CreateDefaultCompositeStyle() }
#endif
            };
            vl.VectorScaleRange.Add(defaultRange);

            this.Item = vl;
        }

        public void CreateRasterLayer()
        {
            var gl = new GridLayerDefinitionType()
            {
                GridScaleRange = new System.ComponentModel.BindingList<GridScaleRangeType>(),
            };

            gl.AddGridScaleRange(new GridScaleRangeType()
            {
                ColorStyle = new GridColorStyleType()
                {
                    ColorRule = new System.ComponentModel.BindingList<GridColorRuleType>() 
                    { 
                        new GridColorRuleType() { 
                            LegendLabel = string.Empty,
                            Color = new GridColorType() 
                        },
                        new GridColorRuleType() { 
                            LegendLabel = string.Empty,
                            Color = new GridColorType()
                        }
                    }
                },
                RebuildFactor = 1.0
            });

            gl.GetScaleRangeAt(0).ColorStyle.GetColorRuleAt(0).Color.SetValue("000000"); //NOXLATE
            gl.GetScaleRangeAt(0).ColorStyle.GetColorRuleAt(1).Color.SetValue("FFFFFF"); //NOXLATE

            this.Item = gl;
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
                LineStyle = "Solid", //NOXLATE
                Thickness = "1", //NOXLATE
                Unit = LengthUnitType.Points
            };
        }

        public IStroke CreateDefaultStroke()
        {
            return CreateStroke(Color.Black);
        }

        public IPointVectorStyle CreateDefaultPointStyle()
        {
            IPointVectorStyle pts = new PointTypeStyleType()
            {
                PointRule = new System.ComponentModel.BindingList<PointRuleType>()
            };
            pts.AddRule(CreateDefaultPointRule());
            return pts;
        }

        public ILineVectorStyle CreateDefaultLineStyle()
        {
            ILineVectorStyle lts = new LineTypeStyleType()
            {
                LineRule = new System.ComponentModel.BindingList<LineRuleType>()
            };
            lts.AddRule(CreateDefaultLineRule());
            return lts;
        }

        public IAreaVectorStyle CreateDefaultAreaStyle()
        {
            IAreaVectorStyle ats = new AreaTypeStyleType()
            {
                AreaRule = new System.ComponentModel.BindingList<AreaRuleType>()
            };
            ats.AddRule(CreateDefaultAreaRule());
            return ats;
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
            return CreateFill("Solid", Color.White, Color.White); //NOXLATE
        }

        public IMarkSymbol CreateDefaultMarkSymbol()
        {
            IMarkSymbol sym = new MarkSymbolType()
            {
                
                SizeContext = SizeContextType.DeviceUnits,
                MaintainAspect = true,
                Shape = ShapeType.Square,
                Rotation = "0", //NOXLATE
                SizeX = "10", //NOXLATE
                SizeY = "10", //NOXLATE
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
                FontName = "Arial", //NOXLATE
                Rotation = "0", //NOXLATE
                SizeX = "10", //NOXLATE
                SizeY = "10", //NOXLATE
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
                LegendLabel = string.Empty
            };
            pr.PointSymbolization2D = CreateDefaultPointSymbolization2D();
            return pr;
        }

        public IAreaRule CreateDefaultAreaRule()
        {
            IAreaRule ar = new AreaRuleType()
            {
                LegendLabel = string.Empty
            };
            ar.AreaSymbolization2D = CreateDefaultAreaSymbolizationFill();
            return ar;
        }

        public IAreaSymbolizationFill CreateDefaultAreaSymbolizationFill()
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

        public IAdvancedPlacement CreateDefaultAdvancedPlacement(double scaleLimit)
        {
            return new TextSymbolTypeAdvancedPlacement() { ScaleLimit = scaleLimit };
        }

        public ITextSymbol CreateDefaultTextSymbol()
        {
            return new TextSymbolType()
            {
                AdvancedPlacement = null,
                BackgroundColor = Utility.SerializeHTMLColor(Color.White, true),
                BackgroundStyle = BackgroundStyleType.Transparent,
                Bold = "false", //NOXLATE
                FontName = "Arial", //NOXLATE
                ForegroundColor = Utility.SerializeHTMLColor(Color.Black, true),
                HorizontalAlignment = "'Center'", //NOXLATE
                Italic = "false", //NOXLATE
                Rotation = "0", //NOXLATE
                SizeContext = SizeContextType.DeviceUnits,
                SizeX = "10", //NOXLATE
                SizeY = "10", //NOXLATE
                Text = string.Empty,
                Underlined = "false", //NOXLATE
                Unit = LengthUnitType.Points,
                VerticalAlignment = "'Baseline'" //NOXLATE
            };
        }


        public IW2DSymbol CreateDefaultW2DSymbol(string symbolLibrary, string symbolName)
        {
            return new W2DSymbolType()
            {
                W2DSymbol = new W2DSymbolTypeW2DSymbol()
                {
                    ResourceId = symbolLibrary,
                    LibraryItemName = symbolName
                },
            };
        }

        public ICompositeRule CreateDefaultCompositeRule()
        {
#if LDF_100
            throw new NotImplementedException();
#else
            return new CompositeRule()
            {
                LegendLabel = string.Empty,
                CompositeSymbolization = new CompositeSymbolization()
                {
                    SymbolInstance = new System.ComponentModel.BindingList<SymbolInstance>()  
                }
            };
#endif
        }


        public ICompositeTypeStyle CreateDefaultCompositeStyle()
        {
#if LDF_100
            throw new NotImplementedException();
#else
            var cts = new CompositeTypeStyle()
            {
                CompositeRule = new System.ComponentModel.BindingList<CompositeRule>(),
#if LDF_110 || LDF_120
#else
                ShowInLegend = true,
#endif
            };
            cts.AddCompositeRule(CreateDefaultCompositeRule());
            return cts;
#endif
        }

        public IUrlData CreateUrlData()
        {
#if LDF_240
            return new URLDataType();
#else
            throw new NotImplementedException();
#endif
        }
    }
}
