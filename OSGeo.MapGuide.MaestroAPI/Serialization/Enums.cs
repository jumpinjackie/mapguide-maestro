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

//This file contains a copy/conversion of some of the internal MapGuide Header files
//This file is prone to breaking if the MapGuide Server is changed

namespace OSGeo.MapGuide.MaestroAPI.Serialization
{
	internal enum MgPacketHeader 
		: uint
	{
		Unknown                   =   0x1111FF01,
		Operation                 =   0x1111FF02,
		OperationResponse         =   0x1111FF03,
		ArgumentSimple            =   0x1111FF04,
		ArgumentCollection        =   0x1111FF05,
		ArgumentBinaryStream      =   0x1111FF06,
		Control                   =   0x1111FF07,
		Error                     =   0x1111FF08,
		END                       =   0x1111FF09
	};

	internal enum MgServiceID
	{
		Unknown           =   0x1111FE01,
		Drawing           =   0x1111FE02,
		Feature           =   0x1111FE03,
		Mapping           =   0x1111FE04,
		Rendering         =   0x1111FE05,
		Resource          =   0x1111FE06,
		ServerAdmin       =   0x1111FE07,
		Site              =   0x1111FE08,
		Tile              =   0x1111FE09,
		Kml               =   0x1111FE0A, 
		END               =   0x1111FE0B 

	};

	internal enum MgECode
	{
		Success            =   0x1111FD01,
		SuccessWithWarning =   0x1111FD02,
		Failure            =   0x1111FD03
	};

	internal enum MgArgumentType 
		: uint
	{
		Unknown          =   0x1111FC01,
		INT8             =   0x1111FC02,
		UINT8            =   0x1111FC03,
		INT16            =   0x1111FC04,
		UINT16           =   0x1111FC05,
		INT32            =   0x1111FC06,
		UINT32           =   0x1111FC07,
		INT64            =   0x1111FC08,
		UINT64           =   0x1111FC09,
		Float            =   0x1111FC0A,
		Double           =   0x1111FC0B,
		String           =   0x1111FC0C,
		Stream           =   0x1111FC0D,
		ClassId          =   0x1111FC0E,
		END              =   0x1111FC0F
	};

	internal enum MgControlID
	{
		Unknown       =   0x1111FB01,
		UserStop      =   0x1111FB02,
		ServerStop    =   0x1111FB03,
		Pause         =   0x1111FB04,
		Continue      =   0x1111FB05,
		Close         =   0x1111FB06
	};

	internal enum MgStreamContentType
	{
		Unknown = 0x1111FA00
	};

	internal enum UserInformationType
	{
		None = 0, 
		Mg,  
		MgSession,
	};

	internal enum MgStreamHeaderValues
	{
		StreamStart   =   0x1111F801,
		StreamData    =   0x1111F802,
		StreamEnd     =   0x1111F803
	};

	internal abstract class MgBasicPacket
	{
		public MgPacketHeader PacketHeader;

		public MgBasicPacket()
		{
		}

		public MgBasicPacket(MgPacketHeader header)
		{
			PacketHeader = header;
		}
	};

	internal class MgArgumentPacket
		: MgBasicPacket
	{
		public MgArgumentType ArgumentType;
		public object Data;
		public UInt64 Length;

		public MgArgumentPacket()
		{
		}

		public MgArgumentPacket(MgPacketHeader header, MgArgumentType type, object data, UInt64 length)
			: base(header)
		{
			ArgumentType = type;
			Data = data;
			Length = length;
		}
	}

	internal class MgBinaryStreamArgumentPacket
		: MgArgumentPacket
	{
		public UInt32 Version;
		public const System.UInt32 MG_STREAM_VERSION = 1;

		public MgBinaryStreamArgumentPacket()
		{
		}

		public MgBinaryStreamArgumentPacket(MgPacketHeader header, MgArgumentType type, object data, UInt64 length, UInt32 version)
			: base(header, type, data, length)
		{
			Version = version;
		}
	}

	internal class MgStreamHeader
	{
		public MgStreamHeaderValues StreamStart;
		public UInt32 StreamVersion;
		public MgStreamHeaderValues StreamDataHdr;

		public const UInt32 CurrentStreamVersion = (3<<16) + 0;

		public MgStreamHeader()
		{
		}

		public MgStreamHeader(MgStreamHeaderValues streamStart, UInt32 streamVersion, MgStreamHeaderValues streamDataHdr)
		{
			StreamStart = streamStart;
			StreamVersion = streamVersion;
			StreamDataHdr = streamDataHdr;
		}
	}

	internal class MgOperationPacket
		: MgBasicPacket
	{
		public UInt32 PacketVersion;
		public UInt32 ServiceID;
		public UInt32 OperationID;
		public UInt32 OperationVersion;
		public UInt32 NumArguments;
		public MgUserInformation UserInfo;

		public MgOperationPacket()
		{
		}

		public MgOperationPacket(MgPacketHeader header, UInt32 packetVersion, UInt32 serviceID, UInt32 operationID, UInt32 operationVersion, UInt32 numArguments, MgUserInformation userInfo)
			: base(header)
		{
			PacketVersion = packetVersion;
			ServiceID = serviceID;
			OperationID = operationID;
			OperationVersion = operationVersion;
			NumArguments = numArguments;
			UserInfo = userInfo;
		}

	}

	internal class MgUserInformation
	{
		/*private static UInt32 m_cls_id = 30606;
		private string m_username;
		private string m_password;
		private string m_sessionId;
		private string m_locale;
		private UserInformationType m_type;

		private string m_clientAgent;
		private string m_clientIp;*/
	}

	internal class MgOperationResponsePacket
		: MgBasicPacket
	{
		public UInt32 PacketVersion;
		public MgECode ECode;
		public UInt32 NumReturnValues;

		public MgOperationResponsePacket() 
		{
		}

		public MgOperationResponsePacket(MgPacketHeader header, UInt32 packetVersion, MgECode eCode, UInt32 numReturnValues)
			: base(header)
		{
			PacketVersion = packetVersion;
			ECode = eCode;
			NumReturnValues = numReturnValues ;
		}

	}

	internal class MgControlPacket
		: MgBasicPacket
	{
		public UInt32 PacketVersion;
		public UInt32 ControlID;

		public MgControlPacket() 
		{
		}

		public MgControlPacket(MgPacketHeader header, UInt32 packetVersion, UInt32 controlID)
			: base(header)
		{
			PacketVersion = packetVersion;
			ControlID = controlID;
		}
	}


}
