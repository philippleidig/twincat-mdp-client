using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.IntegrationTests
{
	public partial class MdpClientTests
	{
		// ReadParameter with Type parameter (non-generic)
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnValue_When_ReadingParameterWithTypeParameter()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var result = client.ReadParameter(ModuleType.CPU, 1, 1, typeof(int));

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(int));
			Assert.IsTrue((int)result > 0);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnValue_When_ReadingParameterAsyncWithTypeParameter()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var result = await client.ReadParameterAsync(ModuleType.CPU, 1, 1, typeof(int));

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(int));
			Assert.IsTrue((int)result > 0);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnStringValue_When_ReadingParameterWithTypeParameter()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var result = client.ReadParameter(ModuleType.NIC, 0, 3, typeof(string));

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(string));
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnBoolValue_When_ReadingParameterWithTypeParameter()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			var result = client.ReadParameter(ModuleType.NIC, 1, 4, typeof(bool));

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(bool));
		}

		// Connect overloads
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ConnectSuccessfully_When_ConnectingWithAmsNetIdOnly()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target);

			Assert.IsTrue(client.IsConnected);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ConnectSuccessfully_When_ConnectingAsyncWithAmsNetIdOnly()
		{
			MdpClient client = new MdpClient();

			await client.ConnectAsync(Target);

			Assert.IsTrue(client.IsConnected);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ConnectToLocalSystem_When_ConnectingWithoutParameters()
		{
			MdpClient client = new MdpClient();

			client.Connect();

			Assert.IsTrue(client.IsConnected);
			Assert.IsTrue(client.IsLocal);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ConnectToLocalSystem_When_ConnectingAsyncWithoutParameters()
		{
			MdpClient client = new MdpClient();

			await client.ConnectAsync();

			Assert.IsTrue(client.IsConnected);
			Assert.IsTrue(client.IsLocal);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ConnectSuccessfully_When_ConnectingWithStringAmsNetId()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target.ToString());

			Assert.IsTrue(client.IsConnected);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ConnectSuccessfully_When_ConnectingAsyncWithStringAmsNetId()
		{
			MdpClient client = new MdpClient();

			await client.ConnectAsync(Target.ToString());

			Assert.IsTrue(client.IsConnected);
		}

		// WriteParameter with different types
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteSuccessfully_When_WritingDoubleParameter()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should not throw
			client.WriteParameter(ModuleType.NIC, 1, 5, 123.456);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteSuccessfully_When_WritingDoubleParameterAsync()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should not throw
			await client.WriteParameterAsync(ModuleType.NIC, 1, 5, 123.456);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteSuccessfully_When_WritingLongParameter()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should not throw
			client.WriteParameter(ModuleType.NIC, 1, 5, 123456789L);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteSuccessfully_When_WritingLongParameterAsync()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should not throw
			await client.WriteParameterAsync(ModuleType.NIC, 1, 5, 123456789L);
		}

		// Test moduleIndex parameter
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnValue_When_ReadingParameterWithModuleIndex()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should work with default moduleIndex (1)
			var result = client.ReadParameter<bool>(ModuleType.NIC, 1, 4, 1);

			Assert.IsTrue(result == true || result == false);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnValue_When_ReadingParameterAsyncWithModuleIndex()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should work with default moduleIndex (1)
			var result = await client.ReadParameterAsync<bool>(ModuleType.NIC, 1, 4, 1);

			Assert.IsTrue(result == true || result == false);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_WriteSuccessfully_When_WritingParameterWithModuleIndex()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should not throw with moduleIndex (1)
			client.WriteParameter(ModuleType.NIC, 1, 5, false, 1);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteSuccessfully_When_WritingParameterAsyncWithModuleIndex()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			// Should not throw with moduleIndex (1)
			await client.WriteParameterAsync(ModuleType.NIC, 1, 5, false, 1);
		}

		// Test CancellationToken
		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ConnectSuccessfully_When_ConnectingAsyncWithCancellationToken()
		{
			MdpClient client = new MdpClient();
			CancellationTokenSource cts = new CancellationTokenSource();

			await client.ConnectAsync(Target, cts.Token);

			Assert.IsTrue(client.IsConnected);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_ReturnValue_When_ReadingParameterAsyncWithCancellationToken()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			CancellationTokenSource cts = new CancellationTokenSource();

			var result = await client.ReadParameterAsync<int>(
				ModuleType.CPU,
				1,
				1,
				1,
				cts.Token
			);

			Assert.IsTrue(result > 0);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public async Task Should_WriteSuccessfully_When_WritingParameterAsyncWithCancellationToken()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target, Port);

			CancellationTokenSource cts = new CancellationTokenSource();

			// Should not throw
			await client.WriteParameterAsync(ModuleType.NIC, 1, 5, false, 1, cts.Token);
		}
	}
}
