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
using Ciloci.Flee;
using System.Reflection;
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace OSGeo.MapGuide.MaestroAPI.Expression
{
    /// <summary>
    /// A FDO expression evaluation engine
    /// </summary>
    internal class ExpressionEngine
    {
        private ReaderBase _reader;
        private ExpressionContext _context;

        public ExpressionEngine(ReaderBase reader, CultureInfo ci)
        {
            _reader = reader;
            //_cls = cls;
            _context = new ExpressionContext();
            _context.Options.ParseCulture = ci;
            //Register standard FDO functions
            _context.Imports.AddType(typeof(FdoFunctionNamespace));
            //Register standard MapGuide function
            _context.Imports.AddType(typeof(MgFunctionNamespace));
        }

        public void RegisterFunction(MethodInfo method, string @namespace)
        {
            _context.Imports.AddMethod(method, @namespace);
        }

        public void RegisterFunctions(Type functionType)
        {
            _context.Imports.AddType(functionType);
        }

        /*
        public ExpressionEngine(FeatureSetReader reader, object customFunctions)
            : this(reader)
        {
            
        }*/

        /// <summary>
        /// Updates the variables in the expression context. This is called by the reader
        /// as part of advancing to the next feature.
        /// </summary>
        internal void UpdateVariables()
        {
            if (_reader.Current == null)
                throw new InvalidOperationException(Strings.ErrorCurrentRecordIsEmpty);

            _context.Variables.Clear();
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                string name = _reader.GetName(i);
                if (_reader.IsNull(i))
                {
                    _context.Variables.Add(name, DBNull.Value);
                }
                else
                {
                    object value = _reader[i];
                    _context.Variables.Add(name, value);
                }
            }
        }

        /// <summary>
        /// Evalutes the expression against the current row
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal object Evaluate(string expression)
        {
            //Under FDO expression syntax:
            //
            // Double-quoted string literals represent identifiers
            // Single-quoted string literals represent string literals
            //
            // So convert this to FLEE-compatible format by:
            //
            // Stripping double quotes (variables are not quoted)
            // Convert single to double quotes (string literals are just CLR strings)
            var exprText = expression.Replace("\"", string.Empty) //NOXLATE
                                     .Replace("'", "\""); //NOXLATE

            if (_reader.Current == null)
                throw new InvalidOperationException(Strings.ErrorExprCurrentRowIsEmpty);

            try
            {
                var expr = _context.CompileDynamic(exprText);
                return expr.Evaluate();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal T Evaluate<T>(string expression)
        {
            //Under FDO expression syntax:
            //
            // Double-quoted string literals represent identifiers
            // Single-quoted string literals represent string literals
            //
            // So convert this to FLEE-compatible format by:
            //
            // Stripping double quotes (variables are not quoted)
            // Convert single to double quotes (string literals are just CLR strings)
            var exprText = expression.Replace("\"", string.Empty) //NOXLATE
                                     .Replace("'", "\""); //NOXLATE

            if (_reader.Current == null)
                throw new InvalidOperationException(Strings.ErrorExprCurrentRowIsEmpty);

            var expr = _context.CompileGeneric<T>(exprText);
            return expr.Evaluate();
        }
    }
}
