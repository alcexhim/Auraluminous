using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Lighting.Fixture;

namespace Auraluminous
{
    public partial class MainWindow : Form
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Program.Engine.Stop();
        }

        private string mvarFileName = "Projects/Please Don't Go/Please Don't Go.alu";

        private void mnuPlaybackPlay_Click(object sender, EventArgs e)
        {
            PlayPause();
        }

        public void PlayPause()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(PlayPause));
                return;
            }

            if (mnuPlaybackPlay.Text == "&Play")
            {
                if (Program.Engine.AudioPlayer.State == Surodoine.AudioPlayerState.Paused)
                {
                    Program.Engine.Pause();
                    mnuPlaybackPlay.Text = "&Pause";
                    mnuPlaybackPlay.Enabled = true;
                }
                else if (Program.Engine.AudioPlayer.State == Surodoine.AudioPlayerState.Stopped)
                {
                    // LoadFile(mvarFileName);
                    Program.Engine.Play();

                    mnuPlaybackPlay.Text = "&Pause";
                    mnuPlaybackStop.Enabled = true;
                    mnuPlaybackPlay.Enabled = true;
                }
                else
                {
                    Program.Engine.Stop();

                    // LoadFile(mvarFileName);

                    // Program.Engine.Play();

                    mnuPlaybackPlay.Text = "&Play";
                    mnuPlaybackStop.Enabled = false;
                    mnuPlaybackPlay.Enabled = true;
                }
            }
            else if (mnuPlaybackPlay.Text == "&Pause")
            {
                Program.Engine.Pause();
                mnuPlaybackPlay.Text = "&Play";
            }
        }
        public void ReloadFile()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ReloadFile));
                return;
            }

            mnuPlaybackStop_Click(null, EventArgs.Empty);

            txtSongTitle.Text = "Compiling...";
            Application.DoEvents();

            LoadFile(mvarFileName);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                mnuPlaybackPlay_Click(this, e);
            }
            else if (e.KeyCode == Keys.S)
            {
                mnuPlaybackStop_Click(this, e);
            }
        }

        private void LoadFile(string mvarFileName)
        {
            Program.Engine.Load(mvarFileName);
            
            txtSongTitle.Text = Program.Engine.Script.Title;
            if (!String.IsNullOrEmpty(Program.Engine.Script.Artist))
            {
                txtSongTitle.Text = Program.Engine.Script.Artist + " - " + txtSongTitle.Text;
            }
            txtTime.Text = "00:00:00:00";
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mnuPlaybackPlay_Click(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtTime.Text = Program.Engine.AudioPlayer.Timestamp.ToString();
        }

        private void mnuEditGoTo_Click(object sender, EventArgs e)
        {
            GoToDialog dlg = new GoToDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Program.Engine.Pause();

                System.Threading.Thread.Sleep(50);
                Program.Engine.AudioPlayer.Timestamp = dlg.Timestamp;
                Program.Engine.Script.Frames.Reset();
                System.Threading.Thread.Sleep(50);

                Program.Engine.Pause();
            }
        }

        private void mnuPlaybackStop_Click(object sender, EventArgs e)
        {
            Program.Engine.Stop();
            mnuPlaybackPlay.Text = "&Play";
            mnuPlaybackStop.Enabled = false;
        }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Auraluminous script|*.alu";
            ofd.InitialDirectory = "Projects";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mvarFileName = ofd.FileName;

                LoadFile(mvarFileName);
            }
        }

        private void mnuToolsChannelAdjust_Click(object sender, EventArgs e)
        {
            FrameEditDialog dlg = new FrameEditDialog();
            dlg.txtTimestamp.Text = txtTime.Text;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // add the line into the script file at the current time
                
            }
        }

        private void mnuFileReload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reloading the file from disk will lose any changes you have made in Auraluminous Editor. Continue?", "Reload from Disk", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No) return;
            ReloadFile();
        }
    }
}
