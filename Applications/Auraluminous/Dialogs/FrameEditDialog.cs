using System;
using System.Linq;
using MBS.Framework.UserInterface;
using MBS.Framework.UserInterface.Controls;
using MBS.Framework.UserInterface.Controls.ListView;
using UniversalEditor.ObjectModels.Lighting.Fixture;

namespace Auraluminous.Dialogs
{
	[ContainerLayout(typeof(FrameEditDialog), "Auraluminous.Dialogs.FrameEditDialog.glade")]
	public class FrameEditDialog : CustomDialog
	{
		private Button cmdOK;
		private Toolbar tbFixture;
		private ListViewControl tvFixtures;
		private Toolbar tbFrame;
		private ListViewControl tvFrames;
		private TextBox txtTimestamp;

		public string Timestamp { get; set; } = null;

		protected override void OnCreated(EventArgs e)
		{
			base.OnCreated(e);
			(tbFixture.Items["tsbFixtureAdd"] as ToolbarItemButton).Click += tsbFixtureAdd_Click;

			DefaultButton = cmdOK;
		}

		private void tsbFixtureAdd_Click(object sender, EventArgs e)
		{
			FixtureBrowserDialog dlgFixtures = new FixtureBrowserDialog();
			if (dlgFixtures.ShowDialog() == DialogResult.OK)
			{
				foreach (FixtureObjectModel fixture in dlgFixtures.SelectedItems)
				{
					TreeModelRow lvi = new TreeModelRow(new TreeModelRowColumn[]
					{
						new TreeModelRowColumn(tvFixtures.Model.Columns[0], fixture.Model),
						new TreeModelRowColumn(tvFixtures.Model.Columns[1], fixture.Manufacturer)
					});
					lvi.SetExtraData<FixtureObjectModel>("fixture", fixture);
					tvFixtures.Model.Rows.Add(lvi);
				}
			}
		}

		private void cmdFramesAdd_Click(object sender, EventArgs e)
		{

		}

		private void lvFixtures_SelectedIndexChanged(object sender, EventArgs e)
		{
			tvFrames.Model.Rows.Clear();
			if (tvFixtures.SelectedRows.Count == 1)
			{
				(tbFrame.Items["tsbFrameAdd"] as ToolbarItemButton).Enabled = true;
				tvFrames.Enabled = true;
				(tbFrame.Items["tsbFrameEdit"] as ToolbarItemButton).Enabled = (tvFrames.SelectedRows.Count > 0);
				(tbFrame.Items["tsbFrameRemove"] as ToolbarItemButton).Enabled = (tvFrames.SelectedRows.Count > 0);
				// (tbFrame.Items["tsbFrameClear"] as ToolbarItemButton).Enabled = (tvFrames.Model.Rows.Count > 0);
			}
			else
			{

			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (Timestamp != null)
			{
				txtTimestamp.Text = Timestamp;
			}
		}
	}
}
