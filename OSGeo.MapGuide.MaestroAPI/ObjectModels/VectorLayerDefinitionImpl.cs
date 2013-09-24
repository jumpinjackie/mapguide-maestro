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
using OSGeo.MapGuide.ObjectModels.Common;
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System.Globalization;

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
    using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
    using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;

    abstract partial class BaseLayerDefinitionType : ISubLayerDefinition
#if LDF_230 || LDF_240
        , ISubLayerDefinition2
#endif
    {
        [XmlIgnore]
        public abstract LayerType LayerType { get; }

#if LDF_230 || LDF_240
        [XmlIgnore]
        IEnumerable<OSGeo.MapGuide.ObjectModels.WatermarkDefinition.IWatermark> IWatermarkCollection.Watermarks
        {
            get 
            {
                foreach (var wm in this.Watermarks)
                    yield return wm;
            }
        }

        IWatermark IWatermarkCollection.AddWatermark(OSGeo.MapGuide.ObjectModels.WatermarkDefinition.IWatermarkDefinition watermark)
        {
            return WatermarkCollectionUtil.AddWatermark(this.Watermarks, watermark);
        }

        void IWatermarkCollection.RemoveWatermark(OSGeo.MapGuide.ObjectModels.WatermarkDefinition.IWatermark watermark)
        {
            WatermarkCollectionUtil.RemoveWatermark(this.Watermarks, watermark);
        }

        [XmlIgnore]
        int IWatermarkCollection.WatermarkCount
        {
            get { return this.Watermarks.Count; }
        }
#endif
    }

    partial class NameStringPairType : INameStringPair
    {
        [XmlIgnore]
        string INameStringPair.Name
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        [XmlIgnore]
        string INameStringPair.Value
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }
    }

    partial class VectorScaleRangeType : IVectorScaleRange
#if !LDF_100
        , IVectorScaleRange2
#endif
    {
        #region Missing generated stuff
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool minScaleFieldSpecified;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinScaleSpecified
        {
            get
            {
                return this.minScaleFieldSpecified;
            }
            set
            {
                if ((minScaleFieldSpecified.Equals(value) != true))
                {
                    this.minScaleFieldSpecified = value;
                    this.OnPropertyChanged("MinScaleSpecified"); //NOXLATE
                }
            }
        }
        #endregion

        [XmlIgnore]
        public IAreaVectorStyle AreaStyle
        {
            get
            {
                foreach (var item in this.itemsField)
                {
                    if (typeof(IAreaVectorStyle).IsAssignableFrom(item.GetType()))
                        return (IAreaVectorStyle)item;
                }

                return null;
            }
            set
            {
                //Remove old one if it exists
                var item = this.AreaStyle;
                if (item != null)
                {
                    this.itemsField.Remove(item);
                }
                //Put the new one in if it is not null
                if (value != null)
                {
                    this.itemsField.Add(value);   
                }
            }
        }

        [XmlIgnore]
        public ILineVectorStyle LineStyle
        {
            get
            {
                foreach (var item in this.itemsField)
                {
                    if (typeof(ILineVectorStyle).IsAssignableFrom(item.GetType()))
                        return (ILineVectorStyle)item;
                }

                return null;
            }
            set
            {
                //Remove old one if it exists
                var item = this.LineStyle;
                if (item != null)
                {
                    this.itemsField.Remove(item);
                }
                //Put the new one in if it is not null
                if (value != null)
                {
                    this.itemsField.Add(value);
                }
            }
        }
        
        [XmlIgnore]
        public IPointVectorStyle PointStyle
        {
            get
            {
                foreach (var item in this.itemsField)
                {
                    if (typeof(IPointVectorStyle).IsAssignableFrom(item.GetType()))
                        return (IPointVectorStyle)item;
                }

                return null;
            }
            set
            {
                //Remove old one if it exists
                var item = this.PointStyle;
                if (item != null)
                {
                    this.itemsField.Remove(item);
                }
                //Put the new one in if it is not null
                if (value != null)
                {
                    this.itemsField.Add(value);
                }
            }
        }

        [XmlIgnore]
        double? IVectorScaleRange.MinScale
        {
            get
            {
                return this.MinScaleSpecified ? new Nullable<double>(this.MinScale) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.MinScaleSpecified = true;
                    this.MinScale = value.Value;
                }
                else
                {
                    this.MinScaleSpecified = false;
                }
            }
        }

        [XmlIgnore]
        double? IVectorScaleRange.MaxScale
        {
            get
            {
                return this.MaxScaleSpecified ? new Nullable<double>(this.MaxScale) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.MaxScaleSpecified = true;
                    this.MaxScale = value.Value;
                }
                else
                {
                    this.MaxScaleSpecified = false;
                }
            }
        }

        [XmlIgnore]
        public IList<IVectorStyle> AllStyles
        {
            get { return (IList<IVectorStyle>)this.Items; }
        }

        IVectorScaleRange IVectorScaleRange.Clone()
        {
            return VectorScaleRangeType.Deserialize(this.Serialize());
        }

#if LDF_110 || LDF_120
        [XmlIgnore]
        public IEnumerable<ICompositeTypeStyle> CompositeStyle
        {
            get
            {
                foreach (var item in this.itemsField)
                {
                    if (typeof(ICompositeTypeStyle).IsAssignableFrom(item.GetType()))
                        yield return (ICompositeTypeStyle)item;
                }
            }
            set
            {
                //Clear old ones
                var remove = new List<object>();
                foreach (var item in this.itemsField)
                {
                    if (typeof(ICompositeTypeStyle).IsAssignableFrom(item.GetType()))
                        remove.Add(item);
                }

                foreach(var obj in remove)
                {
                    this.itemsField.Remove(obj);
                }

                //Put the new ones in if it is not null
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        if (!typeof(ICompositeTypeStyle).IsAssignableFrom(item.GetType()))
                            throw new InvalidOperationException(Strings.ErrorObjectNotICompositeTypeStyle);

                        this.itemsField.Add(item);
                    }
                }
            }
        }
#elif LDF_130 || LDF_230 || LDF_240
        [XmlIgnore]
        public IEnumerable<ICompositeTypeStyle> CompositeStyle
        {
            get
            {
                foreach (var item in this.itemsField)
                {
                    if (typeof(ICompositeTypeStyle2).IsAssignableFrom(item.GetType()))
                        yield return (ICompositeTypeStyle2)item;
                }
            }
            set
            {
                //Clear old ones
                var remove = new List<object>();
                foreach (var item in this.itemsField)
                {
                    if (typeof(ICompositeTypeStyle2).IsAssignableFrom(item.GetType()))
                        remove.Add(item);
                }

                foreach(var obj in remove)
                {
                    this.itemsField.Remove(obj);
                }

                //Put the new ones in if it is not null
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        if (!typeof(ICompositeTypeStyle2).IsAssignableFrom(item.GetType()))
                            throw new InvalidOperationException(Strings.ErrorObjectNotICompositeTypeStyle);

                        this.itemsField.Add(item);
                    }
                }
            }
        }
#endif

#if LDF_100
#else
        IElevationSettings IVectorScaleRange2.ElevationSettings
        {
            get { return this.ElevationSettings; }
            set { this.ElevationSettings = (ElevationSettingsType)value; }
        }

        IElevationSettings IVectorScaleRange2.Create(string zOffset, string zExtrusion, ElevationTypeType zOffsetType, LengthUnitType unit)
        {
            return new ElevationSettingsType()
            {
                ZOffset = zOffset,
                ZOffsetType = zOffsetType,
                ZExtrusion = zExtrusion,
                Unit = unit
            };
        }

        [XmlIgnore]
        int IVectorScaleRange2.CompositeStyleCount
        {
            get
            {
                int count = 0;
                foreach (var item in this.itemsField)
                {
                    if (typeof(ICompositeTypeStyle).IsAssignableFrom(item.GetType()))
                        count++;
                }
                return count;
            }
        }
#endif
    }

#if LDF_100
#else
    partial class ElevationSettingsType : IElevationSettings { }
#endif

#if LDF_100
    partial class StrokeType : IStroke
#else
    partial class StrokeType : IStroke, IStroke2
#endif
    {
        internal StrokeType() { }

        IStroke ICloneableLayerElement<IStroke>.Clone()
        {
            return StrokeType.Deserialize(this.Serialize());
        }

#if LDF_100
#else
        IStroke2 ICloneableLayerElement<IStroke2>.Clone()
        {
            return StrokeType.Deserialize(this.Serialize());
        }
#endif
    }

    partial class FillType : IFill
    {
        internal FillType() { }

        IFill ICloneableLayerElement<IFill>.Clone()
        {
            return FillType.Deserialize(this.Serialize());
        }
    }

    partial class AreaTypeStyleType : IAreaVectorStyle
#if LDF_130 || LDF_230 || LDF_240
        , IAreaVectorStyle2
#endif
    {
        [XmlIgnore]
        IEnumerable<IAreaRule> IAreaVectorStyle.Rules
        {
            get 
            {
                foreach (var ar in this.AreaRule)
                {
                    yield return ar;
                }
            }
        }

        [XmlIgnore]
        int IVectorStyle.RuleCount 
        { 
            get 
            {
                if (this.AreaRule != null)
                    return this.AreaRule.Count;
                return 0;
            } 
        }

        void IAreaVectorStyle.RemoveAllRules()
        {
            this.AreaRule.Clear();
        }

        void IAreaVectorStyle.AddRule(IAreaRule rule)
        {
            var ar = rule as AreaRuleType;
            if (ar != null)
                this.AreaRule.Add(ar);
        }

        void IAreaVectorStyle.RemoveRule(IAreaRule rule)
        {
            var ar = rule as AreaRuleType;
            if (ar != null)
                this.AreaRule.Remove(ar);
        }

        [XmlIgnore]
        StyleType IVectorStyle.StyleType
        {
            get { return StyleType.Area; }
        }

        #region IRuleCollection<IAreaRule> Members

        public int IndexOfRule(IAreaRule rule)
        {
            var r = rule as AreaRuleType;
            if (r != null)
                return this.AreaRule.IndexOf(r);
            return -1;
        }

        public IAreaRule GetRuleAt(int index)
        {
            return this.AreaRule[index];
        }

        public bool MoveUp(IAreaRule rule)
        {
            var r = rule as AreaRuleType;
            if (r != null)
            {
                return CollectionUtil.MoveUp(this.AreaRule, r);
            }
            return false;
        }

        public bool MoveDown(IAreaRule rule)
        {
            var r = rule as AreaRuleType;
            if (r != null)
            {
                return CollectionUtil.MoveDown(this.AreaRule, r);
            }
            return false;
        }

        #endregion

        #region IRuleCollection Members
        IVectorRule IRuleCollection.GetRuleAt(int index)
        {
            return this.AreaRule[index];
        }

        public int IndexOfRule(IVectorRule rule)
        {
            var r = rule as AreaRuleType;
            if (r != null)
                return this.AreaRule.IndexOf(r);
            return -1;
        }

        public bool MoveUp(IVectorRule rule)
        {
            var r = rule as AreaRuleType;
            if (r != null)
            {
                return CollectionUtil.MoveUp(this.AreaRule, r);
            }
            return false;
        }

        public bool MoveDown(IVectorRule rule)
        {
            var r = rule as AreaRuleType;
            if (r != null)
            {
                return CollectionUtil.MoveDown(this.AreaRule, r);
            }
            return false;
        }
        #endregion
    }

    internal static class CollectionUtil
    {
        public static bool MoveUp<T>(IList<T> list, T obj)
        {
            var idx = list.IndexOf(obj);
            if (idx == 0) //Already at top
                return false;

            var newIdx = idx - 1;
            //Get object at index
            var obj2 = list[newIdx];
            //Swap them
            list[idx] = obj2;
            list[newIdx] = obj;

            return true;
        }

        public static bool MoveDown<T>(IList<T> list, T obj)
        {
            var idx = list.IndexOf(obj);
            if (idx == list.Count - 1) //Already at bottom
                return false;

            var newIdx = idx + 1;
            //Get object at index
            var obj2 = list[newIdx];
            //Swap them
            list[idx] = obj2;
            list[newIdx] = obj;

            return true;
        }
    }

    partial class AreaRuleType : IAreaRule
    {
        [XmlIgnore]
        IAreaSymbolizationFill IAreaRule.AreaSymbolization2D
        {
            get
            {
                return this.Item;
            }
            set
            {
                this.Item = (AreaSymbolizationFillType)value;
            }
        }

        [XmlIgnore]
        ITextSymbol IBasicVectorRule.Label
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
    }

    partial class PointTypeStyleType : IPointVectorStyle
#if LDF_130 || LDF_230 || LDF_240
        , IPointVectorStyle2
#endif
    {
        [XmlIgnore]
        public IEnumerable<IPointRule> Rules
        {
            get 
            { 
                foreach(var pr in this.PointRule)
                {
                    yield return pr;
                }
            }
        }

        [XmlIgnore]
        int IVectorStyle.RuleCount
        {
            get
            {
                if (this.PointRule != null)
                    return this.PointRule.Count;
                return 0;
            }
        }

        public void RemoveAllRules()
        {
            this.PointRule.Clear();
        }

        public void AddRule(IPointRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                this.PointRule.Add(pr);
        }

        public void RemoveRule(IPointRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                this.PointRule.Remove(pr);
        }

        [XmlIgnore]
        public StyleType StyleType
        {
            get { return StyleType.Point; }
        }

        #region IRuleCollection<IPointRule> Members

        public int IndexOfRule(IPointRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                return this.PointRule.IndexOf(pr);
            return -1;
        }

        public IPointRule GetRuleAt(int index)
        {
            return this.PointRule[index];
        }

        public bool MoveUp(IPointRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                return CollectionUtil.MoveUp(this.PointRule, pr);
            return false;
        }

        public bool MoveDown(IPointRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                return CollectionUtil.MoveDown(this.PointRule, pr);
            return false;
        }

        #endregion

        #region IRuleCollection Members
        IVectorRule IRuleCollection.GetRuleAt(int index)
        {
            return this.PointRule[index];
        }

        public int IndexOfRule(IVectorRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                return this.PointRule.IndexOf(pr);
            return -1;
        }

        public bool MoveUp(IVectorRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                return CollectionUtil.MoveUp(this.PointRule, pr);
            return false;
        }

        public bool MoveDown(IVectorRule rule)
        {
            var pr = rule as PointRuleType;
            if (pr != null)
                return CollectionUtil.MoveDown(this.PointRule, pr);
            return false;
        }
        #endregion
    }


    partial class PointRuleType : IPointRule
    {
        [XmlIgnore]
        ITextSymbol IBasicVectorRule.Label
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
        IPointSymbolization2D IPointRule.PointSymbolization2D
        {
            get { return this.Item; }
            set { this.Item = (PointSymbolization2DType)value; }
        }
    }

    partial class PointSymbolization2DType : IPointSymbolization2D
    {
        [XmlIgnore]
        ISymbol IPointSymbolization2D.Symbol
        {
            get { return (ISymbol)this.Item; }
            set { this.Item = (SymbolType)value; }
        }

        IPointSymbolization2D ICloneableLayerElement<IPointSymbolization2D>.Clone()
        {
            return PointSymbolization2DType.Deserialize(this.Serialize());
        }
    }

    partial class LineTypeStyleType : ILineVectorStyle
#if LDF_130 || LDF_230 || LDF_240
        , ILineVectorStyle2
#endif
    {
        [XmlIgnore]
        public IEnumerable<ILineRule> Rules
        {
            get
            {
                foreach (var lr in this.LineRule)
                {
                    yield return lr;
                }
            }
        }

        [XmlIgnore]
        int IVectorStyle.RuleCount
        {
            get
            {
                if (this.LineRule != null)
                    return this.LineRule.Count;
                return 0;
            }
        }

        public ILineRule GetRuleAt(int index)
        {
            return this.LineRule[index];
        }

        public void RemoveAllRules()
        {
            this.LineRule.Clear();
        }

        public void AddRule(ILineRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                this.LineRule.Add(lr);
        }

        public void RemoveRule(ILineRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                this.LineRule.Remove(lr);
        }

        [XmlIgnore]
        public StyleType StyleType
        {
            get { return StyleType.Line; }
        }

        #region IRuleCollection<ILineRule> Members

        public int IndexOfRule(ILineRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                return this.LineRule.IndexOf(lr);
            return -1;
        }

        public bool MoveUp(ILineRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                return CollectionUtil.MoveUp(this.LineRule, lr);
            return false;
        }

        public bool MoveDown(ILineRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                return CollectionUtil.MoveDown(this.LineRule, lr);
            return false;
        }

        #endregion

        #region IRuleCollection Members
        IVectorRule IRuleCollection.GetRuleAt(int index)
        {
            return this.LineRule[index];
        }

        public int IndexOfRule(IVectorRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                return this.LineRule.IndexOf(lr);
            return -1;
        }

        public bool MoveUp(IVectorRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                return CollectionUtil.MoveUp(this.LineRule, lr);
            return false;
        }

        public bool MoveDown(IVectorRule rule)
        {
            var lr = rule as LineRuleType;
            if (lr != null)
                return CollectionUtil.MoveDown(this.LineRule, lr);
            return false;
        }
        #endregion
    }

    partial class LineRuleType : ILineRule
    {
        [XmlIgnore]
        IEnumerable<IStroke> ILineRule.Strokes
        {
            get
            {
                foreach (var str in this.Items)
                {
                    yield return str;
                }
            }
        }

        [XmlIgnore]
        int ILineRule.StrokeCount
        {
            get { return this.Items.Count; }
        }

        void ILineRule.SetStrokes(IEnumerable<IStroke> strokes)
        {
            Check.NotNull(strokes, "strokes"); //NOXLATE
            this.Items.Clear();
            foreach (var stroke in strokes)
            {
                var st = stroke as StrokeType;
                if (st != null)
                    this.Items.Add(st);
            }
        }

        void ILineRule.AddStroke(IStroke stroke)
        {
            var st = stroke as StrokeType;
            if (st != null)
                this.Items.Add(st);
        }

        void ILineRule.RemoveStroke(IStroke stroke)
        {
            var st = stroke as StrokeType;
            if (st != null)
                this.Items.Remove(st);
        }

        [XmlIgnore]
        ITextSymbol IBasicVectorRule.Label
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
    }

    partial class TextSymbolType : ITextSymbol
    {
        [XmlIgnore]
        IAdvancedPlacement ITextSymbol.AdvancedPlacement
        {
            get
            {
                return this.AdvancedPlacement;
            }
            set
            {
                this.AdvancedPlacement = (TextSymbolTypeAdvancedPlacement)value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.Text
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.FontName
        {
            get
            {
                return this.FontName;
            }
            set
            {
                this.FontName = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.ForegroundColor
        {
            get
            {
                return this.ForegroundColor;
            }
            set
            {
                this.ForegroundColor = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.BackgroundColor
        {
            get
            {
                return this.BackgroundColor;
            }
            set
            {
                this.BackgroundColor = value;
            }
        }

        [XmlIgnore]
        BackgroundStyleType ITextSymbol.BackgroundStyle
        {
            get
            {
                return this.BackgroundStyle;
            }
            set
            {
                this.BackgroundStyle = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.HorizontalAlignment
        {
            get
            {
                return this.HorizontalAlignment;
            }
            set
            {
                this.HorizontalAlignment = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.VerticalAlignment
        {
            get
            {
                return this.VerticalAlignment;
            }
            set
            {
                this.VerticalAlignment = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.Bold
        {
            get
            {
                return this.Bold;
            }
            set
            {
                this.Bold = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.Italic
        {
            get
            {
                return this.Italic;
            }
            set
            {
                this.Italic = value;
            }
        }

        [XmlIgnore]
        string ITextSymbol.Underlined
        {
            get
            {
                return this.Underlined;
            }
            set
            {
                this.Underlined = value;
            }
        }

        [XmlIgnore]
        public override PointSymbolType Type
        {
            get { return PointSymbolType.Font; }
        }

        ITextSymbol ICloneableLayerElement<ITextSymbol>.Clone()
        {
            return TextSymbolType.Deserialize(this.Serialize());
        }
    }

    partial class MarkSymbolType : IMarkSymbol
    {
        [XmlIgnore]
        ShapeType IMarkSymbol.Shape
        {
            get
            {
                return this.Shape;
            }
            set
            {
                this.Shape = value;
            }
        }

        [XmlIgnore]
        IFill IMarkSymbol.Fill
        {
            get
            {
                return (IFill)this.Fill;
            }
            set
            {
                this.Fill = (FillType)value;
            }
        }

        [XmlIgnore]
        IStroke IMarkSymbol.Edge
        {
            get
            {
                return (IStroke)this.Edge;
            }
            set
            {
                this.Edge = (StrokeType)value;
            }
        }

        [XmlIgnore]
        public override PointSymbolType Type
        {
            get { return PointSymbolType.Mark; }
        }

        IMarkSymbol ICloneableLayerElement<IMarkSymbol>.Clone()
        {
            return MarkSymbolType.Deserialize(this.Serialize());
        }
    }

    internal class ImageBinaryContainer : IInlineImageSymbol
    {
        [XmlIgnore]
        public byte[] Content
        {
            get;
            set;
        }

        [XmlIgnore]
        public ImageSymbolReferenceType Type
        {
            get { return ImageSymbolReferenceType.Inline; }
        }

        IInlineImageSymbol ICloneableLayerElement<IInlineImageSymbol>.Clone()
        {
            byte[] array = null;
            if (this.Content != null)
            {
                array = new byte[this.Content.Length];
                Array.Copy(this.Content, array, this.Content.Length);
            }
            return new ImageBinaryContainer()
            {
                Content = array
            };
        }
    }


    partial class ImageSymbolType : IImageSymbol
    {
        [XmlIgnore]
        IBaseImageSymbol IImageSymbol.Image
        {
            get
            {
                IBaseImageSymbol img = null;
                if (this.Item == null)
                    return null;

                if (typeof(byte[]).IsAssignableFrom(this.Item.GetType()))
                    img = new ImageBinaryContainer() { Content = (byte[])this.Item };
                else
                    img = (ISymbolReference)this.Item;
                return img;
            }
            set
            {
                if (typeof(IInlineImageSymbol).IsAssignableFrom(value.GetType()))
                    this.Item = ((IInlineImageSymbol)value).Content;
                else
                    this.Item = (ImageSymbolTypeImage)value;
            }
        }

        [XmlIgnore]
        public override PointSymbolType Type
        {
            get { return PointSymbolType.Image; }
        }

        IImageSymbol ICloneableLayerElement<IImageSymbol>.Clone()
        {
            return ImageSymbolType.Deserialize(this.Serialize());
        }
    }

    abstract partial class SymbolType : ISymbol
    {
        [XmlIgnore]
        LengthUnitType ISymbol.Unit
        {
            get
            {
                return this.Unit;
            }
            set
            {
                this.Unit = value;
            }
        }

        [XmlIgnore]
        SizeContextType ISymbol.SizeContext
        {
            get
            {
                return this.SizeContext;
            }
            set
            {
                this.SizeContext = value;
            }
        }

        [XmlIgnore]
        bool ISymbol.MaintainAspect
        {
            get
            {
                return this.MaintainAspect;
            }
            set
            {
                this.MaintainAspect = true;
            }
        }

        [XmlIgnore]
        public abstract PointSymbolType Type { get; }

        [XmlIgnore]
        string ISymbol.InsertionPointY
        {
            get
            {
                return this.InsertionPointY.ToString();
            }
            set
            {
                this.InsertionPointY = Convert.ToDouble(value);
            }
        }
    }

    partial class FontSymbolType : IFontSymbol
    {
        [XmlIgnore]
        string IFontSymbol.FontName
        {
            get
            {
                return this.FontName;
            }
            set
            {
                this.FontName = value;
            }
        }

        [XmlIgnore]
        string IFontSymbol.Character
        {
            get
            {
                return this.Character;
            }
            set
            {
                this.Character = value;
            }
        }

        [XmlIgnore]
        bool? IFontSymbol.Bold
        {
            get
            {
                return this.BoldSpecified ? new Nullable<bool>(this.Bold) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.Bold = value.Value;
                    this.BoldSpecified = true;
                }
                else
                {
                    this.BoldSpecified = false;
                }
            }
        }

        [XmlIgnore]
        bool? IFontSymbol.Italic
        {
            get
            {
                return this.ItalicSpecified ? new Nullable<bool>(this.Italic) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.Italic = value.Value;
                    this.ItalicSpecified = true;
                }
                else
                {
                    this.ItalicSpecified = false;
                }
            }
        }

        [XmlIgnore]
        bool? IFontSymbol.Underlined
        {
            get
            {
                return this.UnderlinedSpecified ? new Nullable<bool>(this.Underlined) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.Underlined = value.Value;
                    this.UnderlinedSpecified = true;
                }
                else
                {
                    this.UnderlinedSpecified = false;
                }
            }
        }

        [XmlIgnore]
        string IFontSymbol.ForegroundColor
        {
            get
            {
                return this.ForegroundColor;
            }
            set
            {
                this.ForegroundColor = value;
            }
        }

        [XmlIgnore]
        public override PointSymbolType Type
        {
            get { return PointSymbolType.Font; }
        }

        IFontSymbol ICloneableLayerElement<IFontSymbol>.Clone()
        {
            return FontSymbolType.Deserialize(this.Serialize());
        }
    }

    partial class BlockSymbolType : IBlockSymbol
    {
        [XmlIgnore]
        string IBlockSymbol.DrawingName
        {
            get
            {
                return this.DrawingName;
            }
            set
            {
                this.DrawingName = value;
            }
        }

        [XmlIgnore]
        string IBlockSymbol.BlockName
        {
            get
            {
                return this.BlockName;
            }
            set
            {
                this.BlockName = value;
            }
        }

        [XmlIgnore]
        string IBlockSymbol.BlockColor
        {
            get
            {
                return this.BlockColor;
            }
            set
            {
                this.BlockColor = value;
            }
        }

        [XmlIgnore]
        string IBlockSymbol.LayerColor
        {
            get
            {
                return this.LayerColor;
            }
            set
            {
                this.LayerColor = value;
            }
        }

        [XmlIgnore]
        public override PointSymbolType Type
        {
            get { return PointSymbolType.Block; }
        }

        IBlockSymbol ICloneableLayerElement<IBlockSymbol>.Clone()
        {
            return BlockSymbolType.Deserialize(this.Serialize());
        }
    }

    partial class W2DSymbolType : IW2DSymbol
    {
        [XmlIgnore]
        ISymbolReference IW2DSymbol.W2DSymbol
        {
            get
            {
                return (ISymbolReference)this.W2DSymbol;
            }
            set
            {
                this.W2DSymbol = (W2DSymbolTypeW2DSymbol)value;
            }
        }

        [XmlIgnore]
        string IW2DSymbol.FillColor
        {
            get
            {
                return this.FillColor;
            }
            set
            {
                this.FillColor = value;
            }
        }

        [XmlIgnore]
        string IW2DSymbol.LineColor
        {
            get
            {
                return this.LineColor;
            }
            set
            {
                this.LineColor = value;
            }
        }

        [XmlIgnore]
        string IW2DSymbol.TextColor
        {
            get
            {
                return this.TextColor;
            }
            set
            {
                this.TextColor = value;
            }
        }

        [XmlIgnore]
        public override PointSymbolType Type
        {
            get { return PointSymbolType.W2D; }
        }

        IW2DSymbol ICloneableLayerElement<IW2DSymbol>.Clone()
        {
            return W2DSymbolType.Deserialize(this.Serialize());
        }
    }

    partial class W2DSymbolTypeW2DSymbol : ISymbolReference
    {
        [XmlIgnore]
        ImageSymbolReferenceType IBaseImageSymbol.Type
        {
            get { return ImageSymbolReferenceType.SymbolReference; }
        }

        ISymbolReference ICloneableLayerElement<ISymbolReference>.Clone()
        {
            return W2DSymbolTypeW2DSymbol.Deserialize(this.Serialize());
        }
    }

    partial class ImageSymbolTypeImage : ISymbolReference
    {
        [XmlIgnore]
        string ISymbolReference.ResourceId
        {
            get
            {
                return this.ResourceId == null ? string.Empty : this.ResourceId.ToString();
            }
            set
            {
                this.ResourceId = value;
            }
        }

        [XmlIgnore]
        string ISymbolReference.LibraryItemName
        {
            get
            {
                return this.LibraryItemName == null ? string.Empty : this.LibraryItemName.ToString();
            }
            set
            {
                this.LibraryItemName = value;
            }
        }

        [XmlIgnore]
        ImageSymbolReferenceType IBaseImageSymbol.Type
        {
            get { return ImageSymbolReferenceType.SymbolReference; }
        }

        ISymbolReference ICloneableLayerElement<ISymbolReference>.Clone()
        {
            return ImageSymbolTypeImage.Deserialize(this.Serialize());
        }
    }

    partial class TextSymbolTypeAdvancedPlacement : IAdvancedPlacement
    {
        
    }

    partial class AreaSymbolizationFillType : IAreaSymbolizationFill
    {
        [XmlIgnore]
        IFill IAreaSymbolizationFill.Fill
        {
            get
            {
                return this.Fill;
            }
            set
            {
                this.Fill = (FillType)value;
            }
        }

        [XmlIgnore]
        IStroke IAreaSymbolizationFill.Stroke
        {
            get
            {
                return this.Stroke;
            }
            set
            {
                this.Stroke = (StrokeType)value;
            }
        }

        IAreaSymbolizationFill ICloneableLayerElement<IAreaSymbolizationFill>.Clone()
        {
            return AreaSymbolizationFillType.Deserialize(this.Serialize());
        }
    }

    partial class LayerDefinition : ILayerDefinition
    {
        //internal LayerDefinition() { } 

        #if LDF_110
        private static readonly Version RES_VERSION = new Version(1, 1, 0);
        #elif LDF_120
        private static readonly Version RES_VERSION = new Version(1, 2, 0);
        #elif LDF_130
        private static readonly Version RES_VERSION = new Version(1, 3, 0);
        #elif LDF_230
        private static readonly Version RES_VERSION = new Version(2, 3, 0);
        #elif LDF_240
        private static readonly Version RES_VERSION = new Version(2, 4, 0);
        #else
        private static readonly Version RES_VERSION = new Version(1, 0, 0);
        #endif

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
                    throw new InvalidOperationException(Strings.ErrorInvalidResourceIdentifier);

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.LayerDefinition.ToString())
                    throw new InvalidOperationException(string.Format(Strings.ErrorUnexpectedResourceType, res.ToString(), ResourceTypes.LayerDefinition));

                _resId = value;
                this.OnPropertyChanged("ResourceID"); //NOXLATE
            }
        }

        [XmlIgnore]
        public ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.LayerDefinition;
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

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")] //NOXLATE
        public string ValidatingSchema
        {
#if LDF_110
            get { return "LayerDefinition-1.1.0.xsd"; } //NOXLATE
#elif LDF_120
            get { return "LayerDefinition-1.2.0.xsd"; } //NOXLATE
#elif LDF_130
            get { return "LayerDefinition-1.3.0.xsd"; } //NOXLATE
#elif LDF_230
            get { return "LayerDefinition-2.3.0.xsd"; } //NOXLATE
#elif LDF_240
            get { return "LayerDefinition-2.4.0.xsd"; } //NOXLATE
#else
            get { return "LayerDefinition-1.0.0.xsd"; } //NOXLATE
#endif
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        [XmlIgnore]
        public ISubLayerDefinition SubLayer
        {
            get { return this.Item; }
        }

        public INameStringPair CreatePair(string name, string value)
        {
            return new NameStringPairType() { Name = name, Value = value };
        }
    }

    partial class DrawingLayerDefinitionType : IDrawingLayerDefinition
    {
        [XmlIgnore]
        public override LayerType LayerType
        {
            get { return LayerType.Drawing; }
        }

        [XmlIgnore]
        string ISubLayerDefinition.ResourceId
        {
            get { return this.ResourceId; }
            set { this.ResourceId = value; }
        }
    }

    partial class VectorLayerDefinitionType : IVectorLayerDefinition
#if LDF_240
        , IVectorLayerDefinition2
#endif
    {
        [XmlIgnore]
        public override LayerType LayerType
        {
            get { return LayerType.Vector; }
        }

        [XmlIgnore]
        string ISubLayerDefinition.ResourceId
        {
            get { return this.ResourceId; }
            set { this.ResourceId = value; }
        }

        [XmlIgnore]
        string IVectorLayerDefinition.FeatureName
        {
            get { return this.FeatureName; }
            set { this.FeatureName = value; }
        }

        [XmlIgnore]
        string IVectorLayerDefinition.Geometry
        {
            get { return this.Geometry; }
            set { this.Geometry = value; }
        }

        [XmlIgnore]
        string IVectorLayerDefinition.Url
        {
#if LDF_240
            get
            {
                if (this.urlDataField == null)
                    return string.Empty;
                else
                    return this.urlDataField.Content;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (this.urlDataField == null)
                        this.urlDataField = new URLDataType();
                    this.urlDataField.Content = value;
                    OnPropertyChanged("Url"); //NOXLATE
                }
                else 
                {
                    //NOTE: None of the other URLData properties seem to be used atm
                    //hence why we are nulling this
                    this.urlDataField = null;
                    OnPropertyChanged("Url"); //NOXLATE
                }
            }
#else
            get { return this.Url; }
            set { this.Url = value; }
#endif
        }

        [XmlIgnore]
        string IVectorLayerDefinition.ToolTip
        {
            get { return this.ToolTip; }
            set { this.ToolTip = value; }
        }

        [XmlIgnore]
        string IVectorLayerDefinition.Filter
        {
            get { return this.Filter; }
            set { this.Filter = value; }
        }

        [XmlIgnore]
        IEnumerable<IVectorScaleRange> IVectorLayerDefinition.VectorScaleRange
        {
            get 
            {
                foreach (var vsr in this.VectorScaleRange)
                {
                    yield return vsr;
                }
            }
        }

        [XmlIgnore]
        IEnumerable<INameStringPair> IVectorLayerDefinition.PropertyMapping
        {
            get
            {
                foreach (var pair in this.PropertyMapping)
                {
                    yield return pair;
                }
            }
        }

        INameStringPair IVectorLayerDefinition.GetPropertyMapping(string name)
        {
            foreach (var p in this.PropertyMapping)
            {
                if (p.Name == name)
                    return p;
            }
            return null;
        }

        INameStringPair IVectorLayerDefinition.GetPropertyMappingAt(int index)
        {
            if (index < 0 || index >= this.PropertyMapping.Count)
                throw new ArgumentOutOfRangeException();

            var item = this.PropertyMapping[index];
            return item;
        }

        void IVectorLayerDefinition.AddVectorScaleRange(IVectorScaleRange range)
        {
            var r = range as VectorScaleRangeType;
            if (r != null)
                this.VectorScaleRange.Add(r);
        }

        void IVectorLayerDefinition.RemoveVectorScaleRange(IVectorScaleRange range)
        {
            var r = range as VectorScaleRangeType;
            if (r != null)
                this.VectorScaleRange.Remove(r);
        }

        void IVectorLayerDefinition.AddPropertyMapping(INameStringPair pair)
        {
            var p = pair as NameStringPairType;
            if (p != null)
                this.PropertyMapping.Add(p);
        }

        void IVectorLayerDefinition.RemovePropertyMapping(INameStringPair pair)
        {
            var p = pair as NameStringPairType;
            if (p != null)
                this.PropertyMapping.Remove(p);
        }

        int IVectorLayerDefinition.GetPosition(INameStringPair pair)
        {
            var p = pair as NameStringPairType;
            if (p != null)
                return this.PropertyMapping.IndexOf(p);

            return -1;
        }

        int IVectorLayerDefinition.MoveUp(INameStringPair pair)
        {
            int pos = ((IVectorLayerDefinition)this).GetPosition(pair);
            if (pos > 0)
            {
                int dest = pos - 1;
                var p = this.PropertyMapping[dest];
                var p2 = (NameStringPairType)pair;

                //Swap
                this.PropertyMapping[dest] = p2;
                this.PropertyMapping[pos] = p;

                return dest;
            }
            return -1;
        }

        int IVectorLayerDefinition.MoveDown(INameStringPair pair)
        {
            int pos = ((IVectorLayerDefinition)this).GetPosition(pair);
            if (pos < this.PropertyMapping.Count - 1)
            {
                int dest = pos + 1;
                var p = this.PropertyMapping[dest];
                var p2 = (NameStringPairType)pair;

                //Swap
                this.PropertyMapping[dest] = p2;
                this.PropertyMapping[pos] = p;

                return dest;
            }
            return -1;
        }

        void IVectorLayerDefinition.RemoveAllScaleRanges()
        {
            this.VectorScaleRange.Clear();
        }

        int IVectorLayerDefinition.IndexOfScaleRange(IVectorScaleRange range)
        {
            var r = range as VectorScaleRangeType;
            if (r != null)
                return this.VectorScaleRange.IndexOf(r);

            return -1;
        }

        IVectorScaleRange IVectorLayerDefinition.GetScaleRangeAt(int index)
        {
            if (index >= this.VectorScaleRange.Count)
                return null;

            return this.VectorScaleRange[index];
        }

        [XmlIgnore]
        Version IVectorLayerDefinition.SymbolDefinitionVersion
        {
            get
            {
#if LDF_110
                return new Version(1, 0, 0);
#elif LDF_120 || LDF_130 || LDF_230
                return new Version(1, 1, 0);
#elif LDF_240
                return new Version(2, 4, 0);
#else
                return null;
#endif          
            }
        }

#if LDF_240
        [XmlIgnore]
        IUrlData IVectorLayerDefinition2.UrlData
        {
            get
            {
                return this.UrlData;
            }
            set
            {
                this.UrlData = (URLDataType)value;
            }
        }
#endif
    }
    #region Composite Symbolization
#if !LDF_100

    partial class CompositeRule : ICompositeRule
    {
        [XmlIgnore]
        ICompositeSymbolization ICompositeRule.CompositeSymbolization
        {
            get
            {
                return this.CompositeSymbolization;
            }
            set
            {
                this.CompositeSymbolization = (CompositeSymbolization)value;
            }
        }
    }

    partial class CompositeSymbolization : ICompositeSymbolization
    {
        [XmlIgnore]
        IEnumerable<ISymbolInstance> ICompositeSymbolization.SymbolInstance
        {
            get 
            {
                foreach (var sym in this.SymbolInstance)
                {
                    yield return sym;
                }
            }
        }

        public void AddSymbolInstance(ISymbolInstance inst)
        {
            var sym = inst as SymbolInstance;
            if (sym != null)
                this.SymbolInstance.Add(sym);
        }

        public void RemoveSymbolInstance(ISymbolInstance inst)
        {
            var sym = inst as SymbolInstance;
            if (sym != null)
                this.SymbolInstance.Remove(sym);
        }

        public ISymbolInstance CreateSymbolReference(string resourceId)
        {
            return new SymbolInstance()
            {
                Item = resourceId,
                ParameterOverrides = new ParameterOverrides()
                {
                    Override = new BindingList<Override>()
                },
            };
        }


        public ISymbolInstance CreateInlineSimpleSymbol(ISimpleSymbolDefinition symDef)
        {
            return new SymbolInstance()
            {
#if LDF_110
                Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.SimpleSymbolDefinition)symDef,
#elif LDF_120 || LDF_130 || LDF_230
                Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0.SimpleSymbolDefinition)symDef,
#else
                Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0.SimpleSymbolDefinition)symDef,
#endif
                ParameterOverrides = new ParameterOverrides()
                {
                    Override = new BindingList<Override>()
                }
            };
        }

        public ISymbolInstance CreateInlineCompoundSymbol(ICompoundSymbolDefinition compDef)
        {
            return new SymbolInstance()
            {
#if LDF_110
                Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.CompoundSymbolDefinition)compDef,
#elif LDF_120 || LDF_130 || LDF_230
                Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0.CompoundSymbolDefinition)compDef,
#else
                Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0.CompoundSymbolDefinition)compDef,
#endif
                ParameterOverrides = new ParameterOverrides()
                {
                    Override = new BindingList<Override>()
                }
            };
        }
    }

    partial class SymbolInstance : ISymbolInstance
#if LDF_100 || LDF_110
#else
                                 , ISymbolInstance2
#endif
    {
        [XmlIgnore]
        IParameterOverrideCollection ISymbolInstance.ParameterOverrides
        {
            get { return this.ParameterOverrides; }
        }

        [XmlIgnore]
        public ISymbolInstanceReference Reference
        {
            get
            {
                var libId = this.Item as string;
#if LDF_110
                var simpSym = this.Item as OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.SimpleSymbolDefinition;
                var compSym = this.Item as OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.CompoundSymbolDefinition;
#elif LDF_120 || LDF_130 || LDF_230
                var simpSym = this.Item as OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0.SimpleSymbolDefinition;
                var compSym = this.Item as OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0.CompoundSymbolDefinition;
#else
                var simpSym = this.Item as OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0.SimpleSymbolDefinition;
                var compSym = this.Item as OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0.CompoundSymbolDefinition;
#endif
                if (libId != null)
                    return new SymbolInstanceLibrary() { ResourceId = libId };
                else if (simpSym != null)
                    return new SymbolInstanceInline() { SymbolDefinition = simpSym };
                else if (compSym != null)
                    return new SymbolInstanceInline() { SymbolDefinition = compSym };
                return null;
            }
            set
            {
                var sr = value as ISymbolInstanceReferenceLibrary;
                var ir = value as ISymbolInstanceReferenceInline;
                if (sr != null)
                {
                    this.Item = sr.ResourceId;
                }
                else if (ir != null)
                {
#if LDF_110
                    if (ir.SymbolDefinition.Type == SymbolDefinitionType.Simple)
                        this.Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.SimpleSymbolDefinition)ir.SymbolDefinition;
                    else if (ir.SymbolDefinition.Type == SymbolDefinitionType.Compound)
                        this.Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0.CompoundSymbolDefinition)ir.SymbolDefinition;
#elif LDF_120 || LDF_130 || LDF_230
                    if (ir.SymbolDefinition.Type == SymbolDefinitionType.Simple)
                        this.Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0.SimpleSymbolDefinition)ir.SymbolDefinition;
                    else if (ir.SymbolDefinition.Type == SymbolDefinitionType.Compound)
                        this.Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0.CompoundSymbolDefinition)ir.SymbolDefinition;
#else
                    if (ir.SymbolDefinition.Type == SymbolDefinitionType.Simple)
                        this.Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0.SimpleSymbolDefinition)ir.SymbolDefinition;
                    else if (ir.SymbolDefinition.Type == SymbolDefinitionType.Compound)
                        this.Item = (OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0.CompoundSymbolDefinition)ir.SymbolDefinition;
#endif
                }
                this.Item = null;
            }
        }
    }

    partial class SymbolInstanceLibrary : ISymbolInstanceReferenceLibrary
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string _resId;

        [XmlIgnore]
        public string ResourceId
        {
            get { return _resId; }
            set 
            { 
                if (_resId == value) return;
                _resId = value;
                OnPropertyChanged("ResourceId"); //NOXLATE
            }
        }

        [XmlIgnore]
        public SymbolInstanceType Type
        {
            get { return SymbolInstanceType.Reference; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }

    partial class SymbolInstanceInline : ISymbolInstanceReferenceInline
    {
        [XmlIgnore]
        public ISymbolDefinitionBase SymbolDefinition
        {
            get;
            set;
        }

        [XmlIgnore]
        public SymbolInstanceType Type
        {
            get { return SymbolInstanceType.Inline; }
        }
    }

    partial class ParameterOverrides : IParameterOverrideCollection
    {
        [XmlIgnore]
        IEnumerable<IParameterOverride> IParameterOverrideCollection.Override
        {
            get 
            {
                foreach (var ov in this.Override)
                {
                    yield return ov;
                }
            }
        }

        [XmlIgnore]
        public int Count
        {
            get { return this.Override.Count; }
        }

        [XmlIgnore]
        public IParameterOverride this[int index]
        {
            get
            {
                if (index >= this.Override.Count)
                    throw new ArgumentOutOfRangeException();

                return this.Override[index];
            }
        }

        public void AddOverride(IParameterOverride ov)
        {
            var o = ov as Override;
            if (o != null)
                this.Override.Add(o);
        }

        public void RemoveOverride(IParameterOverride ov)
        {
            var o = ov as Override;
            if (o != null)
                this.Override.Remove(o);
        }

        public IParameterOverride CreateParameterOverride(string symbol, string name)
        {
            return new Override()
            {
                ParameterIdentifier = name, 
                SymbolName = symbol
            };
        }
    }

    partial class Override : IParameterOverride
    {

    }

    partial class CompositeTypeStyle : ICompositeTypeStyle
#if LDF_130 || LDF_230 || LDF_240
        , ICompositeTypeStyle2
#endif
    {
        [XmlIgnore]
        string ICompositeTypeStyle.DisplayString
        {
            get
            {
                return Strings.CompositeStyleDisplayString;
            }
        }

        [XmlIgnore]
        IEnumerable<ICompositeRule> ICompositeTypeStyle.CompositeRule
        {
            get 
            {
                foreach (var cr in this.CompositeRule)
                {
                    yield return cr;
                }
            }
        }

        public void AddCompositeRule(ICompositeRule compRule)
        {
            var cr = compRule as CompositeRule;
            if (cr != null)
                this.CompositeRule.Add(cr);
        }

        public void RemoveCompositeRule(ICompositeRule compRule)
        {
            var cr = compRule as CompositeRule;
            if (cr != null)
                this.CompositeRule.Remove(cr);
        }

        [XmlIgnore]
        public StyleType StyleType
        {
            get { return StyleType.Composite; }
        }

        [XmlIgnore]
        public int RuleCount
        {
            get { return this.CompositeRule.Count; }
        }

        #region IRuleCollection<ICompositeRule> Members

        public int IndexOfRule(ICompositeRule rule)
        {
            var cr = rule as CompositeRule;
            if (cr != null)
                return this.CompositeRule.IndexOf(cr);
            return -1;
        }

        public ICompositeRule GetRuleAt(int index)
        {
            return this.CompositeRule[index];
        }

        public bool MoveUp(ICompositeRule rule)
        {
            var cr = rule as CompositeRule;
            if (cr != null)
                return CollectionUtil.MoveUp(this.CompositeRule, cr);
            return false;
        }

        public bool MoveDown(ICompositeRule rule)
        {
            var cr = rule as CompositeRule;
            if (cr != null)
                return CollectionUtil.MoveDown(this.CompositeRule, cr);
            return false;
        }

        #endregion

        #region IRuleCollection Members
        public int IndexOfRule(IVectorRule rule)
        {
            var cr = rule as CompositeRule;
            if (cr != null)
                return this.CompositeRule.IndexOf(cr);
            return -1;
        }

        IVectorRule IRuleCollection.GetRuleAt(int index)
        {
            return this.CompositeRule[index];
        }

        public bool MoveUp(IVectorRule rule)
        {
            var cr = rule as CompositeRule;
            if (cr != null)
                return CollectionUtil.MoveUp(this.CompositeRule, cr);
            return false;
        }

        public bool MoveDown(IVectorRule rule)
        {
            var cr = rule as CompositeRule;
            if (cr != null)
                return CollectionUtil.MoveDown(this.CompositeRule, cr);
            return false;
        }
        #endregion
    }
#endif
    #endregion

#if LDF_240
    partial class URLDataType : IUrlData
    {

    }
#endif
}
