#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
//
// Original code from SharpDevelop 3.2.1 licensed under the same terms (LGPL 2.1)
// Copyright 2002-2010 by
//
//  AlphaSierraPapa, Christoph Wille
//  Vordernberger Strasse 27/8
//  A-8700 Leoben
//  Austria
//
//  email: office@alphasierrapapa.com
//  court of jurisdiction: Landesgericht Leoben
//
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

using Maestro.Editors.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Maestro.AddIn.Scripting.Lang.Python
{
    internal class PythonOutputStream : Stream
    {
        private PythonConsole _console;
        private readonly ITextEditor textEditor;

        public PythonOutputStream(PythonConsole console, ITextEditor textEditor)
        {
            _console = console;
            this.textEditor = textEditor;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0;

        public override long Position
        {
            get { return 0; }
            set { }
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin) => 0;

        public override void SetLength(long value)
        {
        }

        /// <summary>
        /// This is a control flag to determine if the read stream should end. Set to true to signal end of stream when
        /// the next line is received. Set to false to signal that reading the next line should start or continue
        /// </summary>
        public bool DoneReadingForNow
        {
            get;
            set;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.DoneReadingForNow)
                return 0;

            Debug.WriteLine("({0}): PythonOutputStream.Read() - buffer: {1}, offset: {2}, count: {3}, currently reading: {4}", Thread.CurrentThread.ManagedThreadId, buffer.Length, offset, count, _console.IsReadingInput);
            _console.IsReadingInput = true;
            int read = 0;
            string line = _console.GetLineForInput(_lastWrittenText);
            if (line != null)
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(line)))
                {
                    read = ms.Read(buffer, offset, count);
                }
                _lastCapturedText = line;
            }
            else
            {
                this.DoneReadingForNow = false;
                return 0;
            }
            Debug.WriteLine("({0}): PythonOutputStream.Read() read {1} bytes", Thread.CurrentThread.ManagedThreadId, read);
            this.DoneReadingForNow = true;
            return read;
        }

        private string _lastCapturedText;
        private string _lastWrittenText;

        /// <summary>
        /// Assumes the bytes are UTF8 and writes them to the text editor.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            string text = UTF8Encoding.UTF8.GetString(buffer, offset, count);
            _lastWrittenText = text;
            textEditor.Write(text);
        }
    }
}