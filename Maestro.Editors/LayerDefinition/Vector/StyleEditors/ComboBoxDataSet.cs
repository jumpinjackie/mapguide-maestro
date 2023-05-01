using System;
using System.IO;
using System.Xml.Serialization;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    /// <summary>
    /// In-memory replacement for the typed ComboBoxDataSet
    /// </summary>
    [XmlRoot]
    public class ComboBoxDataSet
    {
        static Lazy<XmlSerializer> Serializer = new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(ComboBoxDataSet)));

        public static ComboBoxDataSet InitFromXml(string xml)
        {
            using var stringReader = new System.IO.StringReader(xml);
            return ((ComboBoxDataSet)(Serializer.Value.Deserialize(System.Xml.XmlReader.Create(stringReader))));
        }

        [XmlElement]
        public SymbolMark[] SymbolMark { get; set; }

        [XmlElement]
        public SizeContext[] SizeContext { get; set; }

        [XmlElement]
        public Units[] Units { get; set; }

        [XmlElement]
        public Rotation[] Rotation { get; set; }

        [XmlElement]
        public BackgroundType[] BackgroundType { get; set; }

        [XmlElement]
        public Vertical[] Vertical { get; set; }

        [XmlElement]
        public Horizontal[] Horizontal { get; set; }

        [XmlElement]
        public LabelJustification[] LabelJustification { get; set; }
    }

    public abstract class ComboBoxItemEntry 
    {
        [XmlElement]
        public string Display { get; set; }

        [XmlElement]
        public string Value { get; set; }
    }

    public class SymbolMark : ComboBoxItemEntry { }

    public class SizeContext : ComboBoxItemEntry { }

    public class Units : ComboBoxItemEntry { }

    public class Rotation : ComboBoxItemEntry { }

    public class BackgroundType : ComboBoxItemEntry { }

    public class Vertical : ComboBoxItemEntry { }

    public class Horizontal : ComboBoxItemEntry { }

    public class LabelJustification : ComboBoxItemEntry { }
}
