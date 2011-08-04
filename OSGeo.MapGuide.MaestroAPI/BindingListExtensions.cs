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
using System.ComponentModel;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Extension method class 
    /// </summary>
    public static class BindingListExtensions
    {
        //We target .net 2.0 so we have to roll our own extension methods

        /// <summary>
        /// Gets an array from the specified binding list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] ToArray<T>(this BindingList<T> list)
        {
            T[] values = new T[list.Count];
            int i = 0;
            foreach (T v in list)
            {
                values[i] = v;
                i++;
            }
            return values;
        }
    }
}
/*
//A well known hack to get extension methods working in .net framework 2.0

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Compiler attribute to support extension methods
    /// </summary>
    public class ExtensionAttribute : Attribute
    {

    }
}*/
