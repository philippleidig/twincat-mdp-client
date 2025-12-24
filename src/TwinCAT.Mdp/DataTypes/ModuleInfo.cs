using System;
using System.Collections.Generic;
using System.Text;

namespace TwinCAT.Mdp.DataTypes
{
	/// <summary>
	/// Contains information about a Device Manager module.
	/// </summary>
	public class ModuleInfo
	{
		/// <summary>
		/// Gets or sets the unique module identifier.
		/// </summary>
		public ushort ID;

		/// <summary>
		/// Gets or sets the module type.
		/// </summary>
		public ModuleType Type;
	}
}
