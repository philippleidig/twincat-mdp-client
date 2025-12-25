using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.IntegrationTests
{
	public partial class MdpClientTests
	{
		// ReadAny/WriteAny tests with MdpAddress
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnValue_When_ReadingAnyWithMdpAddress()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1,
				Flag = 0
			};

			var result = client.ReadAny(address, typeof(int));

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(int));
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnValue_When_ReadingAnyAsyncWithMdpAddress()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1,
				Flag = 0
			};

			var result = await client.ReadAnyAsync(address, typeof(int));

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(int));
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnValue_When_ReadingAnyAsyncGenericWithMdpAddress()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1,
				Flag = 0
			};

			var result = await client.ReadAnyAsync<int>(address);

			Assert.IsTrue(result > 0);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteSuccessfully_When_WritingAnyWithMdpAddress()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 2,
				TableID = 1,
				SubIndex = 3,
				Flag = 0
			};

			// Should not throw
			client.WriteAny(address, "TestString");
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteSuccessfully_When_WritingAnyAsyncWithMdpAddress()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 2,
				TableID = 1,
				SubIndex = 3,
				Flag = 0
			};

			// Should not throw
			await client.WriteAnyAsync(address, "TestString");
		}

		// Raw byte buffer Read/Write tests
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReadBytes_When_ReadingWithByteBuffer()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1,
				Flag = 0
			};

			byte[] buffer = new byte[4];
			int bytesRead = client.Read(address, buffer);

			Assert.IsTrue(bytesRead > 0);
			Assert.IsTrue(bytesRead <= buffer.Length);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReadBytes_When_ReadingAsyncWithByteBuffer()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 1,
				TableID = 1,
				SubIndex = 1,
				Flag = 0
			};

			byte[] buffer = new byte[4];
			var result = await client.ReadAsync(address, buffer);

			Assert.IsTrue(result.ReadBytes > 0);
			Assert.IsTrue(result.ReadBytes <= buffer.Length);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteSuccessfully_When_WritingWithByteBuffer()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 2,
				TableID = 1,
				SubIndex = 3,
				Flag = 0
			};

			byte[] buffer = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"

			// Should not throw
			client.Write(address, buffer);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteSuccessfully_When_WritingAsyncWithByteBuffer()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var address = new MdpAddress
			{
				Area = MdpArea.Config,
				ModuleID = 2,
				TableID = 1,
				SubIndex = 3,
				Flag = 0
			};

			byte[] buffer = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"

			// Should not throw
			await client.WriteAsync(address, buffer);
		}
	}
}
