using MBS.Framework.UserInterface;
using MBS.Framework.UserInterface.Controls;
using MBS.Audio;
using System;

namespace Auraluminous.Dialogs
{
	[ContainerLayout(typeof(GoToDialog), "Auraluminous.Dialogs.GoToDialog.glade")]
	public class GoToDialog : CustomDialog
    {
		private Button cmdOK;
		private NumericTextBox txtHours;
		private NumericTextBox txtMinutes;
		private NumericTextBox txtSeconds;
		private NumericTextBox txtMilliseconds;

		private AudioTimestamp mvarTimestamp = AudioTimestamp.Empty;
        public AudioTimestamp Timestamp { get { return mvarTimestamp; } set { mvarTimestamp = value; } }

		private int mvarSampleRate = 44100;
		public int SampleRate { get { return mvarSampleRate; } set { mvarSampleRate = value; } }

		[EventHandler(nameof(cmdOK), "Click")]
		private void cmdOK_Click(object sender, EventArgs e)
		{
			mvarTimestamp = AudioTimestamp.FromHMS((int)txtHours.Value, (int)txtMinutes.Value, (int)txtSeconds.Value, (int)txtMilliseconds.Value, mvarSampleRate * 2);

			DialogResult = DialogResult.OK;
			Close();
		}
    }
}
