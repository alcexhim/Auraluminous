using MBS.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Auraluminous.Script.Commands;
using UniversalEditor.ObjectModels.Lighting.Fixture;

namespace Auraluminous
{
	public class LightingEngine
	{
		private System.Threading.Thread tLightingThread = null;

		private ScriptObjectModel mvarScript = null;
		public ScriptObjectModel Script { get { return mvarScript; } set { mvarScript = value; } }

		public void Start()
		{
			if (tLightingThread != null)
			{
				tLightingThread.Abort();
				tLightingThread = null;
			}

			tLightingThread = new System.Threading.Thread(tLightingThread_ThreadStart);
			tLightingThread.Start();
		}
		public void Stop()
		{
			if (tLightingThread == null) return;
			tLightingThread.Abort();
			tLightingThread = null;
		}

		public ITransport Transport { get; set; }

		private Device mvarCurrentDevice = null;
		public Device CurrentDevice { get { return mvarCurrentDevice; } set { mvarCurrentDevice = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="LightingEngine" /> should drop frames
		/// that arrive later than <see cref="FrameDropMargin" /> ticks / milliseconds.
		/// </summary>
		/// <value><c>true</c> if drop late frames; otherwise, <c>false</c>.</value>
		public bool DropLateFrames { get; set; } = false;

		// this won't work until we figure out how to calculate TotalFrames from a BBT timecode!

		public int FrameDropMargin { get; set; } = 100;

		public CompiledFrame.CompiledFrameCollection compiledFrames = null;

		private void tLightingThread_ThreadStart()
		{
			bool resetFrames = false;

			while (true)
			{
				if (Transport != null)
				{
					AudioTimestamp elapsed = Transport.Timestamp;
					if (Transport.State == AudioPlayerState.Playing)
					{
						if (mvarScript != null)
						{
							resetFrames = true;

							// see if we have a frame available
							if (compiledFrames != null)
							{
								CompiledFrame nextFrame = compiledFrames.Pop();
								if (nextFrame != null)
								{
									if (nextFrame.BarBeatTick != BarBeatTick.Empty)
									{
										BarBeatTick bbt = elapsed.ToBBTTimeSpan();
										if (bbt >= nextFrame.BarBeatTick)
										{
											Console.ForegroundColor = ConsoleColor.Cyan;
											Console.Write("RenderFrame     ");
											Console.ForegroundColor = ConsoleColor.Yellow;
											Console.Write(nextFrame.BarBeatTick.ToString().PadRight(16, ' '));
											Console.ForegroundColor = ConsoleColor.Cyan;
											Console.Write("    ");
											Console.ForegroundColor = ConsoleColor.Green;
											Console.Write(elapsed.ToTimeSpan().ToString().PadRight(16, '0'));
											Console.WriteLine();
											Console.ForegroundColor = ConsoleColor.Gray;

											DisplayFrame(nextFrame);
										}
										else
										{
											compiledFrames.Push(nextFrame);
										}
									}
								}
								else if (compiledFrames.Remaining == 0)
								{
									System.Threading.Thread.Sleep(500);
								}
							}
							else
							{
								Frame next = mvarScript.Frames.Pop();
								if (next != null)
								{
									if (next.BarBeatTick != BarBeatTick.Empty)
									{
										if (elapsed.ToBBTTimeSpan() >= next.BarBeatTick)
										{
											if (DropLateFrames && elapsed.ToBBTTimeSpan() >= next.BarBeatTick.Add(FrameDropMargin))
											{
												continue;
											}
											Console.ForegroundColor = ConsoleColor.Cyan;
											Console.Write("RenderFrame     ");
											Console.ForegroundColor = ConsoleColor.Yellow;
											Console.Write(next.TimeSpan.ToString().PadRight(16, '0'));
											Console.ForegroundColor = ConsoleColor.Cyan;
											Console.Write("    ");
											Console.ForegroundColor = ConsoleColor.Green;
											Console.Write(elapsed.ToTimeSpan().ToString().PadRight(16, '0'));
											Console.WriteLine();
											Console.ForegroundColor = ConsoleColor.Gray;

											DisplayFrame(next);
										}
										else
										{
											mvarScript.Frames.Push(next);
										}
									}
									else
									{
										if (elapsed.ToTimeSpan() >= next.TimeSpan)
										{
											Console.ForegroundColor = ConsoleColor.Cyan;
											Console.Write("RenderFrame     ");
											Console.ForegroundColor = ConsoleColor.Yellow;
											Console.Write(next.TimeSpan.ToString().PadRight(16, '0'));
											Console.ForegroundColor = ConsoleColor.Cyan;
											Console.Write("    ");
											Console.ForegroundColor = ConsoleColor.Green;
											Console.Write(elapsed.ToTimeSpan().ToString().PadRight(16, '0'));
											Console.WriteLine();
											Console.ForegroundColor = ConsoleColor.Gray;

											DisplayFrame(next);
										}
										else
										{
											mvarScript.Frames.Push(next);
										}
									}
								}
							}
						}
					}
					else if (Transport.State == AudioPlayerState.Stopped)
					{
						// we don't care about frame-by-frame sync while we're paused, so save some CPU cycles
						System.Threading.Thread.Sleep(100);

						if (mvarScript != null && resetFrames)
						{
							mvarScript.Frames.Reset();
							if (compiledFrames != null)
							{
								compiledFrames.Reset();
							}
							resetFrames = false;
						}
					}
					else if (Transport.State == AudioPlayerState.Paused)
					{
						// we don't care about frame-by-frame sync while we're paused, so save some CPU cycles
						System.Threading.Thread.Sleep(100);
					}
				}
			}
		}

		private void DisplayFrame(CompiledFrame frame)
		{
			mvarCurrentDevice.SetChannelValues(frame.Data);
		}
		private void DisplayFrame(Frame frame)
		{
			foreach (FrameFixture fixture in frame.Fixtures)
			{
				foreach (Command cmd in fixture.Commands)
				{
					if (cmd is ChannelCommand)
					{
						ChannelCommand channel = (cmd as ChannelCommand);

						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write("ChannelSet      ");
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write(fixture.Fixture.InitialAddress.ToString().PadLeft(3, '0'));
						Console.Write("         ");

						if (channel.ChannelObject is ModeChannel)
						{
							Console.Write((channel.ChannelObject as ModeChannel).RelativeAddress.ToString().PadLeft(3, '0'));
							Console.Write("         ({0})    :    ", ((channel.ChannelObject as ModeChannel).RelativeAddress + fixture.Fixture.InitialAddress).ToString().PadLeft(3, '0'));
							Console.ForegroundColor = ConsoleColor.Green;
							Console.Write(channel.Value.ToString().PadLeft(3, '0'));
							Console.WriteLine();
							Console.ForegroundColor = ConsoleColor.Gray;

							mvarCurrentDevice.SetChannelValue(fixture.Fixture.InitialAddress, (channel.ChannelObject as ModeChannel).RelativeAddress, (byte)channel.Value);
						}
					}
					else if (cmd is PixelSetCommand)
					{
						PixelSetCommand pixel = (cmd as PixelSetCommand);
						MBS.Framework.Drawing.Color color = MBS.Framework.Drawing.Color.Empty;

						if (pixel.Value is MBS.Framework.Drawing.Color)
						{
							color = (MBS.Framework.Drawing.Color)pixel.Value;
						}
						else if (pixel.Value is Guid)
						{
							// this is a reference to a SequenceParameterValue, which is unused in this context
						}

						if (pixel.Index is string && (pixel.Index as string).Equals("all"))
						{
							// set all channels to the given value

						}
						else if (pixel.Index is int)
						{
							// set the specified channel to the given value

						}
					}
				}
			}
		}
	}
}
