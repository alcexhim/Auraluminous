using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class ScriptObjectModel : ObjectModel
    {
        public override void Clear()
        {
            mvarAudioFileName = String.Empty;
        }

        public override void CopyTo(ObjectModel where)
        {
            ScriptObjectModel clone = (where as ScriptObjectModel);
            clone.AudioFileName = (mvarAudioFileName.Clone() as string);
        }

		private string mvarTitle = String.Empty;
		public string Title { get { return mvarTitle; } set { mvarTitle = value; } }

		private string mvarArtist = String.Empty;
		public string Artist { get { return mvarArtist; } set { mvarArtist = value; } }

        private string mvarAudioFileName = String.Empty;
        public string AudioFileName { get { return mvarAudioFileName; } set { mvarAudioFileName = value; } }

        private Task.TaskCollection mvarTasks = new Task.TaskCollection();
        public Task.TaskCollection Tasks { get { return mvarTasks; } }

        private Frame.FrameCollection mvarFrames = new Frame.FrameCollection();
        public Frame.FrameCollection Frames { get { return mvarFrames; } }
    }
}
