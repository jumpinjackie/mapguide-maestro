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
using OSGeo.MapGuide.ObjectModels.SymbolLibrary;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.SymbolLibrary_1_0_0
{
    partial class SymbolLibraryType : ISymbolLibrary
    {
        internal SymbolLibraryType() { }

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
                    throw new InvalidOperationException("Not a valid resource identifier"); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.SymbolLibrary.ToString())
                    throw new InvalidOperationException("Invalid resource identifier for this type of object: " + res.Extension); //LOCALIZE

                _resId = value;
                this.OnPropertyChanged("ResourceID");
            }
        }

        [XmlIgnore]
        public ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.SymbolLibrary;
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

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string ValidatingSchema 
        { 
            get { return "SymbolLibrary-1.0.0.xsd"; }
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        IEnumerable<ISymbol> ISymbolLibrary.Symbol
        {
            get
            {
                foreach (var sym in this.Symbol)
                {
                    yield return sym;
                }
            }
        }
    }

    partial class SymbolType : ISymbol
    {
    }
}
