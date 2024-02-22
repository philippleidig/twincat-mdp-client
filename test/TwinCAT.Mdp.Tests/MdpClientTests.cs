using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.Mdp.Tests.Mocks;

namespace TwinCAT.Mdp.Tests
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
		public void TestConnectInvalidAmsNetId()
		{
			MdpClient client = new MdpClient();		

			Assert.ThrowsException<FormatException>(
				() => client.Connect("100.100.100.1.1")
			);
		}

		[TestMethod]
		public void TestConnectNotReachable()
		{
			MdpClient client = new MdpClient();

			var exception = Assert.ThrowsException<AdsErrorException>(
				() => client.Connect("100.100.100.100.1.1")
			);
		}

		[TestMethod]
		public void TestConnect()
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
		public async Task TestConnectAsync()
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
		public void TestDisconnect()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.IsTrue(client.IsConnected);

			client.Disconnect();

			Assert.IsFalse(client.IsConnected);
			Assert.AreEqual(ConnectionState.Disconnected, client.ConnectionState);
			Assert.AreEqual(0, client.ModuleCount);
			Assert.AreEqual(0, client.Modules.Count());
		}

		[TestMethod]
		public void TestConnectOnSystemWithoutDeviceManager()
		{
			MdpClient client = new MdpClient();
		
			Assert.ThrowsException<AdsErrorException>(
				() => client.Connect(AmsNetId.Local, (int)AmsPort.SystemService)
			);
		}

		[TestMethod]
		public void TestNotConnected()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsException<ClientNotConnectedException>(
				() => client.ReadParameter<bool>(ModuleType.NIC, 1, 4)
			);

			Assert.IsFalse(client.IsConnected);
		}

		[TestMethod]
		public void TestObjectDisposed()
		{
			MdpClient client = new MdpClient();

			client.Dispose();

			Assert.ThrowsException<ObjectDisposedException>(
				() => client.ReadParameter<bool>(ModuleType.NIC, 1, 4)
			);

			Assert.IsTrue(client.IsDisposed);
		}
	}
}
