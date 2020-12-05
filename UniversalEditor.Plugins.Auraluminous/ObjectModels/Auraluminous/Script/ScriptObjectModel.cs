using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalEditor.ObjectModels.Multimedia.Audio.Waveform;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class ScriptObjectModel : ObjectModel
    {
        public override void Clear()
        {
			AudioFileName = String.Empty;
        }

        public override void CopyTo(ObjectModel where)
        {
            ScriptObjectModel clone = (where as ScriptObjectModel);
            clone.AudioFileName = (AudioFileName.Clone() as string);
		}

		public string Title { get; set; } = String.Empty;
		public string Artist { get; set; } = String.Empty;
		public string AudioFileName { get; set; } = String.Empty;
		public string ArdourFileName { get; set; } = null;

		public WaveformAudioObjectModel Audio { get; set; } = null;

		public Task.TaskCollection Tasks { get; } = new Task.TaskCollection();
		public Frame.FrameCollection Frames { get; } = new Frame.FrameCollection();
		public Sequence.SequenceCollection Sequences { get; } = new Sequence.SequenceCollection();

		public ScriptAction.ScriptActionCollection Actions { get; } = new ScriptAction.ScriptActionCollection();

		public Fixture.FixtureCollection Fixtures { get; } = new Fixture.FixtureCollection();
	}
}
