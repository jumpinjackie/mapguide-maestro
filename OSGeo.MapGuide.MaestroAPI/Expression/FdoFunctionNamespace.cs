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
using GeoAPI.Geometries;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.MaestroAPI.Expression
{
    /// <summary>
    /// Expression class that implements the standard FDO expression functions.
    /// 
    /// Despite the modifier. This class is for Expression Engine use only.
    /// </summary>
    public static class FdoFunctionNamespace
    {
        //TODO: Use a data type hierachy akin to FDO's LiteralValue and its descendants

        //
        // NOTE: ExpressionEngine will auto-coerce null values to DBNull.Value so when
        // testing for nulls, test for DBNull.Value
        //

        #region Aggregate
        #endregion

        #region Conversion
        public static object NullValue(object first, object second)
        {
            return first == DBNull.Value ? second : first;
        }

        public static DateTime ToDate(string str)
        {
            return DateTime.Parse(str);
        }

        public static double ToDouble(object value)
        {
            return Convert.ToDouble(value);
        }

        public static float ToFloat(object value)
        {
            return Convert.ToSingle(value);
        }

        public static int ToInt32(object value)
        {
            return Convert.ToInt32(value);
        }

        public static long ToInt64(object value)
        {
            return Convert.ToInt64(value);
        }

        public static string ToString(object value)
        {
            return Coalesce(value);
        }
        #endregion

        #region Date
        public static DateTime AddMonths(object value, object months)
        {
            return Convert.ToDateTime(value).AddMonths(Convert.ToInt32(months));
        }

        public static DateTime CurrentDate()
        {
            return DateTime.Now;
        }

        public static DateTime Extract(object date, object fromDate)
        {
            throw new NotImplementedException();
        }

        public static double ExtractToDouble(object date, object fromDate)
        {
            throw new NotImplementedException();
        }

        public static int ExtractToInt(object date, object fromDate)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Geometry

        public static double Area2D(IGeometry geom)
        {
            return geom.Area;
        }

        public static double Length2D(IGeometry geom)
        {
            return geom.Length;
        }

        public static double M(IGeometry geom)
        {
            return double.NaN; //M dimensions not supported
        }

        public static double X(IGeometry geom)
        {
            var pt = geom as IPoint;
            if (pt != null)
                return pt.X;

            return double.NaN;
        }

        public static double Y(IGeometry geom)
        {
            var pt = geom as IPoint;
            if (pt != null)
                return pt.Y;

            return double.NaN;
        }

        public static double Z(IGeometry geom)
        {
            var pt = geom as IPoint;
            if (pt != null)
                return pt.Z;

            return double.NaN;
        }

        #endregion

        #region Math
        public static double Abs(object value)
        {
            return Math.Abs(Convert.ToDouble(value));
        }

        public static double Acos(object value)
        {
            return Math.Acos(Convert.ToDouble(value));
        }

        public static double Asin(object value)
        {
            return Math.Asin(Convert.ToDouble(value));
        }

        public static double Atan(object value)
        {
            return Math.Atan(Convert.ToDouble(value));
        }

        public static double Atan2(object y, object x)
        {
            return Math.Atan2(Convert.ToDouble(y), Convert.ToDouble(x));
        }

        public static double Cos(object value)
        {
            return Math.Cos(Convert.ToDouble(value));
        }

        public static double Exp(object value)
        {
            return Math.Exp(Convert.ToDouble(value));
        }

        public static double Ln(object value)
        {
            return Math.Log(Convert.ToDouble(value));
        }

        public static double Log(object @base, object value)
        {
            return Math.Log(Convert.ToDouble(value), Convert.ToDouble(@base));
        }

        public static double Mod(object value, object divisor)
        {
            return Convert.ToDouble(value) % Convert.ToDouble(divisor);
        }

        public static double Power(double value, double power)
        {
            return Math.Pow(value, power);
        }

        public static int Power(int value, int power)
        {
            return Convert.ToInt32(Math.Pow(value, power));
        }

        public static double Remainder(object value, object divisor)
        {
            return Convert.ToDouble(value) % Convert.ToDouble(divisor);
        }

        public static double Sin(object value)
        {
            return Math.Sin(Convert.ToDouble(value));
        }

        public static double Sqrt(object value)
        {
            return Math.Sqrt(Convert.ToDouble(value));
        }

        public static double Tan(object value)
        {
            return Math.Tan(Convert.ToDouble(value));
        }
        #endregion

        #region Numeric
        
        public static double Ceil(double value)
        {
            return Math.Ceiling(value);
        }

        public static double Floor(double value)
        {
            return Math.Floor(value);
        }

        public static double Round(double value)
        {
            return Math.Round(value);
        }

        public static double Sign(double value)
        {
            return Math.Sign(value);
        }

        public static double Trunc(double value)
        {
            return Math.Truncate(value);
        }
        #endregion

        #region String
        /// <summary>
        /// Returns a concatenated result of 2 string expressions
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string Concat(object a, object b)
        {
            return Coalesce(a) + Coalesce(b);
        }

        private static string Coalesce(object obj)
        {
            return (obj == null || obj == DBNull.Value) ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// Returns a concatentated result of 2 or more string expressions
        /// </summary>
        /// <param name="a"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Concat(object a, params object[] args)
        {
            StringBuilder sb = new StringBuilder(Coalesce(a));
            foreach (var str in args)
            {
                sb.Append(Coalesce(str));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the position of a string within a base string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static int Instr(object str, object searchText)
        {
            return Coalesce(str).IndexOf(Coalesce(searchText));
        }

        /// <summary>
        /// Determines the length of a string expression
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int Length(object str)
        {
            return Coalesce(str).Length;
        }

        /// <summary>
        /// Converts all upper case letters in the string expression to lower case
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Lower(object str)
        {
            return Coalesce(str).ToLower();
        }

        /// <summary>
        /// Pads a string expression as directed to the left
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Lpad(object str)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Trims a string expression to the left
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Ltrim(object str)
        {
            return Coalesce(str).TrimStart();
        }

        /// <summary>
        /// Pads a string expression as directed to the right
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Rpad(object str)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Trims a string expression to the right
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Rtrim(object str)
        {
            return Coalesce(str).TrimEnd();
        }

        /// <summary>
        /// Returns the phonetic representation of a string expression
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Soundex(object str)
        {
            // +---------------------------------------------------------------------------
            // | The function processes a call to the function SOUNDEX.
            // | NOTE:
            // | The implementation of the function utilizes the following algorithm:
            // |
            // |   Step 1: Capitalize all letters in the word and remove all punctation
            // |           marks and digits.
            // |   Step 2: Retain the first letter of the word.
            // |   Step 3: Replace any letter from the set {A, E, I, O, U, H, W, Y} with
            // |           a 0 (zero).
            // |   Step 4: Replace any letter from the set {B, F, P, V} with a 1.
            // |           Replace any letter from the set {C, G, J, K, Q, S, X, Z) with
            // |           a 2
            // |           Replace any letter from the set {D, T) with a 3
            // |           Replace any letter from the set {L) with a 4
            // |           Replace any letter from the set {M, N) with a 5
            // |           Replace any letter from the set {R) with a 6
            // |   Step 5: Remove all pairs of digits which occur beside each other from
            // |           the that resulted from step 4.
            // |   Step 6: Remove all the zeros from the string that resulted from step 5.
            // |   Step 7: Pad the string resulting from step 6 with trailing zeros and
            // |           return the first 4 positions (resulting in a string of the 
            // |           structure <letter><number><number><number>).
            // +---------------------------------------------------------------------------
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a substring from the provided string as defined
        /// </summary>
        /// <param name="str"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string Substr(object str, object position)
        {
            return Coalesce(str).Substring(Convert.ToInt32(position));
        }

        public static string Translate(object str, object from, object to)
        {
            // Navigate through the source string and execute the replacement. The
            // following rules apply:
            //
            //  - any character in the from-set is replaced with the character in
            //    the to-set at the same position.
            //  - if the character in the from-set does not have a replacement
            //    character in the to-set at the same position, the character is
            //    deleted.
            //
            // NOTE: It is not possible to use the function REPLACE offered with the
            //       class FdoStringP because this may result in incorrect results.
            //       For example, if the call is TRANSLATE('abcd', 'ae', 'eS'), the
            //       result should be 'ebcd', not 'Sbcd' which would be the case if
            //       the mentioned function is used to do the job.
            throw new NotImplementedException();
        }

        public static string Trim(object str)
        {
            return Coalesce(str).Trim();
        }

        public static string Upper(object str)
        {
            return Coalesce(str).ToUpper();
        }
        #endregion
    }
}
