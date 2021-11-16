using System;
using System.Collections.Generic;
using Auraluminous.Controls;
using Auraluminous.Dialogs;
using MBS.Audio;
using MBS.Framework;
using MBS.Framework.Drawing;
using MBS.Framework.UserInterface;
using MBS.Framework.UserInterface.Controls;
using MBS.Framework.UserInterface.Controls.ListView;
using MBS.Framework.UserInterface.Controls.Timeline;
using MBS.Framework.UserInterface.Dialogs;
using MBS.Framework.UserInterface.Drawing;
using MBS.Framework.UserInterface.Input.Keyboard;
using MBS.Framework.UserInterface.Layouts;
using UniversalEditor.Accessors;
using UniversalEditor.DataFormats.Auraluminous.Playlist;
using UniversalEditor.DataFormats.Multimedia.Playlist;
using UniversalEditor.ObjectModels.Auraluminous.Playlist;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Auraluminous.Script.Commands;
using UniversalEditor.ObjectModels.Lighting.Fixture;
using UniversalEditor.ObjectModels.Multimedia.Playlist;

namespace Auraluminous
{
	[ContainerLayout(typeof(MainWindow), "Auraluminous.MainWindow.glade")]
    public class MainWindow : MBS.Framework.UserInterface.MainWindow
    {
		private TextBox txtLyric;
		private TextBox txtSongTitle;
		private TextBox txtTime;
		private TextBox txtBBT;
		private ListViewControl tvSongList;

		private Container ctSimpleDesk;
		private Auraluminous.Controls.TimelineControl timeline;

		protected override void OnClosing(WindowClosingEventArgs e)
		{
			base.OnClosing(e);

            Program.Engine.Stop();
			Application.Instance.Stop();
        }

		public ScriptObjectModel ObjectModel { get; set; } = null;

		public Timer tmr = new Timer();

		protected override void OnCreated(EventArgs e)
		{
			base.OnCreated(e);

			tmr.Tick += tmr_Tick;
			tmr.Duration = 50;
			tmr.Enabled = true;
			Program.Engine.Transport.StateChanged += AudioPlayer_StateChanged;

			Font bigfont = Font.FromFamily(null, 32);
			txtLyric.Font = bigfont;
			txtSongTitle.Font = bigfont;
			txtTime.Font = bigfont;
			txtBBT.Font = bigfont;

			txtLyric.Text = "(lyrics will appear here)";
			txtSongTitle.Text = "(no song loaded)";
			txtTime.Text = "00:00:00.000";
			txtBBT.Text = "0 | 0 | 0";

			TimelineGroup tgFixture = new TimelineGroup();
			tgFixture.Title = "Intimidator 1";

			TimelineGroup tgFixtureChannel1 = new TimelineGroup();
			tgFixtureChannel1.Title = "Channel 1";
			tgFixtureChannel1.Height = 64;
			TimelineObject tobj1 = new TimelineObject(14, 24);
			tobj1.SetExtraData<UniversalEditor.ObjectModels.Auraluminous.Script.Command>("command", new UniversalEditor.ObjectModels.Auraluminous.Script.Commands.ColorCommand(Colors.Blue, Colors.Red));
			tgFixtureChannel1.Objects.Add(tobj1);
			tgFixture.Groups.Add(tgFixtureChannel1);
			timeline.Groups.Add(tgFixture);

			TimelineGroup tgFixture2 = new TimelineGroup();
			tgFixture2.Title = "LED Shadow";
			timeline.Groups.Add(tgFixture2);

			Program.Engine.LightingEngine.Start();

			ScriptObjectModel script = (ObjectModel as ScriptObjectModel);
			if (script != null)
			{
				Program.Engine.LightingEngine.Stop();

				Program.Engine.LightingEngine.compiledFrames = CompileScript(script);

				for (int i = 0;  i < script.Fixtures.Count; i++)
				{
					InitSimpleDeskFixtureEditor(script.Fixtures[i]);
				}
			}
		}

		private CompiledFrame.CompiledFrameCollection CompileScript(ScriptObjectModel script)
		{
			CompiledFrame.CompiledFrameCollection frames = new CompiledFrame.CompiledFrameCollection();

			// reload the script
			AudioTimestamp tsPrev = AudioTimestamp.Empty;
			BarBeatTick bbtPrev = BarBeatTick.Empty;
			CompiledFrame frame = null;
			byte[] frameData = new byte[512];

			for (int i = 0; i < script.Frames.Count; i++)
			{
				if (script.Frames[i].BarBeatTick != BarBeatTick.Empty)
				{
					if (script.Frames[i].BarBeatTick == bbtPrev)
					{
						if (frame == null)
						{
							frame = new CompiledFrame(bbtPrev, frameData);
						}

						CompileFrame(frame, script.Frames[i], ref frameData);

						frame.Data = frameData.Clone() as byte[];
					}
					else if (script.Frames[i].BarBeatTick >= bbtPrev)
					{
						bbtPrev = script.Frames[i].BarBeatTick;

						if (frame != null)
						{
							frames.Add(frame);
						}
						frame = new CompiledFrame(bbtPrev, frameData);

						CompileFrame(frame, script.Frames[i], ref frameData);

						frame.Data = frameData.Clone() as byte[];
					}
				}
				else if (script.Frames[i].Timestamp >= tsPrev)
				{
					tsPrev = script.Frames[i].Timestamp;

					if (frame != null)
					{
						frames.Add(frame);
					}
					frame = new CompiledFrame(tsPrev, frameData);

					CompileFrame(frame, script.Frames[i], ref frameData);

					frame.Data = frameData.Clone() as byte[];
				}
			}

			if (frame != null)
			{
				frames.Add(frame);
			}

			return frames;
		}

		private void CompileFrame(CompiledFrame compiledFrame, Frame frame, ref byte[] frameData)
		{
			for (int j = 0; j < frame.Fixtures.Count; j++)
			{
				int initialAddress = frame.Fixtures[j].Fixture.InitialAddress;
				for (int k = 0; k < frame.Fixtures[j].Commands.Count; k++)
				{
					if (frame.Fixtures[j].Commands[k] is ChannelCommand)
					{
						ChannelCommand cmd = frame.Fixtures[j].Commands[k] as ChannelCommand;
						int relativeAddress = 0;
						if (cmd.ChannelObject is ModeChannel)
						{
							relativeAddress = ((ModeChannel)cmd.ChannelObject).RelativeAddress;
						}
						int actualAddress = initialAddress + relativeAddress;

						if (cmd.Value is byte)
						{
							frameData[actualAddress] = (byte)cmd.Value;
						}
					}
					else if (frame.Fixtures[j].Commands[k] is PixelSetCommand)
					{
						PixelSetCommand pixel = frame.Fixtures[j].Commands[k] as PixelSetCommand;
						Color color = Color.Empty;

						if (pixel.Value is Color)
						{
							color = (Color)pixel.Value;
						}
						else if (pixel.Value is Guid)
						{
							if (frame.SequenceReference.ParameterValues[frame.Sequence.Parameters[(Guid)pixel.Value]] != null)
							{
								color = (Color)frame.SequenceReference.ParameterValues[frame.Sequence.Parameters[(Guid)pixel.Value]].Value;
							}
							else
							{
								object efaultValue = frame.Sequence.Parameters[(Guid)pixel.Value].DefaultValue;
								color = (Color)frame.Sequence.Parameters[(Guid)pixel.Value].DefaultValue;
							}
						}
						else
						{

						}

						if (pixel.Index is string && (pixel.Index as string).Equals("all"))
						{
							// HACK: this is hardcoded for a Chauvet COLORband PiX IP!!!
							for (int i = 0; i < 12; i++)
							{
								int actualAddressR = initialAddress + (3 * i);
								int actualAddressG = initialAddress + (3 * i) + 1;
								int actualAddressB = initialAddress + (3 * i) + 2;

								frameData[actualAddressR] = (byte)(color.R * 255);
								frameData[actualAddressG] = (byte)(color.G * 255);
								frameData[actualAddressB] = (byte)(color.B * 255);
							}
						}
						else if (pixel.Index is int)
						{
							int actualAddressR = initialAddress + (int)pixel.Index;
							int actualAddressG = initialAddress + (int)pixel.Index + 1;
							int actualAddressB = initialAddress + (int)pixel.Index + 2;

							frameData[actualAddressR] = (byte)(color.R * 255);
							frameData[actualAddressG] = (byte)(color.G * 255);
							frameData[actualAddressB] = (byte)(color.B * 255);
						}
					}
				}
			}
		}

		void tmr_Tick(object sender, EventArgs e)
		{
			txtTime.Text = Program.jackClient.Transport.Timestamp.ToString();
			txtBBT.Text = Program.jackClient.Transport.Timestamp.ToBBTString(" | ");

			timeline.CurrentFrame = (int)(Program.jackClient.Transport.Timestamp.TotalMilliseconds / 100);
		}


		void AudioPlayer_StateChanged(object sender, MBS.Audio.AudioPlayerStateChangedEventArgs e)
		{
			if (e.State == MBS.Audio.AudioPlayerState.Stopped && e.Reason == MBS.Audio.AudioPlayerStateChangedReason.SongEnded)
			{
				// go to the next song in the list
				int currentIndex = scripts.IndexOf(Program.CurrentScript);
				if (currentIndex != -1)
				{
					currentIndex++;
					if (currentIndex >= scripts.Count)
					{
						currentIndex = 0;

						Program.Engine.Stop();
						Program.Engine.Play(scripts[currentIndex]);
					}
					else if (currentIndex < 0)
					{
						currentIndex = scripts.Count - 1;
					}

					if (currentIndex >= 0 && currentIndex < scripts.Count)
					{
						Program.Engine.Stop();
						Program.Engine.Play(scripts[currentIndex]);
					}
				}
			}
		}


		private Container InitSimpleDeskFixtureEditor(Fixture fixture)
		{
			FixtureObjectModel fixt = fixture.FixtureObject;
			Container ct = new Container(new BoxLayout(Orientation.Vertical));

			Label lblTitle = new Label();
			lblTitle.Text = fixt.Model;
			ct.Controls.Add(lblTitle, new BoxLayout.Constraints(false, false));

			NumericTextBox txtValue = new NumericTextBox();
			ct.Controls.Add(txtValue, new BoxLayout.Constraints(true, true));

			Button cmdMute = new Button();
			cmdMute.Text = "_Mute";
			ct.Controls.Add(cmdMute, new BoxLayout.Constraints(false, false));
			return ct;
		}

		private string mvarFileName = "Projects/Please Don't Go/Please Don't Go.alu";
		private DateTime InitialTime = DateTime.Now;

        private void mnuPlaybackPlay_Click(object sender, EventArgs e)
        {
            PlayPause();
        }

        public void PlayPause()
        {
			if (Program.jackClient.Transport.State == MBS.Audio.AudioPlayerState.Playing)
			{
				Program.jackClient.Transport.Pause();

				Application.Instance.Commands["PlaybackPlay"].Enabled = true;
				Application.Instance.Commands["PlaybackPause"].Enabled = false;
				Application.Instance.Commands["PlaybackStop"].Enabled = true;
			}
			else // if (Program.jackClient.Transport.State == MBS.Audio.AudioPlayerState.Paused)
			{
				Program.jackClient.Transport.Play();

				InitialTime = DateTime.Now;
				UpdateTimes();

				// LoadFile(mvarFileName);
				if (Program.CurrentScript == null)
				{
					// MessageDialog.ShowDialog("Please open or choose a script to play.", "Error", MessageDialogButtons.OK, MessageDialogIcon.Error);
					// return;
				}
				Program.Engine.Play(Program.CurrentScript);

				Application.Instance.Commands["PlaybackPlay"].Enabled = false;
				Application.Instance.Commands["PlaybackPause"].Enabled = true;
				Application.Instance.Commands["PlaybackStop"].Enabled = true;
			}
			/*
			else
			{
				Program.Engine.Stop();

				// LoadFile(mvarFileName);

				// Program.Engine.Play();

				Application.Instance.Commands["PlaybackPlay"].Enabled = true;
				Application.Instance.Commands["PlaybackPause"].Enabled = false;
				Application.Instance.Commands["PlaybackStop"].Enabled = false;
			}
			*/
        }

		private void UpdateTimes()
		{
			TimeSpan ts = new TimeSpan();
			foreach (TreeModelRow row in tvSongList.Model.Rows)
			{
				row.RowColumns[3].Value = (InitialTime + ts).ToString();
				ts = TimeSpan.Parse(row.RowColumns[2].Value.ToString());
			}
		}

		public void ReloadFile()
        {
            mnuPlaybackStop_Click(null, EventArgs.Empty);

            txtSongTitle.Text = "Compiling...";
            (Application.Instance as UIApplication).DoEvents();

            LoadFile(mvarFileName, true);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
			if (e.Key == KeyboardKey.Home)
			{
				Program.jackClient.Transport.Seek(0);
			}
			else if (e.Key == KeyboardKey.Space)
            {
                mnuPlaybackPlay_Click(this, e);
				e.Cancel = true;
            }
            else if (e.Key == KeyboardKey.S)
            {
                mnuPlaybackStop_Click(this, e);
				e.Cancel = true;
			}
        }

		// quick HACK: replace with proper playlist when we have time
		private List<ScriptObjectModel> scripts = new List<ScriptObjectModel>();

		[EventHandler(nameof(tvSongList), "RowActivated")]
		private void tvSongList_RowActivated(object sender, ListViewRowActivatedEventArgs e)
		{
			if (e.Row != null)
			{
				ScriptObjectModel script = e.Row.GetExtraData<ScriptObjectModel>("script");
				if (script != null)
				{
					if (Program.Engine.Transport.IsPlaying)
					{
						Program.Engine.Stop();
					}
					Program.CurrentScript = script;
					UpdateScript();
				}
			}
		}

		private System.Diagnostics.Process pArdour = null;

		public void LoadFile(string fileName, bool reload = false)
        {
			string ext = System.IO.Path.GetExtension(fileName);
			if (ext == ".alp")
			{
				AuraluxPlaylistObjectModel playlist = new AuraluxPlaylistObjectModel();
				UniversalEditor.Document.Load(playlist, new XMLPlaylistDataFormat(), new FileAccessor(fileName));

				foreach (AuraluxPlaylistEntry entry in playlist.Entries)
				{
					/*
					ScriptObjectModel script = Program.Engine.Load(entry.FileName, false);

					double startTime = 0;
					if (scripts.Count > 0)
					{
						startTime = (scripts[scripts.Count - 1].Audio?.Duration).GetValueOrDefault(0);
					}
					double duration = (script.Audio?.Duration).GetValueOrDefault(0);
					*/

					TreeModelRow row = new TreeModelRow(new TreeModelRowColumn[]
					{
						new TreeModelRowColumn(tvSongList.Model.Columns[0], entry.Author),
						new TreeModelRowColumn(tvSongList.Model.Columns[1], entry.Title),
						new TreeModelRowColumn(tvSongList.Model.Columns[2], TimeSpan.FromSeconds(0).ToString()),
						new TreeModelRowColumn(tvSongList.Model.Columns[3], entry.StartTime.ToAudioTimestamp().ToString()),
						new TreeModelRowColumn(tvSongList.Model.Columns[4], TimeSpan.FromSeconds(0).ToString())
					});
					row.SetExtraData<AuraluxPlaylistEntry>("entry", entry);
					// scripts.Add(script);

					tvSongList.Model.Rows.Add(row);
					UpdateScript();
				}
			}
			else if (ext == ".alu")
			{
				mvarFileName = fileName;

				ScriptObjectModel script = Program.Engine.Load(mvarFileName, reload);
				if (script.ArdourFileName != null)
				{
					if (pArdour == null)
					{
						if (MessageDialog.ShowDialog("An Ardour session is associated with this project. Launch it now? (use --auto-launch-ardour=yes|no to override", "Launch Ardour", MessageDialogButtons.YesNo, MessageDialogIcon.Warning) == DialogResult.Yes)
						{
							pArdour = (Application.Instance as UIApplication).Launch("ardour", String.Format("\"{0}\"", script.ArdourFileName));
						}
					}
				}

				double startTime = 0;
				if (scripts.Count > 0)
				{
					startTime = (scripts[scripts.Count - 1].Audio?.Duration).GetValueOrDefault(0);
				}

				double duration = (script.Audio?.Duration).GetValueOrDefault(0);
				TreeModelRow row = new TreeModelRow(new TreeModelRowColumn[]
				{
					new TreeModelRowColumn(tvSongList.Model.Columns[0], script.Artist),
					new TreeModelRowColumn(tvSongList.Model.Columns[1], script.Title),
					new TreeModelRowColumn(tvSongList.Model.Columns[2], TimeSpan.FromSeconds(duration).ToString()),
					new TreeModelRowColumn(tvSongList.Model.Columns[3], TimeSpan.FromSeconds(startTime).ToString()),
					new TreeModelRowColumn(tvSongList.Model.Columns[4], TimeSpan.FromSeconds(startTime + duration).ToString())
				});
				row.SetExtraData<ScriptObjectModel>("script", script);
				scripts.Add(script);

				Program.CurrentScript = script;
				tvSongList.Model.Rows.Add(row);
				UpdateScript();
			}
		}

		private void UpdateScript()
		{
			txtTime.Text = "00:00:00.000";
			txtSongTitle.Text = "(no song loaded)";
			txtLyric.Text = "(lyrics will appear here)";

			if (Program.CurrentScript != null)
			{
				if (!String.IsNullOrEmpty(Program.CurrentScript.Artist))
				{
					txtSongTitle.Text = String.Format("{0} - {1}", Program.CurrentScript.Artist, Program.CurrentScript.Title);
				}
				else
				{
					txtSongTitle.Text = Program.CurrentScript.Title;
				}
				txtTime.Text = "00:00:00.000";

				timeline.Groups.Clear();
				foreach (Fixture fixt in Program.CurrentScript.Fixtures)
				{
					TimelineGroup group = new TimelineGroup();
					group.Title = fixt.Title;
					if (fixt.Mode != null)
					{
						foreach (ModeChannel ch in fixt.Mode.Channels)
						{
							TimelineGroup groupChannel = new TimelineGroup();
							groupChannel.Title = ch.Channel.Name;
							group.Groups.Add(groupChannel);
						}
					}
					timeline.Groups.Add(group);
				}
				foreach (Frame frame in Program.CurrentScript.Frames)
				{
					foreach (FrameFixture fixt in frame.Fixtures)
					{
						TimelineGroup groupFixture = timeline.Groups[Program.CurrentScript.Fixtures.IndexOf(fixt.Fixture)];
						foreach (UniversalEditor.ObjectModels.Auraluminous.Script.Command cmd in fixt.Commands)
						{
							/*
							TimelineGroup groupChannel = groupFixture.Groups[fixt.Fixture.Mode.Channels.IndexOf(fch.ChannelObject)];
							TimelineObject tobj = new TimelineObject((int)(frame.TimeSpan.TotalMilliseconds / fpms), (int)((frame.TimeSpan.TotalMilliseconds / fpms) + 1));						
							groupChannel.Objects.Add(tobj);
							*/						
						}
					}
				}

				Program.Engine.LightingEngine.compiledFrames = CompileScript(Program.CurrentScript);
			}
		}

		private int fpms = (30 * 60);

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mnuPlaybackPlay_Click(sender, e);
        }

		private void mnuPlaybackStop_Click(object sender, EventArgs e)
		{
			Program.Engine.Stop();
			Program.jackClient.Transport.Stop();

			Application.Instance.Commands["PlaybackPause"].Enabled = false;
			Application.Instance.Commands["PlaybackStop"].Enabled = false;
		}
    }
}
