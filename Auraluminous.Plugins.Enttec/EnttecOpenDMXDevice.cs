using System;
using System.Collections.Generic;
using System.Linq;

namespace Auraluminous.Plugins.Enttec
{
	public class EnttecOpenDMXDevice : Device
	{
		private global::Enttec.OpenDMX.Interface intf = new global::Enttec.OpenDMX.Interface(0);

		protected override void SetChannelValueInternal(int address, byte value)
		{
			// Enttec OpenDMX does not support partial data streams
			intf.SetChannelValue(0, address, value);
		}

		protected override void ResetInternal()
		{
		}
	}
}
