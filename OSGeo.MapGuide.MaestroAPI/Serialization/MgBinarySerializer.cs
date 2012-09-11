#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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

namespace OSGeo.MapGuide.MaestroAPI.Serialization
{
    /// <summary>
    /// A utility for deserializing data in the internal MapGuide format.
    /// </summary>
    public class MgBinarySerializer
    {
        private Stream m_stream;
        private Version m_siteVersion;


        /// <summary>
        /// 
        /// </summary>
        public static int UInt16Len = BitConverter.GetBytes((UInt16)0).Length;
        /// <summary>
        /// 
        /// </summary>
        public static int UInt32Len = BitConverter.GetBytes((UInt32)0).Length;
        /// <summary>
        /// 
        /// </summary>
        public static int UInt64Len = BitConverter.GetBytes((UInt64)0).Length;
        /// <summary>
        /// 
        /// </summary>
        public static int FloatLen =  BitConverter.GetBytes((float)0).Length;
        /// <summary>
        /// 
        /// </summary>
        public static int DoubleLen =  BitConverter.GetBytes((double)0).Length;
        /// <summary>
        /// 
        /// </summary>
        public static int UInt8Len =  1; //a byte...

        /// <summary>
        /// Gets the site version.
        /// </summary>
        /// <value>The site version.</value>
        public Version SiteVersion { get { return m_siteVersion; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MgBinarySerializer"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="siteversion">The siteversion.</param>
        public MgBinarySerializer(Stream stream, Version siteversion)
        {
            m_stream = stream;
            m_siteVersion = siteversion;
        }

        /// <summary>
        /// Writes the coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="dimensions">The dimensions.</param>
        public void WriteCoordinates(double[] coordinates, int dimensions)
        {
            int ndim = dimensions;
            if (ndim == 0)
                ndim = 2;
            else if (ndim > 4)
                throw new Exception(string.Format(Strings.ErrorBinarySerializerInvalidCoordinateDimensionCount, dimensions));

            if ((coordinates.Length % ndim) != 0)
                throw new Exception(Strings.ErrorBinarySerializerInvalidAmountOfCoordinates);

            if (m_siteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1))
                WriteClassId(18000);
            else
                WriteClassId(20000);
            Write(coordinates.Length / ndim);
            Write(dimensions);

            for(int i = 0;i < coordinates.Length; i++)
                Write(coordinates[i]);
        }

        /// <summary>
        /// Writes the resource identifier.
        /// </summary>
        /// <param name="resourceID">The resource ID.</param>
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


        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(string value)
        {
            if (m_siteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.String, null, (ulong)value.Length + 1));
                WriteStringInternal(value);
            }
            else
            {
                byte[] buf = System.Text.Encoding.UTF8.GetBytes(value + "\0"); //NOXLATE
                Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.String, null, (ulong)buf.Length));
                WriteRaw(buf);
            }
        }

        /// <summary>
        /// Writes the string internal.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteStringInternal(string value)
        {
            if (value == null)
                value = "";
            byte[] buf = System.Text.Encoding.Unicode.GetBytes(value + "\0"); //NOXLATE

            int charWidth = System.Text.Encoding.Unicode.GetByteCount(" "); //NOXLATE
            m_stream.WriteByte((byte)charWidth);

            m_stream.Write(BitConverter.GetBytes((UInt32)value.Length + 1), 0, UInt32Len);
            m_stream.Write(buf, 0, buf.Length);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(short value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT16, null, (ulong)UInt16Len));
            m_stream.Write(BitConverter.GetBytes(value), 0, UInt16Len);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(int value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT32, null, (ulong)UInt32Len));
            m_stream.Write(BitConverter.GetBytes(value), 0, UInt32Len);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(long value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT64, null, (ulong)UInt64Len));
            m_stream.Write(BitConverter.GetBytes(value), 0, UInt64Len);
        }

        /*
        private void Write(float value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.Float, null, (ulong)FloatLen));
            m_stream.Write(BitConverter.GetBytes(value), 0, FloatLen);
        }*/

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(double value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.Double, null, (ulong)DoubleLen));
            m_stream.Write(BitConverter.GetBytes(value), 0, DoubleLen);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void Write(bool value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT8, null, (ulong)UInt8Len));
            m_stream.WriteByte((byte)(value ? 1 : 0));
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(byte value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.INT8, null, (ulong)UInt8Len));
            m_stream.WriteByte(value);
        }

        /// <summary>
        /// Writes the class id.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteClassId(int value)
        {
            Write(new MgArgumentPacket(MgPacketHeader.ArgumentSimple, MgArgumentType.ClassId, null, (ulong)UInt32Len));
            m_stream.Write(BitConverter.GetBytes(value), 0, UInt32Len);
        }

        /// <summary>
        /// Writes the stream end.
        /// </summary>
        public void WriteStreamEnd()
        {
            m_stream.Write(BitConverter.GetBytes((UInt32)MgStreamHeaderValues.StreamEnd), 0, UInt32Len);
            m_stream.Flush();
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(IBinarySerializable value)
        {
            if (value == null)
                WriteClassId(0);
            else
                value.Serialize(this);
        }


        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(MgBinaryStreamArgumentPacket value)
        {
            m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.ArgumentType), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.Version), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt64)value.Length), 0, UInt64Len);
        }

        /// <summary>
        /// Writes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
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

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(MgArgumentPacket value)
        {
            m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.ArgumentType), 0, UInt32Len);
            if (value.ArgumentType == MgArgumentType.String)
                m_stream.Write(BitConverter.GetBytes((UInt32)value.Length), 0, UInt32Len);
        }

        /*
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(MgStreamHeader value)
        {
            m_stream.Write(BitConverter.GetBytes((UInt32)value.StreamStart), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.StreamVersion), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.StreamDataHdr), 0, UInt32Len);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(MgOperationPacket value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(MgOperationResponsePacket value)
        {
            m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketVersion), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.ECode), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.NumReturnValues), 0, UInt32Len);
        }

        /// <summary>
        /// Writes the specified e code.
        /// </summary>
        /// <param name="eCode">The e code.</param>
        /// <param name="noOfRetValues">The no of ret values.</param>
        private void Write(MgECode eCode, UInt32 noOfRetValues)
        {
            Write(new MgStreamHeader(MgStreamHeaderValues.StreamStart, MgStreamHeader.CurrentStreamVersion, MgStreamHeaderValues.StreamData));
            Write(new MgOperationResponsePacket(MgPacketHeader.OperationResponse, 1, eCode, noOfRetValues));
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void Write(MgControlPacket value)
        {
            Write(new MgStreamHeader(MgStreamHeaderValues.StreamStart, MgStreamHeader.CurrentStreamVersion, MgStreamHeaderValues.StreamData));
            m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketHeader), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.PacketVersion), 0, UInt32Len);
            m_stream.Write(BitConverter.GetBytes((UInt32)value.ControlID), 0, UInt32Len);
            WriteStreamEnd();
        }
        */
        /// <summary>
        /// Writes the raw.
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void WriteRaw(byte[] buf)
        {
            m_stream.Write(buf, 0, buf.Length);
        }

    }
}
