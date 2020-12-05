using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class FrameFixture : ICloneable
    {
        public class FrameFixtureCollection
            : System.Collections.ObjectModel.Collection<FrameFixture>
        {
        }

		public Fixture Fixture { get; set; } = null;
		public Command.CommandCollection Commands { get; } = new Command.CommandCollection();

		public object Clone()
		{
			FrameFixture clone = new FrameFixture();
			clone.Fixture = Fixture;
			foreach (Command command in Commands)
			{
				clone.Commands.Add(command.Clone() as Command);
			}
			return clone;
		}
	}
}
