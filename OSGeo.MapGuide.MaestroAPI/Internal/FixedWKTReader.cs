﻿#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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

using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;
using OSGeo.MapGuide.MaestroAPI.Geometry;
using RTools_NTS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.Internal
{
    /// <summary>
    /// An exception that is thrown when parsing geometry WKT fails
    /// </summary>
    [Serializable]
    public class WktParseException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WktParseException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public WktParseException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public WktParseException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected WktParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// A fixed version of WKTReader that can parse 3D geometry WKT
    /// </summary>
    public class FixedWKTReader : IGeometryTextReader
    {
        private readonly GeometryFactory geometryFactory;
        private PrecisionModel precisionModel;
        private int index;

        /// <summary>
        /// Creates a <c>WKTReader</c> that creates objects using a basic GeometryFactory.
        /// </summary>
        public FixedWKTReader()
            : this(GeometryFactory.Default)
        {
        }

        /// <summary>
        /// Creates a <c>WKTReader</c> that creates objects using the given
        /// <c>GeometryFactory</c>.
        /// </summary>
        /// <param name="geometryFactory">The factory used to create <c>Geometry</c>s.</param>
        private FixedWKTReader(GeometryFactory geometryFactory)
        {
            this.geometryFactory = geometryFactory;
            precisionModel = geometryFactory.PrecisionModel;
        }

        /// <summary>
        /// Converts a Well-known Text representation to a <c>Geometry</c>.
        /// </summary>
        /// <param name="wellKnownText">
        /// one or more Geometry Tagged Text strings (see the OpenGIS
        /// Simple Features Specification) separated by whitespace.
        /// </param>
        /// <returns>
        /// A <c>Geometry</c> specified by <c>wellKnownText</c>
        /// </returns>
        public IGeometryRef Read(string wellKnownText)
        {
            using (StringReader reader = new StringReader(wellKnownText))
            {
                return Read(reader);
            }
        }

        /// <summary>
        /// Converts a Well-known Text representation to a <c>Geometry</c>.
        /// </summary>
        /// <param name="reader">
        /// A Reader which will return a "Geometry Tagged Text"
        /// string (see the OpenGIS Simple Features Specification).
        /// </param>
        /// <returns>A <c>Geometry</c> read from <c>reader</c>.
        /// </returns>
        public IGeometryRef Read(TextReader reader)
        {
            StreamTokenizer tokenizer = new StreamTokenizer(reader);
            var tokens = new List<Token>();
            tokenizer.Tokenize(tokens);     // Read directly all tokens
            index = 0;                      // Reset pointer to start of tokens

            try
            {
                var geom = ReadGeometryTaggedText(tokens);
                return new NTSGeometryRef(geom);
            }
            catch (IOException e)
            {
                throw new WktParseException(e.ToString());
            }
        }

        /// <summary>
        /// Returns the next array of <c>Coordinate</c>s in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next element returned by the stream should be "(" (the
        /// beginning of "(x1 y1, x2 y2, ..., xn yn)") or "EMPTY".
        /// </param>
        /// <param name="skipExtraParenthesis">
        /// if set to <c>true</c> skip extra parenthesis around coordinates.
        /// </param>
        /// <returns>
        /// The next array of <c>Coordinate</c>s in the
        /// stream, or an empty array if "EMPTY" is the next element returned by
        /// the stream.
        /// </returns>
        private Coordinate[] GetCoordinates(IList tokens, Boolean skipExtraParenthesis)
        {
            string nextToken = GetNextEmptyOrOpener(tokens);
            if (nextToken.Equals("EMPTY")) //NOXLATE
                return new Coordinate[] { };
            var coordinates = new List<Coordinate>();
            coordinates.Add(GetPreciseCoordinate(tokens, skipExtraParenthesis));
            nextToken = GetNextCloserOrComma(tokens);
            while (nextToken.Equals(",")) //NOXLATE
            {
                coordinates.Add(GetPreciseCoordinate(tokens, skipExtraParenthesis));
                nextToken = GetNextCloserOrComma(tokens);
            }
            return coordinates.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="skipExtraParenthesis"></param>
        /// <returns></returns>
        private Coordinate GetPreciseCoordinate(IList tokens, Boolean skipExtraParenthesis)
        {
            var coord = new CoordinateZM();
            Boolean extraParenthesisFound = false;
            if (skipExtraParenthesis)
            {
                extraParenthesisFound = IsStringValueNext(tokens, "("); //NOXLATE
                if (extraParenthesisFound)
                {
                    index++;
                }
            }
            coord.X = GetNextNumber(tokens);
            coord.Y = GetNextNumber(tokens);
            if (IsNumberNext(tokens))
                coord.Z = GetNextNumber(tokens);
            if (IsNumberNext(tokens))
                coord.M = GetNextNumber(tokens);

            if (skipExtraParenthesis &&
                extraParenthesisFound &&
                IsStringValueNext(tokens, ")")) //NOXLATE
            {
                index++;
            }

            precisionModel.MakePrecise((Coordinate)coord);
            return coord;
        }

        private Boolean IsStringValueNext(IList tokens, String stringValue)
        {
            Token token = tokens[index] as Token;
            return token.StringValue == stringValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private bool IsNumberNext(IList tokens)
        {
            Token token = tokens[index] as Token;
            return token is FloatToken || token is IntToken;
        }

        /// <summary>
        /// Returns the next number in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next token must be a number.
        /// </param>
        /// <returns>The next number in the stream.</returns>
        private double GetNextNumber(IList tokens)
        {
            Token token = tokens[index++] as Token;

            if (token == null)
                throw new ArgumentNullException(nameof(tokens), Strings.ErrorTokenListContainsNullValue); //NOXLATE
            else if (token is EofToken)
                throw new WktParseException(Strings.ErrorParseExpectedNumberEos);
            else if (token is EolToken)
                throw new WktParseException(Strings.ErrorParseExpectedNumberEol);
            else if (token is FloatToken || token is IntToken)
                return (double)token.ConvertToType(typeof(double));
            else if (token is WordToken)
                throw new WktParseException(string.Format(Strings.ErrorParseExpectedNumberGotWord, token.StringValue));
            else if (token.StringValue == "(") //NOXLATE
                throw new WktParseException(string.Format(Strings.ErrorParseExpectedNumber, '(')); //NOXLATE
            else if (token.StringValue == ")") //NOXLATE
                throw new WktParseException(string.Format(Strings.ErrorParseExpectedNumber, ')')); //NOXLATE
            else if (token.StringValue == ",") //NOXLATE
                throw new WktParseException(string.Format(Strings.ErrorParseExpectedNumber, ',')); //NOXLATE
            else
            {
                Assert.ShouldNeverReachHere();
                return double.NaN;
            }
        }

        /// <summary>
        /// Returns the next "EMPTY" or "(" in the stream as uppercase text.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next token must be "EMPTY" or "(".
        /// </param>
        /// <returns>
        /// The next "EMPTY" or "(" in the stream as uppercase text.</returns>
        private string GetNextEmptyOrOpener(IList tokens)
        {
            //The next word may be the dimension specifier. In such a case, read the
            //next word after that.
            string nextWord = GetNextWord(tokens);
            if (nextWord.Equals("XYZ")) //NOXLATE
            {
                nextWord = GetNextWord(tokens);
            }
            else if (nextWord.Equals("XYM")) //NOXLATE
            {
                nextWord = GetNextWord(tokens);
            }
            else if (nextWord.Equals("XYZM")) //NOXLATE
            {
                nextWord = GetNextWord(tokens);
            }
            else if (nextWord.Equals("ZM")) //NOXLATE
            {
                nextWord = GetNextWord(tokens);
            }
            if (nextWord.Equals("EMPTY") || nextWord.Equals("(")) //NOXLATE
                return nextWord;
            throw new WktParseException(string.Format(Strings.ErrorParseExpectedEmpty, nextWord));
        }

        /// <summary>
        /// Returns the next ")" or "," in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next token must be ")" or ",".
        /// </param>
        /// <returns>
        /// The next ")" or "," in the stream.</returns>
        private string GetNextCloserOrComma(IList tokens)
        {
            string nextWord = GetNextWord(tokens);
            if (nextWord.Equals(",") || nextWord.Equals(")")) //NOXLATE
                return nextWord;

            throw new WktParseException(string.Format(Strings.ErrorParseExpectedCloserOrComma, nextWord));
        }

        /// <summary>
        /// Returns the next ")" in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next token must be ")".
        /// </param>
        /// <returns>
        /// The next ")" in the stream.</returns>
        private string GetNextCloser(IList tokens)
        {
            string nextWord = GetNextWord(tokens);
            if (nextWord.Equals(")")) //NOXLATE
                return nextWord;
            throw new WktParseException(string.Format(Strings.ErrorParseExpectedCloser, nextWord));
        }

        /// <summary>
        /// Returns the next word in the stream as uppercase text.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next token must be a word.
        /// </param>
        /// <returns>The next word in the stream as uppercase text.</returns>
        private string GetNextWord(IList tokens)
        {
            Token token = tokens[index++] as Token;

            if (token is EofToken)
                throw new WktParseException(Strings.ErrorParseExpectedNumberEos);
            else if (token is EolToken)
                throw new WktParseException(Strings.ErrorParseExpectedNumberEol);
            else if (token is FloatToken || token is IntToken)
                throw new WktParseException(string.Format(Strings.ErrorParseExpectedWord, token.StringValue));
            else if (token is WordToken)
                return token.StringValue.ToUpper();
            else if (token.StringValue == "(") //NOXLATE
                return "("; //NOXLATE
            else if (token.StringValue == ")") //NOXLATE
                return ")"; //NOXLATE
            else if (token.StringValue == ",") //NOXLATE
                return ","; //NOXLATE
            else
            {
                Assert.ShouldNeverReachHere();
                return null;
            }
        }

        /// <summary>
        /// Creates a <c>Geometry</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a &lt;Geometry Tagged Text.
        /// </param>
        /// <returns>A <c>Geometry</c> specified by the next token
        /// in the stream.</returns>
        private NetTopologySuite.Geometries.Geometry ReadGeometryTaggedText(IList tokens)
        {
            /*
             * A new different implementation by Marc Jacquin:
             * this code manages also SRID values.
             */
            NetTopologySuite.Geometries.Geometry returned = null;
            string sridValue = null;
            string type = tokens[0].ToString();

            if (type == "SRID") //NOXLATE
            {
                sridValue = tokens[2].ToString();
                // tokens.RemoveRange(0, 4);
                tokens.RemoveAt(0);
                tokens.RemoveAt(0);
                tokens.RemoveAt(0);
                tokens.RemoveAt(0);
            }
            else type = GetNextWord(tokens);
            if (type.Equals("POINT")) //NOXLATE
                returned = ReadPointText(tokens);
            else if (type.Equals("LINESTRING")) //NOXLATE
                returned = ReadLineStringText(tokens);
            else if (type.Equals("LINEARRING")) //NOXLATE
                returned = ReadLinearRingText(tokens);
            else if (type.Equals("POLYGON")) //NOXLATE
                returned = ReadPolygonText(tokens);
            else if (type.Equals("MULTIPOINT")) //NOXLATE
                returned = ReadMultiPointText(tokens);
            else if (type.Equals("MULTILINESTRING")) //NOXLATE
                returned = ReadMultiLineStringText(tokens);
            else if (type.Equals("MULTIPOLYGON")) //NOXLATE
                returned = ReadMultiPolygonText(tokens);
            else if (type.Equals("GEOMETRYCOLLECTION")) //NOXLATE
                returned = ReadGeometryCollectionText(tokens);
            else throw new WktParseException(string.Format(Strings.ErrorParseUnknownType, type));

            if (returned == null)
                throw new NullReferenceException(Strings.ErrorParseGeometryRead);

            if (sridValue != null)
                returned.SRID = Convert.ToInt32(sridValue);

            return returned;
        }

        /// <summary>
        /// Creates a <c>Point</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a &lt;Point Text.
        /// </param>
        /// <returns>A <c>Point</c> specified by the next token in
        /// the stream.</returns>
        private Point ReadPointText(IList tokens)
        {
            string nextToken = GetNextEmptyOrOpener(tokens);
            if (nextToken.Equals("EMPTY")) //NOXLATE
                return geometryFactory.CreatePoint((Coordinate)null);
            var point = geometryFactory.CreatePoint(GetPreciseCoordinate(tokens, false));
            GetNextCloser(tokens);
            return point;
        }

        /// <summary>
        /// Creates a <c>LineString</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a &lt;LineString Text.
        /// </param>
        /// <returns>
        /// A <c>LineString</c> specified by the next
        /// token in the stream.</returns>
        private LineString ReadLineStringText(IList tokens)
        {
            return geometryFactory.CreateLineString(GetCoordinates(tokens, false));
        }

        /// <summary>
        /// Creates a <c>LinearRing</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a &lt;LineString Text.
        /// </param>
        /// <returns>A <c>LinearRing</c> specified by the next
        /// token in the stream.</returns>
        private LinearRing ReadLinearRingText(IList tokens)
        {
            return geometryFactory.CreateLinearRing(GetCoordinates(tokens, false));
        }

        /// <summary>
        /// Creates a <c>MultiPoint</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a &lt;MultiPoint Text.
        /// </param>
        /// <returns>
        /// A <c>MultiPoint</c> specified by the next
        /// token in the stream.</returns>
        private MultiPoint ReadMultiPointText(IList tokens)
        {
            return geometryFactory.CreateMultiPoint(ToPoints(GetCoordinates(tokens, true)));
        }

        /// <summary>
        /// Creates an array of <c>Point</c>s having the given <c>Coordinate</c>s.
        /// </summary>
        /// <param name="coordinates">
        /// The <c>Coordinate</c>s with which to create the <c>Point</c>s
        /// </param>
        /// <returns>
        /// <c>Point</c>s created using this <c>WKTReader</c>
        /// s <c>GeometryFactory</c>.
        /// </returns>
        private Point[] ToPoints(Coordinate[] coordinates)
        {
            var points = new List<Point>();
            for (int i = 0; i < coordinates.Length; i++)
                points.Add(geometryFactory.CreatePoint(coordinates[i]));
            return points.ToArray();
        }

        /// <summary>
        /// Creates a <c>Polygon</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a Polygon Text.
        /// </param>
        /// <returns>
        /// A <c>Polygon</c> specified by the next token
        /// in the stream.
        /// </returns>
        private Polygon ReadPolygonText(IList tokens)
        {
            string nextToken = GetNextEmptyOrOpener(tokens);
            if (nextToken.Equals("EMPTY")) //NOXLATE
                return geometryFactory.CreatePolygon(geometryFactory.CreateLinearRing(Array.Empty<Coordinate>()));

            var holes = new List<LinearRing>();
            var shell = ReadLinearRingText(tokens);
            nextToken = GetNextCloserOrComma(tokens);
            while (nextToken.Equals(",")) //NOXLATE
            {
                var hole = ReadLinearRingText(tokens);
                holes.Add(hole);
                nextToken = GetNextCloserOrComma(tokens);
            }
            return geometryFactory.CreatePolygon(shell, holes.ToArray());
        }

        /// <summary>
        /// Creates a <c>MultiLineString</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a MultiLineString Text.
        /// </param>
        /// <returns>
        /// A <c>MultiLineString</c> specified by the
        /// next token in the stream.</returns>
        private MultiLineString ReadMultiLineStringText(IList tokens)
        {
            string nextToken = GetNextEmptyOrOpener(tokens);
            if (nextToken.Equals("EMPTY")) //NOXLATE
                return geometryFactory.CreateMultiLineString(Array.Empty<LineString>());

            var lineStrings = new List<LineString>();
            var lineString = ReadLineStringText(tokens);
            lineStrings.Add(lineString);
            nextToken = GetNextCloserOrComma(tokens);
            while (nextToken.Equals(",")) //NOXLATE
            {
                lineString = ReadLineStringText(tokens);
                lineStrings.Add(lineString);
                nextToken = GetNextCloserOrComma(tokens);
            }
            return geometryFactory.CreateMultiLineString(lineStrings.ToArray());
        }

        /// <summary>
        /// Creates a <c>MultiPolygon</c> using the next token in the stream.
        /// </summary>
        /// <param name="tokens">Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a MultiPolygon Text.
        /// </param>
        /// <returns>
        /// A <c>MultiPolygon</c> specified by the next
        /// token in the stream, or if if the coordinates used to create the
        /// <c>Polygon</c> shells and holes do not form closed linestrings.</returns>
        private MultiPolygon ReadMultiPolygonText(IList tokens)
        {
            string nextToken = GetNextEmptyOrOpener(tokens);
            if (nextToken.Equals("EMPTY")) //NOXLATE
                return geometryFactory.CreateMultiPolygon(Array.Empty<Polygon>());

            var polygons = new List<Polygon>();
            var polygon = ReadPolygonText(tokens);
            polygons.Add(polygon);
            nextToken = GetNextCloserOrComma(tokens);
            while (nextToken.Equals(",")) //NOXLATE
            {
                polygon = ReadPolygonText(tokens);
                polygons.Add(polygon);
                nextToken = GetNextCloserOrComma(tokens);
            }
            return geometryFactory.CreateMultiPolygon(polygons.ToArray());
        }

        /// <summary>
        /// Creates a <c>GeometryCollection</c> using the next token in the
        /// stream.
        /// </summary>
        /// <param name="tokens">
        /// Tokenizer over a stream of text in Well-known Text
        /// format. The next tokens must form a &lt;GeometryCollection Text.
        /// </param>
        /// <returns>
        /// A <c>GeometryCollection</c> specified by the
        /// next token in the stream.</returns>
        private GeometryCollection ReadGeometryCollectionText(IList tokens)
        {
            string nextToken = GetNextEmptyOrOpener(tokens);
            if (nextToken.Equals("EMPTY")) //NOXLATE
                return geometryFactory.CreateGeometryCollection(Array.Empty<NetTopologySuite.Geometries.Geometry>());

            var geometries = new List<NetTopologySuite.Geometries.Geometry>();
            var geometry = ReadGeometryTaggedText(tokens);
            geometries.Add(geometry);
            nextToken = GetNextCloserOrComma(tokens);
            while (nextToken.Equals(",")) //NOXLATE
            {
                geometry = ReadGeometryTaggedText(tokens);
                geometries.Add(geometry);
                nextToken = GetNextCloserOrComma(tokens);
            }
            return geometryFactory.CreateGeometryCollection(geometries.ToArray());
        }
    }
}