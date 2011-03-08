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
using System.IO;
using OSGeo.MapGuide.MaestroAPI.IO;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    /// <summary>
    /// Represents a method that returns a <see cref="OSGeo.MapGuide.MgByteReader"/> instance
    /// </summary>
    /// <returns></returns>
    public delegate MgByteReader GetByteReaderMethod();

    /// <summary>
    /// A read-only <see cref="System.IO.Stream"/> adapter for the <see cref="OSGeo.MapGuide.MgByteReader"/>
    /// class.
    /// </summary>
    public class MgReadOnlyStream : ReadOnlyRewindableStream
    {
        private MgByteReader _reader;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="method"></param>
        public MgReadOnlyStream(GetByteReaderMethod method)
        {
            _reader = method();
        }

        ~MgReadOnlyStream()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _reader.Dispose();
            }
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            
        }

        public override long Length
        {
            //NOTE: MgByteReader only returns remaining length! Should we
            //be keeping track of position and adding on this value?
            get { return _reader.GetLength(); } 
        }

        public override bool CanRewind
        {
            get { return _reader.IsRewindable(); }
        }

        public override void Rewind()
        {
            if (!CanRewind)
                throw new InvalidOperationException("This stream is not rewindable"); //LOCALIZEME

            _reader.Rewind();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            //For good times, please always have the offset as 0
            if (offset == 0)
            {
                read = _reader.Read(buffer, count);
            }
            else //So you want to play the hard way eh? Bad performance for you!
            {
                byte[] b = new byte[count];
                read = _reader.Read(b, count);
                Array.Copy(b, 0, buffer, offset, read); 
            }
            return read;
        }
    }
}
