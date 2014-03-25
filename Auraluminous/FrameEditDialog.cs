using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalEditor.ObjectModels.Lighting.Fixture;

namespace Auraluminous
{
    public partial class FrameEditDialog : Form
    {
        public FrameEditDialog()
        {
            InitializeComponent();
        }

        private void cmdFixturesAdd_Click(object sender, EventArgs e)
        {
            FixtureBrowserDialog dlgFixtures = new FixtureBrowserDialog();
            if (dlgFixtures.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (FixtureObjectModel fixture in dlgFixtures.SelectedItems)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = fixture;
                    lvi.Text = fixture.Manufacturer + " " + fixture.Model;
                    lvFixtures.Items.Add(lvi);
                }
            }
        }

        private void cmdFramesAdd_Click(object sender, EventArgs e)
        {
            
        }

        private void lvFixtures_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvFrames.Items.Clear();
            if (lvFixtures.SelectedItems.Count == 1)
            {
                cmdFramesAdd.Enabled = true;
                lvFrames.Enabled = true;
                cmdFramesModify.Enabled = (lvFrames.SelectedItems.Count > 0);
                cmdFramesRemove.Enabled = (lvFrames.SelectedItems.Count > 0);
                cmdFramesClear.Enabled = (lvFrames.Items.Count > 0);
            }
            else
            {

            }
        }
    }
}
