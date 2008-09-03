#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.IO;
using System.Threading;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Represents a buffered reader that relays stream data, using a file buffer, thus exhausting the source fast.
	/// </summary>
	public class FileBufferedStreamReader
		: Stream, IDisposable
	{
		private object m_sync = new object();
		private System.Threading.AutoResetEvent m_dataReady;
		private string m_file;
		private Stream m_source;
		private Stream m_buffer;
		private int m_length;

		private Thread m_reader;

		public FileBufferedStreamReader(Stream source, int length)
		{
			m_source = source;
			m_length = length;
			m_dataReady = new AutoResetEvent(false);

			m_file = System.IO.Path.GetTempFileName();
			m_buffer = File.Open(m_file, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			m_reader = new Thread(new ThreadStart(RunReader));
		}

		public override bool CanRead { get { return true; } }
		public override bool CanSeek { get { return false; } }
		public override bool CanWrite { get { return false; } }

		public override void Flush()
		{
		}

		public override void Close()
		{
			base.Close ();
		}

		public override long Length
		{
			get
			{
				throw new Exception("Cannot read length of non-seekable stream");
			}
		}

		public override long Position
		{
			get
			{
				throw new Exception("Cannot get position of non-seekable stream");
			}
			set
			{
				throw new Exception("Cannot set position of non-seekable stream");
			}
		}

		public override void SetLength(long value)
		{
			throw new Exception("Cannot set length of non-seekable stream");
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new Exception("Cannot seek in non-seekable stream");
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			do
			{
				lock(m_sync)
				{
					//If we have enough data, or all avalible data
					if (m_buffer.Position - m_buffer.Length >= count || !m_reader.IsAlive)
						return m_buffer.Read(buffer, offset, count);
				}

				//Block until data is avalible
				m_dataReady.WaitOne();
			} while(true);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new Exception("Stream is not writeable");
		}





		private void RunReader()
		{
			byte[] buffer = new byte[1024];
			int r = 0;
			int remain = (m_length <= 0 ? int.MaxValue : m_length) ;

			do
			{
				r = m_source.Read(buffer,0, Math.Min(buffer.Length, remain));
				lock (m_sync)
				{
					long prevpos = m_buffer.Position;
					m_buffer.Position = m_buffer.Length;
					m_buffer.Write(buffer, 0, r);
					m_buffer.Position = prevpos;
					m_dataReady.Set();
				}
			} while (r > 0);

			m_source.Close();
			m_dataReady.Set();
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (m_reader != null)
				try { m_reader.Abort(); }
				catch { }

			if (m_source != null)
				try { m_source.Close(); }
				catch { }

			if (m_buffer != null)
				try { m_buffer.Close(); }
				catch { }

			if (m_file != null)
				try { File.Delete(m_file); }
				catch {}
				
			m_reader = null;
			m_source = null;
			m_buffer = null;
			m_file = null;
		}

		#endregion
	}
}
