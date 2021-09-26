using System;
using System.Collections.Generic;
using System.Text;
using MBS.Audio;
using UniversalEditor;
using UniversalEditor.Accessors;
using UniversalEditor.DataFormats.Auraluminous.Script;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Lighting.Fixture;
using UniversalEditor.ObjectModels.Multimedia.Audio.Waveform;

namespace Auraluminous
{
	public class Engine
	{
		public List<FixtureObjectModel> Fixtures { get; } = new List<FixtureObjectModel>();
		public ITransport Transport { get { return LightingEngine.Transport; } set { LightingEngine.Transport = value; } }

		public LightingEngine LightingEngine { get; } = new LightingEngine();
		public Device OpenDMXInterface { get; set; } = null;

		public ScriptObjectModel Load(string FileName, bool reload = false)
		{
			XMLScriptDataFormat apdf = new XMLScriptDataFormat();
			foreach (FixtureObjectModel fixture in Fixtures)
			{
				apdf.FixtureDefinitions.Add(fixture);
			}

			ScriptObjectModel script = new ScriptObjectModel();
			OpenDMXInterface = Device.GetDefaultDevice();
			
			Document.Load(script, apdf, new FileAccessor(FileName), true);

			if (!reload)
			{
				if (script.AudioFileName != null)
				{
					if (System.IO.File.Exists(script.AudioFileName))
					{
						if (UniversalEditor.Common.Reflection.GetAvailableObjectModel<WaveformAudioObjectModel>(script.AudioFileName, out WaveformAudioObjectModel wave))
						{
							script.Audio = wave;
						}
					}
				}
			}
			return script;
		}

		private ScriptObjectModel _script = null;
		public void Play(ScriptObjectModel script)
		{
			if (script == null)
				return;

			if (_script != null)
			{
				Stop();
				_script = null;
			}

			_script = script;

			LightingEngine.CurrentDevice = OpenDMXInterface;
			LightingEngine.Transport = Transport;
			LightingEngine.Script = script;
			script.Frames.Reset();

			if (script.Audio != null)
			{
				if (Transport.IsPlaying) Transport.Stop();
				Transport.Play();
				// AudioPlayer.Play(script.Audio, true);
			}

			LightingEngine.Start();
		}
		public void Stop()
		{
			Transport.Stop();
			LightingEngine.Stop();

			if (_script != null)
			{
				_script.Frames.Reset();
				_script = null;
			}
			Console.Clear();

			try
			{
				OpenDMXInterface.Reset();
			}
			catch
			{
			}
		}

		public void Pause()
		{
			Transport.Pause();

			if (Transport.State == MBS.Audio.AudioPlayerState.Paused)
			{
				LightingEngine.Stop();
			}
			else
			{
				LightingEngine.Start();
			}
		}
	}
}
