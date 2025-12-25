using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.UnitTests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowClientNotConnectedException_When_ReadingAnyWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1
			};

			Assert.ThrowsExactly<ClientNotConnectedException>(
				() => client.ReadAny(address, typeof(int))
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowClientNotConnectedException_When_ReadingAnyAsyncWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1
			};

			await Assert.ThrowsExactlyAsync<ClientNotConnectedException>(
				async () => await client.ReadAnyAsync(address, typeof(int))
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowClientNotConnectedException_When_WritingAnyWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1
			};

			Assert.ThrowsExactly<ClientNotConnectedException>(
				() => client.WriteAny(address, 123)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowClientNotConnectedException_When_WritingAnyAsyncWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1
			};

			await Assert.ThrowsExactlyAsync<ClientNotConnectedException>(
				async () => await client.WriteAnyAsync(address, 123)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowClientNotConnectedException_When_ReadingWithByteBufferWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1
			};

			byte[] buffer = new byte[4];

			Assert.ThrowsExactly<ClientNotConnectedException>(
				() => client.Read(address, buffer)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Should_ThrowClientNotConnectedException_When_WritingWithByteBufferWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1
			};

			byte[] buffer = new byte[4];

			Assert.ThrowsExactly<ClientNotConnectedException>(
				() => client.Write(address, buffer)
			);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public async Task Should_ThrowClientNotConnectedException_When_WritingAsyncWithByteBufferWithoutConnection()
		{
			MdpClient client = new MdpClient();

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1
			};

			byte[] buffer = new byte[4];

			await Assert.ThrowsExactlyAsync<ClientNotConnectedException>(
				async () => await client.WriteAsync(address, buffer)
			);
		}
	}
}
