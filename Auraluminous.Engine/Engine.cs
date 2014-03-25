using System;
using System.Collections.Generic;
using System.Text;
using UniversalEditor.DataFormats.Auraluminous.Script;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Lighting.Fixture;
using UniversalEditor.ObjectModels.Multimedia.Audio.Waveform;

namespace Auraluminous
{
    public class Engine
    {
        private List<FixtureObjectModel> mvarFixtures = new List<FixtureObjectModel>();
        public List<FixtureObjectModel> Fixtures { get { return mvarFixtures; } }

        private ScriptObjectModel mvarScript = null;
        public ScriptObjectModel Script { get { return mvarScript; } }

        private WaveformAudioObjectModel mvarAudio = null;
        public WaveformAudioObjectModel Audio { get { return mvarAudio; } set { mvarAudio = value; } }

        private Surodoine.AudioPlayer mvarAudioPlayer = new Surodoine.AudioPlayer();
        public Surodoine.AudioPlayer AudioPlayer { get { return mvarAudioPlayer; } }

        private XMLScriptDataFormat apdf = new XMLScriptDataFormat();

        private LightingEngine lighting = new LightingEngine();

        private Enttec.OpenDMX.Interface mvarOpenDMXInterface = null;
        public Enttec.OpenDMX.Interface OpenDMXInterface { get { return mvarOpenDMXInterface; } set { mvarOpenDMXInterface = value; } }

        public void Load(string FileName)
        {
            apdf.Fixtures.Clear();
            foreach (FixtureObjectModel fixture in mvarFixtures)
            {
                apdf.Fixtures.Add(fixture.ID, fixture);
            }

            mvarScript = new ScriptObjectModel();
            
            UniversalEditor.Accessors.File.FileAccessor.Load(FileName, mvarScript, apdf, true);

            mvarAudio = UniversalEditor.Common.Reflection.GetAvailableObjectModel<WaveformAudioObjectModel>(mvarScript.AudioFileName);
        }
        public void Play()
        {
            lighting.OpenDMXInterface = mvarOpenDMXInterface;
            lighting.AudioPlayer = mvarAudioPlayer;
            lighting.Script = mvarScript;
            mvarScript.Frames.Reset();

            if (mvarAudio != null)
            {
                Surodoine.AudioEngine.Initialize();
                // mvarAudioPlayer.InputDevice = Surodoine.AudioDevice.Devices[7];
                // mvarAudioPlayer.OutputDevice = Surodoine.AudioDevice.Devices[9];

                mvarAudioPlayer.InputDevice = Surodoine.AudioDevice.DefaultInput;
                mvarAudioPlayer.OutputDevice = Surodoine.AudioDevice.DefaultOutput;

                if (mvarAudioPlayer.IsPlaying) mvarAudioPlayer.Stop();
                mvarAudioPlayer.Play(mvarAudio, true);
            }

            lighting.Start();
        }
        public void Stop()
        {
            mvarAudioPlayer.Stop();
            lighting.Stop();
            Console.Clear();

            try
            {
                mvarOpenDMXInterface.Reset();
            }
            catch
            {
            }
        }

        public void Pause()
        {
            mvarAudioPlayer.Pause();

            if (mvarAudioPlayer.State == Surodoine.AudioPlayerState.Paused)
            {
                lighting.Stop();
            }
            else
            {
                lighting.Start();
            }
        }
    }
}
