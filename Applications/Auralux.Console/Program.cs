//
//  Program.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2021 Mike Becker's Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace Auralux.Console
{
	class MainClass
	{
		public const int UDP_PORT = 52200;

		public static void Main(string[] args)
		{
			System.Net.Sockets.UdpClient udp = new System.Net.Sockets.UdpClient(UDP_PORT);

			byte[] data = null;
			if (args.Length > 0)
			{
				switch (args[0].ToLower())
				{
					case "play":
					{
						data = new byte[]
						{
							(byte)'A', (byte)'l', (byte)'u', (byte)'X',
							0x11, 0x00
						};
						break;
					}
					case "stop":
					{
						data = new byte[]
						{
							(byte)'A', (byte)'l', (byte)'u', (byte)'X',
							0x12, 0x00
						};
						break;
					}
					case "pause":
					{
						data = new byte[]
						{
							(byte)'A', (byte)'l', (byte)'u', (byte)'X',
							0x13, 0x00
						};
						break;
					}
				}
			}

			if (data != null)
			{
				udp.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Broadcast, UDP_PORT));
				udp.Send(data, data.Length);
			}
		}
	}
}
