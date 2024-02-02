using System;
using System.Collections.Generic;
using System.Text;

namespace TwinCAT.Mdp.DataTypes
{
	/// <summary>
	/// When addressing a table, it is important to specify which area is to be addressed, because the Beckhoff Device Manager is divided into different areas:
	/// </summary>
	public enum MdpArea : byte
	{
		/// <summary>
		/// In the General Area various general data of the IPC are summarized.
		/// </summary>
		General = 0x1,

		/// <summary>
		/// Here the individual modules and their offered information are created.
		/// </summary>
		Config = 0x8,

		/// <summary>
		/// The Service Transfer Area provides functional access.
		/// </summary>
		Service = 0xB,

		/// <summary>
		/// Here the IPC diagnosis enters, for example, which modules were automatically recognized at startup, depending on the hardware and software. In contrast to the Configuration Area, the Device Area does not consist of subordinate modules, but only of individual tables.
		/// </summary>
		Device = 0xF
	}
}
