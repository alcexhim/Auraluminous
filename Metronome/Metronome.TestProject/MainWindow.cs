using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metronome
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
			metro.Tick += metro_Tick;
		}

		private void metro_Tick(object sender, TickEventArgs e)
		{
			Invoke(new Action(delegate()
			{
				txtBeat.Text = metro.Beatstamp.ToString();
			}));
		}

		private Metronome metro = new Metronome();

		private void cmdStartStop_Click(object sender, EventArgs e)
		{
			metro.Tempo = Double.Parse(txtTempo.Text);
			metro.BeatsPerMeasure = Int32.Parse(txtBeatsPerMeasure.Text);

			metro.Start();
		}
	}
}
