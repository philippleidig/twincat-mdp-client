using System;
using System.Collections.Generic;
using System.Text;

namespace TwinCAT.Mdp.DataTypes
{
	public enum ModuleType
	{
		NIC = 0x0002,
		Time = 0x0003,
		UserManagement = 0x0004,
		RAS = 0x0005,
		FTP = 0x0006,
		SMB = 0x0007,
		TwinCAT = 0x0008,
		Datastore = 0x0009,
		Software = 0x000A,
		CPU = 0x000B,
		Memory = 0x000C,
		Firewall = 0x000E,
		FileSystemObject = 0x0010,
		PLC = 0x0012,
		DisplayDevice = 0x0013,
		EWF = 0x0014,
		FBWF = 0x0015,
		SiliconDrive = 0x0017,
		OS = 0x0018,
		Raid = 0x0019,
		Fan = 0x001B,
		Mainboard = 0x001C,
		DiskManagement = 0x001D,
		UPS = 0x001E,
		PhysicalDrive = 0x001F,
		MassStorage = 0x0020,
		Misc = 0x0100
	}
}
