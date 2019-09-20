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
    public class LeafletStyleTranslator : StyleTranslatorBase
    {
        readonly string _featureVarName;

        public LeafletStyleTranslator(string featureVarName)
        {
            _featureVarName = featureVarName;
        }

        public string FeatureVariableName => _featureVarName;

        public async Task WritePointMarker(string varName, IVectorScaleRange vsr, StreamWriter sw)
        {
            //TODO: This should be offloaded to LeafletStyleTranslator
            await sw.WriteLineAsync("//Point feature function");
            if (vsr.PointStyle != null && vsr.PointStyle.RuleCount > 0)
            {
                var pr = vsr.PointStyle.Rules.First();
                if (pr.PointSymbolization2D != null && pr.PointSymbolization2D.Symbol is IMarkSymbol msm)
                {
                    await sw.WriteLineAsync($"var {varName}_style_point = function(point, latlng) {{;");
                    switch (msm.Shape)
                    {
                        case ShapeType.Circle:
                            await sw.WriteLineAsync("    return L.circleMarker(latlng);");
                            break;
                        default:
                            await sw.WriteLineAsync("    return L.marker(latlng);");
                            break;
                    }
                    await sw.WriteLineAsync("};");
                }
                else
                {
                    await sw.WriteLineAsync($"var {varName}_style_point = null;");
                }
            }
            else
            {
                await sw.WriteLineAsync($"var {varName}_style_point = null;");
            }
        }

        public async Task WritePointStyleFunctionAsync(StreamWriter sw, IVectorLayerDefinition vl, IPointVectorStyle pointStyle)
        {
            await sw.WriteLineAsync(INDENT + "var fillColor;");
            await sw.WriteLineAsync(INDENT + "var edgeColor;");
            await sw.WriteLineAsync(INDENT + "var edgeThickness;");
            await sw.WriteLineAsync(INDENT + "var pointRadius = 10;");

            var sb = new StringBuilder(512);
            var defaultRuleBlock = "";
            var themes = new List<(string conditon, string filter, string block)>();

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
            await sw.WriteLineAsync(INDENT + INDENT + $"fillColor: fillColor,");
            await sw.WriteLineAsync(INDENT + INDENT + $"color: edgeColor, weight: edgeThickness");
            await sw.WriteLineAsync(INDENT + "}");
            await sw.WriteLineAsync(INDENT + "return style;");
        }

        public async Task WriteLineStyleFunctionAsync(StreamWriter sw, IVectorLayerDefinition vl, ILineVectorStyle lineStyle)
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
            await sw.WriteLineAsync(INDENT + INDENT + $"color: edgeColor, weight: edgeThickness");
            await sw.WriteLineAsync(INDENT + "}");
            await sw.WriteLineAsync(INDENT + "return style;");
        }

        public async Task WritePolygonStyleFunctionAsync(StreamWriter sw, IVectorLayerDefinition vl, IAreaVectorStyle areaStyle)
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
            await sw.WriteLineAsync(INDENT + INDENT + $"opacity: 1, fillOpacity: 1,");
            await sw.WriteLineAsync(INDENT + INDENT + $"fillColor: fillColor,");
            await sw.WriteLineAsync(INDENT + INDENT + $"color: edgeColor, weight: Math.max(edgeThickness, 1)");
            await sw.WriteLineAsync(INDENT + "}");
            await sw.WriteLineAsync(INDENT + "return style;");
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

        protected override string TryTranslateIdentifier(FdoIdentifier ident, string featureVar)
            => $"({featureVar}.properties.{ident.Name})";
    }
}
