using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Tests
{
	public partial class MdpClientTests
	{
		// NOTE: Device Manager does not throw an exception on invalid write access!

		//[TestMethod]
		//public void TestWriteParameterReadOnly()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	ushort tcMajorVersion = 4;
		//
		//	Assert.ThrowsException<ArgumentException>(
		//		() => client.WriteParameter(ModuleType.TwinCAT, 1, 1, tcMajorVersion)
		//	);
		//}
		//
		//[TestMethod]
		//public async Task TestWriteParameterReadOnlyAsync()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	ushort tcMajorVersion = 4;
		//
		//	await Assert.ThrowsExceptionAsync<ArgumentException>(
		//		async () => await client.WriteParameterAsync(ModuleType.TwinCAT, 1, 1, tcMajorVersion, CancellationToken.None)
		//	);
		//}

		[TestMethod]
		public void TestWriteParameterBool()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var enableDhcp = true;

			client.WriteParameter(ModuleType.NIC, 1, 4, enableDhcp);

			var dhcp = client.ReadParameter<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, dhcp);
		}


		[TestMethod]
		public async Task TestWriteParameterBoolAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			var enableDhcp = true;

			await client.WriteParameterAsync(ModuleType.NIC, 1, 4, enableDhcp, CancellationToken.None);

			var dhcp = await client.ReadParameterAsync<bool>(ModuleType.NIC, 1, 4, CancellationToken.None);

			Assert.AreEqual(true, dhcp);
		}

		[TestMethod]
		public void TestWriteParameterInt()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			int sntpRefreshSeconds = 16;

			client.WriteParameter(ModuleType.Time, 1, 2, sntpRefreshSeconds);

			var seconds = client.ReadParameter<int>(ModuleType.Time, 1, 2);

			Assert.AreEqual(16, seconds);
		}


		[TestMethod]
		public async Task TestWriteParameterIntAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			int sntpRefreshSeconds = 16;

			await client.WriteParameterAsync(ModuleType.Time, 1, 2, sntpRefreshSeconds, CancellationToken.None);

			var seconds = await client.ReadParameterAsync<int>(ModuleType.Time, 1, 2, CancellationToken.None);

			Assert.AreEqual(16, seconds);
		}

		[TestMethod]
		public void TestWriteParameterString()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			string sntpServer = "172.20.10.100";

			client.WriteParameter(ModuleType.Time, 1, 1, sntpServer);

			var server = client.ReadParameter<string>(ModuleType.Time, 1, 1);

			Assert.AreEqual("172.20.10.100", server);
		}

		[TestMethod]
		public async Task TestWriteParameterStringAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			string sntpServer = "172.20.10.101";

			await client.WriteParameterAsync (ModuleType.Time, 1, 1, sntpServer, CancellationToken.None);

			var server = await client.ReadParameterAsync<string>(ModuleType.Time, 1, 1, CancellationToken.None);

			Assert.AreEqual("172.20.10.101", server);
		}

		[TestMethod]
		public void TestWriteParameterInvalidType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			byte value = 0;

			Assert.ThrowsException<ArgumentException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, value)
			);
		}

		[TestMethod]
		public async Task TestWriteParameterInvalidTypeAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			byte value = 0;

			await Assert.ThrowsExceptionAsync<ArgumentException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, value, CancellationToken.None)
			);
		}

		//[TestMethod]
		//public void TestWriteParameterWrongType()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	bool value = false;
		//
		//	Assert.ThrowsException<AdsErrorException>(
		//		() => client.WriteParameter(ModuleType.CPU, 1, 1, value)
		//	);
		//}
		//
		//[TestMethod]
		//public async Task TestWriteParameterWrongTypeAsync()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	bool value = false;
		//
		//	await Assert.ThrowsExceptionAsync<AdsErrorException>(
		//		async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, value, CancellationToken.None)
		//	);
		//}

		//[TestMethod]
		//public void TestWriteParameterWrongTableID()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	Assert.ThrowsException<AdsErrorException>(
		//		() => client.WriteParameter(ModuleType.CPU, 222, 1, 200)
		//	);
		//}
		//
		//[TestMethod]
		//public async Task TestWriteParameterWrongTableIDAsync()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	await Assert.ThrowsExceptionAsync<AdsErrorException>(
		//		async () => await client.WriteParameterAsync(ModuleType.CPU, 222, 1, 200, CancellationToken.None)
		//	);
		//}

		//[TestMethod]
		//public void TestWriteParameterWrongSubIndex()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	Assert.ThrowsException<AdsErrorException>(
		//		() => client.WriteParameter(ModuleType.CPU, 1, 222, 200)
		//	);
		//}
		//
		//[TestMethod]
		//public async Task TestWriteParameterWrongSubIndexAsync()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	await Assert.ThrowsExceptionAsync<AdsErrorException>(
		//		async () => await client.WriteParameterAsync (ModuleType.CPU, 1, 222, 200, CancellationToken.None)
		//	);
		//}
		
		[TestMethod]
		public void TestWriteParameterWrongModuleIndex()
		{
			MdpClient client = new MdpClient();
		
			client.Connect(Target);
		
			Assert.ThrowsException<IndexOutOfRangeException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, 200, 2)
			);
		}
		
		[TestMethod]
		public async Task TestWriteParameterWrongModuleIndexAsync()
		{
			MdpClient client = new MdpClient();
		
			client.Connect(Target);
		
			await Assert.ThrowsExceptionAsync<IndexOutOfRangeException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, 200, CancellationToken.None, 2)
			);
		}
		
		[TestMethod]
		public void TestWriteParameterNullModuleIndex()
		{
			MdpClient client = new MdpClient();
		
			client.Connect(Target);
		
			Assert.ThrowsException<IndexOutOfRangeException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, 200, 0)
			);
		}
		
		[TestMethod]
		public async Task TestWriteParameterNullModuleIndexAsync()
		{
			MdpClient client = new MdpClient();
		
			client.Connect(Target);
		
			await Assert.ThrowsExceptionAsync<IndexOutOfRangeException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, 200, CancellationToken.None, 0)
			);
		}
		
		[TestMethod]
		public void TestWriteParameterMissingModule()
		{
			MdpClient client = new MdpClient();
		
			client.Connect(Target);
		
			Assert.ThrowsException<ArgumentOutOfRangeException>(
				() => client.WriteParameter(ModuleType.Raid, 1, 1, false)
			);
		}
		
		[TestMethod]
		public async Task TestWriteParameterMissingModuleAsync()
		{
			MdpClient client = new MdpClient();
		
			client.Connect(Target);
		
			await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
				async () => await client.WriteParameterAsync (ModuleType.Raid, 1, 1, false, CancellationToken.None)
			);
		}
	}
}
