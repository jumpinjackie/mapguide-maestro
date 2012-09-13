#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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

// Original code by Michael Potter, made available under Public Domain
//
// http://www.codeproject.com/Articles/6943/A-Generic-Reusable-Diff-Algorithm-in-C-II/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Comparison
{
    /*
    public class BinaryFileDiffList : IDiffList
    {
        private byte[] _byteList;

        public BinaryFileDiffList(string fileName)
        {
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                int len = (int)fs.Length;
                br = new BinaryReader(fs);
                _byteList = br.ReadBytes(len);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (br != null) br.Close();
                if (fs != null) fs.Close();
            }

        }
        #region IDiffList Members

        public int Count()
        {
            return _byteList.Length;
        }

        public IComparable GetByIndex(int index)
        {
            return _byteList[index];
        }

        #endregion
    }
     */
}
