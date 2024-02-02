using System;
using System.Collections.Generic;
using System.Text;

namespace TwinCAT.Mdp.DataTypes
{
	public enum MdpParameterType
	{
		BOOLEAN, // bool
		SIGNED16, // short
		SIGNED32, // int
		SIGNED64, // long
		UNSIGNED8, // byte
		UNSIGNED16, // ushort
		UNSIGNED32, // uint
		UNSIGNED64, // ulong
		REAL32, // float
		VISIBLE_STRING // string
	}
}
