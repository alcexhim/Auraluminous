using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class FrameFixture
    {
        public class FrameFixtureCollection
            : System.Collections.ObjectModel.Collection<FrameFixture>
        {
        }

        private Fixture mvarFixture = null;
        public Fixture Fixture { get { return mvarFixture; } set { mvarFixture = value; } }

        private Channel.ChannelCollection mvarChannels = new Channel.ChannelCollection();
        public Channel.ChannelCollection Channels { get { return mvarChannels; } }
    }
}
