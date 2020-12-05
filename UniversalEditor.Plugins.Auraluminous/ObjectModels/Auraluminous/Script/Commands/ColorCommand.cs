using System;
using MBS.Framework.Drawing;

namespace UniversalEditor.ObjectModels.Auraluminous.Script.Commands
{
	public class ColorCommand : Command
	{
		public Color StartColor { get; set; }
		public Color EndColor { get; set; }

		public ColorCommand(Color startColor, Color endColor)
		{
			StartColor = startColor;
			EndColor = endColor;
		}

		public override object Clone()
		{
			ColorCommand clone = new ColorCommand(StartColor, EndColor);
			return clone;
		}
	}
}
