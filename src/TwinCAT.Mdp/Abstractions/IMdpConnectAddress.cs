using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace TwinCAT.Mdp.Abstractions
{
	/// <summary>
	/// Interface for method to connect the MDP client via AMS Address.
	/// </summary>
	public interface IMdpConnectAddress
	{
		/// <summary>
		/// Connects the target ADS Device.
		/// </summary>
		/// <param name="target">The address of the target device.</param>
		void Connect(AmsNetId target);

		/// <summary>
		/// Connects to the target ADS Device asyncronuously.
		/// </summary>
		/// <param name="target">The address of the target device.</param>
		/// <param name="cancel">The cancel.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task ConnectAsync(AmsNetId target, CancellationToken cancel);

		/// <summary>
		/// Connects to the target local ADS Device.
		/// </summary>
		void Connect();

		/// <summary>
		/// Connects to the target ADS Device.
		/// </summary>
		/// <param name="target">The TwinCAT.Ads.AmsNetId of the ADS target device specified as string.</param>
		void Connect(string target);

		/// <summary>
		/// Connects to the local target ADS Device asyncronuously.
		/// </summary>
		/// <param name="cancel">The cancel.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task ConnectAsync(CancellationToken cancel);

		/// <summary>
		/// Connects to the target ADS Device asyncronuously.
		/// </summary>
		/// <param name="target">The TwinCAT.Ads.AmsNetId of the ADS target device specified as string.</param>
		/// <param name="cancel">The cancel.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task ConnectAsync(string target, CancellationToken cancel);

		/// <summary>
		/// Gets a value indicating whether the MDP client is connected.
		/// </summary>
		bool IsConnected { get; }
	}
}
