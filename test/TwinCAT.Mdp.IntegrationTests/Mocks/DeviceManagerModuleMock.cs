using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Mdp.Abstractions;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.IntegrationTests.Mocks
{
	internal class DeviceManagerModuleMock
	{
		public ushort ID { get; set; }
		public ModuleType Type { get; set; }

		public IEnumerable<MdpAddressMock> MdpAddresses => _mdpAddresses;

		private List<MdpAddressMock> _mdpAddresses;

		public DeviceManagerModuleMock(ushort id, ModuleType type)
		{
			ID = id;
			Type = type;
			_mdpAddresses = new List<MdpAddressMock>();
		}

		public DeviceManagerModuleMock RegisterMdpAddress<T>(
			byte tableID,
			byte subIndex,
			Memory<byte> data,
			bool isReadOnly = true
		)
		{
			var expectedSize = 0;

			if (typeof(T).Equals(typeof(string)))
			{
				expectedSize = 255;
			}
			else
			{
				expectedSize = Marshal.SizeOf(typeof(T));
			}

			var address = new MdpAddressMock
			{
				Area = MdpArea.Config,
				ModuleID = ID,
				Flag = 0,
				TableID = tableID,
				SubIndex = subIndex,
				IsReadOnly = isReadOnly,
				Value = data,
				ExpectedSize = expectedSize
			};

			_mdpAddresses.Add(address);

			return this;
		}
	}
}
