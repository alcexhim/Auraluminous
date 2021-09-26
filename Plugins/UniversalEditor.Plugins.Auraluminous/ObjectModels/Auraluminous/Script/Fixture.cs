using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class Fixture
    {
        public class FixtureCollection
            : System.Collections.ObjectModel.Collection<Fixture>
        {
			private Dictionary<Guid, Fixture> fixturesByID = new Dictionary<Guid, Fixture>();

			public Fixture this[Guid id]
			{
				get
				{
					if (fixturesByID.ContainsKey(id))
						return fixturesByID[id];
					return null;
				}
			}

			protected override void InsertItem(int index, Fixture item)
			{
				base.InsertItem(index, item);
				fixturesByID[item.ID] = item;
			}
			protected override void RemoveItem(int index)
			{
				if (fixturesByID.ContainsKey(this[index].ID))
				{
					fixturesByID.Remove(this[index].ID);
				}
				base.RemoveItem(index);
			}
			protected override void ClearItems()
			{
				base.ClearItems();
				fixturesByID.Clear();
			}
		}

        private Guid mvarID = Guid.Empty;
        public Guid ID { get { return mvarID; } set { mvarID = value; } }

        private UniversalEditor.ObjectModels.Lighting.Fixture.FixtureObjectModel mvarFixtureObject = null;
        public UniversalEditor.ObjectModels.Lighting.Fixture.FixtureObjectModel FixtureObject { get { return mvarFixtureObject; } set { mvarFixtureObject = value; } }

        private UniversalEditor.ObjectModels.Lighting.Fixture.Mode mvarMode = null;
        public UniversalEditor.ObjectModels.Lighting.Fixture.Mode Mode { get { return mvarMode; } set { mvarMode = value; } }

        private int mvarInitialAddress = 0;
        public int InitialAddress { get { return mvarInitialAddress; } set { mvarInitialAddress = value; } }

		public string Title { get; set; } = null;
	}
}
