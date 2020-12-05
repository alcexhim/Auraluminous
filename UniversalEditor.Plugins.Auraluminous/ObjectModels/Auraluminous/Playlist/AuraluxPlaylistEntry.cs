using System;
using UniversalEditor.ObjectModels.Multimedia.Playlist;

namespace UniversalEditor.ObjectModels.Auraluminous.Playlist
{
	public class AuraluxPlaylistEntry : PlaylistEntry
	{
		public class AuraluxPlaylistEntryCollection
			: System.Collections.ObjectModel.Collection<AuraluxPlaylistEntry>
		{

		}

		public AuraluxPlaylistEntry()
		{
		}

		public AuraluxPlaylistEntryTimestamp StartTime { get; set; }
	}
}
