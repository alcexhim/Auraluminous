using System;
using System.Collections.Generic;
using System.Linq;

namespace Auraluminous.Plugins.uDMX
{
	public class uDMXDevice : Device
	{
		private global::uDMX.Interface intf = null;
		public uDMXDevice()
		{
		}

		protected override void ResetInternal()
		{
		}
		protected override void SetChannelValueInternal(int address, byte value)
		{
		}
		protected override void SetChannelValuesInternal(byte[] values)
		{
		}
	}
}
