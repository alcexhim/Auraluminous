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
			Device[] types = Common.Reflection.GetAvailableInstances<Device>();
			return types;
		}
		public static Device GetDefaultDevice()
		{
			Device[] devices = GetDevices();
			if (devices.Length == 0) return null;
			return devices[0];
		}

		protected abstract void SetChannelValueInternal(int address, byte value);

		public void SetChannelValue(int initialAddress, int relativeAddress, byte value)
		{
			SetChannelValueInternal(initialAddress + relativeAddress, value);
		}

		protected abstract void ResetInternal();
		public void Reset()
		{
			ResetInternal();
		}
	}
}
