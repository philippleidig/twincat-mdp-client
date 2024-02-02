using System;
using System.Collections.Generic;
using System.Text;
using TwinCAT.Mdp.DataTypes;
using TwinCAT.TypeSystem;

namespace TwinCAT.Mdp.Reactive
{
	public class ParameterChangedEventArgs : EventArgs
	{
		private readonly MdpAddress _mdpAddress;
		private readonly DateTime _dateTime;

		public MdpAddress MdpAddress => _mdpAddress;
		public DateTimeOffset DateTime => _dateTime;

		public ParameterChangedEventArgs(MdpAddress address, DateTime timeStamp)
		{
			_mdpAddress = address;
			_dateTime = timeStamp;
		}
	}
}
