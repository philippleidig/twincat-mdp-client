using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.Mdp.Tests.Mocks;

namespace TwinCAT.Mdp.Tests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		public void TestReadParameterBool()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicDhcp = client.ReadParameter<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, nicDhcp);
		}


		[TestMethod]
		public async Task TestReadParameterBoolAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicDhcp = await client.ReadParameterAsync<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, nicDhcp);
		}

		[TestMethod]
		public void TestReadParameterInt()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var cpuFrequency = client.ReadParameter<int>(ModuleType.CPU, 1, 1);

			Assert.AreEqual(2496, cpuFrequency, 100);
		}


		[TestMethod]
		public async Task TestReadParameterIntAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var cpuFrequency = await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1);

			Assert.AreEqual(2496, cpuFrequency, 100);
		}

		[TestMethod]
		public void TestReadParameterString()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicAdapterName = client.ReadParameter<string>(ModuleType.NIC, 0, 3);

			Assert.AreEqual("em0", nicAdapterName);
		}

		[TestMethod]
		public async Task TestReadParameterStringAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicAdapterName = await client.ReadParameterAsync<string>(ModuleType.NIC, 0, 3);

			Assert.AreEqual("em0", nicAdapterName);
		}

		[TestMethod]
		public void TestReadParameterInvalidType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<ArgumentException>(
				() => client.ReadParameter<byte>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		public async Task TestReadParameterInvalidTypeAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<ArgumentException>(
				async () => await client.ReadParameterAsync<byte>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		public void TestReadParameterWrongType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = Assert.ThrowsExactly<AdsErrorException>(
				() => client.ReadParameter(ModuleType.CPU, 1, 1, Memory<byte>.Empty)
			);

			Assert.AreEqual(0xECA60107, (uint)exception.ErrorCode);
		}

		[TestMethod]
		public async Task TestReadParameterWrongTypeAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => client.ReadParameter(ModuleType.CPU, 1, 1, Memory<byte>.Empty)
			);

			Assert.AreEqual(0xECA60107, (uint)exception.ErrorCode);
		}

		[TestMethod]
		public void TestReadParameterWrongTableID()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = Assert.ThrowsExactly<AdsErrorException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 222, 1)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		public async Task TestReadParameterWrongTableIDAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 222, 1)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		public void TestReadParameterWrongSubIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = Assert.ThrowsExactly<AdsErrorException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 222)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		public async Task TestReadParameterWrongSubIndexAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 222)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		public void TestReadParameterWrongModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<IndexOutOfRangeException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 1, 2)
			);
		}

		[TestMethod]
		public async Task TestReadParameterWrongModuleIndexAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<IndexOutOfRangeException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1, 2)
			);
		}

		[TestMethod]
		public void TestReadParameterNullModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<IndexOutOfRangeException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 1, 0)
			);
		}

		[TestMethod]
		public async Task TestReadParameterNullModuleIndexAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<IndexOutOfRangeException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1, 0)
			);
		}

		[TestMethod]
		public void TestReadParameterMissingModule()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<ArgumentOutOfRangeException>(
				() => client.ReadParameter<bool>(ModuleType.Raid, 1, 1)
			);
		}

		[TestMethod]
		public async Task TestReadParameterMissingModuleAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(
				async () => await client.ReadParameterAsync<bool>(ModuleType.Raid, 1, 1)
			);
		}
	}
}
