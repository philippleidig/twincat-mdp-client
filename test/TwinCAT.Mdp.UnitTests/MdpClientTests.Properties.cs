using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.UnitTests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ReturnPositiveValue_When_GettingDefaultTimeout()
		{
			MdpClient client = new MdpClient();

			// Default timeout should be positive
			Assert.IsGreaterThan(0, client.Timeout);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ReturnSetValue_When_SettingAndGettingTimeout()
		{
			MdpClient client = new MdpClient();

			client.Timeout = 5000;

			Assert.AreEqual(5000, client.Timeout);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ReturnFalse_When_GettingIsLocalWithoutConnection()
		{
			MdpClient client = new MdpClient();

			// Should not throw when not connected
			var isLocal = client.IsLocal;

			Assert.IsFalse(isLocal);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_GettingIsLocalAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(() => _ = client.IsLocal);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_GettingConnectionStateAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(() => _ = client.ConnectionState);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_GettingModulesAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(() => _ = client.Modules);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowObjectDisposedException_When_GettingModuleCountAfterDispose()
		{
			MdpClient client = new MdpClient();
			client.Dispose();

			Assert.ThrowsExactly<ObjectDisposedException>(() => _ = client.ModuleCount);
		}
	}
}
