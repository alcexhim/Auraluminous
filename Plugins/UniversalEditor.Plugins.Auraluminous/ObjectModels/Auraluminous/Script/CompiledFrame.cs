using System;
using System.Collections.Generic;
using MBS.Audio;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
	public class CompiledFrame
	{
		public class CompiledFrameCollection
			: System.Collections.ObjectModel.Collection<CompiledFrame>
		{
			private int mvarCurrentIndex = 0;

			public int Remaining { get { return Count - mvarCurrentIndex; } }

			public CompiledFrame Pop()
			{
				if (mvarCurrentIndex >= Count) return null;

				CompiledFrame frame = this[mvarCurrentIndex];
				mvarCurrentIndex++;
				return frame;
			}
			public void Push(CompiledFrame frame)
			{
				mvarCurrentIndex = IndexOf(frame);
			}

			public void Reset()
			{
				mvarCurrentIndex = 0;
			}
		}

		public CompiledFrame(BarBeatTick time, byte[] data)
		{
			BarBeatTick = time;
			Data = data.Clone() as byte[];
		}

		public BarBeatTick BarBeatTick { get; private set; } = BarBeatTick.Empty;
		public byte[] Data { get; set; } = null;
	}
}
