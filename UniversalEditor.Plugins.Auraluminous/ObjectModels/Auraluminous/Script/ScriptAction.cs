using System;
using MBS.Audio;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
	public abstract class ScriptAction
	{
		public class ScriptActionCollection
			: System.Collections.ObjectModel.Collection<ScriptAction>
		{

		}

		public BarBeatTick StartTime { get; set; } = BarBeatTick.Empty;

		public ScriptAction()
		{
		}
	}
}
