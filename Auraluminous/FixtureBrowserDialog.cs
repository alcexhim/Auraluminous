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
    public partial class FixtureBrowserDialog : Form
    {
        public FixtureBrowserDialog()
        {
            InitializeComponent();
        }

        private List<FixtureObjectModel> mvarSelectedItems = new List<FixtureObjectModel>();
        public List<FixtureObjectModel> SelectedItems { get { return mvarSelectedItems; } }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            foreach (FixtureObjectModel fixture in Program.Engine.Fixtures)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = fixture.Model;
                lvi.SubItems.Add(fixture.Manufacturer);
                lvi.Tag = fixture;
                lv.Items.Add(lvi);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            mvarSelectedItems.Clear();
            foreach (ListViewItem lvi in lv.SelectedItems)
            {
                mvarSelectedItems.Add(lvi.Tag as FixtureObjectModel);
            }
        }
    }
}
