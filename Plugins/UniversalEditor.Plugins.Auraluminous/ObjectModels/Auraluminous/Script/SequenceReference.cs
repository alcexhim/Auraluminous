using System;
using MBS.Audio;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
	public class SequenceReference : ScriptAction
	{
		public Sequence Sequence { get; private set; } = null;
		public int RepeatCount { get; set; } = 1;
		public SequenceParameterValue.SequenceParameterValueCollection ParameterValues { get; } = new SequenceParameterValue.SequenceParameterValueCollection();

		public SequenceReference(Sequence seq, BarBeatTick startTime, SequenceParameterValue[] parameterValues = null, int repeatCount = 1)
		{
			Sequence = seq;
			StartTime = startTime;
			if (parameterValues != null)
			{
				for (int i = 0; i < parameterValues.Length; i++)
				{
					ParameterValues.Add(parameterValues[i]);
				}
			}
			RepeatCount = repeatCount;
		}
	}
}
