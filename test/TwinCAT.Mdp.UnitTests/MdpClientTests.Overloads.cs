using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.UnitTests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowArgumentException_When_ReadingParameterWithInvalidType()
		{
			MdpClient client = new MdpClient();
			client.Connect(AmsNetId.Local);

			Assert.ThrowsExactly<ArgumentException>(
				() => client.ReadParameter(ModuleType.CPU, 1, 1, typeof(byte))
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowArgumentException_When_ReadingParameterAsyncWithInvalidType()
		{
			MdpClient client = new MdpClient();
			client.Connect(AmsNetId.Local);

			await Assert.ThrowsExactlyAsync<ArgumentException>(
				async () => await client.ReadParameterAsync(ModuleType.CPU, 1, 1, typeof(byte))
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowArgumentException_When_WritingParameterWithInvalidType()
		{
			MdpClient client = new MdpClient();
			client.Connect(AmsNetId.Local);

			Assert.ThrowsExactly<ArgumentException>(
				() => client.WriteParameter(ModuleType.NIC, 1, 5, (byte)123)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowArgumentException_When_WritingParameterAsyncWithInvalidType()
		{
			MdpClient client = new MdpClient();
			client.Connect(AmsNetId.Local);

			await Assert.ThrowsExactlyAsync<ArgumentException>(
				async () => await client.WriteParameterAsync(ModuleType.NIC, 1, 5, (byte)123)
			);
		}
	}
}
