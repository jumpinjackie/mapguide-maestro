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
using System.Xml.Serialization;

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
    using OSGeo.MapGuide.ObjectModels.LayerDefinition;

    partial class GridLayerDefinitionType : IRasterLayerDefinition
    {
        [XmlIgnore]
        public override LayerType LayerType
        {
            get { return LayerType.Raster; }
        }

        [XmlIgnore]
        string ISubLayerDefinition.ResourceId
        {
            get { return this.ResourceId; }
            set { this.ResourceId = value; }
        }

        [XmlIgnore]
        string IRasterLayerDefinition.FeatureName
        {
            get
            {
                return this.FeatureName;
            }
            set
            {
                this.FeatureName = value;
            }
        }

        [XmlIgnore]
        string IRasterLayerDefinition.Geometry
        {
            get
            {
                return this.Geometry;
            }
            set
            {
                this.Geometry = value;
            }
        }

        [XmlIgnore]
        IEnumerable<IGridScaleRange> IRasterLayerDefinition.GridScaleRange
        {
            get 
            {
                foreach (var gsr in this.GridScaleRange)
                {
                    yield return gsr;
                }
            }
        }

        public void AddGridScaleRange(IGridScaleRange range)
        {
            var gsr = range as GridScaleRangeType;
            if (gsr != null)
            {
                this.GridScaleRange.Add(gsr);
            }
        }

        public void RemoveGridScaleRange(IGridScaleRange range)
        {
            var gsr = range as GridScaleRangeType;
            if (gsr != null)
            {
                this.GridScaleRange.Remove(gsr);
            }
        }

        public int IndexOfScaleRange(IGridScaleRange range)
        {
            var gsr = range as GridScaleRangeType;
            if (gsr != null)
            {
                this.GridScaleRange.IndexOf(gsr);
            }
            return -1;
        }

        public IGridScaleRange GetScaleRangeAt(int index)
        {
            return this.GridScaleRange[index];
        }

        [XmlIgnore]
        public int GridScaleRangeCount { get { return this.GridScaleRange.Count; } }
    }

    partial class ChannelBandType : IChannelBand
    {
        [XmlIgnore]
        string IChannelBand.Band
        {
            get
            {
                return this.Band;
            }
            set
            {
                this.Band = value;
            }
        }

        [XmlIgnore]
        double? IChannelBand.LowBand
        {
            get
            {
                return this.LowBandSpecified ? new Nullable<double>(this.LowBand) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.LowBand = value.Value;
                    this.LowBandSpecified = true;
                }
                else
                {
                    this.LowBandSpecified = false;
                }
            }
        }

        [XmlIgnore]
        double? IChannelBand.HighBand
        {
            get
            {
                return this.HighBandSpecified ? new Nullable<double>(this.HighBand) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.HighBand = value.Value;
                    this.HighBandSpecified = true;
                }
                else
                {
                    this.HighBandSpecified = false;
                }
            }
        }

        [XmlIgnore]
        byte IChannelBand.LowChannel
        {
            get
            {
                return this.LowChannel;
            }
            set
            {
                this.LowChannel = value;
            }
        }

        [XmlIgnore]
        byte IChannelBand.HighChannel
        {
            get
            {
                return this.HighChannel;
            }
            set
            {
                this.HighChannel = value;
            }
        }
    }

    partial class GridColorBandsType : IGridColorBands
    {
        [XmlIgnore]
        IChannelBand IGridColorBands.RedBand
        {
            get
            {
                return this.RedBand;
            }
            set
            {
                this.RedBand = (ChannelBandType)value;
            }
        }

        [XmlIgnore]
        IChannelBand IGridColorBands.GreenBand
        {
            get
            {
                return this.GreenBand;
            }
            set
            {
                this.GreenBand = (ChannelBandType)value;
            }
        }

        [XmlIgnore]
        IChannelBand IGridColorBands.BlueBand
        {
            get
            {
                return this.BlueBand;
            }
            set
            {
                this.BlueBand = (ChannelBandType)value;
            }
        }
    }

    //Class that addresses the shoddy impedence mismatch between the xsd
    //and the generated code
    internal abstract class ExplicitColorBase : IExplicitColor
    {
        [XmlIgnore]
        public abstract ItemChoiceType Type { get; }
    }

    //Class that addresses the shoddy impedence mismatch between the xsd
    //and the generated code
    internal class ExplicitColorBand : ExplicitColorBase, IExplicitColorBand
    {
        [XmlIgnore]
        public string Band { get; set; }

        [XmlIgnore]
        public override ItemChoiceType Type
        {
            get { return ItemChoiceType.Band; }
        }
    }

    //Class that addresses the shoddy impedence mismatch between the xsd
    //and the generated code
    internal class ExplicitColor : ExplicitColorBase, IExplictColorValue
    {
        [XmlIgnore]
        public string Value { get; set; }

        [XmlIgnore]
        public override ItemChoiceType Type
        {
            get { return ItemChoiceType.ExplicitColor; }
        }
    }

    //Class that addresses the shoddy impedence mismatch between the xsd
    //and the generated code
    internal class ExplicitColorBands : ExplicitColorBase, IExplicitColorBands
    {
        [XmlIgnore]
        public IGridColorBands Bands { get; set; }

        [XmlIgnore]
        public override ItemChoiceType Type
        {
            get { return ItemChoiceType.Bands; }
        }
    }


    partial class GridColorType : IGridColor
    {
        [XmlIgnore]
        IExplicitColor IGridColor.ExplicitColor
        {
            get
            {
                return ParseItem();
            }
            set
            {
                switch (this.ItemElementName)
                {
                    case ItemChoiceType.Band:
                        this.Item = ((ExplicitColorBand)value).Band;
                        break;
                    case ItemChoiceType.Bands:
                        this.Item = ((ExplicitColorBands)value).Bands;
                        break;
                    case ItemChoiceType.ExplicitColor:
                        this.Item = ((ExplicitColor)value).Value;
                        break;
                }
            }
        }

        private IExplicitColor ParseItem()
        {
            if (this.Item == null)
                return null;

            switch (this.ItemElementName)
            {
                case ItemChoiceType.Band:
                    return new ExplicitColorBand() { Band = (string)this.Item };
                case ItemChoiceType.Bands:
                    return new ExplicitColorBands() { Bands = (IGridColorBands)this.Item };
                case ItemChoiceType.ExplicitColor:
                    return new ExplicitColor() { Value = (string)this.Item };
            }

            throw new Exception(); //Should never get here
        }

        public void SetValue(string htmlColor)
        {
            this.ItemElementName = ItemChoiceType.ExplicitColor;
            this.Item = htmlColor;
        }

        public string GetValue()
        {
            if (this.Item != null && this.ItemElementName == ItemChoiceType.ExplicitColor)
            {
                return this.Item.ToString();
            }
            return null;
        }
    }

    partial class GridColorRuleType : IGridColorRule
    {
        [XmlIgnore]
        string IGridColorRule.LegendLabel
        {
            get
            {
                return this.LegendLabel;
            }
            set
            {
                this.LegendLabel = value;
            }
        }

        [XmlIgnore]
        string IGridColorRule.Filter
        {
            get
            {
                return this.Filter;
            }
            set
            {
                this.Filter = value;
            }
        }

        [XmlIgnore]
        ITextSymbol IGridColorRule.Label
        {
            get
            {
                return this.Label;
            }
            set
            {
                this.Label = (TextSymbolType)value;
            }
        }

        [XmlIgnore]
        IGridColor IGridColorRule.Color
        {
            get
            {
                return this.Color;
            }
            set
            {
                this.Color = (GridColorType)value;
            }
        }
    }

    partial class HillShadeType : IHillShade
    {
        [XmlIgnore]
        string IHillShade.Band
        {
            get
            {
                return this.Band;
            }
            set
            {
                this.Band = value;
            }
        }

        [XmlIgnore]
        double IHillShade.Azimuth
        {
            get
            {
                return this.Azimuth;
            }
            set
            {
                this.Azimuth = value;
            }
        }

        [XmlIgnore]
        double IHillShade.Altitude
        {
            get
            {
                return this.Altitude;
            }
            set
            {
                this.Altitude = value;
            }
        }

        [XmlIgnore]
        double IHillShade.ScaleFactor
        {
            get
            {
                return this.ScaleFactor;
            }
            set
            {
                this.ScaleFactor = value;
            }
        }
    }

    partial class GridColorStyleType : IGridColorStyle
    {
        [XmlIgnore]
        IHillShade IGridColorStyle.HillShade
        {
            get
            {
                return this.HillShade;
            }
            set
            {
                this.HillShade = (HillShadeType)value;
            }
        }

        [XmlIgnore]
        string IGridColorStyle.TransparencyColor
        {
            get
            {
                return this.TransparencyColor == null ? string.Empty : this.TransparencyColor.ToString();
            }
            set
            {
                this.TransparencyColor = value;
            }
        }

        [XmlIgnore]
        double? IGridColorStyle.BrightnessFactor
        {
            get
            {
                return this.BrightnessFactor;
            }
            set
            {
                this.BrightnessFactor = value.HasValue ? value.Value : default(double);
            }
        }

        [XmlIgnore]
        double? IGridColorStyle.ContrastFactor
        {
            get
            {
                return this.ContrastFactor;
            }
            set
            {
                this.ContrastFactor = value.HasValue ? value.Value : default(double);
            }
        }

        [XmlIgnore]
        IEnumerable<IGridColorRule> IGridColorStyle.ColorRule
        {
            get 
            {
                foreach (var cr in this.ColorRule)
                {
                    yield return cr;
                }
            }
        }

        void IGridColorStyle.AddColorRule(IGridColorRule rule)
        {
            var cr = rule as GridColorRuleType;
            if (cr != null)
                this.ColorRule.Add((GridColorRuleType)cr);
        }


        void IGridColorStyle.RemoveColorRule(IGridColorRule rule)
        {
            var cr = rule as GridColorRuleType;
            if (cr != null)
                this.ColorRule.Remove((GridColorRuleType)cr);
        }

        [XmlIgnore]
        public int ColorRuleCount
        {
            get { return this.ColorRule.Count; }
        }

        public IGridColorRule GetColorRuleAt(int index)
        {
            return this.ColorRule[index];
        }

        public IHillShade CreateHillShade()
        {
            return new HillShadeType();
        }
    }

    partial class GridSurfaceStyleType : IGridSurfaceStyle
    {
        [XmlIgnore]
        string IGridSurfaceStyle.Band
        {
            get
            {
                return this.Band;
            }
            set
            {
                this.Band = value;
            }
        }

        [XmlIgnore]
        double IGridSurfaceStyle.ZeroValue
        {
            get
            {
                return this.ZeroValue;
            }
            set
            {
                this.ZeroValue = value;
            }
        }

        [XmlIgnore]
        double IGridSurfaceStyle.ScaleFactor
        {
            get
            {
                return this.ScaleFactor;
            }
            set
            {
                this.ScaleFactor = value;
            }
        }

        [XmlIgnore]
        string IGridSurfaceStyle.DefaultColor
        {
            get
            {
                return this.DefaultColor;
            }
            set
            {
                this.DefaultColor = value;
            }
        }
    }

    partial class GridScaleRangeType : IGridScaleRange
    {
        [XmlIgnore]
        double? IGridScaleRange.MinScale
        {
            get
            {
                return this.MinScale;
            }
            set
            {
                if (value.HasValue)
                    this.MinScale = value.Value;
            }
        }

        [XmlIgnore]
        double? IGridScaleRange.MaxScale
        {
            get
            {
                return this.MaxScaleSpecified ? new Nullable<double>(this.MaxScale) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.MaxScale = value.Value;
                    this.MaxScaleSpecified = true;
                }
                else
                {
                    this.MaxScaleSpecified = false;
                }
            }
        }

        [XmlIgnore]
        IGridSurfaceStyle IGridScaleRange.SurfaceStyle
        {
            get
            {
                return this.SurfaceStyle;
            }
            set
            {
                this.SurfaceStyle = (GridSurfaceStyleType)value;
            }
        }

        [XmlIgnore]
        IGridColorStyle IGridScaleRange.ColorStyle
        {
            get
            {
                return this.ColorStyle;
            }
            set
            {
                this.ColorStyle = (GridColorStyleType)value;
            }
        }

        [XmlIgnore]
        double IGridScaleRange.RebuildFactor
        {
            get
            {
                return this.RebuildFactor;
            }
            set
            {
                this.RebuildFactor = value;
            }
        }

        public IGridColorStyle CreateColorStyle()
        {
            return new GridColorStyleType()
            {
                ColorRule = new System.ComponentModel.BindingList<GridColorRuleType>(),
                HillShade = new HillShadeType()
            };
        }

        public IGridSurfaceStyle CreateSurfaceStyle()
        {
            return new GridSurfaceStyleType();
        }
    }

}
