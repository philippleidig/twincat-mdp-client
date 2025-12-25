using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.Mdp.IntegrationTests.Mocks;

namespace TwinCAT.Mdp.IntegrationTests
{
	[TestClass]
	public partial class MdpClientTests
	{
		private DeviceManagerMock deviceManagerMock;

		[TestInitialize]
		public void TestInitialize()
		{
			deviceManagerMock = new DeviceManagerMock();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			deviceManagerMock.Dispose();
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_ConnectingToUnreachableTarget()
		{
			MdpClient client = new MdpClient();

			var exception = Assert.ThrowsExactly<AdsErrorException>(
				() => client.Connect("100.100.100.100.1.1")
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ConnectSuccessfully_When_ValidTargetProvided()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.IsTrue(client.IsConnected);
			Assert.AreEqual(ConnectionState.Connected, client.ConnectionState);

			Assert.AreEqual(deviceManagerMock.ModuleCount, client.ModuleCount);
			Assert.AreEqual(
				deviceManagerMock.Modules.Where(m => m == ModuleType.NIC).Count(),
				client.Modules.Where(m => m == ModuleType.NIC).Count()
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ConnectSuccessfully_When_ValidTargetProvidedAsync()
		{
			MdpClient client = new MdpClient();

			await client.ConnectAsync(Target, Port);

			Assert.IsTrue(client.IsConnected);
			Assert.AreEqual(ConnectionState.Connected, client.ConnectionState);

			Assert.AreEqual(deviceManagerMock.ModuleCount, client.ModuleCount);
			Assert.AreEqual(
				deviceManagerMock.Modules.Where(m => m == ModuleType.NIC).Count(),
				client.Modules.Where(m => m == ModuleType.NIC).Count()
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_DisconnectSuccessfully_When_AlreadyConnected()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.IsTrue(client.IsConnected);

			client.Disconnect();

			Assert.IsFalse(client.IsConnected);
			Assert.AreEqual(ConnectionState.Connected, client.ConnectionState);
			Assert.AreEqual(0, client.ModuleCount);
			Assert.AreEqual(0, client.Modules.Count());
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_ConnectingToSystemWithoutDeviceManager()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<AdsErrorException>(
				() => client.Connect(AmsNetId.Local, (int)AmsPort.SystemService)
			);
		}
	}
}
