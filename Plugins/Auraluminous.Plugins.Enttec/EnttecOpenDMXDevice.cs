using System;
using System.Collections.Generic;
using System.Linq;

namespace Auraluminous.Plugins.Enttec
{
	public class EnttecOpenDMXDevice : Device
	{
		private global::Enttec.OpenDMX.Interface intf = new global::Enttec.OpenDMX.Interface(0x0403, 0x6001);

		protected override void SetChannelValueInternal(int address, byte value)
		{
			// Enttec OpenDMX does not support partial data streams
			intf.SetChannelValue(1, address, value);
		}
		protected override void SetChannelValuesInternal(byte[] values)
		{
			intf.SetChannelValues(values);
		}

		protected override void ResetInternal()
		{
			intf.Reset();
		}
	}
}
