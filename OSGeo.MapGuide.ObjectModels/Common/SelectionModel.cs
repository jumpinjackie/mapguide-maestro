#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.ObjectModels.SelectionModel
{
    [XmlRoot(ElementName = "Class")]
    public class Class
    {
        [XmlElement(ElementName = "ID")]
        public List<string> ID { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "Layer")]
    public class Layer
    {
        [XmlElement(ElementName = "Class")]
        public Class Class { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "FeatureSet")]
    public class FeatureSet
    {
        [XmlElement(ElementName = "Layer")]
        public List<Layer> Layer { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string NoNamespaceSchemaLocation { get; set; }
    }

    [XmlRoot(ElementName = "Property")]
    public class Property
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "DisplayName")]
        public string DisplayName { get; set; }
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "LayerMetadata")]
    public class LayerMetadata
    {
        [XmlElement(ElementName = "Property")]
        public List<Property> Property { get; set; }
    }

    [XmlRoot(ElementName = "Feature")]
    public class Feature
    {
        [XmlElement(ElementName = "Property")]
        public List<Property> Property { get; set; }
    }

    [XmlRoot(ElementName = "SelectedLayer")]
    public class SelectedLayer
    {
        [XmlElement(ElementName = "LayerMetadata")]
        public LayerMetadata LayerMetadata { get; set; }
        [XmlElement(ElementName = "Feature")]
        public List<Feature> Feature { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "SelectedFeatures")]
    public class SelectedFeatures
    {
        [XmlElement(ElementName = "SelectedLayer")]
        public List<SelectedLayer> SelectedLayer { get; set; }
    }

    [XmlRoot(ElementName = "FeatureInformation")]
    public class FeatureInformation
    {
        [XmlElement(ElementName = "FeatureSet")]
        public FeatureSet FeatureSet { get; set; }
        [XmlElement(ElementName = "Tooltip")]
        public string Tooltip { get; set; }
        [XmlElement(ElementName = "Hyperlink")]
        public string Hyperlink { get; set; }
        [XmlElement(ElementName = "InlineSelectionImage")]
        public string InlineSelectionImage { get; set; }
        [XmlElement(ElementName = "SelectedFeatures")]
        public SelectedFeatures SelectedFeatures { get; set; }
    }
}

