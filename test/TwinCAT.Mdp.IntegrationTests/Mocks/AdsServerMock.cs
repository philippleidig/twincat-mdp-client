using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.Server;
using TwinCAT.Ads.TcpRouter;

namespace TwinCAT.Mdp.IntegrationTests.Mocks
{
	/// <summary>
	/// Behavior / response for a ADS ReadIndication
	/// </summary>
	public record ReadIndicationBehavior(
		uint IndexGroup,
		uint IndexOffset,
		Memory<byte> ResponseData,
		AdsErrorCode ErrorCode = AdsErrorCode.Succeeded,
		Func<uint, uint, ReadOnlyMemory<byte>> onCall = null
	) : Behavior(IndexGroup, IndexOffset, ResponseData, ErrorCode);

	/// <summary>
	/// Behavior / response for a ADS WriteIndication
	/// </summary>
	public record WriteIndicationBehavior(
		uint IndexGroup,
		uint IndexOffset,
		int ExpectedLength,
		AdsErrorCode ErrorCode = AdsErrorCode.Succeeded,
		Action<uint, uint, ReadOnlyMemory<byte>> onCall = null
	) : Behavior(IndexGroup, IndexOffset, null, ErrorCode);

	/// <summary>
	/// Simple implementation of <see cref="AdsServer"/> which can be used for mocking ADS read/write functions.
	/// </summary>
	public class AdsServerMock : AdsServer
	{
		private ILogger? _Logger = null;
		private BehaviorManager? _behaviorManager = null;
		public BehaviorManager? BehaviorManager => _behaviorManager;
		private string _portName = "";
		private AmsTcpIpRouter? _router;

		public AdsServerMock(string portName)
			: base(0, portName)
		{
			Init(portName);
			Connect();
		}

		public AdsServerMock(ushort port, string portName)
			: base(port, portName)
		{
			Init(portName);
			Connect();
		}

		public AdsServerMock(string portName, ILogger? logger)
			: base(0, portName, CreateLoggerFactory(logger))
		{
			Init(portName, logger);
			Connect();
		}

		public AdsServerMock(ushort port, string portName, ILogger? logger)
			: base(port, portName, CreateLoggerFactory(logger))
		{
			Init(portName, logger);
			Connect();
		}

		public AdsServerMock(
			ushort port,
			string portName,
			bool useSingleNotificationHandler,
			ILogger? logger
		)
			: base(port, portName, null, CreateLoggerFactory(logger))
		{
			Init(portName, logger);
			Connect();
		}

		private static ILoggerFactory? CreateLoggerFactory(ILogger? logger)
		{
			if (logger == null)
				return null;

			return new SimpleLoggerFactory(logger);
		}

		private class SimpleLoggerFactory : ILoggerFactory
		{
			private readonly ILogger _logger;

			public SimpleLoggerFactory(ILogger logger)
			{
				_logger = logger;
			}

			public void AddProvider(ILoggerProvider provider) { }

			public ILogger CreateLogger(string categoryName)
			{
				return _logger;
			}

			public void Dispose() { }
		}

		public override bool Disconnect()
		{
			if (_router != null)
				_router.Stop();
			return base.Disconnect();
		}

		protected override void Dispose(bool disposing)
		{
			if (_router != null)
				_router.Stop();
			base.Dispose(disposing);
		}

		private async void Init(string portName, ILogger? logger = null)
		{
			_Logger = logger;
			_portName = portName;
			_behaviorManager = new BehaviorManager(logger);

			try
			{
				_router = new AmsTcpIpRouter(AmsNetId.LocalHost);
				await _router.StartAsync(CancellationToken.None);
			}
			catch { }
		}

		private void Connect()
		{
			try
			{
				var res = base.ConnectServer();
				if (IsConnected)
					_Logger?.LogDebug(
						"Server \"{ServerName}\" with address \"{ServerAddress}\" connected.",
						_portName,
						ServerAddress
					);
				else
					_Logger?.LogWarning(
						"Could not connect server \"{ServerName}\" with address \"{ServerAddress}\".",
						_portName,
						ServerAddress
					);
			}
			catch (AdsException ex)
			{
				_Logger?.LogError(ex, "Could not register server \"{ServerName}\".", _portName);
			}
		}

		/// <summary>
		/// Registers a behavior, which controls the response of a ADS read/write function.
		/// </summary>
		/// <param name="behavior"></param>
		/// <returns></returns>
		public AdsServerMock RegisterBehavior(Behavior behavior)
		{
			_behaviorManager?.RegisterBehavior(behavior);

			return this;
		}

		private ValueTuple<bool, int> RequestedReadLengthIsLessOrEqualToResponseDataLength(
			Memory<byte>? responseData,
			int readLength
		)
		{
			return responseData switch
			{
				null => (readLength == 0, 0),
				not null => ((readLength > 0), Math.Min(responseData.Value.Length, readLength))
			};
		}

		protected override Task<ResultReadBytes> OnReadAsync(
			AmsAddress sender,
			uint invokeId,
			uint indexGroup,
			uint indexOffset,
			int readLength,
			CancellationToken cancel
		)
		{
			var behavior = _behaviorManager?.GetBehaviorOfType<ReadIndicationBehavior>(b =>
				b.IndexGroup == indexGroup && b.IndexOffset == indexOffset
			);

			if (behavior is null)
			{
				AdsErrorCode errorCode;
				unchecked
				{
					errorCode = (AdsErrorCode)0xECA60100; // Device Manager specific error code
				}

				return Task.FromResult(new ResultReadBytes(errorCode, Memory<byte>.Empty));
			}

			var (lenOk, len) = RequestedReadLengthIsLessOrEqualToResponseDataLength(
				behavior.ResponseData,
				readLength
			);

			if (!lenOk)
			{
				AdsErrorCode errorCode;
				unchecked
				{
					errorCode = (AdsErrorCode)0xECA60107; // Device Manager specific error code
				}

				return Task.FromResult(new ResultReadBytes(errorCode, Memory<byte>.Empty));
			}

			if (behavior.onCall != null)
			{
				var responseData = behavior.onCall.Invoke(indexGroup, indexOffset);

				if (responseData.Length > 0)
				{
					return Task.FromResult(
						new ResultReadBytes(behavior.ErrorCode, responseData.Slice(0, len))
					);
				}

				return Task.FromResult(
					new ResultReadBytes(behavior.ErrorCode, behavior.ResponseData.Slice(0, len))
				);
			}

			return Task.FromResult(
				new ResultReadBytes(behavior.ErrorCode, behavior.ResponseData.Slice(0, len))
			);
		}

		protected override Task<ResultWrite> OnWriteAsync(
			AmsAddress target,
			uint invokeId,
			uint indexGroup,
			uint indexOffset,
			ReadOnlyMemory<byte> writeData,
			CancellationToken cancel
		)
		{
			var behavior = _behaviorManager?.GetBehaviorOfType<WriteIndicationBehavior>(b =>
				b.IndexGroup == indexGroup && b.IndexOffset == indexOffset
			);

			if (behavior is null)
			{
				AdsErrorCode errorCode;
				unchecked
				{
					errorCode = (AdsErrorCode)0xECA60100; // Device Manager specific error code
				}

				return Task.FromResult(new ResultWrite(errorCode, invokeId));
			}

			if (behavior.onCall != null)
			{
				behavior.onCall.Invoke(indexGroup, indexOffset, writeData);
			}

			var errCode =
				behavior.ExpectedLength >= writeData.Length
					? behavior.ErrorCode
					: AdsErrorCode.DeviceInvalidSize;
			return Task.FromResult(new ResultWrite(errCode, invokeId));
		}
	}
}
