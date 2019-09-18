#region Disclaimer / License

// Copyright (C) 2019, Jackie Ng
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

using OSGeo.FDO.Expressions;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestro.StaticMapPublisher.Common
{
    public class OLStyleTranslator
    {
        readonly string _featureVarName;

        public OLStyleTranslator(string featureVarName)
        {
            _featureVarName = featureVarName;
        }

        public string FeatureVariableName => _featureVarName;

        public async Task WriteOLPointStyleFunctionAsync(StreamWriter sw, IVectorLayerDefinition vl, IPointVectorStyle pointStyle)
        {
            await sw.WriteLineAsync(INDENT + "var fillColor;");
            await sw.WriteLineAsync(INDENT + "var edgeColor;");
            await sw.WriteLineAsync(INDENT + "var edgeThickness;");
            await sw.WriteLineAsync(INDENT + "var pointRadius = 10;");

            var sb = new StringBuilder(512);
            var defaultRuleBlock = "";
            var themes = new List<(string conditon, string filter, string block)>();

            await sw.WriteLineAsync(INDENT + "var pointStyle = OLPointCircle;");

            foreach (var rule in pointStyle.Rules)
            {
                if (rule.PointSymbolization2D != null)
                {
                    if (string.IsNullOrEmpty(rule.Filter)) //No filter = default rule effectively
                    {
                        if (string.IsNullOrEmpty(defaultRuleBlock))
                        {
                            BuildPointRuleAssignment(sb, rule.PointSymbolization2D.Symbol as IMarkSymbol);
                            defaultRuleBlock = sb.ToString();
                        }
                    }
                    else
                    {
                        //Parse this filter and see if it's translateable
                        var filter = FdoFilter.Parse(rule.Filter);
                        var check = TryTranslateFdoFilter(filter, _featureVarName);
                        BuildPointRuleAssignment(sb, rule.PointSymbolization2D.Symbol as IMarkSymbol);
                        themes.Add((check, rule.Filter, sb.ToString()));
                    }
                }
            }

            //Now write out the theme
            if (themes.Count > 0)
            {
                bool bFirst = true;
                foreach (var th in themes)
                {
                    if (bFirst)
                    {
                        await sw.WriteLineAsync(INDENT + $"if ({th.conditon}) {{ // Translated from rule filter: {th.filter}");
                        await sw.WriteLineAsync(th.block);
                        await sw.WriteLineAsync(INDENT + "}");
                    }
                    else
                    {
                        await sw.WriteLineAsync(INDENT + $"else if ({th.conditon}) {{  // Translated from rule filter: {th.filter}");
                        await sw.WriteLineAsync(th.block);
                        await sw.WriteLineAsync(INDENT + "}");
                    }

                    bFirst = false;
                }

                if (!string.IsNullOrEmpty(defaultRuleBlock))
                {
                    await sw.WriteLineAsync(INDENT + $"else {{  // Default rule");
                    await sw.WriteLineAsync(defaultRuleBlock);
                    await sw.WriteLineAsync(INDENT + "}");
                }
            }
            else
            {
                await sw.WriteLineAsync("//Default rule");
                await sw.WriteLineAsync(defaultRuleBlock);
            }

            await sw.WriteLineAsync(INDENT + "var style = {");
            await sw.WriteLineAsync(INDENT + INDENT + "radius: pointRadius,");
            await sw.WriteLineAsync(INDENT + INDENT + $"fill: new ol.style.Fill({{ color: fillColor }}),");
            await sw.WriteLineAsync(INDENT + INDENT + $"stroke: new ol.style.Stroke({{ color: edgeColor, width: edgeThickness }})");
            await sw.WriteLineAsync(INDENT + "}");
            await sw.WriteLineAsync(INDENT + "return new ol.style.Style({ image: pointStyle(style) });");
        }

        public async Task WriteOLLineStyleFunctionAsync(StreamWriter sw, IVectorLayerDefinition vl, ILineVectorStyle lineStyle)
        {
            await sw.WriteLineAsync(INDENT + "var edgeColor;");
            await sw.WriteLineAsync(INDENT + "var edgeThickness;");

            var sb = new StringBuilder(512);
            var defaultRuleBlock = "";
            var themes = new List<(string conditon, string filter, string block)>();

            foreach (var rule in lineStyle.Rules)
            {
                if (rule.StrokeCount > 0)
                {
                    if (string.IsNullOrEmpty(rule.Filter)) //No filter = default rule effectively
                    {
                        if (string.IsNullOrEmpty(defaultRuleBlock))
                        {
                            BuildLineRuleAssignment(sb, rule);
                            defaultRuleBlock = sb.ToString();
                        }
                    }
                    else
                    {
                        //Parse this filter and see if it's translateable
                        var filter = FdoFilter.Parse(rule.Filter);
                        var check = TryTranslateFdoFilter(filter, _featureVarName);
                        BuildLineRuleAssignment(sb, rule);
                        themes.Add((check, rule.Filter, sb.ToString()));
                    }
                }
            }

            //Now write out the theme
            if (themes.Count > 0)
            {
                bool bFirst = true;
                foreach (var th in themes)
                {
                    if (bFirst)
                    {
                        await sw.WriteLineAsync(INDENT + $"if ({th.conditon}) {{ // Translated from rule filter: {th.filter}");
                        await sw.WriteLineAsync(th.block);
                        await sw.WriteLineAsync(INDENT + "}");
                    }
                    else
                    {
                        await sw.WriteLineAsync(INDENT + $"else if ({th.conditon}) {{  // Translated from rule filter: {th.filter}");
                        await sw.WriteLineAsync(th.block);
                        await sw.WriteLineAsync(INDENT + "}");
                    }

                    bFirst = false;
                }

                if (!string.IsNullOrEmpty(defaultRuleBlock))
                {
                    await sw.WriteLineAsync(INDENT + $"else {{  // Default rule");
                    await sw.WriteLineAsync(defaultRuleBlock);
                    await sw.WriteLineAsync(INDENT + "}");
                }
            }
            else
            {
                await sw.WriteLineAsync("//Default rule");
                await sw.WriteLineAsync(defaultRuleBlock);
            }

            await sw.WriteLineAsync(INDENT + "var style = {");
            await sw.WriteLineAsync(INDENT + INDENT + $"stroke: new ol.style.Stroke({{ color: edgeColor, width: edgeThickness }})");
            await sw.WriteLineAsync(INDENT + "}");
            await sw.WriteLineAsync(INDENT + "return new ol.style.Style(style);");
        }

        const string INDENT = "    ";

        public async Task WriteOLPolygonStyleFunctionAsync(StreamWriter sw, IVectorLayerDefinition vl, IAreaVectorStyle areaStyle)
        {
            await sw.WriteLineAsync(INDENT + "var fillColor;");
            await sw.WriteLineAsync(INDENT + "var edgeColor;");
            await sw.WriteLineAsync(INDENT + "var edgeThickness;");

            var sb = new StringBuilder(512);
            var defaultRuleBlock = "";
            var themes = new List<(string conditon, string filter, string block)>();

            foreach (var rule in areaStyle.Rules)
            {
                if (rule.AreaSymbolization2D != null)
                {
                    if (string.IsNullOrEmpty(rule.Filter)) //No filter = default rule effectively
                    {
                        if (string.IsNullOrEmpty(defaultRuleBlock))
                        {
                            BuildPolygonRuleAssignment(sb, rule);
                            defaultRuleBlock = sb.ToString();
                        }
                    }
                    else
                    {
                        //Parse this filter and see if it's translateable
                        var filter = FdoFilter.Parse(rule.Filter);
                        var check = TryTranslateFdoFilter(filter, _featureVarName);
                        BuildPolygonRuleAssignment(sb, rule);
                        themes.Add((check, rule.Filter, sb.ToString()));
                    }
                }
            }

            //Now write out the theme
            if (themes.Count > 0)
            {
                bool bFirst = true;
                foreach (var th in themes)
                {
                    if (bFirst)
                    {
                        await sw.WriteLineAsync(INDENT + $"if ({th.conditon}) {{ // Translated from rule filter: {th.filter}");
                        await sw.WriteLineAsync(th.block);
                        await sw.WriteLineAsync(INDENT + "}");
                    }
                    else
                    {
                        await sw.WriteLineAsync(INDENT + $"else if ({th.conditon}) {{  // Translated from rule filter: {th.filter}");
                        await sw.WriteLineAsync(th.block);
                        await sw.WriteLineAsync(INDENT + "}");
                    }

                    bFirst = false;
                }

                if (!string.IsNullOrEmpty(defaultRuleBlock))
                {
                    await sw.WriteLineAsync(INDENT + $"else {{  // Default rule");
                    await sw.WriteLineAsync(defaultRuleBlock);
                    await sw.WriteLineAsync(INDENT + "}");
                }
            }
            else
            {
                await sw.WriteLineAsync("//Default rule");
                await sw.WriteLineAsync(defaultRuleBlock);
            }

            await sw.WriteLineAsync(INDENT + "var style = {");
            await sw.WriteLineAsync(INDENT + INDENT + $"fill: new ol.style.Fill({{ color: fillColor }}),");
            await sw.WriteLineAsync(INDENT + INDENT + $"stroke: new ol.style.Stroke({{ color: edgeColor, width: edgeThickness }})");
            await sw.WriteLineAsync(INDENT + "}");
            await sw.WriteLineAsync(INDENT + "return new ol.style.Style(style);");
        }

        private static void BuildPointRuleAssignment(StringBuilder sb, IMarkSymbol sym)
        {
            sb.Clear();
            if (sym.Fill != null)
            {
                sb.Append(INDENT + INDENT).AppendLine($"fillColor = '#{ToHtmlColor(sym.Fill.ForegroundColor)}';");
            }
            if (sym.Edge != null)
            {
                sb.Append(INDENT + INDENT).AppendLine($"edgeColor = '#{ToHtmlColor(sym.Edge.Color)}';");
                sb.Append(INDENT + INDENT).AppendLine($"edgeThickness = '{sym.Edge.Thickness}';");
            }
            switch (sym.Shape)
            {
                case ShapeType.Circle:
                    sb.Append(INDENT + INDENT).AppendLine($"pointStyle = OLPointCircle");
                    break;
                case ShapeType.Cross:
                    sb.Append(INDENT + INDENT).AppendLine($"pointStyle = OLPointCross");
                    break;
                case ShapeType.Square:
                    sb.Append(INDENT + INDENT).AppendLine($"pointStyle = OLPointSquare");
                    break;
                case ShapeType.Star:
                    sb.Append(INDENT + INDENT).AppendLine($"pointStyle = OLPointStar");
                    break;
                case ShapeType.Triangle:
                    sb.Append(INDENT + INDENT).AppendLine($"pointStyle = OLPointTriangle");
                    break;
                case ShapeType.X:
                    sb.Append(INDENT + INDENT).AppendLine($"pointStyle = OLPointX");
                    break;
            }
            
        }

        static string ToHtmlColor(string color)
        {
            return color.Substring(2);
        }

        private static void BuildLineRuleAssignment(StringBuilder sb, ILineRule rule)
        {
            sb.Clear();
            //First stroke wins. Not supporting composite line styles
            var stroke = rule.Strokes.First();
            sb.Append(INDENT + INDENT).AppendLine($"edgeColor = '#{ToHtmlColor(stroke.Color)}';");
            sb.Append(INDENT + INDENT).AppendLine($"edgeThickness = '{stroke.Thickness}';");
        }

        private static void BuildPolygonRuleAssignment(StringBuilder sb, IAreaRule rule)
        {
            sb.Clear();
            if (rule.AreaSymbolization2D.Fill != null)
            {
                sb.Append(INDENT + INDENT).AppendLine($"fillColor = '#{ToHtmlColor(rule.AreaSymbolization2D.Fill.ForegroundColor)}';");
            }
            if (rule.AreaSymbolization2D.Stroke != null)
            {
                sb.Append(INDENT + INDENT).AppendLine($"edgeColor = '#{ToHtmlColor(rule.AreaSymbolization2D.Stroke.Color)}';");
                sb.Append(INDENT + INDENT).AppendLine($"edgeThickness = '{rule.AreaSymbolization2D.Stroke.Thickness}';");
            }
        }

        private static string TryTranslateExpression(FdoExpression expr, string featureVar)
        {
            switch (expr)
            {
                case FdoIdentifier ident:
                    return $"({featureVar}.get('{ident.Name}'))";
                case FdoStringValue strVal:
                    return $"('{strVal.Value}')";
                case FdoInt32Value ival:
                    return $"{ival.Value}";
                case FdoDoubleValue dval:
                    return $"{dval.Value}";
                case FdoBooleanValue bval:
                    return bval.Value ? "true" : "false";
            }
            return null;
        }

        private static string TryTranslateComparsionOperator(ComparisonOperations op)
        {
            switch (op)
            {
                case ComparisonOperations.EqualsTo:
                    return " == ";
                case ComparisonOperations.GreaterThan:
                    return " > ";
                case ComparisonOperations.GreaterThanOrEqualTo:
                    return " >= ";
                case ComparisonOperations.LessThan:
                    return " < ";
                case ComparisonOperations.LessThanOrEqualTo:
                    return " <= ";
                case ComparisonOperations.Like:
                    return null;
                case ComparisonOperations.NotEqualsTo:
                    return " != ";
            }

            return null;
        }

        private static string TryTranslateFdoFilter(FdoFilter filter, string featureVar)
        {
            switch (filter)
            {
                case FdoComparisonCondition cmpCond:
                    {
                        var lhs = TryTranslateExpression(cmpCond.Left, featureVar);
                        var rhs = TryTranslateExpression(cmpCond.Right, featureVar);
                        var op = TryTranslateComparsionOperator(cmpCond.Operator);
                        if (lhs != null && rhs != null && op != null)
                        {
                            return lhs + op + rhs;
                        }
                        return null;
                    }
                default:
                    return null;
            }
        }
    }
}
