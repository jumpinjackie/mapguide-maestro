#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

namespace Maestro.Editors.LayerDefinition.Vector.Thematics
{
    /// <summary>
    /// Represents a ColorBrewer color set
    /// </summary>
    internal class ColorBrewer
    {
        private string m_name;
        private string m_type;
        private List<System.Drawing.Color> m_colors;

        /// <summary>
        /// Gets the name of the ColorBrewer set
        /// </summary>
        public string Name { get { return m_name; } }
        /*
        /// <summary>
        /// Gets the assigned type for the ColorBrewer set
        /// </summary>
        public string Type { get { return m_type; } }
         */
        /// <summary>
        /// Gets the ordered list of colors to use
        /// </summary>
        public List<System.Drawing.Color> Colors { get { return m_colors; } }

        /// <summary>
        /// Constructs a new ColorBrewer instance
        /// </summary>
        /// <param name="name">The name of the set</param>
        /// <param name="type">The set type</param>
        /// <param name="colors">The colors in the set</param>
        public ColorBrewer(string name, string type, List<System.Drawing.Color> colors)
        {
            m_name = name;
            m_type = type;
            m_colors = colors;
        }

        /// <summary>
        /// Parses a CSV file for ColorBrewer setup, uses double quotes for text delimiter, and comma for record delimiter
        /// </summary>
        /// <param name="filename">The name of the file to parse</param>
        /// <returns>A list of parsed ColorBrewer sets</returns>
        public static List<ColorBrewer> ParseCSV(string filename)
        {
            return ParseCSV(filename, '"', ',');
        }

        /// <summary>
        /// Parses a CSV file for ColorBrewer setup
        /// </summary>
        /// <param name="filename">The name of the file to parse</param>
        /// <param name="recordDelimiter">The character used to delimit the records</param>
        /// <param name="textDelimiter">The character used to enclose strings</param>
        /// <returns>A list of parsed ColorBrewer sets</returns>
        public static List<ColorBrewer> ParseCSV(string filename, char textDelimiter, char recordDelimiter)
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename, System.Text.Encoding.UTF8, true))
            {
                List<ColorBrewer> result = new List<ColorBrewer>();
                Dictionary<string, int> columns = new Dictionary<string, int>();
                List<string> colnames = TokenizeLine(sr.ReadLine(), recordDelimiter, textDelimiter);

                for (int i = 0; i < colnames.Count; i++)
                    colnames[i] = colnames[i].ToLower();

                columns.Add("ColorName", colnames.IndexOf("colorname"));
                columns.Add("NumOfColors", colnames.IndexOf("numofcolors"));
                columns.Add("Type", colnames.IndexOf("type"));
                columns.Add("CritVal", colnames.IndexOf("critval"));
                columns.Add("ColorNum", colnames.IndexOf("colornum"));
                columns.Add("ColorLetter", colnames.IndexOf("colorletter"));
                columns.Add("R", colnames.IndexOf("r"));
                columns.Add("G", colnames.IndexOf("g"));
                columns.Add("B", colnames.IndexOf("b"));
                columns.Add("SchemeType", colnames.IndexOf("schemetype"));

                if (columns["ColorName"] < 0)
                    throw new Exception(string.Format(Strings.MissingColumnError, "ColorName"));
                if (columns["Type"] < 0)
                    throw new Exception(string.Format(Strings.MissingColumnError, "Type"));
                if (columns["NumOfColors"] < 0)
                    throw new Exception(string.Format(Strings.MissingColumnError, "NumOfColors"));
                if (columns["R"] < 0)
                    throw new Exception(string.Format(Strings.MissingColumnError, "R"));
                if (columns["G"] < 0)
                    throw new Exception(string.Format(Strings.MissingColumnError, "G"));
                if (columns["B"] < 0)
                    throw new Exception(string.Format(Strings.MissingColumnError, "B"));

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    List<string> values = TokenizeLine(line, recordDelimiter, textDelimiter);
                    
                    if (values.Count != columns.Count)
                        throw new Exception(string.Format(Strings.InvalidFieldCountError, line));

                    string colorName = values[columns["ColorName"]];
                    string type = values[columns["Type"]];
                    
                    if (string.IsNullOrEmpty(colorName) || string.IsNullOrEmpty(type))
                        continue; //Assume comment

                    int colorCount;
                    if (!int.TryParse(values[columns["NumOfColors"]], out colorCount))
                        continue; //Assume comment

                    List<System.Drawing.Color> colors = new List<System.Drawing.Color>();

                    while (!sr.EndOfStream && colorCount > 0)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            values = TokenizeLine(line, recordDelimiter, textDelimiter);
                            if (values.Count != colnames.Count)
                                throw new Exception(string.Format(Strings.InvalidRecordCountError, line));
                            
                            byte r, g, b;
                            if (!byte.TryParse(values[columns["R"]], out r))
                                throw new Exception(string.Format(Strings.InvalidColorComponent, "R", values[columns["R"]], line));
                            if (!byte.TryParse(values[columns["G"]], out g))
                                throw new Exception(string.Format(Strings.InvalidColorComponent, "G", values[columns["G"]], line));
                            if (!byte.TryParse(values[columns["B"]], out b))
                                throw new Exception(string.Format(Strings.InvalidColorComponent, "B", values[columns["B"]], line));

                            colors.Add(System.Drawing.Color.FromArgb(r, g, b));
                            colorCount--;
                        }

                        if (colorCount > 0)
                            line = sr.ReadLine();
                    }

                    if (colorCount != 0)
                        throw new Exception(string.Format(Strings.ColorCountError, colorCount, line));

                    result.Add(new ColorBrewer(colorName, type, colors));
                }

                return result;
            }
        }

        /// <summary>
        /// Splits a line into record fields, and removes text delimiters
        /// </summary>
        /// <param name="line">The line to tokenize</param>
        /// <param name="recordDelimiter">The character used to delimit the records</param>
        /// <param name="textDelimiter">The character used to enclose strings</param>
        /// <returns>A list of records</returns>
        private static List<string> TokenizeLine(string line, char recordDelimiter, char textDelimiter)
        {
            if (string.IsNullOrEmpty(line))
                return new List<string>();

            bool inQuotes = false;
            int startIndex = 0;
            List<string> records = new List<string>();

            for (int i = 0; i < line.Length; i++)
                if (line[i] == textDelimiter)
                    inQuotes = !inQuotes;
                else if (!inQuotes && line[i] == recordDelimiter)
                {
                    string rec = line.Substring(startIndex, i - startIndex);
                    if (rec.StartsWith(textDelimiter.ToString()) && rec.EndsWith(textDelimiter.ToString()))
                        rec = rec.Substring(1, rec.Length - 2);
                    records.Add(rec);
                    startIndex = i+1;
                }

            if (startIndex <= line.Length)
            {
                string rec = line.Substring(startIndex);
                if (rec.StartsWith(textDelimiter.ToString()) && rec.EndsWith(textDelimiter.ToString()))
                    rec = rec.Substring(1, rec.Length - 2);

                records.Add(rec);
            }
            
            return records;
        }

        /// <summary>
        /// Returns the objects string representation
        /// </summary>
        /// <returns>The string the represents the object</returns>
        public override string ToString()
        {
            return m_name + " - " + DisplayType;
        }

        /// <summary>
        /// Gets a display friendly version of the display type
        /// </summary>
        public string DisplayType
        {
            get
            {
                switch (m_type)
                {
                    case "qual":
                        return Strings.QualitativeName;
                    case "seq":
                        return Strings.SequentialName;
                    case "div":
                        return Strings.DivergingName;
                    default:
                        return m_type;
                }
            }
        }

        /// <summary>
        /// Helper class to display ColorBrewer items in a list or combobox
        /// </summary>
        public class ColorBrewerListItem
        {
            /// <summary>
            /// Indicates what type of string to use
            /// </summary>
            public enum DisplayMode
            {
                /// <summary>
                /// Displays &quot;Group - Name&quot;
                /// </summary>
                Full,
                /// <summary>
                /// Just display the group
                /// </summary>
                Type,
                /// <summary>
                /// Just display the set name
                /// </summary>
                Set
            }

            /// <summary>
            /// A reference to the set
            /// </summary>
            private ColorBrewer m_set;

            /// <summary>
            /// The current display mode
            /// </summary>
            private DisplayMode m_mode;

            /// <summary>
            /// Gets the ColorBrewer set
            /// </summary>
            public ColorBrewer Set { get { return m_set; } }

            public ColorBrewerListItem(ColorBrewer set, DisplayMode mode)
            {
                m_set = set;
                m_mode = mode;
            }

            /// <summary>
            /// Returns a display version of the item
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (m_mode == DisplayMode.Type)
                    return m_set.DisplayType;
                else if (m_mode == DisplayMode.Set)
                    return m_set.Name;
                else
                    return m_set.ToString();
            }
        }

        internal void Flip()
        {
            this.Colors.Reverse();
        }
    }
}
