using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalEditor.ObjectModels.Auraluminous.Script
{
    public class Task
    {
        public class TaskCollection
            : System.Collections.ObjectModel.Collection<Task>
        {
            public Task this[Guid id]
            {
                get
                {
                    foreach (Task task in this)
                    {
                        if (task.ID == id) return task;
                    }
                    return null;
                }
            }
        }

        private Guid mvarID = Guid.Empty;
        public Guid ID { get { return mvarID; } set { mvarID = value; } }

        private string mvarTitle = String.Empty;
        public string Title { get { return mvarTitle; } set { mvarTitle = value; } }

        private FrameFixture.FrameFixtureCollection mvarFixtures = new FrameFixture.FrameFixtureCollection();
        public FrameFixture.FrameFixtureCollection Fixtures { get { return mvarFixtures; } }

        public override string ToString()
        {
            return mvarTitle;
        }
    }
}
