using System;
using System.Collections.Generic;
using System.Text;

namespace TwinCAT.Mdp.DataTypes
{
	/// <summary>
	/// Defines the different module types available in the Beckhoff Device Manager.
	/// Each module provides specific functionality and parameters for managing IPC components.
	/// </summary>
	public enum ModuleType : ushort
	{
		/// <summary>Network Interface Card module for network configuration.</summary>
		NIC = 0x0002,

		/// <summary>Time module for system time and time zone configuration.</summary>
		Time = 0x0003,

		/// <summary>User Management module for user accounts and permissions.</summary>
		UserManagement = 0x0004,

		/// <summary>Remote Access Service module.</summary>
		RAS = 0x0005,

		/// <summary>FTP Server module configuration.</summary>
		FTP = 0x0006,

		/// <summary>SMB/CIFS file sharing module.</summary>
		SMB = 0x0007,

		/// <summary>TwinCAT runtime and configuration module.</summary>
		TwinCAT = 0x0008,

		/// <summary>Datastore module for persistent data storage.</summary>
		Datastore = 0x0009,

		/// <summary>Software management and installation module.</summary>
		Software = 0x000A,

		/// <summary>CPU information and configuration module.</summary>
		CPU = 0x000B,

		/// <summary>Memory information and configuration module.</summary>
		Memory = 0x000C,

		/// <summary>Firewall configuration module.</summary>
		Firewall = 0x000E,

		/// <summary>File system object management module.</summary>
		FileSystemObject = 0x0010,

		/// <summary>PLC (Programmable Logic Controller) module.</summary>
		PLC = 0x0012,

		/// <summary>Display device configuration module.</summary>
		DisplayDevice = 0x0013,

		/// <summary>Enhanced Write Filter module for disk write protection.</summary>
		EWF = 0x0014,

		/// <summary>File-Based Write Filter module.</summary>
		FBWF = 0x0015,

		/// <summary>Silicon Drive (SSD) management module.</summary>
		SiliconDrive = 0x0017,

		/// <summary>Operating System information module.</summary>
		OS = 0x0018,

		/// <summary>RAID configuration and status module.</summary>
		Raid = 0x0019,

		/// <summary>Fan monitoring and control module.</summary>
		Fan = 0x001B,

		/// <summary>Mainboard information module.</summary>
		Mainboard = 0x001C,

		/// <summary>Disk management module for partitioning and formatting.</summary>
		DiskManagement = 0x001D,

		/// <summary>UPS (Uninterruptible Power Supply) monitoring module.</summary>
		UPS = 0x001E,

		/// <summary>Physical drive information and SMART data module.</summary>
		PhysicalDrive = 0x001F,

		/// <summary>Mass storage device management module.</summary>
		MassStorage = 0x0020,

		/// <summary>Miscellaneous module for other functionality.</summary>
		Misc = 0x0100
	}
}
