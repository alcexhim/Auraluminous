using System;
using System.Collections.Generic;
using System.Text;
using MBS.Audio;
using MBS.Framework;
using MBS.Framework.Collections.Generic;
using MBS.Framework.Drawing;
using UniversalEditor.Accessors;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor.ObjectModels.Auraluminous.Script;
using UniversalEditor.ObjectModels.Auraluminous.Script.Commands;
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

		public FixtureObjectModel.FixtureObjectModelCollection FixtureDefinitions { get; } = new FixtureObjectModel.FixtureObjectModelCollection();

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

			MarkupTagElement tagInformation = (tagAuraluminousScript.Elements["Information"] as MarkupTagElement);
			if (tagInformation != null)
			{
				MarkupTagElement tagTitle = (tagInformation.Elements["Title"] as MarkupTagElement);
				if (tagTitle != null)
				{
					script.Title = tagTitle.Value;
				}
				MarkupTagElement tagArtist = (tagInformation.Elements["Artist"] as MarkupTagElement);
				if (tagArtist != null)
				{
					script.Artist = tagArtist.Value;
				}

				MarkupTagElement tagTempo = (tagInformation.Elements["Tempo"] as MarkupTagElement);
				if (tagTempo != null)
				{
					if (Single.TryParse(tagTempo.Attributes["BeatsPerBar"]?.Value, out float beatsPerBar))
					{
						script.BeatsPerBar = beatsPerBar;
					}
				}
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

			MarkupTagElement tagArdour = (tagAuraluminousScript.Elements["Ardour"] as MarkupTagElement);
			if (tagArdour != null)
			{
				MarkupAttribute attFileName = tagArdour.Attributes["FileName"];
				if (attFileName != null)
				{
					script.ArdourFileName = UniversalEditor.Common.Path.MakeAbsolutePath(attFileName.Value, BasePath);
				}
			}

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

						MarkupAttribute attTitle = tagFixture.Attributes["Title"];
						if (attTitle != null)
						{
							fixture.Title = attTitle.Value;
						}

						MarkupAttribute attInitialAddress = tagFixture.Attributes["InitialAddress"];
                        if (attInitialAddress != null)
                        {
                            fixture.InitialAddress = Int32.Parse(attInitialAddress.Value);
                        }

                        fixture.ID = id;
                        fixture.FixtureObject = FixtureDefinitions[typeID];
                        fixture.Mode = fixture.FixtureObject.Modes[modeID];
						script.Fixtures.Add(fixture);
                    }
                }
            }
			#endregion
			#region Tasks
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
							FrameFixture fixture = LoadFrameFixture(script, elFixture as MarkupTagElement);
							if (fixture == null) continue;

                            task.Fixtures.Add(fixture);
                        }
                    }

                    script.Tasks.Add(task);
                }
            }
			#endregion
			#region Sequences
			MarkupTagElement tagSequences = tagAuraluminousScript.Elements["Sequences"] as MarkupTagElement;
			if (tagSequences != null)
			{
				foreach (MarkupTagElement tagSequence in tagSequences.Elements.OfType<MarkupTagElement>())
				{
					Sequence seq = new Sequence(new Guid(tagSequence.Attributes["ID"]?.Value), tagSequence.Attributes["Title"]?.Value);
					MarkupTagElement tagParameters = tagSequence.Elements["Parameters"] as MarkupTagElement;
					if (tagParameters != null)
					{
						foreach (MarkupTagElement tagParameter in tagParameters.Elements.OfType<MarkupTagElement>())
						{
							if (tagParameter.Name != "Parameter") continue;

							Type dataType = Reflection.FindType(tagParameter.Attributes["DataType"]?.Value);
							if (dataType == null) continue;

							SequenceParameter parm = new SequenceParameter(new Guid(tagParameter.Attributes["ID"]?.Value), tagParameter.Attributes["Title"]?.Value, dataType, tagParameter.Attributes["DefaultValue"]?.Value?.Parse(dataType));
							seq.Parameters.Add(parm);
						}
					}
					MarkupTagElement tagSequenceFrames = tagSequence.Elements["Frames"] as MarkupTagElement;
					if (tagSequenceFrames != null)
					{
						foreach (MarkupTagElement tagFrame in tagSequenceFrames.Elements.OfType<MarkupTagElement>())
						{
							if (tagFrame.Name != "Frame") continue;

							Frame frame = LoadFrame(script, tagFrame);
							frame.Sequence = seq;
							seq.Frames.Add(frame);
						}
					}
					script.Sequences.Add(seq);
				}
			}
			#endregion
			#region Frames
			MarkupTagElement tagFrames = (tagAuraluminousScript.Elements["Frames"] as MarkupTagElement);
			if (tagFrames != null)
			{
				foreach (MarkupElement elFrame in tagFrames.Elements)
				{
					MarkupTagElement tagFrame = (elFrame as MarkupTagElement);
					if (tagFrame == null) continue;
					if (tagFrame.FullName == "Frame")
					{
						Frame frame = LoadFrame(script, tagFrame);
						script.Frames.Add(frame);
					}
					else if (tagFrame.FullName == "SequenceRef")
					{
						// HACK: new 2020 SequenceRef command - we should probably figure out a better place for this
						Guid sequenceID = new Guid(tagFrame.Attributes["SequenceID"]?.Value);

						Sequence seq = script.Sequences[sequenceID];
						if (seq == null) continue;

						MarkupTagElement tagStartTime = tagFrame.Elements["StartTime"] as MarkupTagElement;
						MarkupTagElement tagRepeat = tagFrame.Elements["Repeat"] as MarkupTagElement;
						MarkupTagElement tagParameterValues = tagFrame.Elements["ParameterValues"] as MarkupTagElement;

						SequenceReference sr = new SequenceReference(seq, XmlToBBT(tagStartTime, script), GetParameterValues(seq, tagParameterValues), (tagRepeat?.Attributes["Times"]?.Value?.Parse<int>()).GetValueOrDefault(1));
						script.Actions.Add(sr);

						// right now we just unroll it directly into Frames, but eventually we'll leave that to the pre-compile stage
						BarBeatTick start = sr.StartTime;
						for (int q = 0; q < sr.RepeatCount; q++)
						{
							for (int i = 0; i < seq.Frames.Count; i++)
							{
								Frame frame = seq.Frames[i].Clone() as Frame;
								frame.SequenceReference = sr;

								start += seq.Frames[i].BarBeatTick;
								frame.BarBeatTick = start;

								script.Frames.Add(frame);
							}
						}
					}
				}
			}
			#endregion
		}

		private Frame LoadFrame(ScriptObjectModel script, MarkupTagElement tagFrame)
		{
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

			MarkupAttribute attBars = tagFrame.Attributes["Bars"];
			MarkupAttribute attBeats = tagFrame.Attributes["Beats"];
			MarkupAttribute attTicks = tagFrame.Attributes["Ticks"];

			Frame frame = new Frame();

			if (attBars != null || attBeats != null || attTicks != null)
			{
				int bars = (attBars?.Value.TryParse<int>(0)).GetValueOrDefault(0);
				int beats = (attBeats?.Value.TryParse<int>(0)).GetValueOrDefault(0);
				int ticks = (attTicks?.Value.TryParse<int>(0)).GetValueOrDefault(0);

				BarBeatTick bbt = BarBeatTick.FromBBT(bars, beats, ticks, script.BeatsPerBar, script.TicksPerBeat);
				frame.BarBeatTick = bbt;
			}

			int samplesPerSecond = 48000;
			frame.Timestamp = AudioTimestamp.FromHMS(d, h, m, s, ms, samplesPerSecond);

			MarkupAttribute attTaskID = tagFrame.Attributes["TaskID"];
			if (attTaskID != null)
			{
				Guid taskID = new Guid(attTaskID.Value);
				ApplyTask(frame, script.Tasks[taskID]);
			}
			else
			{
				MarkupTagElement tagFrameTasks = (tagFrame.Elements["Tasks"] as MarkupTagElement);
				if (tagFrameTasks != null)
				{
					foreach (MarkupElement elTask in tagFrameTasks.Elements)
					{
						MarkupTagElement tagTask = (elTask as MarkupTagElement);
						if (tagTask == null) continue;
						if (tagTask.FullName != "Task") continue;

						MarkupAttribute attTaskTaskID = tagTask.Attributes["TaskID"];
						Guid taskID = new Guid(attTaskTaskID.Value);
						ApplyTask(frame, script.Tasks[taskID]);
					}
				}

				MarkupTagElement tagFixtures = (tagFrame.Elements["Fixtures"] as MarkupTagElement);
				if (tagFixtures != null)
				{
					foreach (MarkupElement elFixture in tagFixtures.Elements)
					{
						FrameFixture fixture = LoadFrameFixture(script, elFixture as MarkupTagElement);
						if (fixture == null) continue;

						frame.Fixtures.Add(fixture);
					}
				}
			}
			return frame;
		}

		private BarBeatTick XmlToBBT(MarkupTagElement tag, ScriptObjectModel script)
		{
			float? beatsPerBar = null, ticksPerBeat = null;
			if (script.BeatsPerBar != null)
			{
				beatsPerBar = script.BeatsPerBar;
			}

			int bars = (tag.Attributes["Bars"]?.Value?.Parse<int>()).GetValueOrDefault(0);
			int beats = (tag.Attributes["Beats"]?.Value?.Parse<int>()).GetValueOrDefault(0);
			int ticks = (tag.Attributes["Ticks"]?.Value?.Parse<int>()).GetValueOrDefault(0);
			return BarBeatTick.FromBBT(bars, beats, ticks, beatsPerBar, ticksPerBeat);
		}

		private SequenceParameterValue[] GetParameterValues(Sequence seq, MarkupTagElement tag)
		{
			List<SequenceParameterValue> list = new List<SequenceParameterValue>();
			if (tag != null)
			{
				foreach (MarkupTagElement tag1 in tag.Elements.OfType<MarkupTagElement>())
				{
					if (tag1.Name == "ParameterValue")
					{
						Guid parameterID = new Guid(tag1.Attributes["ParameterID"]?.Value);
						string parameterValue = tag1.Attributes["Value"]?.Value;

						SequenceParameter parm = seq.Parameters[parameterID];
						if (parm == null) continue;

						SequenceParameterValue value = new SequenceParameterValue(parm, parameterValue.Parse(parm.DataType));
						list.Add(value);
					}
				}
			}
			return list.ToArray();
		}

		private FrameFixture LoadFrameFixture(ScriptObjectModel script, MarkupTagElement tagFixture)
		{
			if (tagFixture == null) return null;
			if (tagFixture.FullName != "Fixture") return null;

			MarkupAttribute attFixtureID = tagFixture.Attributes["ID"];
			if (attFixtureID == null) return null;

			Guid fixtureID = new Guid(attFixtureID.Value);

			FrameFixture fixture = new FrameFixture();
			fixture.Fixture = script.Fixtures[fixtureID];

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
					fixture.Commands.Add(new ChannelCommand(fixture.Fixture.Mode.Channels[channelID], Byte.Parse(attValue.Value)));
				}
			}

			MarkupTagElement tagCommands = (tagFixture.Elements["Commands"] as MarkupTagElement);
			if (tagCommands != null)
			{
				foreach (MarkupElement elCommand in tagCommands.Elements)
				{
					MarkupTagElement tagCommand = (elCommand as MarkupTagElement);
					if (tagCommand == null) continue;
					switch (tagCommand.FullName)
					{
						case "PixelSet":
						{
							// FIXME: this compiles directly into a FrameFixture and also assumes that R,G,B channels are declared immediately adjacent to each other
							// this really should look at the fixture definition to see if a PixelSet command is even POSSIBLE (i.e. Intimidator Spots cannot do pixels)
							MarkupAttribute attIndex = tagCommand.Attributes["Index"];
							MarkupAttribute attValue = tagCommand.Attributes["Value"];

							object index = null, value = null;
							if (attIndex.Value == "all")
							{
								index = "all";
							}
							else
							{
								index = (Int32.Parse(attIndex.Value) * 3);
							}

							MarkupAttribute attParameterID = tagCommand.Attributes["ParameterID"];
							if (attParameterID != null && value == null)
							{
								value = new Guid(attParameterID.Value);
							}
							else
							{
								value = Color.Parse(attValue.Value);
							}

							/*
							byte r = (byte)(color.R * 255);
							byte g = (byte)(color.G * 255);
							byte b = (byte)(color.B * 255);

							ObjectModels.Auraluminous.Script.Channel chanR = new ObjectModels.Auraluminous.Script.Channel();
							chanR.ChannelObject = fixture.Fixture.Mode.Channels[index];
							chanR.Value = r;
							fixture.Channels.Add(chanR);

							ObjectModels.Auraluminous.Script.Channel chanG = new ObjectModels.Auraluminous.Script.Channel();
							chanG.ChannelObject = fixture.Fixture.Mode.Channels[index + 1];
							chanG.Value = g;
							fixture.Channels.Add(chanG);
								
							ObjectModels.Auraluminous.Script.Channel chanB = new ObjectModels.Auraluminous.Script.Channel();
							chanB.ChannelObject = fixture.Fixture.Mode.Channels[index + 2];
							chanB.Value = b;
							fixture.Channels.Add(chanB);
							*/
							fixture.Commands.Add(new PixelSetCommand(index, value));						
							break;
						}
						case "ChannelSet":
						{
							// FIXME: this compiles directly into a FrameFixture and also assumes that R,G,B channels are declared immediately adjacent to each other
							// this really should look at the fixture definition to see if a PixelSet command is even POSSIBLE (i.e. Intimidator Spots cannot do pixels)
							MarkupAttribute attChannelID = tagCommand.Attributes["ChannelID"];
							MarkupAttribute attValue = tagCommand.Attributes["Value"];

							object channelID = null, value = null;
							if (attChannelID.Value == "all")
							{
								channelID = "all";
							}
							else
							{
								channelID = new Guid(attChannelID.Value);
							}

							MarkupAttribute attParameterID = tagCommand.Attributes["ParameterID"];
							if (attParameterID != null && value == null)
							{
								value = new Guid(attParameterID.Value);
							}
							else
							{
								value = Byte.Parse(attValue.Value);
							}
							fixture.Commands.Add(new ChannelCommand(channelID, value));						
							break;
						}
					}
				}
			}
			return fixture;
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
