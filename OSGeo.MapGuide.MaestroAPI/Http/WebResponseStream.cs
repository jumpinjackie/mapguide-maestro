using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    internal class WebResponseStream : Stream
    {
        readonly HttpWebResponse _response;
        readonly Stream _innerStream;

        internal WebResponseStream(HttpWebResponse response)
        {
            _response = response;
            _innerStream = _response.GetResponseStream();
        }

        public override void Close()
        {
            // WTF: If I override Dispose(), it isn't called???
            //
            // Only through reading the CoreFX source for System.IO.Stream
            // do I find that its Dispose() will call Close(), which is
            // indeed called when overridden!

            _innerStream.Close();
            _innerStream.Dispose();

            _response.Close();
            _response.Dispose();

            base.Close();
        }

        public override bool CanRead => _innerStream.CanRead;

        public override bool CanSeek => _innerStream.CanSeek;

        public override bool CanWrite => _innerStream.CanWrite;

        public override long Length => _innerStream.Length;

        public override long Position
        {
            get { return _innerStream.Position; }
            set { _innerStream.Position = value; }
        }

        public override void Flush() => _innerStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
            => _innerStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin)
            => _innerStream.Seek(offset, origin);

        public override void SetLength(long value)
            => _innerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
            => _innerStream.Write(buffer, offset, count);
    }
}
