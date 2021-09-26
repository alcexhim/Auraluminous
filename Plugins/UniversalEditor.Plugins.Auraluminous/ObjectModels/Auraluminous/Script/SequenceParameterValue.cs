using System;
using System.Collections.Generic;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
	public class SequenceParameterValue
	{
		public class SequenceParameterValueCollection
			: System.Collections.ObjectModel.Collection<SequenceParameterValue>
		{
			private Dictionary<SequenceParameter, SequenceParameterValue> _itemsByParameter = new Dictionary<SequenceParameter, SequenceParameterValue>();
			protected override void ClearItems()
			{
				base.ClearItems();
				_itemsByParameter.Clear();
			}
			protected override void InsertItem(int index, SequenceParameterValue item)
			{
				base.InsertItem(index, item);
				_itemsByParameter[item.Parameter] = item;
			}
			protected override void RemoveItem(int index)
			{
				_itemsByParameter.Remove(this[index].Parameter);
				base.RemoveItem(index);
			}

			public SequenceParameterValue this[SequenceParameter parm]
			{
				get
				{
					if (_itemsByParameter.ContainsKey(parm))
						return _itemsByParameter[parm];
					return null;
				}
			}
		}

		public SequenceParameter Parameter { get; private set; } = null;
		public object Value { get; set; } = null;

		public SequenceParameterValue(SequenceParameter parm, object value)
		{
			Parameter = parm;
			Value = value;
		}
	}
}
