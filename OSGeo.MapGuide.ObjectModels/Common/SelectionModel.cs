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
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.ObjectModels.SelectionModel
{
    /// <summary>
    /// A feature class of a selected layer
    /// </summary>
    [XmlRoot(ElementName = "Class")]
    public class Class
    {
        /// <summary>
        /// A list of selected feature ids
        /// </summary>
        [XmlElement(ElementName = "ID")]
        public List<string> ID { get; set; }

        /// <summary>
        /// The name of the feature class
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// A selected layer
    /// </summary>
    [XmlRoot(ElementName = "Layer")]
    public class Layer
    {
        /// <summary>
        /// The feature class
        /// </summary>
        [XmlElement(ElementName = "Class")]
        public Class Class { get; set; }

        /// <summary>
        /// The runtime layer object id
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Models a selection set
    /// </summary>
    [XmlRoot(ElementName = "FeatureSet")]
    public class FeatureSet
    {
        /// <summary>
        /// The list of layers in the selection set
        /// </summary>
        [XmlElement(ElementName = "Layer")]
        public List<Layer> Layer { get; set; }
    }

    /// <summary>
    /// A selected feature property
    /// </summary>
    [XmlRoot(ElementName = "Property")]
    public class Property
    {
        /// <summary>
        /// The name of the property
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of the property
        /// </summary>
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// The display name of the property
        /// </summary>
        [XmlElement(ElementName = "DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The value of the property
        /// </summary>
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
    }

    /// <summary>
    /// Metadata of a selected layer
    /// </summary>
    [XmlRoot(ElementName = "LayerMetadata")]
    public class LayerMetadata
    {
        /// <summary>
        /// The visible properties of this layer
        /// </summary>
        [XmlElement(ElementName = "Property")]
        public List<Property> Property { get; set; }
    }

    /// <summary>
    /// Attributes of a selected feature
    /// </summary>
    [XmlRoot(ElementName = "Feature")]
    public class Feature
    {
        /// <summary>
        /// The property values of this feature
        /// </summary>
        [XmlElement(ElementName = "Property")]
        public List<Property> Property { get; set; }
    }

    /// <summary>
    /// A layer in a selection set
    /// </summary>
    [XmlRoot(ElementName = "SelectedLayer")]
    public class SelectedLayer
    {
        /// <summary>
        /// Metadata of this layer
        /// </summary>
        [XmlElement(ElementName = "LayerMetadata")]
        public LayerMetadata LayerMetadata { get; set; }

        /// <summary>
        /// The selected feature attribute collection
        /// </summary>
        [XmlElement(ElementName = "Feature")]
        public List<Feature> Feature { get; set; }

        /// <summary>
        /// The runtime layer object id
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the layer
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// A list of selected layer
    /// </summary>
    [XmlRoot(ElementName = "SelectedFeatures")]
    public class SelectedFeatures
    {
        /// <summary>
        /// The layer list
        /// </summary>
        [XmlElement(ElementName = "SelectedLayer")]
        public List<SelectedLayer> SelectedLayer { get; set; }
    }

    /// <summary>
    /// Root element of a QUERYMAPFEATURES response
    /// </summary>
    [XmlRoot(ElementName = "FeatureInformation")]
    public class FeatureInformation
    {
        /// <summary>
        /// The selection set
        /// </summary>
        [XmlElement(ElementName = "FeatureSet")]
        public FeatureSet FeatureSet { get; set; }

        /// <summary>
        /// The evaluated tooltip (if requested)
        /// </summary>
        [XmlElement(ElementName = "Tooltip")]
        public string Tooltip { get; set; }

        /// <summary>
        /// The evaluated hyperlink (if requested)
        /// </summary>
        [XmlElement(ElementName = "Hyperlink")]
        public string Hyperlink { get; set; }

        /// <summary>
        /// The evaluated inline selection image (if requested)
        /// </summary>
        [XmlElement(ElementName = "InlineSelectionImage")]
        public string InlineSelectionImage { get; set; }

        /// <summary>
        /// Selected feature attributes
        /// </summary>
        [XmlElement(ElementName = "SelectedFeatures")]
        public SelectedFeatures SelectedFeatures { get; set; }

        static Lazy<XmlSerializer> smFeatInfoSer = new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(OSGeo.MapGuide.ObjectModels.SelectionModel.FeatureInformation)));

        /// <summary>
        /// Parses the given XML to a <see cref="FeatureInformation"/>
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static FeatureInformation ParseFromXml(string xml)
        {
            try
            {
                using (var sr = new StringReader(xml))
                    return smFeatInfoSer.Value.Deserialize(sr) as FeatureInformation;
            }
            catch
            {
                return null;
            }
        }
    }
}

