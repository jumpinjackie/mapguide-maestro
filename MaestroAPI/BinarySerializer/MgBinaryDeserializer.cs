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

namespace OSGeo.MapGuide.MaestroAPI.BinarySerializer
{
	/// <summary>
	/// A utility for serializing objects to the internal MapGuide Format
	/// </summary>
	public class MgBinaryDeserializer
	{
		private Stream m_stream;
		private byte[] m_buf = new byte[Math.Max(MgBinarySerializer.DoubleLen, MgBinarySerializer.UInt64Len)];
		private Version m_siteVersion;

		public Version SiteVersion { get { return m_siteVersion; } }

		public MgBinaryDeserializer(Stream stream, Version siteversion)
		{
			m_stream = stream;
			m_siteVersion = siteversion;
		}

		private MgStreamHeader ReadStreamHeader()
		{
			MgStreamHeader h = new MgStreamHeader();
			m_stream.Read(m_buf, 0, MgBinarySerializer.UInt32Len);
			h.StreamStart = (MgStreamHeaderValues)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			h.StreamVersion = BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			h.StreamDataHdr = (MgStreamHeaderValues)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			return h;
		}

		/*private MgOperationHeader ReadOperationHeader()
		{
			throw new NotImplementedException();
		}

		private MgOperationResponseHeader ReadOperationResponseHeader()
		{
			throw new NotImplementedException();
		}*/

		private MgArgumentPacket ReadArgumentPacket()
		{
			MgArgumentPacket p = new MgArgumentPacket();
			p.PacketHeader = (MgPacketHeader)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			p.ArgumentType = (MgArgumentType)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			if (p.ArgumentType == MgArgumentType.String)
				p.Length = BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			return p;
		}

		private MgBinaryStreamArgumentPacket ReadBinaryStreamArgumentPacket()
		{
			MgBinaryStreamArgumentPacket p = new MgBinaryStreamArgumentPacket();
			p.PacketHeader = (MgPacketHeader)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			p.ArgumentType = (MgArgumentType)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			p.Version = BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			p.Length = BitConverter.ToUInt64(ReadStream(MgBinarySerializer.UInt64Len), 0);
			return p;
		}

		public string ReadString()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.String)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"string\" was expected");

			if (m_siteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				return ReadInternalString();
			}
			else
			{
				byte[] buf = ReadStreamRepeat((int)p.Length);

				string b = System.Text.Encoding.UTF8.GetString(buf);
				//Chop of C zero terminator... Why store it, when the length is also present?
				return b.Substring(0, b.Length - 1); 
			}
			
		}

		public string ReadInternalString()
		{
			int charwidth = m_stream.ReadByte();
			System.Text.Encoding ec;
			switch(charwidth)
			{
				case 1:
					ec = System.Text.Encoding.UTF8;
					break;
				case 2:
					ec = System.Text.Encoding.GetEncoding("UTF-16");
					break;
				default:
					throw new Exception("Failed to get valid string char width");
			}

			byte[] t = ReadStreamRepeat(MgBinarySerializer.UInt32Len);
			int len = BitConverter.ToInt32(t, 0);
			string b = ec.GetString(ReadStreamRepeat(len * charwidth));
			//Chop of C zero terminator... Why store it, when the length is also present?
			return b.Substring(0, b.Length - 1);
		}

		public string ReadResourceIdentifier()
		{
			int classId = ReadClassId();
			if (classId == 0)
				return null;

			if (m_siteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classId != 12003)
				throw new Exception("Object was not a resourceidentifier");
			if (m_siteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classId != 11500)
				throw new Exception("Object was not a resourceidentifier");

			if (m_siteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				return ReadInternalString();
			else
				return ReadString();
		}

		public long ReadInt64()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.INT64)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"Int64\" was expected");

			return BitConverter.ToInt64(ReadStream(MgBinarySerializer.UInt64Len), 0);
		}

		public int ReadInt32()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.INT32)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"Int32\" was expected");

			return BitConverter.ToInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
		}

		public short ReadInt16()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.INT16)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"Int16\" was expected");

			return BitConverter.ToInt16(ReadStream(MgBinarySerializer.UInt16Len), 0);
		}

		public bool ReadBool()
		{
			return ReadByte() != 0;
		}

		public byte ReadByte()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.INT8)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"Int8\" was expected");

			return (byte)m_stream.ReadByte();
		}

		public float ReadSingle()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.Float)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"Float\" was expected");

			return BitConverter.ToSingle(ReadStream(MgBinarySerializer.FloatLen), 0);
		}
		
		public double ReadDouble()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.Double)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"Double\" was expected");

			return BitConverter.ToDouble(ReadStream(MgBinarySerializer.DoubleLen), 0);
		}

		public double[] ReadCoordinates()
		{
			int classid = ReadClassId();
			if (m_siteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 18000)
				throw new Exception("Coordinate expected, but got object: " + classid.ToString());
			if (m_siteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 20000)
				throw new Exception("Coordinate expected, but got object: " + classid.ToString());

			int count = ReadInt32();
			int dimensions = ReadInt32();
			if (dimensions == 0)
				dimensions = 2;
			else if (dimensions > 4)
				throw new Exception("Coordinate count reported: " + dimensions.ToString() + ", for a single coordinate");

			double[] args = new double[dimensions * count];
			for(int i = 0;i < (dimensions * count); i++)
				args[i] = ReadDouble();

			return args;
		}

		public Stream ReadStream()
		{
			MgBinaryStreamArgumentPacket p = ReadBinaryStreamArgumentPacket();
			if (p.ArgumentType != MgArgumentType.Stream)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"Stream\" was expected");

			if (ReadBool())
				return null;

			//TODO: If the stream is large, we use a lot of memory, perhaps the filebacked async stream could be used
			//Due to the annoying embedded buffer size markers, we cannot return the stream 'as is' Grrrrrr!

			int r;
			ulong remain = p.Length; 
			byte[] buf = new byte[1024 * 8];

			MemoryStream ms = new MemoryStream();
			do
			{
				int blocksize = ReadInt32();
				r = 1;

				while( r > 0 && blocksize > 0)
				{
					r = m_stream.Read(buf, 0, (int)Math.Min((ulong)remain, (ulong)buf.Length));
					blocksize -= r;
					remain -= (ulong)r;
					ms.Write(buf, 0, r);
				}

				if (blocksize != 0)
					throw new Exception("Stream ended prematurely");

			} while (remain > 0);

			ms.Position = 0;
			return ms;
		}

		public int ReadClassId()
		{
			MgArgumentPacket p = ReadArgumentPacket();
			if (p.ArgumentType != MgArgumentType.ClassId)
				throw new InvalidCastException("Data in stream had type: " + p.ArgumentType.ToString() + ", type \"ClassId\" was expected");

			return BitConverter.ToInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
		}

		public void ReadStreamEnd()
		{
			int v = BitConverter.ToInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
			if (v != (int)MgStreamHeaderValues.StreamEnd)
				throw new Exception("The read value was: " + v.ToString() + " while " + ((int)MgStreamHeaderValues.StreamEnd).ToString() + " was expected");
		}

		public IBinarySerializeable ReadObject()
		{
			int classId = ReadClassId();
			if (classId == 0)
				return null;

			IBinarySerializeable obj = null;
			switch(classId)
			{
				case 0:
				break;
				default:
					throw new Exception("Unknown object type: " + classId.ToString());
			}
			obj.Deserialize(this);

			return obj;
		}

		public byte[] ReadStreamRepeat(int len)
		{
			byte[] buf = new byte[len];
			ReadStreamRepeat(buf, 0, len);
			return buf;
		}

		/// <summary>
		/// Internal helper that will read from a potentially fragmented stream
		/// </summary>
		/// <param name="len">The number of bytes to read</param>
		/// <returns></returns>
		public void ReadStreamRepeat(byte[] buf, int offset, int len)
		{
			int r;
			int _offset = 0;
			
			if (buf.Length < len + offset)
				throw new OverflowException("Buffer is too small");

			do
			{
				r = m_stream.Read(buf, offset + _offset, len - _offset);
				_offset += r;
			} while (r > 0);

			if (_offset != len)
				throw new Exception("Stream exhausted while reading " + len.ToString() + " bytes");
		}

		/// <summary>
		/// Helper function that will throw an exception if the stream is unexceptedly exhausted
		/// </summary>
		/// <param name="len">The number of bytes to read</param>
		/// <returns>The internal buffer object</returns>
		private byte[] ReadStream(int len)
		{
			ReadStreamRepeat(m_buf, 0, len); 
			return m_buf;
		}
	}
}
