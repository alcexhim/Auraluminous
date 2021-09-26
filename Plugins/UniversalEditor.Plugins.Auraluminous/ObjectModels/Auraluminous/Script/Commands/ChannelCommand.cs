using System;
namespace UniversalEditor.ObjectModels.Auraluminous.Script.Commands
{
	public class ChannelCommand : Command
	{
		public object ChannelObject { get; set; } = null;
		public object Value { get; set; } = 0;

		public ChannelCommand(object channelObject, object value)
		{
			ChannelObject = channelObject;
			Value = value;
		}

		public override string ToString()
		{
			return String.Format("ChannelSet ( {0}, {1} )", ChannelObject, Value);
		}

		public override object Clone()
		{
			ChannelCommand clone = new ChannelCommand(ChannelObject, Value);
			return clone;
		}
	}
}
