using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Tests
{
	[TestClass]
	public class MdpClientTests
	{
		private const string Target = "39.157.153.107.1.1";

		[TestMethod]
		public void TestConnect()
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

		[TestMethod]
		public void TestReadParameterBool()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var nicDhcp = client.ReadParameter<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, nicDhcp);
		}

		[TestMethod]
		public void TestReadParameterInt()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var cpuFrequency = client.ReadParameter<int>(ModuleType.CPU, 1, 1);

			Assert.AreEqual(2496, cpuFrequency);
		}

		[TestMethod]
		public void TestReadParameterString()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var nicAdapterName = client.ReadParameter<string>(ModuleType.NIC, 0, 3);

			Assert.AreEqual("em0", nicAdapterName);
		}

		[TestMethod]
		public void TestReadParameterInvalidType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.ThrowsException<ArgumentException>(
				() => client.ReadParameter<byte>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		public void TestReadParameterWrongType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.ThrowsException<AdsErrorException>(
				() => client.ReadParameter<bool>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		public void TestReadParameterWrongTableID()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.ThrowsException<AdsErrorException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 222, 1)
			);
		}

		[TestMethod]
		public void TestReadParameterWrongSubIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.ThrowsException<AdsErrorException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 222)
			);
		}

		[TestMethod]
		public void TestReadParameterWrongModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.ThrowsException<IndexOutOfRangeException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 1, 2)
			);
		}

		[TestMethod]
		public void TestReadParameterNullModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.ThrowsException<IndexOutOfRangeException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 1, 0)
			);
		}

		[TestMethod]
		public void TestReadParameterMissingModule()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.ThrowsException<ArgumentOutOfRangeException>(
				() => client.ReadParameter<bool>(ModuleType.Raid, 1, 1)
			);
		}
	}
}
