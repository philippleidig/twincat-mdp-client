using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.IntegrationTests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnTrue_When_ConnectedToLocalSystem()
		{
			MdpClient client = new MdpClient();

			client.Connect(AmsNetId.Local, Port);

			Assert.IsTrue(client.IsLocal);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnModules_When_EnumeratingConnectedModules()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var modules = client.Modules.ToList();

			Assert.IsTrue(modules.Count > 0);
			Assert.IsTrue(modules.Contains(ModuleType.NIC));
			Assert.IsTrue(modules.Contains(ModuleType.CPU));
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnSameCount_When_EnumeratingModulesMultipleTimes()
		{
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var modules1 = client.Modules.ToList();
			var modules2 = client.Modules.ToList();

			Assert.AreEqual(modules1.Count, modules2.Count);
		}
	}
}
