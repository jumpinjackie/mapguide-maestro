#region Disclaimer / License

// Copyright (C) 2023, Jackie Ng
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

using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Maestro.Editors.Fusion
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class CustomProjections
    {

        private CustomProjectionEntry[] projectionField;

        /// <remarks/>
        [XmlElement("Projection")]
        public CustomProjectionEntry[] Projection
        {
            get => this.projectionField;
            set => this.projectionField = value;
        }
    }

    /// <remarks/>
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class CustomProjectionEntry : INotifyPropertyChanged
    {

        private ushort epsgField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public ushort epsg
        {
            get => this.epsgField;
            set
            {
                if (this.epsgField != value)
                {
                    this.epsgField = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(epsg)));
                }
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get => this.valueField;
            set
            {
                if (this.valueField != value)
                {
                    this.valueField = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
