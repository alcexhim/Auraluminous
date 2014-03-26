using System;
using System.Collections.Generic;
using System.Text;
using UniversalEditor.Accessors;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Lighting.Fixture;
using UniversalEditor.ObjectModels.Markup;

namespace UniversalEditor.DataFormats.Auraluminous.Script
{
    public class XMLScriptDataFormat : XMLDataFormat
    {
        protected override void BeforeLoadInternal(Stack<ObjectModel> objectModels)
        {
            base.BeforeLoadInternal(objectModels);
            objectModels.Push(new MarkupObjectModel());
        }

        private Dictionary<Guid, FixtureObjectModel> mvarFixtures = new Dictionary<Guid, FixtureObjectModel>();
        public Dictionary<Guid, FixtureObjectModel> Fixtures { get { return mvarFixtures; } }

        protected override void AfterLoadInternal(Stack<ObjectModel> objectModels)
        {
            base.AfterLoadInternal(objectModels);

            MarkupObjectModel mom = (objectModels.Pop() as MarkupObjectModel);
            ScriptObjectModel script = (objectModels.Pop() as ScriptObjectModel);

            string BasePath = String.Empty;
            FileAccessor fa = (script.Accessor as FileAccessor);
            if (fa != null)
            {
                BasePath = System.IO.Path.GetDirectoryName(fa.FileName);
            }

            MarkupTagElement tagAuraluminousScript = (mom.Elements["AuraluminousScript"] as MarkupTagElement);
            if (tagAuraluminousScript == null)
            {
                throw new InvalidDataFormatException("File does not contain a top-level \"AuraluminousScript\" tag");
            }

            MarkupTagElement tagAudio = (tagAuraluminousScript.Elements["Audio"] as MarkupTagElement);
            if (tagAudio != null)
            {
                MarkupAttribute attFileName = tagAudio.Attributes["FileName"];
                if (attFileName != null)
                {
                    script.AudioFileName = UniversalEditor.Common.Path.MakeAbsolutePath(attFileName.Value, BasePath);
                }
            }

            Dictionary<Guid, Fixture> fixturesByID = new Dictionary<Guid, Fixture>();
            #region Fixtures
            {
                MarkupTagElement tagFixtures = (tagAuraluminousScript.Elements["Fixtures"] as MarkupTagElement);
                if (tagFixtures != null)
                {
                    foreach (MarkupElement elFixture in tagFixtures.Elements)
                    {
                        MarkupTagElement tagFixture = (elFixture as MarkupTagElement);
                        if (tagFixture == null) continue;
                        if (tagFixture.FullName != "Fixture") continue;

                        MarkupAttribute attID = tagFixture.Attributes["ID"];
                        MarkupAttribute attTypeID = tagFixture.Attributes["TypeID"];
                        MarkupAttribute attModeID = tagFixture.Attributes["ModeID"];
                        if (attID == null || attTypeID == null || attModeID == null) continue;

                        Guid id = new Guid(attID.Value);
                        Guid typeID = new Guid(attTypeID.Value);
                        Guid modeID = new Guid(attModeID.Value);

                        Fixture fixture = new Fixture();

                        MarkupAttribute attInitialAddress = tagFixture.Attributes["InitialAddress"];
                        if (attInitialAddress != null)
                        {
                            fixture.InitialAddress = Int32.Parse(attInitialAddress.Value);
                        }

                        fixture.ID = id;
                        fixture.FixtureObject = mvarFixtures[typeID];
                        fixture.Mode = fixture.FixtureObject.Modes[modeID];
                        fixturesByID.Add(id, fixture);
                    }
                }
            }
            #endregion

            MarkupTagElement tagTasks = (tagAuraluminousScript.Elements["Tasks"] as MarkupTagElement);
            if (tagTasks != null)
            {
                foreach (MarkupElement elTask in tagTasks.Elements)
                {
                    MarkupTagElement tagTask = (elTask as MarkupTagElement);
                    if (tagTask == null) continue;
                    if (tagTask.FullName != "Task") continue;

                    Task task = new Task();
                    MarkupAttribute attID = tagTask.Attributes["ID"];
                    if (attID != null) task.ID = new Guid(attID.Value);

                    MarkupAttribute attTitle = tagTask.Attributes["Title"];
                    if (attTitle != null) task.Title = attTitle.Value;

                    MarkupTagElement tagFixtures = (tagTask.Elements["Fixtures"] as MarkupTagElement);
                    if (tagFixtures != null)
                    {
                        foreach (MarkupElement elFixture in tagFixtures.Elements)
                        {
                            MarkupTagElement tagFixture = (elFixture as MarkupTagElement);
                            if (tagFixture == null) continue;
                            if (tagFixture.FullName != "Fixture") continue;

                            MarkupAttribute attFixtureID = tagFixture.Attributes["ID"];
                            if (attFixtureID == null) continue;

                            Guid fixtureID = new Guid(attFixtureID.Value);

                            FrameFixture fixture = new FrameFixture();
                            fixture.Fixture = fixturesByID[fixtureID];

                            MarkupTagElement tagChannels = (tagFixture.Elements["Channels"] as MarkupTagElement);
                            if (tagChannels != null)
                            {
                                foreach (MarkupElement elChannel in tagChannels.Elements)
                                {
                                    MarkupTagElement tagChannel = (elChannel as MarkupTagElement);
                                    if (tagChannel == null) continue;
                                    if (tagChannel.FullName != "Channel") continue;

                                    MarkupAttribute attChannelID = tagChannel.Attributes["ID"];
                                    if (attChannelID == null) continue;
                                    MarkupAttribute attValue = tagChannel.Attributes["Value"];
                                    if (attValue == null) continue;

                                    Guid channelID = new Guid(attChannelID.Value);

                                    ObjectModels.Auraluminous.Script.Channel channel = new ObjectModels.Auraluminous.Script.Channel();
                                    channel.ChannelObject = fixture.Fixture.Mode.Channels[channelID];
                                    channel.Value = Byte.Parse(attValue.Value);
                                    fixture.Channels.Add(channel);
                                }
                            }

                            task.Fixtures.Add(fixture);
                        }
                    }

                    script.Tasks.Add(task);
                }
            }

            MarkupTagElement tagFrames = (tagAuraluminousScript.Elements["Frames"] as MarkupTagElement);
            if (tagFrames != null)
            {
                foreach (MarkupElement elFrame in tagFrames.Elements)
                {
                    MarkupTagElement tagFrame = (elFrame as MarkupTagElement);
                    if (tagFrame == null) continue;
                    if (tagFrame.FullName != "Frame") continue;

                    int d = 0, h = 0, m = 0, s = 0, ms = 0;
                    MarkupAttribute attDays = tagFrame.Attributes["Days"];
                    if (attDays != null) d = Int32.Parse(attDays.Value);
                    MarkupAttribute attHours = tagFrame.Attributes["Hours"];
                    if (attHours != null) h = Int32.Parse(attHours.Value);
                    MarkupAttribute attMinutes = tagFrame.Attributes["Minutes"];
                    if (attMinutes != null) m = Int32.Parse(attMinutes.Value);
                    MarkupAttribute attSeconds = tagFrame.Attributes["Seconds"];
                    if (attSeconds != null) s = Int32.Parse(attSeconds.Value);
                    MarkupAttribute attMilliseconds = tagFrame.Attributes["Milliseconds"];
                    if (attMilliseconds != null) ms = Int32.Parse(attMilliseconds.Value);

                    Frame frame = new Frame();
                    frame.TimeSpan = new TimeSpan(d, h, m, s, ms);

                    MarkupAttribute attTaskID = tagFrame.Attributes["TaskID"];
                    if (attTaskID != null)
                    {
                        Guid taskID = new Guid(attTaskID.Value);
                        ApplyTask(frame, script.Tasks[taskID]);
                    }
                    else
                    {
                        MarkupTagElement tagFixtures = (tagFrame.Elements["Fixtures"] as MarkupTagElement);
                        if (tagFixtures != null)
                        {
                            foreach (MarkupElement elFixture in tagFixtures.Elements)
                            {
                                MarkupTagElement tagFixture = (elFixture as MarkupTagElement);
                                if (tagFixture == null) continue;
                                if (tagFixture.FullName != "Fixture") continue;

                                MarkupAttribute attFixtureID = tagFixture.Attributes["ID"];
                                if (attFixtureID == null) continue;

                                Guid fixtureID = new Guid(attFixtureID.Value);

                                FrameFixture fixture = new FrameFixture();
                                fixture.Fixture = fixturesByID[fixtureID];

                                MarkupTagElement tagChannels = (tagFixture.Elements["Channels"] as MarkupTagElement);
                                if (tagChannels != null)
                                {
                                    foreach (MarkupElement elChannel in tagChannels.Elements)
                                    {
                                        MarkupTagElement tagChannel = (elChannel as MarkupTagElement);
                                        if (tagChannel == null) continue;
                                        if (tagChannel.FullName != "Channel") continue;

                                        MarkupAttribute attChannelID = tagChannel.Attributes["ID"];
                                        if (attChannelID == null) continue;
                                        MarkupAttribute attValue = tagChannel.Attributes["Value"];
                                        if (attValue == null) continue;

                                        Guid channelID = new Guid(attChannelID.Value);

                                        ObjectModels.Auraluminous.Script.Channel channel = new ObjectModels.Auraluminous.Script.Channel();
                                        channel.ChannelObject = fixture.Fixture.Mode.Channels[channelID];
                                        channel.Value = Byte.Parse(attValue.Value);
                                        fixture.Channels.Add(channel);
                                    }
                                }

                                frame.Fixtures.Add(fixture);
                            }
                        }
                    }
                    script.Frames.Add(frame);
                }
            }
        }

        private void ApplyTask(Frame frame, Task task)
        {
            foreach (FrameFixture fixture in task.Fixtures)
            {
                frame.Fixtures.Add(fixture);
            }
        }
        protected override void BeforeSaveInternal(Stack<ObjectModel> objectModels)
        {
            base.BeforeSaveInternal(objectModels);
        }
    }
}
