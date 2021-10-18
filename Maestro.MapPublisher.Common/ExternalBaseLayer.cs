#region Disclaimer / License

// Copyright (C) 2019, Jackie Ng
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

using Maestro.MapPublisher.Common.Serialization;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Maestro.MapPublisher.Common
{
    [JsonConverter(typeof(ExternalBaseLayerConverter))]
    public abstract class ExternalBaseLayer : INamedLayer, INotifyPropertyChanged
    {
        public abstract ExternalBaseLayerType Type { get; }

        private string _name;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private bool _visible;

        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    OnPropertyChanged(nameof(Visible));
                }
            }
        }

        public override string ToString() => $"{Name} ({Type})";
    }

    public class OSMBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.OSM;
    }

    public enum StamenLayerType
    {
        Toner,
        Terrain,
        WaterColor
    }

    public class StamenBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.Stamen;

        private StamenLayerType _layerType;

        public StamenLayerType LayerType
        {
            get { return _layerType; }
            set
            {
                if (_layerType != value)
                {
                    _layerType = value;
                    OnPropertyChanged(nameof(LayerType));
                }
            }
        }
    }

    public class BingMapsBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.BingMaps;

        private string _layerType;

        public string LayerType
        {
            get { return _layerType; }
            set
            {
                if (_layerType != value)
                {
                    _layerType = value;
                    OnPropertyChanged(nameof(LayerType));
                }
            }
        }

        private string _apiKey;

        public string ApiKey
        {
            get { return _apiKey; }
            set
            {
                if (_apiKey != value)
                {
                    _apiKey = value;
                    OnPropertyChanged(nameof(ApiKey));
                }
            }
        }
    }

    public class XYZBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.XYZ;

        private string _urlTemplate;

        public string UrlTemplate
        {
            get { return _urlTemplate; }
            set
            {
                if (_urlTemplate != value)
                {
                    _urlTemplate = value;
                    OnPropertyChanged(nameof(UrlTemplate));
                }
            }
        }
    }
}
