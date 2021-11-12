#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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

using Newtonsoft.Json;
using OSGeo.FDO.Expressions;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maestro.MapPublisher.Common
{
    public class MrlStyleTranslator
    {
        enum TextPlacementKind
        {
            line,
            point
        }

        class StyleSet
        {
            public IAreaRule Area { get; set; }

            public IPointRule Point { get; set; }

            public ILineRule Line { get; set; }

            public string Filter { get; set; }

            public bool IsEmpty => Area == null && Point == null && Line == null;
        }

        readonly IVectorLayerDefinition _vl;

        public MrlStyleTranslator(IVectorLayerDefinition vl)
        {
            _vl = vl;
        }

        static StyleSet AddOrUpdateDict(Dictionary<string, StyleSet> dict, string filter)
        {
            if (!dict.ContainsKey(filter))
                dict[filter] = new StyleSet { Filter = filter };

            return dict[filter];
        }

        public async Task WriteVectorStyleDefnAsync(string variableName, StreamWriter sw)
        {
            var vsr = _vl.VectorScaleRange.FirstOrDefault();
            if (vsr != null)
            {
                var defaultStyle = new StyleSet();
                var styleDict = new Dictionary<string, StyleSet>();
                if (vsr.AreaStyle != null)
                {
                    foreach (var ar in vsr.AreaStyle.Rules)
                    {
                        if (!string.IsNullOrWhiteSpace(ar.Filter))
                        {
                            var xfilter = TryTranslateFilter(ar.Filter);
                            if (!string.IsNullOrEmpty(xfilter))
                            {
                                AddOrUpdateDict(styleDict, xfilter).Area = ar;
                            }
                        }
                        else
                        {
                            defaultStyle.Area = ar;
                        }
                    }
                }
                if (vsr.PointStyle != null)
                {
                    foreach (var pr in vsr.PointStyle.Rules)
                    {
                        if (!string.IsNullOrWhiteSpace(pr.Filter))
                        {
                            var xfilter = TryTranslateFilter(pr.Filter);
                            if (!string.IsNullOrEmpty(xfilter))
                            {
                                AddOrUpdateDict(styleDict, xfilter).Point = pr;
                            }
                        }
                        else
                        {
                            defaultStyle.Point = pr;
                        }
                    }
                }
                if (vsr.LineStyle != null)
                {
                    foreach (var lr in vsr.LineStyle.Rules)
                    {
                        if (!string.IsNullOrWhiteSpace(lr.Filter))
                        {
                            var xfilter = TryTranslateFilter(lr.Filter);
                            if (!string.IsNullOrEmpty(xfilter))
                            {
                                AddOrUpdateDict(styleDict, xfilter).Line = lr;
                            }
                        }
                        else
                        {
                            defaultStyle.Line = lr;
                        }
                    }
                }

                IDictionary<string, object> style = new ExpandoObject();
                if (!defaultStyle.IsEmpty)
                {
                    style["default"] = TranslateStyle(defaultStyle);
                }
                foreach (var kvp in styleDict)
                {
                    style[kvp.Key] = TranslateStyle(kvp.Value);
                }

                var json = JsonConvert.SerializeObject(style, Formatting.Indented);
                await sw.WriteLineAsync($"var {variableName} = {json};");
            }
        }

        private string TryTranslateFilter(string filter)
        {
            var pFilter = FdoFilter.Parse(filter);
            if (pFilter.FilterType == FilterType.ComparisonCondition)
            {
                var compFilter = (FdoComparisonCondition)pFilter;
                var lhs = compFilter.Left;
                var rhs = compFilter.Right;
                var op = compFilter.Operator;
                if (op == ComparisonOperations.EqualsTo)
                {
                    //HACK-ish: As our generated FDO parser only goes from [string] -> [expr/filter] and not
                    //the other way, we can't stitch together a fixed filter from the parsed parts. So we're
                    //blindly assuming that lhs and rhs won't have the '=' token and that the '=' token is
                    //solely the equals operator
                    return filter.Replace("=", "=="); //Replace with '=' as '==' is JS assignment when evaluated by mrl
                }
            }

            // For everyting else, we're assuming the filter is directly JS evaluatable and does not required any fixups
            return filter;
        }

        private dynamic TranslateStyle(StyleSet defaultStyle)
        {
            dynamic st = new ExpandoObject();
            
            if (defaultStyle.Area != null)
            {
                var asym = defaultStyle.Area.AreaSymbolization2D;
                if (asym != null)
                {
                    st.polygon = new ExpandoObject();
                    if (asym.Fill != null)
                    {
                        st.polygon.fill = new ExpandoObject();
                        ApplyFill(st.polygon.fill, asym.Fill);
                    }
                    if (asym.Stroke != null)
                    {
                        st.polygon.stroke = new ExpandoObject();
                        ApplyStroke(st.polygon.stroke, asym.Stroke);
                    }
                }
            }
            if (defaultStyle.Point != null)
            {
                var psym = defaultStyle.Point.PointSymbolization2D.Symbol;
                if (psym.Type == PointSymbolType.Mark)
                {
                    st.point = new ExpandoObject();
                    st.point.type = "Circle";

                    var msym = (IMarkSymbol)psym;
                    if (msym.Fill != null)
                    {
                        st.point.fill = new ExpandoObject();
                        ApplyFill(st.point.fill, msym.Fill);
                    }
                    if (msym.Edge != null)
                    {
                        st.point.stroke = new ExpandoObject();
                        ApplyStroke(st.point.stroke, msym.Edge);
                    }

                    st.point.radius = TryParseRadius(psym) ?? 1;
                }

                if (defaultStyle.Point.Label != null)
                {
                    st.point.label = new ExpandoObject();
                    ApplyLabel(st.point.label, defaultStyle.Point.Label, TextPlacementKind.point);
                }
                
            }
            if (defaultStyle.Line != null)
            {
                var lsym = defaultStyle.Line.Strokes.FirstOrDefault();
                if (lsym != null)
                {
                    st.line = new ExpandoObject();
                    ApplyStroke(st.line, lsym);
                    if (defaultStyle.Line.Label != null)
                    {
                        st.line.label = new ExpandoObject();
                        ApplyLabel(st.line.label, defaultStyle.Line.Label, TextPlacementKind.line);
                    }
                }
            }

            return st;
        }

        double? TryParseRadius(ISymbol sym)
        {
            var eX = FdoExpression.Parse(sym.SizeX);
            var eY = FdoExpression.Parse(sym.SizeY);

            double? nX = TryGetDoubleExprValue(eX);
            double? nY = TryGetDoubleExprValue(eY);
            if (nX.HasValue && nY.HasValue)
            {
                //Average it out
                return (nX.Value + nY.Value) / 2.0;
            }
            return null;
        }

        double? TryGetDoubleExprValue(FdoExpression expr)
        {
            switch (expr.ExpressionType)
            {
                case ExpressionType.DoubleValue:
                    return ((FdoDoubleValue)expr).Value;
                case ExpressionType.Int32Value:
                    return ((FdoInt32Value)expr).Value;
            }
            return null;
        }

        void ApplyFill(dynamic df, IFill fill)
        {
            var (c, a) = ArgbToRgba(fill.ForegroundColor);
            df.color = c;
            df.alpha = a;
        }

        void ApplyStroke(dynamic ds, IStroke stroke)
        {
            var (c, a) = ArgbToRgba(stroke.Color);
            ds.color = c;
            ds.alpha = a;

            var ew = FdoExpression.Parse(stroke.Thickness);
            ds.width = Math.Max(TryGetDoubleExprValue(ew) ?? 0, 0.1);
        }

        void ApplyLabel(dynamic lbl, ITextSymbol label, TextPlacementKind kind)
        {
            lbl.placement = kind.ToString();

            var eText = FdoExpression.Parse(label.Text);
            if (eText.ExpressionType == ExpressionType.Identifier)
            {
                lbl.text = new ExpandoObject();
                lbl.text.expr = label.Text;
            }
            else
            {
                lbl.text = label.Text;
            }

            var (size, units) = GetUnits(label);
            if (!size.HasValue || units == null)
            {
                size = 12;
                units = "px";
            }

            lbl.font = $"{size}{units} \"{label.FontName}\"";
            var eRot = FdoExpression.Parse(label.Rotation);
            lbl.rotation = TryGetDoubleExprValue(eRot);
            // TODO: More properties
        }

        private (double? size, string units) GetUnits(ITextSymbol label)
        {
            var eSize = FdoExpression.Parse(label.SizeY);
            var size = TryGetDoubleExprValue(eSize);
            if (label.SizeContext == SizeContextType.DeviceUnits)
            {
                switch (label.Unit)
                {
                    case LengthUnitType.Points:
                        return (size, "pt");
                }
            }
            return (null, null);
        }

        (string rgb, int alpha) ArgbToRgba(string argb)
        {
            var sAlpha = argb.Substring(0, 2);
            var sRgb = "#" + argb.Substring(2);

            return (sRgb, Convert.ToInt32($"0x{sAlpha}", 16));
        }
    }
}
