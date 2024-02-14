using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Tests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		public void TestReadParameterBool()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var nicDhcp = client.ReadParameter<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, nicDhcp);
		}


		[TestMethod]
		public async Task TestReadParameterBoolAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var nicDhcp = await client.ReadParameterAsync<bool>(ModuleType.NIC, 1, 4, CancellationToken.None);

			Assert.AreEqual(true, nicDhcp);
		}

		[TestMethod]
		public void TestReadParameterInt()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var cpuFrequency = client.ReadParameter<int>(ModuleType.CPU, 1, 1);

			Assert.AreEqual(2495, cpuFrequency, 5);
		}


		[TestMethod]
		public async Task TestReadParameterIntAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var cpuFrequency = await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1, CancellationToken.None);

			Assert.AreEqual(2495, cpuFrequency, 5);
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
		public async Task TestReadParameterStringAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var nicAdapterName = await client.ReadParameterAsync<string>(ModuleType.NIC, 0, 3, CancellationToken.None);

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
		public async Task TestReadParameterInvalidTypeAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			await Assert.ThrowsExceptionAsync<ArgumentException>(
				async () => await client.ReadParameterAsync<byte>(ModuleType.CPU, 1, 1, CancellationToken.None)
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
		public async Task TestReadParameterWrongTypeAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			await Assert.ThrowsExceptionAsync<AdsErrorException>(
				async () => await client.ReadParameterAsync<bool>(ModuleType.CPU, 1, 1, CancellationToken.None)
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
		public async Task TestReadParameterWrongTableIDAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			await Assert.ThrowsExceptionAsync<AdsErrorException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 222, 1, CancellationToken.None)
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
		public async Task TestReadParameterWrongSubIndexAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			await Assert.ThrowsExceptionAsync<AdsErrorException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 222, CancellationToken.None)
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
		public async Task TestReadParameterWrongModuleIndexAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			await Assert.ThrowsExceptionAsync<IndexOutOfRangeException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1, CancellationToken.None, 2)
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
		public async Task TestReadParameterNullModuleIndexAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			await Assert.ThrowsExceptionAsync<IndexOutOfRangeException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1, CancellationToken.None, 0)
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

		[TestMethod]
		public async Task TestReadParameterMissingModuleAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
				async () => await client.ReadParameterAsync<bool>(ModuleType.Raid, 1, 1, CancellationToken.None)
			);
		}
	}
}
