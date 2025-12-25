using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.IntegrationTests.Mocks
{
	internal class MdpAddressMock : MdpAddress
	{
		public int ExpectedSize { get; set; }

		public bool IsReadOnly { get; set; }
		public Memory<byte> Value { get; set; }
	}
}
