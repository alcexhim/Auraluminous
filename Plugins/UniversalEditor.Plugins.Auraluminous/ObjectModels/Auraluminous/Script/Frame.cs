using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBS.Audio;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class Frame
    {
        public class FrameCollection
            : System.Collections.ObjectModel.Collection<Frame>
        {
            private Dictionary<AudioTimestamp, Frame> framesByTimeSpan = new Dictionary<AudioTimestamp, Frame>();

            protected override void InsertItem(int index, Frame item)
            {
                base.InsertItem(index, item);
                framesByTimeSpan[item.Timestamp] = item;
            }
            protected override void RemoveItem(int index)
            {
                if (framesByTimeSpan.ContainsKey(this[index].Timestamp))
                {
                    framesByTimeSpan.Remove(this[index].Timestamp);
                }
                base.RemoveItem(index);
            }
            protected override void ClearItems()
            {
                base.ClearItems();
                framesByTimeSpan.Clear();
            }

            public Frame this[AudioTimestamp timespan]
            {
                get
                {
                    if (framesByTimeSpan.ContainsKey(timespan)) return framesByTimeSpan[timespan];
                    return null;
                }
            }

            private int mvarCurrentIndex = 0;
            public Frame Pop()
            {
                if (mvarCurrentIndex >= Count) return null;

                Frame frame = this[mvarCurrentIndex];
                mvarCurrentIndex++;
                return frame;
            }
            public void Push(Frame frame)
            {
                mvarCurrentIndex = IndexOf(frame);
            }

			public void Reset()
			{
				mvarCurrentIndex = 0;
			}
		}

		public AudioTimestamp Timestamp { get; set; } = AudioTimestamp.Empty;

        private FrameFixture.FrameFixtureCollection mvarFixtures = new FrameFixture.FrameFixtureCollection();
        public FrameFixture.FrameFixtureCollection Fixtures { get { return mvarFixtures; } }

		public BarBeatTick BarBeatTick { get; set; } = BarBeatTick.Empty;
		public Sequence Sequence { get; set; } = null; // eww
		public SequenceReference SequenceReference { get; set; } = null;

		public object Clone()
		{
			Frame clone = new Frame();
			clone.BarBeatTick = BarBeatTick;
			foreach (FrameFixture fixt in Fixtures)
			{
				clone.Fixtures.Add(fixt.Clone() as FrameFixture);
			}
			clone.Sequence = Sequence;
			clone.SequenceReference = SequenceReference;
			clone.Timestamp = Timestamp;
			return clone;
		}

		public override string ToString()
		{
			return String.Format("( {0} )", BarBeatTick != BarBeatTick.Empty ? BarBeatTick.ToString() : Timestamp.ToString());
		}
	}
}
