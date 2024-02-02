using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Reactive.Tests
{
	[TestClass]
	public class MdpClientTests
	{
		private const string Target = "39.157.153.107.1.1";

		[TestMethod]
		public void TestPollParameter()
		{
			TestScheduler testScheduler = new TestScheduler();
			MdpClient client = new MdpClient();

			var expected = new List<int> { 2496, 2496, 2496 };
			var actual = new List<int>();

			client.Connect(Target);

			client
				.PollParameter(ModuleType.CPU, 1, 1, typeof(int), TimeSpan.FromSeconds(1))
				.ObserveOn(testScheduler)
				.Subscribe(i => actual.Add((int)i));

			testScheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);

			Assert.AreEqual(expected[0], actual[0]);
		}

		[TestMethod]
		public void TestWhenValueChanged()
		{
			MdpClient client = new MdpClient();
			client.Connect(Target);

			var expected = new[] { 2496, 2496, 2496 };
			var actual = new List<int>();

			var obserable = client
				.WhenValueChanged<int>(ModuleType.CPU, 1, 1)
				.Subscribe(i => actual.Add(i));

			Assert.AreEqual(expected, actual);

			obserable.Dispose();
		}
	}
}
