using System;
using MBS.Audio;

namespace Auraluminous
{
	public static class ExtensionMethods
	{
		public static void Reset(this UniversalEditor.ObjectModels.Auraluminous.Script.Fixture fixt)
		{
			Program.Engine.OpenDMXInterface.SetChannelValue(fixt.InitialAddress, 10, 80);
			System.Threading.Thread.Sleep(1000);
			Program.Engine.OpenDMXInterface.SetChannelValue(fixt.InitialAddress, 10, 0);
		}

		public static AudioTimestamp ToAudioTimestamp(this UniversalEditor.ObjectModels.Auraluminous.Playlist.AuraluxPlaylistEntryTimestamp timestamp)
		{
			AudioTimestamp audioTimestamp = AudioTimestamp.FromHMS(timestamp.Hours, timestamp.Minutes, timestamp.Seconds, timestamp.Milliseconds, 44000);
			return audioTimestamp;
		}
	}
}
