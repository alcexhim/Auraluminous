using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UniversalEditor.ObjectModels.Auraluminous.Script;

namespace Auraluminous
{
    public partial class ChannelEditDialog : Form
    {
        public ChannelEditDialog()
        {
            InitializeComponent();
        }

        private Fixture mvarFixture = null;
        public Fixture Fixture { get { return mvarFixture; } }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            foreach (UniversalEditor.ObjectModels.Lighting.Fixture.Channel channel in mvarFixture.FixtureObject.Channels)
            {
                cboChannels.Items.Add(channel);
            }
        }

        private bool inhibitUpdateTextBoxValue = false;
        private bool inhibitUpdateSliderValue = false;
        private void txtValue_ValueChanged(object sender, EventArgs e)
        {
            if (inhibitUpdateTextBoxValue) return;
            inhibitUpdateSliderValue = true;
            sldValue.Value = (int)txtValue.Value;
            RefreshFixture();
            inhibitUpdateSliderValue = false;
        }

        private void sldValue_Scroll(object sender, EventArgs e)
        {
            if (inhibitUpdateSliderValue) return;
            inhibitUpdateTextBoxValue = true;
            txtValue.Value = (decimal)sldValue.Value;
            RefreshFixture();
            inhibitUpdateTextBoxValue = false;
        }

        private void RefreshFixture()
        {
            Channel channel = (cboChannels.SelectedItem as Channel);
            Program.Engine.OpenDMXInterface.SetChannelValue(mvarFixture.InitialAddress, channel.ChannelObject.RelativeAddress, (byte)txtValue.Value);
        }
    }
}
