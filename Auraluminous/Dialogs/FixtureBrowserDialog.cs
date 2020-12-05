using System;
using System.Collections.Generic;
using MBS.Framework.UserInterface;
using MBS.Framework.UserInterface.Controls;
using MBS.Framework.UserInterface.Controls.ListView;
using UniversalEditor.ObjectModels.Lighting.Fixture;

namespace Auraluminous
{
	[ContainerLayout(typeof(FixtureBrowserDialog), "Auraluminous.Dialogs.FixtureBrowserDialog.glade")]
	public class FixtureBrowserDialog : CustomDialog
	{
		private Button cmdOK;
		private ListViewControl tvFixtures;

		public List<FixtureObjectModel> SelectedItems { get; } = new List<FixtureObjectModel>();

		protected override void OnCreated(EventArgs e)
		{
			base.OnCreated(e);
			DefaultButton = cmdOK;

			foreach (FixtureObjectModel fixture in Program.Engine.Fixtures)
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

		[EventHandler(nameof(cmdOK), "Click")]
		private void cmdOK_Click(object sender, EventArgs e)
		{
			SelectedItems.Clear();
			foreach (TreeModelRow lvi in tvFixtures.SelectedRows)
			{
				SelectedItems.Add(lvi.GetExtraData<FixtureObjectModel>("fixture"));
			}

			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
