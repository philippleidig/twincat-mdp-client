using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.UnitTests
{
	[TestClass]
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowFormatException_When_ConnectingWithInvalidAmsNetId()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<FormatException>(() => client.Connect("100.100.100.1.1"));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowClientNotConnectedException_When_ReadingParameterWithoutConnection()
		{
			MdpClient client = new MdpClient();

			Assert.ThrowsExactly<ClientNotConnectedException>(
				() => client.ReadParameter<bool>(ModuleType.NIC, 1, 4)
			);

			Assert.IsFalse(client.IsConnected);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_ReadingParameterAfterDispose()
		{
			MdpClient client = new MdpClient();

			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(
				() => client.ReadParameter<bool>(ModuleType.NIC, 1, 4)
			);

			Assert.IsTrue(client.IsDisposed);
		}
	}
}
