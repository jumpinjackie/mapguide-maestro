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
using System.Drawing;
using System.ComponentModel;

namespace OSGeo.MapGuide.ObjectModels.PrintLayout
{
    /// <summary>
    /// Print Layouts
    /// </summary>
    public interface IPrintLayout : IResource
    {
        /// <summary>
        /// Gets the page properties.
        /// </summary>
        /// <value>The page properties.</value>
        IPrintLayoutPageProperties PageProperties { get; }
        /// <summary>
        /// Gets the layout properties.
        /// </summary>
        /// <value>The layout properties.</value>
        IPrintLayoutProperties LayoutProperties { get; }

        /// <summary>
        /// Creates the size of the logo.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        ISize CreateLogoSize(float width, float height, string units);
        /// <summary>
        /// Creates the font.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="height">The height.</param>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        IFont CreateFont(string name, float height, string units);
        /// <summary>
        /// Creates the logo position.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        IPosition CreateLogoPosition(float left, float bottom, string units);
        /// <summary>
        /// Creates the text position.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        IPosition CreateTextPosition(float left, float bottom, string units);

        /// <summary>
        /// Gets the custom logos.
        /// </summary>
        /// <value>The custom logos.</value>
        IEnumerable<ILogo> CustomLogos { get; }
        /// <summary>
        /// Creates the logo.
        /// </summary>
        /// <param name="symbolLibraryId">The symbol library id.</param>
        /// <param name="symbolName">Name of the symbol.</param>
        /// <param name="size">The size.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        ILogo CreateLogo(string symbolLibraryId, string symbolName, ISize size, IPosition position);
        /// <summary>
        /// Adds the logo.
        /// </summary>
        /// <param name="logo">The logo.</param>
        void AddLogo(ILogo logo);
        /// <summary>
        /// Removes the logo.
        /// </summary>
        /// <param name="logo">The logo.</param>
        void RemoveLogo(ILogo logo);

        /// <summary>
        /// Gets the custom text elements.
        /// </summary>
        /// <value>The custom text elements.</value>
        IEnumerable<IText> CustomText { get; }
        /// <summary>
        /// Creates the text element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="font">The font.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        IText CreateText(string value, IFont font, IPosition text);
        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="text">The text.</param>
        void AddText(IText text);
        /// <summary>
        /// Removes the text.
        /// </summary>
        /// <param name="text">The text.</param>
        void RemoveText(IText text);
    }

    /// <summary>
    /// Page properties of the print layout
    /// </summary>
    public interface IPrintLayoutPageProperties : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        Color BackgroundColor { get; set; }
    }

    /// <summary>
    /// Layout properties of the print layout
    /// </summary>
    public interface IPrintLayoutProperties : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show title].
        /// </summary>
        /// <value><c>true</c> if [show title]; otherwise, <c>false</c>.</value>
        bool ShowTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show legend].
        /// </summary>
        /// <value><c>true</c> if [show legend]; otherwise, <c>false</c>.</value>
        bool ShowLegend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show scale bar].
        /// </summary>
        /// <value><c>true</c> if [show scale bar]; otherwise, <c>false</c>.</value>
        bool ShowScaleBar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show north arrow].
        /// </summary>
        /// <value><c>true</c> if [show north arrow]; otherwise, <c>false</c>.</value>
        bool ShowNorthArrow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show URL].
        /// </summary>
        /// <value><c>true</c> if [show URL]; otherwise, <c>false</c>.</value>
        bool ShowURL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show date time].
        /// </summary>
        /// <value><c>true</c> if [show date time]; otherwise, <c>false</c>.</value>
        bool ShowDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show custom logos].
        /// </summary>
        /// <value><c>true</c> if [show custom logos]; otherwise, <c>false</c>.</value>
        bool ShowCustomLogos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show custom text].
        /// </summary>
        /// <value><c>true</c> if [show custom text]; otherwise, <c>false</c>.</value>
        bool ShowCustomText { get; set; }
    }

    /// <summary>
    /// Represents a position
    /// </summary>
    public interface IPosition : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the left margin.
        /// </summary>
        /// <value>The left margin.</value>
        float Left { get; set; }
        /// <summary>
        /// Gets or sets the bottom margin.
        /// </summary>
        /// <value>The bottom margin.</value>
        float Bottom { get; set; }
        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        string Units { get; set; }
    }

    /// <summary>
    /// Represents a size
    /// </summary>
    public interface ISize : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        float Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        float Height { get; set; }
        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        string Units { get; set; }
    }

    /// <summary>
    /// Represents a logo
    /// </summary>
    public interface ILogo : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        IPosition Position { get; }
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        ISize Size { get; }
        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        float? Rotation { get; set; }
    }

    /// <summary>
    /// Represents a font
    /// </summary>
    public interface IFont : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        float Height { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        string Units { get; set; }
    }

    /// <summary>
    /// Represents a text element
    /// </summary>
    public interface IText : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        IPosition Position { get; }

        /// <summary>
        /// Gets the font.
        /// </summary>
        /// <value>The font.</value>
        IFont Font { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        string Value { get; set; }
    }
}
