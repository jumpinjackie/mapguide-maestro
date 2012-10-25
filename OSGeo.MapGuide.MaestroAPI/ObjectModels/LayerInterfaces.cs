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
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using System.ComponentModel;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;

namespace OSGeo.MapGuide.ObjectModels.LayerDefinition
{
    #region core enums

    /// <summary>
    /// The type of length unit
    /// </summary>
    [System.SerializableAttribute()]
    public enum LengthUnitType
    {

        /// <remarks/>
        Millimeters,

        /// <remarks/>
        Centimeters,

        /// <remarks/>
        Meters,

        /// <remarks/>
        Kilometers,

        /// <remarks/>
        Inches,

        /// <remarks/>
        Feet,

        /// <remarks/>
        Yards,

        /// <remarks/>
        Miles,

        /// <remarks/>
        Points,
    }

    /// <summary>
    /// The type of size context
    /// </summary>
    [System.SerializableAttribute()]
    public enum SizeContextType
    {

        /// <remarks/>
        MappingUnits,

        /// <remarks/>
        DeviceUnits,
    }

    /// <summary>
    /// The type of shape
    /// </summary>
    [System.SerializableAttribute()]
    public enum ShapeType
    {

        /// <remarks/>
        Square,

        /// <remarks/>
        Circle,

        /// <remarks/>
        Triangle,

        /// <remarks/>
        Star,

        /// <remarks/>
        Cross,

        /// <remarks/>
        X,
    }

    /// <summary>
    /// The type of background style
    /// </summary>
    [System.SerializableAttribute()]
    public enum BackgroundStyleType
    {

        /// <remarks/>
        Transparent,

        /// <remarks/>
        Opaque,

        /// <remarks/>
        Ghosted,
    }

    /// <summary>
    /// The type of feature name
    /// </summary>
    [System.SerializableAttribute()]
    public enum FeatureNameType
    {

        /// <remarks/>
        FeatureClass,

        /// <remarks/>
        NamedExtension,
    }

    /// <summary>
    /// The type of explicit color
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        Band,

        /// <remarks/>
        Bands,

        /// <remarks/>
        ExplicitColor,
    }

    #endregion

    #region core

    /// <summary>
    /// The type of layer definition
    /// </summary>
    public enum LayerType
    {
        /// <summary>
        /// DWF-based drawing layer
        /// </summary>
        Drawing,
        /// <summary>
        /// Vector layer
        /// </summary>
        Vector,
        /// <summary>
        /// Raster layer
        /// </summary>
        Raster
    }

    /// <summary>
    /// Represents elements that can create clones of themselves
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneableLayerElement<T>
    {
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }

    /// <summary>
    /// Helper class to clone certain elements
    /// </summary>
    public static class LayerElementCloningUtil
    {
        /// <summary>
        /// Clones the strokes.
        /// </summary>
        /// <param name="strokes">The strokes.</param>
        /// <returns></returns>
        public static IList<IStroke> CloneStrokes(IEnumerable<IStroke> strokes)
        {
            Check.NotNull(strokes, "strokes"); //NOXLATE
            var list = new List<IStroke>();
            foreach (var st in strokes)
            {
                list.Add(st.Clone());
            }
            return list;
        }
    }

    /// <summary>
    /// Factory interface to assist in creating common layer definition elements
    /// </summary>
    public interface ILayerElementFactory
    {
        /// <summary>
        /// Creates a name-value pair
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        INameStringPair CreatePair(string name, string value);

        /// <summary>
        /// Creates a default area rule (monochromatic)
        /// </summary>
        /// <returns></returns>
        IAreaRule CreateDefaultAreaRule();

        /// <summary>
        /// Creates a default area style (monochromatic)
        /// </summary>
        /// <returns></returns>
        IAreaVectorStyle CreateDefaultAreaStyle();

        /// <summary>
        /// Creates a default fill (monochromatic)
        /// </summary>
        /// <returns></returns>
        IFill CreateDefaultFill();

        /// <summary>
        /// Creates a default line style (monochromatic)
        /// </summary>
        /// <returns></returns>
        ILineVectorStyle CreateDefaultLineStyle();

        /// <summary>
        /// Creates a default mark symbol
        /// </summary>
        /// <returns></returns>
        IMarkSymbol CreateDefaultMarkSymbol();

        /// <summary>
        /// Creates a default point style
        /// </summary>
        /// <returns></returns>
        IPointVectorStyle CreateDefaultPointStyle();

        /// <summary>
        /// Creates a default 2D point symbolization
        /// </summary>
        /// <returns></returns>
        IPointSymbolization2D CreateDefaultPointSymbolization2D();
        
        /// <summary>
        /// Creates a default stroke
        /// </summary>
        /// <returns></returns>
        IStroke CreateDefaultStroke();

        /// <summary>
        /// Creates a default text symbol
        /// </summary>
        /// <returns></returns>
        ITextSymbol CreateDefaultTextSymbol();

        /// <summary>
        /// Creates a default advanced placement setting
        /// </summary>
        /// <param name="scaleLimit"></param>
        /// <returns></returns>
        IAdvancedPlacement CreateDefaultAdvancedPlacement(double scaleLimit);

        /// <summary>
        /// Creates a fill
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="background"></param>
        /// <param name="foreground"></param>
        /// <returns></returns>
        IFill CreateFill(string pattern, System.Drawing.Color background, System.Drawing.Color foreground);

        /// <summary>
        /// Creates a default line rule
        /// </summary>
        /// <returns></returns>
        ILineRule CreateDefaultLineRule();
        
        /// <summary>
        /// Creates a default point rule
        /// </summary>
        /// <returns></returns>
        IPointRule CreateDefaultPointRule();

        /// <summary>
        /// Creates a stroke of the specified color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        IStroke CreateStroke(System.Drawing.Color color);

        /// <summary>
        /// Creates a vector scale range
        /// </summary>
        /// <returns></returns>
        IVectorScaleRange CreateVectorScaleRange();

        /// <summary>
        /// Creates a font symbol
        /// </summary>
        /// <returns></returns>
        IFontSymbol CreateDefaultFontSymbol();

        /// <summary>
        /// Creates a W2D symbol from a Symbol Library
        /// </summary>
        /// <param name="symbolLibrary"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        IW2DSymbol CreateDefaultW2DSymbol(string symbolLibrary, string symbolName);

        /// <summary>
        /// Creates the default composite rule.
        /// </summary>
        /// <returns></returns>
        ICompositeRule CreateDefaultCompositeRule();

        /// <summary>
        /// Creates the default composite style.
        /// </summary>
        /// <returns></returns>
        ICompositeTypeStyle CreateDefaultCompositeStyle();

        /// <summary>
        /// Creates tehe default URL data
        /// </summary>
        /// <returns></returns>
        IUrlData CreateUrlData();

        /// <summary>
        /// Creates a default AreaSymbolizationFill element
        /// </summary>
        /// <returns></returns>
        IAreaSymbolizationFill CreateDefaultAreaSymbolizationFill();
    }

    /// <summary>
    /// Top-level interface of the layer definition
    /// </summary>
    public interface ILayerDefinition : IResource, ILayerElementFactory
    {
        /// <summary>
        /// Gets the sub layer.
        /// </summary>
        /// <value>The sub layer.</value>
        ISubLayerDefinition SubLayer { get; }
    }

    /// <summary>
    /// Represents the base of all layer definitions
    /// </summary>
    public interface ISubLayerDefinition : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the type of layer definition
        /// </summary>
        LayerType LayerType { get; }

        /// <summary>
        /// Gets or sets the resource id which is the data source for this layer
        /// </summary>
        string ResourceId { get; set; }
    }

    /// <summary>
    /// Represents the base of all layer definitions. Based on Layer Definition schema
    /// version 2.3.0
    /// </summary>
    public interface ISubLayerDefinition2 : ISubLayerDefinition, IWatermarkCollection
    {
    }

    /// <summary>
    /// Represents a layer definition based on a vector-based feature source
    /// </summary>
    public interface IVectorLayerDefinition : ISubLayerDefinition
    {
        /// <summary>
        /// Gets or sets fully qualified name of the feature class which this layer applies
        /// </summary>
        string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets the geometry field of the feature class which this layer applies
        /// </summary>
        string Geometry { get; set; }

        /// <summary>
        /// Gets or sets an FDO expression which represents the URL that is opened when
        /// a feature is selected
        /// </summary>
        /// <remarks>
        /// If this is a <see cref="T:OSGeo.MapGuide.ObjectModels.LayerDefinition.IVectorLayerDefinition2"/>
        /// instance, this property is a pass through to the <see cref="P:OSGeo.MapGuide.ObjectModels.LayerDefinition.IUrlData.Content"/>
        /// property value. In other words it is equivalent to getting or setting the value of UrlData.Content
        /// </remarks>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets an FDO expression which represents the HTML content that is displayed
        /// when the mouse is over the current feature
        /// </summary>
        string ToolTip { get; set; }

        /// <summary>
        /// Gets or sets an FDO filter which is applied when rendering/styling features
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Gets the scale ranges for this layer
        /// </summary>
        IEnumerable<IVectorScaleRange> VectorScaleRange { get; }

        /// <summary>
        /// Gets the collection index of this scale range
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        int IndexOfScaleRange(IVectorScaleRange range);

        /// <summary>
        /// Removes all scale ranges from this layer
        /// </summary>
        void RemoveAllScaleRanges();

        /// <summary>
        /// Gets the scale range at this specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IVectorScaleRange GetScaleRangeAt(int index);

        /// <summary>
        /// Adds the specified vector scale range
        /// </summary>
        /// <param name="range"></param>
        void AddVectorScaleRange(IVectorScaleRange range);

        /// <summary>
        /// Removes the specified vector scale range
        /// </summary>
        /// <param name="range"></param>
        void RemoveVectorScaleRange(IVectorScaleRange range);

        /// <summary>
        /// Gets the property mappings for this layer. This determines which properties
        /// are displayed (and what labels to use) in the property pane and 
        /// </summary>
        IEnumerable<INameStringPair> PropertyMapping { get; }

        /// <summary>
        /// Adds the specified property mapping
        /// </summary>
        /// <param name="pair"></param>
        void AddPropertyMapping(INameStringPair pair);

        /// <summary>
        /// Removes the specified property mapping
        /// </summary>
        /// <param name="pair"></param>
        void RemovePropertyMapping(INameStringPair pair);

        /// <summary>
        /// Gets the property mapping at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        INameStringPair GetPropertyMappingAt(int index);

        /// <summary>
        /// Gets the property mapping for the feature class property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        INameStringPair GetPropertyMapping(string name);

        /// <summary>
        /// Gets the position of the specified pair in the order of property mappings
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        int GetPosition(INameStringPair pair);

        /// <summary>
        /// Moves the specified pair up the order of property mappings
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        int MoveUp(INameStringPair pair);

        /// <summary>
        /// Moves the specified pair down the order of property mappings
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        int MoveDown(INameStringPair pair);

        /// <summary>
        /// Gets the supported symbol definition version to use for composite symbolization.
        /// If the Layer Definition does not support composite symbolization, null is returned
        /// </summary>
        Version SymbolDefinitionVersion { get; }
    }

    /// <summary>
    /// Based on Layer Definition schema 2.4.0
    /// </summary>
    public interface IVectorLayerDefinition2 : IVectorLayerDefinition
    {
        /// <summary>
        /// Gets or sets the URL data.
        /// </summary>
        /// <value>
        /// The URL data.
        /// </value>
        IUrlData UrlData { get; set; }
    }

    /// <summary>
    /// URL information for features
    /// </summary>
    public interface IUrlData
    {
        /// <summary>
        /// Gets or sets the real address of the URL. This can be a string FDO expression
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        string Content { get; set; }
        /// <summary>
        /// Gets or sets the description of the URL. This can be a string FDO expression
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; set; }
        /// <summary>
        /// Gets or sets the override of URL content for a specific feature which can be a string FDO expression
        /// </summary>
        /// <value>
        /// The content override.
        /// </value>
        string ContentOverride { get; set; }
        /// <summary>
        /// Gets or sets the override of URL description for a specific feature which can be a string FDO expression
        /// </summary>
        /// <value>
        /// The description override.
        /// </value>
        string DescriptionOverride { get; set; }
    }

    /// <summary>
    /// Represents a layer definition based on a raster-based feature source
    /// </summary>
    public interface IRasterLayerDefinition : ISubLayerDefinition
    {
        /// <summary>
        /// Gets or sets the name of the feature class.
        /// </summary>
        /// <value>The name of the feature class.</value>
        string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets the raster property.
        /// </summary>
        /// <value>The raster property.</value>
        string Geometry { get; set; }

        /// <summary>
        /// Gets the grid scale ranges.
        /// </summary>
        /// <value>The grid scale ranges.</value>
        IEnumerable<IGridScaleRange> GridScaleRange { get; }

        /// <summary>
        /// Adds the specified grid scale range
        /// </summary>
        /// <param name="range"></param>
        void AddGridScaleRange(IGridScaleRange range);

        /// <summary>
        /// Removes the specified grid scale range
        /// </summary>
        /// <param name="range"></param>
        void RemoveGridScaleRange(IGridScaleRange range);

        /// <summary>
        /// Indexes the of scale range.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        int IndexOfScaleRange(IGridScaleRange range);

        /// <summary>
        /// Gets the scale range at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        IGridScaleRange GetScaleRangeAt(int index);

        /// <summary>
        /// Gets the grid scale range count.
        /// </summary>
        int GridScaleRangeCount { get; }
    }

    //If only all layers were as simple as this one...

    /// <summary>
    /// Represents a layer definition based on a DWF-based drawing source
    /// </summary>
    public interface IDrawingLayerDefinition : ISubLayerDefinition
    {
        /// <summary>
        /// Gets or sets the sheet of the DWF to user
        /// </summary>
        string Sheet { get; set; }

        /// <summary>
        /// Gets or sets the layers to show from the specified sheet. Shows all layers if this is not specified
        /// </summary>
        string LayerFilter { get; set; }

        /// <summary>
        /// Gets or sets the zoomed in part of the scale range. Defaults to 0 if not specified. Inclusive
        /// </summary>
        double MinScale { get; set; }

        /// <summary>
        /// Gets or sets the zoomed out part of the scale range. Defaults to the application's maximum value if not specified. Exclusive
        /// </summary>
        double MaxScale { get; set; }
    }

    #endregion

    #region vector layer

    /// <summary>
    /// The stylization to be applied to the vector features for a given scale range
    /// </summary>
    public interface IVectorScaleRange
    {
        /// <summary>
        /// The zoomed in part of the scale range. Defaults to 0 if not specified. Inclusive
        /// </summary>
        double? MinScale { get; set; }

        /// <summary>
        /// The zoomed out part of the scale range. Defaults to the application's maximum value if not specified. Exclusive
        /// </summary>
        double? MaxScale { get; set; }

        /// <summary>
        /// Gets or sets the area style for this scale range
        /// </summary>
        IAreaVectorStyle AreaStyle { get; set; }

        /// <summary>
        /// Gets or sets the point style for this scale range
        /// </summary>
        IPointVectorStyle PointStyle { get; set; }

        /// <summary>
        /// Gets or sets the line style for this scale range
        /// </summary>
        ILineVectorStyle LineStyle { get; set; }

        /// <summary>
        /// Creates a clone of this instance
        /// </summary>
        /// <returns></returns>
        IVectorScaleRange Clone();
    }

    /// <summary>
    /// The stylization to be applied to the vector features for a given scale range. Supports elevation
    /// and extrusion settings.
    /// 
    /// Supported by Layer Definition 1.1.0 and higher
    /// </summary>
    public interface IVectorScaleRange2 : IVectorScaleRange
    {
        /// <summary>
        /// Creates a new instance of <see cref="IElevationSettings"/>. This instance is detached
        /// and needs to be assigned to the <see cref="ElevationSettings"/> property to take effect.
        /// </summary>
        /// <param name="zOffset"></param>
        /// <param name="zExtrusion"></param>
        /// <param name="zOffsetType"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        IElevationSettings Create(string zOffset, string zExtrusion, ElevationTypeType zOffsetType, LengthUnitType unit);

        /// <summary>
        /// Gets or sets the elevation settings
        /// </summary>
        IElevationSettings ElevationSettings { get; set; }

        /// <summary>
        /// Gets or sets the composite styles for this scale range
        /// </summary>
        IEnumerable<ICompositeTypeStyle> CompositeStyle { get; set; }

        /// <summary>
        /// Gets the composite style count.
        /// </summary>
        int CompositeStyleCount { get; }
    }

    /// <summary>
    /// Represents a name-value pair
    /// </summary>
    public interface INameStringPair
    {
        /// <summary>
        /// Gets or set the name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        string Value { get; set; }
    }

    /// <summary>
    /// base interface for all style specifications
    /// </summary>
    public interface IVectorStyle : IRuleCollection
    {
        /// <summary>
        /// Gets the type of this style specification
        /// </summary>
        StyleType StyleType { get; }

        /// <summary>
        /// Gets the rule count.
        /// </summary>
        int RuleCount { get; }
    }

    /// <summary>
    /// Indicates the type of geometry this style specification is for
    /// </summary>
    public enum StyleType
    {
        /// <summary>
        /// 
        /// </summary>
        Area,
        /// <summary>
        /// 
        /// </summary>
        Line,
        /// <summary>
        /// 
        /// </summary>
        Point,
        /// <summary>
        /// 
        /// </summary>
        Composite
    }

    /// <summary>
    /// Style specification for a area geometry layer
    /// </summary>
    public interface IAreaVectorStyle : IVectorStyle, IRuleCollection<IAreaRule>
    {
        /// <summary>
        /// Enumerates the rules in this specification
        /// </summary>
        IEnumerable<IAreaRule> Rules { get; }

        /// <summary>
        /// Adds a rule to this specification
        /// </summary>
        /// <param name="rule"></param>
        void AddRule(IAreaRule rule);

        /// <summary>
        /// Removes a rule from this specification
        /// </summary>
        /// <param name="rule"></param>
        void RemoveRule(IAreaRule rule);

        /// <summary>
        /// Removes all rules.
        /// </summary>
        void RemoveAllRules();
    }

    /// <summary>
    /// Style specification for a line geometry layer
    /// </summary>
    public interface ILineVectorStyle : IVectorStyle, IRuleCollection<ILineRule>
    {
        /// <summary>
        /// Enumerates the rules in this specification
        /// </summary>
        IEnumerable<ILineRule> Rules { get; }

        /// <summary>
        /// Adds a rule to this specification
        /// </summary>
        /// <param name="rule"></param>
        void AddRule(ILineRule rule);

        /// <summary>
        /// Removes a rule from this specification
        /// </summary>
        /// <param name="rule"></param>
        void RemoveRule(ILineRule rule);

        /// <summary>
        /// Removes all current rules
        /// </summary>
        void RemoveAllRules();
    }

    /// <summary>
    /// Defines a collection of style rules
    /// </summary>
    public interface IRuleCollection
    {
        /// <summary>
        /// Gets the rule at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IVectorRule GetRuleAt(int index);
        /// <summary>
        /// Gets the index of the specified rule
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        int IndexOfRule(IVectorRule rule);
        /// <summary>
        /// Moves the specified rule up the list
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        bool MoveUp(IVectorRule rule);
        /// <summary>
        /// Moves the specified rule down the list
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        bool MoveDown(IVectorRule rule);
    }

    /// <summary>
    /// Defines a collection of style rules
    /// </summary>
    /// <typeparam name="TRule">The type of the rule.</typeparam>
    public interface IRuleCollection<TRule> : IRuleCollection
    {
        /// <summary>
        /// Gets the index of the specified rule
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        int IndexOfRule(TRule rule);
        /// <summary>
        /// Gets the rule at the specified index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        TRule GetRuleAt(int index);
        /// <summary>
        /// Moves the specified rule up the list
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        bool MoveUp(TRule rule);
        /// <summary>
        /// Moves the specified rule down the list
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        bool MoveDown(TRule rule);
    }

    /// <summary>
    /// Style specification for a point geometry layer
    /// </summary>
    public interface IPointVectorStyle : IVectorStyle, IRuleCollection<IPointRule>
    {
        /// <summary>
        /// Gets or sets whether to create a text layer
        /// </summary>
        bool DisplayAsText { get; set; }

        /// <summary>
        /// Gets or sets whether to allow labels from any map layer (including the current layer) to obscure features on the current layer
        /// </summary>
        bool AllowOverpost { get; set; }

        /// <summary>
        /// Enumerates the rules for this specification
        /// </summary>
        IEnumerable<IPointRule> Rules { get; }

        /// <summary>
        /// Adds a rule to this specification
        /// </summary>
        /// <param name="rule"></param>
        void AddRule(IPointRule rule);

        /// <summary>
        /// Removes a rule from this specification
        /// </summary>
        /// <param name="rule"></param>
        void RemoveRule(IPointRule rule);

        /// <summary>
        /// Removes all current rules
        /// </summary>
        void RemoveAllRules();
    }

    /// <summary>
    /// Base interface for style rules of all geometric types
    /// </summary>
    public interface IVectorRule 
    {
        /// <summary>
        /// Gets or sets the label for the rule to be displayed in the legend
        /// </summary>
        string LegendLabel { get; set; }

        /// <summary>
        /// Gets or sets the filter for this rule
        /// </summary>
        string Filter { get; set; }
    }

    /// <summary>
    /// Base interface for basic style rules for all geometric types
    /// </summary>
    public interface IBasicVectorRule : IVectorRule
    {
        /// <summary>
        /// Gets or sets the the label
        /// </summary>
        ITextSymbol Label { get; set; }
    }

    /// <summary>
    /// A style rule for the point geometry type
    /// </summary>
    public interface IPointRule : IBasicVectorRule
    {
        /// <summary>
        /// Gets or sets the symbolization settings for this point rule
        /// </summary>
        IPointSymbolization2D PointSymbolization2D { get; set; }
    }

    /// <summary>
    /// A style rule for the line geometry type
    /// </summary>
    public interface ILineRule : IBasicVectorRule
    {
        /// <summary>
        /// Gets the number of strokes in this line rule
        /// </summary>
        int StrokeCount { get; }

        /// <summary>
        /// Gets the symbolization settings for this line rule
        /// </summary>
        IEnumerable<IStroke> Strokes { get; }

        /// <summary>
        /// Removes any existing strokes and adds the specified list of strokes
        /// </summary>
        /// <param name="strokes"></param>
        void SetStrokes(IEnumerable<IStroke> strokes);

        /// <summary>
        /// Adds a stroke to this rule
        /// </summary>
        /// <param name="stroke"></param>
        void AddStroke(IStroke stroke);

        /// <summary>
        /// Removes a stroke from this rule
        /// </summary>
        /// <param name="stroke"></param>
        void RemoveStroke(IStroke stroke);
    }

    /// <summary>
    /// A style rule for the area/polygon geometry type
    /// </summary>
    public interface IAreaRule : IBasicVectorRule
    {
        /// <summary>
        /// Gets or sets the polygon stylization settings 
        /// </summary>
        IAreaSymbolizationFill AreaSymbolization2D { get; set; }
    }

    /// <summary>
    /// Encapsulates the stylization of a line
    /// </summary>
    public interface IStroke : ICloneableLayerElement<IStroke>
    {
        /// <summary>
        /// Gets or sets the line style
        /// </summary>
        string LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the thickness
        /// </summary>
        string Thickness { get; set; }

        /// <summary>
        /// Gets or sets the color
        /// </summary>
        string Color { get; set; }

        /// <summary>
        /// Gets or sets the thickness unit
        /// </summary>
        LengthUnitType Unit { get; set; }
    }

    /// <summary>
    /// Encapsulates the stylization of a line. Supported in Layer Definition schema
    /// 1.1.0 and newer
    /// </summary>
    public interface IStroke2 : IStroke, ICloneableLayerElement<IStroke2>
    {
        /// <summary>
        /// Gets or sets the size context of the thickness units
        /// </summary>
        SizeContextType SizeContext { get; set; }
    }

    /// <summary>
    /// Symbolization characteristics for areas.
    /// </summary>
    public interface IAreaSymbolizationFill : ICloneableLayerElement<IAreaSymbolizationFill>
    {
        /// <summary>
        /// Gets or sets the style of the polygon fill.
        /// </summary>
        IFill Fill { get; set; }

        /// <summary>
        /// Gets or sets the style of the polygon edge
        /// </summary>
        IStroke Stroke { get; set; }
    }

    /// <summary>
    /// The type of point symbol
    /// </summary>
    public enum PointSymbolType
    {
        /// <summary>
        /// A textual symbol
        /// </summary>
        Text,
        /// <summary>
        /// A predefined shape such as a square or circle.
        /// </summary>
        Mark,
        /// <summary>
        /// A raster or image symbol.  Note that these do not scale well, but sometimes this is all that you have.  Supported formats are application specific.
        /// </summary>
        Image,
        /// <summary>
        /// A symbol specified using a font character
        /// </summary>
        Font,
        /// <summary>
        /// A vector symbol defined using a W2D stream
        /// </summary>
        W2D,
        /// <summary>
        /// A vector symbol specifed from a block
        /// </summary>
        Block
    }

    /// <summary>
    /// Defines common properties for all symbols
    /// </summary>
    public interface ISymbol
    {
        /// <summary>
        /// Gets the type of symbol
        /// </summary>
        PointSymbolType Type { get; }

        /// <summary>
        /// Gets or sets the units that the sizes are specified in
        /// </summary>
        LengthUnitType Unit { get; set; }

        /// <summary>
        /// Gets or sets whether the sizes are with respect to the earth or the user's display device
        /// </summary>
        SizeContextType SizeContext { get; set; }

        /// <summary>
        /// Gets or sets the width of the symbol. This is a double FDO expression. Does not apply to font symbols
        /// </summary>
        string SizeX { get; set; }

        /// <summary>
        /// Gets or sets the height of the symbol. This is a double FDO expression.
        /// </summary>
        string SizeY { get; set; }

        /// <summary>
        /// Gets or sets the amount to rotate the symbol in degrees. This is a double FDO expression. Does not apply to line labels
        /// </summary>
        string Rotation { get; set; }

        /// <summary>
        /// Hint for the UI only. When the user enters a constant size for the width or height, the other dimension should be adjusted accordingly.  Does not apply to font symbols or labels.
        /// </summary>
        bool MaintainAspect { get; set; }

        /// <summary>
        /// Gets or sets the X offset for the symbol specified in symbol space. This is a double FDO expression. Does not apply to labels.
        /// </summary>
        string InsertionPointX { get; set; }

        /// <summary>
        /// Gets or sets the Y offset for the symbol specified in symbol space. This is a double FDO expression. Does not apply to labels.
        /// </summary>
        string InsertionPointY { get; set; }
    }

    /// <summary>
    /// Advanced placement settings
    /// </summary>
    public interface IAdvancedPlacement
    {
        /// <summary>
        /// Gets or sets the scale limit.
        /// </summary>
        /// <value>The scale limit.</value>
        double ScaleLimit { get; set; }
    }

    /// <summary>
    /// Represents a text symbol
    /// </summary>
    public interface ITextSymbol : ISymbol, ICloneableLayerElement<ITextSymbol>
    {
        /// <summary>
        /// Gets or sets the textual content
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the name of the font
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets the foreground color
        /// </summary>
        string ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background style
        /// </summary>
        BackgroundStyleType BackgroundStyle { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment
        /// </summary>
        string HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment
        /// </summary>
        string VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets whether to bold the text
        /// </summary>
        string Bold { get; set; }

        /// <summary>
        /// Gets or sets whether to italicize the text
        /// </summary>
        string Italic { get; set; }

        /// <summary>
        /// Gets or sets whether to underline the text
        /// </summary>
        string Underlined { get; set; }

        /// <summary>
        /// Gets or sets the advanced placement settings
        /// </summary>
        IAdvancedPlacement AdvancedPlacement { get; set; }
    }

    /// <summary>
    /// Stylization of a predefined shape (ShapeType)
    /// </summary>
    public interface IMarkSymbol : ISymbol, ICloneableLayerElement<IMarkSymbol>
    {
        /// <summary>
        /// Gets or sets the type of shape
        /// </summary>
        ShapeType Shape { get; set; }

        /// <summary>
        /// Gets or sets the fill settings
        /// </summary>
        IFill Fill { get; set; }

        /// <summary>
        /// Gets or sets the outline settings
        /// </summary>
        IStroke Edge { get; set; }
    }

    /// <summary>
    /// Symbols that are specified by a font and character.
    /// </summary>
    public interface IFontSymbol : ISymbol, ICloneableLayerElement<IFontSymbol>
    {
        /// <summary>
        /// Gets or sets the name of the font. If the font is not installed, the actual font used is application dependent.
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets the character
        /// </summary>
        string Character { get; set; }

        /// <summary>
        /// Gets or sets whether to bold the text
        /// </summary>
        bool? Bold { get; set; }

        /// <summary>
        /// Gets or sets whether to italicize the text
        /// </summary>
        bool? Italic { get; set; }

        /// <summary>
        /// Gets or sets whether to underline the text
        /// </summary>
        bool? Underlined { get; set; }

        /// <summary>
        /// Gets or sets the foreground color
        /// </summary>
        string ForegroundColor { get; set; }
    }

    /// <summary>
    /// Represents a DWF-based W2D symbol
    /// </summary>
    public interface IW2DSymbol : ISymbol, ICloneableLayerElement<IW2DSymbol>
    {
        /// <summary>
        /// Gets or sets the reference to the symbol
        /// </summary>
        ISymbolReference W2DSymbol { get; set; }

        /// <summary>
        /// Gets or sets the fill color
        /// </summary>
        string FillColor { get; set; }

        /// <summary>
        /// Gets or sets the line color
        /// </summary>
        string LineColor { get; set; }

        /// <summary>
        /// Gets or sets the text color
        /// </summary>
        string TextColor { get; set; }
    }

    /// <summary>
    /// Symbols that are comprised of a raster.
    /// </summary>
    public interface IImageSymbol : ISymbol, ICloneableLayerElement<IImageSymbol>
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        IBaseImageSymbol Image { get; set; }
    }

    /// <summary>
    /// The types of image symbol references
    /// </summary>
    public enum ImageSymbolReferenceType
    {
        /// <summary>
        /// 
        /// </summary>
        SymbolReference,
        /// <summary>
        /// 
        /// </summary>
        Inline
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IBaseImageSymbol
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        ImageSymbolReferenceType Type { get; }
    }

    /// <summary>
    /// Represents a reference to a symbol library item image
    /// </summary>
    public interface ISymbolReference : IBaseImageSymbol, ICloneableLayerElement<ISymbolReference>
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the library item.
        /// </summary>
        /// <value>The name of the library item.</value>
        string LibraryItemName { get; set; }
    }

    /// <summary>
    /// Represents an inline symbol image
    /// </summary>
    public interface IInlineImageSymbol : IBaseImageSymbol, ICloneableLayerElement<IInlineImageSymbol>
    {
        /// <summary>
        /// Gets or sets the BinHex data for image
        /// </summary>
        byte[] Content { get; set; }
    }

    /// <summary>
    /// Represents a block symbol
    /// </summary>
    public interface IBlockSymbol : ISymbol, ICloneableLayerElement<IBlockSymbol>
    {
        /// <summary>
        /// Gets or sets the name of the drawing
        /// </summary>
        string DrawingName { get; set; }

        /// <summary>
        /// Gets or sets the name of the block
        /// </summary>
        string BlockName { get; set; }

        /// <summary>
        /// Gets or sets the color of the block
        /// </summary>
        string BlockColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the layer
        /// </summary>
        string LayerColor { get; set; }
    }

    /// <summary>
    /// Symbolization characteristics for points.
    /// </summary>
    public interface IPointSymbolization2D : ICloneableLayerElement<IPointSymbolization2D>
    {
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        ISymbol Symbol { get; set; }
    }

    /// <summary>
    /// Represents a fill
    /// </summary>
    public interface IFill : ICloneableLayerElement<IFill>
    {
        /// <summary>
        /// Gets or sets the fill pattern
        /// </summary>
        string FillPattern { get; set; }

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color
        /// </summary>
        string ForegroundColor { get; set; }
    }

    #endregion

    #region raster layer

    /// <summary>
    /// Defines how to scale numbers into a color channel
    /// </summary>
    public interface IChannelBand
    {
        /// <summary>
        /// Gets or sets the name of the band
        /// </summary>
        string Band { get; set; }

        /// <summary>
        /// Gets or sets the low band value. Default is low value found in band.  Band values less than this are snapped to this number
        /// </summary>
        double? LowBand { get; set; }

        /// <summary>
        /// Gets or sets the high band value. Default is high value found in band.  Band values greater than this are snapped to this number
        /// </summary>
        double? HighBand { get; set; }

        /// <summary>
        /// Gets or sets the low channel value. Default is 0.  Range is 0:255.  LowBand is mapped to this number.  LowChannel can be greater than HighChannel
        /// </summary>
        byte LowChannel { get; set; }

        /// <summary>
        /// Gets or sets the high channel value. Default is 255.  Range is 0:255
        /// </summary>
        byte HighChannel { get; set; }
    }

    /// <summary>
    /// Specifies a color using distinct RGB values
    /// </summary>
    public interface IGridColorBands
    {
        /// <summary>
        /// Gets or sets the red channel band
        /// </summary>
        IChannelBand RedBand { get; set; }

        /// <summary>
        /// Gets or sets the green channel band
        /// </summary>
        IChannelBand GreenBand { get; set; }

        /// <summary>
        /// Gets or sets the blue channel band
        /// </summary>
        IChannelBand BlueBand { get; set; }
    }

    /// <summary>
    /// An explicit color
    /// </summary>
    public interface IExplicitColor
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        ItemChoiceType Type { get; }
    }

    /// <summary>
    /// An explicit color value
    /// </summary>
    public interface IExplictColorValue : IExplicitColor
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        string Value { get; }
    }

    /// <summary>
    /// An explicit color band
    /// </summary>
    public interface IExplicitColorBand : IExplicitColor
    {
        /// <summary>
        /// Gets the band.
        /// </summary>
        /// <value>The band.</value>
        string Band { get; }
    }

    /// <summary>
    /// An explicit color band
    /// </summary>
    public interface IExplicitColorBands : IExplicitColor
    {
        /// <summary>
        /// Gets the bands.
        /// </summary>
        /// <value>The bands.</value>
        IGridColorBands Bands { get; }
    }

    /// <summary>
    /// A grid color
    /// </summary>
    public interface IGridColor
    {
        /// <summary>
        /// Gets or sets the color of the explicit.
        /// </summary>
        /// <value>The color of the explicit.</value>
        IExplicitColor ExplicitColor { get; set; }

        /// <summary>
        /// Set the color
        /// </summary>
        /// <param name="htmlColor"></param>
        void SetValue(string htmlColor);

        /// <summary>
        /// Gets the html color value
        /// </summary>
        /// <returns></returns>
        string GetValue();
    }

    /// <summary>
    /// A grid color rule
    /// </summary>
    public interface IGridColorRule
    {
        /// <summary>
        /// Gets or sets the label for the rule to be displayed in the legend
        /// </summary>
        string LegendLabel { get; set; }

        /// <summary>
        /// Gets or sets a filter for the rule.  This is a boolean FDO expression.  Any features that pass this filter are styled using this rule's stylization
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Gets or sets a label for the rule.  Does not apply to GridColorRule
        /// </summary>
        ITextSymbol Label { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        IGridColor Color { get; set; }
    }

    /// <summary>
    /// Specifies how to shade given a band and a light source
    /// </summary>
    public interface IHillShade
    {
        /// <summary>
        /// Gets or sets the name of the band used for the computation
        /// </summary>
        string Band { get; set; }

        /// <summary>
        /// Gets or sets the azimuth of the sun in degrees
        /// </summary>
        double Azimuth { get; set; }

        /// <summary>
        /// Gets or sets the altitude of the sun in degrees
        /// </summary>
        double Altitude { get; set; }

        /// <summary>
        /// Gets or sets the scale factor applied to the band prior to computing hillshade.  Defaults to 1 if not specified
        /// </summary>
        double ScaleFactor { get; set; }
    }

    /// <summary>
    /// A grid color style
    /// </summary>
    public interface IGridColorStyle
    {
        /// <summary>
        /// Gets or sets the hill shade.
        /// </summary>
        /// <value>The hill shade.</value>
        IHillShade HillShade { get; set; }

        /// <summary>
        /// Gets or sets the transparency color. If a pixel color prior to factoring in HillShade is this value then the pixel is transparent
        /// </summary>
        string TransparencyColor { get; set; }

        /// <summary>
        /// Gets or sets the brightness factor
        /// </summary>
        double? BrightnessFactor { get; set; }

        /// <summary>
        /// Gets or sets the contrast factor
        /// </summary>
        double? ContrastFactor { get; set; }

        /// <summary>
        /// Gets the color rules for this style
        /// </summary>
        IEnumerable<IGridColorRule> ColorRule { get; }

        /// <summary>
        /// Gets the number of color rules
        /// </summary>
        int ColorRuleCount { get; }

        /// <summary>
        /// Gets the color rule at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IGridColorRule GetColorRuleAt(int index);

        /// <summary>
        /// Adds a color rule to this style
        /// </summary>
        /// <param name="rule"></param>
        void AddColorRule(IGridColorRule rule);

        /// <summary>
        /// Removes the specified color rule from this style
        /// </summary>
        /// <param name="rule"></param>
        void RemoveColorRule(IGridColorRule rule);

        /// <summary>
        /// Creates a default hillshade
        /// </summary>
        /// <returns></returns>
        IHillShade CreateHillShade();
    }

    /// <summary>
    /// A grid surface style
    /// </summary>
    public interface IGridSurfaceStyle
    {
        /// <summary>
        /// Gets or sets the band to use for 3D data
        /// </summary>
        string Band { get; set; }

        /// <summary>
        /// Gets or sets the value that determines which input value is mapped to zero elevation.  Defaults to 0 if not specified
        /// </summary>
        double ZeroValue { get; set; }

        /// <summary>
        /// Gets or sets the value that determines how to scale the inputs into a consistent elevation.  Defaults to 1 if not specified
        /// </summary>
        double ScaleFactor { get; set; }

        /// <summary>
        /// Gets or sets the default color to use if no ColorStyle is defined at a pixel
        /// </summary>
        string DefaultColor { get; set; }
    }

    /// <summary>
    /// A grid scale range
    /// </summary>
    public interface IGridScaleRange
    {
        /// <summary>
        /// Gets or sets the zoomed in part of the scale range.  Defaults to 0 if not specified.  Inclusive
        /// </summary>
        double? MinScale { get; set; }

        /// <summary>
        /// Gets or sets the zoomed out part of the scale range.  Defaults to the application's maximum value if not specified.  Exclusive
        /// </summary>
        double? MaxScale { get; set; }

        /// <summary>
        /// Defines the height field of the grid
        /// </summary>
        IGridSurfaceStyle SurfaceStyle { get; set; }

        /// <summary>
        /// Gets or sets the color style.
        /// </summary>
        /// <value>The color style.</value>
        IGridColorStyle ColorStyle { get; set; }

        /// <summary>
        /// When the user has zoomed in by this amount, a request for more detailed raster data is made
        /// </summary>
        double RebuildFactor { get; set; }

        /// <summary>
        /// Creates a default grid color style
        /// </summary>
        /// <returns></returns>
        IGridColorStyle CreateColorStyle();

        /// <summary>
        /// Creates a default grid surface style
        /// </summary>
        /// <returns></returns>
        IGridSurfaceStyle CreateSurfaceStyle();
    }

    #endregion

    #region Layer Definition 1.1.0 interfaces

    /// <summary>
    /// Type of elevation
    /// </summary>
    [System.SerializableAttribute()]
    public enum ElevationTypeType
    {

        /// <remarks/>
        RelativeToGround,

        /// <remarks/>
        Absolute,
    }

    /// <summary>
    /// Represents a composite style definition
    /// </summary>
    public interface ICompositeTypeStyle : IVectorStyle, IRuleCollection<ICompositeRule>
    {
        /// <summary>
        /// Gets a display string for data-binding purposes
        /// </summary>
        string DisplayString { get; }

        /// <summary>
        /// Gets the composite rules.
        /// </summary>
        /// <value>The composite rules.</value>
        IEnumerable<ICompositeRule> CompositeRule { get; }

        /// <summary>
        /// Adds the composite rule.
        /// </summary>
        /// <param name="compRule">The comp rule.</param>
        void AddCompositeRule(ICompositeRule compRule);

        /// <summary>
        /// Removes the composite rule.
        /// </summary>
        /// <param name="compRule">The comp rule.</param>
        void RemoveCompositeRule(ICompositeRule compRule);
    }

    /// <summary>
    /// Represents a composite rule
    /// </summary>
    public interface ICompositeRule : IVectorRule
    {
        /// <summary>
        /// Gets or sets the composite symbolization.
        /// </summary>
        /// <value>The composite symbolization.</value>
        ICompositeSymbolization CompositeSymbolization { get; set; }
    }

    /// <summary>
    /// Represents a composite symbolization
    /// </summary>
    public interface ICompositeSymbolization
    {
        /// <summary>
        /// Gets the symbol instances.
        /// </summary>
        /// <value>The symbol instances.</value>
        IEnumerable<ISymbolInstance> SymbolInstance { get; }

        /// <summary>
        /// Adds the symbol instance.
        /// </summary>
        /// <param name="inst">The inst.</param>
        void AddSymbolInstance(ISymbolInstance inst);

        /// <summary>
        /// Removes the symbol instance.
        /// </summary>
        /// <param name="inst">The inst.</param>
        void RemoveSymbolInstance(ISymbolInstance inst);

        /// <summary>
        /// Creates a symbol reference.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        ISymbolInstance CreateSymbolReference(string resourceId);

        /// <summary>
        /// Creates an inline simple symbol instance
        /// </summary>
        /// <param name="symDef"></param>
        /// <returns></returns>
        ISymbolInstance CreateInlineSimpleSymbol(ISimpleSymbolDefinition symDef);

        /// <summary>
        /// Creates an inline compound symbol instance
        /// </summary>
        /// <param name="compDef"></param>
        /// <returns></returns>
        ISymbolInstance CreateInlineCompoundSymbol(ICompoundSymbolDefinition compDef);
    }

    /// <summary>
    /// Represents a parameter override
    /// </summary>
    public interface IParameterOverride
    {
        /// <summary>
        /// Gets or sets the name of the symbol definition containing that parameter being overridden
        /// </summary>
        string SymbolName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parameter being overridden
        /// </summary>
        string ParameterIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the override value for the parameter
        /// </summary>
        string ParameterValue { get; set; }
    }

    /// <summary>
    /// A collection of parameter overrides
    /// </summary>
    public interface IParameterOverrideCollection
    {
        /// <summary>
        /// Gets the parameter overrides.
        /// </summary>
        /// <value>The parameter overrides.</value>
        IEnumerable<IParameterOverride> Override { get; }

        /// <summary>
        /// Gets the number of parameter overrides
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the parameter override at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IParameterOverride this[int index] { get; }

        /// <summary>
        /// Adds the parameter override.
        /// </summary>
        /// <param name="ov">The parameter override</param>
        void AddOverride(IParameterOverride ov);

        /// <summary>
        /// Removes the parameter override.
        /// </summary>
        /// <param name="ov">The parameter override</param>
        void RemoveOverride(IParameterOverride ov);

        /// <summary>
        /// Creates a parameter override
        /// </summary>
        /// <param name="symbol">The symbol name</param>
        /// <param name="name">The name of the parameter to override</param>
        /// <returns></returns>
        IParameterOverride CreateParameterOverride(string symbol, string name);
    }

    /// <summary>
    /// Represents elevation settings
    /// </summary>
    public interface IElevationSettings
    {
        /// <summary>
        /// Gets or sets the Z offset.
        /// </summary>
        /// <value>The Z offset.</value>
        string ZOffset { get; set; }
        /// <summary>
        /// Gets or sets the Z extrusion.
        /// </summary>
        /// <value>The Z extrusion.</value>
        string ZExtrusion { get; set; }
        /// <summary>
        /// Gets or sets the type of the Z offset.
        /// </summary>
        /// <value>The type of the Z offset.</value>
        ElevationTypeType ZOffsetType { get; set; }
        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>The unit.</value>
        LengthUnitType Unit { get; set; }
    }

    #endregion

    #region Layer Definition 1.2.0 interfaces

    /// <summary>
    /// The types of usage context
    /// </summary>
    [System.SerializableAttribute()]
    public enum UsageContextType
    {

        /// <remarks/>
        Unspecified,

        /// <remarks/>
        Point,

        /// <remarks/>
        Line,

        /// <remarks/>
        Area,
    }

    /// <summary>
    /// The types of geometry context
    /// </summary>
    [System.SerializableAttribute()]
    public enum GeometryContextType
    {

        /// <remarks/>
        Unspecified,

        /// <remarks/>
        Point,

        /// <remarks/>
        LineString,

        /// <remarks/>
        Polygon,
    }

    /// <summary>
    /// Provides legend labeling information for a theme
    /// </summary>
    public interface IThemeLabel
    {
        /// <summary>
        /// Gets or sets the legend description for the theme
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the default legend format to use for each category
        /// </summary>
        string CategoryFormat { get; set; }
    }

    /// <summary>
    /// Version 2 of parameter overrides that supports theme labels. Applies to v1.2.0 of the Layer Definition schema
    /// </summary>
    public interface IParameterOverride2 : IParameterOverride
    {
        /// <summary>
        /// Gets or sets the theme label.
        /// </summary>
        /// <value>The theme label.</value>
        IThemeLabel ThemeLabel { get; set; }
    }

    /// <summary>
    /// Version 2 of symbol instance that supports rendering passes and specific contexts. Applies to v1.2.0 of the Layer Definition schema
    /// </summary>
    public interface ISymbolInstance2 : ISymbolInstance
    {
        /// <summary>
        /// Gets or sets the rendering pass.
        /// </summary>
        /// <value>The rendering pass.</value>
        string RenderingPass { get; set; }

        /// <summary>
        /// Gets or sets the usage context.
        /// </summary>
        /// <value>The usage context.</value>
        UsageContextType UsageContext { get; set; }

        /// <summary>
        /// Gets or sets the geometry context.
        /// </summary>
        /// <value>The geometry context.</value>
        GeometryContextType GeometryContext { get; set; }
    }

    #endregion

    #region Layer Definition 1.3.0 interfaces

    /// <summary>
    /// A point vector style introduced in the v1.3.0 layer definition schema
    /// </summary>
    public interface IPointVectorStyle2 : IPointVectorStyle
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show in legend].
        /// </summary>
        /// <value><c>true</c> if [show in legend]; otherwise, <c>false</c>.</value>
        bool ShowInLegend { get; set; }
    }

    /// <summary>
    /// A line vector style introduced in the v1.3.0 layer definition schema
    /// </summary>
    public interface ILineVectorStyle2 : ILineVectorStyle
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show in legend].
        /// </summary>
        /// <value><c>true</c> if [show in legend]; otherwise, <c>false</c>.</value>
        bool ShowInLegend { get; set; }
    }

    /// <summary>
    /// An area vector style introduced in the v1.3.0 layer definition schema
    /// </summary>
    public interface IAreaVectorStyle2 : IAreaVectorStyle
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show in legend].
        /// </summary>
        /// <value><c>true</c> if [show in legend]; otherwise, <c>false</c>.</value>
        bool ShowInLegend { get; set; }
    }

    /// <summary>
    /// A composite style introduced in the v1.3.0 layer definition schema
    /// </summary>
    public interface ICompositeTypeStyle2 : ICompositeTypeStyle
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show in legend].
        /// </summary>
        /// <value><c>true</c> if [show in legend]; otherwise, <c>false</c>.</value>
        bool ShowInLegend { get; set; }
    }

    #endregion

    #region Layer Definition 2.3.0 interfaces
    #endregion
}
