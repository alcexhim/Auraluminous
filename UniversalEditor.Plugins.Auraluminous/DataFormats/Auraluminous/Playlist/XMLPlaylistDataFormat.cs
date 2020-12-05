using System;
using System.Collections.Generic;
using MBS.Framework;
using MBS.Framework.Collections.Generic;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor.ObjectModels.Auraluminous.Playlist;
using UniversalEditor.ObjectModels.Markup;

namespace UniversalEditor.DataFormats.Auraluminous.Playlist
{
	public class XMLPlaylistDataFormat : XMLDataFormat
	{
		protected override void BeforeLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeLoadInternal(objectModels);
			objectModels.Push(new MarkupObjectModel());

		}
		protected override void AfterLoadInternal(Stack<ObjectModel> objectModels)
		{
			MarkupObjectModel mom = (objectModels.Pop() as MarkupObjectModel);
			AuraluxPlaylistObjectModel playlist = (objectModels.Pop() as AuraluxPlaylistObjectModel);

			MarkupTagElement tagAuraluxPlaylist = mom.Elements["AuraluxPlaylist"] as MarkupTagElement;
			MarkupTagElement tagScripts = tagAuraluxPlaylist.Elements["Scripts"] as MarkupTagElement;

			MarkupTagElement[] elements = tagScripts.Elements.OfType<MarkupTagElement>();
			foreach (MarkupTagElement elScript in elements)
			{
				if (elScript.Name == "Script")
				{
					AuraluxPlaylistEntry entry = new AuraluxPlaylistEntry();
					entry.FileName = elScript.Attributes["FileName"]?.Value;

					MarkupTagElement tagStartTime = elScript.Elements["StartTime"] as MarkupTagElement;
					if (tagStartTime != null)
					{
						entry.StartTime = AuraluxPlaylistEntryTimestampFromTag(tagStartTime);
					}
					playlist.Entries.Add(entry);
				}
			}

			base.AfterLoadInternal(objectModels);
		}

		private AuraluxPlaylistEntryTimestamp AuraluxPlaylistEntryTimestampFromTag(MarkupTagElement tag)
		{
			AuraluxPlaylistEntryTimestamp value = new AuraluxPlaylistEntryTimestamp();

			MarkupAttribute attFrames = tag.Attributes["Frames"];
			value.Samples = (attFrames?.Value?.TryParse<int>()).GetValueOrDefault(0);

			MarkupAttribute attDays = tag.Attributes["Days"];
			value.Days = (attDays?.Value?.TryParse<int>()).GetValueOrDefault(0);

			MarkupAttribute attHours = tag.Attributes["Hours"];
			value.Hours = (attHours?.Value?.TryParse<int>()).GetValueOrDefault(0);

			MarkupAttribute attMinutes = tag.Attributes["Minutes"];
			value.Minutes = (attMinutes?.Value?.TryParse<int>()).GetValueOrDefault(0);

			MarkupAttribute attSeconds = tag.Attributes["Seconds"];
			value.Seconds = (attSeconds?.Value?.TryParse<int>()).GetValueOrDefault(0);
			return value;
		}

		protected override void BeforeSaveInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeSaveInternal(objectModels);
		}
	}
}
