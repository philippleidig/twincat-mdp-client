using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.Mdp.Reactive;

namespace TwinCAT.Mdp.IntegrationTests
{
	public partial class MdpClientTests
	{
		[TestMethod]
		[TestCategory("Integration")]
		public void Should_ReturnValues_When_PollingParameter()
		{
			TestScheduler testScheduler = new TestScheduler();
			MdpClient client = new MdpClient();

			var expected = new List<int> { 2496, 2496, 2496 };
			var actual = new List<int>();

			client.Connect(Target, Port);

			testScheduler.Start();

			client
				.PollParameter(ModuleType.CPU, 1, 1, typeof(int), TimeSpan.FromSeconds(1))
				.ObserveOn(testScheduler)
				.Subscribe(i => actual.Add((int)i));

			testScheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);

			Assert.AreEqual(expected.Count, actual.Count);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void Should_NotifyOnValueChanges_When_ObservingParameter()
		{
			TestScheduler testScheduler = new TestScheduler();
			MdpClient client = new MdpClient();

			client.Connect(Target, Port);

			var expected = new List<int> { 2496, 2496, 2496 };
			var actual = new List<int>();

			var obserable = client
				.WhenValueChanged(ModuleType.CPU, 1, 1, typeof(int))
				.ObserveOn(testScheduler)
				.Subscribe(i => actual.Add((int)i));

			testScheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);

			Assert.AreEqual(expected.Count, actual.Count);

			obserable.Dispose();
		}
	}
}
