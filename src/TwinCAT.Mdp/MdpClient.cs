using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;
using TwinCAT.Mdp.Abstractions;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.Mdp.Extensions;
using TwinCAT.TypeSystem;

namespace TwinCAT.Mdp
{
	/// <summary>
	/// Initializes a new instance of the TwinCAT.Mdp.MdpClient class.
	/// </summary>
	public sealed class MdpClient
		: IMdpDisposableConnection,
			IMdpConnection,
			IMdpConnectAddress,
			IConnectionStateProvider,
			IDisposable
	{
		private const uint AdsCoEIndexGroup = 0xF302;

		private readonly AdsClient _adsClient;
		private readonly ILogger _logger;
		private readonly Dictionary<uint, ModuleType> _modules = new Dictionary<uint, ModuleType>();

		private bool _disposed;
		private AmsNetId _target;

		/// <summary>
		/// Initializes a new instance of the TwinCAT.Mdp.MdpClient class.
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="settings"></param>
		public MdpClient(ILogger logger, AdsClientSettings settings)
		{
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			this._logger = logger;

			_adsClient = new AdsClient(null, settings, _logger);
		}

		/// <summary>
		/// Initializes a new instance of the TwinCAT.Mdp.MdpClient class.
		/// </summary>
		/// <param name="logger"></param>
		public MdpClient(ILogger logger)
			: this(logger, AdsClientSettings.Default) { }

		/// <summary>
		/// Initializes a new instance of the TwinCAT.Mdp.MdpClient class.
		/// </summary>
		public MdpClient()
			: this(null, AdsClientSettings.Default) { }

		/// <summary>
		/// Initializes a new instance of the TwinCAT.Mdp.MdpClient class.
		/// </summary>
		/// <param name="settings"></param>
		public MdpClient(AdsClientSettings settings)
			: this(null, settings) { }

		/// <summary>
		/// Finalizes an instance of the TwinCAT.Mdp.MdpClient class.
		/// </summary>
		~MdpClient() => this.Dispose(false);

		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		public bool IsDisposed => this._disposed;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (!this._disposed)
			{
				this.Dispose(true);
			}
			this._disposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			if (disposing && this._adsClient.IsConnected)
			{
				this.Disconnect();
				_adsClient.Dispose();
			}
		}

		/// <summary>
		/// Connects to the target MDP Device.
		/// </summary>
		/// <param name="target">The AmsNetId of the target device.</param>
		public void Connect(AmsNetId target)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(this.ToString());
			}

			if (this.IsConnected)
			{
				this.Disconnect();
			}

			_target = target;

			this._adsClient.ConnectionStateChanged += (s, e) =>
			{
				this.ConnectionStateChanged?.Invoke(s, e);
			};

			this._adsClient.Connect(target, AmsPort.SystemService);

			this.ScanModules();
		}

		/// <summary>
		/// Connects to the target MDP Device.
		/// </summary>
		/// <param name="target">The AmsNetId of the target device.</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>Returns a task object that represents the operation.</returns>
		public async Task ConnectAsync(AmsNetId target, CancellationToken cancel)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(this.ToString());
			}

			if (this.IsConnected)
			{
				this.Disconnect();
			}

			_target = target;
			this._adsClient.Connect(target, AmsPort.SystemService);

			await this.ScanModulesAsync(cancel);
		}

		/// <summary>
		/// Connects to the target MDP Device.
		/// </summary>
		public void Connect() => this.Connect(AmsNetId.Local);

		/// <summary>
		/// Connects to the target MDP Device.
		/// </summary>
		/// <param name="target"></param>
		public void Connect(string target) => this.Connect(AmsNetId.Parse(target));

		/// <summary>
		/// Connects to the target MDP Device.
		/// </summary>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>Returns a task object that represents the operation.</returns>
		public Task ConnectAsync(CancellationToken cancel) =>
			this.ConnectAsync(AmsNetId.Local, cancel);

		/// <summary>
		/// Connects to the target MDP Device.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>Returns a task object that represents the operation.</returns>
		public Task ConnectAsync(string target, CancellationToken cancel) =>
			this.ConnectAsync(AmsNetId.Parse(target), cancel);

		/// <summary>
		/// Gets the target TwinCAT.Ads.AmsNetId of of the established MDP connection (Destination side).
		/// </summary>
		public AmsNetId Target => this._target;

		/// <summary>
		/// Gets the raw ADS connection / ADS client
		/// </summary>
		public IAdsConnection Connection => this._adsClient as IAdsConnection;

		/// <summary>
		/// Gets a value indicating whether the local ADS port was opened successfully. It does not indicate if the target port is available. Use the method ReadState to determine if the target port is available.
		/// </summary>
		public bool IsConnected => this._adsClient.IsConnected;

		/// <summary>
		/// Gets a value indicating whether the ADS client is connected to a ADS Server on the local computer.
		/// </summary>
		public bool IsLocal => this._adsClient.IsLocal;

		/// <summary>
		/// Sets the timeout for the ads communication. Unit is in ms.
		/// </summary>
		public int Timeout
		{
			get => this._adsClient.Timeout;
			set => this._adsClient.Timeout = value;
		}

		/// <summary>
		/// Gets the current Connection state of the TwinCAT.IConnectionStateProvider
		/// </summary>
		public ConnectionState ConnectionState => this._adsClient.ConnectionState;

		/// <summary>
		/// Occurs when the connection state has been changed.
		/// </summary>
		public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

		/// <summary>
		///  Disconnects this TwinCAT.Ads.AdsClient from the local ADS router.
		/// </summary>
		/// <returns>true if disconnected, false otherwise.</returns>
		public bool Disconnect()
		{
			_modules.Clear();
			return _adsClient.Disconnect();
		}

		/// <summary>
		/// Gets a enumerable of all available MDP modules.
		/// </summary>
		public IEnumerable<ModuleType> Modules => _modules.Values;

		/// <summary>
		/// Gets the count of all available MDP modules.
		/// </summary>
		public int ModuleCount => _modules.Count;

		/// <summary>
		/// Reads any data from the specified MDP address with the given data type.
		/// </summary>
		/// <param name="address">The MDP address to read.</param>
		/// <param name="type">The data type to read.</param>
		/// <returns>The read data.</returns>
		public object ReadAny(MdpAddress address, Type type)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(type))
			{
				throw new ArgumentException(type.ToString());
			}

			if (type == typeof(string))
			{
				return _adsClient.ReadAnyString(
					AdsCoEIndexGroup,
					ToIndexOffset(address),
					255,
					System.Text.Encoding.ASCII
				);
			}
			else
			{
				return _adsClient.ReadAny(AdsCoEIndexGroup, ToIndexOffset(address), type);
			}
		}

		/// <summary>
		/// Reads any data from the specified MDP address with the given data type asynchronously.
		/// </summary>
		/// <param name="address">The MDP address to read.</param>
		/// <param name="type">The data type to read.</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public async Task<object> ReadAnyAsync(
			MdpAddress address,
			Type type,
			CancellationToken cancel
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(type))
			{
				throw new ArgumentException(type.ToString());
			}

			if (type == typeof(string))
			{
				var result = await _adsClient.ReadAnyStringAsync(
					AdsCoEIndexGroup, 
					ToIndexOffset(address),
					255, 
					System.Text.Encoding.ASCII, cancel
				);

				result.ThrowOnError();

				return result.Value;
			}
			else
			{
				var result = await _adsClient.ReadAnyAsync(
					AdsCoEIndexGroup,
					ToIndexOffset(address),
					type,
					null,
					cancel
				);

				result.ThrowOnError();

				return result.Value;
			}
		}

		/// <summary>
		/// Reads any data from the specified MDP address with the given data type.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="address">The MDP address to read.</param>
		/// <returns>The read data.</returns>
		public T ReadAny<T>(MdpAddress address)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(typeof(T)))
			{
				throw new ArgumentException(typeof(T).ToString());
			}

			if (typeof(T) == typeof(string))
			{
				var value = _adsClient.ReadAnyString(AdsCoEIndexGroup, ToIndexOffset(address), 255, System.Text.Encoding.ASCII);
				return (T)Convert.ChangeType(value, typeof(T));
			}
			else
			{
				return _adsClient.ReadAny<T>(AdsCoEIndexGroup, ToIndexOffset(address));
			}
		}

		/// <summary>
		/// Reads any data from the specified MDP address with the given data type asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="address">The MDP address to read.</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public async Task<T> ReadAnyAsync<T>(MdpAddress address, CancellationToken cancel)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(typeof(T)))
			{
				throw new ArgumentException(typeof(T).ToString());
			}

			if (typeof(T) == typeof(string))
			{
				var result = await _adsClient.ReadAnyStringAsync(
					AdsCoEIndexGroup,
					ToIndexOffset(address),
					255,
					System.Text.Encoding.ASCII,
				cancel
				);

				result.ThrowOnError();

				return (T)Convert.ChangeType(result.Value, typeof(T));
			}
			else
			{
				var result = await _adsClient.ReadAnyAsync(
					AdsCoEIndexGroup,
					ToIndexOffset(address),
					typeof(T),
					cancel
				);

				result.ThrowOnError();

				return (T)Convert.ChangeType(result.Value, typeof(T));
			}
		}

		/// <summary>
		/// Reads data from the specified MDP address into the provided buffer.
		/// </summary>
		/// <param name="address">The MDP address to read.</param>
		/// <param name="readBuffer">The buffer to store the read data.</param>
		/// <returns>The number of bytes read.</returns>
		public int Read(MdpAddress address, Memory<byte> readBuffer)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			return _adsClient.Read(AdsCoEIndexGroup, ToIndexOffset(address), readBuffer);
		}

		/// <summary>
		/// Reads data from the specified MDP address into the provided buffer asynchronously.
		/// </summary>
		/// <param name="address">The MDP address to read.</param>
		/// <param name="readBuffer">The buffer to store the read data.</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public Task<ResultRead> ReadAsync(
			MdpAddress address,
			Memory<byte> readBuffer,
			CancellationToken cancel
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			return _adsClient.ReadAsync(AdsCoEIndexGroup, ToIndexOffset(address), readBuffer, cancel);
		}

		/// <summary>
		/// Writes any data to the specified MDP address.
		/// </summary>
		/// <param name="address">The MDP address to write to.</param>
		/// <param name="value">The data to write.</param>
		public void WriteAny(MdpAddress address, object value)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			if (!IsValidDataType(value.GetType()))
			{
				throw new ArgumentException(value.GetType().ToString());
			}

			if (value.GetType() == typeof(string))
			{
				string data = Convert.ToString(value);
				_adsClient.WriteAnyString(AdsCoEIndexGroup, ToIndexOffset(address), data, data.Length, System.Text.Encoding.ASCII);
			}
			else
			{
				_adsClient.WriteAny(AdsCoEIndexGroup, ToIndexOffset(address), value);
			}
		}

		/// <summary>
		/// Writes any data to the specified MDP address asynchronously.
		/// </summary>
		/// <param name="address">The MDP address to write to.</param>
		/// <param name="value">The data to write.</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public async Task WriteAnyAsync(MdpAddress address, object value, CancellationToken cancel)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			if (!IsValidDataType(value.GetType()))
			{
				throw new ArgumentException(value.GetType().ToString());
			}

			if (value.GetType() == typeof(string))
			{
				string data = Convert.ToString(value);
				var result = await _adsClient.WriteAnyStringAsync(AdsCoEIndexGroup, ToIndexOffset(address), data, data.Length, System.Text.Encoding.ASCII, cancel);
				result.ThrowOnError();
			}
			else
			{
				var result = await _adsClient.WriteAnyAsync(AdsCoEIndexGroup, ToIndexOffset(address), value, cancel);
				result.ThrowOnError();
			}
		}

		/// <summary>
		/// Writes data from the provided buffer to the specified MDP address.
		/// </summary>
		/// <param name="address">The MDP address to write to.</param>
		/// <param name="writeBuffer">The buffer containing the data to write.</param>
		public void Write(MdpAddress address, ReadOnlyMemory<byte> writeBuffer)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			_adsClient.Write(AdsCoEIndexGroup, ToIndexOffset(address), writeBuffer);
		}

		/// <summary>
		/// Writes data from the provided buffer to the specified MDP address asynchronously.
		/// </summary>
		/// <param name="address">The MDP address to write to.</param>
		/// <param name="writeBuffer">The buffer containing the data to write.</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public async Task WriteAsync(
			MdpAddress address,
			ReadOnlyMemory<byte> writeBuffer,
			CancellationToken cancel
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			var result = await _adsClient.WriteAsync(AdsCoEIndexGroup, ToIndexOffset(address), writeBuffer, cancel);
			result.ThrowOnError();
		}

		/// <summary>
		/// Reads a parameter from the specified MDP address with the given data type.
		/// </summary>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="readBuffer">The buffer to store the read data.</param>
		/// <param name="moduleIndex">The module index (default is 1).</param>
		public void ReadParameter(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Memory<byte> readBuffer,
			uint moduleIndex = 1
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!_modules.Values.Contains(moduleType))
			{
				throw new ArgumentOutOfRangeException(nameof(moduleType));
			}

			var moduleID = GetModuleID(moduleType, moduleIndex);

			Read(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = moduleID,
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				},
				readBuffer
			);
		}

		/// <summary>
		/// Reads a parameter from the specified MDP address with the given data type.
		/// </summary>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type to read.</param>
		/// <param name="moduleIndex">The module index (default is 1).</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public object ReadParameter(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			uint moduleIndex = 1
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(type))
			{
				throw new ArgumentException(type.ToString());
			}

			if (!_modules.Values.Contains(moduleType))
			{
				throw new ArgumentOutOfRangeException(nameof(moduleType));
			}

			var moduleID = GetModuleID(moduleType, moduleIndex);

			return ReadAny(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = moduleID,
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				},
				type
			);
		}

		/// <summary>
		/// Reads a parameter from the specified MDP address with the given data type asynchronously.
		/// </summary>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type to read.</param>
		/// <param name="moduleIndex">The module index (default is 1).</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public Task<object> ReadParameterAsync(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			CancellationToken cancel,
			uint moduleIndex = 1
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(type))
			{
				throw new ArgumentException(type.ToString());
			}

			if (!_modules.Values.Contains(moduleType))
			{
				throw new ArgumentOutOfRangeException(nameof(moduleType));
			}

			var moduleID = GetModuleID(moduleType, moduleIndex);

			return ReadAnyAsync(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = moduleID,
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				},
				type,
				cancel
			);
		}

		/// <summary>
		/// Reads a parameter from the specified MDP address with the given data type.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="moduleIndex">The module index (default is 1).</param>
		/// <returns>The read data.</returns>
		public T ReadParameter<T>(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			uint moduleIndex = 1
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(typeof(T)))
			{
				throw new ArgumentException(typeof(T).ToString());
			}

			if (!_modules.Values.Contains(moduleType))
			{
				throw new ArgumentOutOfRangeException(nameof(moduleType));
			}

			var moduleID = GetModuleID(moduleType, moduleIndex);

			return ReadAny<T>(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = moduleID,
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				}
			);
		}

		/// <summary>
		/// Reads a parameter from the specified MDP address with the given data type asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <param name="moduleIndex">The module index (default is 1).</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public Task<T> ReadParameterAsync<T>(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			CancellationToken cancel,
			uint moduleIndex = 1
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(typeof(T)))
			{
				throw new ArgumentException(typeof(T).ToString());
			}

			if (!_modules.Values.Contains(moduleType))
			{
				throw new ArgumentOutOfRangeException(nameof(moduleType));
			}

			var moduleID = GetModuleID(moduleType, moduleIndex);

			return ReadAnyAsync<T>(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = moduleID,
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				},
				cancel
			);
		}

		/// <summary>
		/// Writes a parameter from the specified MDP address with the given data type.
		/// </summary>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="value">The data to read.</param>
		/// <param name="moduleIndex">The module index (default is 1).</param>
		/// <returns>Nothing.</returns>
		public void WriteParameter(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			object value, 
			uint moduleIndex = 1
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(value.GetType()))
			{
				throw new ArgumentException(value.GetType().ToString());
			}

			if (!_modules.Values.Contains(moduleType))
			{
				throw new ArgumentOutOfRangeException(nameof(moduleType));
			}

			var moduleID = GetModuleID(moduleType, moduleIndex);

			WriteAny(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = moduleID,
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				},
				value
			);
		}

		/// <summary>
		/// Writes a parameter from the specified MDP address with the given data type asynchronously.
		/// </summary>
		/// <param name="moduleType">The MDP module type.</param>
		/// <param name="tableID">The table ID.</param>
		/// <param name="subIndex">The sub-index.</param>
		/// <param name="type">The data type to read.</param>
		/// <param name="moduleIndex">The module index (default is 1).</param>
		/// <param name="cancel">Cancellation Token.</param>
		/// <returns>A task object that represents the asynchronous operation.</returns>
		public Task WriteParameterAsync(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			object value,
			CancellationToken cancel,
			uint moduleIndex = 1
		)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("MdpClient");
			}

			if (!this.IsConnected)
			{
				throw new ClientNotConnectedException(_adsClient);
			}

			if (!IsValidDataType(value.GetType()))
			{
				throw new ArgumentException(value.GetType().ToString());
			}

			if (!_modules.Values.Contains(moduleType))
			{
				throw new ArgumentOutOfRangeException(nameof(moduleType));
			}

			var moduleID = GetModuleID(moduleType, moduleIndex);

			return WriteAnyAsync(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = moduleID,
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				},
				value,
				cancel
			);
		}

		/// <summary>
		/// Converts a MDP module type to the corresponding unique module ID.
		/// </summary>
		/// <param name="moduleType">MDP module type.</param>
		/// <param name="moduleIndex">Index if more than one module is available of the same type.</param>
		/// <returns>The unique module ID.</returns>
		public uint GetModuleID(ModuleType moduleType, uint moduleIndex = 1)
		{
			var moduleID = _modules.Where(m => m.Value == moduleType).Select(m => m.Key).ToArray()[
				moduleIndex - 1
			];

			if (moduleID == 0)
			{
				throw new IndexOutOfRangeException();
			}

			return moduleID;
		}

		private int GetModuleCount()
		{
			return (ushort)_adsClient.ReadAny(0xF302, 0xF0200000, typeof(ushort));
		}

		private async Task<int> GetModuleCountAsync(CancellationToken cancel)
		{
			var result = await _adsClient.ReadAnyAsync(
				0xF302,
				0xF0200000,
				typeof(ushort),
				null,
				cancel
			);
			result.ThrowOnError();
			return (ushort)result.Value;
		}

		private void ScanModules()
		{
			var moduleCount = this.GetModuleCount();

			for (int i = 0; i < moduleCount + 1; i++)
			{
				try
				{
					var moduleInfo = this.ScanModule((uint)(0xF0200000 + i));

					if (!_modules.ContainsKey(moduleInfo.ID))
					{
						_modules.Add(moduleInfo.ID, moduleInfo.Type);
					}
				}
				catch (Exception ex) { }
			}
		}

		private async Task ScanModulesAsync(CancellationToken cancel)
		{
			var moduleCount = await this.GetModuleCountAsync(cancel);

			for (int i = 0; i < moduleCount + 1; i++)
			{
				var moduleInfo = await this.ScanModuleAsync((uint)(0xF0200000 + i), cancel);

				if (!_modules.ContainsKey(moduleInfo.ID))
				{
					_modules.Add(moduleInfo.ID, moduleInfo.Type);
				}
			}
		}

		private ModuleInfo ScanModule(uint address)
		{
			var mdpModule = (uint)_adsClient.ReadAny(AdsCoEIndexGroup, address, typeof(uint));

			return new ModuleInfo
			{
				ID = mdpModule,
				Type = (ModuleType)((mdpModule & 0xFFFF0000) >> 16)
			};
		}

		private async Task<ModuleInfo> ScanModuleAsync(uint address, CancellationToken cancel)
		{
			var mdpModule = await _adsClient.ReadAnyAsync(AdsCoEIndexGroup, address, typeof(uint), cancel);

			return new ModuleInfo
			{
				ID = (uint)mdpModule.Value,
				Type = (ModuleType)(((uint)mdpModule.Value & 0xFFFF0000) >> 16)
			};
		}

		private bool IsValidDataType(Type type)
		{
			if (
				type != typeof(bool)
				&& type != typeof(ushort)
				&& type != typeof(uint)
				&& type != typeof(ulong)
				&& type != typeof(short)
				&& type != typeof(int)
				&& type != typeof(long)
				&& type != typeof(float)
				&& type != typeof(double)
				&& type != typeof(string)
			)
			{
				return false;
			}

			return true;
		}

		private uint ToIndexOffset(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			uint moduleIndex = 1
		)
		{
			return this.ToIndexOffset(
				new MdpAddress
				{
					Area = MdpArea.Config,
					ModuleID = this.GetModuleID(moduleType, moduleIndex),
					Flag = 0,
					TableID = tableID,
					SubIndex = subIndex
				}
			);
		}

		private uint ToIndexOffset(MdpAddress address)
		{
			/*
			IndexOffset = MDP index und subindex.
			    32          24          16          8           0
			    |           |           |           |           |
			    |		Index - 16Bit   |flags-8Bit |SubIdx-8Bit|
			    |           |           |           |           |

			  Index:	0x AnnX
			          A:	4Bit Area Code
			          nn:	8Bit Module ID
			          X:	4Bit Table ID
			*/

			return (
				((uint)address.Area << 28)
				| (address.ModuleID << 20)
				| ((uint)address.TableID << 16)
				| ((uint)address.Flag << 8)
				| address.SubIndex
			);
		}
	}
}
