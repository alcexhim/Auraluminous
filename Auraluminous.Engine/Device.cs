using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auraluminous
{
	public abstract class Device
	{
		public static Device[] GetDevices()
		{

		}
		public static Device GetDefaultDevice()
		{
			Device[] devices = GetDevices();
			if (devices.Length == 0) return null;
			return devices[0];
		}

		protected abstract void Write(byte[] data, int offset, int length);
	}
}
