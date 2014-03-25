using Surodoine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalEditor.ObjectModels.Auraluminous.Script;

namespace Auraluminous
{
    class LightingEngine
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

        private AudioPlayer mvarAudioPlayer = null;
        public AudioPlayer AudioPlayer { get { return mvarAudioPlayer; } set { mvarAudioPlayer = value; } }

        private void tLightingThread_ThreadStart()
        {
            if (mvarScript == null) return;

            System.Threading.Thread.Sleep(100);

            while (true)
            {
                AudioTimestamp elapsed = mvarAudioPlayer.Timestamp;
                
                // see if we have a frame available
                Frame next = mvarScript.Frames.Pop();
                if (next != null)
                {
                    if (elapsed.ToTimeSpan() >= next.TimeSpan)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("RenderFrame    ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(next.TimeSpan.ToString().PadRight(16, '0'));
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("    ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(elapsed.ToTimeSpan().ToString().PadRight(16, '0'));
                        Console.WriteLine();

                        DisplayFrame(next);
                    }
                    else
                    {
                        mvarScript.Frames.Push(next);
                    }
                }
            }
        }

        private Enttec.OpenDMX.Interface mvarOpenDMXInterface = null;
        public Enttec.OpenDMX.Interface OpenDMXInterface { get { return mvarOpenDMXInterface; } set { mvarOpenDMXInterface = value; } }

        private void DisplayFrame(Frame frame)
        {
            try
            {
                foreach (FrameFixture fixture in frame.Fixtures)
                {
                    foreach (Channel channel in fixture.Channels)
                    {
                        mvarOpenDMXInterface.SetChannelValue(fixture.Fixture.InitialAddress, channel.ChannelObject.RelativeAddress, channel.Value);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
