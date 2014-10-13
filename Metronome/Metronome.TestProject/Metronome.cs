using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metronome
{
	public class Metronome
	{
		private double mvarTempo = 0.0;
		public double Tempo { get { return mvarTempo; } set { mvarTempo = value; } }

		private int mvarBeatsPerMeasure = 4;
		public int BeatsPerMeasure { get { return mvarBeatsPerMeasure; } set { mvarBeatsPerMeasure = value; } }

		private TimeSpan mvarTimestamp = new TimeSpan();
		public TimeSpan Timestamp { get { return mvarTimestamp; } set { mvarTimestamp = value; } }

		private Beatstamp mvarBeatstamp = new Beatstamp(0, 0);
		public Beatstamp Beatstamp { get { return mvarBeatstamp; } set { mvarBeatstamp = value; } }

		public event TickEventHandler Tick;
		protected virtual void OnTick(TickEventArgs e)
		{
			if (Tick != null) Tick(this, e);
		}

		private System.Threading.Thread _t = null;
		private void _t_ThreadStart()
		{
			while (true)
			{
				mvarBeatstamp.Tick++;
				OnTick(new TickEventArgs());
				System.Threading.Thread.Sleep(1);
			}
		}

		public void Start()
		{
			if (_t != null) Stop();
			_t = new System.Threading.Thread(_t_ThreadStart);

			double beatsPerSecond = (mvarTempo / 60);
			double beatsPerMillisecond = (beatsPerSecond / 1000);
			double millisecondsPerBeat = (1 / beatsPerMillisecond);
			mvarBeatstamp = new Beatstamp(millisecondsPerBeat, mvarBeatsPerMeasure);

			_t.Start();
		}
		public void Stop()
		{
			if (_t == null) return;
			_t.Abort();
			_t = null;
		}
	}
	public delegate void TickEventHandler(object sender, TickEventArgs e);
	public class TickEventArgs
	{
		
	}

	public class Beatstamp
	{
		private double mvarTicksPerBeat = 0;
		private int mvarBeatsPerMeasure = 0;

		public Beatstamp(double ticksPerBeat, int beatsPerMeasure)
		{
			mvarTicksPerBeat = ticksPerBeat;
			mvarBeatsPerMeasure = beatsPerMeasure;
		}

		private int mvarMeasure = 0;
		public int Measure { get { return mvarMeasure; } set { mvarMeasure = value; } }

		private int mvarBeat = 0;
		public int Beat
		{
			get { return mvarBeat; }
			set
			{
				mvarBeat = value;
				if (mvarBeat > mvarBeatsPerMeasure)
				{
					mvarBeat = 1;
					Measure++;
				}
			}
		}

		private int mvarTick = 0;
		public int Tick
		{
			get { return mvarTick; }
			set
			{
				mvarTick = value;
				if (mvarTick > mvarTicksPerBeat)
				{
					mvarTick = 0;
					Beat++;
				}
			}
		}

		public override string ToString()
		{
			return mvarMeasure.ToString() + "|" + mvarBeat.ToString() + "|" + mvarTick.ToString().PadLeft(3, '0');
		}
	}
}
