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
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace OSGeo.MapGuide.MaestroAPI.Expression
{
    /// <summary>
    /// A subclass <see cref="ReaderBase"/> that supports expression evaluation
    /// </summary>
    internal class ExpressionFeatureReader : ReaderBase
    {
        private ExpressionEngine _exprEngine;

        private ReaderBase _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionFeatureReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public ExpressionFeatureReader(ReaderBase reader)
            : this(reader, System.Threading.Thread.CurrentThread.CurrentCulture)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionFeatureReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="ci"></param>
        public ExpressionFeatureReader(ReaderBase reader, CultureInfo ci)
            : base()
        {
            _reader = reader;
            _exprEngine = new ExpressionEngine(_reader, ci);
        }

        /// <summary>
        /// Evaluates the specified expression against the current row. If evaluation
        /// fails, null is returned.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public object Evaluate(string expression)
        {
            return _exprEngine.Evaluate(expression);
        }

        /// <summary>
        /// Evaluates the specified expression returning a strongly-typed value. The specified expression
        /// must evaluate to the type parameter specified (or inferred) in the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns>The evaluated result</returns>
        /// <exception cref="ExpressionException">Thrown if evaluation fails</exception>
        public T Evaluate<T>(string expression)
        {
            try
            {
                return _exprEngine.Evaluate<T>(expression);
            }
            catch
            {
                throw new ExpressionException(expression);
            }
        }

        public override Type GetFieldType(int i)
        {
            return _reader.GetFieldType(i);
        }

        public override string GetName(int index)
        {
            return _reader.GetName(index);
        }

        public override Schema.PropertyValueType GetPropertyType(int index)
        {
            return _reader.GetPropertyType(index);
        }

        public override Schema.PropertyValueType GetPropertyType(string name)
        {
            return _reader.GetPropertyType(name);
        }

        public override ReaderType ReaderType
        {
            get { return _reader.ReaderType; }
        }

        protected override IRecord ReadNextRecord()
        {
            if (_reader.ReadNext())
            {
                _exprEngine.UpdateVariables();
                return _reader.Current;
            }
            return null; 
        }
    }
}
