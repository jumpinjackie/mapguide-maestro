#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

namespace OSGeo.MapGuide.ObjectModels.Common
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TileProviderList
    {

        private TileProvider[] tileProviderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TileProvider")]
        public TileProvider[] TileProvider
        {
            get
            {
                return this.tileProviderField;
            }
            set
            {
                this.tileProviderField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TileProvider
    {

        private string nameField;

        private string displayNameField;

        private string descriptionField;

        private TileProviderConnectionProperty[] connectionPropertiesField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string DisplayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ConnectionProperty", IsNullable = false)]
        public TileProviderConnectionProperty[] ConnectionProperties
        {
            get
            {
                return this.connectionPropertiesField;
            }
            set
            {
                this.connectionPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TileProviderConnectionProperty
    {

        private string nameField;

        private string localizedNameField;

        private string defaultValueField;

        private string[] valueField;

        private bool enumerableField;

        private bool protectedField;

        private bool requiredField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string LocalizedName
        {
            get
            {
                return this.localizedNameField;
            }
            set
            {
                this.localizedNameField = value;
            }
        }

        /// <remarks/>
        public string DefaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public string[] Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public bool Enumerable
        {
            get
            {
                return this.enumerableField;
            }
            set
            {
                this.enumerableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public bool Protected
        {
            get
            {
                return this.protectedField;
            }
            set
            {
                this.protectedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute]
        public bool Required
        {
            get
            {
                return this.requiredField;
            }
            set
            {
                this.requiredField = value;
            }
        }
    }
}
