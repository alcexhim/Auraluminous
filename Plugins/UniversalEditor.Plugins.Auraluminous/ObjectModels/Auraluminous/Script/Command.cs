using System;
namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
	public abstract class Command : ICloneable
	{
		public class CommandCollection : System.Collections.ObjectModel.Collection<Command>
		{
		}

		public abstract object Clone();
	}
}
