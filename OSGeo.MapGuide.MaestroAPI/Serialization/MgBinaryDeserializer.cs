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
    /// A utility for serializing objects to the internal MapGuide Format
    /// </summary>
    public class MgBinaryDeserializer
    {
        private Stream m_stream;
        private byte[] m_buf = new byte[Math.Max(MgBinarySerializer.DoubleLen, MgBinarySerializer.UInt64Len)];
        private Version m_siteVersion;

        /// <summary>
        /// Gets the site version.
        /// </summary>
        /// <value>The site version.</value>
        public Version SiteVersion { get { return m_siteVersion; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MgBinaryDeserializer"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="siteversion">The siteversion.</param>
        public MgBinaryDeserializer(Stream stream, Version siteversion)
        {
            m_stream = stream;
            m_siteVersion = siteversion;
        }
        
        /*
        private MgStreamHeader ReadStreamHeader()
        {
            MgStreamHeader h = new MgStreamHeader();
            m_stream.Read(m_buf, 0, MgBinarySerializer.UInt32Len);
            h.StreamStart = (MgStreamHeaderValues)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
            h.StreamVersion = BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
            h.StreamDataHdr = (MgStreamHeaderValues)BitConverter.ToUInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
            return h;
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

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.String)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "string")); //NOXLATE

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

        /// <summary>
        /// Reads the internal string.
        /// </summary>
        /// <returns></returns>
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
                    ec = System.Text.Encoding.GetEncoding("UTF-16"); //NOXLATE
                    break;
                default:
                    throw new Exception(Strings.ErrorBinarySerializerGetCharWidth);
            }

            byte[] t = ReadStreamRepeat(MgBinarySerializer.UInt32Len);
            int len = BitConverter.ToInt32(t, 0);
            string b = ec.GetString(ReadStreamRepeat(len * charwidth));
            //Chop of C zero terminator... Why store it, when the length is also present?
            return b.Substring(0, b.Length - 1);
        }

        /// <summary>
        /// Reads the resource identifier.
        /// </summary>
        /// <returns></returns>
        public string ReadResourceIdentifier()
        {
            int classId = ReadClassId();
            if (classId == 0)
                return null;

            if (m_siteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classId != 12003)
                throw new Exception(Strings.ErrorInvalidResourceIdentifier);
            if (m_siteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classId != 11500)
                throw new Exception(Strings.ErrorInvalidResourceIdentifier);

            if (m_siteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
                return ReadInternalString();
            else
                return ReadString();
        }

        /// <summary>
        /// Reads the int64.
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.INT64)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "Int64")); //NOXLATE

            return BitConverter.ToInt64(ReadStream(MgBinarySerializer.UInt64Len), 0);
        }

        /// <summary>
        /// Reads the int32.
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.INT32)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "Int32")); //NOXLATE

            return BitConverter.ToInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
        }

        /// <summary>
        /// Reads the int16.
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.INT16)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "Int16")); //NOXLATE

            return BitConverter.ToInt16(ReadStream(MgBinarySerializer.UInt16Len), 0);
        }

        /// <summary>
        /// Reads the bool.
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            return ReadByte() != 0;
        }

        /// <summary>
        /// Reads the byte.
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.INT8)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "Int8")); //NOXLATE

            return (byte)m_stream.ReadByte();
        }

        /// <summary>
        /// Reads the single.
        /// </summary>
        /// <returns></returns>
        public float ReadSingle()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.Float)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "Float")); //NOXLATE

            return BitConverter.ToSingle(ReadStream(MgBinarySerializer.FloatLen), 0);
        }

        /// <summary>
        /// Reads the double.
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.Double)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "Double")); //NOXLATE

            return BitConverter.ToDouble(ReadStream(MgBinarySerializer.DoubleLen), 0);
        }

        /// <summary>
        /// Reads the coordinates.
        /// </summary>
        /// <returns></returns>
        public double[] ReadCoordinates()
        {
            int classid = ReadClassId();
            if (m_siteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 18000)
                throw new Exception(string.Format(Strings.ErrorBinarySerializerCoordinateUnexpectedType, classid));
            if (m_siteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 20000)
                throw new Exception(string.Format(Strings.ErrorBinarySerializerCoordinateUnexpectedType, classid));

            int count = ReadInt32();
            int dimensions = ReadInt32();
            if (dimensions == 0)
                dimensions = 2;
            else if (dimensions > 4)
                throw new Exception(string.Format(Strings.ErrorBinarySerializerInvalidCoordinateDimensionCount, dimensions));

            double[] args = new double[dimensions * count];
            for(int i = 0;i < (dimensions * count); i++)
                args[i] = ReadDouble();

            return args;
        }

        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <returns></returns>
        public Stream ReadStream()
        {
            MgBinaryStreamArgumentPacket p = ReadBinaryStreamArgumentPacket();
            if (p.ArgumentType != MgArgumentType.Stream)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "Stream")); //NOXLATE

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
                    throw new Exception(Strings.ErrorBinarySerializerPrematureEndOfStream);

            } while (remain > 0);

            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Reads the class id.
        /// </summary>
        /// <returns></returns>
        public int ReadClassId()
        {
            MgArgumentPacket p = ReadArgumentPacket();
            if (p.ArgumentType != MgArgumentType.ClassId)
                throw new InvalidCastException(string.Format(Strings.ErrorBinarySerializerUnexpectedType, p.ArgumentType.ToString(), "ClassId")); //NOXLATE

            return BitConverter.ToInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
        }

        /// <summary>
        /// Reads the stream end.
        /// </summary>
        public void ReadStreamEnd()
        {
            int v = BitConverter.ToInt32(ReadStream(MgBinarySerializer.UInt32Len), 0);
            if (v != (int)MgStreamHeaderValues.StreamEnd)
                throw new Exception(string.Format(Strings.ErrorBinarySerializerExpectedEndOfStream, v.ToString()));
        }

        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <returns></returns>
        public IBinarySerializable ReadObject()
        {
            int classId = ReadClassId();
            if (classId == 0)
                return null;

            IBinarySerializable obj = null;
            switch(classId)
            {
                case 0:
                    break;
                default:
                    throw new Exception(string.Format(Strings.ErrorBinarySerializerUnknownObjectType, classId));
            }
            obj.Deserialize(this);

            return obj;
        }

        /// <summary>
        /// Reads the stream repeat.
        /// </summary>
        /// <param name="len">The len.</param>
        /// <returns></returns>
        public byte[] ReadStreamRepeat(int len)
        {
            byte[] buf = new byte[len];
            ReadStreamRepeat(buf, 0, len);
            return buf;
        }

        /// <summary>
        /// Internal helper that will read from a potentially fragmented stream
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="len">The number of bytes to read</param>
        public void ReadStreamRepeat(byte[] buf, int offset, int len)
        {
            int r;
            int _offset = 0;
            
            if (buf.Length < len + offset)
                throw new OverflowException(Strings.ErrorBinarySerializerBufferTooSmall);

            do
            {
                r = m_stream.Read(buf, offset + _offset, len - _offset);
                _offset += r;
            } while (r > 0);

            if (_offset != len)
                throw new Exception(string.Format(Strings.ErrorBinarySerializerStreamExhausted, len));
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
