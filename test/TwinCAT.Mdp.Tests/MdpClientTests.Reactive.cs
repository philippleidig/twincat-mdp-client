using Microsoft.Reactive.Testing;
using System.Reactive.Linq;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.Mdp.Reactive;

namespace TwinCAT.Mdp.Tests
{
	public partial class MdpClientTests
	{
		//[TestMethod]
		//public void TestPollParameter()
		//{
		//	TestScheduler testScheduler = new TestScheduler();
		//	MdpClient client = new MdpClient();
		//
		//	var expected = new List<int> { 2496, 2496, 2496 };
		//	var actual = new List<int>();
		//
		//	client.Connect(Target, Port);
		//
		//	testScheduler.Start();
		//
		//	client
		//		.PollParameter(ModuleType.CPU, 1, 1, typeof(int), TimeSpan.FromSeconds(1))
		//		.ObserveOn(testScheduler)
		//		.Subscribe(i => actual.Add((int)i));
		//
		//	testScheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);
		//
		//	Assert.AreEqual(expected.Count, actual.Count);
		//}
		//
		//[TestMethod]
		//public void TestWhenValueChanged()
		//{
		//	TestScheduler testScheduler = new TestScheduler();
		//	MdpClient client = new MdpClient();
		//
		//	client.Connect(Target, Port);
		//
		//	var expected = new List<int>{ 2496, 2496, 2496 };
		//	var actual = new List<int>();
		//
		//	var obserable = client
		//		.WhenValueChanged(ModuleType.CPU, 1, 1, typeof(int))
		//		.ObserveOn(testScheduler)
		//		.Subscribe(i => actual.Add((int)i));
		//
		//	testScheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);
		//
		//	Assert.AreEqual(expected.Count, actual.Count);
		//
		//	obserable.Dispose();
		//}
	}
}
