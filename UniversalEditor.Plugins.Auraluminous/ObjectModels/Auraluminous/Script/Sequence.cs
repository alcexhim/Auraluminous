using System;
using System.Collections.Generic;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
	public class Sequence
	{
		public class SequenceCollection
			: System.Collections.ObjectModel.Collection<Sequence>
		{
			private Dictionary<Guid, Sequence> _itemsByID = new Dictionary<Guid, Sequence>();
			protected override void ClearItems()
			{
				base.ClearItems();
				_itemsByID.Clear();
			}
			protected override void InsertItem(int index, Sequence item)
			{
				base.InsertItem(index, item);
				_itemsByID[item.ID] = item;
			}
			protected override void RemoveItem(int index)
			{
				_itemsByID.Remove(this[index].ID);
				base.RemoveItem(index);
			}

			public Sequence this[Guid id]
			{
				get
				{
					if (_itemsByID.ContainsKey(id))
						return _itemsByID[id];
					return null;
				}
			}
		}

		public Guid ID { get; private set; } = Guid.Empty;
		public string Title { get; private set; } = null;

		public SequenceParameter.SequenceParameterCollection Parameters { get; } = new SequenceParameter.SequenceParameterCollection();
		public Frame.FrameCollection Frames { get; } = new Frame.FrameCollection();

		public Sequence(Guid id, string title)
		{
			ID = id;
			Title = title;
		}

		public override string ToString()
		{
			return String.Format("{0} : {1}", ID, Title);
		}
	}
}
