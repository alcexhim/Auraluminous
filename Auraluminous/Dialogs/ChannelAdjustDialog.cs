using System;
using MBS.Framework.UserInterface;
using MBS.Framework.UserInterface.Controls;
using UniversalEditor.ObjectModels.Auraluminous.Script;

namespace Auraluminous.Dialogs
{
	// [ContainerLayout(typeof(ChannelAdjustDialog), "Auraluminous.Dialogs.ChannelAdjustDialog.glade")]
    public class ChannelAdjustDialog : CustomDialog
    {
		private ComboBox cboChannels;

        private Fixture mvarFixture = null;
        public Fixture Fixture { get { return mvarFixture; } }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            foreach (UniversalEditor.ObjectModels.Lighting.Fixture.Channel channel in mvarFixture.FixtureObject.Channels)
            {
				TreeModelRow row = new TreeModelRow(new TreeModelRowColumn[]
				{
					new TreeModelRowColumn((cboChannels.Model as DefaultTreeModel).Columns[0], channel.Name)
				});
				row.SetExtraData<UniversalEditor.ObjectModels.Lighting.Fixture.Channel>("channel", channel);
				(cboChannels.Model as DefaultTreeModel).Rows.Add(row);
            }
        }

        private void RefreshFixture()
        {
			UniversalEditor.ObjectModels.Lighting.Fixture.Channel channel = cboChannels.SelectedItem.GetExtraData<UniversalEditor.ObjectModels.Lighting.Fixture.Channel>("channel");
            // Program.Engine.OpenDMXInterface.SetChannelValue(mvarFixture.InitialAddress, channel.ChannelObject.RelativeAddress, (byte)txtValue.Value);
        }
    }
}
