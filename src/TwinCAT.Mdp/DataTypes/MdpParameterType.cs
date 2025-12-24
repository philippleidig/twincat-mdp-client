using System;
using System.Collections.Generic;
using System.Text;

namespace TwinCAT.Mdp.DataTypes
{
	/// <summary>
	/// Defines the data types supported by MDP (Modular Device Profile) parameters.
	/// These types correspond to standard .NET primitive types.
	/// </summary>
	public enum MdpParameterType
	{
		/// <summary>Boolean value (maps to <see cref="bool"/>).</summary>
		BOOLEAN,

		/// <summary>16-bit signed integer (maps to <see cref="short"/>).</summary>
		SIGNED16,

		/// <summary>32-bit signed integer (maps to <see cref="int"/>).</summary>
		SIGNED32,

		/// <summary>64-bit signed integer (maps to <see cref="long"/>).</summary>
		SIGNED64,

		/// <summary>8-bit unsigned integer (maps to <see cref="byte"/>).</summary>
		UNSIGNED8,

		/// <summary>16-bit unsigned integer (maps to <see cref="ushort"/>).</summary>
		UNSIGNED16,

		/// <summary>32-bit unsigned integer (maps to <see cref="uint"/>).</summary>
		UNSIGNED32,

		/// <summary>64-bit unsigned integer (maps to <see cref="ulong"/>).</summary>
		UNSIGNED64,

		/// <summary>32-bit floating point number (maps to <see cref="float"/>).</summary>
		REAL32,

		/// <summary>Variable-length string (maps to <see cref="string"/>).</summary>
		VISIBLE_STRING
	}
}
