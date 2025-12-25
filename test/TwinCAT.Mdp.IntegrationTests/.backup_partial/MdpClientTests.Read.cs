using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.Mdp.IntegrationTests.Mocks;

namespace TwinCAT.Mdp.IntegrationTests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnBoolValue_When_ReadingBoolParameter()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicDhcp = client.ReadParameter<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, nicDhcp);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnBoolValue_When_ReadingBoolParameterAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicDhcp = await client.ReadParameterAsync<bool>(ModuleType.NIC, 1, 4);

			Assert.AreEqual(true, nicDhcp);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnIntValue_When_ReadingIntParameter()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var cpuFrequency = client.ReadParameter<int>(ModuleType.CPU, 1, 1);

			Assert.AreEqual(2496, cpuFrequency, 100);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnIntValue_When_ReadingIntParameterAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var cpuFrequency = await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1);

			Assert.AreEqual(2496, cpuFrequency, 100);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnStringValue_When_ReadingStringParameter()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicAdapterName = client.ReadParameter<string>(ModuleType.NIC, 0, 3);

			Assert.AreEqual("em0", nicAdapterName);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnStringValue_When_ReadingStringParameterAsync()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var nicAdapterName = await client.ReadParameterAsync<string>(ModuleType.NIC, 0, 3);

			Assert.AreEqual("em0", nicAdapterName);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowArgumentException_When_ReadingParameterWithInvalidType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<ArgumentException>(
				() => client.ReadParameter<byte>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowArgumentException_When_ReadingParameterAsyncWithInvalidType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<ArgumentException>(
				async () => await client.ReadParameterAsync<byte>(ModuleType.CPU, 1, 1)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_ReadingParameterWithWrongType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = Assert.ThrowsExactly<AdsErrorException>(
				() => client.ReadParameter(ModuleType.CPU, 1, 1, Memory<byte>.Empty)
			);

			Assert.AreEqual(0xECA60107, (uint)exception.ErrorCode);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowAdsErrorException_When_ReadingParameterAsyncWithWrongType()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => client.ReadParameter(ModuleType.CPU, 1, 1, Memory<byte>.Empty)
			);

			Assert.AreEqual(0xECA60107, (uint)exception.ErrorCode);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_ReadingParameterWithWrongTableID()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = Assert.ThrowsExactly<AdsErrorException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 222, 1)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowAdsErrorException_When_ReadingParameterAsyncWithWrongTableID()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 222, 1)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowAdsErrorException_When_ReadingParameterWithWrongSubIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = Assert.ThrowsExactly<AdsErrorException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 222)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowAdsErrorException_When_ReadingParameterAsyncWithWrongSubIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var exception = await Assert.ThrowsExactlyAsync<AdsErrorException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 222)
			);

			Assert.AreEqual(0xECA60100, (uint)exception.ErrorCode);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowIndexOutOfRangeException_When_ReadingParameterWithWrongModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<IndexOutOfRangeException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 1, 2)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowIndexOutOfRangeException_When_ReadingParameterAsyncWithWrongModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<IndexOutOfRangeException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1, 2)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowIndexOutOfRangeException_When_ReadingParameterWithNullModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<IndexOutOfRangeException>(
				() => client.ReadParameter<int>(ModuleType.CPU, 1, 1, 0)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowIndexOutOfRangeException_When_ReadingParameterAsyncWithNullModuleIndex()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<IndexOutOfRangeException>(
				async () => await client.ReadParameterAsync<int>(ModuleType.CPU, 1, 1, 0)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ThrowArgumentOutOfRangeException_When_ReadingParameterFromMissingModule()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			Assert.ThrowsExactly<ArgumentOutOfRangeException>(
				() => client.ReadParameter<bool>(ModuleType.Raid, 1, 1)
			);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ThrowArgumentOutOfRangeException_When_ReadingParameterAsyncFromMissingModule()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(
				async () => await client.ReadParameterAsync<bool>(ModuleType.Raid, 1, 1)
			);
		}
	}
}
