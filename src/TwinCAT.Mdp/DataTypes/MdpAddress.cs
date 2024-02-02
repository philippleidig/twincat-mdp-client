using System;
using System.Collections.Generic;
using System.Text;

namespace TwinCAT.Mdp.DataTypes
{
	/// <summary>
	/// Modular Device Profile address to access a specific parameter
	/// </summary>
	public class MdpAddress
	{
		/// <summary>
		/// Area [range: 0x0 - 0xF]
		/// </summary>
		public MdpArea Area;

		/// <summary>
		/// Dynamic Module Id[range: 0x00 - 0xFF]
		/// </summary>
		public uint ModuleID;

		/// <summary>
		/// Table Id[range: 0x0 - 0xF]
		/// </summary>
		public byte TableID;

		/// <summary>
		/// NOT USED - Flags [range: 0x00 - 0xFF]
		/// </summary>
		public byte Flag;

		/// <summary>
		/// SubIndex [range: 0x00 - 0xFF]
		/// </summary>
		public byte SubIndex;
	}
}
