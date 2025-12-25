using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;
using TwinCAT.Mdp.IntegrationTests.Mocks;

namespace TwinCAT.Mdp.IntegrationTests
{
	public partial class MdpClientTests
	{
		// Real TwinCAT/BSD Device Manager
		//public static AmsNetId Target = AmsNetId.Parse("39.157.153.107.1.1");
		//public static int Port = (int)AmsPort.SystemService;

		// Device Manager Mock
		public static AmsNetId Target = AmsNetId.Parse("127.0.0.1.1.1");
		public static int Port = DeviceManagerMock.SystemServiceAdsPort;
	}
}
