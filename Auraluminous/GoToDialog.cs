using Surodoine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auraluminous
{
    internal partial class GoToDialogBase : Form
    {
        public GoToDialogBase()
        {
            InitializeComponent();
        }

        private AudioTimestamp mvarTimestamp = AudioTimestamp.Empty;
        public AudioTimestamp Timestamp { get { return mvarTimestamp; } set { mvarTimestamp = value; } }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private int mvarSampleRate = 44100;
        public int SampleRate { get { return mvarSampleRate; } set { mvarSampleRate = value; } }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            int h = 0, m = 0, s = 0, ms = 0;
            if (!Int32.TryParse(txtHours.Text, out h) || !Int32.TryParse(txtMinutes.Text, out m) || !Int32.TryParse(txtSeconds.Text, out s) || !Int32.TryParse(txtMilliseconds.Text, out ms))
            {
                MessageBox.Show("All parameters must be numeric.  Please check that you have entered valid values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mvarTimestamp = new AudioTimestamp(h, m, s, ms, mvarSampleRate * 2);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
    public class GoToDialog
    {
        private AudioTimestamp mvarTimestamp = AudioTimestamp.Empty;
        public AudioTimestamp Timestamp { get { return mvarTimestamp; } set { mvarTimestamp = value; } }

        public DialogResult ShowDialog()
        {
            GoToDialogBase dlg = new GoToDialogBase();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                mvarTimestamp = dlg.Timestamp;
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }
    }
}
