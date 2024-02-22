using System.Text;
using TwinCAT.Ads;
using TwinCAT.Mdp.DataTypes;

namespace TwinCAT.Mdp.Tests.Mocks
{
	internal class DeviceManagerMock : AdsServerMock, IDisposable
	{
		public static ushort SystemServiceAdsPort = 12345;

		private const uint AdsCoEIndexGroup = 0xF302;

		private readonly Dictionary<uint, DeviceManagerModuleMock> _modules = new Dictionary<uint, DeviceManagerModuleMock>();

		private readonly Dictionary<uint, byte[]> _writeableMdpAddresses = new Dictionary<uint, byte[]>();

		public IEnumerable<ModuleType> Modules => _modules.Values.Select(m => m.Type);
		public int ModuleCount => _modules.Count;

		public DeviceManagerMock()
			: base(SystemServiceAdsPort, "")
		{
			RegisterModules();
		}

		private void RegisterModules()
		{
			// CPU
			RegisterModule(new DeviceManagerModuleMock(1, ModuleType.CPU)
									.RegisterMdpAddress<uint>(1, 1, BitConverter.GetBytes(2496)));

			// NIC
			RegisterModule(new DeviceManagerModuleMock(2, ModuleType.NIC)
									// NIC Name
									.RegisterMdpAddress<string>(0, 3, Encoding.ASCII.GetBytes("em0"))
									// NIC DHCP
									.RegisterMdpAddress<bool>(1, 4, BitConverter.GetBytes(true), false));

			// Time
			RegisterModule(new DeviceManagerModuleMock(3, ModuleType.Time)
									// SNTP Server address
									.RegisterMdpAddress<string>(1, 1, Encoding.ASCII.GetBytes("111.111.111.111"), false)
									// SNTP Refresh time in seconds
									.RegisterMdpAddress<uint>(1, 2, BitConverter.GetBytes(32), false));

			RegisterModule(new DeviceManagerModuleMock ( 4, ModuleType.Memory ));
			RegisterModule(new DeviceManagerModuleMock ( 5, ModuleType.TwinCAT ));
			RegisterModule(new DeviceManagerModuleMock ( 6, ModuleType.Firewall ));
			RegisterModule(new DeviceManagerModuleMock ( 7, ModuleType.Misc ));
			RegisterModule(new DeviceManagerModuleMock ( 8, ModuleType.DiskManagement ));
			RegisterModule(new DeviceManagerModuleMock ( 9, ModuleType.Mainboard ));
			RegisterModule(new DeviceManagerModuleMock ( 10, ModuleType.Software ));
			RegisterModule(new DeviceManagerModuleMock ( 11, ModuleType.OS ));

			// Get module count
			RegisterBehavior(
				new ReadIndicationBehavior(
					AdsCoEIndexGroup, 
					0xF0200000, 
					BitConverter.GetBytes(_modules.Count)
				)
			);
		}

		private void RegisterModule (DeviceManagerModuleMock module)
		{
			_modules.Add(module.ID, module);
			RegisterModuleInDeviceArea(module);
			RegisterModuleInConfigurationArea(module);
		}

		private void RegisterModuleInDeviceArea(DeviceManagerModuleMock module)
		{
			uint indexOffset = 0xF0200000 + (uint)_modules.Count;

			RegisterBehavior(
				new ReadIndicationBehavior(
					AdsCoEIndexGroup, 
					indexOffset, 
					BitConverter.GetBytes(ToModuleInfo(module.ID, module.Type)
					)
				)
			);
		}

		private void RegisterModuleInConfigurationArea(DeviceManagerModuleMock module)
		{
			foreach(var mdpAddress in module.MdpAddresses)
			{
				RegisterMdpAddress(mdpAddress);
			}
		}

		private void RegisterMdpAddress(MdpAddressMock address)
		{
			uint indexOffset = ToIndexOffset(address);

			RegisterBehavior(
				new ReadIndicationBehavior(
						AdsCoEIndexGroup,
						indexOffset,
						address.Value,
						AdsErrorCode.Succeeded,
						OnReadIndication
					)
			);

			if(!address.IsReadOnly)
			{
				_writeableMdpAddresses.Add(indexOffset, address.Value.ToArray());

				RegisterBehavior(
					new WriteIndicationBehavior(
							AdsCoEIndexGroup,
							indexOffset,
							address.ExpectedSize,
							AdsErrorCode.Succeeded,
							OnWriteIndication
						)
				);
			}

		}

		private ReadOnlyMemory<byte> OnReadIndication(uint indexGroup, uint indexOffset)
		{
			byte[] readData;
			_writeableMdpAddresses.TryGetValue(indexOffset, out readData);
			return readData.AsMemory();
		}

		private void OnWriteIndication (uint indexGroup, uint indexOffset, ReadOnlyMemory<byte> writeData)
		{
			if (_writeableMdpAddresses.ContainsKey(indexOffset))
			{
				writeData.CopyTo(_writeableMdpAddresses[indexOffset]);
			}
		}

		private uint ToModuleInfo(uint id, ModuleType type)
		{
			return ((uint)type << 16) | id;
		}

		private uint ToIndexOffset(MdpAddress address)
		{
			/*
			IndexOffset = MDP index und subindex.
			    32          24          16          8           0
			    |           |           |           |           |
			    |		Index - 16Bit   |flags-8Bit |SubIdx-8Bit|
			    |           |           |           |           |

			  Index:	0x AnnX
			          A:	4Bit Area Code
			          nn:	8Bit Module ID
			          X:	4Bit Table ID
			*/

			return (
				((uint)address.Area << 28)
				| (address.ModuleID << 20)
				| ((uint)address.TableID << 16)
				| ((uint)address.Flag << 8)
				| address.SubIndex
			);
		}
	}
}
