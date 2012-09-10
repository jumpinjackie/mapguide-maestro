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
using System.Windows.Forms;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// Helper class to bind object properties to <see cref="NumericUpDown"/> derived classes for 
    /// immediate updates as opposed to updates on loss of focus.
    /// </summary>
    public static class NumericBinder
    {
        // We need to force WriteValue() on CheckedChanged otherwise it will only call WriteValue()
        // on loss of focus.
        // http://stackoverflow.com/questions/1060080/databound-winforms-control-does-not-recognize-change-until-losing-focus

        /// <summary>
        /// Binds the specified NumericUpDown control
        /// </summary>
        /// <param name="num"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Binding BindValueChanged(NumericUpDown num, Binding b)
        {
            num.DataBindings.Add(b);
            num.ValueChanged += (sender, e) => { b.WriteValue(); };

            return b;
        }

        /// <summary>
        /// Binds the specified NumericUpDown control
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <returns></returns>
        public static Binding BindValueChanged(NumericUpDown num, object dataSource, string dataMember)
        {
            var binding = num.DataBindings.Add("Value", dataSource, dataMember); //NOXLATE
            num.ValueChanged += (sender, e) => { binding.WriteValue(); };

            return binding;
        }
    }
}
