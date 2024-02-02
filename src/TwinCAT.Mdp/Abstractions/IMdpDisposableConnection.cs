using System;
using System.Collections.Generic;
using System.Text;
using TwinCAT.Ads;

namespace TwinCAT.Mdp.Abstractions
{
	/// <summary>
	/// Implements the IDisposable interface
	/// </summary>
	public interface IMdpDisposableConnection : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		bool IsDisposed { get; }
	}
}
