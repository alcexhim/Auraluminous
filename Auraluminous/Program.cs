using Surodoine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UniversalEditor.DataFormats.Lighting.Fixture.Auraluminous;
using UniversalEditor.ObjectModels.Lighting.Fixture;

namespace Auraluminous
{
    static class Program
    {
        private static Engine mvarEngine = new Engine();
        public static Engine Engine { get { return mvarEngine; } }

        private static MainWindow mw = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AwesomeControls.Theming.BuiltinThemes.VisualStudio2012Theme theme = new AwesomeControls.Theming.BuiltinThemes.VisualStudio2012Theme(AwesomeControls.Theming.BuiltinThemes.VisualStudio2012Theme.ColorMode.Dark);
            theme.UseAllCapsMenus = false;
            AwesomeControls.Theming.Theme.CurrentTheme = theme;

            InitializeFixtures();
            try
            {
                mvarEngine.OpenDMXInterface = new Enttec.OpenDMX.Interface(0);
            }
            catch
            {
            }

            Program.Engine.AudioPlayer.StateChanged += AudioPlayer_StateChanged;

            MonoMidi.Listener.MessageReceived += Listener_MessageReceived;
            MonoMidi.Listener.Start();
            
            mw = new MainWindow();
            Application.Run(mw);

            MonoMidi.Listener.Stop();
        }

        static void AudioPlayer_StateChanged(object sender, AudioPlayerStateChangedEventArgs e)
        {
            switch (e.State)
            {
                case AudioPlayerState.Playing:
                {
                    if (MonoMidi.Listener.OutputDevice != null)
                    {
                        MonoMidi.Listener.OutputDevice.Send(new MonoMidi.Message(MonoMidi.MessageType.ControlChange, 7, 90, 1));
                    }
                    break;
                }
                case AudioPlayerState.Stopped:
                {
                    if (MonoMidi.Listener.OutputDevice != null)
                    {
                        MonoMidi.Listener.OutputDevice.Send(new MonoMidi.Message(MonoMidi.MessageType.ControlChange, 7, 90, 0));
                    }
                    break;
                }
            }
        }
        
        private static void Listener_MessageReceived(object sender, MonoMidi.MessageReceivedEventArgs e)
        {
            float amount = (float)((float)e.Message.Parameter2 / 127);
            byte value = (byte)(amount * 255);

            switch (e.Message.Channel)
            {
                case 1:
                {
                    switch (e.Message.Parameter1)
                    {
                        case 0:
                        {
                            // off/on light
                            switch (e.Message.Parameter2)
                            {
                                case 0:
                                {
                                    Program.Engine.OpenDMXInterface.SetChannelValue(1, 10, 0);
                                    Program.Engine.OpenDMXInterface.SetChannelValue(1, 11, 0);
                                    break;
                                }
                                case 1:
                                {
                                    Program.Engine.OpenDMXInterface.SetChannelValue(1, 10, 255);
                                    Program.Engine.OpenDMXInterface.SetChannelValue(1, 11, 255);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
                case 7:
                {
                    switch (e.Message.Parameter1)
                    {
                        case 1:
                        {
                            Program.Engine.OpenDMXInterface.SetChannelValue(1, 1, value);
                            break;
                        }
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                        case 70:
                        case 71:
                        {
                            byte[] colorValues = new byte[] { 0, 10, 20, 25, 30, 36, 48 };
                            Program.Engine.OpenDMXInterface.SetChannelValue(1, 6, colorValues[e.Message.Parameter1 - 65]);
                            break;
                        }
                        case 73:
                        {
                            switch (e.Message.Parameter2)
                            {
                                case 0:
                                {
                                    for (int i = 1; i <= 5; i++)
                                    {
                                        Program.Engine.OpenDMXInterface.SetChannelValue(27, i, 0);
                                        Program.Engine.OpenDMXInterface.SetChannelValue(32, i, 0);
                                    }
                                    break;
                                }
                                case 1:
                                {
                                    for (int i = 1; i <= 5; i++)
                                    {
                                        Program.Engine.OpenDMXInterface.SetChannelValue(27, i, 255);
                                        Program.Engine.OpenDMXInterface.SetChannelValue(32, i, 255);
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                        case 89:
                        {
                            if (e.Message.Parameter2 == 1)
                            {
                                mw.ReloadFile();
                                MonoMidi.Listener.OutputDevice.Send(new MonoMidi.Message(MonoMidi.MessageType.ControlChange, 7, 89, 0));
                            }
                            break;
                        }
                        case 90:
                        {
                            mw.PlayPause();
                            break;
                        }
                        case 93:
                        {
                            Program.Engine.OpenDMXInterface.SetChannelValue(1, 3, value);
                            break;
                        }
                        case 94:
                        {
                            Program.Engine.OpenDMXInterface.SetChannelValue(1, 10, value);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        static void InitializeFixtures()
        {
            if (!System.IO.Directory.Exists("Fixtures")) return;

            XMLFixtureDataFormat afdf = new XMLFixtureDataFormat();
            string[] FixtureDefinitions = System.IO.Directory.GetFiles("Fixtures", "*.alf", System.IO.SearchOption.AllDirectories);
            foreach (string file in FixtureDefinitions)
            {
                FixtureObjectModel fixture = new FixtureObjectModel();
                UniversalEditor.Accessors.File.FileAccessor.Load(file, fixture, afdf, true);
                mvarEngine.Fixtures.Add(fixture);
            }
        }
    }
}
