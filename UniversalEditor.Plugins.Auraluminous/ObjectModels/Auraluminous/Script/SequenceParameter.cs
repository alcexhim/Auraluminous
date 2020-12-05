using System;
using System.Collections.Generic;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
	public class SequenceParameter
	{
		public class SequenceParameterCollection
			: System.Collections.ObjectModel.Collection<SequenceParameter>
		{
			private Dictionary<Guid, SequenceParameter> _itemsByID = new Dictionary<Guid, SequenceParameter>();
			protected override void ClearItems()
			{
				base.ClearItems();
				_itemsByID.Clear();
			}
			protected override void InsertItem(int index, SequenceParameter item)
			{
				base.InsertItem(index, item);
				_itemsByID[item.ID] = item;
			}
			protected override void RemoveItem(int index)
			{
				_itemsByID.Remove(this[index].ID);
				base.RemoveItem(index);
			}

			public SequenceParameter this[Guid id]
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
		public string Title { get; set; } = null;

		public Type DataType { get; private set; } = null;
		public object DefaultValue { get; set; } = null;

		public SequenceParameter(Guid id, string title, Type dataType, object defaultValue = null)
		{
			ID = id;
			Title = title;
			DataType = dataType;
			DefaultValue = defaultValue;
		}

		public override string ToString()
		{
			return String.Format("{0} : {1} <{2}> ({3})", ID, Title, DataType, DefaultValue);
		}
	}
}
