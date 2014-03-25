using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class Channel
    {
        public class ChannelCollection
            : System.Collections.ObjectModel.Collection<Channel>
        {
        }

        private UniversalEditor.ObjectModels.Lighting.Fixture.ModeChannel mvarChannelObject = null;
        public UniversalEditor.ObjectModels.Lighting.Fixture.ModeChannel ChannelObject { get { return mvarChannelObject; } set { mvarChannelObject = value; } }

        private byte mvarValue = 0;
        public byte Value { get { return mvarValue; } set { mvarValue = value; } }
    }
}
