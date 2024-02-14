using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Tests
{
	[TestClass]
	public partial class MdpClientTests
	{
		private const string Target = "39.157.153.107.1.1";


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

			Assert.ThrowsException<AdsErrorException>(
				() => client.Connect("100.100.100.100.1.1")
			);
		}

		[TestMethod]
		public void TestConnectAsAmsNetId()
		{
			MdpClient client = new MdpClient();

			client.Connect(new AmsNetId(Target));

			Assert.AreEqual(new AmsNetId(Target), client.Target);

			Assert.IsTrue(client.IsConnected);
			Assert.AreEqual(ConnectionState.Connected, client.ConnectionState);
			Assert.IsFalse(client.IsLocal);

			Assert.AreEqual(12, client.ModuleCount);
			Assert.AreEqual(1, client.Modules.Where(m => m == ModuleType.NIC).Count());
		}

		public void TestConnectAsString()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.AreEqual(new AmsNetId(Target), client.Target);

			Assert.IsTrue(client.IsConnected);
			Assert.AreEqual(ConnectionState.Connected, client.ConnectionState);
			Assert.IsFalse(client.IsLocal);

			Assert.AreEqual(12, client.ModuleCount);
			Assert.AreEqual(1, client.Modules.Where(m => m == ModuleType.NIC).Count());
		}

		[TestMethod]
		public void TestDisconnect()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

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

			Assert.ThrowsException<AdsErrorException>(() => client.Connect());
		}

		[TestMethod]
		public void TestNotConnected()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsException<ClientNotConnectedException>(
				() => client.ReadParameter<bool>(ModuleType.NIC, 1, 4)
			);
		}

		[TestMethod]
		public void TestObjectDisposed()
		{
			MdpClient client = new MdpClient();

			client.Dispose();

			Assert.ThrowsException<ObjectDisposedException>(
				() => client.ReadParameter<bool>(ModuleType.NIC, 1, 4)
			);
		}
	}
}
