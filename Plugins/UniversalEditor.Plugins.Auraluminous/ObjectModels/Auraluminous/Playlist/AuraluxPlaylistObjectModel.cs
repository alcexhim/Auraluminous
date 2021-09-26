using System;
namespace UniversalEditor.ObjectModels.Auraluminous.Playlist
{
	public class AuraluxPlaylistObjectModel : ObjectModel
	{
		public AuraluxPlaylistEntry.AuraluxPlaylistEntryCollection Entries { get; } = new AuraluxPlaylistEntry.AuraluxPlaylistEntryCollection();

		public override void Clear()
		{
			Entries.Clear();
		}

		public override void CopyTo(ObjectModel where)
		{
		}
	}
}
