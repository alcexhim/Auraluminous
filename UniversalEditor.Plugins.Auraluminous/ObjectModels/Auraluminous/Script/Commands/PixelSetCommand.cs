using System;
namespace UniversalEditor.ObjectModels.Auraluminous.Script.Commands
{
	public class PixelSetCommand : Command
	{
		public object Index { get; set; } = null;
		public object Value { get; set; } = null;

		public PixelSetCommand(object index, object value)
		{
			Index = index;
			Value = value;
		}

		public override string ToString()
		{
			return String.Format("PixelSet ( {0}, {1} )", Index, Value);
		}

		public override object Clone()
		{
			PixelSetCommand clone = new PixelSetCommand(Index, Value);
			return clone;
		}
	}
}
