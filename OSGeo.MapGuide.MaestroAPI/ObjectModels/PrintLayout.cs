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
using OSGeo.MapGuide.ObjectModels.PrintLayout;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0
{
    partial class PrintLayout : IPrintLayout
    {
        internal PrintLayout() { }

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
                    throw new InvalidOperationException(OSGeo.MapGuide.MaestroAPI.Strings.ErrorInvalidResourceIdentifier);

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.PrintLayout.ToString())
                    throw new InvalidOperationException(string.Format(OSGeo.MapGuide.MaestroAPI.Strings.ErrorUnexpectedResourceType, res.ToString(), ResourceTypes.PrintLayout));

                _resId = value;
                this.OnPropertyChanged("ResourceID"); //NOXLATE
            }
        }

        [XmlIgnore]
        public ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.PrintLayout;
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
            get { return "PrintLayout-1.0.0.xsd"; } //NOXLATE
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        [XmlIgnore]
        IPrintLayoutPageProperties IPrintLayout.PageProperties
        {
            get { return this.PageProperties; }
        }

        [XmlIgnore]
        IPrintLayoutProperties IPrintLayout.LayoutProperties
        {
            get { return this.LayoutProperties; }
        }

        [XmlIgnore]
        IEnumerable<ILogo> IPrintLayout.CustomLogos
        {
            get 
            {
                foreach (var logo in this.CustomLogos)
                {
                    yield return logo;
                }
            }
        }

        void IPrintLayout.RemoveLogo(ILogo logo)
        {
            var l = logo as PrintLayoutLogo;
            if (l != null)
            {
                this.CustomLogos.Remove(l);
                OnPropertyChanged("CustomLogos"); //NOXLATE
            }
        }

        [XmlIgnore]
        IEnumerable<IText> IPrintLayout.CustomText
        {
            get 
            {
                foreach (var text in this.CustomText)
                {
                    yield return text;
                }
            }
        }

        void IPrintLayout.AddText(IText text)
        {
            var t = text as PrintLayoutText;
            if (t != null)
            {
                this.CustomText.Add(t);
                OnPropertyChanged("CustomText"); //NOXLATE
            }
        }

        void IPrintLayout.RemoveText(IText text)
        {
            var t = text as PrintLayoutText;
            if (t != null)
            {
                this.CustomText.Remove(t);
                OnPropertyChanged("CustomText"); //NOXLATE
            }
        }

        ISize IPrintLayout.CreateLogoSize(float width, float height, string units)
        {
            return new PrintLayoutLogoSize() { Width = width, Height = height, Units = units };
        }

        IPosition IPrintLayout.CreateLogoPosition(float left, float bottom, string units)
        {
            return new PrintLayoutLogoPosition() { Left = left, Bottom = bottom, Units = units };
        }

        IPosition IPrintLayout.CreateTextPosition(float left, float bottom, string units)
        {
            return new PrintLayoutTextPosition() { Left = left, Bottom = bottom, Units = units };
        }

        IFont IPrintLayout.CreateFont(string name, float height, string units)
        {
            return new PrintLayoutTextFont() { Name = name, Height = height, Units = units };
        }

        ILogo IPrintLayout.CreateLogo(string symbolLibraryId, string symbolName, ISize size, IPosition position)
        {
            var logo = new PrintLayoutLogo()
            {
                Name = symbolName,
                ResourceId = symbolLibraryId,
                Position = (PrintLayoutLogoPosition)position,
                Size = (PrintLayoutLogoSize)size
            };

            return logo;
        }

        void IPrintLayout.AddLogo(ILogo logo)
        {
            var l = logo as PrintLayoutLogo;
            if (l != null)
            {
                this.CustomLogos.Add(l);
                OnPropertyChanged("CustomLogos"); //NOXLATE
            }
        }

        IText IPrintLayout.CreateText(string value, IFont font, IPosition pos)
        {
            var text = new PrintLayoutText()
            {
                Font = (PrintLayoutTextFont)font,
                Position = (PrintLayoutTextPosition)pos,
                Value = value
            };
            return text;
        }
    }

    partial class PrintLayoutPageProperties : IPrintLayoutPageProperties
    {
        [XmlIgnore]
        System.Drawing.Color IPrintLayoutPageProperties.BackgroundColor
        {
            get
            {
                int r;
                int g;
                int b;
                if (int.TryParse(this.BackgroundColor.Red, out r) &&
                    int.TryParse(this.BackgroundColor.Green, out g) &&
                    int.TryParse(this.BackgroundColor.Blue, out b))
                {
                    return System.Drawing.Color.FromArgb(r, g, b);
                }
                return new System.Drawing.Color();
            }
            set
            {
                this.BackgroundColor.Red = value.R.ToString();
                this.BackgroundColor.Green = value.G.ToString();
                this.BackgroundColor.Blue = value.B.ToString();
            }
        }
    }

    partial class PrintLayoutLayoutProperties : IPrintLayoutProperties
    {

    }

    partial class PrintLayoutLogo : ILogo
    {
        [XmlIgnore]
        IPosition ILogo.Position
        {
            get { return this.Position; }
        }

        [XmlIgnore]
        ISize ILogo.Size
        {
            get { return this.Size; }
        }

        [XmlIgnore]
        float? ILogo.Rotation
        {
            get
            {
                return this.RotationSpecified ? new Nullable<float>(this.Rotation) : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.Rotation = value.Value;
                    this.RotationSpecified = true;
                }
                else
                {
                    this.RotationSpecified = false;
                }
            }
        }
    }

    partial class PrintLayoutLogoPosition : IPosition
    {

    }

    partial class PrintLayoutLogoSize : ISize
    {

    }

    partial class PrintLayoutText : IText
    {
        [XmlIgnore]
        IPosition IText.Position
        {
            get { return this.Position; }
        }

        [XmlIgnore]
        IFont IText.Font
        {
            get { return this.Font; }
        }
    }

    partial class PrintLayoutTextFont : IFont
    {

    }

    partial class PrintLayoutTextPosition : IPosition
    { 
    
    }
}
