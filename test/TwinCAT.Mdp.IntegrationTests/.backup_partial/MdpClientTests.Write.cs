using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.IntegrationTests
{
	public partial class MdpClientTests
	{
		// NOTE: Device Manager does not throw an exception on invalid write access!

		//[TestMethod]
		[TestCategory("Integration")]
		//public void TestWriteParameterReadOnly()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	ushort tcMajorVersion = 4;
		//
		//	Assert.ThrowsExactly<ArgumentException>(
		//		() => client.WriteParameter(ModuleType.TwinCAT, 1, 1, tcMajorVersion)
		//	);
		//}
		//
		//[TestMethod]
		[TestCategory("Integration")]
		//public async Task TestWriteParameterReadOnlyAsync()
		//{
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target);
		//
		//	ushort tcMajorVersion = 4;
		//
		//	await Assert.ThrowsExactlyAsync<ArgumentException>(
		//		async () => await client.WriteParameterAsync(ModuleType.TwinCAT, 1, 1, tcMajorVersion, CancellationToken.None)
		//	);
		//}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteAndReadBoolValue_When_WritingBoolParameter()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var enableDhcp = true;
			client.WriteParameter(ModuleType.NIC, 1, 4, enableDhcp);
			var isDhcpEnabled = client.ReadParameter<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, isDhcpEnabled);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteAndReadBoolValue_When_WritingBoolParameterAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var enableDhcp = true;
			await client.WriteParameterAsync(ModuleType.NIC, 1, 4, enableDhcp);
			var isDhcpEnabled = await client.ReadParameterAsync<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, isDhcpEnabled);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteAndReadIntValue_When_WritingIntParameter()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			int sntpRefreshSeconds = 16;

			client.WriteParameter(ModuleType.Time, 1, 2, sntpRefreshSeconds);

			var seconds = client.ReadParameter<int>(ModuleType.Time, 1, 2);

			Assert.AreEqual(16, seconds);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteAndReadIntValue_When_WritingIntParameterAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			int sntpRefreshSeconds = 16;

			await client.WriteParameterAsync(ModuleType.Time, 1, 2, sntpRefreshSeconds);

			var seconds = await client.ReadParameterAsync<int>(ModuleType.Time, 1, 2);

			Assert.AreEqual(16, seconds);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteAndReadStringValue_When_WritingStringParameter()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			string sntpServer = "172.20.10.100";

			client.WriteParameter(ModuleType.Time, 1, 1, sntpServer);

			var server = client.ReadParameter<string>(ModuleType.Time, 1, 1);

			Assert.AreEqual("172.20.10.100", server.TrimEnd('\0', '1'));
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteAndReadStringValue_When_WritingStringParameterAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			string sntpServer = "172.20.10.102";

			await client.WriteParameterAsync(ModuleType.Time, 1, 1, sntpServer);

			var server = await client.ReadParameterAsync<string>(ModuleType.Time, 1, 1);

			Assert.AreEqual("172.20.10.102", server.TrimEnd('\0', '1'));
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowArgumentException_When_WritingParameterWithInvalidType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			byte value = 0;

			Assert.ThrowsExactly<ArgumentException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, value)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowArgumentException_When_WritingParameterAsyncWithInvalidType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			byte value = 0;

			await Assert.ThrowsExactlyAsync<ArgumentException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, value)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_WritingParameterWithWrongType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			bool value = false;

			Assert.ThrowsExactly<AdsErrorException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, value)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowAdsErrorException_When_WritingParameterAsyncWithWrongType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			bool value = false;

			await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, value)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_WritingParameterWithWrongTableID()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<AdsErrorException>(
				() => client.WriteParameter(ModuleType.CPU, 222, 1, 200)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowAdsErrorException_When_WritingParameterAsyncWithWrongTableID()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 222, 1, 200)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_WritingParameterWithWrongSubIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<AdsErrorException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 222, 200)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowAdsErrorException_When_WritingParameterAsyncWithWrongSubIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 222, 200)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowIndexOutOfRangeException_When_WritingParameterWithWrongModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<IndexOutOfRangeException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, 200, 2)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowIndexOutOfRangeException_When_WritingParameterAsyncWithWrongModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<IndexOutOfRangeException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, 200, 2)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowIndexOutOfRangeException_When_WritingParameterWithNullModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<IndexOutOfRangeException>(
				() => client.WriteParameter(ModuleType.CPU, 1, 1, 200, 0)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowIndexOutOfRangeException_When_WritingParameterAsyncWithNullModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<IndexOutOfRangeException>(
				async () => await client.WriteParameterAsync(ModuleType.CPU, 1, 1, 200, 0)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowArgumentOutOfRangeException_When_WritingParameterToMissingModule()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<ArgumentOutOfRangeException>(
				() => client.WriteParameter(ModuleType.Raid, 1, 1, false)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowArgumentOutOfRangeException_When_WritingParameterAsyncToMissingModule()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(
				async () => await client.WriteParameterAsync(ModuleType.Raid, 1, 1, false)
			);
		}
	}
}
