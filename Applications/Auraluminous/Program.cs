using Auraluminous.Dialogs;
using MBS.Framework;
using MBS.Framework.UserInterface;
using MBS.Framework.UserInterface.Dialogs;
using MBS.Audio;
using System;

using UniversalEditor;
using UniversalEditor.Accessors;
using UniversalEditor.DataFormats.Lighting.Fixture.Auraluminous;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Lighting.Fixture;
using MBS.Audio.Jack;

namespace Auraluminous
{
	static class Program
	{
		private static Engine mvarEngine = null;
		public static Engine Engine { get { return mvarEngine; } }

		public static ScriptObjectModel CurrentScript { get; set; } = null;

		internal static JackClient jackClient = new JackClient("auralux");

		private static JackInputPort default_inL = null;
		private static JackInputPort default_inR = null;
		private static JackOutputPort default_outL = null;
		private static JackOutputPort default_outR = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application app = new UIApplication();
			Application.Instance = app;

			jackClient.Open();
			jackClient.PortRegistered += JackClient_PortRegistered;
			jackClient.Process += JackClient_Process;

			default_inL = jackClient.RegisterInput("default_in_l");
			default_inR = jackClient.RegisterInput("default_in_r");
			default_outL = jackClient.RegisterOutput("default_out_l");
			default_outR = jackClient.RegisterOutput("default_out_r");

			// default_out.Write()

			jackClient.Activate();

			app.ShortName = "auraluminous";

			app.AttachCommandEventHandler("ToolsAdjust", delegate (object sender1, EventArgs e1)
			{
				FrameEditDialog dlg = new FrameEditDialog();
				// dlg.Timestamp = txtTime.Text;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					// add the line into the script file at the current time

				}
			});
			app.AttachCommandEventHandler("ToolsReset", delegate (object sender1, EventArgs e1)
			{
				Program.Engine.OpenDMXInterface.Reset();

				if (CurrentScript != null)
				{
					foreach (Fixture fixt in CurrentScript.Fixtures)
					{
						fixt.Reset();
					}
				}
			});
			app.AttachCommandEventHandler("FileExit", delegate (object sender1, EventArgs e1)
			{
				app.Stop();
			});
			app.AttachCommandEventHandler("FileOpen", delegate (object sender1, EventArgs e1)
			{
				FileDialog ofd = new FileDialog();
				ofd.FileNameFilters.Add("All supported files", "*.alu;*.alp");
				ofd.FileNameFilters.Add("Auraluminous script", "*.alu");
				ofd.FileNameFilters.Add("Auraluminous playlist", "*.alp");
				// ofd.InitialDirectory = "Projects";
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					((app as UIApplication).Engine.LastWindow as MainWindow).LoadFile(ofd.SelectedFileName);
				}
			});
			app.AttachCommandEventHandler("FileReload", delegate (object sender1, EventArgs e1)
			{
				if (MessageDialog.ShowDialog("Reloading the file from disk will lose any changes you have made in Auraluminous Editor. Continue?", "Reload from Disk", MessageDialogButtons.YesNo, MessageDialogIcon.Warning) == DialogResult.No) return;
				((app as UIApplication).Engine.LastWindow as MainWindow).ReloadFile();
			});

			app.AttachCommandEventHandler("EditGoTo", delegate (object sender1, EventArgs e1)
			{
				GoToDialog dlg = new GoToDialog();
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					// Program.jackClient.Transport.Pause();

					System.Threading.Thread.Sleep(50);
					Program.jackClient.Transport.Timestamp = dlg.Timestamp;

					if (CurrentScript != null)
					{
						CurrentScript.Frames.Reset();
					}

					System.Threading.Thread.Sleep(50);

					// Program.jackClient.Transport.Pause();
				}
			});

			(app as UIApplication).ConfigurationFileNameFilter = "*.alxml";
			app.Initialize();

			app.BeforeShutdown += Application_BeforeShutdown;
			(app as UIApplication).Startup += Application_Startup;
			(app as UIApplication).Activated += Application_Activated;

			app.Start();

			MonoMidi.Listener.Stop();
		}

		static void JackClient_Process(object sender, JackProcessEventArgs e)
		{
			while (true)
			{
				// so, jack is weird...

				// we have to keep this thread running
				// otherwise jack deactivates our client
				// losing our connections..

				// we always have to keep writing something to the buffer

				JackTransportState state = jackClient.Transport.TransportState;

				float[] dataL = default_inL.Read(e.FrameCount);
				float[] dataR = default_inR.Read(e.FrameCount);
				for (int i = 0; i < e.FrameCount; i++)
				{
					dataL[i] += 0.2f;                 //sine wave value generation                        
					dataR[i] += 0.2f;                 //sine wave value generation                        
				}
				default_outL.Write(dataL, e.FrameCount);
				default_outR.Write(dataR, e.FrameCount);
			}
		}
		static void JackClient_PortRegistered(object sender, JackPortRegisteredEventArgs e)
		{
		}


		static void Application_BeforeShutdown(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Program.Engine.Stop();
		}


		static void Application_Activated(object sender, ApplicationActivatedEventArgs e)
		{
			MainWindow mw = new MainWindow();
			mw.Show();
		}


		static void Application_Startup(object sender, EventArgs e)
		{
			UIApplication app = (Application.Instance as UIApplication);
			mvarEngine = new Auraluminous.Engine();

			Program.Engine.OpenDMXInterface = Device.GetDefaultDevice();
			Program.Engine.OpenDMXInterface.Reset();

			InitializeFixtures();

			Program.Engine.Transport = jackClient.Transport;
			Program.Engine.Transport.StateChanged += AudioPlayer_StateChanged;

			MonoMidi.Listener.MessageReceived += Listener_MessageReceived;
			MonoMidi.Listener.Start();
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
								// mw.ReloadFile();
								MonoMidi.Listener.OutputDevice.Send(new MonoMidi.Message(MonoMidi.MessageType.ControlChange, 7, 89, 0));
							}
							break;
						}
						case 90:
						{
							// mw.PlayPause();
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
				Document.Load(fixture, afdf, new FileAccessor(file), true);
				mvarEngine.Fixtures.Add(fixture);
			}
		}
	}
}
