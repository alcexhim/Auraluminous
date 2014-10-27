using System;
using System.Collections.Generic;
using System.Linq;

namespace Auraluminous.Plugins.uDMX
{
	public class uDMXDevice : Device
	{
		private global::uDMX.Interface intf = new global::uDMX.Interface();

		protected override void Write(byte[] data, int offset, int length)
		{
			// usb request for SetChannelRange:
			// bmRequestType - ignored by device, should be USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT
			// bRequest	- cmd_SetChannelRange (2)
			// wValue: number of channels to set (1 .. 512 - wIndex)
			// wIndex: index of first channel to set (0 .. 511)
			// wLength: length of data, must be >= wValue

		}
	}
}
