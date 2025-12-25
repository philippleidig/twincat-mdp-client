using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.UnitTests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Unit")]
		public void Should_NotThrow_When_DisposingMultipleTimes()
		{
			MdpClient client = new MdpClient();
			client.Dispose();
			client.Dispose(); // Should not throw

			Assert.IsTrue(client.IsDisposed);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ReturnFalse_When_CheckingIsConnectedAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.IsFalse(client.IsConnected);
			Assert.IsTrue(client.IsDisposed);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ReturnZero_When_GettingModuleCountWithoutConnection()
		{
			MdpClient client = new MdpClient();

			Assert.AreEqual(0, client.ModuleCount);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ReturnEmptyCollection_When_GettingModulesWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var modules = client.Modules;

			Assert.IsNotNull(modules);
			Assert.AreEqual(0, modules.Count());
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ReturnDisconnected_When_GettingConnectionStateWithoutConnection()
		{
			MdpClient client = new MdpClient();

			Assert.AreEqual(ConnectionState.Disconnected, client.ConnectionState);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowArgumentException_When_ConnectingWithEmptyAmsNetId()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<ArgumentException>(() => client.Connect(string.Empty));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowArgumentNullException_When_ConnectingWithNullAmsNetId()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<ArgumentNullException>(() => client.Connect((string)null!));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowFormatException_When_ConnectingWithInvalidFormat()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<FormatException>(() => client.Connect("invalid"));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowFormatException_When_ConnectingWithTooManyOctets()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<FormatException>(() => client.Connect("1.2.3.4.5.6.7"));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowFormatException_When_ConnectingWithTooFewOctets()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<FormatException>(() => client.Connect("1.2.3.4"));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowArgumentNullException_When_ConnectingAsyncWithNullAmsNetId()
		{
			MdpClient client = new MdpClient();

			await Assert.ThrowsExactlyAsync<ArgumentNullException>(
				async () => await client.ConnectAsync((string)null!)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowFormatException_When_ConnectingAsyncWithInvalidAmsNetId()
		{
			MdpClient client = new MdpClient();

			await Assert.ThrowsExactlyAsync<FormatException>(
				async () => await client.ConnectAsync("invalid")
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_NotThrow_When_DisconnectingWithoutConnection()
		{
			MdpClient client = new MdpClient();
			client.Disconnect(); // Should not throw

			Assert.IsFalse(client.IsConnected);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_DisconnectingAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(() => client.Disconnect());
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_ReadingIntParameterAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowObjectDisposedException_When_ReadingParameterAsyncAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			await Assert.ThrowsExactlyAsync<ObjectDisposedException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_WritingParameterAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, 100)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowObjectDisposedException_When_WritingParameterAsyncAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			await Assert.ThrowsExactlyAsync<ObjectDisposedException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, 100)
			);
		}
	}
}
