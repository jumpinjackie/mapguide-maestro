#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.DrawingSource.v1_0_0
{
    partial class DrawingSource : IDrawingSource
    {
        internal DrawingSource()
        {
        }

        private static readonly Version RES_VERSION = new Version(1, 0, 0);

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
                    throw new InvalidOperationException(Strings.ErrorInvalidResourceIdentifier); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.DrawingSource.ToString())
                    throw new InvalidOperationException(string.Format(Strings.ErrorUnexpectedResourceType, res.ToString(), ResourceTypes.DrawingSource)); //LOCALIZE

                _resId = value;
                this.OnPropertyChanged("ResourceID"); //NOXLATE
            }
        }

        [XmlIgnore]
        public string ResourceType
        {
            get
            {
                return ResourceTypes.DrawingSource.ToString();
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
            get { return "DrawingSource-1.0.0.xsd"; } //NOXLATE
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        void IDrawingSource.RemoveAllSheets()
        {
            this.Sheet.Clear();
        }

        [XmlIgnore]
        IEnumerable<IDrawingSourceSheet> IDrawingSource.Sheet
        {
            get
            {
                foreach (var sht in this.Sheet)
                {
                    yield return sht;
                }
            }
        }

        IDrawingSourceSheet IDrawingSource.CreateSheet(string name, double minx, double miny, double maxx, double maxy)
        {
            return new DrawingSourceSheet()
            {
                Extent = new DrawingSourceSheetExtent()
                {
                    MinX = minx,
                    MinY = miny,
                    MaxX = maxx,
                    MaxY = maxy
                },
                Name = name
            };
        }

        void IDrawingSource.AddSheet(IDrawingSourceSheet sheet)
        {
            var sht = sheet as DrawingSourceSheet;
            if (sht != null)
                this.Sheet.Add(sht);
        }

        void IDrawingSource.RemoveSheet(IDrawingSourceSheet sheet)
        {
            var sht = sheet as DrawingSourceSheet;
            if (sht != null)
                this.Sheet.Remove(sht);
        }
    }

    partial class DrawingSourceSheetExtent : IEnvelope
    { }

    partial class DrawingSourceSheet : IDrawingSourceSheet
    {
        [XmlIgnore]
        IEnvelope IDrawingSourceSheet.Extent
        {
            get
            {
                return this.Extent;
            }
            set
            {
                if (value == null)
                {
                    this.Extent = null;
                }
                else
                {
                    if (this.Extent == null)
                        this.Extent = new DrawingSourceSheetExtent();

                    this.Extent.MaxX = value.MaxX;
                    this.Extent.MaxY = value.MaxY;
                    this.Extent.MinX = value.MinX;
                    this.Extent.MinY = value.MinY;
                }
            }
        }
    }
}