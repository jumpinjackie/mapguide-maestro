#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
	/// A utility for deserializing data in the internal MapGuide format.
	/// </summary>
	public class MgBinarySerializer
	{
		private Stream m_stream;
		private Version m_siteVersion;


		public static int UInt16Len = BitConverter.GetBytes((UInt16)0).Length;
		public static int UInt32Len = BitConverter.GetBytes((UInt32)0).Length;
		public static int UInt64Len = BitConverter.GetBytes((UInt64)0).Length;
		public static int FloatLen =  BitConverter.GetBytes((float)0).Length;
		public static int DoubleLen =  BitConverter.GetBytes((double)0).Length;
		public static int UInt8Len =  1; //a byte...

		public Version SiteVersion { get { return m_siteVersion; } }

		public MgBinarySerializer(Stream stream, Version siteversion)
		{
			m_stream = stream;
			m_siteVersion = siteversion;
		}

		public void WriteCoordinates(double[] coordinates, int dimensions)
		{
			int ndim = dimensions;
			if (ndim == 0)
				ndim = 2;
			else if (ndim > 4)
				throw new Exception("Coordinate count reported: " + dimensions.ToString() + ", for a single coordinate");

			if ((coordinates.Length % ndim) != 0)
				throw new Exception("Invalid amount of coordinates...");

			if (m_siteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1))
				WriteClassId(18000);
			else
				WriteClassId(20000);
			Write(coordinates.Length / ndim);
			Write(dimensions);

			for(int i = 0;i < coordinates.Length; i++)
				Write(coordinates[i]);
		}

		public void WriteResourceIdentifier(string resourceID)
		{
			if (m_siteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1))
				WriteClassId(12003);
			else
				WriteClassId(11500);
			if (m_siteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				WriteStringInternal(resourceID);
			else
				Write(resourceID);
		}


		public void Write(string value)
		{
			if (m_siteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.String, null, (ulong)value.Length + 1));
				WriteStringInternal(value);
			}
			else
			{
				byte[] buf = System.Text.Encoding.UTF8.GetBytes(value + "\0");
				Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.String, null, (ulong)buf.Length));
				WriteRaw(buf);
			}
		}

		public void WriteStringInternal(string value)
		{
			byte[] buf = System.Text.Encoding.Unicode.GetBytes(value + "\0");

			int charWidth = System.Text.Encoding.Unicode.GetByteCount(" ");
			m_stream.WriteByte((byte)charWidth);

			m_stream.Write(BitConverter.GetBytes((UInt32)value.Length + 1), 0, UInt32Len);
			m_stream.Write(buf, 0, buf.Length);
		}

		public void Write(short value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT16, null, (ulong)UInt16Len));
			m_stream.Write(BitConverter.GetBytes(value), 0, UInt16Len);
		}

		public void Write(int value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT32, null, (ulong)UInt32Len));
			m_stream.Write(BitConverter.GetBytes(value), 0, UInt32Len);
		}

		public void Write(long value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT64, null, (ulong)UInt64Len));
			m_stream.Write(BitConverter.GetBytes(value), 0, UInt64Len);
		}

		private void Write(float value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.Float, null, (ulong)FloatLen));
			m_stream.Write(BitConverter.GetBytes(value), 0, FloatLen);
		}

		public void Write(double value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.Double, null, (ulong)DoubleLen));
			m_stream.Write(BitConverter.GetBytes(value), 0, DoubleLen);
		}

		public void Write(bool value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT8, null, (ulong)UInt8Len));
			m_stream.WriteByte((byte)(value ? 1 : 0));
		}

		public void Write(byte value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT8, null, (ulong)UInt8Len));
			m_stream.WriteByte(value);
		}

		public void WriteClassId(int value)
		{
			Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.ClassId, null, (ulong)UInt32Len));
			m_stream.Write(BitConverter.GetBytes(value), 0, UInt32Len);
		}

		public void WriteStreamEnd()
		{
			m_stream.Write(BitConverter.GetBytes((UInt32)MgStreamHeaderValues.StreamEnd), 0, UInt32Len);
			m_stream.Flush();
		}

		public void Write(IBinarySerializeable value)
		{
			if (value == null)
				WriteClassId(0);
			else
				value.Serialize(this);
		}

		
		private void Write(MgBinaryStreamArgumentPacket value)
		{
			m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.ArgumentType), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.Version), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt64)value.Length), 0, UInt64Len);
		}

		public void Write(Stream stream)
		{
			Write(new MgBinaryStreamArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT8, null, (ulong)(stream == null ? 0 : stream.Length), MgBinaryStreamArgumentPacket.MG_STREAM_VERSION ));
			Write(stream == null);
			if (stream != null)
			{
				byte[] buf = new byte[8192];
				int r = stream.Read(buf, 0, buf.Length);
				while (r > 0)
				{
					m_stream.Write(BitConverter.GetBytes((UInt32)r), 0, UInt32Len);
					r = stream.Read(buf, 0, buf.Length);
				}
				m_stream.Write(BitConverter.GetBytes((UInt32)0), 0, UInt32Len);
			}
		}

		private void Write(MgArgumentPacket value)
		{
			m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.ArgumentType), 0, UInt32Len);
			if (value.ArgumentType == MgArgumentType.String)
				m_stream.Write(BitConverter.GetBytes((UInt32)value.Length), 0, UInt32Len);
		}

		private void Write(MgStreamHeader value)
		{
			m_stream.Write(BitConverter.GetBytes((UInt32)value.StreamStart), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.StreamVersion), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.StreamDataHdr), 0, UInt32Len);
		}

		private void Write(MgOperationPacket value)
		{
			throw new NotImplementedException();
		}

		private void Write(MgOperationResponsePacket value)
		{
			m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketVersion), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.ECode), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.NumReturnValues), 0, UInt32Len);
		}

		private void Write(MgECode eCode, UInt32 noOfRetValues)
		{
			Write(new MgStreamHeader(MgStreamHeaderValues.StreamStart, MgStreamHeader.CurrentStreamVersion, MgStreamHeaderValues.StreamData));
			Write(new MgOperationResponsePacket(MgPacketHeader.OperationResponse, 1, eCode, noOfRetValues));
		}

		private void Write(MgControlPacket value)
		{
			Write(new MgStreamHeader(MgStreamHeaderValues.StreamStart, MgStreamHeader.CurrentStreamVersion, MgStreamHeaderValues.StreamData));
			m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketVersion), 0, UInt32Len);
			m_stream.Write(BitConverter.GetBytes((UInt32)value.ControlID), 0, UInt32Len);
			WriteStreamEnd();
		}

		public void WriteRaw(byte[] buf)
		{
			m_stream.Write(buf, 0, buf.Length);
		}

	}
}
