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
using System.Xml.Serialization;
using System.Diagnostics;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.Common
{
    partial class UnmanagedDataListUnmanagedDataFile
    {
        [XmlIgnore]
        public string FileName
        {
            get
            {
                string str = this.unmanagedDataIdField;
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.Contains("/")) //NOXLATE
                    {
                        return str.Substring(str.LastIndexOf("/") + 1); //NOXLATE
                    }
                    else
                    {
                        return str.Substring(str.LastIndexOf("]") + 1); //NOXLATE
                    }
                }
                return string.Empty;
            }
        }
    }

    partial class UnmanagedDataListUnmanagedDataFolder
    {
        [XmlIgnore]
        public string FolderName
        {
            get
            {
                string str = this.unmanagedDataIdField;
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.EndsWith("/"))  //NOXLATE
                        str = str.Substring(0, str.Length - 1);

                    var tokens = str.Split('/'); //NOXLATE
                    if (tokens.Length == 1)
                    {
                        var p = tokens[0];
                        if (p.EndsWith("]")) //NOXLATE
                            return p;
                        else
                            return p.Substring(p.IndexOf("]") + 1); //NOXLATE
                    }
                    else
                        return tokens[tokens.Length - 1];
                }
                return string.Empty;
            }
        }
    }
}
