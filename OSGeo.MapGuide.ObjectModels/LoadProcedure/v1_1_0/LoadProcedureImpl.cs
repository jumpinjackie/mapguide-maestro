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

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_1_0
{
    partial class LoadProcedure : ILoadProcedure
    {
        private static readonly Version RES_VERSION = new Version(1, 1, 0);

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
                if (res.Extension != ResourceTypes.LoadProcedure.ToString())
                    throw new InvalidOperationException(string.Format(Strings.ErrorUnexpectedResourceType, res.ToString(), ResourceTypes.LoadProcedure));

                _resId = value;
                this.OnPropertyChanged(nameof(ResourceID));
            }
        }

        [XmlIgnore]
        public string ResourceType => ResourceTypes.LoadProcedure.ToString();

        [XmlIgnore]
        public Version ResourceVersion => RES_VERSION;

        object ICloneable.Clone() => this.Clone();

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")] //NOXLATE
        public string ValidatingSchema
        {
            get { return "LoadProcedure-1.1.0.xsd"; } //NOXLATE
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped => true;

        [XmlIgnore]
        IBaseLoadProcedure ILoadProcedure.SubType => this.Item;
    }

    abstract partial class LoadProcedureType : IBaseLoadProcedure
    {
        public void AddFile(string file)
        {
            if (!this.sourceFileField.Contains(file))
            {
                this.sourceFileField.Add(file);
            }
        }

        public void AddFiles(IEnumerable<string> files)
        {
            Check.ArgumentNotNull(files, nameof(files));

            var _files = this.sourceFileField;
            foreach (var f in files)
            {
                if (!_files.Contains(f))
                    _files.Add(f);
            }
        }

        public void RemoveFile(string file)
        {
            Check.ArgumentNotEmpty(file, nameof(file));

            if (this.sourceFileField.Contains(file))
            {
                this.sourceFileField.Remove(file);
            }
        }

        [XmlIgnore]
        bool? IBaseLoadProcedure.GenerateMaps
        {
            get
            {
                return this.GenerateMapsSpecified ?
                    new Nullable<bool>(this.GenerateMaps) :
                    null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.GenerateMaps = value.Value;
                    this.GenerateMapsSpecified = true;
                }
                else
                {
                    this.GenerateMapsSpecified = false;
                }
            }
        }

        [XmlIgnore]
        bool? IBaseLoadProcedure.GenerateSymbolLibraries
        {
            get
            {
                return this.GenerateSymbolLibrariesSpecified ?
                    new Nullable<bool>(this.GenerateSymbolLibraries) :
                    null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.GenerateSymbolLibraries = value.Value;
                    this.GenerateSymbolLibrariesSpecified = true;
                }
                else
                {
                    this.GenerateSymbolLibrariesSpecified = false;
                }
            }
        }

        [XmlIgnore]
        public abstract LoadType Type { get; }
    }

    partial class SdfLoadProcedureType : ISdfLoadProcedure
    {
        [XmlIgnore]
        public override LoadType Type => LoadType.Sdf;
    }

    partial class ShpLoadProcedureType : IShpLoadProcedure
    {
        [XmlIgnore]
        public override LoadType Type => LoadType.Shp;
    }

    partial class DwfLoadProcedureType : IDwfLoadProcedure
    {
        [XmlIgnore]
        public override LoadType Type => LoadType.Dwf;
    }

    partial class DwgLoadProcedureType : IDwgLoadProcedure
    {
        [XmlIgnore]
        public override LoadType Type => LoadType.Dwg;
    }

    partial class RasterLoadProcedureType : IRasterLoadProcedure
    {
        [XmlIgnore]
        public override LoadType Type => LoadType.Raster;
    }
}