using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Abstractions
{
	public interface IMdpConnection
	{
		IAdsConnection Connection { get; }

		IEnumerable<ModuleType> Modules { get; }

		int ModuleCount { get; }

		object ReadAny(MdpAddress address, Type type);

		T ReadAny<T>(MdpAddress address);

		Task<object> ReadAnyAsync(MdpAddress address, Type type, CancellationToken cancel);

		Task<T> ReadAnyAsync<T>(MdpAddress address, CancellationToken cancel);

		int Read(MdpAddress address, Memory<byte> readBuffer);

		Task<ResultRead> ReadAsync(
			MdpAddress address,
			Memory<byte> readBuffer,
			CancellationToken cancel
		);

		void WriteAny(MdpAddress address, object value);

		Task WriteAnyAsync(MdpAddress address, object value, CancellationToken cancel);

		void Write(MdpAddress address, ReadOnlyMemory<byte> writeBuffer);

		Task WriteAsync(
			MdpAddress address,
			ReadOnlyMemory<byte> writeBuffer,
			CancellationToken cancel
		);

		void ReadParameter(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Memory<byte> readBuffer,
			uint moduleIndex = 1
		);

		object ReadParameter(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			uint moduleIndex = 1
		);

		T ReadParameter<T>(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			uint moduleIndex = 1
		);

		Task<T> ReadParameterAsync<T>(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			CancellationToken cancel,
			uint moduleIndex = 1
		);

		Task<object> ReadParameterAsync(
			ModuleType moduleType,
			byte tableID,
			byte subIndex,
			Type type,
			CancellationToken cancel,
			uint moduleIndex = 1
		);

		uint GetModuleID(ModuleType moduleType, uint moduleIndex = 1);
	}
}
